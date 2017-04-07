// Author(s): Paul Calande
// Script for the Mountain God game controller.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (XPLevels))]
public class GameController : LateInit
{
    [Tooltip("Reference to the enemy controller instance.")]
    public GameObject enemyController;
    [Tooltip("Reference to the village instance.")]
    public GameObject village;
    [Tooltip("The string that is the icon for XP.")]
    public string xpString = "XP";

    // Component references.
    private XPLevels compLevels;
    private Village compVillage;
    private EnemyController compEnemyController;
    private RisingTextCreator compRisingTextCreatorXP;

    private void Awake()
    {
        compLevels = GetComponent<XPLevels>();
        compRisingTextCreatorXP = GetComponent<RisingTextCreator>();
    }

    public override void Init()
    {
        compVillage = village.GetComponent<Village>();
        compEnemyController = enemyController.GetComponent<EnemyController>();
        base.Init();
    }

    protected override void EventsSubscribe()
    {
        compVillage.AllVillagersDied += Village_AllVillagersDied;
        compEnemyController.EnemyDied += EnemyController_EnemyDied;
    }
    protected override void EventsUnsubscribe()
    {
        compVillage.AllVillagersDied -= Village_AllVillagersDied;
        compEnemyController.EnemyDied -= EnemyController_EnemyDied;
    }

    // Enemy death event payload.
    private void EnemyController_EnemyDied(GameObject enemy, int xp)
    {
        // Increment XP.
        compLevels.AddXP(xp);
        // Spawn XP text at the enemy position.
        compRisingTextCreatorXP.message = "+" + xp + " " + xpString;
        compRisingTextCreatorXP.CreateRisingText(enemy.transform.position);
    }

    // Village "no more villagers" event payload.
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