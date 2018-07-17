using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Building : NetworkBehaviour
{
    GameObject collided;

    public void Build()
    {
        if (collided != null)
        {
            transform.SetParent(collided.transform);

			transform.position = collided.transform.position;

            collided.GetComponent<Collider>().isTrigger = false;

			Player[] players = GameObject.FindObjectsOfType<Player>();
			for (int x = 0; x < players.Length; x++)
			{
				if (players[x].GetComponent<NetworkIdentity>().hasAuthority)
				{
					BuildPoint bp = collided.GetComponent<BuildPoint>();
					string name = gameObject.name;

					switch (bp.buildID)
					{
						case 0:
							players[x].build1 = (GameObject)Resources.Load(gameObject.name);
							break;
						case 1:
							players[x].build2 = (GameObject)Resources.Load(gameObject.name);
							break;
						case 2:
							players[x].build3 = (GameObject)Resources.Load(gameObject.name);
							break;
						case 3:
							players[x].build4 = (GameObject)Resources.Load(gameObject.name);
							break;
					}
				}
			}
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("BuildPoint"))
        {
            collided = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        collided = null;
    }
}
