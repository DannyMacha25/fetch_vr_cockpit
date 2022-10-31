using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ros_CSharp;
using Messages;
using Messages.sensor_msgs;
using UnityEngine.UI;


public class CameraPanelActions : MonoBehaviour {
	//Uses heat_tilt_joint and head_pan_joint

	public ROSCore rosmaster;
	public Button[] movementButtons;
	public string topic;
	//public double degrees;
	public double panChangeInDegrees;
	public double tiltChangeInDegrees;

	private double currRotationDeg;
	private double currTiltRotationDeg;
	private bool canEnable; //Hack fix because cant enable buttons in side thread
	private bool buttonsDisabled = false;
	private Publisher<JointState> pub;
	private NodeHandle nh;
	private Queue<double[]> q;
	void Start () {
		q = new Queue<double[]>();
		nh = rosmaster.getNodeHandle();

		pub = nh.advertise<JointState>(topic, 10);
		Subscriber<Messages.std_msgs.Int32> sub = nh.subscribe<Messages.std_msgs.Int32>("/head_command_status", 10, stateCB);
		//resetHeadPos();
	}

	void Update()
    {
        if (canEnable)
        {
			enableButtons();
        }
    }

	private void stateCB(Messages.std_msgs.Int32 msg)
    {
		Debug.Log(msg.data);
		double[] arr = q.Dequeue();
		if(msg.data == 6)
        {
			currRotationDeg = arr[0];
			currTiltRotationDeg = arr[1];
		}


		if(currRotationDeg > 90)
        {
			currRotationDeg = 90;
        }
		if(currRotationDeg < -90)
        {
			currRotationDeg = -90;
        }

		if(currTiltRotationDeg > 90)
        {
			currTiltRotationDeg = 90;
        }
		if(currTiltRotationDeg < -45)
        {
			currTiltRotationDeg = -45;
        }

		Debug.Log(currRotationDeg + " " + currTiltRotationDeg);
		//Enable buttons
		canEnable = true;
    }
	
	private void disableButtons()
    {
		canEnable = false;
		buttonsDisabled = true;
		foreach (Button b in movementButtons){
			b.interactable = false;
        }
    }

	private void enableButtons()
    {
		if (buttonsDisabled)
		{
			foreach (Button b in movementButtons)
			{
				b.interactable = true;
			}
			buttonsDisabled = true;
		}
    }

	private double degreeToRadians(double d)
    {
		return d * (Mathf.PI / 180.0);
    }
	
	/// <summary>
	/// Creates a JointState goal for paning the head. Negative degree
	/// for right and positive for left. With Fetch, +-90 degrees of 
	/// range.
	/// </summary>
	/// <param name="d"</param>
	/// <returns>JointState Goal</returns>
	private JointState makeHeadGoal(double dPan,double dTilt)
    {
		JointState goal = new JointState();
		goal.name = new string[2];
		goal.position = new double[2];
		goal.velocity = new double[2];
		goal.effort = new double[2];

		goal.name[0] = "head_pan_joint";
		goal.position[0] = degreeToRadians(dPan);
		goal.velocity[0] = 0;
		goal.effort[0] = 0;

		goal.name[1] = "head_tilt_joint";
		goal.position[1] = degreeToRadians(dTilt);
		goal.velocity[1] = 0;
		goal.effort[1] = 0;
		return goal;
	}

	public void panLeft()
	{
		JointState goal = makeHeadGoal(currRotationDeg + panChangeInDegrees, currTiltRotationDeg);
		//Definitely should wait for goal succeed before changing the current rotation
		double[] arr = { currRotationDeg + panChangeInDegrees, currTiltRotationDeg };
		q.Enqueue(arr); //Enqueues the new angle for the new joint

		disableButtons();
		pub.publish(goal);
	}
	public void panRight()
    {
		JointState goal = makeHeadGoal(currRotationDeg - panChangeInDegrees, currTiltRotationDeg);
		//Definitely should wait for goal succeed before changing the current rotation
		double[] arr = { currRotationDeg - panChangeInDegrees, tiltChangeInDegrees };
		q.Enqueue(arr); //Enqueues the new angle for the new joint

		disableButtons();
		pub.publish(goal);
	}

	public void tiltUp()
    {
		JointState goal = makeHeadGoal(currRotationDeg,currTiltRotationDeg - tiltChangeInDegrees);
		//Definitely should wait for goal succeed before changing the current rotation
		double[] arr = { currRotationDeg, currTiltRotationDeg - tiltChangeInDegrees };
		q.Enqueue(arr); //Enqueues the new angle for the new joint

		disableButtons();
		pub.publish(goal);
	}

	public void tiltDown()
    {
		JointState goal = makeHeadGoal(currRotationDeg, currTiltRotationDeg + tiltChangeInDegrees);
		//Definitely should wait for goal succeed before changing the current rotation
		double[] arr = { currRotationDeg, currTiltRotationDeg + tiltChangeInDegrees };
		q.Enqueue(arr); //Enqueues the new angle for the new joint

		disableButtons();
		pub.publish(goal);
	}

	/// <summary>
	/// Resets the rotations of the head. Use if rotation not
	/// properly calibrated with currRotationDeg.
	/// </summary>
	public void resetHeadPos()
    {
		disableButtons();
		pub.publish(makeHeadGoal(0, 0)); //Could cause problems if not connected to robot properly???
		double[] arr = { 0,0 };
		q.Enqueue(arr); //Enqueues the new angle for the new joint
	}
}