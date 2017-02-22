using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;
using System.IO;

[RequireComponent(typeof(MainDeck))]
public class Random_Card : MonoBehaviour {

    public static int handSize = 13;
    public static int noOfPlayers;
    public static int deckSize = 53;
    public static int noOfDecks;
    public static int noOfCards;


    public Image[] cardImg;
    public Image[] cardImg1;

    public Image openCard, jokerCard;

    #region for randomnumbergeneration
    public static int[] randomvalues;
    #endregion

    #region RandomNumberAlgorithm
    public class ISAAC
    {
        public static void Assert(bool condition)
        {
            if (!condition) throw new System.Exception("Assertion failed");
        }
        public ISAAC(byte[] seed, bool flag = true)
        {
            var t = new uint[8];
            for (var i = 0; i < 8; i++) t[i] = 0x9E3779B9;
            for (var i = 0; i < 4; i++) mix(t);
            for (var i = 0; i < 256; i++)
            {
                mm[i] = 0;
                data[i] = i < seed.Length ? (uint)seed[i] : 0U;
            }
            for (var p = 0; p < 2; p++)
                for (var i = 0; i < 256; i += 8)
                {
                    if (p == 0)
                    {
                        if (flag) for (var n = 0; n < 8; n++) t[n] += data[i + n];
                        mix(t);
                        for (var n = 0; n < 8; n++) mm[i + n] = t[n];
                        continue;
                    }
                    if (!flag) break;
                    for (var n = 0; n < 8; n++) t[n] += mm[i + n];
                    mix(t);
                    for (var n = 0; n < 8; n++) mm[i + n] = t[n];
                }
            isaac();
        }

        public int Random(int domain = 0)
        {
            //log.Info("Cards Randomization");
            Assert(domain >= 0);
            if (domain == 0)
            {
                var x = data[size];
                size = (size + 1) & 255;
                if (size == 0) isaac();
                return (int)x;
            }
            if (domain == 1) return 0;
            var t = 0;
            for (var i = 0; i < 8; i++)
            {
                var x = (int)(data[size] % domain);
                size = (size + 1) & 255;
                if (size == 0) isaac();
                t = (t + x) % domain;
            }
            return t;
        }

        static void mix(uint[] data)
        {
            data[0] ^= data[1] << 11;
            data[3] += data[0];
            data[1] += data[2];
            data[1] ^= data[2] >> 2;
            data[4] += data[1];
            data[2] += data[3];
            data[2] ^= data[3] << 8;
            data[5] += data[2];
            data[3] += data[4];
            data[3] ^= data[4] >> 16;
            data[6] += data[3];
            data[4] += data[5];
            data[4] ^= data[5] << 10;
            data[7] += data[4];
            data[5] += data[6];
            data[5] ^= data[6] >> 4;
            data[0] += data[5];
            data[6] += data[7];
            data[6] ^= data[7] << 8;
            data[1] += data[6];
            data[7] += data[0];
            data[7] ^= data[0] >> 9;
            data[2] += data[7];
            data[0] += data[1];
        }

        void isaac()
        {
            cc++;
            bb += cc;
            for (var i = 0; i < 256; i++)
            {
                var s = i & 3;
                aa ^= s == 3 ? aa >> 16 : s == 2 ? aa << 2 : s == 1 ? aa >> 6 : aa << 13;
                aa += mm[(i + 128) & 255];
                var x = mm[i];
                var y = mm[(x >> 2) & 255] + aa + bb;
                mm[i] = y;
                bb = mm[(y >> 10) & 255] + x;
                data[i] = bb;
            }
        }

        uint[] data = new uint[256];
        uint size = 0;
        uint[] mm = new uint[256];
        uint aa = 0;
        uint bb = 0;
        uint cc = 0;
    }
    #endregion

    #region PokerClassFilesandFunction
    public enum SUIT { Spades, Hearts, Diamonds, Clubs, Joker }
    public enum RANK
    {
        Ace, Two, Three, Four, Five, Six, Seven, Eight,
        Nine, Ten, Jack, Queen, King, Joker
    }
   
    public class Card : IComparable
    {
        public RANK _rank;
        public SUIT _suit;

        // IComparable interface method
        public int CompareTo(object o)
        {
            if (o is Card)
            {
                Card c = (Card)o;
                if (_rank < c.rank)
                    return -1;
                else if (_rank > c.rank)
                    return 1;
                return 0;
            }
            throw new ArgumentException("Object is not a Card");
        }

