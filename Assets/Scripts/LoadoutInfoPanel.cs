using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : UIBaseBehaviour
{
    public Image image;
    public TextMeshProUGUI title;
    public TextMeshProUGUI category;
    public TextMeshProUGUI description;

    public void RefreshContent(SlotElement loadoutObject)
    {
        image.sprite = loadoutObject.sprite;
        title.text = loadoutObject.objectName;
        category.text = loadoutObject.GetType().ToString();
        description.text = loadoutObject.description;

        var item = loadoutObject as Item;
        if (item != null)
            UpdateItemContent(item);

        var skill = loadoutObject as Skill;
        if (skill != null)
            UpdateSkillContent(skill);
    }

    public void UpdateSkillContent(Skill skill)
    {

    }

    public void UpdateItemContent(Item item)
    {

    }
}
