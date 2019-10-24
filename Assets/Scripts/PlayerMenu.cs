using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenu : MonoBehaviour
{
    public MenuOption startingOption;
    [Header("Main")]
    public ToggleGroup actionToggleGroup;

    [Header("Magic")]
    public ToggleGroup magicToggleGroup;
    public List<MenuOption> spellsToggles = new List<MenuOption>();

    private MenuOption _selected;
    public MenuOption Selected
    {
        get
        {
            return _selected;
        }
        private set
        {
            if (_selected)
                _selected.Deselect();

            _selected = value;
            _selected.Select();
        }
    }

    private void Start()
    {
        Selected = startingOption;
    }

    public void MoveSelection(Direction direction)
    {
        Selectable newSelected = null;
        switch (direction)
        {
            case Direction.Left:
                newSelected = Selected.Toggle.FindSelectableOnLeft();
                break;
            case Direction.Down:
                newSelected = Selected.Toggle.FindSelectableOnDown();
                break;
            case Direction.Up:
                newSelected = Selected.Toggle.FindSelectableOnUp();
                break;
            case Direction.Right:
                newSelected = Selected.Toggle.FindSelectableOnRight();
                break;
        }

        if (newSelected != null)
        {
            Selected = newSelected.GetComponent<MenuOption>();
        }
    }
}
