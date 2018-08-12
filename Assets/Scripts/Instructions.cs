using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instructions : MonoBehaviour
{
	public Button prev;
	public Button next;

	public Image show;

	public Sprite[] pics;

	public int place = 0;

	void Update()
	{
		show.sprite = pics[place];

		if (place == pics.Length-1)
		{
			next.interactable = false;
		}
		else
		{
			next.interactable = true;
		}

		if (place == 0)
		{
			prev.interactable = false;
		}
		else
		{
			prev.interactable = true;
		}
	}

	public void Next()
	{
		if (place < pics.Length)
		{
			place++;
		}
	}

	public void Previous()
	{
		if (place > 0)
		{
			place--;
		}
	}
}
