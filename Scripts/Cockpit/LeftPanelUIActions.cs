using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ros_CSharp;
using Messages;


public class LeftPanelUIActions : MonoBehaviour {
	public ROSCore rosmaster;
	private NodeHandle nh;
	public double torso_val;
	[Header("Topics")]
	public string topic;
	public string torso_topic;

	private Publisher<Messages.control_msgs.GripperCommandGoal> pub;
	private Publisher<Messages.std_msgs.String> pub_test;
	private Publisher<Messages.sensor_msgs.JointState> pub_torso;
	// Use this for initialization
	void Start () {
		nh = rosmaster.getNodeHandle();
		pub = nh.advertise<Messages.control_msgs.GripperCommandGoal>(topic,10);
		pub_torso = nh.advertise<Messages.sensor_msgs.JointState>(torso_topic, 10);
		pub_test = nh.advertise<Messages.std_msgs.String>("/test_topic", 5);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OpenGripper()
    {
		Messages.control_msgs.GripperCommandGoal msg = new Messages.control_msgs.GripperCommandGoal();


		msg.command = new Messages.control_msgs.GripperCommand();

		msg.command.position = .1;

		pub.publish(msg);


    }

	public void CloseGripper()
	{
		
		Debug.Log("pushed");
		Messages.control_msgs.GripperCommandGoal msg = new Messages.control_msgs.GripperCommandGoal();
		msg.command = new Messages.control_msgs.GripperCommand();

		msg.command.position = 0.0;

		pub.publish(msg);


	}

	public void RaiseTorso()
    {
		Messages.sensor_msgs.JointState goal = new Messages.sensor_msgs.JointState();
		goal.header = new Messages.std_msgs.Header();

		goal.name = new string[1];
		goal.position = new double[1];

		goal.name[0] = "torso_lift_joint";
		goal.position[0] = torso_val;
		

		pub_torso.publish(goal);
		
	}
}
