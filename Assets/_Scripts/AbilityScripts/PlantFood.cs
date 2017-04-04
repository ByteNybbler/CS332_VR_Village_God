// Author(s): Hunter Golden, Paul Calande
// Script for planting food.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantFood : LateInit
{
    [Tooltip("Crop prefab reference.")]
    public GameObject prefabCrop;
    [Tooltip("Shrine instance reference.")]
    public GameObject instanceShrine;
    [Tooltip("Reference to the controller script which summons the crops.")]
    public GameObject locationObj;
    [Tooltip("The maximum number of crops that can exist in the scene at once.")]
    public int maxNumberOfCrops = 10;

    // List of existing crops.
    private List<GameObject> crops = new List<GameObject>();

    // Component references.
    private Shrine compShrine;
    private CastRay compCastRay;

    public override void Init()
    {
        compShrine = instanceShrine.GetComponent<Shrine>();
        compCastRay = locationObj.GetComponent<CastRay>();

        base.Init();
    }

    public void MakeFood()
    {
        if (crops.Count < maxNumberOfCrops)
        {
            Vector3 location;
            if (compCastRay.Cast(out location))
            {
                if (compShrine.SpendPoints(10))
                {
                    GameObject cropInstance = Instantiate(prefabCrop, location, Quaternion.identity);
                    cropInstance.GetComponent<PlantHealth>().Died += PlantHealth_Died;
                    crops.Add(cropInstance);
                }
            }
        }
    }

    private void PlantHealth_Died(GameObject victim)
    {
        crops.Remove(victim);
    }

    protected override void EventsSubscribe()
    {
        foreach (GameObject crop in crops)
        {
            if (crop != null)
            {
                crop.GetComponent<PlantHealth>().Died += PlantHealth_Died;
            }
        }
    }

    protected override void EventsUnsubscribe()
    {
        foreach (GameObject crop in crops)
        {
            if (crop != null)
            {
                crop.GetComponent<PlantHealth>().Died -= PlantHealth_Died;
            }
        }
    }
}