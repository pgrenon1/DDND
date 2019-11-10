using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutSlot : MonoBehaviour
{
    public float scrollOvershoot = 0.15f;
    public Image arrowUp;
    public Image arrowDown;
    public Transform contentParent;

    private PlayerMenu _playerMenu;
    public PlayerMenu PlayerMenu
    {
        get
        {
            if (_playerMenu == null)
                _playerMenu = GetComponentInParent<PlayerMenu>();

            return _playerMenu;
        }
    }

    public Dictionary<LoadoutObject, MenuOption> MenuOptions { get; set; } = new Dictionary<LoadoutObject, MenuOption>();
    public MenuOption Picked { get; set; }

    private ScrollRect _scrollRect;
    private ToggleGroup _toggleGroup;

    private void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _toggleGroup = GetComponent<ToggleGroup>();

        PlayerMenu.SelectionChanged += PlayerMenu_SelectionChanged;
    }

    private void PlayerMenu_SelectionChanged(MenuOption selected)
    {
        if (MenuOptions.Values.Contains(selected))
        {
            Picked = selected;
            ScrollToSelected(selected);
            PlayerMenu.FocusedLoadoutSlot = this;
        }

        RefreshInteractable();
    }

    private void RefreshInteractable()
    {
        foreach (var menuOption in MenuOptions)
        {
            menuOption.Value.Toggle.interactable = PlayerMenu.FocusedLoadoutSlot == this;
        }
    }

    private void Update()
    {
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        var showArrows = MenuOptions.ContainsValue(PlayerMenu.SelectedMenuOption);

        arrowUp.gameObject.SetActive(showArrows);
        arrowDown.gameObject.SetActive(showArrows);
    }

    private void RemoveExtraMenuOptions(List<LoadoutObject> inventory)
    {
        var keysToRemove = new List<LoadoutObject>();

        foreach (var pair in MenuOptions)
        {
            var loadoutObject = pair.Key;
            if (!inventory.Contains(loadoutObject))
            {
                keysToRemove.Add(loadoutObject);
            }
        }

        foreach (var key in keysToRemove)
        {
            Destroy(MenuOptions[key].gameObject);
            MenuOptions.Remove(key);
        }
    }

    private void AddMissingMenuOptions(List<LoadoutObject> loadoutObjects)
    {
        foreach (var loadoutObject in loadoutObjects)
        {
            if (!MenuOptions.Any(pair => pair.Key == loadoutObject))
            {
                var menuOption = Instantiate(PlayerMenu.loadoutObjectPrefab, contentParent);
                menuOption.Toggle.group = _toggleGroup;
                menuOption.mainImage.sprite = loadoutObject.sprite;
                menuOption.name = loadoutObject.GetType().ToString() + loadoutObject.objectName;
                MenuOptions.Add(loadoutObject, menuOption);
            }
        }
    }

    public void Refresh(List<LoadoutObject> inventory)
    {
        RemoveExtraMenuOptions(inventory);
        AddMissingMenuOptions(inventory);
    }

    public void ScrollToSelected(MenuOption selectedMenuOption)
    {
        var values = new List<MenuOption>();

        foreach (var pair in MenuOptions)
        {
            values.Add(pair.Value);
        }

        var index = values.IndexOf(selectedMenuOption);
        float target = (1f - (index / (MenuOptions.Count - 1f))).Remap(0f, 1f, 0f - scrollOvershoot, 1f + scrollOvershoot);
        DOTween.To(() => _scrollRect.verticalScrollbar.value, x => _scrollRect.verticalScrollbar.value = x, target, PlayerMenu.scrollSpeed);
    }
}
