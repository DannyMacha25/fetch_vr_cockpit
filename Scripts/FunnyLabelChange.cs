using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunnyLabelChange : MonoBehaviour {

	// Use this for initialization
	Text text;
	void Start () {
		text = this.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void changeLabel1()
    {
		text.text = "hehe bongos";	
    }
	public void changeLabel2()
	{
		text.text = "hehe not bongos";
	}

}
