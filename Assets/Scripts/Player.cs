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
    public RegistrationPanel registrationPanel;

    [Header("Energy")]
    public TextMeshProUGUI energyText;
    public Image energyFill;

    [Header("Settings")]
    public MenuOption menuOptionPrefab;
    public float scrollSpeed = 0.15f;

    public PlayerClass PlayerClass { get; set; }
    public bool IsDancing { get; set; }
    public Conductor Conductor { get; private set; }
    public LoadoutPanel PlayerMenu { get; private set; }
    public List<Item> Items { get; set; } = new List<Item>();
    public List<Skill> Skills { get; set; } = new List<Skill>();
    public Targetable CurrentTarget { get; set; }
    public bool IsReady { get; set; }
    public float Energy { get; set; }
    public PlayerActions Actions { get; set; }
    public PlayerParent PlayerParent { get; set; }
    public int PlayerLevel { get; set; }
    public MenuOption SelectedMenuOption { get; set; }

    //public delegate void OnSelectionChanged(MenuOption Selected);
    //public event OnSelectionChanged SelectionChanged;

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

        PlayerMenu = GetComponentInChildren<LoadoutPanel>();
        PlayerMenu.Player = this;
    }

    public void InitPlayer(PlayerClass playerClass)
    {
        PlayerClass = playerClass;

        InitItems();
        InitSkills();
        PlayerMenu.InitLoadoutSlots();

        Energy = EnergyMax;
    }

    private void InitItems()
    {
        foreach (var itemData in PlayerClass.startingItemDatas)
        {
            var newItem = new Item(itemData);
            newItem.Owner = this;
            Items.Add(newItem);
        }
    }

    private void InitSkills()
    {
        foreach (var skillData in PlayerClass.startingSkillDatas)
        {
            Skills.Add(new Skill(skillData));
        }
    }

    private void Update()
    {
        GameManager.Instance.State.HandleInputs(this);
    }

    public void UpdateEnergy()
    {
        energyText.text = Mathf.FloorToInt(Energy).ToString();
        energyFill.fillAmount = Energy / EnergyMax;
    }

    public void StartDancing()
    {
        PlayerMenu.gameObject.SetActive(false);

        Conductor.Play();

        IsDancing = true;
    }

    //private void DirectionPressed(Direction direction)
    //{
    //if (IsDancing)
    //{
    //    Conductor.JudgeHit(direction);
    //}
    //else
    //{
    //    PlayerMenu.MoveSelection(direction);
    //}

    //DirectionFeedback(direction);
    //}

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
                Debug.LogWarning("Invalid Direction");
                return null;
        }
    }

    public void PickLoadout()
    {
        //PlayerMenu.InitLoadoutSlots(PlayerClass);

        //PlayerMenu.RefreshLoadout();

        //PlayerMenu.Select(PlayerMenu.LoadoutSlots.First().Key.GetComponent<Selectable>());
    }

    public void ActivateSkill(CornerButton button)
    {
        var loadoutSlotElement = GetLoadoutSlotElement(button);

        //var item = loadoutSlotElement as Item;
        //if (item != null)
        //{
        //    //"Activate" item?
        //}

        var skill = loadoutSlotElement as Skill;
        if (skill != null)
        {
            skill.Activate();
        }
    }

    private LoadoutSlotElement GetLoadoutSlotElement(CornerButton button)
    {
        var slot = PlayerMenu.LoadoutSlots[(int)button];
        return slot.Key.GetPickedSlotElement<Skill>();
    }
}
