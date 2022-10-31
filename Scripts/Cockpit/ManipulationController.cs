using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ros_CSharp;
using Messages.sensor_msgs;
using Messages.geometry_msgs;
using Messages;

public class ManipulationController : MonoBehaviour {
	[Header("Ros")]
	public ROSCore rosmaster;
	public string topic;
	public string mapFrameId;
	
	private NodeHandle nh;
	private Publisher<JointState> pub;
	private Publisher<PoseArray> posePub;
	void Start () {
		nh = rosmaster.getNodeHandle();

		posePub = nh.advertise<PoseArray>(topic, 10);
		pub = nh.advertise<JointState>(topic, 10);

	}
	
	void Update () {
		
	}

	private double degreeToRadians(double d)
	{
		return d * (Mathf.PI / 180.0);
	}

	private double[] degreeToRadians(double[] vals)
    {
		double[] newArr = new double[vals.Length];
		for(int i = 0; i < vals.Length; i++)
        {
			newArr[i] = degreeToRadians(vals[i]);
        }

		return newArr;
    }

	private JointState CreateJointStateMsg(string[] joints, double[] values)
    {
		JointState msg = new JointState();
		msg.name = new string[joints.Length];
		msg.position = new double[values.Length];

		msg.name = joints;
		msg.position = values;

		return msg;
    }

	private JointState CreateArmJointStateMsg(double[] values)
    {
		string[] joints = { "shoulder_pan_joint", "shoulder_lift_joint", "upperarm_roll_joint", "elbow_flex_joint", "forearm_roll_joint", "wrist_flex_joint", "wrist_roll_joint" };

		return CreateJointStateMsg(joints, values);
    }

	public void ClawMode()
    {
		publishGoal(5, 5, 5);

    }

	public void ResetArm()
    {
		publishGoal(1.245,.897,1.160);
	}

	public void publishGoal(double x=0,double y=0, double z=0)
	{
		PoseArray poseArray = new PoseArray();
		poseArray.header = new Messages.std_msgs.Header();
		poseArray.header.frame_id = mapFrameId;
		poseArray.poses = new Messages.geometry_msgs.Pose[1];

		poseArray.poses[0] = new Messages.geometry_msgs.Pose();
		poseArray.poses[0].position = new Point();

		poseArray.poses[0].position.x = x;
		poseArray.poses[0].position.y = y;
		poseArray.poses[0].position.z = z;

		poseArray.poses[0].orientation = new Messages.geometry_msgs.Quaternion();
		poseArray.poses[0].orientation.x = .804;
		poseArray.poses[0].orientation.y = .030;
		poseArray.poses[0].orientation.z = .593;
		poseArray.poses[0].orientation.w = -.022;
		posePub.publish(poseArray);

	}
}
