// Author(s): Paul Calande
// Static class that handles levels and XP.

// Comment out the following line to disable debug messages for level ups and XP gains.
#define PRINT_LEVEL_UP

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour
{
    public static int level = 1;

    private static int xpCurrent = 0;
    private static int xpNeeded = CalculateXPNeeded();

    // Use this static function to reward XP to the player.
    public static void AddXP(int amount)
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

#if PRINT_LEVEL_UP
            Debug.Log("Leveled up to level " + level + "!");
#endif
        }
    }

    private static int CalculateXPNeeded()
    {
        return level * 100;
    }
}