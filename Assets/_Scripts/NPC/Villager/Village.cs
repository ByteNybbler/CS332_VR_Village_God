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
    [Tooltip("Reference to the shrine GameObject.")]
    public GameObject shrineObject;
    [Tooltip("Reference to the villager prefab.")]
    public GameObject villagerPrefab;

    // A list of villagers.
    private List<GameObject> villagers = new List<GameObject>();
    // Component references.
    private KeyPoints kp;

    // All of the villagers are spawned from the Start method.
    private void Start()
    {
        kp = GetComponent<KeyPoints>();
        List<Transform> housePositions = kp.GetKeyPoints();

#if PRINT_HOUSE_COUNT
        Debug.Log("Number of houses: " + housePositions.Count);
#endif

        // Loop for each transform.
        foreach (Transform trans in housePositions)
        {
            // Instantiate a new villager.
            GameObject newVillager = Instantiate(villagerPrefab, trans.position, Quaternion.identity);
            // Get the villager's movement component for assignment operations.
            VillagerMovement vm = newVillager.GetComponent<VillagerMovement>();
            // Assign the house to the villager.
            vm.houseTransform = trans;
            // Assign the shrine to the villager.
            vm.shrineObject = shrineObject;
            // Add the villager to the list of existing villagers.
            villagers.Add(newVillager);
        }
    }

    private void OnEnable()
    {
        Health.OnDeath += SomeoneDied;
    }
    private void OnDisable()
    {
        Health.OnDeath -= SomeoneDied;
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

    // Return true if obj is a villager.
    public bool IsVillager(GameObject obj)
    {
        foreach (GameObject villager in villagers)
        {
            if (obj == villager)
            {
                return true;
            }
        }
        return false;
    }

    private void GameOver()
    {
        Debug.Log("Game over, baby.");
        // TODO: Stuff happens when you lose.
    }

    // Event payload.
    private void SomeoneDied(GameObject victim)
    {
        // If the victim is a villager...
        if (IsVillager(victim))
        {
            // Remove the villager from the villagers list.
            villagers.Remove(victim);
            // If there are no villagers left, game over.
            if (villagers.Count == 0)
            {
                GameOver();
            }
        }
    }
}