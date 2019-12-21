using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutPanel : UIBaseBehaviour
{
    [Header("Loadout")]
    public MenuOption menuOptionPrefab;
    public Slot loadoutSlotPrefab;
    public Transform slotsParent;

    public List<KeyValuePair<Slot, SlotType>> LoadoutSlots { get; set; } = new List<KeyValuePair<Slot, SlotType>>();
    public MenuOption CurrentMenuOption { get; private set; }
    public Player Player { get; set; }
    public int FocusedLoadoutSlotIndex { get; set; }
    public Slot FocusedLoadoutSlot
    {
        get
        {
            return LoadoutSlots[FocusedLoadoutSlotIndex].Key;
        }
    }

    private void Update()
    {
        foreach (var loadoutSlot in LoadoutSlots)
        {
            var interactable = loadoutSlot.Key == FocusedLoadoutSlot;

            foreach (var slotElement in loadoutSlot.Key.SlotElements)
            {
                slotElement.Key.Toggle.interactable = interactable;
            }
        }
    }

    public void InitLoadoutSlots()
    {
        foreach (var slotType in Player.PlayerClass.slots)
        {
            var loadoutSlot = Instantiate(loadoutSlotPrefab, slotsParent);
            LoadoutSlots.Add(new KeyValuePair<Slot, SlotType>(loadoutSlot, slotType));
        }

        PopulateLoadoutSlots();
    }

    private void PopulateLoadoutSlots()
    {
        foreach (var loadoutSlot in LoadoutSlots)
        {
            var loadoutSlotElements = new List<SlotElement>();

            if (loadoutSlot.Value == SlotType.Both || loadoutSlot.Value == SlotType.Item)
            {
                loadoutSlotElements.AddRange(Player.Items);
            }

            if (loadoutSlot.Value == SlotType.Both || loadoutSlot.Value == SlotType.Skill)
            {
                loadoutSlotElements.AddRange(Player.Skills);
            }

            loadoutSlot.Key.Refresh(loadoutSlotElements);
            loadoutSlot.Key.SelectFirst();
        }
    }

    public void SwitchFocus(Direction direction)
    {
        if (direction == Direction.Left && FocusedLoadoutSlotIndex > 0)
        {
            FocusedLoadoutSlotIndex--;
        }
        else if (direction == Direction.Right && FocusedLoadoutSlotIndex < LoadoutSlots.Count - 1)
        {
            FocusedLoadoutSlotIndex++;
        }
    }
}