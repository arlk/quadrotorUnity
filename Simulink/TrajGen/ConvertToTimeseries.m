
%INSERT UNITY's XZY order  !! UNITY'S y is matlab's z!! 
%Initial pos and heading
xi = -10.89; zi = 2.33; yi = 48.737; hi = 0;
I = [xi yi zi hi];
%Final pos and heading
xf = 63.37; zf = 2.33; yf = 48.737; hf = 0;
F = [xf yf zf hf];
l = 10;
%%
[P,a] = createTrajectories(I,F,l);
clear xi yi zi hi xf zf yf hf F I l 
speed = 10;%% incre,slow,...decreas,speed up

x = interp(P(:,1),speed);

ToPyPath = [];
time = 0.001:0.001:length(x)*0.001;

   
    x = interp(P(:,1),speed);
    y = interp(P(:,2),speed);
    
    ToPyPath = [ToPyPath x y time'];

clear x y time speed
csvwrite('Trajectories.csv', [ToPyPath] );