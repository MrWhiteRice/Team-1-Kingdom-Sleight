using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CardLogic : NetworkBehaviour
{
	public Image cardImage;
	public Text text;
	public Text att;
	public Text def;
	public Text cmc;

	public string cardName;
    public int cost;
	public int cycleCost = 20;

	public GameObject card;
	public static GameObject spawnedCard;

	public int ID;

	public enum CardType
	{
		creature,
		spell,
		buliding
	};
	public CardType type;

	private void Start()
	{
		cmc.text = "" + (cost/10);
	}

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
		cmc.enabled = false;

		if (att != null)
			att.enabled = false;
		if (def != null)
			def.enabled = false;

		spawnedCard = Instantiate(card);

		if (type == CardType.creature)
		{
			//set data
			spawnedCard.GetComponent<WorldCard>().desc_creature.text = text.text;
			spawnedCard.GetComponent<WorldCard>().card_creature.sprite = cardImage.sprite;

			//disable spell stuff
			spawnedCard.GetComponent<WorldCard>().desc_spell.enabled = false;
			spawnedCard.GetComponent<WorldCard>().card_spell.enabled = false;

			//set creature data
			spawnedCard.GetComponent<WorldCard>().attack.text = att.text;
			spawnedCard.GetComponent<WorldCard>().health.text = def.text;
		}
		else if(type == CardType.spell || type == CardType.buliding)
		{
			//set data
			spawnedCard.GetComponent<WorldCard>().desc_spell.text = text.text;
			spawnedCard.GetComponent<WorldCard>().card_spell.sprite = cardImage.sprite;

			//disable creature stuff
			spawnedCard.GetComponent<WorldCard>().desc_creature.enabled = false;
			spawnedCard.GetComponent<WorldCard>().card_creature.enabled = false;

			spawnedCard.GetComponent<WorldCard>().attack.enabled = false;
			spawnedCard.GetComponent<WorldCard>().health.enabled = false;
		}
		
		spawnedCard.GetComponent<WorldCard>().cmc.text = cmc.text;

		if (!GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().isMain)
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
            if (Physics.Raycast(ray, out hit, 9, LayerMask.GetMask("Tile")))
            {
				//******************** Creature ********************
				if (hit.transform.GetComponent<CardPoint>())
				{
					if (hit.transform.GetComponent<CardPoint>().isPlayer)
					{
						if (GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().isMain)
						{
							if (GameObject.FindObjectOfType<Sliders>().mana.value >= cost)
							{
								if (type == CardType.creature)
								{
									SpawnCreature(hit);
								}
							}
						}
					}
					else if (hit.transform.GetComponent<CardPoint>().isPlayer == false)
					{
						if (GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().isMain == false)
						{
							if (GameObject.FindObjectOfType<Sliders>().mana.value >= cost)
							{
								if (type == CardType.creature)
								{
									SpawnCreature(hit);
								}
							}
						}
					}
				}

				//******************** Spell ********************
				if (hit.transform.GetComponent<MoveTile>() || hit.transform.GetComponent<CardPoint>())
				{
					if (hit.transform.GetComponent<CardPoint>())
					{
						if (hit.transform.GetComponent<CardPoint>().isPlayer)
						{
							if (GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().isMain)
							{
								if (GameObject.FindObjectOfType<Sliders>().mana.value >= cost)
								{
									if (type == CardType.spell)
									{
										SpawnSpell(hit);
									}
								}
							}
						}
					}

					if (GameObject.FindObjectOfType<Sliders>().mana.value >= cost)
					{
						if (type == CardType.spell)
						{
							SpawnSpell(hit);
						}
					}
				}

				//******************** Building ********************
				if (hit.transform.GetComponent<BuildPoint>())
				{
					if (hit.transform.GetComponent<BuildPoint>().isPlayer)
					{
						if (GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().isMain)
						{
							if (hit.transform.GetComponent<BuildPoint>().isBuilt == false)
							{
								if (GameObject.FindObjectOfType<Sliders>().mana.value >= cost)
								{
									if (type == CardType.buliding)
									{
										hit.transform.GetComponent<BuildPoint>().isBuilt = true;
										SpawnBuilding(hit, hit.transform.gameObject);
									}
								}
							}
						}
					}
					else if (hit.transform.GetComponent<BuildPoint>().isPlayer == false)
					{
						if (GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().isMain == false)
						{
							if (hit.transform.GetComponent<BuildPoint>().isBuilt == false)
							{
								if (GameObject.FindObjectOfType<Sliders>().mana.value >= cost)
								{
									if (type == CardType.buliding)
									{
										hit.transform.GetComponent<BuildPoint>().isBuilt = true;
										SpawnBuilding(hit, hit.transform.gameObject);
									}
								}
							}
						}
					}
				}
			}

			Ray bray = new Ray(spawnedCard.transform.position + (Vector3.up * 2), Vector3.down);
			RaycastHit bhit;
			if (Physics.Raycast(bray, out bhit))
			{
				//******************** DISCARD ********************
				if (bhit.transform.name.Contains("Barracks"))
				{
					if (bhit.transform.GetComponent<Building>().isMine)
					{
						//find next empty slot in grave
						for (int x = 0; x < GameObject.FindObjectOfType<Deck>().graveyard.Length; x++)
						{
							if (GameObject.FindObjectOfType<Deck>().graveyard[x] == null)
							{
								GameObject.FindObjectOfType<Deck>().graveyard[x] = GameObject.FindObjectOfType<Deck>().hand[ID];
								break;
							}
						}

						GameObject.FindObjectOfType<Deck>().hand[ID] = null;
						GameObject.FindObjectOfType<Sliders>().mana.value -= cycleCost;

						GameObject.FindObjectOfType<Deck>().DrawCard();
						Destroy(gameObject);
					}
				}
			}

			Destroy(spawnedCard);
        }

        //else go back to hand
		cardImage.enabled = true;
		text.enabled = true;
		cmc.enabled = true;

		if (att != null)
			att.enabled = true;
		if (att != null)
			def.enabled = true;
	}

	void SpawnCreature(RaycastHit hit)
	{
		print("Cast Creature: " + cardName);

		//set position
		Vector3 spawnPoint = hit.transform.position;
		spawnPoint.y = hit.point.y;
		
		//minus cost | clean up
		GameObject.FindObjectOfType<Sliders>().mana.value -= cost;

		//find next empty slot in grave
		for (int x = 0; x < GameObject.FindObjectOfType<Deck>().graveyard.Length; x++)
		{
			if (GameObject.FindObjectOfType<Deck>().graveyard[x] == null)
			{
				GameObject.FindObjectOfType<Deck>().graveyard[x] = GameObject.FindObjectOfType<Deck>().hand[ID];
				break;
			}
		}

		Destroy(gameObject);
		GameObject.FindObjectOfType<Deck>().hand[ID] = null;

		//spawn spell
		GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().CmdSpawnMinion(hit.transform.position, hit.transform.eulerAngles, cardName, GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().isMain);
	}

	void SpawnBuilding(RaycastHit hit, GameObject pointBuilding)
	{
		//set position
		Vector3 spawnPoint = hit.transform.position;
		spawnPoint.y = hit.point.y;

		//minus cost | clean up
		GameObject.FindObjectOfType<Sliders>().mana.value -= cost;

		//find next empty slot in grave
		for (int x = 0; x < GameObject.FindObjectOfType<Deck>().graveyard.Length; x++)
		{
			if (GameObject.FindObjectOfType<Deck>().graveyard[x] == null)
			{
				GameObject.FindObjectOfType<Deck>().graveyard[x] = GameObject.FindObjectOfType<Deck>().hand[ID];
				break;
			}
		}

		Destroy(gameObject);
		GameObject.FindObjectOfType<Deck>().hand[ID] = null;

		//spawn spell
		GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().CmdSpawnBuilding(cardName, spawnPoint, GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().pID, pointBuilding);
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


		//find next empty slot in grave
		for(int x = 0; x < GameObject.FindObjectOfType<Deck>().graveyard.Length; x++)
		{
			if (GameObject.FindObjectOfType<Deck>().graveyard[x] == null)
			{
				GameObject.FindObjectOfType<Deck>().graveyard[x] = GameObject.FindObjectOfType<Deck>().hand[ID];
				break;
			}
		}

		GameObject.FindObjectOfType<Deck>().hand[ID] = null;
	}
}