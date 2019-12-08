using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Slot : UIBaseBehaviour
{
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

    public Dictionary<MenuOption, SlotElement> SlotElements { get; set; } = new Dictionary<MenuOption, SlotElement>();

    public MenuOption _selectedMenuOption;
    public MenuOption SelectedMenuOption
    {
        get
        {
            return _selectedMenuOption;
        }

        private set
        {
            _selectedMenuOption = value;
        }
    }
    public SlotElement SelectedSlotElement
    {
        get
        {
            if (SelectedMenuOption != null)
                return SlotElements[SelectedMenuOption];
            else
                return null;
        }
    }

    public T GetPickedSlotElement<T>() where T : SlotElement
    {
        return SelectedSlotElement as T;
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

    public IEnumerable<T> GetSlotElements<T>() where T : SlotElement
    {
        foreach (var slotElement in SlotElements)
            if (slotElement.Value is T)
                yield return slotElement.Value as T;
    }

    private void SetPicked(MenuOption newPicked)
    {
        SelectedMenuOption = newPicked;
        ScrollToSelected();

        if (_infoPanel != null)
            _infoPanel.RefreshContent(SelectedSlotElement);
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

    private void RemoveExtraMenuOptions(List<SlotElement> loadoutObjects)
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

    private void AddMissingMenuOptions(List<SlotElement> loadoutObjects)
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

    public void Refresh(List<SlotElement> slotElements)
    {
        RemoveExtraMenuOptions(slotElements);
        AddMissingMenuOptions(slotElements);

        if (SelectedMenuOption == null)
            SetPicked(SlotElements.First().Key);
    }

    public void ScrollToSelected()
    {
        var menuOptions = new List<MenuOption>();

        foreach (var pair in SlotElements)
        {
            menuOptions.Add(pair.Key);
        }

        var indexOfPicked = menuOptions.IndexOf(SelectedMenuOption);
        var loadoutObjectHeight = menuOptions[0].RectTransform.rect.height;

        for (int i = 0; i < menuOptions.Count; i++)
        {
            var indexDifference = indexOfPicked - i;

            var targetY = indexDifference * loadoutObjectHeight;

            menuOptions[i].RectTransform.DOLocalMove(new Vector2(0f, targetY), PlayerMenu.scrollSpeed);
        }
    }

    public void MoveSelection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Down:
                Select(SelectedMenuOption.Toggle.FindSelectableOnDown());
                break;
            case Direction.Up:
                Select(SelectedMenuOption.Toggle.FindSelectableOnUp());
                break;
        }
    }

    public void Select(Selectable selectable)
    {
        if (selectable == null)
            return;

        selectable.Select();

        var menuOption = selectable.GetComponent<MenuOption>();
        SelectedMenuOption = menuOption;
    }
}
