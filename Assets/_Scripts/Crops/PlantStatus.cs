// Author(s): Hunter Golden, Paul Calande
// Plant status script.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantStatus : MonoBehaviour
{
    [Tooltip("The current health of the plant.")]
    public int health = 10;
    [Tooltip("Reference to the crop stalks parent object.")]
    public GameObject stalks;
    [Tooltip("The speed at which the plants grow.")]
    public float rate;
    [Tooltip("The maximum scale of the plant.")]
    public float maxScale;

    public delegate void DiedHandler(GameObject victim);
    public event DiedHandler Died;

    // Whether the plant is fully grown.
    private bool isGrown = false;
    // The current scale of the plant.
    private float currentScale = 0f;
    // Component references.
    Meter meter;

    private void Awake()
    {
        meter = GetComponent<Meter>();
    }

    private void Start()
    {
        meter.SetInitialState(currentScale, maxScale);
    }

    private void Update()
    {
        if (!isGrown)
        {
            float increase = rate * Time.deltaTime;
            stalks.transform.localScale += Vector3.up * increase;
            stalks.transform.position += (Vector3.up * increase) / 2;
            currentScale += increase;
            if (currentScale >= maxScale)
            {
                isGrown = true;
            }
            meter.SetValue(currentScale);
        }
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
        return isGrown;
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