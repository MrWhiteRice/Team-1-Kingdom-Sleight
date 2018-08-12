using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public int id;

	public Object[] cards;

	void Start()
	{
		cards = Resources.LoadAll("Cards/", typeof(GameObject));
	}

	void Update()
	{
		for (int x = 0; x < cards.Length; x++)
		{
			GameObject boi = (GameObject)cards[x];
			if (boi.GetComponent<CardLogic>().ID == id)
			{
				GetComponentInChildren<Image>().sprite = boi.GetComponent<CardLogic>().cardImage.sprite;
				return;
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		GameObject.FindObjectOfType<DeckBuilder>().selectedCard = gameObject;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GameObject.FindObjectOfType<DeckBuilder>().selectedCard = null;
	}
}