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

		if (Input.GetMouseButtonDown(1))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.transform.GetComponent<Building>())
				{
					if (hit.transform.GetComponent<Building>().isMine)
					{
						CmdDestroyBuilding(hit.transform.gameObject);
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

	//set capture logic
	[ClientRpc]
	public void RpcSetPoint(string type)
	{
		print(type);
		if(type == "Host")
		{
			print("host");
			GameObject.FindObjectOfType<CapturePoint>().host = true;
			GameObject.FindObjectOfType<CapturePoint>().client = false;
		}
		if(type == "Client")
		{
			print("client");
			GameObject.FindObjectOfType<CapturePoint>().client = true;
			GameObject.FindObjectOfType<CapturePoint>().host = false;
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

	//deletes building
	[Command]
	public void CmdDestroyBuilding(GameObject building)
	{
		if (!isServer)
		{
			return;
		}
		NetworkServer.Destroy(building);
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