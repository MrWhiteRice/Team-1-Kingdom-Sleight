using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using System.Collections;

public class MenuLogic : NetworkBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

	public void ReturnToMenu()
	{
		if(NetworkManager.singleton.IsClientConnected())
		{
			//SceneManager.LoadScene(0, LoadSceneMode.Single);
			NetworkGameManager.Disconnect();
		}
	}

	public void Join()
	{
		GameObject.FindObjectOfType<NetworkGameManager>().JoinGame();
	}

	public void Host()
	{
		GameObject.FindObjectOfType<NetworkGameManager>().StartHost();
	}
}
