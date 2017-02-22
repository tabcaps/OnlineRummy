using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardFlipper : MonoBehaviour 
{
	SpriteRenderer spriteRenderer;
	public AnimationCurve scaleCurve;
	public Image cardImage;
	//public Image cardImage2;
	public float duration=0.5f;
	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();
		
	}
	public void flipCard(Sprite startImage,Sprite endImage)
	{
		Debug.Log ("flipcard");
		StopCoroutine(flip(startImage, endImage));
        StartCoroutine (flip (startImage,endImage));
	}
	IEnumerator flip(Sprite startImage,Sprite endImage)
	{
		Debug.Log ("flip");
        cardImage.sprite =  startImage;
		float time = 0f;
		while(time<=1f)
		{
			float scale = scaleCurve.Evaluate (time);
			time = time + Time.deltaTime / duration;
			Vector3 localScale = transform.localScale;
			localScale.x = scale;
			transform.localScale = localScale;
			if(time>=0.5f)
			{
                cardImage.sprite = endImage;
			}
			yield return new WaitForFixedUpdate ();
		}
		//if (cardIndex == -1) {
			//cardModel.toggleFace (false);
		//} 
		//else 
		//{
			//cardModel.cardIndex = cardIndex;
			//cardModel.toggleFace (true);
		//}
	}

}
