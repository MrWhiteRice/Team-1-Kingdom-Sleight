using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BuildingLogic : NetworkBehaviour
{
    GameObject grabbedObject;
    GameObject placedObject;
    Vector3 position;
    Camera cam;

    bool isMine;

    void Start()
    {
        cam = GameObject.FindObjectOfType<Camera>();

        if (!isLocalPlayer)
        {
            return;
        }

        isMine = true;
    }

    void Update()
    {
        if (grabbedObject != null)
        {
            int layer = LayerMask.GetMask("BuildRay");

            //item placement
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 50, layer))
            {
                position = hit.point;
            }

            grabbedObject.transform.position = position;

            if (Input.GetMouseButtonUp(0))
            {
                ReleaseMouse();
            }
        }
    }

    public void Spawn(string name)
    {
        GameObject item = (GameObject)Resources.Load("Buildings/" + name);
        grabbedObject = Instantiate(item);
    }

    public void ReleaseMouse()
    {
        print("Released Mouse");

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 50))
        {
            if (hit.transform.gameObject.name.Contains("BuildPoint"))
            {
                print("Valid Position");
                grabbedObject.transform.SetParent(hit.transform);
                grabbedObject.transform.position = Vector3.zero;
                placedObject = grabbedObject;
                grabbedObject = null;

                if (!isClient)
                {
                    return;
                }

                RpcBuild();
            }
            else
            {
                print("yeah naw");
                Object.Destroy(grabbedObject);
            }
        }
    }

    [ClientRpc]
    public void RpcBuild()
    {
        print("Build!");

        NetworkServer.Spawn(placedObject);
    }
}