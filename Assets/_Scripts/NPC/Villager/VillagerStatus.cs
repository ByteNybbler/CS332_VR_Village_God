// Author(s): Paul Calande
// Villager component for events and status.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Health))]
[RequireComponent (typeof (VillagerMovement))]
public class VillagerStatus : LateInit
{
    [Tooltip("Reference to the ability controller object.")]
    public GameObject instanceAbilityController;
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
        compVillagerMovement = GetComponent<VillagerMovement>();
    }

    public override void Init()
    {
        compPlantFood = instanceAbilityController.GetComponent<PlantFood>();
        StartCoroutine(HungerTimer());

        base.Init();
    }

    // Set the closest viable crop to the villager as the target.
    // Make sure viable crops exist before calling this function!
    // You will have a null reference otherwise!
    public void SetCropTargetToClosest()
    {
        compVillagerMovement.cropTarget = compPlantFood.GetClosestViableCrop(transform.position);
        compVillagerMovement.destinationIsFood = true;
    }

    protected override void EventsSubscribe()
    {
        npchealth.Died += NPCHealth_Died;
        compPlantFood.CropDied += PlantFood_CropDied;
        //compPlantFood.CropGrown += PlantFood_CropGrown;
        //health.Damaged += Health_Damaged;
    }
    protected override void EventsUnsubscribe()
    {
        npchealth.Died -= NPCHealth_Died;
        compPlantFood.CropDied -= PlantFood_CropDied;
        //compPlantFood.CropGrown -= PlantFood_CropGrown;
        //health.Damaged -= Health_Damaged;
    }

    IEnumerator HungerTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(hungerTime);
            health.Damage(1);
        }
    }

    // Health death event payload.
    private void NPCHealth_Died()
    {
        // Invoke the villager death event.
        OnDied(gameObject);
    }

    // Callback function for when a plant dies.
    private void PlantFood_CropDied(GameObject victim)
    {
        // If the plant is the villager's current target...
        if (victim == compVillagerMovement.cropTarget)
        {
            // Check if there are any crops left and act accordingly.
            if (compPlantFood.GetViableCropCount() == 0)
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

    /*
    // Crop grown event callback.    
    private void PlantFood_CropGrown(GameObject crop)
    {
        // If the villager isn't heading to food and doesn't have full health...
        if (compVillagerMovement.destinationIsFood == false && !health.IsHealthFull())
        {
            // That must mean there were no crops in the scene.
            // Head towards the one that was just created.
            compVillagerMovement.cropTarget = crop;
            compVillagerMovement.destinationIsFood = true;
        }
    }
    */

    /*
    // Health damaged event callback.
    private void Health_Damaged(int amount)
    {
        // If the villager is not heading towards food yet and crops are present...
        if (compVillagerMovement.destinationIsFood == false && compPlantFood.GetViableCropCount() != 0)
        {
            // Head towards food.
            SetCropTargetToClosest();
        }
    }
    */

    // Event invocations.
    private void OnDied(GameObject obj)
    {
        if (Died != null)
        {
            Died(obj);
        }
    }
}