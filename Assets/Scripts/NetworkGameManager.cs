using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkGameManager : NetworkManager
{
    int port = 7777;
    string ip = "localhost";
    
    //******************* HOST *********************
    public void StartupHost()
    {
        SetPort(port);
        NetworkManager.singleton.StartHost();
    }

    //******************* JOIN *********************
    public void JoinGame()
    {
		SetIPAddress();
		SetPort(port);
		NetworkManager.singleton.StartClient();
	}

    //******************* SETTERS *********************
    void SetPort(int port)
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
	}

	//******************* OTHER *********************
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		print("Scene loaded: " + scene.name);

		if(scene.buildIndex == 0)
		{
			print("Menu");
			//InitButtons();
		}
		else
		{
			print("Not menu: " + scene.buildIndex);
			//InitDisconnect();
		}
	}

	void InitButtons()
	{
		GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.RemoveAllListeners();
		GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.AddListener(StartupHost);

		GameObject.Find("ButtonJoinGame").GetComponent<Button>().onClick.RemoveAllListeners();
		GameObject.Find("ButtonJoinGame").GetComponent<Button>().onClick.AddListener(JoinGame);
	}

	void InitDisconnect()
	{
		GameObject.Find("NM_Disconnect").GetComponent<Button>().onClick.RemoveAllListeners();
		GameObject.Find("NM_Disconnect").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);
	}
}