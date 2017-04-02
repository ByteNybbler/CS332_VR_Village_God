﻿// Author(s): Paul Calande
// Script for the Mountain God game controller.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (XPLevels))]
public class GameController : MonoBehaviour
{
    [Tooltip("Reference to the enemy controller.")]
    public GameObject enemyController;
    [Tooltip("Reference to the village.")]
    public GameObject village;

    // Component references.
    private XPLevels levels;
    private Village compVillage;
    private EnemyController compEnemyController;

    private void Awake()
    {
        levels = GetComponent<XPLevels>();
    }

    private void Start()
    {
        compVillage = village.GetComponent<Village>();
        compEnemyController = enemyController.GetComponent<EnemyController>();

        compVillage.OnAllVillagersDead += GameOver;
        compEnemyController.OnEnemyDeath += EnemyDied;
    }

    // Enemy death event payload.
    private void EnemyDied(GameObject enemy, int xp)
    {
        // Increment XP.
        levels.AddXP(xp);
    }

    // Village "no more villagers" event payload.
    private void GameOver()
    {
        Debug.Log("Game over, baby.");
        // TODO: Stuff happens when you lose.
    }
}