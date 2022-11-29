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
with controls for the robot. The **camera control** panel contains a dpad, a reset button, and 2 adjustment
sections labeled *Pan* and *Tilt*. The up and down arrow on the dpad tilt the robot's head up and down. The 
left and right on the dpad pan the head in the respective direction. The change in degrees each press of a 
button makes is represented by the adjustment sections. Higher or lower these to your liking.

**IMPORTANT NOTE:** The reset button sets the robots camera to a neutral 0 degree tilt and 0 degree pan.
The cockpit keeps track of the current angles of the robot's head, but when starting up the cockpit
it assumes the robot's head is already at its neutral position. So upon each start of the cockpit **please**
**reset the head.**

## TODO: Setup