﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
	public GameObject grabbyHand;

	public GameObject[] cards = new GameObject[20];

	public GameObject[] graveyard = new GameObject[20];

	public GameObject[] usingCards;

	public GameObject[] hand;

	public bool GraveShifted;

	void Start()
	{
		GenerateNewDeck(cards.Length);
		GenerateHand();
	}

	public void GenerateNewDeck(int length)
	{
		string ass = PlayerPrefs.GetString("Deck");

		string[] split = ass.Split(","[0]);

		usingCards = new GameObject[length];

		//cycle 20-21
		for (int i = 0; i < split.Length; i++)
		{
			if (split[i] != "")
			{
				Object[] loadedCards = Resources.LoadAll("Cards/", typeof(GameObject));
				for (int x = 0; x < loadedCards.Length; x++)
				{
					GameObject boi = (GameObject)loadedCards[x];
					if (boi.GetComponent<CardLogic>().ID.ToString() == split[i])
					{
						print("Ree" + boi.GetComponent<CardLogic>().ID.ToString() + "," + split[i]);
						usingCards[i] = boi;
					}
				}
			}
		}

		//usingCards = new GameObject[length];

		/*for(int x = 0; x < cards.Length; x++)
		{
			usingCards[x] = cards[x];
		}*/
	}

	public void GenerateHand()
	{
		hand = new GameObject[6];
	}

	public void MoveGrave()
	{
		print("moved the graveyard!");

		usingCards = new GameObject[graveyard.Length];

		for (int x = 0; x < graveyard.Length; x++)
		{
			usingCards[x] = graveyard[x];
		}

		graveyard = new GameObject[20];

		GraveShifted = true;

		DrawCard();
	}

	public void DrawCard()
	{
		if(usingCards.Length <= 0 || usingCards[0] == null)
		{
			MoveGrave();
			return;
		}

		//select a card
		int draw = Random.Range(0, usingCards.Length);
		int graveSelect = -1;

		//cycle through hand
		for(int x = 0; x < hand.Length; x++)
		{
			//find next empty
			if(hand[x] == null)
			{
				if (GraveShifted)
				{
					//add next
					for (int i = 0; i < usingCards.Length; i++)
					{
						if (usingCards[i] != null)
						{
							hand[x] = usingCards[i];
							graveSelect = i;
							break;
						}
					}
				}
				else
				{
					//add card to hand
					hand[x] = usingCards[draw];
				}

				//spawn object in hand
				GameObject card = Instantiate(hand[x]);
				card.transform.SetParent(grabbyHand.transform, false);

				card.GetComponent<CardLogic>().ID = x;

				//clear the card
				if (GraveShifted)
				{
					usingCards[graveSelect] = null;
				}
				else
				{
					usingCards[draw] = null;
				}

				//generate a temp deck thats 1 length shorter
				GameObject[] tempDeck = new GameObject[usingCards.Length - 1];

				//cycle through all usingCards
				for(int y = 0; y < usingCards.Length; y++)
				{
					//if not null transfer
					if(usingCards[y] != null)
					{
						//find next empty in tempDeck
						for(int z = 0; z < tempDeck.Length; z++)
						{
							if(tempDeck[z] == null)
							{
								tempDeck[z] = usingCards[y];
								break;
							}
						}
					}
				}

				usingCards = tempDeck;

				return;
			}
		}
	}

	public int HandSize()
	{
		int size = 0;

		for(int x = 0; x < hand.Length; x++)
		{
			if(hand[x] != null)
			{
				size++;
			}
		}

		return size;
	}
}