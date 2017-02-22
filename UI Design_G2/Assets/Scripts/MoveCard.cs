using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
//using Random_Card;

public class MoveCard : MonoBehaviour
{

    int i = 0;
    ////public Image cardImage;
    CardFlipper flipper;
    GameObject go, go1;
    // Use this for initialization
    public bool cdClicked = false;
    public bool odClicked = false;
    public Image[] cardImg2 = new Image[13];
    Vector3 discPosition = new Vector3(578, 615, 0);
    Random_Card rc = new Random_Card();
    void Start()
    {
        go1 = GameObject.Find("OPenCardPanel");
        flipper = GetComponent<CardFlipper>();
    }
    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown (0)) {
			flip ();
		}*/

    }
    int remainingCards;

    void flip()
    {
        Random_Card.openCardClicked = true;
        remainingCards = Random_Card.noOfCards - Random_Card.counter;
        if (Random_Card.openCardClicked == true)
        {
            Random_Card.openCardClicked = false;

            if (remainingCards != 0)
            {
                Debug.Log("In Flip()");
                if (cdClicked == false)
                {
                    cdClicked = true;
                    this.transform.Translate(635, -517, 0);
                    Random_Card.RummyHand.hand1.Add(Random_Card.RummyHand.closedDeck[0]);
                    Random_Card.RummyHand.closedDeck.RemoveAt(0);
                }
                else if(odClicked == false)
                {
                    odClicked = true;
                    this.transform.Translate(399, -453.5715f, 0);
                    Random_Card.RummyHand.hand1.Add(Random_Card.RummyHand.openDeck[Random_Card.RummyHand.openDeck.Count - 1]);
                    Random_Card.RummyHand.openDeck.RemoveAt(Random_Card.RummyHand.openDeck.Count - 1);
                }
                //this.transform.Translate(350, -160, 0);
                Debug.Log("Calling Flip.....");
                flipper.flipCard(Resources.Load<Sprite>("Image/1") as Sprite, Resources.Load<Sprite>("Image/" + Random_Card.RummyHand.gameDeck[++Random_Card.counter]) as Sprite);
               
                go1.SetActive(true);
            }
        }

    }
    public void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
        flip();
    }
    
    public void Clear()
    {
        ////Random_Card.RummyHand.openDeck.Add(Random_Card.RummyHand.hand1[Random_Card.RummyHand.hand1.Count - 1]);
        ////Random_Card.RummyHand.hand1.RemoveAt(Random_Card.RummyHand.hand1.Count - 1);
        ////Random_Card.openCardClicked = true;
        ////Vector3 translate = discPosition;
        ////GameObject.Find("ClosedDeck").GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/" + Random_Card.RummyHand.gameDeck[Random_Card.counter]) as Sprite;
        ////go = GameObject.Find("ClosedDeck");
        //////go.transform.position = translate;
        ////go.transform.position = translate;

        ////go1.SetActive(false);
               
       
    }

    public void Grouping()
    {
        int spades = 0;
        int hearts = 0;
        int diamonds = 0;
        int clubs = 0;

        List<Random_Card.Card> newHand = new List<Random_Card.Card>();
        newHand = Random_Card.RummyHand.hand1;
        rc.Sort(newHand);
        for (int i = 0; i < newHand.Count; i++)
        {
            if(newHand[i].suit.ToString() == "Spades")
            {
                spades++;
            }
            else if (newHand[i].suit.ToString() == "Hearts")
            {
                hearts++;
            }
            else if (newHand[i].suit.ToString() == "Diamonds")
            {
                diamonds++;
            }
            else if (newHand[i].suit.ToString() == "Clubs")
            {
                clubs++;
            }
        }
        if (spades > 0)
        {
            GameObject panel = new GameObject("spadePanel");
            panel.AddComponent<CanvasRenderer>();
            Image im = panel.AddComponent<Image>();
            im.sprite= Resources.Load<Sprite>("Image/" + newHand[0]) as Sprite;
            go = GameObject.Find("Canvas");
            panel.transform.SetParent(go.transform, false);
        }
        /*if (hearts > 0)
        {
            GameObject panel = new GameObject("heartPanel");
            panel.AddComponent<CanvasRenderer>();
            Image im = panel.AddComponent<Image>();
            im.color = Color.red;
            go = GameObject.Find("Canvas");
            panel.transform.SetParent(go.transform, false);
        }
        if (diamonds > 0)
        {
            GameObject panel = new GameObject("diamondPanel");
            panel.AddComponent<CanvasRenderer>();
            Image im = panel.AddComponent<Image>();
            im.color = Color.red;
            go = GameObject.Find("Canvas");
            panel.transform.SetParent(go.transform, false);
        }
        if (clubs > 0)
        {
            GameObject panel = new GameObject("clubPanel");
            panel.AddComponent<CanvasRenderer>();
            Image im = panel.AddComponent<Image>();
            im.color = Color.red;
            go = GameObject.Find("Canvas");
            panel.transform.SetParent(go.transform, false);
        }*/
    }
   
    public void Sort(List<Random_Card.Card> hand)
    {
        int counter = 0;
        List<Random_Card.Card> newHand = new List<Random_Card.Card>();
        Random_Card.Card[,] bucketCards = new Random_Card.Card[5, 14];
        for (int i = 0; i < hand.Count; i++)
        {
            if (bucketCards[Convert.ToInt32(hand[i].suit), Convert.ToInt32(hand[i].rank)] != null)
            {
                newHand.Add(hand[i]);
                counter++;
            }
            else
            {
                bucketCards[Convert.ToInt32(hand[i].suit), Convert.ToInt32(hand[i].rank)] = hand[i];
            }
        }

        int pointer = 0;
        string str1, str2;
        int incr = 0;
        sampleSort(newHand);
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 14; j++)
            {
                if (bucketCards[i, j] != null)
                {
                    str1 = bucketCards[i, j].ToString();
                    if (newHand.Count > 0)
                    {
                        if (incr < newHand.Count)
                        {
                            if (newHand[incr] != null)
                            {
                                str2 = newHand[incr].ToString();
                            }
                            else
                            {
                                str2 = null;
                            }
                            if (str1 == str2)//bucketCards[i,j] == newHand[incr])
                            {
                                incr++;
                                hand[pointer] = bucketCards[i, j];
                                pointer++;
                                hand[pointer] = bucketCards[i, j];
                                pointer++;
                            }
                            else
                            {
                                pointer++;
                                hand[--pointer] = bucketCards[i, j];
                                pointer++;
                            }
                        }
                        else
                        {
                            pointer++;
                            hand[--pointer] = bucketCards[i, j];
                            pointer++;
                        }
                    }
                    else
                    {
                        hand[pointer] = bucketCards[i, j];
                        pointer++;
                    }

                }
            }
        }
        Random_Card.RummyHand.hand1 = hand;
    }
    public void sampleSort(List<Random_Card.Card> newHand)
    {

        Random_Card.Card[,] bucket = new Random_Card.Card[5, 14];
        int pointer = 0;
        for (int i = 0; i < newHand.Count; i++)
        {
            if (newHand[i] != null)
                bucket[Convert.ToInt32(newHand[i].suit), Convert.ToInt32(newHand[i].rank)] = newHand[i];

        }
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 14; j++)
            {
                if (bucket[i, j] != null)
                {
                    newHand[pointer] = bucket[i, j];
                    pointer++;

                }
            }
        }
    }
 
    public void sorting()
    {
        Sort(Random_Card.RummyHand.hand1);
        //Destroy(this.gameObject);
        //for (int i = 0; i < Random_Card.handSize; i++)
        //{
        //    rc.cardImg[i].sprite = Resources.Load<Sprite>("Image/" + Random_Card.RummyHand.hand1[i]) as Sprite;
        //}
        rc.showSortedCards();
    }
}
