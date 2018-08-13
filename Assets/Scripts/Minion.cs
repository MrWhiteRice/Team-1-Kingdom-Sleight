using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Minion : NetworkBehaviour
{
    [SyncVar]
    public int id;

    [SyncVar]
    public string pID;

	public int health;
	public int damage;

    public int position;
    GameObject moveTile;

	public float baseSpeed;
	public float slowCounts = 0;
	public int slowSpeed = 3;

	public bool main;

	GameObject iceObject;

	public AudioClip die_Sound;

	public enum Lane
    {
        left,
        middle,
        right
    };
    public Lane lane;

	public enum Type
	{
		Melee,
		Ranged
	}

	public Type type;

    private void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position + transform.up * 0.5f, Vector3.down), out hit))
        {
            print(hit.transform.name);
            switch (hit.transform.GetComponent<CardPoint>().lane)
            {
                case CardPoint.Lane.left:
                    lane = Lane.left;
                    break;

                case CardPoint.Lane.middle:
                    lane = Lane.middle;
                    break;

                case CardPoint.Lane.right:
                    lane = Lane.right;
                    break;
            }

			position = hit.transform.GetComponent<CardPoint>().buildID;
        }

        //if opponent rotate
        if (main)
        {
            //position = 0;
		}
		else
        {
			//position = 2;
			transform.Rotate(Vector3.up * 180);
		}
    }

    void Update()
    {
		if (!isServer)
		{
			return;
		}

        GetComponentInChildren<Slider>().value += Time.deltaTime;

		if(health <= 0)
		{
			Die();
		}

		if (iceObject != null)
		{
			GetComponentInChildren<Slider>().maxValue = baseSpeed;
		}
		else
		{
			if (slowCounts <= 4)
			{
				GetComponentInChildren<Slider>().maxValue = baseSpeed + (slowSpeed * slowCounts);
			}
			else
			{
				GetComponentInChildren<Slider>().maxValue = baseSpeed + (slowSpeed * 4);
			}
		}

		if (moveTile != null)
        {
			transform.position = Vector3.Lerp(transform.position, moveTile.transform.position - transform.forward * 0.25f, Time.deltaTime * 2);
        }

        if (GetComponentInChildren<Slider>().value == GetComponentInChildren<Slider>().maxValue)
        {
            CheckAction();
            GetComponentInChildren<Slider>().value = 0;
        }
    }

    void CheckAction()
    {
		//test type (ranged and melee)

		//raycast forward grabbing all objects | cycle through all finding opponents less than 2 units away | attack
		if(type == Type.Ranged)
		{
			print("i am a ranged boi");
			RaycastHit[] hits;
			hits = Physics.RaycastAll(new Ray(transform.position + (Vector3.up*0.25f), transform.forward), 2.5f);

			for (int x = 0; x < hits.Length; x++)
			{
				if (hits[x].transform.name.Contains("Creature"))
				{
					print("hit");
					if(!hits[x].transform.GetComponent<Minion>().main)
					{
						print("re");
						hits[x].transform.GetComponent<Minion>().health -= damage;
					}
					return;
				}
			}

			print("r move");
			Move();
		}

		if(type == Type.Melee)
		{
			//raycast forward | if hit object is less than 1 unit away | attack (melee)
			RaycastHit hit;
			if (Physics.Raycast(new Ray(transform.position + (Vector3.up*0.25f), transform.forward), out hit, 1.5f))
			{
				if (hit.transform.name.Contains("Creature"))
				{
					hit.transform.GetComponent<Minion>().health -= damage;
					return;
				}
				else
				{
					Move();
				}
			}
			else//else | move
			{
				Move();
			}
		}
    }

    private void Move()
    {
		if (main)
		{
			position++;
		}
		else
		{
			position--;
		}

		print(position);

		//find all tiles
		MoveTile[] tiles = GameObject.FindObjectsOfType<MoveTile>();

		if (main)
		{
			if (position == 4)
			{
				DealDamage(damage);
			}
			
			if(position == 1)
			{
				for (int i = 0; i < tiles.Length; i++)
				{
					if (lane.ToString() == tiles[i].lane.ToString())
					{
						print(lane.ToString());
						if(lane.ToString() == "middle")
						{
							print("reeee host");
							GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().RpcSetPoint("Host");
						}
					}
				}
			}
		}
		else
		{
			if(position == 1)
			{
				for (int i = 0; i < tiles.Length; i++)
				{
					if (lane.ToString() == tiles[i].lane.ToString())
					{
						print(lane.ToString());
						if(lane.ToString() == "middle")
						{
							print("reee middle");
							GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().RpcSetPoint("Client");
						}
					}
				}
			}

			if (position == -2)
			{
				DealDamage(damage);
			}
		}

		//cycle through them
		for (int i = 0; i < tiles.Length; i++)
		{
			if (lane.ToString() == tiles[i].lane.ToString())
			{
				if (tiles[i].position == position)
				{
					moveTile = tiles[i].gameObject;
					return;
				}
			}
		}

		CardPoint[] points = GameObject.FindObjectsOfType<CardPoint>();

		for (int j = 0; j < points.Length; j++)
		{
			if (lane.ToString() == points[j].lane.ToString())
			{
				if (points[j].buildID == position)
				{
					moveTile = points[j].gameObject;
					return;
				}
			}
		}

		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.name.Contains("Spell_Fire"))
		{
			health -= 40;
		}
	}

	private void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("Spell_Ice"))
        {
			iceObject = other.transform.gameObject;

			if (slowCounts <= 4)
			{
				GetComponentInChildren<Slider>().maxValue = baseSpeed + (3 * slowCounts);
			}
			else
			{
				GetComponentInChildren<Slider>().maxValue = baseSpeed + (3 * 4);
			}
		}

		if (other.name.Contains("Spell_Lightning"))
        {
			//////////////////////
			if(isServer)
			{
				Die();
			}
        }
    }

	void Die()
	{
		GameObject.FindObjectOfType<SoundsManager>().PlaySound(die_Sound);
		NetworkServer.Destroy(this.gameObject);
	}

    void DealDamage(int dmg)
    {
		Die();

		//main player
		if(main)
		{
			GameObject.FindGameObjectWithTag("EnemyPlayer").GetComponent<Player>().RpcTakeDamage(dmg);
		}
		else
		{
			GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().RpcTakeDamage(dmg);
		}
	}
}