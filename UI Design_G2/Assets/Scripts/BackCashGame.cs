﻿using UnityEngine;
using System.Collections;

public class BackCashGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey (KeyCode.Escape)) 
		{
			Application.LoadLevel ("Practise Scene For Cash Game");
		}
	}
}
