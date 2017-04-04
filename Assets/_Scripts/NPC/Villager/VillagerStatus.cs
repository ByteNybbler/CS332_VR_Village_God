// Author(s): Paul Calande
// Villager component for events and status.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Health))]
[RequireComponent (typeof (VillagerMovement))]
public class VillagerStatus : LateInit
{
    [Tooltip("Reference to the food controller object.")]
    public GameObject instanceFoodController;
    [Tooltip("The rate at which the villager gets hungrier. Larger = faster.")]
    public float hungerRate;
    [Tooltip("The percentage of health the villager has remaining before it flees to a crop.")]
    public float fleeToCropHealthRatio;

    public delegate void DiedHandler(GameObject victim);
    public event DiedHandler Died;

    private float fleeToCropAtThisHealth;

    // Component references.
    private Health health;
    private PlantFood compPlantFood;
    private VillagerMovement compVillagerMovement;

    private void Awake()
    {
        health = GetComponent<Health>();
        fleeToCropAtThisHealth = health.healthMax * fleeToCropHealthRatio;
        compVillagerMovement = GetComponent<VillagerMovement>();
    }

    public override void Init()
    {
        compPlantFood = instanceFoodController.GetComponent<PlantFood>();

        base.Init();
    }

    private void Update()
    {
        health.Damage(Time.deltaTime * hungerRate);
        if (compVillagerMovement.destinationIsFood == false)
        {
            if (health.healthCurrent < fleeToCropAtThisHealth && compPlantFood.GetViableCropCount() != 0)
            {
                compVillagerMovement.destinationIsFood = true;
                compVillagerMovement.SetCropTargetToClosest();
            }
        }


        if (health.healthCurrent < fleeToCropAtThisHealth)
        {
            if (compPlantFood.GetViableCropCount() != 0)
            {
                compVillagerMovement.destinationIsFood = true;
            }
            else
            {
                compVillagerMovement.destinationIsFood = false;
            }
        }
        else
        {
            compVillagerMovement.destinationIsFood = false;
        }
    }

    protected override void EventsSubscribe()
    {
        health.Died += Health_Died;
    }
    protected override void EventsUnsubscribe()
    {
        health.Died -= Health_Died;
    }

    // Health death event payload.
    private void Health_Died()
    {
        // Call the villager death event.
        OnDied(gameObject);
        // Destroy this villager.
        Destroy(gameObject);
    }

    private void OnDied(GameObject obj)
    {
        if (Died != null)
        {
            Died(obj);
        }
    }
}