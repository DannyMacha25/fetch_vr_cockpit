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
	public string joint_topic;
	public string pose_topic;
	public string mapFrameId;

	[Header("Test")]
	public double[] degrees;

	private const double X_MAX = .650, Y_MAX = .4, Z_MAX = 1.2;
	private const double X_MIN = .200, Y_MIN = -.4, Z_MIN = .4;

	private double currX, currY, currZ;
	private double currRotX, currRotY, currRotZ, currRotW;
	private const double DELTA_X = .05, DELTA_Y = .05, DELTA_Z = .05; 

	private Messages.geometry_msgs.Vector3 originPos;
	private Messages.geometry_msgs.TransformStamped origin;
	private NodeHandle nh;
	private Publisher<JointState> pub;
	private Publisher<PoseArray> posePub;
	private Publisher<Messages.std_msgs.Bool> confirmPub;
	void Start () {
		nh = rosmaster.getNodeHandle();

		posePub = nh.advertise<PoseArray>(pose_topic, 10);
		pub = nh.advertise<JointState>(joint_topic, 10);
		confirmPub = nh.advertise<Messages.std_msgs.Bool>("/joint_plan_confirmation", 10);

		nh.subscribe<Messages.geometry_msgs.TransformStamped>("/gripper_position",10, posCB);

	}
	
	void Update () {
		
	}
	private void assignValues()
    {
		/*currX = originPos.x;
		currY = originPos.y;
		currZ = originPos.z;

		xMax = originPos.x + X_MAX;
		xMin = originPos.x + X_MIN;

		yMax = originPos.y + Y_MAX;
		Y_MIN = originPos.y + Y_MIN;

		zMax = originPos.z + Z_MAX;
		zMin = originPos.z + Z_MIN;

		currRotX = origin.transform.rotation.x;
		currRotY = origin.transform.rotation.y;
		currRotZ = origin.transform.rotation.z;
		currRotW = origin.transform.rotation.w;*/
		currX = .608;
		currY = .209;
		currZ = .915;

		currRotX = .726;
		currRotY = .128;
		currRotZ = -.665;
		currRotW = .117;
	}
	private void posCB(Messages.geometry_msgs.TransformStamped msg)
	{
		originPos = msg.transform.translation;
		origin = msg;
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

	private JointState CreateJointStateMsg(string joint, double val)
	{
		JointState msg = new JointState();
		msg.name = new string[1];
		msg.position = new double[1];

		msg.name[0] = joint;
		msg.position[0] = val;

		return msg;
	}

	private JointState CreateArmJointStateMsg(double[] values)
    {
		string[] joints = { "shoulder_pan_joint", "shoulder_lift_joint", "upperarm_roll_joint", "elbow_flex_joint", "forearm_roll_joint", "wrist_flex_joint", "wrist_roll_joint" };

		return CreateJointStateMsg(joints, values);
    }

	public void ClawMode()
    {
		double[] vals = { 20, -60, 180, -50, 0, -60, 0 };
		double[] radian_vals = new double[7];
		radian_vals = degreeToRadians(degrees);
		Debug.Log(radian_vals);
		pub.publish(CreateArmJointStateMsg(radian_vals));
		assignValues();

	}

	public void ClawBack()
    {
		currX -= DELTA_X; 
		if(currX < X_MIN)
        {
			currX = X_MIN;
        }
		publishGoal(currX, currY, currZ);
		StartCoroutine(pubTrue());
    }

	public void ClawForward()
	{
		currX += DELTA_X;
		if (currX > X_MAX)
		{
			currX = X_MAX;
		}
		publishGoal(currX, currY, currZ);
		StartCoroutine(pubTrue());
	}

	public void ClawLeft()
	{
		currY += DELTA_Y;
		if (currY > Y_MAX)
		{
			currY = Y_MAX;
		}
		publishGoal(currX, currY, currZ);
		StartCoroutine(pubTrue());
	}

	public void ClawRight()
	{
		currY -= DELTA_Y;
		Debug.Log(currY + " " + Y_MIN);
		if (currY < Y_MIN)
		{
			currY = Y_MIN;
		}
		publishGoal(currX, currY, currZ);
		StartCoroutine(pubTrue());
	}



	IEnumerator pubTrue()
    {
		Messages.std_msgs.Bool t = new Messages.std_msgs.Bool();
		t.data = true;
		yield return new WaitForSeconds(4);
		confirmPub.publish(t);
	}


	public void ResetArm()
    {
		double[] vals = {92, 90, 0, 86.4, -7.1, 105, 0 };
		StartCoroutine(pubJoints(vals));
		//Debug.Log(radian_vals);
		//pub.publish(CreateJointStateMsg(radian_vals));

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
		poseArray.poses[0].orientation.x = currRotX;
		poseArray.poses[0].orientation.y = currRotY;
		poseArray.poses[0].orientation.z = currRotZ;
		poseArray.poses[0].orientation.w = currRotW;
		posePub.publish(poseArray);


	}

	IEnumerator pubJoints(double[] vals)
    {
		double[] radian_vals = new double[7];
		yield return new WaitForSeconds(2);
		radian_vals = degreeToRadians(degrees);
		string[] joints = { "shoulder_pan_joint", "shoulder_lift_joint", "upperarm_roll_joint", "elbow_flex_joint", "forearm_roll_joint", "wrist_flex_joint", "wrist_roll_joint" };
		for (int i = 0; i < 7; i++)
		{
			Debug.Log("Pubbing " + joints[i]);
			pub.publish(CreateJointStateMsg(joints[i], radian_vals[i]));
			yield return new WaitForSeconds(2);
		}
	}
		
}
