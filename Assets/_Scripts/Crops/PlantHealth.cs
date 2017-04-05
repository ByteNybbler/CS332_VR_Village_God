// Author(s): Hunter Golden, Paul Calande
// Plant health script.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantHealth : MonoBehaviour
{
    [Tooltip("The current health of the plant.")]
    public int health = 10;
    [Tooltip("Reference to the crop stalks parent object.")]
    public GameObject stalks;

    public delegate void DiedHandler(GameObject victim);
    public event DiedHandler Died;

    // Component references.
    private Crops compCrops;

    private void Start()
    {
        compCrops = stalks.GetComponent<Crops>();
    }

    // Decrease plant health. This function is called each time a villager eats the crop.
    public void DecreaseHealth()
    {
        // Destroy one of the stalks' children, hence removing one crop.
        Destroy(stalks.transform.GetChild(0));
        // Decrement health.
        health--;
        // Check if the plant is dead yet.
        if (health <= 0)
        {
            Die();
        }
    }

    // Returns true if the crop is fully grown.
    public bool GetIsGrown()
    {
        return compCrops.isGrown;
    }

    private void Die()
    {
        OnDied(gameObject);
        Destroy(gameObject);
    }

    private void OnDied(GameObject obj)
    {
        if (Died != null)
        {
            Died(obj);
        }
    }
}
