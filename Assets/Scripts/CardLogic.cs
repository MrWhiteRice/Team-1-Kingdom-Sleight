using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardLogic : MonoBehaviour
{
	public Image cardImage;
	public Text text;

    public string cardName;
    public int cost;

	public GameObject card;
	public static GameObject spawnedCard;

	public int ID;

	public enum CardType
	{
		creature,
		spell
	};
	public CardType type;

	void Update()
	{
		GetComponent<RectTransform>().anchoredPosition = new Vector2((GetComponent<RectTransform>().sizeDelta.x / 2) + (GetComponent<RectTransform>().sizeDelta.x * ID) + ID * 10, 0);

		if (spawnedCard != null)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("BuildRay")))
			{
				Vector3 point = hit.point;
				point.y = 0.75f;
				spawnedCard.transform.position = point;
			}
		}
	}

    public void Grabbed()
    {
		Destroy(spawnedCard);

		cardImage.enabled = false;
		text.enabled = false;
        spawnedCard = Instantiate(card);

		if(!GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().isMain)
		{
			Vector3 rot = spawnedCard.GetComponent<RectTransform>().eulerAngles;
			rot.y = 180;
			spawnedCard.GetComponent<RectTransform>().eulerAngles = rot;
		}
    }

    public void Released()
    {
        //Check valid
        if (spawnedCard != null)
        {
            Ray ray = new Ray(spawnedCard.transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 9))
            {
				//******************** Creature ********************
				if (hit.transform.GetComponent<CardPoint>())
				{
					//main player
					if(hit.transform.GetComponent<CardPoint>().isPlayer)
					{
						if(GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().isMain)
						{
							if(GameObject.FindObjectOfType<Sliders>().mana.value >= cost)
							{
								if(type == CardType.creature)
								{
									SpawnCreature(hit);
								}
							}
						}
						
					}
					else//secondary player
					{
						if(!GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().isMain)
						{
							if(GameObject.FindObjectOfType<Sliders>().mana.value >= cost)
							{
								if(type == CardType.creature)
								{
									SpawnCreature(hit);
								}
							}
						}
					}
				}

				//******************** Spell ********************
				if (hit.transform.GetComponent<MoveTile>())
				{
					print("spell");

					if (GameObject.FindObjectOfType<Sliders>().mana.value >= cost)
					{
						if (type == CardType.spell)
						{
							SpawnSpell(hit);
						}
					}
				}
			}

            Destroy(spawnedCard);
        }

        //else go back to hand
		cardImage.enabled = true;
		text.enabled = true;
	}

	void SpawnCreature(RaycastHit hit)
	{
		print("Cast Creature: " + cardName);

		//set position
		Vector3 spawnPoint = hit.transform.position;
		spawnPoint.y = hit.point.y;

		//spawn spell
		GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().CmdSpawnMinion(hit.transform.position, hit.transform.eulerAngles, cardName, GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().isMain);
		
		//minus cost | clean up
		GameObject.FindObjectOfType<Sliders>().mana.value -= cost;

		Destroy(gameObject);
		GameObject.FindObjectOfType<Deck>().hand[ID] = null;
	}

	void SpawnSpell(RaycastHit hit)
	{
		print("Cast Spell: " + cardName);

		//set position
		Vector3 spawnPoint = hit.transform.position;
		spawnPoint.y = hit.point.y;

		//spawn spell
		GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().CmdSpawnSpell(hit.transform.position, hit.transform.eulerAngles, cardName);

		//minus cost | clean up
		GameObject.FindObjectOfType<Sliders>().mana.value -= cost;
		Destroy(gameObject);

		for (int x = 0; x < GameObject.FindObjectOfType<Deck>().hand.Length; x++)
		{
			if (GameObject.FindObjectOfType<Deck>().hand[x] != null)
			{
				if (name.Contains(GameObject.FindObjectOfType<Deck>().hand[x].name))
				{
					GameObject.FindObjectOfType<Deck>().hand[x] = null;
					return;
				}
			}
		}
	}
}