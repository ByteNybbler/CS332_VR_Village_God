// Author(s): Paul Calande
// Villager AI.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class VillagerMovement : MonoBehaviour
{
    [Tooltip("The number of seconds to stay at the house.")]
    public float houseIdleTime;
    [Tooltip("The number of seconds to stay at the shrine.")]
    public float shrineIdleTime;
    [Tooltip("How close the villager must be to the shrine in order to charge it.")]
    public float shrineChargeDistance;
    [Tooltip("How close the villager must be to the house to be considered idling.")]
    public float houseDestinationDistance;
    [Tooltip("The transform of the villager's house.")]
    public Transform houseTransform;
    [Tooltip("Reference to the shrine component.")]
    public Shrine shrine;
    [Tooltip("Whether the agent is currently heading towards food. Overrides the other destinations.")]
    public bool destinationIsFood = false;
    [Tooltip("How close the villager must be to a crop in order to eat it.")]
    public float cropEatDistance;
    [Tooltip("The current crop that the villager is heading towards.")]
    public GameObject cropTarget = null;
    [Tooltip("How many seconds the villager has to wait between eating crops.")]
    public float eatCooldownTime;

    // How many seconds the villager has spent idling.
    private float timeIdled = 0f;
    // House position.
    private Vector3 housePosition;
    // Shrine position.
    private Vector3 shrinePosition;
    // Whether the agent's current destination is the shrine.
    private bool destinationIsShrine = true;
    // Whether the villager can currently eat crops.
    private bool canEat = true;
    // Eating cooldown timer.
    private float eatCooldownTimer;

    // Component references.
    private NavMeshAgent agent;
    private Health compHealth;
    private PlantFood compPlantFood;
    private VillagerStatus compVillagerStatus;
    private TimeScale ts;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        compHealth = GetComponent<Health>();
        compVillagerStatus = GetComponent<VillagerStatus>();
        ts = GetComponent<TimeScale>();
        eatCooldownTimer = eatCooldownTime;
    }

    // Get the distance the agent is away from its destination.
    private float GetDestinationDistance()
    {
        return Vector3.Distance(transform.position, agent.destination);
    }

    private void Update()
    {
        float timePassed = ts.GetTimePassed();
        if (!canEat)
        {
            eatCooldownTimer -= timePassed;
            if (eatCooldownTimer < 0f)
            {
                eatCooldownTimer = eatCooldownTime;
                canEat = true;
            }
        }
        // If the villager is heading towards food...
        if (destinationIsFood)
        {
            // GET TO THE FOOD ROY
            agent.destination = cropTarget.transform.position;
            // If the villager is close enough to the crop...
            if ((GetDestinationDistance() < cropEatDistance) && canEat)
            {
                // Eat a bit of the crop.
                cropTarget.GetComponent<PlantStatus>().DecreaseHealth();
                // Restore health.
                compHealth.Heal(1);
                // Temporarily prevent the villager from eating another crop.
                canEat = false;
                // If health is full, return to normal activities.
                if (compHealth.IsHealthFull())
                {
                    cropTarget = null;
                    destinationIsFood = false;
                }
            }
        }
        // If the villager is NOT heading towards food, head towards other things.
        else
        {
            // If the villager is targeting the shrine...
            if (destinationIsShrine)
            {
                agent.destination = shrine.transform.position;
                // If the villager is within shrine charging distance...
                if (GetDestinationDistance() < shrineChargeDistance)
                {
                    // Increase the idle time.
                    timeIdled += timePassed;
                    // Charge up the shrine.
                    shrine.IncreaseChargeSeconds(timePassed, transform.position);
                }
                // If the villager has idled at the shrine for long enough...
                if (timeIdled > shrineIdleTime)
                {
                    // Reset the idle time and stop targeting the shrine.
                    timeIdled = 0f;
                    destinationIsShrine = false;
                    CheckIfWantsCrop();
                }
            }
            // If the villager is targeting the house...
            else
            {
                agent.destination = houseTransform.position;
                // If the villager is close enough to the house destination to be considered idling...
                if (GetDestinationDistance() < houseDestinationDistance)
                {
                    // Increase the idle time.
                    timeIdled += timePassed;
                }
                // If the villager has idled at the house for long enough...
                if (timeIdled > houseIdleTime)
                {
                    // Reset the idle time and start heading to the shrine.
                    timeIdled = 0f;
                    destinationIsShrine = true;
                    CheckIfWantsCrop();
                }
            }
        }
    }

    private void CheckIfWantsCrop()
    {
        // If the villager needs health, target a crop now if possible.
        if (!compHealth.IsHealthFull() && compVillagerStatus.foodController.GetViableCropCount() != 0)
        {
            compVillagerStatus.SetCropTargetToClosest();
        }
    }
}