// Author(s): Paul Calande
// Villager AI.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class VillagerMovement : LateInit
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
    [Tooltip("Reference to the shrine instance.\n" +
    "This reference is populated automatically by the Village class.")]
    public GameObject shrineObject;
    [Tooltip("Reference to the food controller instance.")]
    public GameObject instanceFoodController;
    [Tooltip("Whether the agent is currently heading towards food. Overrides the other destinations.")]
    public bool destinationIsFood = false;
    [Tooltip("How close the villager must be to the crop in order to eat it.")]
    public float cropEatDistance;

    // How many seconds the villager has spent idling.
    private float timeIdled = 0f;
    // House position.
    private Vector3 housePosition;
    // Shrine position.
    private Vector3 shrinePosition;
    // Whether the agent's current destination is the shrine.
    private bool destinationIsShrine = true;
    // The current crop that the villager is heading towards.
    private GameObject cropTarget = null;

    // Component references.
    private NavMeshAgent agent;
    private Shrine shrine;
    private PlantFood compPlantFood;
    private Health compHealth;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        compHealth = GetComponent<Health>();
    }

    public override void Init()
    {
        // Now that we are in the Start event, we can safely fetch the shrine's component.
        shrine = shrineObject.GetComponent<Shrine>();
        // Get the house and shrine positions.
        housePosition = houseTransform.position;
        shrinePosition = shrineObject.transform.position;
        // Set the agent's destination to the shrine first.
        agent.destination = shrinePosition;
        // Get the PlantFood component.
        compPlantFood = instanceFoodController.GetComponent<PlantFood>();

        base.Init();
    }

    protected override void OnEnable()
    {
        if (isInitialized)
        {
            if (destinationIsShrine)
            {
                agent.destination = shrinePosition;
            }
            else
            {
                agent.destination = housePosition;
            }
        }
        base.OnEnable();
    }

    protected override void EventsSubscribe()
    {
        compPlantFood.CropDied += PlantFood_CropDied;
    }
    protected override void EventsUnsubscribe()
    {
        compPlantFood.CropDied -= PlantFood_CropDied;
    }

    private void PlantFood_CropDied(GameObject victim)
    {
        if (victim == cropTarget)
        {
            SetCropTargetToClosest();
        }
    }

    // Set the closest crop to the villager as the target.
    public void SetCropTargetToClosest()
    {
        cropTarget = compPlantFood.GetClosestViableCrop(transform.position);
        if (cropTarget == null)
        {
            StopHuntingForFood();
        }
    }

    private void StopHuntingForFood()
    {
        destinationIsFood = false;
        timeIdled = 0f;
    }

    // Get the distance away from the agent's destination.
    private float GetDestinationDistance()
    {
        return Vector3.Distance(transform.position, agent.destination);
    }

    private void Update()
    {
        // If the villager is heading towards food...
        if (destinationIsFood)
        {
            // GET TO THE FOOD ROY
            /*
            if (cropTarget == null)
            {
                SetCropTargetToClosest();
            }
            else
            {
            */
            agent.destination = cropTarget.transform.position;
            if (GetDestinationDistance() < cropEatDistance)
            {
                // Eat a bit of the crop.
                cropTarget.GetComponent<PlantStatus>().DecreaseHealth();
                // Restore all health.
                compHealth.FullHeal();
                // Return to normal activities.
                StopHuntingForFood();
            }
            //}
        }
        // If the villager is NOT heading towards food, head towards other things.
        else
        {
            // If the villager is targeting the shrine...
            if (destinationIsShrine)
            {
                // If the villager is within shrine charging distance...
                if (GetDestinationDistance() < shrineChargeDistance)
                {
                    // Increase the idle time.
                    timeIdled += Time.deltaTime;
                    // Charge up the shrine.
                    shrine.IncreaseChargeSeconds(Time.deltaTime, transform.position);
                }
                // If the villager has idled at the shrine for long enough...
                if (timeIdled > shrineIdleTime)
                {
                    // Reset the idle time and start heading to the house.
                    timeIdled = 0f;
                    destinationIsShrine = false;
                    agent.destination = housePosition;
                }
            }
            // If the villager is targeting the house...
            else
            {
                // If the villager is close enough to the house destination to be considered idling...
                if (GetDestinationDistance() < houseDestinationDistance)
                {
                    // Increase the idle time.
                    timeIdled += Time.deltaTime;
                }
                // If the villager has idled at the house for long enough...
                if (timeIdled > houseIdleTime)
                {
                    // Reset the idle time and start heading to the shrine.
                    timeIdled = 0f;
                    destinationIsShrine = true;
                    agent.destination = shrinePosition;
                }
            }
        }
    }
}