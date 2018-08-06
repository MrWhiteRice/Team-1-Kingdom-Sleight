using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    public const int maxHealth = 100;

	[SyncVar]
    public int health = maxHealth;

	[SyncVar]
	public string pID;

    public string placedBuilding = "";
    public Slider healthbar;

	public bool isMain;

	void Start()
	{
		if(isLocalPlayer)
		{
			return;
		}

		tag = "EnemyPlayer";
	}

    void Update()
    {
        //if this is not the local player, dont();
        if (!hasAuthority)
        {
            return;
        }

		//a p g backspace alt
		if (Input.GetKey(KeyCode.A))
		{
			if (Input.GetKey(KeyCode.P))
			{
				if (Input.GetKey(KeyCode.G))
				{
					if (Input.GetKey(KeyCode.L))
					{
						if (Input.GetKeyDown(KeyCode.Alpha0))
						{
							GameObject.FindObjectOfType<Sliders>().mana.value = 100;
							GameObject.FindObjectOfType<Sliders>().card.value = 100;
						}
					}
				}
			}
		}
	}

	public override void OnStartLocalPlayer()
	{
		pID = System.Guid.NewGuid().ToString();
		CmdSetID(pID);
		tag = "MyPlayer";

		if(isServer)
		{
			GameObject.Find("Opponent Camera").SetActive(false);
			isMain = true;
			return;
		}
		else
		{
			print("im the away player");
			GameObject.Find("Main Camera").SetActive(false);
			isMain = false;
		}
	}

	//deals damage
	[Command]
	public void CmdDealDamage(int damage, GameObject player)
	{
		if(!isServer)
		{
			return;
		}

		player.GetComponent<Player>().RpcTakeDamage(damage);
	}

	//takes damage
	[ClientRpc]
	public void RpcTakeDamage(int damage)
    {
		health -= damage;
    }

	[Command]
	void CmdSetID(string id)
	{
		pID = id;
	}

    //tells server to spawn minion
    [Command]
    public void CmdSpawnMinion(Vector3 pos, Vector3 rot, string spawnName, bool main)
    {
        //spawn minion
        GameObject a = (GameObject)Instantiate((GameObject)Resources.Load("Units/" + spawnName));

		a.transform.position = pos;
		a.transform.eulerAngles = rot;
		print(main);
		a.GetComponent<Minion>().main = main;

        //spawn it on the network
        NetworkServer.Spawn(a);
    }

	[Command]
	public void CmdSpawnSpell(Vector3 pos, Vector3 rot, string spawnName)
	{
		//spawn spell
        GameObject a = (GameObject)Instantiate((GameObject)Resources.Load("Spells/" + spawnName));

		a.transform.position = pos;
		a.transform.eulerAngles = rot;

        //spawn it on the network
        NetworkServer.Spawn(a);
	}

	//tell server to spawn building
    [Command]
    public void CmdSpawnBuilding(string name, Vector3 pos, string id)
    {
		//physically spawn the object
		GameObject go = Instantiate((GameObject)Resources.Load("Buildings/" + name), pos, Quaternion.identity);

		//go.GetComponent<Building>().id = point;
		go.GetComponent<Building>().pID = id;
		
		//setPosition
		NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }
}