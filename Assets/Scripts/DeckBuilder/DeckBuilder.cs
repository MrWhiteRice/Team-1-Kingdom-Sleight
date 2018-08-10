using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuilder : MonoBehaviour
{
	public CardTile[] cards;
	public string deck;
	public string[] splitboi;
	public GameObject display;

	bool selected;
	bool grabbed;

	public GameObject hoveredObject;
	public GameObject selectedCard;

	public void Start()
	{
		cards = GameObject.FindObjectsOfType<CardTile>();

		deck = PlayerPrefs.GetString("Deck", "0,0,0,0" +
											",1,1,1,1" +
											",2,2,2,2" +
											",3,3,3,3" +
											",4,4,4,4");

		splitboi = deck.Split(","[0]);
		for (int x = 0; x < splitboi.Length; x++)
		{
			if(splitboi[x] != "")
			{
				cards[x].id = int.Parse(splitboi[x]);
			}
		}
	}

	public void Update()
	{
		if (selected)
		{
			display.GetComponent<RectTransform>().position = Input.mousePosition;
		}

		if (grabbed)
		{
			display.GetComponent<RectTransform>().position = Input.mousePosition;
		}
	}

	public void MouseGrab(CardButton obj)
	{
		hoveredObject = obj.gameObject;
		if (Input.GetMouseButton(0))
		{
			Object[] loadedCards = Resources.LoadAll("Cards/", typeof(GameObject));

			for (int x = 0; x < loadedCards.Length; x++)
			{
				GameObject selectedCard = (GameObject)loadedCards[x];
				if (selectedCard.GetComponent<CardLogic>().ID == obj.id)
				{
					print(obj.id);
					grabbed = true;
					Sprite s = selectedCard.GetComponent<CardLogic>().cardImage.sprite;
					display.GetComponent<Image>().sprite = s;
				}
			}
		}
	}

	public void MouseRelease()
	{
		if (selectedCard != null)
		{
			selectedCard.GetComponent<Image>().sprite = display.GetComponent<Image>().sprite;
			selectedCard.GetComponent<CardTile>().id = hoveredObject.GetComponent<CardButton>().id;
		}

		grabbed = false;
		hoveredObject = null;
		display.GetComponent<RectTransform>().position = new Vector2(-10000, -10000);
	}

	public void MouseHover(CardButton boi)
	{
		Object[] loadedCards = Resources.LoadAll("Cards/", typeof(GameObject));

		for (int x = 0; x < loadedCards.Length; x++)
		{
			GameObject selectedCard = (GameObject)loadedCards[x];
			if (selectedCard.GetComponent<CardLogic>().ID == boi.id)
			{
				if (!grabbed)
				{
					selected = true;
					Sprite s = selectedCard.GetComponent<CardLogic>().cardImage.sprite;
					display.GetComponent<Image>().sprite = s;
					return;
				}
			}
		}
	}

	public void SaveDeck()
	{
		string save = "";

		for (int x = 0; x < cards.Length; x++)
		{
			save += cards[x].id + ",";
		}

		print("saving deck with: " + save);
		PlayerPrefs.SetString("Deck", save);
	}

	public void MouseExit()
	{
		selected = false;
		display.GetComponent<RectTransform>().position = new Vector2(-10000, -10000);
	}
}