        public Card(RANK rank, SUIT suit)
        {
            this._rank = rank;
            this._suit = suit;
        }
        

        public override string ToString()
        {
            return this._rank + " " + this._suit;
        }

        public RANK rank
        {
            get { return _rank; }
        }

        public SUIT suit
        {
            get { return _suit; }
        }

       
    }


    public class MainDeck
    {
        // array of Card of object (the real deck)
        public Card[] d;
        // current card index
        public int cc = 0;
        // shuffle variable
        private System.Random rand = new System.Random();

        public MainDeck()
        {
            init();

        }

        private void init()
        {
            cc = 0;
            noOfCards = deckSize * noOfDecks;
            d = new Card[noOfCards];
            int counter = 0;

            for (int i = 0; i < noOfDecks; i++)
            {
                foreach (SUIT s in Enum.GetValues(typeof(SUIT)))
                    foreach (RANK r in Enum.GetValues(typeof(RANK)))

                        if (r.ToString() == "Joker" && s.ToString() == "Joker")
                        {
                            d[counter++] = new Card(r, s);
                        }
                        else if (r.ToString() == "Joker" || s.ToString() == "Joker")
                        {
                        }
                        else
                        {
                            d[counter++] = new Card(r, s);
                        }

            }
        }
        bool rndmflag = false;
        public Card pullCard(int cardset = 0)
        {
            if (cc == noOfCards)//noOfPlayers * handSize)//)
            {
                cc = 0;
                rndmflag = false;
            }
            var seed = new byte[256];
            var crypto = new System.Security.Cryptography.RNGCryptoServiceProvider();
            crypto.GetBytes(seed);
            //RummyHand.crypto.GetBytes(RummyHand.seed);
            // now initialize our ISAAC with the seed
            var random = new ISAAC(seed);
            // get random card (from a deck of 52 cards)
            int randomCard = random.Random(noOfCards);
            //return d[cc++];
            
            switch (cardset)
            {
                case 0:                   
                                       
                    while (randomvalues.Contains(randomCard))
                    {
                        if(randomCard == 0 && rndmflag == false)
                        {
                            rndmflag = true;
                            break;
                        }
                        else
                        {
                            randomCard = random.Random(noOfCards);
                        }
                        
                    }
                    randomvalues[cc] = randomCard;
                    break;
            }
            cc++;
            return d[randomCard];
        }
        
        public Card peekCard()
        {
            return d[cc];
        }

        private void swapCards(int i, int j)
        {
            Card tmp = d[i];
            d[i] = d[j];
            d[j] = tmp;
        }

        /*
         * shuffle the deck and reset the current card
         * index to the beginning
         */
        public void shuffle(int count)
        {
        }

        /*
         * 10 is overkill, 8 should be enough
         */
        public void shuffle()
        {
            // this.shuffle(1);
        }

        public void print()
        {
            foreach (Card c in d)
                Console.WriteLine(c);
        }
    }
    public class RummyHand
    {
        
        public static MainDeck deck;
        //public static Card[] gameDeck;
        //public static Card[] hand1;
        //public static Card[] hand2;
        //public static Card[] hand3;
        //public static Card[] hand4;
        //public static Card[] hand5;
        //public static Card[] hand6;

        public static List<Card> gameDeck;
        public static List<Card> hand1;
        public static List<Card> hand2;
        public static List<Card> hand3;
        public static List<Card> hand4;
        public static List<Card> hand5;
        public static List<Card> hand6;
        public static List<Card> closedDeck;
        public static List<Card> openDeck;

        public RummyHand()
        {

        }
        public RummyHand(MainDeck deck)
        {
            RummyHand.deck = deck;
            //RummyHand.gameDeck = new Card[noOfCards];
            //RummyHand.hand1 = new Card[handSize];
            //RummyHand.hand2 = new Card[handSize];
            //RummyHand.hand3 = new Card[handSize];
            //RummyHand.hand4 = new Card[handSize];
            //RummyHand.hand5 = new Card[handSize];
            //RummyHand.hand6 = new Card[handSize];

            RummyHand.gameDeck = new List<Card>();//[noOfCards];
            RummyHand.hand1 = new List<Card>();
            RummyHand.hand2 = new List<Card>();
            RummyHand.hand3 = new List<Card>();
            RummyHand.hand4 = new List<Card>();
            RummyHand.hand5 = new List<Card>();
            RummyHand.hand6 = new List<Card>();
            RummyHand.closedDeck = new List<Card>();
            RummyHand.openDeck = new List<Card>();

            randomvalues = new int[noOfCards];
        }

