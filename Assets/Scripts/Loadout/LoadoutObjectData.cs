using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutObjectData : OdinSerializedScriptableObject
{
    public string objectName;
    [TextArea]
    public string description;
    [PreviewField]
    public Sprite sprite;
}
