using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Deck : MonoBehaviour 
{
	
	public List<int>cards = new List<int>();
	public IEnumerable<int>getCards()
	{
		int d = 0;
		foreach (int i in cards) 
		{
            if (d < 13)
            {
                yield return i;
            }
            Debug.Log("" + d + " and " + i);
            d++;
        }

	}
	public void shuffle()
	{
		if (cards == null) 
		{
			cards = new List<int> ();
		} 
		else 
		{
			cards.Clear ();
		}
		for (int i = 0; i < 52; i++) 
		{
			cards.Add (i);
		}
		int n = cards.Count;
		while(n>1)
		{
			n--;
			int k = Random.Range (0,n+1);
			int temp = cards [k];
			cards [k] = cards [n];
			cards [n] = temp;
		}
	}
	void Start () 
	{
		shuffle ();
	}

}
