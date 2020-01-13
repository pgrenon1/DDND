using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SlotElement
{
    [FormerlySerializedAs("elementName")]
    public string slotElementName;
    public string description;
    public Sprite sprite;
}