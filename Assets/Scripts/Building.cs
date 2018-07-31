using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Building : NetworkBehaviour
{
	void Start()
	{
		//if opponent rotate
		if (!isServer)
		{
			transform.Rotate(Vector3.up * 180);
		}
	}
}