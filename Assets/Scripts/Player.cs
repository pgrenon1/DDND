using DG.Tweening;
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
    public Image leftArrow;
    public Image upArrow;
    public Image downArrow;
    public Image rightArrow;
    public ClassPickPanel classPickPanel;
    public UIBaseBehaviour readyPanel;

    [Header("Energy")]
    public TextMeshProUGUI energyText;
    public Image energyFill;

    [Header("Settings")]
    public MenuOption menuOptionPrefab;
    public float scrollSpeed = 0.15f;

    public Skill ActiveSkill { get; set; }
    public bool IsInit { get; set; }
    public PlayerClass PlayerClass { get; set; }
    public Conductor Conductor { get; private set; }
    public LoadoutPanel LoadoutPanel { get; private set; }
    public List<Item> Items { get; set; } = new List<Item>();
    public List<Skill> Skills { get; set; } = new List<Skill>();
    public Targetable CurrentTarget { get; set; }
    public bool IsReady { get; set; }
    public float Energy { get; set; }
    public PlayerActions Actions { get; set; }
    public PlayerParent PlayerParent { get; set; }
    public int PlayerLevel { get; set; }
    public MenuOption SelectedMenuOption { get; set; }

    public float EnergyMax
    {
        get
        {
            return PlayerClass.maxEnergy.GetValue(PlayerLevel);
        }
    }

    public void Awake()
    {
        Conductor = GetComponentInChildren<Conductor>();
        Conductor.Player = this;

        LoadoutPanel = GetComponentInChildren<LoadoutPanel>();
        LoadoutPanel.Player = this;
    }

    public void InitPlayer(PlayerClass playerClass)
    {
        PlayerClass = playerClass;

        // Loadout
        InitItems();
        InitSkills();
        LoadoutPanel.InitLoadoutSlots();

        // Stats
        Damageable.maxHealth = playerClass.maxHealth.GetValue(PlayerLevel);
        Damageable.CurrentHealth = Damageable.maxHealth;
        Energy = EnergyMax;
    }

    private void InitItems()
    {
        foreach (var itemData in PlayerClass.startingItemDatas)
        {
            var newItem = new Item(itemData, this);
            Items.Add(newItem);
        }
    }

    private void InitSkills()
    {
        foreach (var skillData in PlayerClass.startingSkillDatas)
        {
            Skills.Add(new Skill(skillData, this));
        }
    }

    private void Update()
    {
        GameManager.Instance.State.HandleInputs(this);

        if (IsReady)
        {
            readyPanel.Show();
        }
        else
        {
            readyPanel.Hide();
        }
    }

    public void UpdateEnergy()
    {
        energyText.text = Mathf.FloorToInt(Energy).ToString();
        energyFill.fillAmount = Energy / EnergyMax;
    }

    public void DirectionFeedback(Direction direction)
    {
        var arrow = GetArrowFromDirection(direction);
        arrow.transform.DOPunchScale(Vector3.one / 3f, 0.15f).From();
    }

    private Image GetArrowFromDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                return leftArrow;
            case Direction.Up:
                return upArrow;
            case Direction.Down:
                return downArrow;
            case Direction.Right:
                return rightArrow;
            default:
                Debug.LogWarning("Invalid Direction, WTF");
                return null;
        }
    }

    public void ActivateSkill(CornerButton button)
    {
        var loadoutSlot = LoadoutPanel.LoadoutSlots[(int)button].Key;
        var slotElement = loadoutSlot.SelectedSlotElement;

        //var item = loadoutSlotElement as Item;
        //if (item != null)
        //{
        //    //"Activate" item?
        //}

        var skill = slotElement as Skill;
        if (skill != null && ActiveSkill == null)
        {
            skill.Activate();
            ActiveSkill = skill;
        }
    }

    //private bool HasActiveSkill()
    //{
    //    foreach (var slot in LoadoutPanel.LoadoutSlots)
    //    {
    //        var skill = slot.Key.SelectedSlotElement as Skill;
    //        if (skill != null && skill.IsActive)
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    private LoadoutSlotElement GetLoadoutSlotElementFromButton(CornerButton button)
    {
        var slot = LoadoutPanel.LoadoutSlots[(int)button];
        return slot.Key.GetPickedSlotElement<Skill>();
    }
}
