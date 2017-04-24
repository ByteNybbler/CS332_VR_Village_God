// Author(s): Hunter Golden, Paul Calande
// Plant status script.

// Comment out the following line of code to disable the plant's meter.
//#define PLANTSTATUS_USEMETER

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
    [Tooltip("The plant's Y scale growth is multiplied by this.")]
    public float yScaleFactor = 1f;
    [Tooltip("The audio clip that plays when the crop becomes fully grown.")]
    public AudioClip soundFullyGrown;

    public delegate void DiedHandler(GameObject victim);
    public event DiedHandler Died;

    // Crop is fully grown.
    public delegate void GrownHandler(GameObject self);
    public event GrownHandler Grown;

    // Whether the plant is fully grown.
    private bool isGrown = false;
    // The current scale of the plant.
    private float currentScale = 0f;

    // Component references.
#if PLANTSTATUS_USEMETER
    private Meter meter;
#endif
    private TimeScale ts;
    private AudioSource audioSource;

    private void Awake()
    {
#if PLANTSTATUS_USEMETER
        meter = GetComponent<Meter>();
#endif
        ts = GetComponent<TimeScale>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
#if PLANTSTATUS_USEMETER
        meter.SetBothValues(currentScale, maxScale);
#endif
    }

    private void Update()
    {
        if (!isGrown)
        {
            float timePassed = ts.GetTimePassed();
            float increase = rate * timePassed;
            stalks.transform.localScale += Vector3.up * yScaleFactor * increase;
            stalks.transform.position += (Vector3.up * increase) / 2;
            currentScale += increase;
            if (currentScale >= maxScale)
            {
                FinishGrowing();
            }
#if PLANTSTATUS_USEMETER
            meter.SetCurrentValue(currentScale);
#endif
        }
    }

    private void FinishGrowing()
    {
        isGrown = true;
        audioSource.PlayOneShot(soundFullyGrown);
        OnGrown(gameObject);
    }

    // Decrease plant health. This function is called each time a villager eats the crop.
    public void DecreaseHealth()
    {
        // Destroy one of the stalks' children, hence removing one crop.
        Destroy(stalks.transform.GetChild(0).gameObject);
        // Decrement health.
        --health;
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

    private void OnGrown(GameObject obj)
    {
        if (Grown != null)
        {
            Grown(obj);
        }
    }
}