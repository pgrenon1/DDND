using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClassPickPanel : UIBaseBehaviour
{
    public GameObject pickAClassParent;

    public Slot PlayerClassSlot { get; private set; }
    public PlayerClassInfoPanel PlayerClassInfoPanel { get; private set; }

    private Player _player;

    private void Start()
    {
        PlayerClassSlot = GetComponentInChildren<Slot>();
        PlayerClassSlot.SelectionChanged += PlayerClassSlot_SelectionChanged;

        _player = GetComponentInParent<Player>();
        PopulatePlayerClasses();

        PlayerClassInfoPanel = GetComponentInChildren<PlayerClassInfoPanel>();

        if (!PlayerClassSlot.SelectedMenuOption)
            PlayerClassSlot.SelectFirst();
    }

    private void PlayerClassSlot_SelectionChanged(SlotElement slotElement)
    {
        var playerClassSlotElement = slotElement as PlayerClassSlotElement;
        PlayerClassInfoPanel.RefreshContent(playerClassSlotElement.PlayerClass);
    }

    private void PopulatePlayerClasses()
    {
        var playerClassSlotElements = new List<SlotElement>();

        foreach (var playerClass in GameManager.Instance.playerClasses)
        {
            var playerClassSlotElement = new PlayerClassSlotElement(playerClass);
            playerClassSlotElements.Add(playerClassSlotElement);
        }

        PlayerClassSlot.Refresh(playerClassSlotElements);
    }

    private void Update()
    {
        pickAClassParent.SetActive(!_player.IsReady);
    }
}
