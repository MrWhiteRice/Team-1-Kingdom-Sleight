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

	public Text manaIndicator;

	private float cardGen = 20f;
	private float manaGen = 0.5f;

	public float cardBuff = 0f;
	public float manaBuff = 0f;

	float manaTimer;

	void Start()
	{
		manaTimer = 1;
	}

	void Update()
    {
		manaIndicator.text = (mana.value / 10) + "";

		if (GameObject.FindObjectOfType<NetworkGameManager>().canStart)
		{
			if (GameObject.FindGameObjectWithTag("MyPlayer"))
			{
				playerHealth.value = GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().health;

				if(playerHealth.value <= 0)
				{
					GameObject.FindObjectOfType<NetworkGameManager>().StartCoroutine("EndGame", "Lose");
					//SceneManager.LoadScene("Lose", LoadSceneMode.Single);
					//NetworkGameManager.Disconnect();
				}
			}

			if (GameObject.FindGameObjectWithTag("EnemyPlayer"))
			{
				enemyHealth.value = GameObject.FindGameObjectWithTag("EnemyPlayer").GetComponent<Player>().health;

				if(enemyHealth.value <= 0)
				{
					GameObject.FindObjectOfType<NetworkGameManager>().StartCoroutine("EndGame", "Win");
					//SceneManager.LoadScene("Win", LoadSceneMode.Single);
					//etworkGameManager.Disconnect();
				}
			}
		}
    }

	public void Buff()
	{
		manaBuff += 0.5f;
		cardBuff += 0.5f;
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