using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuOption : BaseBehaviour
{
    public TextMeshProUGUI optionText;
    public Image mainImage;
    public Image leftArrow;
    public Image rightArrow;

    public delegate void OnConfirm();
    public OnConfirm OnOptionConfirm;

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

    public virtual void Select()
    {
        Toggle.Select();

        if (leftArrow)
            leftArrow.enabled = true;

        if (rightArrow)
            rightArrow.enabled = true;
    }

    public void Deselect()
    {
        if (leftArrow)
            leftArrow.enabled = false;

        if (rightArrow)
            rightArrow.enabled = false;
    }

    public virtual void Confirm()
    {
        OnOptionConfirm();
    }
}
