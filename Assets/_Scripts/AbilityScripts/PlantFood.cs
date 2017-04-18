// Author(s): Hunter Golden, Paul Calande
// Script for planting food.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantFood : LateInit
{
    [Tooltip("Crop prefab reference.")]
    public GameObject prefabCrop;
    [Tooltip("The maximum number of crops that can exist in the scene at once.")]
    public int maxNumberOfCrops = 10;
    [Tooltip("How many faith points it costs to plant a crop.")]
    public int cost = 10;

    public delegate void CropDiedHandler(GameObject victim);
    public event CropDiedHandler CropDied;
    public delegate void CropGrownHandler(GameObject crop);
    public event CropGrownHandler CropGrown;

    // List of existing crops.
    private List<GameObject> crops = new List<GameObject>();

    // Component references.
    private AbilityInterface cai;

    private void Awake()
    {
        cai = GetComponent<AbilityInterface>();
    }

    public void MakeFood()
    {
        if (crops.Count < maxNumberOfCrops)
        {
            Vector3 location;
            if (cai.castRayLeftController.Cast(out location))
            {
                if (cai.shrine.SpendPoints(cost, location))
                {
                    GameObject cropInstance = Instantiate(prefabCrop, location, Quaternion.identity);
                    SubscribeToCrop(cropInstance);
                    crops.Add(cropInstance);
                }
            }
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

    private void SubscribeToCrop(GameObject crop)
    {
        PlantStatus ps = crop.GetComponent<PlantStatus>();
        ps.Died += PlantStatus_Died;
        ps.Grown += PlantStatus_Grown;
    }
    private void UnsubscribeFromCrop(GameObject crop)
    {
        PlantStatus ps = crop.GetComponent<PlantStatus>();
        ps.Died -= PlantStatus_Died;
        ps.Grown -= PlantStatus_Grown;
    }

    protected override void EventsSubscribe()
    {
        foreach (GameObject crop in crops)
        {
            if (crop != null)
            {
                SubscribeToCrop(crop);
            }
        }
    }
    protected override void EventsUnsubscribe()
    {
        foreach (GameObject crop in crops)
        {
            if (crop != null)
            {
                UnsubscribeFromCrop(crop);
            }
        }
    }

    // Event callbacks.
    private void PlantStatus_Died(GameObject victim)
    {
        crops.Remove(victim);
        OnCropDied(victim);
    }

    private void PlantStatus_Grown(GameObject crop)
    {
        OnCropGrown(crop);
    }

    // Event invocations.
    private void OnCropDied(GameObject victim)
    {
        if (CropDied != null)
        {
            CropDied(victim);
        }
    }
    private void OnCropGrown(GameObject crop)
    {
        if (CropGrown != null)
        {
            CropGrown(crop);
        }
    }
}