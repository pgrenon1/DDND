using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuOption : MonoBehaviour
{
    public TextMeshProUGUI optionText;
    public Image mainImage;
    public Image leftArrow;
    public Image rightArrow;

    private Toggle _toggle;
    public Toggle Toggle
    {
        get
        {
            if (_toggle == null)
                _toggle = GetComponent<Toggle>();

            return _toggle;
        }
    }

    private bool _isSelected;

    public void Select()
    {
        _isSelected = true;
        Toggle.Select();

        if (leftArrow)
            leftArrow.enabled = true;

        if (rightArrow)
            rightArrow.enabled = true;
    }

    public void Deselect()
    {
        _isSelected = false;

        if (leftArrow)
            leftArrow.enabled = false;

        if (rightArrow)
            rightArrow.enabled = false;
    }
}
