using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;
using System.IO;




public class Validations : MonoBehaviour {
   // Random_Card rc = new Random_Card();
    int grp1_count;
    
    string [] grp1_str;
    //int [] grp1_str;
    Image[] grp1_images;

    

    GameObject go;

    [SerializeField]
    private Text txt = null;
    // Use this for initialization
    void Start () {

        go = GameObject.Find("second");

        grp1_count = go.transform.childCount;

        
    }
	
	// Update is called once per frame
	void Update () {
        grp1_count = go.transform.childCount;
        
        grp1_str = new string[grp1_count + 1];
        //grp1_str = new int[grp1_count + 1];
        grp1_images = go.GetComponentsInChildren<Image>();
        //grp1_images = go.GetComponentsInChildren<Image>();
        for (int i = 1; i < grp1_count+1; i++)
        {
            grp1_str[i] = grp1_images[i].transform.name.ToString();//.sprite.name.ToString();
           // grp1_str[i] = grp1_images[i].sprite.name.ToString();
        }

        ////if (grp1_count > 2)
        ////{ }
        ////else
        ////{
        ////    txt.text = null;
        ////}

        // validations();
        GrpValid();
    }
    List<Random_Card.Card> grp1_list = new List<Random_Card.Card>();
    string result = null;
    
    public void GrpValid()
    {
        grp1_list.Clear();
        //List<Random_Card.Card> grp1_list2 = new List<Random_Card.Card>();

        //grp1_list2 = Random_Card.RummyHand.hand1;
        for (int i = 1; i < grp1_str.Length; i++)
        {
            grp1_list.Add(Random_Card.RummyHand.hand1[Convert.ToInt32(grp1_str[i])]);
        }
        //Calc(0);
        grp1_list.Sort();
        if (grp1_list[0].rank == Random_Card.RANK.Ace && grp1_list[grp1_count - 1].rank == Random_Card.RANK.King)
        {
            Pure();
        }
        else
        {
            IsPure();
        }
        if (result == "None")
        {
            //sample();
            //Set_with_AllJokers();
             Set();
        }
    }

    public void Calc(int _res)
    {
        switch (_res)
        {
            case 0:
                IsPure();
                break;
            case 1:
                Set();
                break;
            case 2:
                Set_with_Joker();
                break;
        }
    }
   

    public void validations()
    {
        string[] str = new string[grp1_count];
        string[] str1_rank = new string[grp1_count];
        string[] str1_suit = new string[grp1_count];
        int grp1_index;

        
        for (int i = 0; i < grp1_count; i++)
        {
            str[i] = grp1_str[i + 1];
            grp1_index = str[i].IndexOf(' ');
            str1_rank[i] = str[i].Substring(0, grp1_index);

            if (str[i].Contains("Spades"))
                str1_suit[i] = "Spades";
            else if (str[i].Contains("Hearts"))
                str1_suit[i] = "Hearts";
            else if (str[i].Contains("Diamonds"))
                str1_suit[i] = "Diamonds";
            else if (str[i].Contains("Clubs"))
                str1_suit[i] = "Clubs";
            else if (str[i].Contains("Joker"))
                str1_suit[i] = "Joker";
           
           
        }


        
        if (grp1_count == 3)
        {
            if ((str1_suit[0] != str1_suit[1]) && (str1_suit[1] != str1_suit[2]) && (str1_suit[2] != str1_suit[0]))
            {
                if ((str1_rank[0] == str1_rank[1]) && (str1_rank[1] == str1_rank[2]))
                {
                    txt.text = "Valid";
                }
                else
                {
                    txt.text = "Invalid";
                }
            }
            else
            {
                txt.text = "Invalid";
            }
        }
        else if(grp1_count == 4)
        {
            if ((str1_suit[0] != str1_suit[1]) && (str1_suit[0] != str1_suit[2]) && (str1_suit[0] != str1_suit[3]) && (str1_suit[1] != str1_suit[2]) && (str1_suit[1] != str1_suit[3]) && (str1_suit[2] != str1_suit[3]))
            {
                if ((str1_rank[0] == str1_rank[1]) && (str1_rank[1] == str1_rank[2]) && (str1_rank[2] == str1_rank[3]))
                {
                    txt.text = "Valid";
                }
                else
                {
                    txt.text = "Invalid";
                }
            }
            else
            {
                txt.text = "Invalid";
            }
        }
        

    }


