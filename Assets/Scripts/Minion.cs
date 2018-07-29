using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.forward * 2;
    }

    void Update()
    {
        //transform.Translate(Vector3.forward * 2 * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {

    }
}