        public void pullCards(int x = 0)
        {
            
            for (int i = 0; i < noOfCards; i++)
            {
                if (i > noOfCards - 1)
                {
                    x++;
                    i = 0;
                    if (x >= 6)
                    {
                        break;
                    }
                }
                switch (x)
                {
                    case 0:
                        //gameDeck[i] = deck.pullCard();
                        gameDeck.Add(deck.pullCard());
                        //Random_Card.SampleWrittingIntoTextFile(gameDeck[i].ToString());
                        break;
                }
            }
            

        }

        public void AssignHands()
        {
            switch (noOfPlayers)
            {
                case 2:
                    for (int i = 0; i < handSize; i++)
                    {
                        //hand1[i] = gameDeck[noOfPlayers * i];
                        //hand2[i] = gameDeck[(noOfPlayers * i) + 1];
                        hand1.Add(gameDeck[noOfPlayers * i]);
                        hand2.Add(gameDeck[(noOfPlayers * i) + 1]);

                        /*hand1.Add(deck.d[39]);
                        hand1.Add(deck.d[48]);
                        hand1.Add(deck.d[50]);
                        hand1.Add(deck.d[51]);
                        hand1.Add(deck.d[52]);
                        hand1.Add(deck.d[52]);
                        hand1.Add(deck.d[1]);
                        hand1.Add(deck.d[14]);
                        hand1.Add(deck.d[27]);
                        hand1.Add(deck.d[40]);
                        hand1.Add(deck.d[1]);
                        hand1.Add(deck.d[14]);
                        hand1.Add(deck.d[28]);*/

                    }
                    break;
                case 3:
                    for (int i = 0; i < handSize; i++)
                    {
                        //hand1[i] = gameDeck[noOfPlayers * i];
                        //hand2[i] = gameDeck[(noOfPlayers * i) + 1];
                        //hand3[i] = gameDeck[(noOfPlayers * i) + 2];
                        hand1.Add(gameDeck[noOfPlayers * i]);
                        hand2.Add(gameDeck[(noOfPlayers * i) + 1]);
                        hand3.Add(gameDeck[(noOfPlayers * i) + 2]);
                    }
                    break;
                case 4:
                    for (int i = 0; i < handSize; i++)
                    {
                        //hand1[i] = gameDeck[noOfPlayers * i];
                        //hand2[i] = gameDeck[(noOfPlayers * i) + 1];
                        //hand3[i] = gameDeck[(noOfPlayers * i) + 2];
                        //hand4[i] = gameDeck[(noOfPlayers * i) + 3];
                        hand1.Add(gameDeck[noOfPlayers * i]);
                        hand2.Add(gameDeck[(noOfPlayers * i) + 1]);
                        hand3.Add(gameDeck[(noOfPlayers * i) + 2]);
                        hand4.Add(gameDeck[(noOfPlayers * i) + 3]);
                    }
                    break;
                case 5:
                    for (int i = 0; i < handSize; i++)
                    {
                        //hand1[i] = gameDeck[noOfPlayers * i];
                        //hand2[i] = gameDeck[(noOfPlayers * i) + 1];
                        //hand3[i] = gameDeck[(noOfPlayers * i) + 2];
                        //hand4[i] = gameDeck[(noOfPlayers * i) + 3];
                        //hand5[i] = gameDeck[(noOfPlayers * i) + 4];
                        hand1.Add(gameDeck[noOfPlayers * i]);
                        hand2.Add(gameDeck[(noOfPlayers * i) + 1]);
                        hand3.Add(gameDeck[(noOfPlayers * i) + 2]);
                        hand4.Add(gameDeck[(noOfPlayers * i) + 3]);
                        hand5.Add(gameDeck[(noOfPlayers * i) + 4]);
                    }
                    break;
                case 6:
                    for (int i = 0; i < handSize; i++)
                    {
                        //hand1[i] = gameDeck[noOfPlayers * i];
                        //hand2[i] = gameDeck[(noOfPlayers * i) + 1];
                        //hand3[i] = gameDeck[(noOfPlayers * i) + 2];
                        //hand4[i] = gameDeck[(noOfPlayers * i) + 3];
                        //hand5[i] = gameDeck[(noOfPlayers * i) + 4];
                        //hand6[i] = gameDeck[(noOfPlayers * i) + 5];
                        hand1.Add(gameDeck[noOfPlayers * i]);
                        hand2.Add(gameDeck[(noOfPlayers * i) + 1]);
                        hand3.Add(gameDeck[(noOfPlayers * i) + 2]);
                        hand4.Add(gameDeck[(noOfPlayers * i) + 3]);
                        hand5.Add(gameDeck[(noOfPlayers * i) + 4]);
                        hand6.Add(gameDeck[(noOfPlayers * i) + 5]);
                    }
                    break;

            }
            counter = noOfPlayers * handSize;
        }

