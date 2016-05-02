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
UDP_RECV_UNITY_PORT = 25005
Ts = 0.01

############## Communication stuff ##############
recvSock = socket.socket(socket.AF_INET, # Internet
                             socket.SOCK_DGRAM) # UDP
recvSock.bind((UDP_IP, UDP_RECV_UNITY_PORT))

sendSock = socket.socket(socket.AF_INET, # Internet
                             socket.SOCK_DGRAM) # UDP
##################################################

###Initialize OVR
ovr.initialize(None)
hmd, luid = ovr.create()
hmdDesc = ovr.getHmdDesc(hmd)
angularAcc = ovr.Vector3f(0,0,0)
linearAcc = ovr.Vector3f(0,0,0)
timeSec = 0

reset = False
start = False
elapsed = 0.0
scenario = 0

while True:
    if elapsed > 0:
        scenario = 1
    
    if elapsed > 15:
        start = True
    
    if elapsed > 25:
        start = False
        reset = True
        scenario = 0
       
    # Reiterate
    
    # Oculus IMU Stuff
    ts  = ovr.getTrackingState(hmd, ovr.getTimeInSeconds(), True)
    if ts.StatusFlags & (ovr.Status_OrientationTracked | ovr.Status_PositionTracked):
        angularAcc = ts.HeadPose.AngularAcceleration
        linearAcc = ts.HeadPose.LinearAcceleration
        timeSec = ts.HeadPose.TimeInSeconds
        sys.stdout.flush()

    # Sending Stuff
    msg = struct.pack('<??', start, reset)
    sendSock.sendto(msg, (UDP_IP, UDP_SEND_SIMLK_PORT))
    
    msg = struct.pack('<d', scenario)
    sendSock.sendto(msg, (UDP_IP, UDP_SEND_UNITY_PORT))
    
    # Receiving Stuff
    data, addr = recvSock.recvfrom(1024) # Test buffer sizes. (TO DO)
    vx,vy,vz,dx,dy,dz = struct.unpack('<ffffff',data)
    
    # CSV Writing Stuff
    with open(r'Logs\ '+name+'-'+dateToday+'-'+scenario+'.csv','a')as csvfile:
    dataLog = csv.writer(csvfile, delimiter=',')
    dataLog.writerow([vx, vy, vz, dx, dy, dz, angularAcc.x, angularAcc.y, angularAcc.z, linearAcc.x, linearAcc.y, linearAcc.z, timeSec])    
    
    elapsed += Ts
    sleep(Ts)

ovr.destroy(hmd)
ovr.shutdown()