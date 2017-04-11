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
    [Tooltip("Rising text creator for healing.")]
    public RisingTextCreator rtcHealed;
    [Tooltip("Rising text creator for damage.")]
    public RisingTextCreator rtcDamaged;

    public delegate void DiedHandler(GameObject victim);
    public event DiedHandler Died;

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
        fleeToCropAtThisHealth = health.healthMax * fleeToCropHealthRatio;
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
            if (health.healthCurrent < fleeToCropAtThisHealth && compPlantFood.GetViableCropCount() != 0)
            {
                compVillagerMovement.SetCropTargetToClosest();
            }
        }
    }

    protected override void EventsSubscribe()
    {
        npchealth.Died += NPCHealth_Died;
    }
    protected override void EventsUnsubscribe()
    {
        npchealth.Died -= NPCHealth_Died;
    }

    // Health death event payload.
    private void NPCHealth_Died()
    {
        // Invoke the villager death event.
        OnDied(gameObject);
    }

    private void OnDied(GameObject obj)
    {
        if (Died != null)
        {
            Died(obj);
        }
    }
}