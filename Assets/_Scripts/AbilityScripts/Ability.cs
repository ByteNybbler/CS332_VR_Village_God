// Author(s): Paul Calande
// Base class for abilities.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [Tooltip("The cost of the ability in faith points.")]
    public int cost;
    [Tooltip("If true, the ability is on the right controller. If not, the ability is on the left controller.")]
    public bool isOnRightController;
    [Tooltip("If true, the controller must be pointing at the ground to use this ability.")]
    public bool mustHitEnvironment;

    // Reference to references... so meta.
    protected AbilityReferences car;

    // Reference to the CastRay of the controller that the ability is on.
    private CastRay controller;

    private void Awake()
    {
        car = GetComponent<AbilityReferences>();
    }

    private void Start()
    {
        if (isOnRightController)
        {
            controller = car.castRayRightController;
        }
        else
        {
            controller = car.castRayLeftController;
        }
    }

    // Do the ability where the controller is pointing.
    // Spend faith points as well.
    public void UseAbilityAtPointerLocation()
    {
        Vector3 location;
        float maxDistance;
        if (mustHitEnvironment)
        {
            maxDistance = 10000f;
        }
        else
        {
            maxDistance = 25f;
        }
        bool castDidHit = controller.Cast(out location, maxDistance);
        if ((castDidHit || !mustHitEnvironment) && AdditionalPointerChecks(location))
        {
            if (car.shrine.SpendPoints(cost, location))
            {
                PointerLocationAbility(location);
            }
        }
    }

    public virtual bool AdditionalPointerChecks(Vector3 location)
    {
        return true;
    }

    // Function that does the actual ability. Implement this in derived classes.
    public abstract void PointerLocationAbility(Vector3 location);
}