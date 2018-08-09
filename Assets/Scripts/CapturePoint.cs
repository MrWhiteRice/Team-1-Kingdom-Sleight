using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePoint : MonoBehaviour {

	public bool host;
	public bool client;

	private void Update()
	{
		if(host && !client)
		{
			if(GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().isMain)
			{
				SetColour(Color.blue);
			}
			else
			{
				SetColour(Color.red);
			}
			GameObject.FindObjectOfType<Sliders>().Buff();
		}
		else if(!host && client)
		{
			if(GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().isMain == false)
			{
				SetColour(Color.blue);
			}
			else
			{
				SetColour(Color.red);
			}
			GameObject.FindObjectOfType<Sliders>().Buff();
		}
		else
		{
			SetColour(new Color(0.5f, 0.25f, 0.5f));
		}
	}

	private void SetColour(Color c)
	{
		Material[] m = GetComponentInChildren<MeshRenderer>().materials;
		for(int x = 0; x < m.Length; x++)
		{
			if(m[x].name.Contains("Team"))
			{
				m[x].color = c;
			}
		}
	}

}
