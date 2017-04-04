// Author(s): Hunter Golden, Paul Calande
// Plant health script.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantHealth : MonoBehaviour
{
    [Tooltip("The current health of the plant.")]
    public int health = 10;

    public delegate void DiedHandler(GameObject victim);
    public event DiedHandler Died;

    public void DecreaseHealth()
    {
        health--;
        if (health <= 0)
        {
            Die();
        }
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
