using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardLogic : MonoBehaviour
{
    Image[] images;

    public string cardName;
    public int cost;

    public GameObject card;
    public static GameObject spawnedCard;

	void Start()
    {
        images = GetComponentsInChildren<Image>();
    }
	
	void Update()
    {
        if (spawnedCard != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("BuildRay")))
            {
                Vector3 point = hit.point;
                point.y = 0.75f;
                spawnedCard.transform.position = point;
            }
        }
	}

    public void Grabbed()
    {
        for (int x = 0; x < images.Length; x++)
        {
            images[x].enabled = false;
        }

        spawnedCard = Instantiate(card);
    }

    public void Released()
    {
        //Check valid
        if (spawnedCard != null)
        {
            Ray ray = new Ray(spawnedCard.transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name.Contains("Spawn Point"))
                {
                    Vector3 spawnPoint = hit.transform.position;
                    spawnPoint.y = hit.point.y;
                    Instantiate((GameObject)Resources.Load("Units/BasicUnit"), spawnPoint, Quaternion.identity);
                    Destroy(gameObject);
                }
            }

            Destroy(spawnedCard);
        }

        //else go back to hand
        for (int x = 0; x < images.Length; x++)
        {
            images[x].enabled = true;
        }
    }
}