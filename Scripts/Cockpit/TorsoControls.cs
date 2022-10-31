using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Messages;
using Ros_CSharp;
using UnityEngine.UI;

public class TorsoControls : MonoBehaviour {
	public ROSCore rosmaster;
	private NodeHandle nh;
	[Header("Topics")]
	public string torso_topic;
	public Slider slider;


	private Publisher<Messages.sensor_msgs.JointState> pub_torso;
	// Use this for initialization
	void Start()
	{
		nh = rosmaster.getNodeHandle();
		pub_torso = nh.advertise<Messages.sensor_msgs.JointState>(torso_topic, 10);
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeTorsoHeight()
    {
		double valueInMeters = slider.value / 1000.0;
		Messages.sensor_msgs.JointState goal = MakeTorsoGoal(valueInMeters);

		pub_torso.publish(goal);
    }

	private Messages.sensor_msgs.JointState MakeTorsoGoal(double meters)
    {

		Messages.sensor_msgs.JointState goal = new Messages.sensor_msgs.JointState();
		goal.header = new Messages.std_msgs.Header();

		goal.name = new string[1];
		goal.position = new double[1];

		goal.name[0] = "torso_lift_joint";
		goal.position[0] = meters;

		return goal;

	}
}
