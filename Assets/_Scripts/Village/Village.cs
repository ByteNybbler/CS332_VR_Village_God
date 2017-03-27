// Author(s): Paul Calande
// This class encompasses the whole village and spawns the villagers.
// The GameObject reference in the KeyPoints component will determine the house idle positions.
// One villager will be spawned at each of these house idle positions.

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

        Debug.Log("Number of houses: " + housePositions.Count);

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

    public GameObject GetRandomVillager()
    {
        return villagers[Random.Range(0, villagers.Count)];
    }
}