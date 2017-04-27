// Author(s): Paul Calande
// Class for testing Mountain God abilities.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTest : MonoBehaviour
{
    public LightningAbility lightningAbility;
    public TimeStop timeStopAbility;
    public PurgeEvil purgeEvilAbility;
    public VillagerUpgrade villagerUpgradeAbility;

    public void SpawnLightningHere()
    {
        lightningAbility.PointerLocationAbility(transform.position);
    }

    public void StopTime()
    {
        //Debug.Log("AbilityTest: Time stop activated.");
        timeStopAbility.PointerLocationAbility(transform.position);
    }

    public void PurgeEvil()
    {
        purgeEvilAbility.PointerLocationAbility(transform.position);
    }

    public void UpgradeVillager()
    {
        villagerUpgradeAbility.PointerLocationAbility(transform.position);
    }
}