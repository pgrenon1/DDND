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
                return SlotElements[SelectedMenuOption];
            else
                return null;
        }
    }


    public Dictionary<MenuOption, SlotElement> SlotElements { get; set; } = new Dictionary<MenuOption, SlotElement>();

    private ToggleGroup _toggleGroup;
    private InfoPanel _infoPanel;

    private void Awake()
    {
        _toggleGroup = GetComponent<ToggleGroup>();
    }

    //private void Start()
    //{

    //    SelectFirst();
    //}

    public T GetPickedSlotElement<T>() where T : SlotElement
    {
        return SelectedSlotElement as T;
    }
    //private void Start()
    //{
    //    Player.SelectionChanged += PlayerMenu_SelectionChanged;
    //}

    //private void PlayerMenu_SelectionChanged(MenuOption selected)
    //{
    //    if (SlotElements.ContainsKey(selected))
    //    {
    //        Select(selected.Toggle);
    //    }
    //}

    public IEnumerable<T> GetSlotElements<T>() where T : SlotElement
    {
        foreach (var slotElement in SlotElements)
            if (slotElement.Value is T)
                yield return slotElement.Value as T;
    }

    //private void SetPicked(MenuOption newPicked)
    //{
    //    SelectedMenuOption = newPicked;
    //    ScrollToSelected();

    //    if (_infoPanel != null)
    //        _infoPanel.RefreshContent(SelectedSlotElement);
    //}

    private void UpdateInteractable()
    {
        //foreach (var menuOption in SlotElements)
        //{
        //    menuOption.Key.Toggle.interactable = PlayerMenu.LoadoutSlots[PlayerMenu.FocusedLoadoutSlotIndex].Key == this;
        //}
    }

    private void Update()
    {
        UpdateArrowsVisibility();

        UpdateInteractable();
    }

    private void UpdateArrowsVisibility()
    {
        if (Player.SelectedMenuOption == null)
            return;

        var showArrows = SlotElements.ContainsKey(Player.SelectedMenuOption);

        arrowUp.gameObject.SetActive(showArrows && SelectedMenuOption.transform.GetSiblingIndex() > 0);
        arrowDown.gameObject.SetActive(showArrows && SelectedMenuOption.transform.GetSiblingIndex() < contentParent.childCount - 1);
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

    private void AddMissingMenuOptions(List<SlotElement> slotElements)
    {
        foreach (var slotElement in slotElements)
        {
            if (!SlotElements.Any(pair => pair.Value == slotElement))
            {
                var menuOption = Instantiate(Player.menuOptionPrefab, contentParent);
                menuOption.Toggle.group = _toggleGroup;
                menuOption.mainImage.sprite = slotElement.sprite;
                menuOption.name = slotElement.GetType().ToString() + slotElement.elementName;
                SlotElements.Add(menuOption, slotElement);
            }
        }
    }

    public void SelectFirst()
    {
        Select(SlotElements.First().Key.Toggle);
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