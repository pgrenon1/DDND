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

    //private MenuOption _selectedMenuOption;
    //public MenuOption SelectedMenuOption
    //{
    //    get
    //    {
    //        return _selectedMenuOption;
    //    }
    //    private set
    //    {
    //        if (_selectedMenuOption)
    //            _selectedMenuOption.Deselect();

    //        _selectedMenuOption = value;
    //        _selectedMenuOption.Select();

    //        if (SelectionChanged != null)
    //            SelectionChanged(_selectedMenuOption);
    //    }
    //}

    //public void MoveSelection(Direction direction)
    //{
    //    switch (direction)
    //    {
    //        case Direction.Left:
    //            if (FocusedLoadoutSlotIndex > 0)
    //                FocusedLoadoutSlotIndex--;
    //            else
    //                Select(SelectedMenuOption.Toggle.FindSelectableOnLeft());
    //            break;
    //        case Direction.Down:
    //            Select(SelectedMenuOption.Toggle.FindSelectableOnDown());
    //            break;
    //        case Direction.Up:
    //            Select(SelectedMenuOption.Toggle.FindSelectableOnUp());
    //            break;
    //        case Direction.Right:
    //            if (FocusedLoadoutSlotIndex < LoadoutSlots.Count - 1)
    //                FocusedLoadoutSlotIndex++;
    //            else
    //                Select(SelectedMenuOption.Toggle.FindSelectableOnRight());
    //            break;
    //    }
    //}

    public void Confirm()
    {
        //if (!Player.IsReady)
        //{
        //    Player.IsReady = true;

        //    Player.CurrentTarget = FindObjectOfType<Enemy>();
        //}
    }

    //private void RefreshFocusedLoadoutSlot()
    //{
    //    var focusedSlot = LoadoutSlots[FocusedLoadoutSlotIndex];

    //    if (focusedSlot.Key.SelectedMenuOption != null)
    //        Select(focusedSlot.Key.SelectedMenuOption.Toggle);
    //    else
    //        Select(focusedSlot.Key.SlotElements.First().Key.Toggle);
    //}

    public void InitLoadoutSlots()
    {
        foreach (var slotType in Player.PlayerClass.slots)
        {
            var loadoutSlot = Instantiate(loadoutSlotPrefab, slotsParent);
            LoadoutSlots.Add(new KeyValuePair<Slot, SlotType>(loadoutSlot, slotType));
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

    public void RefreshLoadout()
    {
        var loadoutObjects = new List<SlotElement>();

        loadoutObjects.AddRange(Player.Items);
        loadoutObjects.AddRange(Player.Skills);

        foreach (var loadoutSlot in LoadoutSlots)
        {
            loadoutSlot.Key.Refresh(loadoutObjects);
        }

        LoadoutSlots.First().Key.SelectFirst();
    }

    //public void Select(Selectable selectable)
    //{
    //    if (selectable == null)
    //        return;

    //    var menuOption = selectable.GetComponent<MenuOption>();
    //    SelectedMenuOption = menuOption;
    //}
}