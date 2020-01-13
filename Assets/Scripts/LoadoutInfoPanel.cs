using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutInfoPanel : UIBaseBehaviour
{
    public Image image;
    public TextMeshProUGUI title;
    public TextMeshProUGUI category;
    public TextMeshProUGUI description;

    public void RefreshContent(SlotElement loadoutSlotElement)
    {
        image.sprite = loadoutSlotElement.sprite;
        title.text = loadoutSlotElement.slotElementName;
        category.text = loadoutSlotElement.GetType().ToString();
        description.text = loadoutSlotElement.description;

        var item = loadoutSlotElement as Item;
        if (item != null)
            RefreshItemContent(item);

        var skill = loadoutSlotElement as Skill;
        if (skill != null)
            RefreshSkillContent(skill);
    }

    public void RefreshSkillContent(Skill skill)
    {
        //TODO
    }

    public void RefreshItemContent(Item item)
    {
        //TODO
    }
}
