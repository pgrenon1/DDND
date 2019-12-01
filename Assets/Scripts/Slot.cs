using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Slot<T> : UIBaseBehaviour where T : SlotElement
{
    public Image arrowUp;
    public Image arrowDown;
    public Transform contentParent;
    public SlotType slotType;

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

    public Dictionary<MenuOption, T> SlotElements { get; set; } = new Dictionary<MenuOption, T>();
    public MenuOption PickedMenuOption { get; set; }

    public T PickedSlotElement
    {
        get
        {
            if (PickedMenuOption != null)
                return SlotElements[PickedMenuOption];
            else
                return null;
        }
    }

    private ToggleGroup _toggleGroup;
    private InfoPanel _infoPanel;

    private void Awake()
    {
        _toggleGroup = GetComponent<ToggleGroup>();
    }

    private void Start()
    {
        PlayerMenu.SelectionChanged += PlayerMenu_SelectionChanged;
    }

    private void PlayerMenu_SelectionChanged(MenuOption selected)
    {
        if (SlotElements.ContainsKey(selected))
        {
            SetPicked(selected);
        }
    }

    private void SetPicked(MenuOption newPicked)
    {
        PickedMenuOption = newPicked;
        ScrollToSelected();

        if (_infoPanel != null)
            _infoPanel.RefreshContent(PickedSlotElement);
    }

    private void UpdateInteractable()
    {
        foreach (var menuOption in SlotElements)
        {
            menuOption.Key.Toggle.interactable = PlayerMenu.LoadoutSlots[PlayerMenu.FocusedLoadoutSlotIndex].Key == this;
        }
    }

    private void Update()
    {
        UpdateArrowsVisibility();

        UpdateInteractable();
    }

    private void UpdateArrowsVisibility()
    {
        var showArrows = SlotElements.ContainsKey(PlayerMenu.SelectedMenuOption);

        arrowUp.gameObject.SetActive(showArrows && PlayerMenu.SelectedMenuOption.transform.GetSiblingIndex() > 0);
        arrowDown.gameObject.SetActive(showArrows && PlayerMenu.SelectedMenuOption.transform.GetSiblingIndex() < contentParent.childCount - 1);
    }

    private void RemoveExtraMenuOptions(List<T> loadoutObjects)
    {
        var menuOptionsToRemove = new List<MenuOption>();

        foreach (var pair in SlotElements)
        {
            if (!loadoutObjects.Contains(pair.Value))
            {
                menuOptionsToRemove.Add(pair.Key);
            }
        }

        foreach (var menuOption in menuOptionsToRemove)
        {
            Destroy(menuOption);
            SlotElements.Remove(menuOption);
        }
    }

    private void AddMissingMenuOptions(List<T> loadoutObjects)
    {
        foreach (var loadoutObject in loadoutObjects)
        {
            if (!SlotElements.Any(pair => pair.Value == loadoutObject))
            {
                var menuOption = Instantiate(PlayerMenu.loadoutObjectPrefab, contentParent);
                menuOption.Toggle.group = _toggleGroup;
                menuOption.mainImage.sprite = loadoutObject.sprite;
                menuOption.name = loadoutObject.GetType().ToString() + loadoutObject.objectName;
                SlotElements.Add(menuOption, loadoutObject);
            }
        }
    }

    public void Refresh(List<T> inventory)
    {
        RemoveExtraMenuOptions(inventory);
        AddMissingMenuOptions(inventory);

        if (PickedMenuOption == null)
            SetPicked(SlotElements.First().Key);
    }

    public void ScrollToSelected()
    {
        var menuOptions = new List<MenuOption>();

        foreach (var pair in SlotElements)
        {
            menuOptions.Add(pair.Key);
        }

        var indexOfPicked = menuOptions.IndexOf(PickedMenuOption);
        var loadoutObjectHeight = menuOptions[0].RectTransform.rect.height;

        for (int i = 0; i < menuOptions.Count; i++)
        {
            var indexDifference = indexOfPicked - i;

            var targetY = indexDifference * loadoutObjectHeight;

            menuOptions[i].RectTransform.DOLocalMove(new Vector2(0f, targetY), PlayerMenu.scrollSpeed);
        }
    }
}
