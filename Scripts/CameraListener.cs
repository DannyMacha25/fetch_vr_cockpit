using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ros_CSharp;
using Messages;
using UnityEngine.UI;
using System.IO;
using System;

public class CameraListener : MonoBehaviour
{
	public ROSCore rosmaster;
	public string topic_name;
	private NodeHandle nh;
	private Subscriber<Messages.sensor_msgs.Image> sub;
	Messages.sensor_msgs.Image currImg;
	bool recieved = false;
	void Start()
	{
		nh = rosmaster.getNodeHandle();

		sub = nh.subscribe<Messages.sensor_msgs.Image>(topic_name, 1, subCb);

	}
	private void subCb(Messages.sensor_msgs.Image msg)
	{
		Debug.Log("Msg Recieved " + msg.width + "x" + msg.height);
		currImg = msg;
		recieved = true;
	}
	void Update()
    {
		if (recieved)
		{
			updateSprite();
		}
    }
	void updateSprite()
    {
		Texture2D e = new Texture2D((int)currImg.width, (int)currImg.height);
		Debug.Log(currImg.data[5]);
		string arr = "";
		for(int i = 0; i < 50; i++)
        {
			arr += currImg.data[i]+ " ";
        }
		//Debug.Log(arr);
		e.LoadRawTextureData(currImg.data);
		File.WriteAllBytes(Application.dataPath + "/SavedScreen.txt", currImg.data);
		Rect r = new Rect(0f,0f,(float)e.width,(float)e.height);
		//this.GetComponent<UnityEngine.UI.Image>().sprite = Sprite.Create(e, r, new Vector2(0.0f, 0.0f));
		this.GetComponent<RawImage>().texture = e;
	}

}

