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
        //Debug.Log("AbilityTest: Time stop activated.");
        timeStopAbility.PointerLocationAbility(transform.position);
    }
}