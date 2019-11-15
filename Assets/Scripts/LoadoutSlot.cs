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

    public Dictionary<MenuOption, LoadoutObject> LoadoutObjects { get; set; } = new Dictionary<MenuOption, LoadoutObject>();
    public MenuOption PickedMenuOption { get; set; }
    public LoadoutObject PickedLoadoutObject
    {
        get
        {
            if (PickedMenuOption != null)
                return LoadoutObjects[PickedMenuOption];
            else
                return null;
        }
    }

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
        if (LoadoutObjects.ContainsKey(selected))
        {
            SetPicked(selected);
        }
    }

    private void SetPicked(MenuOption newPicked)
    {
        PickedMenuOption = newPicked;
        ScrollToSelected();
        PlayerMenu.FocusedLoadoutSlot = this;

        var loadoutObject = LoadoutObjects[PickedMenuOption];
        PlayerMenu.InfoPanel.UpdateContent(loadoutObject);
    }

    private void UpdateInteractable()
    {
        foreach (var menuOption in LoadoutObjects)
        {
            menuOption.Key.Toggle.interactable = PlayerMenu.FocusedLoadoutSlot == this;
        }
    }

    private void Update()
    {
        UpdateVisibility();

        UpdateInteractable();
    }

    private void UpdateVisibility()
    {
        var showArrows = LoadoutObjects.ContainsKey(PlayerMenu.SelectedMenuOption);

        arrowUp.gameObject.SetActive(showArrows && PlayerMenu.SelectedMenuOption.transform.GetSiblingIndex() > 0);
        arrowDown.gameObject.SetActive(showArrows && PlayerMenu.SelectedMenuOption.transform.GetSiblingIndex() < contentParent.childCount - 1);
    }

    private void RemoveExtraMenuOptions(List<LoadoutObject> loadoutObjects)
    {
        var menuOptionsToRemove = new List<MenuOption>();

        foreach (var pair in LoadoutObjects)
        {
            if (!loadoutObjects.Contains(pair.Value))
            {
                menuOptionsToRemove.Add(pair.Key);
            }
        }

        foreach (var menuOption in menuOptionsToRemove)
        {
            Destroy(menuOption);
            LoadoutObjects.Remove(menuOption);
        }
    }

    private void AddMissingMenuOptions(List<LoadoutObject> loadoutObjects)
    {
        foreach (var loadoutObject in loadoutObjects)
        {
            if (!LoadoutObjects.Any(pair => pair.Value == loadoutObject))
            {
                var menuOption = Instantiate(PlayerMenu.loadoutObjectPrefab, contentParent);
                menuOption.Toggle.group = _toggleGroup;
                menuOption.mainImage.sprite = loadoutObject.sprite;
                menuOption.name = loadoutObject.GetType().ToString() + loadoutObject.objectName;
                LoadoutObjects.Add(menuOption, loadoutObject);
            }
        }
    }

    public void Refresh(List<LoadoutObject> inventory)
    {
        RemoveExtraMenuOptions(inventory);
        AddMissingMenuOptions(inventory);

        if (PickedMenuOption == null)
            SetPicked(LoadoutObjects.First().Key);
    }

    public void ScrollToSelected()
    {
        var menuOptions = new List<MenuOption>();

        foreach (var pair in LoadoutObjects)
        {
            menuOptions.Add(pair.Key);
        }

        var overshoot = 0;
        var viewportHeight = _scrollRect.viewport.rect.height;
        var difference = viewportHeight - menuOptions[0].RectTransform.rect.height;
        var halfDifference = difference / 2;
        var ratio = halfDifference / viewportHeight;
        Debug.Log(ratio);

        var index = menuOptions.IndexOf(PickedMenuOption);
        float target = (1f - (index / (LoadoutObjects.Count - 1f))).Remap(0f, 1f, 0f - ratio, 1f + ratio);
        DOTween.To(() => _scrollRect.verticalScrollbar.value, x => _scrollRect.verticalScrollbar.value = x, target, PlayerMenu.scrollSpeed);
    }
}
