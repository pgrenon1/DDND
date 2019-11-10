using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenuOption : MenuOption
{
    private Item _item;
    public Item Item
    {
        get
        {
            return _item;
        }
        set
        {
            _item = value;
            mainImage.sprite = _item.sprite;
        }
    }

    public override void Select()
    {
        base.Select();

        Inventory.Instance.ShowInfo(Item);
    }

    public override void Confirm()
    {
        //GameManager.Instance.ChooseSong(Item.songData);

        //GameManager.Instance.StartActionMenus();
    }
}
