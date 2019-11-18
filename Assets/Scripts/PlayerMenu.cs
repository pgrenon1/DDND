using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenu : BaseBehaviour
{
    #region Inspector Attributes
    public float scrollSpeed = 0.15f;
    public MenuOption loadoutObjectPrefab;
    public LoadoutSlot loadoutSlotA;
    public LoadoutSlot loadoutSlotB;
    public MenuOption readyMenuOption;
    #endregion

    #region Properties
    public LoadoutSlot FocusedLoadoutSlot { get; set; }
    public MenuOption CurrentMenuOption { get; private set; }
    public Player Player { get; set; }
    public Targetable CurrentTarget { get; set; }

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

    private bool _isVisible;
    public bool IsVisible
    {
        get
        {
            return _isVisible;
        }
        private set
        {
            _isVisible = value;

            ApplyVisibility();
        }
    }

    private InfoPanel _infoPanel;
    public InfoPanel InfoPanel
    {
        get
        {
            if (_infoPanel == null)
                _infoPanel = GetComponentInChildren<InfoPanel>();

            return _infoPanel;
        }
    }
    #endregion

    public delegate void OnSelectionChanged(MenuOption Selected);
    public event OnSelectionChanged SelectionChanged;

    public void MoveSelection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                if (FocusedLoadoutSlot == loadoutSlotB)
                    SwitchLoadoutSlot(loadoutSlotA);
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
                if (FocusedLoadoutSlot == loadoutSlotA)
                    SwitchLoadoutSlot(loadoutSlotB);
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

            CurrentTarget = FindObjectOfType<Enemy>();
        }
    }

    private void SwitchLoadoutSlot(LoadoutSlot loadoutSlot)
    {
        if (loadoutSlot.PickedMenuOption != null)
        {
            Select(loadoutSlot.PickedMenuOption.Toggle);
        }
        else
        {
            Select(loadoutSlot.LoadoutObjects.First().Key.Toggle);
        }
    }

    public void ExitMenu()
    {
        Hide();
    }

    //private void Confirm()
    //{
    //    SelectedMenuOption.Confirm();
    //    //switch (CurrentMenuState)
    //    //{
    //    //    case MenuState.Inventory:
    //    //        var currentItemMenuOption = SelectedMenuOption as ItemMenuOption;
    //    //        if (currentItemMenuOption)
    //    //        {
    //    //            currentItemMenuOption.Confirm();
    //    //        }
    //    //        break;
    //    //    case MenuState.Action:
    //    //        SelectedMenuOption.Confirm();
    //    //        break;
    //    //}
    //}

    public void RefreshLoadout()
    {
        var loadoutObjects = new List<LoadoutObject>();

        loadoutObjects.AddRange(Player.Items);
        loadoutObjects.AddRange(Player.Skills);

        loadoutSlotA.Refresh(loadoutObjects);
        loadoutSlotB.Refresh(loadoutObjects);
    }

    //public void InitActionMenu()
    //{
    //    CurrentMenuState = MenuState.Action;

    //    attackMenuOption.Toggle.interactable = true;
    //    defendMenuOption.Toggle.interactable = true;

    //    Selected = attackMenuOption;
    //}

    //public void InitInventoryMenu()
    //{
    //    CurrentMenuState = MenuState.Inventory;

    //    attackMenuOption.Deselect();
    //    defendMenuOption.Deselect();

    //    attackMenuOption.Toggle.interactable = false;
    //    defendMenuOption.Toggle.interactable = false;

    //    Inventory.Instance.PopulateInventory(Player.Items);

    //    Inventory.Instance.Show();

    //    Select(Inventory.Instance.ItemMenuOptions[0]);
    //}

    public void Select(Selectable selectable)
    {
        if (selectable == null)
            return;

        var menuOption = selectable.GetComponent<MenuOption>();
        SelectedMenuOption = menuOption;
    }

    private void ApplyVisibility()
    {
        CanvasGroup.alpha = IsVisible ? 1f : 0f;
        CanvasGroup.interactable = IsVisible;
    }

    public void Show()
    {
        IsVisible = true;
    }

    public void Hide()
    {
        IsVisible = false;
    }
}
