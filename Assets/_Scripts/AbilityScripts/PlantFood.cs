// Author(s): Hunter Golden, Paul Calande
// Ability class for planting food.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantFood : Ability
{
    [Tooltip("Reference to the food controller.")]
    public FoodController foodController;

    public override void PointerLocationAbility(Vector3 location)
    {
        foodController.CreateCrop(location);
    }
}