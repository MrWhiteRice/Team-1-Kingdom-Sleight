using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spell : NetworkBehaviour
{
	public float deathTimer;

	private void Update()
	{
		if(!isServer)
		{
			return;
		}

		deathTimer -= Time.deltaTime;

		if (deathTimer <= 0)
		{
			NetworkServer.Destroy(gameObject);
		}
	}
}
