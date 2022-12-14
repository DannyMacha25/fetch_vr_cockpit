using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Messages.geometry_msgs;
using Messages;
using Ros_CSharp;
using UnityEngine.UI;


public class Body_Panel_Actions : MonoBehaviour {
	[Header("Ros")]
	public ROSCore rosmaster;
	public string topic;
	[Header("Movement Values")]
	public double speed;
	private NodeHandle nh;
	[Header("Misc")]
	public GameObject controller;
	public Text touchPadButtonText;

	private bool useTouchpadInput = false;
	private double xInputLiniency = .8, yInputLiniency = .8;

	private Publisher<Twist> pub;
	enum BUTTON_STATE
    {
		left,
		right,
		back,
		forward,
		none
    };

	BUTTON_STATE current_state = BUTTON_STATE.none;
	void Start () {
		nh = rosmaster.getNodeHandle();

		pub = nh.advertise<Twist>(topic, 10);
	}
	
	// Update is called once per frame
	void Update () {

		if(current_state != BUTTON_STATE.none)
        {
			if (!controller.GetComponent<VRTK.VRTK_ControllerEvents>().triggerPressed)
			{
				Debug.Log("Released");
				current_state = BUTTON_STATE.none;
			}
		}

		Twist goal;
        switch (current_state)
        {
			
			case BUTTON_STATE.left:
				goal = CreateTwistMsg(rZ: 2);
				pub.publish(goal);
				break;

			case BUTTON_STATE.right:
				goal = CreateTwistMsg(rZ: -2);
				pub.publish(goal);
				break;

			case BUTTON_STATE.back:
				goal = CreateTwistMsg(x: -2);
				pub.publish(goal);
				break;

			case BUTTON_STATE.forward:
				goal = CreateTwistMsg(x: 2);
				pub.publish(goal);
				break;
        }

		//Touchpad Input
		Vector2 touchPadAxis = controller.GetComponent<VRTK.VRTK_ControllerEvents>().GetTouchpadAxis();

		if (useTouchpadInput)
        {
			if(touchPadAxis.x >= xInputLiniency)
            {
				Debug.Log("Turn Right");
				goal = CreateTwistMsg(rZ: -2);
				pub.publish(goal);

			}
			else if(touchPadAxis.x <= -xInputLiniency)
            {
				Debug.Log("Turn Left");
				goal = CreateTwistMsg(rZ: 2);
				pub.publish(goal);
			}
			else if(touchPadAxis.y >= yInputLiniency)
            {
				Debug.Log("Move Forward");
				goal = CreateTwistMsg(x: 2);
				pub.publish(goal);
			}
			else if(touchPadAxis.y <= -yInputLiniency)
            {
				Debug.Log("Move Backward");
				goal = CreateTwistMsg(x: -2);
				pub.publish(goal);
			}
            else
            {
				goal = CreateTwistMsg();
				pub.publish(goal);
            }
        }
	}

	Messages.geometry_msgs.Twist CreateTwistMsg(double x = 0,double y = 0, double z = 0, double rX = 0, double rY = 0, double rZ = 0)
    {
		Messages.geometry_msgs.Twist twist = new Messages.geometry_msgs.Twist();
		twist.angular = new Messages.geometry_msgs.Vector3();
		twist.linear = new Messages.geometry_msgs.Vector3();

		twist.angular.x = rX * speed;
		twist.angular.y = rY * speed;
		twist.angular.z = rZ * speed;

		twist.linear.x = x * speed;
		twist.linear.y = y * speed;
		twist.linear.z = z * speed;

		return twist;
    }

	public void rotateLeftPressed()
    {
		current_state = BUTTON_STATE.left;

    }

	public void buttonReleased()
	{
		//current_state = BUTTON_STATE.none;
        if (!controller.GetComponent<VRTK.VRTK_ControllerEvents>().triggerPressed)
        {
			Debug.Log("Released");
		}
		
	}

	public void rotateRightPressed()
	{
		current_state = BUTTON_STATE.right;
		Debug.Log("Pressed");
	}

	public void moveForwardPressed()
    {
		current_state = BUTTON_STATE.forward;
    }

	public void moveBackwardPressed()
    {
		current_state = BUTTON_STATE.back;
    }

	public void toggleTouchpadInput()
    {
		useTouchpadInput = !useTouchpadInput;
		if (useTouchpadInput)
		{
			touchPadButtonText.text = "Turn Off";
			Debug.Log("[Body Panel Actions]: Touchpad input turned on!");
		}
        else
        {
			touchPadButtonText.text = "Turn On";
			Debug.Log("[Body Panel Actions]: Touchpad input turned off!");
		}
    }
}
