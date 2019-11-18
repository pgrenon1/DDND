using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutObject
{
    public string objectName;
    public string description;
    public Sprite sprite;

    public Player Owner { get; set; }

    public virtual void Trigger(float noteScore)
    {

    }
}