using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegistrationPanel : UIBaseBehaviour
{
    public Slot PlayerClassSlot { get; set; }

    private Player _player;

    private void Start()
    {
        PlayerClassSlot = GetComponentInChildren<Slot>();
        _player = GetComponentInParent<Player>();

        PopulatePlayerClasses();
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

    //private void Update()
    //{
    //    if (!_hasJoined && Input.anyKeyDown)
    //    {
    //        _hasJoined = true;

    //        pressAnyKey.gameObject.SetActive(false);
    //    }
    //}
}
