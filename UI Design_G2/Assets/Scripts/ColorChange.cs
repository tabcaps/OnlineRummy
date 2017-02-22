using UnityEngine;
using System.Collections;
//using UnityEngine.Rendering;
using System;

public class ColorChange : MonoBehaviour 
{
	//public Material material;
	bool clicked;
	// Use this for initialization
	void Start () 
	{
		
			
	}
	
	// Update is called once per frame
	void Update () 
	{
	
		if(Input.GetMouseButtonDown(0))
		{
			if(!clicked)
			{
				//renderer.material.color = Color.red;
		
				clicked = true;
			}
			else
			{
				GetComponent<Renderer>().material.color = Color.white;
				clicked = false;
			}
		}
	}
}
