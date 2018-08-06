using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Sliders : MonoBehaviour
{
    public Slider mana;
    public Slider card;
    public Slider playerHealth;
    public Slider enemyHealth;

	[HideInInspector]
    public float cardGen = 3.5f;
	[HideInInspector]
    public float manaGen = 2.5f;

	[HideInInspector]
    public float cardBuff = 0f;
	[HideInInspector]
    public float manaBuff = 0f;
    
	void Update()
    {
		if (GameObject.FindObjectOfType<NetworkGameManager>().canStart)
		{
			if (GameObject.FindGameObjectWithTag("MyPlayer"))
			{
				playerHealth.value = GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().health;

				if(playerHealth.value <= 0)
				{
					SceneManager.LoadScene("Lose", LoadSceneMode.Single);
					//NetworkGameManager.Disconnect();
				}
			}

			if (GameObject.FindGameObjectWithTag("EnemyPlayer"))
			{
				enemyHealth.value = GameObject.FindGameObjectWithTag("EnemyPlayer").GetComponent<Player>().health;

				if(enemyHealth.value <= 0)
				{
					SceneManager.LoadScene("Win", LoadSceneMode.Single);
					//etworkGameManager.Disconnect();
				}
			}
		}
    }

	private void LateUpdate()
	{
		if (GameObject.FindObjectOfType<NetworkGameManager>().canStart)
		{
			card.value += ((cardGen + cardBuff) * Time.deltaTime);
			mana.value += ((manaGen + manaBuff) * Time.deltaTime);

			if (card.value == 100)
			{
				GameObject.FindObjectOfType<Deck>().DrawCard();
				card.value = 0;
			}

			cardBuff = 0;
			manaBuff = 0;
		}
	}
}