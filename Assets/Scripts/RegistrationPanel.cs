using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegistrationPanel : UIBaseBehaviour
{
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


}
