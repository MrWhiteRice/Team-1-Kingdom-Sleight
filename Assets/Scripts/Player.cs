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

    [SyncVar]
    public int health;
    public RectTransform healthbar;

	void Start()
    {
        health = maxHealth;
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

        healthbar.sizeDelta = new Vector2(health, healthbar.sizeDelta.y);
    }

    void OnChangeHealth(int currentHealth)
    {

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
        GameObject point = GameObject.FindGameObjectWithTag("SpawnPoint");
        GameObject a = Instantiate(minion, point.transform.position, point.transform.rotation);
        NetworkServer.Spawn(a);
    }
}