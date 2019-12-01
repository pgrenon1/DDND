using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenu : UIBaseBehaviour
{
    #region Inspector Attributes
    public MenuOption readyMenuOption;

    [Header("Loadout")]
    public float scrollSpeed = 0.15f;
    public MenuOption loadoutObjectPrefab;
    public Slot<LoadoutSlotElement> loadoutSlotPrefab;
    public Transform slotsParent;
    #endregion

    #region Properties
    public List<KeyValuePair<Slot<LoadoutSlotElement>, SlotType>> LoadoutSlots { get; set; } = new List<KeyValuePair<Slot<LoadoutSlotElement>, SlotType>>();
    public MenuOption CurrentMenuOption { get; private set; }
    public Player Player { get; set; }

    private int _focusedLoadoutSlotIndex;
    public int FocusedLoadoutSlotIndex
    {
        get
        {
            return _focusedLoadoutSlotIndex;
        }
        set
        {
            _focusedLoadoutSlotIndex = value;

            RefreshFocusedLoadoutSlot();
        }
    }

    private MenuOption _selectedMenuOption;
    public MenuOption SelectedMenuOption
    {
        get
        {
            return _selectedMenuOption;
        }
        private set
        {
            if (_selectedMenuOption)
                _selectedMenuOption.Deselect();

            _selectedMenuOption = value;
            _selectedMenuOption.Select();

            if (SelectionChanged != null)
                SelectionChanged(_selectedMenuOption);
        }
    }

    public delegate void OnSelectionChanged(MenuOption Selected);
    public event OnSelectionChanged SelectionChanged;
    #endregion

    public void MoveSelection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                if (FocusedLoadoutSlotIndex > 0)
                    FocusedLoadoutSlotIndex--;
                else
                    Select(SelectedMenuOption.Toggle.FindSelectableOnLeft());
                break;
            case Direction.Down:
                Select(SelectedMenuOption.Toggle.FindSelectableOnDown());
                break;
            case Direction.Up:
                Select(SelectedMenuOption.Toggle.FindSelectableOnUp());
                break;
            case Direction.Right:
                if (FocusedLoadoutSlotIndex < LoadoutSlots.Count - 1)
                    FocusedLoadoutSlotIndex++;
                else
                    Select(SelectedMenuOption.Toggle.FindSelectableOnRight());
                break;
        }
    }

    public void Confirm()
    {
        if (!Player.IsReady)
        {
            Player.IsReady = true;

            Player.CurrentTarget = FindObjectOfType<Enemy>();
        }
    }

    private void RefreshFocusedLoadoutSlot()
    {
        var focusedSlot = LoadoutSlots[FocusedLoadoutSlotIndex];

        if (focusedSlot.Key.PickedMenuOption != null)
            Select(focusedSlot.Key.PickedMenuOption.Toggle);
        else
            Select(focusedSlot.Key.SlotElements.First().Key.Toggle);
    }

    public void ExitMenu()
    {
        Hide();
    }

    public void InitLoadoutSlots(PlayerClass playerClass)
    {
        foreach (var slotType in playerClass.slots)
        {
            var loadoutSlot = Instantiate(loadoutSlotPrefab, slotsParent);
            LoadoutSlots.Add(new KeyValuePair<Slot<LoadoutSlotElement>, SlotType>(loadoutSlot, slotType));
        }
    }

    public void RefreshLoadout()
    {
        var loadoutObjects = new List<LoadoutSlotElement>();

        loadoutObjects.AddRange(Player.Items);
        loadoutObjects.AddRange(Player.Skills);

        foreach (var loadoutSlot in LoadoutSlots)
        {
            loadoutSlot.Key.Refresh(loadoutObjects);
        }
    }

    public void Select(Selectable selectable)
    {
        if (selectable == null)
            return;

        var menuOption = selectable.GetComponent<MenuOption>();
        SelectedMenuOption = menuOption;
    }
}