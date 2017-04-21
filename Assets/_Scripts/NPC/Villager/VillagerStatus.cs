// Author(s): Paul Calande
// Villager component for events and status.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Health))]
[RequireComponent (typeof (VillagerMovement))]
public class VillagerStatus : MonoBehaviour
{
    [Tooltip("Reference to the food controller.")]
    public FoodController foodController;
    [Tooltip("The number of seconds between each instance of the villager taking damage due to hunger.")]
    public float hungerTime;

    public delegate void DiedHandler(GameObject victim);
    public event DiedHandler Died;

    // Component references.
    private Health health;
    private NPCHealth npchealth;
    private PlantFood compPlantFood;
    private VillagerMovement compVillagerMovement;

    private void Awake()
    {
        health = GetComponent<Health>();
        npchealth = GetComponent<NPCHealth>();
        npchealth.Died += NPCHealth_Died;
        compVillagerMovement = GetComponent<VillagerMovement>();
        StartCoroutine(HungerTimer());
    }

    public void Start()
    {
        if (foodController != null)
        {
            foodController.CropDied += FoodController_CropDied;
            foodController.CropGrown += FoodController_CropGrown;
        }
    }

    private void OnDestroy()
    {
        Debug.Log("VillagerStatus OnDestroy() called.");
        if (npchealth != null)
        {
            npchealth.Died -= NPCHealth_Died;
        }
        if (foodController != null)
        {
            foodController.CropDied -= FoodController_CropDied;
            foodController.CropGrown -= FoodController_CropGrown;
        }
    }

    // Set the closest viable crop to the villager as the target.
    // Make sure viable crops exist before calling this function!
    // You will have a null reference otherwise!
    public void SetCropTargetToClosest()
    {
        compVillagerMovement.cropTarget = foodController.GetClosestViableCrop(transform.position);
        compVillagerMovement.destinationIsFood = true;
    }

    IEnumerator HungerTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(hungerTime);
            health.Damage(1);
        }
    }

    // Callback function for when health runs out.
    private void NPCHealth_Died()
    {
        // Invoke the villager death event.
        OnDied(gameObject);
    }

    // Callback function for when a plant dies.
    private void FoodController_CropDied(GameObject victim)
    {
        // If the plant is the villager's current target...
        if (victim == compVillagerMovement.cropTarget)
        {
            // Check if there are any crops left and act accordingly.
            if (foodController.GetViableCropCount() == 0)
            {
                // No crops left. No target. Stop looking for food.
                compVillagerMovement.cropTarget = null;
                compVillagerMovement.destinationIsFood = false;
            }
            else
            {
                SetCropTargetToClosest();
            }
        }
    }

    private void FoodController_CropGrown(GameObject crop)
    {
        if (compVillagerMovement.cropTarget != null)
        {
            // If the new crop is closer than the current crop target, target the new crop.
            if (Vector3.Distance(transform.position, crop.transform.position)
                < Vector3.Distance(transform.position, compVillagerMovement.cropTarget.transform.position))
            {
                compVillagerMovement.cropTarget = crop;
            }
        }
    }

    // Event invocations.
    private void OnDied(GameObject obj)
    {
        if (Died != null)
        {
            Died(obj);
        }
    }
}