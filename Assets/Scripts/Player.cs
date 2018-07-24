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

	public GameObject build1;
	public GameObject build2;
	public GameObject build3;
	public GameObject build4;

    public bool isMine;

	void Start()
    {
        
	}

    void Update()
    {
        //if this is not the local player, dont();
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
        isMine = true;
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