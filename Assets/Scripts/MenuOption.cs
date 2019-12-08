using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuOption : UIBaseBehaviour
{
    public TextMeshProUGUI optionText;
    public Image mainImage;

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
    }

    public void Deselect()
    {

    }

    public virtual void Confirm()
    {
        OnOptionConfirm();
    }
}
