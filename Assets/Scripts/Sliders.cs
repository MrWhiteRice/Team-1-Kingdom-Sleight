using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sliders : MonoBehaviour {

    public Slider mana;
    public Slider card;

    public float cardGen = 5.0f;
    public float manaGen = 5.0f;

    public float cardRate = 2.5f;
    public float manaRate = 5f;

    float cardCount;
    float manaCount;

    float percentage = 0.03f;

    void Start()
    {
        cardCount = cardRate;
        manaCount = manaRate;
    }
	
	void Update()
    {
        card.value += (cardGen * cardRate * Time.deltaTime * percentage);
        mana.value += (manaGen * manaRate * Time.deltaTime * percentage);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            card.value = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            mana.value = 0;
        }

        /*
        //card slider
        if (cardCount >= 0)
        {
            cardCount -= Time.deltaTime;
        }
        else
        {
            card.value += (cardGen / 100);
            cardCount = cardRate;
        }

        //mana slider
        if (manaCount >= 0)
        {
            manaCount -= Time.deltaTime;
        }
        else
        {
            mana.value += (manaGen / 100);
            manaCount = manaRate;
        }
        */
    }
}
