// Author(s): Paul Calande
// This class encompasses the whole village and spawns the villagers.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : MonoBehaviour
{
    // Reference to the shrine GameObject.
    public GameObject shrineObject;
    // Reference to the GameObject that has all the house idle positions as children.
    // One villager will be spawned at each of these house idle positions.
    public GameObject houseIdlePositions;
    // Reference to the villager prefab.
    public GameObject villagerPrefab;

    // How many villagers currently exist.
    [System.NonSerialized]
    public int numberOfVillagers = 0;

    // All of the villagers are spawned from the Start method.
    private void Start()
    {
        // Get the transforms of the house idle positions.
        Transform[] housePosArray = houseIdlePositions.transform.GetComponentsInChildren<Transform>();
        // Loop for each transform.
        foreach (Transform trans in housePosArray)
        {
            // If this transform isn't the parent object...
            if (trans.gameObject != houseIdlePositions)
            {
                // Instantiate a new villager.
                GameObject newVillager = Instantiate(villagerPrefab, trans.position, Quaternion.identity);
                // Get the villager's movement component for assignment operations.
                VillagerMovement vm = newVillager.GetComponent<VillagerMovement>();
                // Assign the house to the villager.
                vm.houseTransform = trans;
                // Assign the shrine to the villager.
                vm.shrineObject = shrineObject;
                // Increment the villager count.
                numberOfVillagers += 1;
            }
        }
    }

    // Helpful editor visuals.
    private void OnDrawGizmos()
    {
        // Draw each house idle position.
        Transform[] transforms = houseIdlePositions.GetComponentsInChildren<Transform>();
        foreach (Transform trans in transforms)
        {
            // If the transform doesn't belong to the house idle positions parent...
            if (trans.gameObject != houseIdlePositions)
            {
                // Draw the gizmo that marks the transform!
                Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
                Gizmos.DrawSphere(trans.position, 0.2f);
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(trans.position, 0.2f);
            }
        }
    }
}