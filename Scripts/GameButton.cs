using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Messages;
using Ros_CSharp;

namespace VRTK
{
	public class GameButton : VRTK_InteractableObject
	{
		[Header("Button")]
		public string text;
		public string topic;
		public ROSCore rosmaster;
		private NodeHandle nh;
		private Publisher<Messages.geometry_msgs.Twist> pub;
		// Use this for initialization
		protected void Start()
		{
			nh = rosmaster.getNodeHandle();
			pub = nh.advertise<Messages.geometry_msgs.Twist>(topic, 10);
		}

		public override void StartUsing(VRTK_InteractUse usingObject)
		{
			base.StartUsing(usingObject);
			Debug.Log(text);
			Messages.geometry_msgs.Twist msg = new Messages.geometry_msgs.Twist();
			msg.linear = new Messages.geometry_msgs.Vector3();
			msg.linear.x = 1.0;
			pub.publish(msg);
        }
	}
}
