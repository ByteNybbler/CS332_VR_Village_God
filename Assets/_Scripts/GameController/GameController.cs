// Author(s): Paul Calande
// Script for the Mountain God game controller.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (XPLevels))]
public class GameController : MonoBehaviour
{
    [Tooltip("Reference to the enemy controller instance.")]
    public EnemyController enemyController;
    [Tooltip("Reference to the village.")]
    public Village village;
    [Tooltip("The string that is the icon for XP.")]
    public string xpString = "XP";

    // The timescale of the game.
    //private float timeScale = 1f;

    // Component references.
    private XPLevels compLevels;
    private RisingTextCreator compRisingTextCreatorXP;

    private void Awake()
    {
        compLevels = GetComponent<XPLevels>();
        compRisingTextCreatorXP = GetComponent<RisingTextCreator>();
    }

    private void Start()
    {
        if (village != null)
        {
            village.AllVillagersDied += Village_AllVillagersDied;
        }
        if (enemyController != null)
        {
            enemyController.EnemyDied += EnemyController_EnemyDied;
        }
    }

    private void OnDestroy()
    {
        village.AllVillagersDied -= Village_AllVillagersDied;
        enemyController.EnemyDied -= EnemyController_EnemyDied;
    }

    // Enemy death event callback.
    private void EnemyController_EnemyDied(GameObject enemy, int xp)
    {
        // Increment XP.
        compLevels.AddXP(xp);
        // Spawn XP text at the enemy position.
        compRisingTextCreatorXP.message = "+" + xp + " " + xpString;
        compRisingTextCreatorXP.CreateRisingText(enemy.transform.position);
    }

    // Village "no more villagers" event callback.
    private void Village_AllVillagersDied()
    {
        GameOver();
    }

    private void GameOver()
    {
        Debug.Log("Game over, baby.");
        // TODO: Stuff happens when you lose.
    }
}