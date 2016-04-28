clc
clear all

%%%%% Aerial Vehicle Model %%%%%

m = 0.650*4; % mass = 0.650[kg]
g = 9.81; %[m/s^2]
Ix_0 = 7.5e-3*1; %wewwwww         inertia on x axis [kg*m^2]
Iy_0 = 6.5e-3*1; % inertia on y axis [kg*m^2]
Iz_0 = 1.3e-2*1; % inertia on z axis [kg*m^2]

b = 3.13e-5/2; % thrust coefficient [N*s^2] 
d = 7.5e-7/2;  % drag coefficient [N*m*s^2]
Jr = 6e-5;   % rotor inertia    [kg*m^2]
l = 0.23; % arm length      [m]

I = diag([Ix_0, Iy_0, Iz_0]);

%%%%% Actuator Dynamics %%%%%
s = tf('s');

G_motor = tf([75],[1 75]);
%G_motor = tf([100],[1 100]);
%G_motor = tf([0.936],[0.178 1]);

Delay_Act = 0.003; 

H_motor = [G_motor 0 0 0 0 0 0 0; ...
           0 G_motor 0 0 0 0 0 0; ...
           0 0 G_motor 0 0 0 0 0; ...
           0 0 0 G_motor 0 0 0 0; ...
           0 0 0 0 G_motor 0 0 0; ...
           0 0 0 0 0 G_motor 0 0; ...
           0 0 0 0 0 0 G_motor 0; ...
           0 0 0 0 0 0 0 G_motor];
       
T_Fail_1st = 2000
T_Fail_2nd = 2000
T_Fail_3rd = 2000
T_Fail_4th = 1200000

Fail_State_1st = [1 1 1 1 1 1 1 1]';
Fail_State_2nd = [1 1 1 1 1 1 1 1]';
Fail_State_3rd = [1 1 1 1 1 1 1 1]';
Fail_State_4th = [1 1 1 1 1 1 1 1]';


%%%%% L1 Adaptive Controller %%%%%

Bm = inv(I);
Am = -20*diag([1, 1, 1]);
Q = 1*eye(3);
P = lyap(Am',Q);
Ts = 0.001;
Gamma = 100000;
K = 100;


%%%%% Adaptive Allocation %%%%%

[A_fil,B_fil,C_fil,D_fil] = tf2ss([.5],[1 .5]);    
Fil_SISO = ss(A_fil,B_fil,C_fil,D_fil);

A = 10*Am;
Q_aloc = eye(3);
P_aloc = lyap(A',Q_aloc);

Gamma_aloc  = 1;


%%%%% Static Allocation %%%%%

Psi_nom = ...
... 
[       0,       0,    b*l,    b*l,      0,      0,    -b*l,    -b*l; ...
     -b*l,    -b*l,      0,      0,    b*l,    b*l,       0,       0; ...
       -d,       d,      d,     -d,     -d,      d,       d,      -d; ...
        b,       b,      b,      b,      b,      b,       b,       b];


Inverse_Psi_nom = Psi_nom.'*inv(Psi_nom*Psi_nom.')
    


%%% Trajectory Tracking BackStepping Controller %%%
alpha1 = 10;
alpha2 = .5;
alpha3 = 10;
alpha4 = 0.5;
alpha5 = 10;
alpha6 = 0.5;

P_angle = 5;
I_angle = 0.1;
D_angle = 0.2;

% P_angle = 10;
% I_angle = .2;
% D_angle = 0.5;

%%% DATA SAMPLE %%%
Data_T_Smpl = 0.01;
