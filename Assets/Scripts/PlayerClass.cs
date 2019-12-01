using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlotType
{
    Item,
    Skill,
    Both
}

[ShowOdinSerializedPropertiesInInspector, CreateAssetMenu(fileName = "PlayerClass_", menuName = "PlayerClass")]
public class PlayerClass : OdinSerializedScriptableObject
{
    public string className;
    public ScalableFloat maxHealth;
    public ScalableFloat maxEnergy;
    public ScalableFloat energyGainPerNote;
    [AssetSelector]
    public List<ItemData> startingItemDatas = new List<ItemData>();
    [AssetSelector]
    public List<SkillData> startingSkillDatas = new List<SkillData>();
    public List<SlotType> slots = new List<SlotType>();
    public bool canChangeLoadoutInSong;
}