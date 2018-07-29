using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Building : NetworkBehaviour
{
	[SyncVar]
	public int id;

	[SyncVar]
	public string pID;

	bool done;

	public override void OnStartAuthority()
	{
		Material[] m = GetComponentInChildren<MeshRenderer>().materials;
		BuildPoint[] point = GameObject.FindObjectsOfType<BuildPoint>();

		for(int x = 0; x < m.Length; x++)
		{
			if(m[x].name.Contains("Team"))
			{
				if(GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().pID == pID)
				{
					m[x].color = Color.blue;
					return;
				}
			}
		}

		for(int y = 0; y < point.Length; y++)
		{
			if(point[y].buildID == id)
			{
				if(GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().pID == pID)
				{
					if(point[y].isPlayer)
					{
						transform.position = point[y].transform.position;
						transform.Rotate(Vector3.up * 180);
						return;
					}
				}
			}
		}
	}

	void Start()
	{
		Invoke("SetDone", 1);
	}

	void Update()
	{
		if(GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().pID != pID && !done)
		{
			Material[] m = GetComponentInChildren<MeshRenderer>().materials;
			BuildPoint[] point = GameObject.FindObjectsOfType<BuildPoint>();

			for(int x = 0; x < m.Length; x++)
			{
				if(m[x].name.Contains("Team"))
				{
					m[x].color = Color.red;
				}
			}

			for(int y = 0; y < point.Length; y++)
			{
				if(point[y].buildID == id)
				{
					if(!point[y].isPlayer)
					{
						transform.position = point[y].transform.position;
						transform.Rotate(Vector3.up * 180);
					}
				}
			}
		}
	}

	void SetDone()
	{
		done = true;
	}
}