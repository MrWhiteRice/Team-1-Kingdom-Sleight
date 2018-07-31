using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
	public GameObject grabbyHand;

	public GameObject[] cards = new GameObject[20];

	public GameObject[] usingCards;

	public GameObject[] hand;

	void Start()
	{
		GenerateNewDeck(cards.Length);
		GenerateHand();
	}

	public void GenerateNewDeck(int length)
	{
		usingCards = new GameObject[length];

		for(int x = 0; x < cards.Length; x++)
		{
			usingCards[x] = cards[x];
		}
	}

	public void GenerateHand()
	{
		hand = new GameObject[6];

		for(int x = 0; x < 3; x++)
		{
			DrawCard();
		}
	}

	public void DrawCard()
	{
		//select a card
		int draw = Random.Range(0, usingCards.Length);

		//cycle through hand
		for(int x = 0; x < hand.Length; x++)
		{
			//find next empty
			if(hand[x] == null)
			{
				//add card to hand
				hand[x] = usingCards[draw];

				//spawn object in hand
				GameObject card = Instantiate(hand[x]);
				card.transform.SetParent(grabbyHand.transform);

				card.GetComponent<CardLogic>().ID = x;

				//clear the card
				usingCards[draw] = null;

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
}