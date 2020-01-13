using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Left = 0,
    Right = 1,
    Up = 2,
    Down = 3
}

public enum CornerButton
{
    Cross,
    Circle,
    Square,
    Triangle
}

public interface IGameState
{
    void OnEnter(GameManager gameManager);

    void OnExit(GameManager gameManager);

    void ToState(GameManager gameManager, GameStateBase targetState);

    void Update(GameManager gameManager);

    void HandleInputs(Player player);
}

public class GameStateBase : IGameState
{
    public static readonly GameStateBase gameStateBase = new GameStateBase();
    public static readonly GameStateRegistration gameStateRegistration = new GameStateRegistration();
    public static readonly GameStateLoadout gameStateLoadout = new GameStateLoadout();
    public static readonly GameStateBattle gameStateBattle = new GameStateBattle();
    public static readonly GameStateMap gameStateMap = new GameStateMap();

    protected List<Player> Players
    {
        get
        {
            return PlayerManager.Instance.Players;
        }
    }

    public virtual void OnEnter(GameManager gameManager)
    {

    }

    public virtual void OnExit(GameManager gameManager)
    {

    }

    public virtual void ToState(GameManager gameManager, GameStateBase targetState)
    {
        Debug.Log(gameManager.State + " -> <b>" + targetState + "</b>");
        gameManager.State.OnExit(gameManager);
        gameManager.State = targetState;
        gameManager.State.OnEnter(gameManager);
    }

    public virtual void Update(GameManager gameManager)
    {

    }

    public virtual void HandleInputs(Player player)
    {
        if (!player.IsInit)
        {
            player.IsInit = true;
            return;
        }

        if (player.Actions.Left.WasPressed)
        {
            DirectionPressed(player, Direction.Left);
        }

        if (player.Actions.Up.WasPressed)
        {
            DirectionPressed(player, Direction.Up);
        }

        if (player.Actions.Down.WasPressed)
        {
            DirectionPressed(player, Direction.Down);
        }

        if (player.Actions.Right.WasPressed)
        {
            DirectionPressed(player, Direction.Right);
        }

        if (player.Actions.Cross.WasPressed)
        {
            ButtonPressed(player, CornerButton.Cross);
        }

        if (player.Actions.Circle.WasPressed)
        {
            ButtonPressed(player, CornerButton.Circle);
        }

        if (player.Actions.Square.WasPressed)
        {
            ButtonPressed(player, CornerButton.Square);
        }

        if (player.Actions.Triangle.WasPressed)
        {
            ButtonPressed(player, CornerButton.Triangle);
        }
    }

    protected virtual void ButtonPressed(Player player, CornerButton button)
    {

    }

    protected virtual void DirectionPressed(Player player, Direction direction)
    {
        player.DirectionFeedback(direction);
    }
}

public class GameStateRegistration : GameStateBase
{
    public override void Update(GameManager gameManager)
    {
        base.Update(gameManager);

        if (Players.Count > 0 && Players.TrueForAll(x => x.IsReady))
        {
            ToState(gameManager, gameStateLoadout);
        }
    }

    protected override void ButtonPressed(Player player, CornerButton button)
    {
        base.ButtonPressed(player, button);

        if (button == CornerButton.Cross)
            player.IsReady = !player.IsReady;
    }

    protected override void DirectionPressed(Player player, Direction direction)
    {
        if (player.IsReady)
            return;

        base.DirectionPressed(player, direction);

        player.classPickPanel.PlayerClassSlot.MoveSelection(direction);
    }

    public override void OnExit(GameManager gameManager)
    {
        base.OnExit(gameManager);

        foreach (var player in Players)
        {
            var pickedPlayerClassSlotElement = player.classPickPanel.PlayerClassSlot.SelectedSlotElement as PlayerClassSlotElement;
            var pickedPlayerClass = pickedPlayerClassSlotElement.PlayerClass;

            player.InitPlayer(pickedPlayerClass);

            player.classPickPanel.Hide();
        }
    }
}

public class GameStateLoadout : GameStateBase
{
    public override void OnEnter(GameManager gameManager)
    {
        base.OnEnter(gameManager);

        foreach (var player in Players)
        {
            player.IsReady = false;
            player.LoadoutPanel.loadoutInfoPanel.Show();
            player.LoadoutPanel.FocusedLoadoutSlotIndex = 0;
            player.LoadoutPanel.FocusedLoadoutSlot.SelectFirst();
        }

        gameManager.SpawnEnemy();
    }

    public override void Update(GameManager gameManager)
    {
        base.Update(gameManager);

        if (Players.TrueForAll(x => x.IsReady))
        {
            ToState(gameManager, gameStateBattle);
        }
    }

    protected override void ButtonPressed(Player player, CornerButton button)
    {
        base.ButtonPressed(player, button);

        if (button == CornerButton.Cross)
            player.IsReady = !player.IsReady;
    }

    protected override void DirectionPressed(Player player, Direction direction)
    {
        base.DirectionPressed(player, direction);

        if (direction == Direction.Down || direction == Direction.Up)
            player.LoadoutPanel.FocusedLoadoutSlot.MoveSelection(direction);
        else if (direction == Direction.Left || direction == Direction.Right)
            player.LoadoutPanel.SwitchFocus(direction);
    }

    //TEMP
    public override void OnExit(GameManager gameManager)
    {
        base.OnExit(gameManager);

        foreach (var player in Players)
        {
            player.LoadoutPanel.loadoutInfoPanel.Hide();
            player.IsReady = false;
        }
    }
}

public class GameStateMap : GameStateBase
{
    public override void OnExit(GameManager gameManager)
    {
        base.OnExit(gameManager);

        foreach (var player in Players)
        {
            player.LoadoutPanel.loadoutInfoPanel.Hide();
            player.IsReady = false;
        }
    }
}

public class GameStateBattle : GameStateBase
{
    public override void OnEnter(GameManager gameManager)
    {
        base.OnEnter(gameManager);

        foreach (var player in Players)
        {
            player.CurrentTarget = gameManager.CurrentEnemy;
        }

        gameManager.PlaySong();
    }

    public override void Update(GameManager gameManager)
    {
        base.Update(gameManager);

        gameManager.UpdateSong();

        if (!gameManager.MusicSource.isPlaying)
            ToState(gameManager, gameStateLoadout);

        foreach (var player in Players)
        {
            player.Conductor.UpdateNotes();
        }
    }

    protected override void ButtonPressed(Player player, CornerButton button)
    {
        base.ButtonPressed(player, button);

        player.ActivateSkill(button);
    }

    protected override void DirectionPressed(Player player, Direction direction)
    {
        base.DirectionPressed(player, direction);

        player.Conductor.JudgeHit(direction);
    }

    public override void OnExit(GameManager gameManager)
    {
        base.OnExit(gameManager);

        foreach (var player in Players)
        {
            player.Conductor.ResetConductor();
        }
    }
}