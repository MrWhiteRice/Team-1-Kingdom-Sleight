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

	private float cardGen = 35f;
	private float manaGen = 1f;

	public float cardBuff = 0f;
	public float manaBuff = 0f;

	float manaTimer;

	void Start()
	{
		manaTimer = 1;
	}

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

	private void FixedUpdate()
	{
		if (GameObject.FindObjectOfType<NetworkGameManager>().canStart)
		{
			card.value += ((cardGen + cardBuff) * Time.deltaTime);
			manaTimer -= Time.deltaTime + ((manaGen+manaBuff) / 60);

			if (card.value == 100)
			{
				GameObject.FindObjectOfType<Deck>().DrawCard();
				card.value = 0;
			}

			if (manaTimer <= 0)
			{
				mana.value += 10;
				manaTimer = 1;
			}
		}

		cardBuff = 0;
		manaBuff = 0;
	}
}