// Author(s): Paul Calande
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    [Tooltip("Crop prefab reference.")]
    public GameObject prefabCrop;

    public delegate void CropDiedHandler(GameObject victim);
    public event CropDiedHandler CropDied;
    public delegate void CropGrownHandler(GameObject crop);
    public event CropGrownHandler CropGrown;

    // List of existing crops.
    private List<GameObject> crops = new List<GameObject>();

    public void CreateCrop(Vector3 location)
    {
        // Instantiate the crop.
        GameObject cropInstance = Instantiate(prefabCrop, location, Quaternion.identity);
        // Pass the time controller reference to the crop.
        TimeControllable tc = cropInstance.GetComponent<TimeControllable>();
        tc.timeController = GetComponent<TimeControllable>().timeController;
        // Subscribe to the crop and add it to the crops list.
        SubscribeToCrop(cropInstance);
        crops.Add(cropInstance);
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

    private void OnEnable()
    {
        foreach (GameObject crop in crops)
        {
            if (crop != null)
            {
                SubscribeToCrop(crop);
            }
        }
    }

    private void OnDisable()
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