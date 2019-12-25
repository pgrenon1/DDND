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
    public LoadoutInfoPanel loadoutInfoPanel;

    public List<KeyValuePair<Slot, SlotType>> LoadoutSlots { get; set; } = new List<KeyValuePair<Slot, SlotType>>();
    public Player Player { get; set; }
    public int FocusedLoadoutSlotIndex { get; set; }
    public Slot FocusedLoadoutSlot
    {
        get
        {
            if (LoadoutSlots.Count > 0)
                return LoadoutSlots[FocusedLoadoutSlotIndex].Key;
            else
                return null;
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

        if (FocusedLoadoutSlot != null && FocusedLoadoutSlot.SelectedSlotElement != null)
            loadoutInfoPanel.RefreshContent(FocusedLoadoutSlot.SelectedSlotElement);
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