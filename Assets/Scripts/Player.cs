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
    public Slider healthbar;

	[SyncVar]
	public GameObject build1;
	[SyncVar]
	public GameObject build2;
	[SyncVar]
	public GameObject build3;
	[SyncVar]
	public GameObject build4;

	void Start()
    {

	}

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(x, 0, y).normalized;

        transform.Translate(dir * Time.deltaTime * speed);

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
        if (!isServer)
        {
            return;
        }

        health -= damage;

        if (health <= 0)
        {
            health = 0;
            print("dis nibba dead");
        }
    }

    public override void OnStartLocalPlayer()
    {
        GetComponentInChildren<Renderer>().material.color = Color.blue;
    }

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
}