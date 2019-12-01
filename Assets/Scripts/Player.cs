using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : Targetable
{
    public int playerLevel;

    [Header("PlayerClass")]
    public PlayerClass playerClass;

    [Header("Energy")]
    public TextMeshProUGUI energyText;
    public Image energyFill;

    public bool IsDancing { get; set; }
    public Conductor Conductor { get; private set; }
    public PlayerMenu PlayerMenu { get; private set; }
    public List<Item> Items { get; set; } = new List<Item>();
    public List<Skill> Skills { get; set; } = new List<Skill>();
    public Targetable CurrentTarget { get; set; }
    public bool IsReady { get; set; }
    public float Energy { get; set; }
    public float EnergyMax
    {
        get
        {
            return playerClass.maxEnergy.GetValue(playerLevel);
        }
    }

    public void Init()
    {
        Conductor = GetComponentInChildren<Conductor>();
        Conductor.Player = this;

        PlayerMenu = GetComponentInChildren<PlayerMenu>();
        PlayerMenu.Player = this;

        //TEMP
        InitPlayer(playerClass);
    }

    public void InitPlayer(PlayerClass playerClass)
    {
        //Temp
        this.playerClass = playerClass;
        InitItems();
        InitSkills();

        Energy = EnergyMax;
    }

    private void InitItems()
    {
        foreach (var itemData in playerClass.startingItemDatas)
        {
            var newItem = new Item(itemData);
            newItem.Owner = this;
            Items.Add(newItem);
        }
    }

    private void InitSkills()
    {
        foreach (var skillData in playerClass.startingSkillDatas)
        {
            Skills.Add(new Skill(skillData));
        }
    }

    private void Update()
    {
        UpdateInputs();

        UpdateEnergy();
    }

    private void UpdateEnergy()
    {
        energyText.text = Mathf.FloorToInt(Energy).ToString();
        energyFill.fillAmount = Energy / EnergyMax;
    }

    private void UpdateInputs()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            DirectionPressed(Direction.Left);
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            DirectionPressed(Direction.Up);
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            DirectionPressed(Direction.Down);
        }

        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            DirectionPressed(Direction.Right);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerMenu.Confirm();
        }
    }

    public void StartDancing()
    {
        PlayerMenu.gameObject.SetActive(false);

        Conductor.Play();

        IsDancing = true;
    }

    private void DirectionPressed(Direction direction)
    {
        if (IsDancing)
        {
            Conductor.JudgeHit(direction);
        }
        else
        {
            PlayerMenu.MoveSelection(direction);
        }

        Conductor.DirectionFeedback(direction);
    }

    public void PickLoadout()
    {
        PlayerMenu.InitLoadoutSlots(playerClass);

        PlayerMenu.RefreshLoadout();

        PlayerMenu.Select(PlayerMenu.LoadoutSlots.First().Key.GetComponent<Selectable>());
    }
}
