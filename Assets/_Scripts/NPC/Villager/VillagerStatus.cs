// Author(s): Paul Calande
// Villager component for events and status.

// Comment out the following line to prevent console messages involving villager destruction.
#define VILLAGERSTATUS_DESTROY_DEBUG

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

    // Hunger timer.
    private float hungerTimer;

    // Component references.
    private Health health;
    private NPCHealth npchealth;
    private PlantFood compPlantFood;
    private VillagerMovement compVillagerMovement;
    private TimeScale ts;

    private void Awake()
    {
        health = GetComponent<Health>();
        npchealth = GetComponent<NPCHealth>();
        npchealth.Died += NPCHealth_Died;
        compVillagerMovement = GetComponent<VillagerMovement>();
        ts = GetComponent<TimeScale>();
    }

    public void Start()
    {
        hungerTimer = hungerTime;
        if (foodController != null)
        {
#if VILLAGERSTATUS_DESTROY_DEBUG
            Debug.Log("VillagerStatus " + GetInstanceID() + " subscribed to FoodController.");
#endif
            foodController.CropDied += FoodController_CropDied;
            foodController.CropGrown += FoodController_CropGrown;
        }
    }

    private void OnDestroy()
    {
#if VILLAGERSTATUS_DESTROY_DEBUG
        Debug.Log("VillagerStatus OnDestroy() called.");
#endif
        if (npchealth != null)
        {
            npchealth.Died -= NPCHealth_Died;
        }
        if (foodController != null)
        {
#if VILLAGERSTATUS_DESTROY_DEBUG
            Debug.Log("VillagerStatus OnDestroy(): Unsubscribed from foodController.");
#endif
            foodController.CropDied -= FoodController_CropDied;
            foodController.CropGrown -= FoodController_CropGrown;
        }
    }

    private void Update()
    {
        float timePassed = ts.GetTimePassed();
        hungerTimer -= timePassed;
        while (hungerTimer <= 0f)
        {
            hungerTimer += hungerTime;
            health.Damage(1);
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