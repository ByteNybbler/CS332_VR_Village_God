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

    public delegate void CropDiedHandler(GameObject victim);
    public event CropDiedHandler CropDied;

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
                if (compShrine.SpendPoints(10, location))
                {
                    GameObject cropInstance = Instantiate(prefabCrop, location, Quaternion.identity);
                    cropInstance.GetComponent<PlantStatus>().Died += PlantStatus_Died;
                    crops.Add(cropInstance);
                }
            }
        }
    }

    private void PlantStatus_Died(GameObject victim)
    {
        crops.Remove(victim);
        OnCropDied(victim);
    }

    private void OnCropDied(GameObject victim)
    {
        if (CropDied != null)
        {
            CropDied(victim);
        }
    }

    // Get the crop that's closest to a certain position.
    public GameObject GetClosestViableCrop(Vector3 position)
    {
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject crop in crops)
        {
            if (crop.GetComponent<PlantStatus>().GetIsGrown())
            {
                float distance = Vector3.Distance(position, crop.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = crop;
                }
            }
        }
        return closestObject;
    }

    public int GetViableCropCount()
    {
        int count = 0;
        foreach (GameObject crop in crops)
        {
            if (crop.GetComponent<PlantStatus>().GetIsGrown())
            {
                count += 1;
            }
        }
        return count;
    }

    protected override void EventsSubscribe()
    {
        foreach (GameObject crop in crops)
        {
            if (crop != null)
            {
                crop.GetComponent<PlantStatus>().Died += PlantStatus_Died;
            }
        }
    }

    protected override void EventsUnsubscribe()
    {
        foreach (GameObject crop in crops)
        {
            if (crop != null)
            {
                crop.GetComponent<PlantStatus>().Died -= PlantStatus_Died;
            }
        }
    }
}