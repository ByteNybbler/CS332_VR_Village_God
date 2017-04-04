// Author(s): Hunter Golden
// Growth meter script.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthMeter : MonoBehaviour
{
    public float startingSize;
    public float finishSize;
    public Crops crops;
    public float rate;

    // Use this for initialization
    void Start()
    {
        crops = GameObject.Find("plant").GetComponent<Crops>();
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
            Destroy(GameObject.Find("meter"));
        }
    }
}