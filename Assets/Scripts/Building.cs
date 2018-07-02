using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    GameObject collided;

    public void Build()
    {
        if (collided != null)
        {
            transform.SetParent(collided.transform);

            Vector3 movePos = collided.transform.position;
            movePos.y += 0.5f;

            transform.position = movePos;

            collided.GetComponent<Collider>().isTrigger = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Build Point"))
        {
            collided = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        collided = null;
    }
}
