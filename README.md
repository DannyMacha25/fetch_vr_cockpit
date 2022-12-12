## Vr Cockpit
A cockpit design to control a fetch robot through ROS and VR.

## Summary
This Unity project is a cockpit design used to control a fetch robot. I developed this 
in hopes of understanding and possibly finding ways to fully utilize such a way to control a
robot. 

## Tutorial
Upon loading up the project (see 'Setup' below) you should go to the scenes folder. Here, open the 
cockpit scene for access to the design. While there is an explanation in the scene on each control,
I'll be explaining them here too. 

The cockpit is split up into several panels, each with their own group of functions. In the center, if
connected to the robot, is a panel that shows the head camera view of the fetch robot. 

To control the robot, there are 3 (as of writing this, also the right red panel is unused) panels
with controls for the robot. 

The **camera control** panel contains a dpad, a reset button, and 2 adjustment
sections labeled *Pan* and *Tilt*. The up and down arrow on the dpad tilt the robot's head up and down. The 
left and right on the dpad pan the head in the respective direction. The change in degrees each press of a 
button makes is represented by the adjustment sections. Higher or lower these to your liking.

**IMPORTANT NOTE:** The reset button sets the robots camera to a neutral 0 degree tilt and 0 degree pan.
The cockpit keeps track of the current angles of the robot's head, but when starting up the cockpit
it assumes the robot's head is already at its neutral position. So upon each start of the cockpit **please**
**reset the head.**

The **body control** panel contains a dpad and a *Joystick Input* toggle. The left and right on the dpad
turn the body of the robot in the respective direction. The up and down on the dpad move the robot forward and backwards.
*Joystick Input* toggle will toggle joystick input of the right joystick. Use the same directions as the dpad to move
the robot.

The **left** panel is used for manipulation. The *claw* button puts the robot in claw machine mode and you can use the
dpad to move the gripper like one would a claw machine. Dpad is for directional input and the two inner directional buttons
are for height adjustment.
**TODO:** A stow button needs to be implemented. The reset button needs to be fully implemented.

## TODO: Setup
Req: A linux environment, a unity environment.

1) Install Unity :3
2) Follow the Unity environment setup [here](https://github.com/uml-robotics/valkyrieVR)
3) Clone this repository in your assets folder
4) Follow the get started section [here](https://github.com/uml-robotics/fetchit)
5) Clone [this](https://github.com/uml-robotics/fetch_vr_backend) backend in your catkin src directory, then compile.

Once you have everything installed, to start everthing...

On the linux side
1) Launch roscore
2) `$ roslaunch fetchit_challenge (any arena you want)` This will launch gazebo 
3) `$ roscd fetch_vr_backend` then `$ make tmux`this provides a backend to parse moveit commands from unity.

On the unity side
1) In the ros
