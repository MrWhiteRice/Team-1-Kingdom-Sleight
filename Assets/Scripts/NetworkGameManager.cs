using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class NetworkGameManager : NetworkManager
{
    public int port;
    public string ip;

	public TMP_InputField ipField;
	public TMP_InputField portField;

	int currentSceneID;

	bool mapLoaded;

	public int existingPlayers = 0;
	public bool canStart;

	//******************* HOST *********************
	public void StartupHost()
	{
		SetPort();
		SetIPAddress();

		PlayerPrefs.SetInt("port", port);
		PlayerPrefs.SetString("ip", ip);

		print("Hosting Game with ip: " + ip + " and port: " + port + "!");

		NetworkManager.singleton.StartHost();
	}

    //******************* JOIN *********************
    public void JoinGame()
    {
		SetIPAddress();
		SetPort();

		PlayerPrefs.SetInt("port", port);
		PlayerPrefs.SetString("ip", ip);

		print("Joining Game with ip: " + ip + " and port: " + port + "!");

		NetworkManager.singleton.StartClient();
	}

	public override void OnServerConnect(NetworkConnection conn)
	{
		print("connecting to server with: " + existingPlayers + " players");
		if (existingPlayers > 1)
		{
			print("cant connect, its full x^x");
			conn.Disconnect();
			conn.Dispose();
			NetworkServer.DestroyPlayersForConnection(conn);
		}
	}

	//******************* SETTERS *********************
	void SetPort()
    {
        NetworkManager.singleton.networkPort = port;
    }

    void SetIPAddress()
    {
        NetworkManager.singleton.networkAddress = ip;
    }
	
	//******************* MAIN *********************
	void Start()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;

		port = PlayerPrefs.GetInt("port", 7777);
		ip = PlayerPrefs.GetString("ip", "localhost");
	}

	private void Update()
	{
		//main menu
		if (currentSceneID == 0)
		{
			if (portField != null && portField.text != "")
			{
				port = int.Parse(portField.text);
			}
			else
			{
				if (GameObject.Find("Port - InputField"))
				{
					portField = GameObject.Find("Port - InputField").GetComponent<TMP_InputField>();
					portField.text = ""+port;
				}
			}

			if (ipField != null)
			{
				ip = ipField.text;
			}
			else
			{
				if (GameObject.Find("Ip - InputField"))
				{
					ipField = GameObject.Find("Ip - InputField").GetComponent<TMP_InputField>();
					ipField.text = ip;
				}
			}
		}
		//game
		else
		{
			if(GameObject.FindObjectOfType<Player>())
			{
				existingPlayers = GameObject.FindObjectsOfType<Player>().Length;

				if (existingPlayers > 1)
				{
					canStart = true;
					Object.Destroy(GameObject.Find("Match Blocker"));
				}
			}
		}
	}

	//******************* OTHER *********************
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		print("Scene loaded: " + scene.name);
		currentSceneID = scene.buildIndex;

		if(scene.buildIndex == 0)
		{
			print("Menu");
			InitButtons();
			mapLoaded = false;
		}
		else
		{
			print("Not menu: " + scene.buildIndex);

			if (!mapLoaded)
			{
				SceneManager.LoadScene(2, LoadSceneMode.Additive);
				mapLoaded = true;
			}

			//InitDisconnect();
		}
	}

	public static void Disconnect()
	{
		NetworkManager.singleton.StopHost();
	}

	public void InitButtons()
	{
		if(GameObject.Find("Button_Host"))
		{
			GameObject.Find("Button_Host").GetComponent<Button>().onClick.RemoveAllListeners();
			GameObject.Find("Button_Host").GetComponent<Button>().onClick.AddListener(StartupHost);
		}

		if(GameObject.Find("Button_Join"))
		{
			GameObject.Find("Button_Join").GetComponent<Button>().onClick.RemoveAllListeners();
			GameObject.Find("Button_Join").GetComponent<Button>().onClick.AddListener(JoinGame);
		}
	}

	void InitDisconnect()
	{
		GameObject.Find("NM_Disconnect").GetComponent<Button>().onClick.RemoveAllListeners();
		GameObject.Find("NM_Disconnect").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);
	}
}