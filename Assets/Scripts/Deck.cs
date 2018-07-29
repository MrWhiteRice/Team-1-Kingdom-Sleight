using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
	public GameObject[] cards = new GameObject[20];

	public GameObject[] usingCards;

	public GameObject[] hand = new GameObject[6];

	void Start()
	{
		GenerateNewDeck();
		GenerateHand();
	}

	public void GenerateNewDeck()
	{
		usingCards = cards;
	}

	public void GenerateHand()
	{
		for(int x = 0; x < hand.Length; x++)
		{
			DrawCard();
		}
	}

	public void DrawCard()
	{
		int draw = Random.Range(0, cards.Length);

		GameObject[] tempDeck = usingCards;
		//make temp deck and do the thing

		print("card Drawn: " + cards[draw].name);

		Object.Destroy(
	}
}