function y = genTraj(IC,FC,tauf,h,ONOFF)

%% Interface
%   IC,FC   - terminal conditions
%   tauf    - length of the dimensionless arc
%   h       - graphics handle:: trajectory plot
% Generated trajectory
%% Initialization and mappingof terminal conditions
% IC-"i" d/dt-"dt"  d/dtau-"p"
%
d2r=1;
xi=IC(1:3);
Vi=IC(4);
teti=d2r*IC(5);psi=d2r*IC(6);
Axi=IC(7);
tetidt=d2r*IC(8);psidt=d2r*IC(9);
% TeminalC-"f" d/dt-"dt"  d/dtau-"p"
%
xf=FC(1:3);
Vf=FC(4);
tetf=d2r*FC(5);psif=d2r*FC(6);
Axf=FC(7);
tetfdt=d2r*FC(8);psifdt=d2r*FC(9);
%% Derivatives at IC
%
xidt=[Vi*cos(teti)*cos(psi);...
    Vi*cos(teti)*sin(psi);...
    Vi*sin(teti)];
xip=xidt/Vi;
xi2dt=xip*Axi+Vi*[-tetidt*sin(teti)*cos(psi)-psidt*cos(teti)*sin(psi);...
    -tetidt*sin(teti)*sin(psi)+psidt*cos(teti)*cos(psi);...
    tetidt*cos(teti)];
xi2p=(xi2dt-xidt*Axi/Vi)/Vi^2;
xi3p=[0; 0; 0];
%% Derivatives at FC
%
xfdt=[Vf*cos(tetf)*cos(psif);...
    Vf*cos(tetf)*sin(psif);...
    Vf*sin(tetf)];
xfp=xfdt/Vf;
xf2dt=xfp*Axf+Vf*[-tetfdt*sin(tetf)*cos(psif)-psifdt*cos(tetf)*sin(psif);...
    -tetfdt*sin(tetf)*sin(psif)+psifdt*cos(tetf)*cos(psif);...
    tetfdt*cos(tetf)];
xf2p=(xf2dt-xfdt*Axf/Vf)/Vf^2;
xf3p=[0; 0; 0];
%% Polynomial coefficients of 7th order polinomial

 a0 = xi; a1 = xip; a2 = xi2p; a3 = xi3p;
 a4 = -(2*xf3p+8*xi3p)/tauf + (30*xf2p - 60*xi2p)/(tauf^2) - (180*xfp + 240*xip)/(tauf^3) + 420*(xf-xi)/(tauf^4);
 a5 = (10*xf3p+20*xi3p)/(tauf^2) - (140*xf2p - 200*xi2p)/(tauf^3) + (780*xfp + 900*xip)/(tauf^4) - 1680*(xf-xi)/(tauf^5);
 a6 = -(15*xf3p+20*xi3p)/(tauf^3) + (195*xf2p - 225*xi2p)/(tauf^4) - (1020*xfp + 1080*xip)/(tauf^5) + 2100*(xf-xi)/(tauf^6);
 a7 = 7*(xf3p+xi3p)/(tauf^4) - 84*(xf2p - xi2p)/(tauf^5) + 420*(xfp + xip)/(tauf^6) - 840*(xf-xi)/(tauf^7);
 a = [a0 a1 a2 a3 a4 a5 a6 a7];
 %% Numerical run along the trajectory
 tau=0;
 S=eye(3,100);
 for i=0:99
     tau=tau+tauf/100;
     %%Tau_vector as a function of variable tau==position along the dimensionless path
     tauvector = [1 tau tau^2/factorial(2) tau^3/factorial(3) tau^4*factorial(2)/factorial(4) tau^5*factorial(3)/factorial(5) tau^6*factorial(4)/factorial(6) ...
         tau^7*factorial(5)/factorial(7)]';
     %% Get trajectory
         x=a*tauvector;
     S(1,i+1)=x(1,1);
     S(2,i+1)=x(2,1);
     S(3,i+1)=x(3,1);
 end
% 

y=a;
end