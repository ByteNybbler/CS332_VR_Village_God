using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plantation : MonoBehaviour
{
    public GameObject meter;

    void Start()
    {
        Instantiate(meter, gameObject.transform.position, gameObject.transform.rotation);
    }
}