    public string IsPure()
    {
        try
        {
            int counter = 0;
           
            if (grp1_list.Count >= 3)
            {
                //for (int i = 0; i < grp1_list.Count; i++)
                //{
                    while (counter != grp1_list.Count)
                    {
                        if (grp1_list[counter].suit == grp1_list[++counter].suit)
                        {
                            if (counter == grp1_list.Count - 1)
                            {
                                counter = 0;
                                grp1_list.Sort();

                                while (counter != grp1_list.Count)
                                {
                                    if (grp1_list[counter].rank == grp1_list[++counter].rank - 1)
                                    {
                                        if (counter == grp1_list.Count - 1)
                                        {
                                            txt.text = "Pure Sequence";
                                            result = "Pure Sequence";
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        txt.text = "Impure Sequence";
                                        result = "None";
                                        //Calc(1);
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            txt.text = "Impure Sequence";
                            result = "None";
                            //Calc(1);
                            break;
                        }

                    }
                //    break;
                //}
            }
        }
        catch (Exception ex)
        {
            //txt.text = ex.Message;
        }
        return result;
    }


    public void Set()
    {
        if (grp1_count == 3)
        {
            if ((grp1_list[0].suit != grp1_list[1].suit) && (grp1_list[1].suit != grp1_list[2].suit) && (grp1_list[2].suit != grp1_list[0].suit))
            {
                if ((grp1_list[0].rank == grp1_list[1].rank) && (grp1_list[1].rank == grp1_list[2].rank))
                {
                    txt.text = "Valid Set";
                }
                else
                {
                    txt.text = "Invalid Set";
                    Set_with_Joker();
                    //Calc(2);
                }
            }
            else
            {
                txt.text = "Invalid Set";
                Set_with_Joker();
                //Calc(2);
            }
        }
        else if (grp1_count == 4)
        {
            if ((grp1_list[0].suit != grp1_list[1].suit) && (grp1_list[0].suit != grp1_list[2].suit) && (grp1_list[0].suit != grp1_list[3].suit) && (grp1_list[1].suit != grp1_list[2].suit) && (grp1_list[1].suit != grp1_list[3].suit) && (grp1_list[2].suit != grp1_list[3].suit))
            {
                if ((grp1_list[0].rank == grp1_list[1].rank) && (grp1_list[1].rank == grp1_list[2].rank) && (grp1_list[2].rank == grp1_list[3].rank))
                {
                    txt.text = "Valid Set";
                }
                else
                {
                    txt.text = "Invalid Set";
                    Set_with_Joker();
                    //Calc(2);
                }
            }
            else
            {
                txt.text = "Invalid Set";
                Set_with_Joker();
                //Calc(2);
            }
        }
    }

    public void Set_with_Joker()
    {
        grp1_list.Sort();
        if (grp1_count == 3)
        {
            if ((grp1_list[0].suit != grp1_list[1].suit) && (grp1_list[1].suit != grp1_list[2].suit) && (grp1_list[2].suit != grp1_list[0].suit))
            {
                if (grp1_list[2].rank == Random_Card.RANK.Joker)
                {
                    if ((grp1_list[0].rank == grp1_list[1].rank))
                    {
                        txt.text = "Valid Set";
                    }
                    else
                    {
                        txt.text = "Invalid Set";
                        Set_with_2_Jokers();
                    }
                }
                else
                {
                    txt.text = "Invalid Set";
                    Set_with_2_Jokers();
                }
            }
            else
            {
                txt.text = "Invalid Set";
                Set_with_2_Jokers();
            }
        }
        else if (grp1_count == 4)
        {
            if ((grp1_list[0].suit != grp1_list[1].suit) && (grp1_list[0].suit != grp1_list[2].suit) && (grp1_list[0].suit != grp1_list[3].suit) && (grp1_list[1].suit != grp1_list[2].suit) && (grp1_list[1].suit != grp1_list[3].suit) && (grp1_list[2].suit != grp1_list[3].suit))
            {
                if (grp1_list[3].rank == Random_Card.RANK.Joker)
                {
                    if ((grp1_list[0].rank == grp1_list[1].rank) && (grp1_list[1].rank == grp1_list[2].rank))
                    {
                        txt.text = "Valid Set";
                    }
                    else
                    {
                        txt.text = "Invalid Set";
                        Set_with_2_Jokers();
                    }
                }
                else
                {
                    txt.text = "Invalid Set";
                    Set_with_2_Jokers();
                }
            }
            else
            {
                txt.text = "Invalid Set";
                Set_with_2_Jokers();
            }
        }
    }

    public void Set_with_2_Jokers()
    {
        grp1_list.Sort();
        if (grp1_count == 3)
        {
            if ((grp1_list[1].rank == Random_Card.RANK.Joker) && (grp1_list[2].rank == Random_Card.RANK.Joker))
            {
                txt.text = "Valid Set";
            }
            else
            {
                txt.text = "Invalid Set";
               // Set_with_AllJokers();
            }
        }
        else if (grp1_count == 4)
        {
            if ((grp1_list[0].suit != grp1_list[1].suit))
            {
                if ((grp1_list[2].rank == Random_Card.RANK.Joker) && (grp1_list[3].rank == Random_Card.RANK.Joker))
                {
                    if ((grp1_list[0].rank == grp1_list[1].rank))
                    {
                        txt.text = "Valid Set";
                    }
                    else
                    {
                        txt.text = "Invalid Set";
                        //Set_with_AllJokers();
                    }
                }
                else
                {
                    txt.text = "Invalid Set";
                    //Set_with_AllJokers();
                }
            }
            else
            {
                txt.text = "Invalid Set";
                //Set_with_AllJokers();
            }
        }
    }

    GameObject goJoker;
    public void Set_with_AllJokers()
    {
        Image[] openJoker;
        string opJoker;
        goJoker = GameObject.Find("Joker");
        openJoker = goJoker.GetComponentsInChildren<Image>();
        opJoker = openJoker[0].sprite.name.ToString();

        int index;

        index = opJoker.IndexOf(' ');
        opJoker = opJoker.Substring(0, index);

        for (int i = 0; i < grp1_list.Count; i++)
        {
            if (grp1_list[i].rank.ToString() == opJoker)
            {
                grp1_list[i] = Random_Card.RummyHand.deck.d[52];
            }
        }

        grp1_list.Sort();

        int counter = grp1_count;
        int count;
        for (int i = 0; i < grp1_list.Count; i++)
        {
            while (counter != 0)
            {
                if (grp1_list[--counter].rank == Random_Card.RANK.Joker)
                {

                }
                else if(counter == 0)
                {
                    txt.text = "Valid Set";
                    break;
                }
                else
                {
                    while (counter != -1)
                    {
                        count = counter;
                        if (grp1_list[counter].suit != grp1_list[--counter].suit)
                        {
                            if (grp1_list[count].rank == grp1_list[--count].rank)
                            {
                                if (counter != -1)
                                {
                                    txt.text = "Valid Set";
                                    break;
                                }
                            }
                            else
                            {
                                txt.text = "Invalid Set";
                                break;
                            }
                        }
                        else
                        {
                            txt.text = "Invalid";
                            break;
                        }
                    }
                }
            }
        }
    }

    public void sample()
    {
        grp1_list.Sort();
        int index = 0;
        int _rindex = 1;

        if (grp1_count >= 3)
        {
            while (index <= grp1_count)
            {
                if (grp1_list[grp1_count - (index + 1)].rank == Random_Card.RANK.Joker)
                {
                    index++;
                    _rindex = index;
                }
                else
                {
                    if (grp1_list[grp1_count - (index + 1)].suit != grp1_list[grp1_count - (index + 2)].suit)
                    {
                        index++;
                        if (index == grp1_count - 1)
                        {
                            if (grp1_list[grp1_count - (index + 1)].suit != grp1_list[grp1_count - _rindex].suit)
                            {
                                while(_rindex != 0)
                                {
                                    if (grp1_list[_rindex].rank == grp1_list[--_rindex].rank)
                                    {
                                        if (_rindex == 0)
                                        {
                                            txt.text = "Valid Set";
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        txt.text = "InValid Set";
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                txt.text = "InValid Set";
                                break;
                            }
                        }
                    }
                    else
                    {
                        txt.text = "InValid Set";
                        break;
                    }
                }
            }
        }
    }


    public void Pure()
    {
        int counter = grp1_list.Count - 1;
        grp1_list.Sort();

        while (counter > 0)
        {
            if (grp1_list[0].rank == Random_Card.RANK.Ace)
            {                
                if (grp1_list[grp1_list.Count - 1].rank == Random_Card.RANK.King)
                {
                    if (grp1_list[counter].rank == grp1_list[--counter].rank + 1)
                    {
                        if (counter == 1 && grp1_count == 3)
                        {
                            counter = grp1_list.Count - 1;
                            txt.text = "Pure Sequence";
                            result = "Pure Sequence";
                            break;
                        }
                        else if (counter == 0)
                        {
                            txt.text = "Pure Sequence";
                            result = "Pure Sequence";
                            break;
                        }
                    }
                    else
                    {
                        txt.text = "Impure Sequence";
                        result = "None";
                        //Calc(1);
                        break;
                    }
                }
                else
                {
                    txt.text = "Impure Sequence";
                    result = "None";
                    //Calc(1);
                    break;
                }
            }
            else
            {
                txt.text = "Impure";
                break;
            }
        }
    }


}
