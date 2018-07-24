using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    float speed = 5;
    public GameObject minion;
    public const int maxHealth = 100;

    [SyncVar(hook = "OnChangeHealth")]
    public int health = maxHealth;

    public string placedBuilding = "";
    public Slider healthbar;

	void Start()
    {
        
	}

    void Update()
    {
        //if this is not the local player, dont();
        if (!hasAuthority)
        {
            return;
        }

        //else, do shit

        //spawn
        if (Input.GetKeyDown(KeyCode.F))
        {
            CmdSpawnMinion();
        }
    }

    void OnChangeHealth(int currentHealth)
    {
        healthbar.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isServer)
        {
            health -= damage;
        }

        if (health <= 0)
        {
            health = 0;
            print("dis nibba dead");
        }
    }

    public override void OnStartLocalPlayer()
    {
        GetComponentInChildren<Renderer>().material.color = Color.blue;
        tag = "MyPlayer";
    }

    //tells server to spawn minion
    [Command]
    void CmdSpawnMinion()
    {
        //find spawn point
        GameObject point = GameObject.FindGameObjectWithTag("SpawnPoint");

        //spawn minion
        GameObject a = Instantiate(minion, point.transform.position, point.transform.rotation);

        //spawn it on the network
        NetworkServer.Spawn(a);
    }

    [Command]
    public void CmdSpawnBuilding(string name, Vector3 pos)
    {
        GameObject go = Instantiate((GameObject)Resources.Load("Buildings/" + name), pos, Quaternion.identity);
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }
}