using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum MenuState
{
    Inventory,
    Action,
    Magic,
    Ready,
    None
}

public class Player : SerializedMonoBehaviour
{
    public List<Item> startingItems = new List<Item>();
    public List<Skill> startingSkills = new List<Skill>();

    public Loadout Loadout { get; set; }
    public bool IsDancing { get; set; }
    public Conductor Conductor { get; private set; }
    public PlayerMenu PlayerMenu { get; private set; }
    //public List<LoadoutObject> LoadoutObjects { get; set; } = new List<LoadoutObject>();
    public List<Item> Items { get; set; } = new List<Item>();
    public List<Skill> Skills { get; set; } = new List<Skill>();

    public bool IsReady
    {
        get
        {
            return PlayerMenu.CurrentMenuState == MenuState.Ready;
        }
    }

    private void Start()
    {
        Conductor = GetComponentInChildren<Conductor>();
        Conductor.PlayerController = this;

        PlayerMenu = GetComponentInChildren<PlayerMenu>();
        PlayerMenu.Player = this;

        InitItems();
        InitSkills();
    }

    private void InitItems()
    {
        foreach (var item in startingItems)
        {
            Items.Add(item);
        }
    }

    private void InitSkills()
    {
        foreach (var skill in startingSkills)
        {
            Skills.Add(skill);
        }
    }

    private void Update()
    {
        UpdateInputs();
    }

    private void UpdateInputs()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ButtonPressed(Direction.Left);
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            ButtonPressed(Direction.Up);
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ButtonPressed(Direction.Down);
        }

        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ButtonPressed(Direction.Right);
        }
    }

    public void StartDancing()
    {
        PlayerMenu.ExitMenu();
        foreach (var conductor in FindObjectsOfType<Conductor>())
            conductor.Play();
        IsDancing = true;
    }

    private void ButtonPressed(Direction direction)
    {
        if (IsDancing)
        {
            Conductor.JudgeHit(direction);
        }
        else
        {
            PlayerMenu.MoveSelection(direction);
        }
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

        PlayerMenu.Select(PlayerMenu.loadoutSlotA.MenuOptions.First().Value.GetComponent<Selectable>());
    }
}