        public void AssignClosedDeck()
        {
            for (int i = counter; i < gameDeck.Count; i++)
            {
                closedDeck.Add(gameDeck[i]);
            }
        }

    }
    //static int discardIndex = 0;
    public static void DrawNextCard(int _index, int discardIndex)
        {

        //var seed = new byte[256];
        //var crypto = new System.Security.Cryptography.RNGCryptoServiceProvider();
        //crypto.GetBytes(seed);
        //// now initialize our ISAAC with the seed
        //var random = new ISAAC(seed);
        //// get random card (from a deck of 52 cards)
        //var randomCard = random.Random(53);
        //hand[discardIndex] = hand[_index];
        //hand[1] = hand[5];
        //hand = hand;
        }


        //public static void RemoceCard(int index )
        //{
        //    discardIndex = Array.IndexOf(hand, deck.d[index]);
        //    Array.Clear(RummyHand.hand, discardIndex, 1);               
        //}

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
        foreach (Card c in RummyHand.gameDeck)
        {
            sb.Append(c);
            sb.Append(", ");
        }
        return sb.ToString();
        }

        public Card this[int index]
        {
            get
            {
            return RummyHand.gameDeck[index];
            }
        }
        public static void WinningSort()
        {
            //Array.Sort(hand, 0, 5);
            //if (hand[4].ToString().Contains("Joker"))
            //{
            //    hand[4] = hand[3];
            //}
        }
       
    #endregion

    public static RummyHand gameHand;
    public static MainDeck deck;
    public static int counter = 0;
    public static bool openCardClicked = false;

    
    // Use this for initialization
    void Start()
    {

        noOfPlayers = 2;
        if (noOfPlayers > 5)
            noOfDecks = 3;
        else
            noOfDecks = 2;
        deck = new MainDeck();
        gameHand = new RummyHand(deck);
        gameHand.pullCards();
        gameHand.AssignHands();
        showCards();
        gameHand.AssignClosedDeck();
        WrittingIntoTextFile();
    }
    GameObject go;
    void showCards()
    {
        
        //Sort(RummyHand.hand1);
        for (int i = 0; i < handSize; i++)
        {
            cardImg[i].sprite= Resources.Load<Sprite>("Image/" + RummyHand.hand1[i]) as Sprite;
        }
        jokerCard.sprite = Resources.Load<Sprite>("Image/" + RummyHand.gameDeck[counter++]) as Sprite;
        RummyHand.openDeck.Add(RummyHand.gameDeck[counter++]);
        openCard.sprite = Resources.Load<Sprite>("Image/" + RummyHand.openDeck[0]) as Sprite;
        

    }

    public void NewCards() // to display new cards in hand
    {
        noOfPlayers = 2;
        if (noOfPlayers > 5)
            noOfDecks = 3;
        else
            noOfDecks = 2;
        deck = new MainDeck();
        gameHand = new RummyHand(deck);
        gameHand.pullCards();
        gameHand.AssignHands();
        showCards();
        gameHand.AssignClosedDeck();
        WrittingIntoTextFile();
    }
   
    #region
    /* public void Sort(List<Card> hand)
     {
         int counter = 0;
         List<Card> newHand = new List<Card>();
         Card[,] bucketCards = new Card[5, 14];
         for (int i = 0; i < hand.Count; i++)
         {
             if (bucketCards[Convert.ToInt32(hand[i].suit), Convert.ToInt32(hand[i].rank)] != null)
             {
                 newHand[counter] = hand[i];
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
         //for (int i = 0; i < 5; i++)
         //{
         //    for (int j = 0; j < 14; j++)
         //    {
         //        if (bucketCards[i, j] != null)
         //        {
         //            str1 = bucketCards[i, j].ToString();

         //            if (newHand[incr] != null)
         //            {
         //                str2 = newHand[incr].ToString();
         //            }
         //            else
         //            {
         //                str2 = null;
         //            }
         //            if (str1 == str2)//bucketCards[i,j] == newHand[incr])
         //            {
         //                incr++;
         //                hand[pointer] = bucketCards[i, j];
         //                pointer++;
         //                hand[pointer] = bucketCards[i, j];
         //                pointer++;
         //            }
         //            else
         //            {
         //                hand[pointer] = bucketCards[i, j];
         //                pointer++;
         //            }

         //        }
         //    }
         //}
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
         RummyHand.hand1 = hand;
     }
     public void sampleSort(List<Card> newHand)
     {

         Card[,] bucket = new Card[5, 14];
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
     }*/
    #endregion

    public void Sort(List<Card> hand)
    {

        int counter = 0;
        List<Card> newHand = new List<Card>();
        Card[,] bucketCards = new Card[5, 14];
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
        RummyHand.hand1 = hand;
    }
    public void sampleSort(List<Card> newHand)
    {

        Card[,] bucket = new Card[5, 14];
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
    public void WrittingIntoTextFile()
    {
        // Set a variable to the My Documents path.
        string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // Append text to an existing file named "WriteLines.txt".
        using (StreamWriter outputFile = new StreamWriter(mydocpath + @"\Randomized_2_deck_of_Cards.txt"))//, true)) set -> true to append text
        {
            outputFile.WriteLine("Randomized 2 deck of Cards");
            outputFile.WriteLine("============================");
            for (int i = 0; i < RummyHand.gameDeck.Count; i++)
            {
                outputFile.WriteLine(""+i.ToString()+"."+RummyHand.gameDeck[i]);
            }
        }

        using (StreamWriter outputFile = new StreamWriter(mydocpath + @"\Player1_Cards.txt"))//, true)) set -> true to append text
        {
            outputFile.WriteLine("Player1 Cards");
            outputFile.WriteLine("================");
            for (int i = 0; i < RummyHand.hand1.Count; i++)
            {
                outputFile.WriteLine(RummyHand.hand1[i]);
            }
        }

        using (StreamWriter outputFile = new StreamWriter(mydocpath + @"\Player2_Cards.txt"))//, true)) set -> true to append text
        {
            outputFile.WriteLine("Player2 Cards");
            outputFile.WriteLine("================");
            for (int i = 0; i < RummyHand.hand2.Count; i++)
            {
                outputFile.WriteLine(RummyHand.hand2[i]);
            }
        }

        using (StreamWriter outputFile = new StreamWriter(mydocpath + @"\Remaining_Cards.txt"))//, true)) -> set true to append text
        {
            outputFile.WriteLine("Remaining Cards");
            outputFile.WriteLine("================");
            int count = noOfPlayers * handSize;
            outputFile.WriteLine("Joker: " + RummyHand.gameDeck[count++]);
            outputFile.WriteLine("Open Card: " + RummyHand.gameDeck[count++]);
            for (int i = count; i < RummyHand.gameDeck.Count; i++)
            {
                outputFile.WriteLine(RummyHand.gameDeck[i]);
            }
        }
    }
    static int j = 0;
    public static void SampleWrittingIntoTextFile(string i)
    {
        // Set a variable to the My Documents path.
        string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // Append text to an existing file named "WriteLines.txt".
        using (StreamWriter outputFile = new StreamWriter(mydocpath + @"\sample_Cards.txt"))//, true))// set -> true to append text
        {
            outputFile.WriteLine(Random_Card.j+". "+i);
            Random_Card.j++;
        }
    }
    public static void SampleWrittingIntoTextFile2(int i)
    {
        // Set a variable to the My Documents path.
        string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // Append text to an existing file named "WriteLines.txt".
        using (StreamWriter outputFile = new StreamWriter(mydocpath + @"\sample_Cards2.txt"))//, true))// set -> true to append text
        {
            outputFile.WriteLine((Random_Card.j/2) + ". " + i.ToString());
            Random_Card.j++;
        }
    }

    
    public void showSortedCards()
    {
        Sort(RummyHand.hand1);
        
         
        go = GameObject.Find("second");
        
     
        for (int i = 0; i < handSize; i++)
        {
            cardImg[i].sprite = Resources.Load<Sprite>("Image/" + RummyHand.hand1[i]) as Sprite;
        }

        
       
    }
}
