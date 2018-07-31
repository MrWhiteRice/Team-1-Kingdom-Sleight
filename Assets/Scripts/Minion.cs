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

    public int position;
    GameObject moveTile;
    public float gen = 1;

	public bool main;

    public enum Lane
    {
        left,
        middle,
        right
    };
    public Lane lane;

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
        }

        //if opponent rotate
        if (!isServer)
        {
            position = 2;
            transform.Rotate(Vector3.up * 180);
			return;
        }
        else
        {
            position = 0;
        }
    }

    void Update()
    {
		if (!isServer)
		{
			return;
		}

        GetComponentInChildren<Slider>().value += Time.deltaTime * gen;

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
        //raycast forward | if hit object is less than 1 unit away | attack
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position, transform.forward), out hit, 0.5f))
        {
            print("123123123123" + hit.transform.name);
            if (hit.transform.name.Contains("Creature"))
            {
                print("Fightin!");
                return;
            }
        }
        else//else | move
        {
            print("No Enemy Found, Moving!");
            Move();
        }
    }

    private void Move()
    {
        MoveTile[] tiles = GameObject.FindObjectsOfType<MoveTile>();

        for (int x = 0; x < tiles.Length; x++)
        {
            if (lane.ToString() == tiles[x].lane.ToString())
            {
                //move
                //find correct number
                if (tiles[x].position == position)
                {
                    moveTile = tiles[x].gameObject;
                    if (main)
                    {
                        position++;
                        return;
                    }
                    else
                    {
                        position--;
                    }
                    return;
                }

                CardPoint[] cardPoints = GameObject.FindObjectsOfType<CardPoint>();

                //check if moving to opponent
                if (isServer)
                {
                    //move to base
                    if(position == 3)
                    {
                        print("At 3");
                        for (int y = 0; y < cardPoints.Length; y++)
                        {
                            if (cardPoints[y].lane.ToString() == lane.ToString())
                            {
                                if (!cardPoints[y].isPlayer)
                                {
                                    moveTile = cardPoints[y].gameObject;
                                    position++;
                                    return;
                                }
                            }
                        }
                    }
                    //deal damage to base
                    else if (position == 4)
                    {
                        print("at 4");
                        DealDamage();
                        return;
                    }
                }
                else//opponent
                {
                    if (position == 0)
                    {
                        print("at 0");
                        for (int y = 0; y < cardPoints.Length; y++)
                        {
                            if (cardPoints[y].lane.ToString() == lane.ToString())
                            {
                                if (cardPoints[y].isPlayer)
                                {
                                    moveTile = cardPoints[y].gameObject;
                                    position--;
                                    return;
                                }
                            }
                        }
                    }
                    else if(position == -1)
                    {
                        print("at -1 | DESTROYING NEXUS!");
                        DealDamage();
                        return;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Spell_Ice"))
        {
            gen = 0.5f;
        }

        if (other.name.Contains("Spell_Lightning"))
        {
			//////////////////////
			Die();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Spell_Ice"))
        {
            gen = 1;
        }

        if (other.name.Contains("Spell_Lightning"))
        {
			///////////////////
			Die();
		}
    }

	void Die()
	{
		Network.Destroy(gameObject);
	}

    void DealDamage()
    {
        NetworkServer.Destroy(gameObject);

        if (isServer)
        {
            GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().CmdTakeDamage(10, GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().pID);
            return;
        }
        else
        {
            GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().CmdTakeDamage(10, GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().pID);
            return;
        }
    }
}