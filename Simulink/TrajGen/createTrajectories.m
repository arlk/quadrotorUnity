figure
Paths = cell(10,2);
 k=1;
%% A->B %from behind to front,overhead   


xi = -10.89;%%INSERT UNITY's XZY order 
zi = 2.33;
yi = 48.737;
hi = 0;

xf = 63.37;%%INSERT UNITY's XZY order  !! UNITY'S y is matlab's z!! 
zf = 2.33;
yf = 48.737;
hf = 0;

l=10;

sim('TrajGen')
X = Trajectory.signals.values(:,1);
X = X(12:end);
Y = Trajectory.signals.values(:,2);
Y = Y(12:end);
Paths{k,1} = [X Y];
Paths{k,2} = 'AB';
k = k+1
 plot(X,Y);
grid on;
hold on;
% plot(186,283,'o')
%plot(193,283,'o')
% plot(177,283,'o')
% plot(169,283,'o')
% plot(176,301,'o')
% plot(182,301,'o')
% plot(192,301,'o')
% plot(163,301,'o')
text(xi,yi,'A')
text(xf,yf,'B')
%%axis([-20 250 270 310])
%% C->D
xi = 65.27;%%INSERT UNITY's XZY order  !! UNITY'S y is matlab's z!! 
zi = 1.59;
yi = 48.95;
hi = 0;

xf = -14;%%INSERT UNITY's XZY order  !! UNITY'S y is matlab's z!! 
zf = 1.59;
yf = 48.95;
hf = 0;
l=10;

sim('TrajGen')
X = Trajectory.signals.values(:,1);
X = X(12:end);
Y = Trajectory.signals.values(:,2);
Y = Y(12:end);
Paths{k,1} = [X Y];
Paths{k,2} = 'BC';
k = k+1
 plot(X,Y);
grid on;
hold on;
% plot(186,283,'o')
% plot(193,283,'o')
% plot(177,283,'o')
% plot(169,283,'o')
% plot(176,301,'o')
% plot(182,301,'o')
% plot(192,301,'o')
% plot(163,301,'o')
text(xi,yi,'C')
text(xf,yf,'D')
%%axis([160 200 270 310])
%% 
xi = -6;
yi = 10;
zi = 2;
hi = -90;

xf = -6;
yf = 2;
zf = 2;
hf = -45;


l=7;

sim('TrajGen')
X = Trajectory.signals.values(:,1);
X = X(12:end);
Y = Trajectory.signals.values(:,2);
Y = Y(12:end);
Paths{k,1} = [X Y];
Paths{k,2} = 'CD';
k = k+1;
plot(X,Y);
grid on;
hold on;

text(xi,yi,'C')
%% D -> A
xi = -6;
yi = 2;
zi = 2;
hi = 20;

xf = 3;
yf = 2;
zf = 2;
hf = 90;


l=4;

sim('TrajGen')
X = Trajectory.signals.values(:,1);
X = X(12:end);
Y = Trajectory.signals.values(:,2);
Y = Y(12:end);
Paths{k,1} = [X Y];
Paths{k,2} = 'AD';
k = k+1;
 plot(X,Y);
grid on;
hold on;

text(xi,yi,'D')

%% B - >D

xi = 2.5;
yi = 6.7;
zi = 2;
hi = 170;

xf = -6;
yf = 2;
zf = 2;
hf = -60;


l=10;

sim('TrajGen')
X = Trajectory.signals.values(:,1);
X = X(12:end);
Y = Trajectory.signals.values(:,2);
Y = Y(12:end);
Paths{k,1} = [X Y];
Paths{k,2} = 'BD';
k = k+1;
 plot(X,Y);

 %% A->C
 
 
xi = 3;
yi = 2;
zi = 0;
hi = 180;

xf = -6;
yf = 10;
zf = 2;
hf = 90;


l=10;

sim('TrajGen')
X = Trajectory.signals.values(:,1);
X = X(12:end);
Y = Trajectory.signals.values(:,2);
Y = Y(12:end);
Paths{k,1} = [X Y];
Paths{k,2} = 'AC';
k = k+1;
 plot(X,Y);
 %% A->E
xi = 3;
yi = 2;
zi = 0;
hi = 180;

xf = -5;
yf = -6;
zf = 2;
hf = -90;
l=18;

sim('TrajGen')
X = Trajectory.signals.values(:,1);
X = X(12:end);
Y = Trajectory.signals.values(:,2);
Y = Y(12:end);
Paths{k,1} = [X Y];
Paths{k,2} = 'AE';
k = k+1;
 plot(X,Y);
grid on;
hold on;
plot(-4,-4,'o')
plot(-4,-1,'o')
text(xi,yi,'A')
%axis([-8.43 4.36 0 12])

 %% B->E
xi = 2.5;
yi = 6.7;
zi = 2;
hi = 170;

xf = -5;
yf = -6;
zf = 2;
hf = -90;
l=17;

sim('TrajGen')
X = Trajectory.signals.values(:,1);
X = X(12:end);
Y = Trajectory.signals.values(:,2);
Y = Y(12:end);
Paths{k,1} = [X Y];
Paths{k,2} = 'AE';
k = k+1;
 plot(X,Y);
grid on;
hold on;
plot(-4,-4,'o')
plot(-4,-1,'o')
plot(0.58,5.67,'o')
plot(-1.96,5.67,'o')
plot(-3.12,7.06,'o')
text(xi,yi,'A')
%axis([-8.43 4.36 0 12])

 %% C->E
xi = -6;
yi = 10;
zi = 2;
hi = -90;

xf = -5;
yf = -6;
zf = 2;
hf = -90;
l=17;

sim('TrajGen')
X = Trajectory.signals.values(:,1);
X = X(12:end);
Y = Trajectory.signals.values(:,2);
Y = Y(12:end);
Paths{k,1} = [X Y];
Paths{k,2} = 'AE';
k = k+1;
 plot(X,Y);
grid on;
hold on;
plot(-4,-4,'o')
plot(-4,-1,'o')
plot(0.58,5.67,'o')
plot(-1.96,5.67,'o')
plot(-3.12,7.06,'o')
plot(-7.6,7.06,'o')
text(xi,yi,'A')
%axis([-8.43 4.36 0 12])


 %% D->E
xi = -6;
yi = 2;
zi = 2;
hi = 20;

xf = -5;
yf = -6;
zf = 2;
hf = -90;
l=3;

sim('TrajGen')
X = Trajectory.signals.values(:,1);
X = X(12:end);
Y = Trajectory.signals.values(:,2);
Y = Y(12:end);
Paths{k,1} = [X Y];
Paths{k,2} = 'AE';
k = k+1;
 plot(X,Y);
grid on;
hold on;
plot(-4,-4,'o')
plot(-4,-1,'o')
plot(0.58,5.67,'o')
plot(-1.96,5.67,'o')
plot(-3.12,7.06,'o')
plot(-7.6,7.06,'o')
text(xi,yi,'A')
%axis([-8.43 4.36 0 12])

 %%
 title('Collision Free Trajectories')
 xlabel('x axis');
 ylabel('z axis');
 save('DronePaths.mat','Paths')