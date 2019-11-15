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
    [Header("Loadout")]
    [AssetSelector]
    public List<ItemData> startingItemDatas = new List<ItemData>();
    [AssetSelector]
    public List<SkillData> startingSkillDatas = new List<SkillData>();

    [Header("Energy")]
    public TextMeshProUGUI energyText;
    public Image energyFill;
    public int energyMax = 50;
    public float energyGainPerSecond = 1f;

    public bool IsDancing { get; set; }
    public Conductor Conductor { get; private set; }
    public PlayerMenu PlayerMenu { get; private set; }
    public List<Item> Items { get; set; } = new List<Item>();
    public List<Skill> Skills { get; set; } = new List<Skill>();
    public bool IsReady { get; set; }
    public float Energy { get; set; }
    public Enemy CurrentTarget { get; set; }

    public void Init()
    {
        Conductor = GetComponentInChildren<Conductor>();
        Conductor.Player = this;

        PlayerMenu = GetComponentInChildren<PlayerMenu>();
        PlayerMenu.Player = this;

        InitItems();
        InitSkills();

        Energy = energyMax;
    }

    private void InitItems()
    {
        foreach (var itemData in startingItemDatas)
        {
            Items.Add(new Item(itemData));
        }
    }

    private void InitSkills()
    {
        foreach (var skillData in startingSkillDatas)
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
        var energyGain = Mathf.Min(energyGainPerSecond * Time.deltaTime, energyMax - Energy);

        Energy += energyGain;

        energyText.text = Mathf.CeilToInt(Energy).ToString();
        energyFill.fillAmount = Energy / energyMax;
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

    //public void StartTurn()
    //{
    //    if (GameManager.Instance.FirstPlayer == this)
    //    {
    //        PlayerMenu.InitInventoryMenu();
    //    }
    //}

    //public void EndTurn()
    //{
    //    GameManager.Instance.NextTurn();
    //}

    public void PickLoadout()
    {
        PlayerMenu.RefreshLoadout();

        PlayerMenu.Select(PlayerMenu.loadoutSlotA.LoadoutObjects.First().Key.GetComponent<Selectable>());
    }
}
