using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingLogic : MonoBehaviour
{
    GameObject grabbedObject;
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

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 50, layer))
            {
                position = hit.point;
            }

            grabbedObject.transform.position = position;
        }
    }

    public void Spawn(string name)
    {
        GameObject item = (GameObject)Resources.Load("Buildings/" + name);
        grabbedObject = Instantiate(item);
    }

    public void Build()
    {
        grabbedObject.GetComponent<Building>().Build();
        grabbedObject = null;
    }
}