using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void OnEnter(Player player);

    void OnExit(Player player);

    void ToState(Player player, PlayerStateBase targetState);

    void Update(Player player);

    void HandleInputs(Player player);
}

public class PlayerStateBase : IPlayerState
{
    public virtual void OnEnter(Player player)
    {

    }

    public virtual void OnExit(Player player)
    {

    }

    public virtual void ToState(Player player, PlayerStateBase targetState)
    {
        //player.State.OnExit(player);
        //player.State = targetState;
        //player.State.OnEnter(player);
    }

    public virtual void Update(Player player)
    {
        HandleInputs(player);
    }

    public virtual void HandleInputs(Player player)
    {
        //if (player.Actions.Left.WasPressed)
        //{
        //    DirectionPressed(player, Direction.Left);
        //}

        //if (player.Actions.Up.WasPressed)
        //{
        //    DirectionPressed(player, Direction.Up);
        //}

        //if (player.Actions.Down.WasPressed)
        //{
        //    DirectionPressed(player, Direction.Down);
        //}

        //if (player.Actions.Right.WasPressed)
        //{
        //    DirectionPressed(player, Direction.Right);
        //}

        //if (player.Actions.Cross.WasPressed)
        //{
        //    ButtonPressed(player, CornerButton.Cross);
        //}

        //if (player.Actions.Circle.WasPressed)
        //{
        //    ButtonPressed(player, CornerButton.Circle);
        //}

        //if (player.Actions.Square.WasPressed)
        //{
        //    ButtonPressed(player, CornerButton.Square);
        //}

        //if (player.Actions.Triangle.WasPressed)
        //{
        //    ButtonPressed(player, CornerButton.Triangle);
        //}
    }

    protected virtual void ButtonPressed(Player player, CornerButton button)
    {

    }

    protected virtual void DirectionPressed(Player player, Direction direction)
    {
        player.DirectionFeedback(direction);
    }
}

public class PlayerStateRegistration : PlayerStateBase
{
    public override void Update(Player player)
    {
        base.Update(player);
    }

    protected override void DirectionPressed(Player player, Direction direction)
    {
        base.DirectionPressed(player, direction);

        player.registrationPanel.PlayerClassSlot.MoveSelection(direction);
    }

    public override void HandleInputs(Player player)
    {
        player.IsReady = player.Actions.Cross.IsPressed;

        base.HandleInputs(player);
    }

    public override void OnExit(Player player)
    {
        base.OnExit(player);

        var pickedPlayerClassSlotElement = player.registrationPanel.PlayerClassSlot.SelectedSlotElement as PlayerClassSlotElement;
        var pickedPlayerClass = pickedPlayerClassSlotElement.PlayerClass;

        player.InitPlayer(pickedPlayerClass);
    }
}

public class PlayerStateLoadout : PlayerStateBase
{
    public override void OnEnter(Player player)
    {
        base.OnEnter(player);

        player.PlayerMenu.RefreshLoadout();
    }

    public override void Update(Player player)
    {
        base.Update(player);
    }

    protected override void DirectionPressed(Player player, Direction direction)
    {
        base.DirectionPressed(player, direction);

        if (direction == Direction.Down || direction == Direction.Up)
            player.PlayerMenu.FocusedLoadoutSlot.MoveSelection(direction);
        else if (direction == Direction.Left || direction == Direction.Right)
            player.PlayerMenu.SwitchFocus(direction);
    }
}

public class PlayerStateBattle : PlayerStateBase
{
    public override void Update(Player player)
    {
        base.Update(player);

        player.UpdateEnergy();
    }

    protected override void DirectionPressed(Player player, Direction direction)
    {
        base.DirectionPressed(player, direction);

        player.Conductor.JudgeHit(direction);
    }

    protected override void ButtonPressed(Player player, CornerButton button)
    {
        base.ButtonPressed(player, button);

        player.ActivateSkill(button);
    }
}