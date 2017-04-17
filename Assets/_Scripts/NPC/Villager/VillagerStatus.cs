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
    [Tooltip("The rate at which the villager gets hungrier. Larger = faster.")]
    public float hungerRate;
    [Tooltip("The percentage of health the villager has remaining before it flees to a crop.")]
    public float fleeToCropHealthRatio;

    public delegate void DiedHandler(GameObject victim);
    public event DiedHandler Died;

    // How much health the villager should have left before fleeing to a crop.
    private float fleeToCropAtThisHealth;

    // Component references.
    private Health health;
    private NPCHealth npchealth;
    private PlantFood compPlantFood;
    private VillagerMovement compVillagerMovement;

    private void Awake()
    {
        health = GetComponent<Health>();
        npchealth = GetComponent<NPCHealth>();
        fleeToCropAtThisHealth = health.GetMaxHealth() * fleeToCropHealthRatio;
        compVillagerMovement = GetComponent<VillagerMovement>();
    }

    public override void Init()
    {
        compPlantFood = instanceAbilityController.GetComponent<PlantFood>();

        base.Init();
    }

    private void Update()
    {
        // Hunger damages the villager constantly over time.
        health.Damage(Time.deltaTime * hungerRate);

        // If the villager needs food and food is present, set the food as a target.
        if (compVillagerMovement.destinationIsFood == false)
        {
            if (health.GetCurrentHealth() < fleeToCropAtThisHealth && compPlantFood.GetViableCropCount() != 0)
            {
                SetCropTargetToClosest();
            }
        }
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
    }
    protected override void EventsUnsubscribe()
    {
        npchealth.Died -= NPCHealth_Died;
        compPlantFood.CropDied -= PlantFood_CropDied;
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

    private void OnDied(GameObject obj)
    {
        if (Died != null)
        {
            Died(obj);
        }
    }
}