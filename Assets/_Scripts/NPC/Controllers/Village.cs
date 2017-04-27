// Author(s): Paul Calande
// This class encompasses the whole village and spawns the villagers.
// The GameObject reference in the KeyPoints component will determine the house idle positions.
// One villager will be spawned at each of these house idle positions.

// Comment out the following line to prevent the house count from being printed.
//#define PRINT_HOUSE_COUNT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (KeyPoints))]
public class Village : MonoBehaviour
{
    [Tooltip("Reference to the shrine.")]
    public Shrine shrine;
    [Tooltip("Reference to the villager prefab.")]
    public GameObject villagerPrefab;
    [Tooltip("Reference to the food controller instance.")]
    public FoodController foodController;
    [Tooltip("How many seconds the isBeingAttacked state lasts.")]
    public float attackTime = 10f;

    // A villager died.
    public delegate void VillagerDiedHandler(VillagerStatus victim);
    public event VillagerDiedHandler VillagerDied;
    // All villagers died.
    public delegate void AllVillagersDiedHandler();
    public event AllVillagersDiedHandler AllVillagersDied;
    // Village is attacked.
    public delegate void AttackStartedHandler();
    public event AttackStartedHandler AttackStarted;
    // Attack on village ended.
    public delegate void AttackEndedHandler();
    public event AttackEndedHandler AttackEnded;

    // A list of villagers.
    private List<VillagerStatus> villagers = new List<VillagerStatus>();
    // Whether the village is currently being attacked by enemies.
    private bool isBeingAttacked = false;
    // Timer that sets isBeingAttacked back to false when it hits 0.
    private float attackTimer = 0f;

    // Component references.
    private KeyPoints kp;
    private TimeScale ts;

    private void Awake()
    {
        kp = GetComponent<KeyPoints>();
        ts = GetComponent<TimeScale>();
    }

    private void Start()
    {
        List<Transform> housePositions = kp.GetKeyPoints();

#if PRINT_HOUSE_COUNT
        Debug.Log("Number of houses: " + housePositions.Count);
#endif

        // All of the villagers are spawned here.
        // Loop for each transform.
        foreach (Transform trans in housePositions)
        {
            // Instantiate a new villager.
            GameObject newVillager = Instantiate(villagerPrefab, trans.position, Quaternion.identity);
            // Get the villager's relevant components for assignment operations.
            VillagerMovement vm = newVillager.GetComponent<VillagerMovement>();
            VillagerStatus vs = newVillager.GetComponent<VillagerStatus>();
            TimeControllable tc = newVillager.GetComponent<TimeControllable>();
            // Assign the house to the villager.
            vm.houseTransform = trans;
            // Pass the shrine to the villager.
            vm.shrine = shrine;
            // Pass the food controller reference to the villager.
            vs.foodController = foodController;
            // Pass village reference to the villager.
            //vs.village = this;
            // Subscribe to the villager's events.
            vs.Died += VillagerStatus_Died;
            vs.AttackedByEnemy += VillagerStatus_AttackedByEnemy;
            // Pass the time controller to the villager.
            tc.timeController = GetComponent<TimeControllable>().timeController;
            TimeScale.PassTimeScale(newVillager, ts);
            // Add the villager to the list of existing villagers.
            villagers.Add(vs);
        }
    }

    private void OnDestroy()
    {
        foreach (VillagerStatus villager in villagers)
        {
            if (villager != null)
            {
                villager.Died -= VillagerStatus_Died;
                villager.AttackedByEnemy -= VillagerStatus_AttackedByEnemy;
            }
        }
    }

    private void Update()
    {
        if (attackTimer > 0f)
        {
            float timePassed = ts.GetTimePassed();
            attackTimer -= timePassed;
            if (attackTimer <= 0f)
            {
                AttackStateDisable();
            }
        }
    }

    public void FullHealAllVillagers(Health.Type type)
    {
        foreach (VillagerStatus villager in villagers)
        {
            villager.FullHeal(type);
        }
    }

    public VillagerStatus GetRandomVillager()
    {
        if (villagers.Count == 0)
        {
            return null;
        }
        else
        {
            return villagers[Random.Range(0, villagers.Count)];
        }
    }

    // Get the closest villager to a position that's within a certain distance.
    // Returns null if no villager is found.
    public VillagerStatus GetClosestVillager(Vector3 position, float maxDistance)
    {
        // The closest villager so far.
        VillagerStatus closestVillager = null;
        // Iterate through all of the villagers.
        foreach (VillagerStatus villager in villagers)
        {
            // Calculate the distance between the villager and the point.
            float distance = Vector3.Distance(villager.transform.position, position);
            // If the distance is the closest one so far...
            if (distance < maxDistance)
            {
                // Update the smallest distance so far (between the villagers and the position).
                maxDistance = distance;
                // This villager is the closest one so far!
                closestVillager = villager;
            }
        }
        return closestVillager;
    }

    private void AttackStateEnable()
    {
        if (isBeingAttacked == false)
        {
            isBeingAttacked = true;
            OnAttackStarted();
            // Start the attack timer.
            attackTimer = attackTime;
        }
    }

    private void AttackStateDisable()
    {
        if (isBeingAttacked == true)
        {
            isBeingAttacked = false;
            OnAttackEnded();
        }
    }

    // Villager death event payload.
    private void VillagerStatus_Died(VillagerStatus victim)
    {
        //Debug.Log("Whoa! A villager DIED!!!");
        // Remove the villager from the villagers list.
        villagers.Remove(victim);
        // Notify subscribers about the villager's death.
        OnVillagerDied(victim);
        // If there are no villagers left, game over.
        if (villagers.Count == 0)
        {
            OnAllVillagersDied();
        }
    }

    private void VillagerStatus_AttackedByEnemy()
    {
        AttackStateEnable();
    }

    // Event invocations.
    private void OnVillagerDied(VillagerStatus victim)
    {
        if (VillagerDied != null)
        {
            VillagerDied(victim);
        }
    }
    private void OnAllVillagersDied()
    {
        if (AllVillagersDied != null)
        {
            AllVillagersDied();
        }
    }
    private void OnAttackStarted()
    {
        if (AttackStarted != null)
        {
            AttackStarted();
        }
    }
    private void OnAttackEnded()
    {
        if (AttackEnded != null)
        {
            AttackEnded();
        }
    }
}