using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTK
{
	public class Joystick : VRTK_InteractableObject
	{
		Transform tf;
		Rigidbody rb;
		public float radius;
		float initX, initY,initZ;
		Vector3 center;
		// Use this for initialization
		protected void Start()
		{
			tf = GetComponent<Transform>();
			rb = GetComponent<Rigidbody>();
			initX = rb.position.x;
			initY = rb.position.y;
			initZ = rb.position.z;

			center = rb.position;

			Debug.Log(initX + " " + initY + " " + initZ);
		}

		protected override void FixedUpdate() {
			base.FixedUpdate();
            if (!IsGrabbed())
            {
				rb.velocity = Vector3.zero;
				rb.position = center;
            }


			float distance = Vector3.Distance(center, rb.position);
            if (distance > radius)
            {
				Vector3 newLocation = rb.position;
				Vector3 fromOriginToObject = newLocation - center; //~GreenPosition~ - *BlackCenter*
				fromOriginToObject *= radius / distance; //Multiply by radius //Divide by Distance
				transform.position = center + fromOriginToObject; //*BlackCenter* + all that Math
            }
            else
            {
				Debug.Log(distance);
			}

            if (IsGrabbed())
            {
				rb.position = center;

			}

		}



	}
}