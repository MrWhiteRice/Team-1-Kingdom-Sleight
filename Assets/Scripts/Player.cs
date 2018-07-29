using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    public const int maxHealth = 100;

    [SyncVar(hook = "OnChangeHealth")]
    public int health = maxHealth;

	[SyncVar]
	public string pID;

    public string placedBuilding = "";
    public Slider healthbar;

    void Update()
    {
        //if this is not the local player, dont();
        if (!hasAuthority)
        {
            return;
        }

        //else, do shit
		if(Input.GetKeyDown(KeyCode.F))
		{
			CmdTakeDamage();
		}
    }

    void OnChangeHealth(int currentHealth)
    {
        healthbar.value = currentHealth;
    }

    public override void OnStartLocalPlayer()
    {
		pID = System.Guid.NewGuid().ToString();
        //GetComponentInChildren<Renderer>().material.color = Color.blue;
        tag = "MyPlayer";
    }

	[Command]
	void CmdTakeDamage()
    {
		health -= 10;
    }

    //tells server to spawn minion
    [Command]
    void CmdSpawnMinion()
    {
        //find spawn point
        GameObject point = GameObject.FindGameObjectWithTag("SpawnPoint");

        //spawn minion
        GameObject a = (GameObject)Instantiate((GameObject)Resources.Load("Minion"), point.transform.position, point.transform.rotation);

        //spawn it on the network
        NetworkServer.Spawn(a);
    }

	//tell server to spawn building
    [Command]
    public void CmdSpawnBuilding(string name, Vector3 pos, int point, string id)
    {
		//physically spawn the object
		GameObject go = Instantiate((GameObject)Resources.Load("Buildings/" + name), pos, Quaternion.identity);

		go.GetComponent<Building>().id = point;
		go.GetComponent<Building>().pID = id;
		
		//setPosition
		NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }
}