// Author(s): Paul Calande
// Shrine script.

// Comment out the following line to prevent the shrine's points from being printed to the console.
//#define DEBUG_SHRINE_POINTS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour
{
    [Tooltip("How many charge seconds it takes to gain 1 point.\n" +
    "One villager idling at the shrine for one second = one charge second.")]
    public float chargeSecondsPerPoint;
    [Tooltip("How many points the shrine currently has.")]
    public int points = 0;
    [Tooltip("Reference to the +x faith rising text creator component.")]
    public RisingTextCreator rtcPlusPoints;
    [Tooltip("Reference to the Not Enough Points rising text creator component.")]
    public RisingTextCreator rtcNotEnoughPoints;
    [Tooltip("Reference to the Spent x Points rising text creator component.")]
    public RisingTextCreator rtcSpentPoints;
    [Tooltip("The string that is the icon for faith.")]
    public string faithString = "faith";
    [Tooltip("Reference to the enemy controller.")]
    public EnemyController enemyController;

    public delegate void PointsUpdatedHandler(int amount);
    public event PointsUpdatedHandler PointsUpdated;

    private void Start()
    {
        enemyController.EnemyDied += EnemyController_EnemyDied;
    }

    private void OnDestroy()
    {
        if (enemyController != null)
        {
            enemyController.EnemyDied -= EnemyController_EnemyDied;
        }
    }

    // Use this public function to add points to the shrine.
    // rootPosition is the spawn position of the +1.
    public void IncreasePoints(int amount, Vector3 rootPosition)
    {
        // Increase points.
        points += amount;
        // Invoke the points updated event with the updated amount of points.
        OnPointsUpdated(points);
        // Instantiate the +1 canvas.
        rtcPlusPoints.message = "+" + amount + " " + faithString;
        rtcPlusPoints.CreateRisingText(rootPosition);

#if DEBUG_SHRINE_POINTS
        Debug.Log("Shrine points: " + points);
#endif
    }

    // This function is used for buying stuff with the shrine's points.
    // If the player has enough points, those points will be spent and true will be returned.
    // Otherwise, return false.
    // amount is the number of points to be spent.
    // location is the spawning location of the various points notifications.
    public bool SpendPoints(int amount, Vector3 location)
    {
        if (points >= amount)
        {
            points -= amount;
            OnPointsUpdated(points);
            rtcSpentPoints.message = "-" + amount + " " + faithString;
            rtcSpentPoints.CreateRisingText(location);
            return true;
        }
        else
        {
			rtcNotEnoughPoints.message = "Need " + amount + " " + faithString;
            //rtcNotEnoughPoints.message = "Not enough faith!\n" + amount + " " + faithString + " needed.";
            rtcNotEnoughPoints.CreateRisingText(location);
            return false;
        }
    }

    private void OnPointsUpdated(int amountOfPoints)
    {
        if (PointsUpdated != null)
        {
            PointsUpdated(amountOfPoints);
        }
    }

    private void EnemyController_EnemyDied(EnemyStatus enemy, int faith)
    {
        Vector3 offset = new Vector3(0f, 3f, 0f);
        IncreasePoints(faith, enemy.transform.position + offset);
    }
}