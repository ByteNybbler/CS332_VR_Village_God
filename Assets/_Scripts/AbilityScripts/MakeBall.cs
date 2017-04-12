// Author(s): Paul Calande
// Ability script for making a ball of death.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeBall : MonoBehaviour
{
    [Tooltip("Ball of death prefab to instantiate.")]
    public GameObject prefabBallOfDeath;
    [Tooltip("The cost of the Ball of Death.")]
    public int cost = 25;

    // Component references.
    private AbilityInterface cai;

    private void Awake()
    {
        cai = GetComponent<AbilityInterface>();
    }

    public void SpawnBall()
    {
        Vector3 location;
        if (cai.castRayRightController.Cast(out location))
        {
            if (cai.shrine.SpendPoints(cost, location))
            {
                // Calculate the required y offset so that the Ball of Death doesn't spawn halfway into the ground.
                float yOffset = prefabBallOfDeath.transform.localScale.y * 0.5f;
                Vector3 creationLocation = new Vector3(location.x, location.y + yOffset, location.z);
                // Instantiate the Ball of Death accordingly.
                Instantiate(prefabBallOfDeath, creationLocation, Quaternion.identity);
            }
        }
    }
}