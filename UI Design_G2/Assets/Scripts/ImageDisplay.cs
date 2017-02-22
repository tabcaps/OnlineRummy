using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageDisplay : MonoBehaviour 
{
	int count = 0;
	public Sprite[] im;
	GameObject go;
	Sprite img=new Sprite();
	Texture2D ui_text;
	Vector3 startPosition;
	Vector3 discPosition=new Vector3(550,441,0);
	void Start()
	{
		startPosition = GameObject.Find ("CardImage").transform.position;
	}
	public void changeImage()
	{
		if (count == 0) 
		{
			Vector3 translate = discPosition;
			GameObject.Find ("CardImage").GetComponent<Image> ().sprite = im [0];
			 go = GameObject.Find ("CardImage");
			//go.transform.position = translate;
			go.transform.position=translate;
			ui_text = Resources.Load<Texture>("texture1")as Texture2D;
			Sprite.Create (ui_text, new Rect (603, 200, 80, 60), new Vector2 (0.5f,0.5f));
			count++;
		}
		else if(count==1)
		{
			GameObject.Find ("CardImage").GetComponent<Image> ().sprite = im [1];
			count++;
			go.transform.position=startPosition;
		}
		else if(count==2)
		{
			GameObject.Find ("CardImage").GetComponent<Image> ().sprite = im [2];
			count++;
		}
		else if(count==3)
		{
			GameObject.Find ("CardImage").GetComponent<Image> ().sprite = im [3];
			count = 0;
		}
	}

}
