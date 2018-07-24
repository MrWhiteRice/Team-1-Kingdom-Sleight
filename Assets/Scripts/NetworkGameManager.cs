using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkGameManager : NetworkBehaviour
{
    [SyncVar]
    public float time = 10;

	void Start()
    {
		
	}
	
	void Update()
    {
        //if we're not the server, go away
        if (!isServer)
        {
            return;
        }

        //if we are the server, count n shit
        time -= Time.deltaTime;

        if (time <= 0)
        {
            
        }
	}
}