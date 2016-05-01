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
Ts = 0.01

############## Communication stuff ##############
sendSockUnity = socket.socket(socket.AF_INET, # Internet
                             socket.SOCK_DGRAM) # UDP


sendSock = socket.socket(socket.AF_INET, # Internet
                             socket.SOCK_DGRAM) # UDP
##################################################

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

    msg = struct.pack('<??', start, reset)
    sendSock.sendto(msg, (UDP_IP, UDP_SEND_SIMLK_PORT))
    
    msg = struct.pack('<d', scenario)
    sendSock.sendto(msg, (UDP_IP, UDP_SEND_UNITY_PORT))
    
    elapsed += Ts
    sleep(Ts)