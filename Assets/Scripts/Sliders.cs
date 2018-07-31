using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sliders : MonoBehaviour
{
    public Slider mana;
    public Slider card;
    public Slider playerHealth;
    public Slider enemyHealth;

    public float cardGen = 5.0f;
    public float manaGen = 5.0f;

    public float cardRate = 2.5f;
    public float manaRate = 5f;

    float percentage = 3f;
    
	void Update()
    {
		if(GameObject.FindGameObjectWithTag("MyPlayer"))
		{
			playerHealth.value = GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().health;
		}

		if(GameObject.FindGameObjectWithTag("EnemyPlayer"))
		{
			enemyHealth.value = GameObject.FindGameObjectWithTag("EnemyPlayer").GetComponent<Player>().health;
		}

		card.value += (cardGen * cardRate * Time.deltaTime * percentage);
        mana.value += (manaGen * manaRate * Time.deltaTime * percentage);

		if(card.value == 100)
		{
			GameObject.FindObjectOfType<Deck>().DrawCard();
			card.value = 0;
		}
    }
}