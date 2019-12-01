using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillEffect
{
    public Skill Skill { get; set; }

    public virtual void Trigger(float score)
    {

    }
}
