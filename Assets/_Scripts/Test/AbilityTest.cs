using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTest : MonoBehaviour
{
    public LightningAbility cLightningAbility;
    public TimeStop timeStopAbility;

    public void SpawnLightningHere()
    {
        cLightningAbility.PointerLocationAbility(transform.position);
    }

    public void StopTime()
    {
        timeStopAbility.PointerLocationAbility(transform.position);
    }
}