using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : SingletonMonoBehaviour<Inventory>
{
    [Header("Content")]
    public Transform gridParent;
    public ItemMenuOption slotPrefab;
    public float scrollSpeed = 0.5f;
    [Header("Details")]
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI songTitleText;
    public TextMeshProUGUI songArtistText;
    public Image itemImage;
    public TextMeshProUGUI attackRatingText;
    public TextMeshProUGUI defenseRatingText;
    public TextMeshProUGUI magicRatingText;

    public List<ItemMenuOption> ItemMenuOptions { get; private set; } = new List<ItemMenuOption>();
    public ScrollRect ScrollRect { get; private set; }
    //public ItemMenuOption Current

    private CanvasGroup _canvasGroup;
    public CanvasGroup CanvasGroup
    {
        get
        {
            if (!_canvasGroup)
                _canvasGroup = GetComponent<CanvasGroup>();

            return _canvasGroup;
        }
    }

    private bool _isVisible;
    public bool IsVisible
    {
        get
        {
            return _isVisible;
        }
        private set
        {
            _isVisible = value;

            ApplyVisibility();
        }
    }


    private void Start()
    {
        ScrollRect = GetComponentInChildren<ScrollRect>();
    }

    public void ShowInfo(Item item)
    {
        //itemNameText.text = item.itemName;
        //descriptionText.text = item.description;
        //songTitleText.text = item.songData.title;
        //songArtistText.text = item.songData.artist;
        itemImage.sprite = item.sprite;
        //attackRatingText.text = item.attackRating.ToString();
        //defenseRatingText.text = item.defenseRating.ToString();
        //magicRatingText.text = item.magicRating.ToString();
    }

    public void UpdateScrollRectPosition(ItemMenuOption selected)
    {
        var index = ItemMenuOptions.IndexOf(selected);
        float target = (1 - (index / (float)(ItemMenuOptions.Count - 1))).Remap(0f, 1f, -0.5f, 1.5f);
        DOTween.To(() => ScrollRect.verticalScrollbar.value, x => ScrollRect.verticalScrollbar.value = x, target, scrollSpeed);
    }

    private void ApplyVisibility()
    {
        CanvasGroup.alpha = IsVisible ? 1f : 0f;
        CanvasGroup.interactable = IsVisible;
    }

    public void PopulateInventory(List<Item> newItems)
    {
        foreach (var item in ItemMenuOptions)
        {
            Destroy(item.gameObject);
        }

        ItemMenuOptions.Clear();

        foreach (var newItem in newItems)
        {
            var itemMenuOption = Instantiate(slotPrefab, gridParent);
            itemMenuOption.Item = newItem;

            ItemMenuOptions.Add(itemMenuOption);
        }
    }

    public void Show()
    {
        IsVisible = true;
    }

    public void Hide()
    {
        IsVisible = false;
    }
}