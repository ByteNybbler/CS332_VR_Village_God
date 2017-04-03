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

    // Component references.
    private XPLevels levels;
    private Village compVillage;
    private EnemyController compEnemyController;

    private void Awake()
    {
        levels = GetComponent<XPLevels>();
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
        levels.AddXP(xp);
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