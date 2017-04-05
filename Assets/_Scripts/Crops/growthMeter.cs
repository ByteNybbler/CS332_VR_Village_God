// Author(s): Hunter Golden, Paul Calande
// Growth meter script.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthMeter : MonoBehaviour
{
    [Tooltip("Reference to the stalks parent object.")]
    public GameObject stalks;

    private float startingSize;
    private float finishSize;
    private float rate;

    // Component references.
    private Crops crops;

    // Use this for initialization
    void Start()
    {
        crops = stalks.GetComponent<Crops>();
        startingSize = 0;
        finishSize = gameObject.transform.localScale.x;
        rate = finishSize / (crops.maxSize / crops.rate);
        gameObject.transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (startingSize < finishSize)
        {
            gameObject.transform.localScale += new Vector3(rate * Time.deltaTime, 0, 0);
            startingSize += rate * Time.deltaTime;
        }
        else
        {
            Destroy(transform.parent.gameObject);
        }
    }
}