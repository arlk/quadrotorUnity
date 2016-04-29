import socket
import struct
import math
import numpy as np
from time import sleep
import datetime
import csv
import ovr
import sys

__author__ = "Arun Lakshmanan"

name = raw_input("Subject ID :")
name = name.replace(" ", "")
dateToday = str(datetime.date.today())

UDP_IP = "127.0.0.1"
UDP_SEND_SIMLK_PORT = 25002
UDP_SEND_UNITY_PORT = 25003
Ts = 0.001

### Reading collison-free trajectories
trajectories = np.genfromtxt('../Simulink/Trajectories.csv', delimiter=',')

############## Communication stuff ##############
sendSockUnity = socket.socket(socket.AF_INET, # Internet
                             socket.SOCK_DGRAM) # UDP


sendSock = socket.socket(socket.AF_INET, # Internet
                             socket.SOCK_DGRAM) # UDP
##################################################

###Randomization
numExp = 8

####TimeIntervals
timeInt =10 ## 1 second pause

trial = np.random.random_integers(0,2)
trial = 0
trajNum = trial*8 + np.arange(numExp) + 1
trajNum = np.repeat(trajNum, 3)
np.random.shuffle(trajNum)
trajNum[:] = 2

###Center of Room
center = np.array([0,0,0])

###Height
height = 2.33 ##BENTIC changed the height from 2 to 2.8, to avoid hitting the game 

###Initialize OVR
ovr.initialize(None)
hmd, luid = ovr.create()
hmdDesc = ovr.getHmdDesc(hmd)

trajIndex = -1
pathIndex = 0
scenario = 0
numData = 2 ##number of columns in Trajectories.csv

Tk = 0
GlobalT = 0
currLoc = 0
nextLoc = 0
prevPath = -1
reached = 1
firsttime = 1

angularAcc = ovr.Vector3f(0,0,0)
linearAcc = ovr.Vector3f(0,0,0)
timeSec = 0

def wait_for_ack():
   ddata = ""
   ack = struct.pack('B', 0xff)
   while ddata != ack:
      ddata = ser.read(1)
   return

##########GSR Code below
if len(sys.argv) > 2:
   print "No device specified."
   print "Specify the serial port of the device you wish to connect to."
   print "Example: exp.py Com12"
else:
   ser = serial.Serial(sys.argv[1], 9600)
   ser.flushInput()
# send the set sensors command
   ser.write(struct.pack('BBBB', 0x08, 0xC4, 0x00, 0x00))  #analog accel, gsr, MPU9150 gyro
   wait_for_ack()
# send the set sampling rate command
   ser.write(struct.pack('BBB', 0x05, 0x80, 0x02)) #51.2Hz (32768/640=51.2Hz: 640 -> 0x0280; has to be done like this for alignment reasons.)
   wait_for_ack()
# send start streaming command
   ser.write(struct.pack('B', 0x07))
   wait_for_ack()

# read incoming data
   ddata = ""
   numbytes = 0
   framesize = 17 # 1byte packet type + 2byte timestamp + 3x2byte Analog Accel + 2byte GSR + 3x2byte MPU9150 gyro

while True:
    
    if trajIndex<numExp:

        ts  = ovr.getTrackingState(hmd, ovr.getTimeInSeconds(), True)
        if ts.StatusFlags & (ovr.Status_OrientationTracked | ovr.Status_PositionTracked):
            ###Estimated Data
            angularAcc = ts.HeadPose.AngularAcceleration
            linearAcc = ts.HeadPose.LinearAcceleration
            timeSec = ts.HeadPose.TimeInSeconds
            sys.stdout.flush()
        
        if reached == 1:
            reached = 0
            trajIndex += 1 
            stasis = np.random.random_integers(timeInt,timeInt*2)
            
            if firsttime:
                stasis = np.random.random_integers(timeInt*2,timeInt*3)
                firsttime = 0

        if stasis >= 0:
            stasis -= 0.01
            scenario = 0
            posn = np.array([72,46])
            zposn = 2.33            

        else:
            scenario = trajNum[trajIndex]
            pathIndex = scenario%4
            if pathIndex == 0:
                pathIndex = 4
            #### Setting trajectories
            if pathIndex != prevPath:
                if pathIndex%2 != 0:
                    traj = trajectories[:,((pathIndex+1)/2-1)*numData:(pathIndex+1)/2*numData]
                else:
                    posnTraj = np.flipud(trajectories[:,(pathIndex/2-1)*numData:(pathIndex/2)*numData-1])
                    timeseries = trajectories[:,(pathIndex/2)*numData-1]
                    traj = np.append(posnTraj,timeseries.reshape(-1,1),axis=1)

                Tk = 0
                reached = 0
                prevPath = pathIndex
            
            #### Generating trajectory command
            index = Tk/Ts
            if index < trajectories.shape[0]:
                ##print scenario, pathIndex, prevPath 
                print "scenario", trajNum[trajIndex], "Path Index", pathIndex
                posn = traj[Tk/Ts,:]
                zposn = height
            else:
                print scenario, pathIndex, prevPath 
                posn = traj[-1]
                zposn = height
                reached = 1
        
    else:
        break

    print("Posn: ",posn[:2], zposn) 
    ##print scenario
    ##print trajNum
	
	##posnFinal=np.array([-17,48,zposn])

    POSN = np.array([posn[0],posn[1],zposn])

    msg = struct.pack('<ddd', posn[0],posn[1],zposn)
    sendSock.sendto(msg, (UDP_IP, UDP_SEND_SIMLK_PORT))

    if scenario <= 8:
        if scenario <=4 :
            scenarioMsg = scenario
        else:
            scenarioMsg = scenario - 4
    elif scenario <= 16:
        if scenario <=12 :
            scenarioMsg = scenario - 4
        else:
            scenarioMsg = scenario - 4 - 4
    elif scenario <= 24:
        if scenario <=20 :
            scenarioMsg = scenario - 4 - 4
        else:
            scenarioMsg = scenario - 4 - 4 - 4

    ##print scenario, scenarioMsg
    scenarioMsg = struct.pack('<d', scenarioMsg)
    sendSock.sendto(scenarioMsg, (UDP_IP, UDP_SEND_UNITY_PORT))


    distance = np.linalg.norm(POSN - center)

    with open(r'Logs\ '+name+'-'+dateToday+'.csv','a')as csvfile:
        dataLog = csv.writer(csvfile, delimiter=',')
        dataLog.writerow([scenario, distance, angularAcc.x, angularAcc.y, angularAcc.z, linearAcc.x, linearAcc.y, linearAcc.z, timeSec])


    if scenario%8 > 4:
        Tk += 2*Ts
    elif scenario == 0:
        Tk += 2*Ts
    else: 
        Tk += 10*Ts
    
    GlobalT += Ts

    sleep(0.01)

ovr.destroy(hmd)
ovr.shutdown()