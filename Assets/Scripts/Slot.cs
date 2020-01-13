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

    public delegate void OnSelectionChanged(SlotElement slotElement);
    public event OnSelectionChanged SelectionChanged;

    private Player _player;
    public Player Player
    {
        get
        {
            if (_player == null)
                _player = GetComponentInParent<Player>();

            return _player;
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
            if (_selectedMenuOption != value)
            {
                _selectedMenuOption = value;

                if (SelectionChanged != null)
                    SelectionChanged(SelectedSlotElement);
            }

            Player.SelectedMenuOption = _selectedMenuOption;
            ScrollToSelected();
        }
    }

    public SlotElement SelectedSlotElement
    {
        get
        {
            if (SelectedMenuOption != null)
                return SlotElements[_selectedIndex].Value;
            else
                return null;
        }
    }

    public List<KeyValuePair<MenuOption, SlotElement>> SlotElements { get; set; } = new List<KeyValuePair<MenuOption, SlotElement>>();

    private int _selectedIndex;
    private ToggleGroup _toggleGroup;

    private void Awake()
    {
        _toggleGroup = GetComponent<ToggleGroup>();
    }

    public T GetPickedSlotElement<T>() where T : SlotElement
    {
        return SelectedSlotElement as T;
    }

    public IEnumerable<T> GetSlotElements<T>() where T : SlotElement
    {
        foreach (var slotElement in SlotElements)
            if (slotElement.Value is T)
                yield return slotElement.Value as T;
    }

    private void Update()
    {
        UpdateArrowsVisibility();
    }

    private void UpdateArrowsVisibility()
    {
        if (Player.SelectedMenuOption == null)
            return;

        var showArrows = SlotElements.ContainsKey(Player.SelectedMenuOption);

        arrowUp.gameObject.SetActive(showArrows && SelectedMenuOption.transform.GetSiblingIndex() > 0);
        arrowDown.gameObject.SetActive(showArrows && SelectedMenuOption.transform.GetSiblingIndex() < contentParent.childCount - 1);
    }

    private void RemoveExtraMenuOptions(List<SlotElement> slotElements)
    {
        var toRemove = new List<KeyValuePair<MenuOption, SlotElement>>();

        foreach (var pair in SlotElements)
        {
            if (!slotElements.Contains(pair.Value))
            {
                toRemove.Add(pair);
            }
        }

        foreach (var kvp in toRemove)
        {
            Destroy(kvp.Key);
            SlotElements.Remove(kvp);
        }
    }

    private void AddMissingMenuOptions(List<SlotElement> slotElements)
    {
        foreach (var slotElement in slotElements)
        {
            if (!SlotElements.Any(pair => pair.Value == slotElement))
            {
                var menuOption = Instantiate(Player.menuOptionPrefab, contentParent);
                menuOption.Toggle.group = _toggleGroup;
                menuOption.mainImage.sprite = slotElement.sprite;
                menuOption.name = slotElement.GetType().ToString() + slotElement.slotElementName;
                SlotElements.Add(new KeyValuePair<MenuOption, SlotElement>(menuOption, slotElement));
            }
        }
    }

    public void SelectFirst()
    {
        Select(SlotElements.First().Key);
    }

    public void Refresh(List<SlotElement> slotElements)
    {
        RemoveExtraMenuOptions(slotElements);
        AddMissingMenuOptions(slotElements);
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

            menuOptions[i].RectTransform.DOLocalMove(new Vector2(0f, targetY), Player.scrollSpeed);
        }
    }

    public void MoveSelection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Down:
                _selectedIndex++;

                var maxSlotElements = SlotElements.Count - 1;
                if (_selectedIndex > maxSlotElements)
                    _selectedIndex = maxSlotElements;
                break;
            case Direction.Up:
                _selectedIndex--;

                if (_selectedIndex < 0)
                    _selectedIndex = 0;
                break;
        }

        Select(SlotElements[_selectedIndex].Key);
    }

    public void Select(MenuOption menuOption)
    {
        if (menuOption == null)
            return;

        menuOption.Select();

        SelectedMenuOption = menuOption;
    }
}