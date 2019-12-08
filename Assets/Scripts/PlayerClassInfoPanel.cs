using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerClassInfoPanel : InfoPanel
{
    public TextMeshProUGUI description;

    public override void RefreshContent(SlotElement slotElement)
    {
        var playerClassSlotElement = slotElement as PlayerClassSlotElement;
        if (playerClassSlotElement != null)
            title.text = playerClassSlotElement.PlayerClass.className;
    }
}
