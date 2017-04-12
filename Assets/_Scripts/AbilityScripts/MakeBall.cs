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
        
    }
}