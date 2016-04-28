load('DronePaths.mat');
CtoD = Paths{6,1};
DtoC = flip(CtoD);
%Expand data so that it follows a slow trajectory 
xDtoC = interp(DtoC(:,1),4);
yDtoC = interp(DtoC(:,2),4);
%Create Timeseries
Tsx = timeseries(xDtoC,0.001:0.001:length(xDtoC)*0.001);
Tsy = timeseries(yDtoC,0.001:0.001:length(xDtoC)*0.001);

%%
P = Paths{1,1};
speed = 10;%% incre,slow,...decreas,speed up

x = interp(P(:,1),speed);

ToPyPath = [];
time = 0.001:0.001:length(x)*0.001;
for k=1:1
    P = Paths{k,1};
    x = interp(P(:,1),speed);
    y = interp(P(:,2),speed);
    
    ToPyPath = [ToPyPath x y time'];
end


csvwrite('Trajectories.csv', [ToPyPath] );