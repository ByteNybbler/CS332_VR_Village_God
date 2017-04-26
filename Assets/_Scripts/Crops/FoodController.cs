// Author(s): Paul Calande
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    [Tooltip("Crop prefab reference.")]
    public GameObject prefabCrop;
    [Tooltip("The Audio Source to use to play sound.")]
    public AudioSource audioSource;
    [Tooltip("The Audio Clip to play when a crop is planted.")]
    public AudioClip soundPlant;

    public delegate void CropDiedHandler(PlantStatus victim);
    public event CropDiedHandler CropDied;
    public delegate void CropGrownHandler(PlantStatus crop);
    public event CropGrownHandler CropGrown;

    // List of existing crops.
    private List<PlantStatus> crops = new List<PlantStatus>();

    public void CreateCrop(Vector3 location)
    {
        // Instantiate the crop.
        GameObject cropInstance = Instantiate(prefabCrop, location, Quaternion.identity);
        // Pass the time controller reference to the crop.
        TimeControllable tc = cropInstance.GetComponent<TimeControllable>();
        tc.timeController = GetComponent<TimeControllable>().timeController;
        TimeScale.PassTimeScale(cropInstance, gameObject);
        // Pass the food controller reference to the crop.
        PlantStatus ps = cropInstance.GetComponent<PlantStatus>();
        ps.foodController = this;
        // Play crop planting sound.
        audioSource.PlayOneShot(soundPlant);
    }

    // Get the crop that's closest to a certain position.
    public PlantStatus GetClosestViableCrop(Vector3 position)
    {
        PlantStatus closestObject = null;
        float closestDistance = Mathf.Infinity;
        foreach (PlantStatus crop in crops)
        {
            if (crop.GetIsGrown())
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
        foreach (PlantStatus crop in crops)
        {
            if (crop.GetIsGrown())
            {
                count += 1;
            }
        }
        return count;
    }

    public void AddCrop(PlantStatus crop)
    {
        crop.Died += PlantStatus_Died;
        crop.Grown += PlantStatus_Grown;
        crops.Add(crop);
    }
    public void RemoveCrop(PlantStatus crop)
    {
        crop.Died -= PlantStatus_Died;
        crop.Grown -= PlantStatus_Grown;
        crops.Remove(crop);
    }

    private void OnDestroy()
    {
        foreach (PlantStatus crop in crops)
        {
            if (crop != null)
            {
                RemoveCrop(crop);
            }
        }
    }

    // Event callbacks.
    private void PlantStatus_Died(PlantStatus victim)
    {
        crops.Remove(victim);
        OnCropDied(victim);
    }

    private void PlantStatus_Grown(PlantStatus crop)
    {
        OnCropGrown(crop);
    }

    // Event invocations.
    private void OnCropDied(PlantStatus victim)
    {
        if (CropDied != null)
        {
            CropDied(victim);
        }
    }
    private void OnCropGrown(PlantStatus crop)
    {
        if (CropGrown != null)
        {
            CropGrown(crop);
        }
    }
}