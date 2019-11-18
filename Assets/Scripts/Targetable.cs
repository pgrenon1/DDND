using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : OdinSerializedBaseBehaviour
{
    public Damageable Damageable { get; set; }

    protected virtual void Start()
    {
        Damageable = GetComponent<Damageable>();
    }
}
