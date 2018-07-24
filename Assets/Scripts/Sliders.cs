using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sliders : MonoBehaviour
{
    public Slider mana;
    public Slider card;

    public float cardGen = 5.0f;
    public float manaGen = 5.0f;

    public float cardRate = 2.5f;
    public float manaRate = 5f;

    float percentage = 0.03f;

    void Start()
    {

    }
    
	void Update()
    {
        card.value += (cardGen * cardRate * Time.deltaTime * percentage);
        mana.value += (manaGen * manaRate * Time.deltaTime * percentage);
    }
}
