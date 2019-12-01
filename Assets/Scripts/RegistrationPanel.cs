using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistrationPanel : UIBaseBehaviour
{

    private Slot<PlayerClassSlotElement> _playerClassSlot;
    private Player _player;

    private void Start()
    {
        _playerClassSlot = GetComponent<Slot<PlayerClassSlotElement>>();
        _player = GetComponentInParent<Player>();

        PopulateClasses();
    }

    private void PopulateClasses()
    {
        foreach (var playerClass in GameManager.Instance.playerClasses)
        {
            var playerClassSlotElement = new PlayerClassSlotElement(playerClass);
        }
    }
}
