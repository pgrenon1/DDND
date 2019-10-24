using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool IsPlaying { get; set; }
    public Conductor Conductor { get; private set; }
    public PlayerMenu PlayerMenu { get; private set; }

    public bool IsActivePlayer
    {
        get
        {
            return GameManager.Instance.ActivePlayer == this;
        }
    }

    private void Start()
    {
        Conductor = GetComponentInChildren<Conductor>();
        Conductor.PlayerController = this;

        PlayerMenu = GetComponentInChildren<PlayerMenu>();
    }

    private void Update()
    {
        if (!IsPlaying && !IsActivePlayer)
            return;

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

    private void ButtonPressed(Direction direction)
    {
        if (IsPlaying)
        {
            Conductor.JudgeHit(direction);
        }
        else
        {
            PlayerMenu.MoveSelection(direction);
        }
    }
}
