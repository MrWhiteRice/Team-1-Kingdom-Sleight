using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingLogic : MonoBehaviour
{
    GameObject grabbedObject;
    string objectName;

    Vector3 position;
    Camera cam;

    void Start()
    {
        cam = GameObject.FindObjectOfType<Camera>();
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
		grabbedObject.GetComponent<Building>().enabled = false;
        objectName = name;
    }

    public void ReleaseMouse()
    {
        print("Released Mouse");

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 50))
        {
            if (hit.transform.gameObject.name.Contains("BuildPoint") && hit.transform.GetComponent<BuildPoint>().isPlayer)
            {
                print("Valid Position");
				hit.transform.gameObject.layer = 2;

				grabbedObject.transform.SetParent(hit.transform);
                grabbedObject.transform.position = hit.transform.position;

				//build object
				GameObject player = GameObject.FindGameObjectWithTag("MyPlayer");
                player.GetComponent<Player>().CmdSpawnBuilding(objectName, grabbedObject.transform.position, hit.transform.GetComponent<BuildPoint>().buildID, GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().pID);

				Object.Destroy(grabbedObject);
                grabbedObject = null;
            }
            else
            {
                print("yeah naw");
                Object.Destroy(grabbedObject);
            }
        }
    }
}