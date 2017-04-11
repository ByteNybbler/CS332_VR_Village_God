using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTest : MonoBehaviour
{
    public LightningAbility cLightningAbility;

    public void SpawnLightningHere()
    {
        cLightningAbility.SpawnLightningAt(transform.position);
    }
}