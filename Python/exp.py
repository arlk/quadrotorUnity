import socket
import struct
import math
import numpy as np
import time as unix
import datetime
import csv
import ovr
import sys
import serial

initalWaitTime = 20.0
droneFlybyTime = 10.0
postWaitTime = 10.0
iterations = 3
MatlabScenario = 0

name = raw_input("Subject ID :")
name = name.replace(" ", "")
myScenario = 1
myCom = 'Com7'

def wait_for_ack():
   ddata = ""
   ack = struct.pack('B', 0xff)
   while ddata != ack:
      ddata = ser.read(1)
   return
   
### Shimmer stuff #############################
ser = serial.Serial(myCom, 9600)
ser.flushInput()
# send the set sensors command
ser.write(struct.pack('BBBB', 0x08, 0x04, 0x02, 0x00))  #analog accel, gsr, MPU9150 gyro
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
framesize = 7 # 1byte packet type + 2byte timestamp + 2byte PPG + 2byte GSR 
################################################

############## Communication stuff ##############
UDP_IP = "127.0.0.1"
UDP_SEND_SIMLK_PORT = 25002
UDP_SEND_UNITY_PORT = 25003
UDP_RECV_UNITY_PORT = 25005
recvSock = socket.socket(socket.AF_INET, # Internet
                             socket.SOCK_DGRAM) # UDP
recvSock.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
recvSock.setblocking(False)
recvSock.bind((UDP_IP, UDP_RECV_UNITY_PORT))

sendSock = socket.socket(socket.AF_INET, # Internet
                             socket.SOCK_DGRAM) # UDP
##################################################

###Initialize OVR#########################
ovr.initialize(None)
hmd, luid = ovr.create()
hmdDesc = ovr.getHmdDesc(hmd)
angularAcc = ovr.Vector3f(0,0,0)
linearAcc = ovr.Vector3f(0,0,0)
timeSec = 0
##########################################

dateToday = str(datetime.date.today())
Ts = 0.01
reset = False
start = False
latched = True
elapsed = 0.0
experimentStart = 0.0
previousTime = unix.time()
scenario = 0

try:
    while True:
        # Oculus IMU Stuff ############
        ts  = ovr.getTrackingState(hmd, ovr.getTimeInSeconds(), True)
        if ts.StatusFlags & (ovr.Status_OrientationTracked | ovr.Status_PositionTracked):
            angularAcc = ts.HeadPose.AngularAcceleration
            linearAcc = ts.HeadPose.LinearAcceleration
            timeSec = ts.HeadPose.TimeInSeconds
            sys.stdout.flush()
        ################################
            
        # GSR Stuff ####################
        while numbytes < framesize:
            ddata += ser.read(framesize)
            numbytes = len(ddata)
         
        data = ddata[0:framesize]
        ddata = ddata[framesize:]
        numbytes = len(ddata)

        (packettype, ) = struct.unpack('B', data[0:1])
        (timestamp, ) = struct.unpack('H', data[1:3])
        (ppg,) = struct.unpack('h', data[3:5])
        (gsr,) = struct.unpack('H', data[5:7])
        gsrrange = (gsr & 0xC000) >> 14
        gsr &= 0xFFF
        ##################################

        # Sending Stuff
        msg = struct.pack('<??', start, reset)
        sendSock.sendto(msg, (UDP_IP, UDP_SEND_SIMLK_PORT))
        
        msg = struct.pack('<d', scenario)
        sendSock.sendto(msg, (UDP_IP, UDP_SEND_UNITY_PORT))
        
        # Receiving Stuff
        try:
            data, addr = recvSock.recvfrom(1024) # Test buffer sizes. (TO DO)
            vx,vy,vz,dx,dy,dz,startFlag = struct.unpack('<fffffff',data)
        except socket.error:
            vx,vy,vz,dx,dy,dz,startFlag = 0,0,0,0,0,0,0
        
        # CSV Writing Stuff
        with open(r'Logs\ '+name+'-'+dateToday+'-'+str(MatlabScenario)+'.csv','a')as csvfile:
            dataLog = csv.writer(csvfile, delimiter=',')
            dataLog.writerow([startFlag, vx, vy, vz, dx, dy, dz, angularAcc.x, angularAcc.y, angularAcc.z, linearAcc.x, linearAcc.y, linearAcc.z, ppg, gsr, elapsed])    
        
        elapsed += unix.time() - previousTime
        previousTime = unix.time()
		
        if latched:
		experimentStart=elapsed
		if startFlag:
			latched=False
            
        if elapsed - experimentStart > initalWaitTime - 5:
            scenario = myScenario
        
        if elapsed - experimentStart > initalWaitTime:
            start = True
        
        if elapsed - experimentStart > initalWaitTime+droneFlybyTime:
            start = False
            reset = True
            scenario = 0
            
        if elapsed - experimentStart > initalWaitTime+droneFlybyTime+postWaitTime and iterations > 0:
            experimentStart = elapsed
            iterations -= 1
            reset=False
        
        if iterations <= 0:
            break
        
        print elapsed
        #unix.sleep(Ts)

except KeyboardInterrupt:
    print "Interrupted"
    
ovr.destroy(hmd)
ovr.shutdown()
ser.write(struct.pack('B', 0x20))
wait_for_ack()
ser.close()
print "All done!"
