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

	public bool main;

	GameObject iceObject;

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
				GetComponentInChildren<Slider>().maxValue = baseSpeed + (3 * slowCounts);
			}
			else
			{
				GetComponentInChildren<Slider>().maxValue = baseSpeed + (3 * 4);
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
        //raycast forward | if hit object is less than 1 unit away | attack
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position + (Vector3.up*0.25f), transform.forward), out hit, 1.5f))
        {
            print("123123123123" + hit.transform.name);
            if (hit.transform.name.Contains("Creature"))
            {
                print("Fightin!");
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
            print("No Enemy Found, Moving!");
            Move();
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

		//cycle through them
		for (int i = 0; i < tiles.Length; i++)
		{
			if (lane.ToString() == tiles[i].lane.ToString())
			{
				print("correctlane");
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

		if (main)
		{
			if (position == 4)
			{
				DealDamage(damage);
			}
		}
		else
		{
			if (position == -2)
			{
				DealDamage(damage);
			}
		}
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("Spell_Ice"))
        {
			print("slowing down");
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