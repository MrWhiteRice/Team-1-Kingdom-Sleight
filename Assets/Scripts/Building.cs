using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Building : NetworkBehaviour
{
	public bool checkPlayer;
	public bool isMain;

	bool isMine;

	public float manaBuff;
	public float cardBuff;

	[SyncVar]
	public string pID;

	void Start()
	{
		//if opponent rotate
		if (!isServer)
		{
			transform.Rotate(Vector3.up * 180);
		}

		gameObject.layer = 0;

		Material[] m = GetComponentInChildren<MeshRenderer>().materials;
		for (int x = 0; x < m.Length; x++)
		{
			if(m[x].name.Contains("Team"))
			{
				if(checkPlayer)
				{
					if(GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().isMain == isMain)
					{
						m[x].color = Color.blue;
						isMine = true;
					}
					else
					{
						m[x].color = Color.red;
					}
				}
				else
				{
					if (GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().pID == pID)
					{
						m[x].color = Color.blue;
						isMine = true;
					}
					else
					{
						m[x].color = Color.red;
					}
				}
			}
		}
	}

	private void Update()
	{
		if (!isMine)
		{
			return;
		}

		GameObject.FindObjectOfType<Sliders>().manaBuff += manaBuff;
		GameObject.FindObjectOfType<Sliders>().cardBuff += cardBuff;
	}
}