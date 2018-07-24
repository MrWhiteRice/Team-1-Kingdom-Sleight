using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkGameManager : NetworkManager
{
    int port = 0000;
    string ip = "LocalHost";
    
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
}