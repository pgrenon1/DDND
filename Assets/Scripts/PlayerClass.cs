using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass : OdinSerializedScriptableObject
{
    public ScalableFloat maxHealth;
    public ScalableFloat maxEnergy;
    public ScalableFloat energyGainPerNote;
    [AssetSelector]
    public List<ItemData> startingItemDatas = new List<ItemData>();
    [AssetSelector]
    public List<SkillData> startingSkillDatas = new List<SkillData>();
    [Range(1, 2)]
    public int numberOfSlots;
    public bool canHaveTwoItems;
    public bool canHaveTwoSkills;
    public bool canChangeLoadoutInSong;
}
