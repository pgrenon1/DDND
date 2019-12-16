using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClassSlotElement : SlotElement
{
    public PlayerClass PlayerClass { get; set; }

    public PlayerClassSlotElement(PlayerClass playerClass)
    {
        PlayerClass = playerClass;
        sprite = playerClass.classIcon;
        //description = playerClass.description;
        elementName = playerClass.className;
    }
}
