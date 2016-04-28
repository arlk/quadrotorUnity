function [P,a] = createTrajectories(I,F,l)
IC = [I(1) I(2) I(3) 1 0 I(4)*pi/180 0 0 0]';
FC = [F(1) F(2) F(3) 1 0 F(4)*pi/180 0 0 0]';
[a , S] = genTraj(IC,FC,l,0,0);
X = S(1,:);
X = X(12:end);
Y = S(2,:);
Y = Y(12:end);
figure;plot(X,Y);
grid on;
P = [X' Y'];


