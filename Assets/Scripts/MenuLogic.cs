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
		GameObject.FindObjectOfType<NetworkGameManager>().offlineScene = SceneManager.GetSceneAt(0).name;
		SceneManager.LoadScene(0, LoadSceneMode.Single);
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
