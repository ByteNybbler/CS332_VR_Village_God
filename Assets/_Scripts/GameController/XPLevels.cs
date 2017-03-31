// Author(s): Paul Calande
// Static class that handles levels and XP.

// Comment out the following line to disable debug messages for level ups and XP gains.
#define PRINT_LEVEL_UP

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPLevels : MonoBehaviour
{
    [Tooltip("The current level.")]
    public int level = 1;
    [Tooltip("The amount of XP per level.")]
    public int xpPerLevel = 10;

    public delegate void LevelUpAction();
    public event LevelUpAction OnLevelUp;

    private int xpCurrent = 0;
    private int xpNeeded;

    private void Awake()
    {
        xpNeeded = CalculateXPNeeded();
    }

    // Reward XP.
    public void AddXP(int amount)
    {
        xpCurrent += amount;

#if PRINT_LEVEL_UP
        Debug.Log("Gained " + amount + " XP. Current XP: " + xpCurrent + "/" + xpNeeded
            + " (Current level: " + level + ")");
#endif

        while (xpCurrent >= xpNeeded)
        {
            xpCurrent -= xpNeeded;
            level += 1;
            xpNeeded = CalculateXPNeeded();
            if (OnLevelUp != null)
            {
                OnLevelUp();
            }

#if PRINT_LEVEL_UP
            Debug.Log("Leveled up to level " + level + "!");
#endif
        }
    }

    private int CalculateXPNeeded()
    {
        return level * xpPerLevel;
    }
}