using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingLogic : MonoBehaviour
{
    GameObject grabbedObject;
    string objectName;

    Vector3 position;
    Camera cam;

    void Update()
    {
		Player p = null;

		if(GameObject.FindGameObjectWithTag("MyPlayer"))
		{
			p = GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>();

			if(p.isMain)
			{
				cam = GameObject.Find("Main Camera").GetComponent<Camera>();
			}

			if(!p.isMain)
			{
				cam = GameObject.Find("Opponent Camera").GetComponent<Camera>();
			}
		}

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
            if (hit.transform.gameObject.name.Contains("BuildPoint"))
            {
				//main
				if(hit.transform.GetComponent<BuildPoint>().isPlayer)
				{
					if(GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().isMain)
					{
						print("main");
						hit.transform.gameObject.layer = 2;

						grabbedObject.transform.SetParent(hit.transform);
						grabbedObject.transform.position = hit.transform.position;
						grabbedObject.transform.rotation = hit.transform.rotation;

						//build object
						GameObject player = GameObject.FindGameObjectWithTag("MyPlayer");
						player.GetComponent<Player>().CmdSpawnBuilding(objectName, grabbedObject.transform.position, player.GetComponent<Player>().pID);

						Object.Destroy(grabbedObject);
						grabbedObject = null;
					}
				}
				else
				{
					if(!GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Player>().isMain)
					{
						print("not main");
						hit.transform.gameObject.layer = 2;

						grabbedObject.transform.SetParent(hit.transform);
						grabbedObject.transform.position = hit.transform.position;
						grabbedObject.transform.rotation = hit.transform.rotation;

						//build object
						GameObject player = GameObject.FindGameObjectWithTag("MyPlayer");
						player.GetComponent<Player>().CmdSpawnBuilding(objectName, grabbedObject.transform.position, player.GetComponent<Player>().pID);

						Object.Destroy(grabbedObject);
						grabbedObject = null;
					}
				}

                Object.Destroy(grabbedObject);
            }
            else
            {
                print("yeah naw");
                Object.Destroy(grabbedObject);
            }
        }
    }
}