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
public class Village : LateInit
{
    [Tooltip("Reference to the shrine GameObject.")]
    public GameObject shrineObject;
    [Tooltip("Reference to the villager prefab.")]
    public GameObject villagerPrefab;
    [Tooltip("Reference to the ability controller instance.")]
    public GameObject instanceAbilityController;

    public delegate void VillagerDiedHandler(GameObject victim);
    public event VillagerDiedHandler VillagerDied;
    public delegate void AllVillagersDiedHandler();
    public event AllVillagersDiedHandler AllVillagersDied;

    // A list of villagers.
    private List<GameObject> villagers = new List<GameObject>();
    // Component references.
    private KeyPoints kp;

    private void Awake()
    {
        kp = GetComponent<KeyPoints>();
    }

    public override void Init()
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
            // Assign the house to the villager.
            vm.houseTransform = trans;
            // Assign the shrine to the villager.
            //Debug.Log("Shrine object: " + shrineObject);
            vm.shrineObject = shrineObject;
            // Add the villager to the list of existing villagers.
            villagers.Add(newVillager);
            // Pass the food controller to the villager.
            vm.instanceAbilityController = instanceAbilityController;
            vs.instanceAbilityController = instanceAbilityController;
            // Initialization time!
            vm.Init();
            vs.Init();
        }

        base.Init();
    }

    public GameObject GetRandomVillager()
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

    // Villager death event payload.
    private void VillagerStatus_Died(GameObject victim)
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

    private void OnVillagerDied(GameObject victim)
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
    
    protected override void EventsSubscribe()
    {
        foreach (GameObject villager in villagers)
        {
            if (villager != null)
            {
                villager.GetComponent<VillagerStatus>().Died += VillagerStatus_Died;
            }
        }
    }
    protected override void EventsUnsubscribe()
    {
        foreach (GameObject villager in villagers)
        {
            if (villager != null)
            {
                villager.GetComponent<VillagerStatus>().Died -= VillagerStatus_Died;
            }
        }
    }
}