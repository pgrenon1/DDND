using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameState
{
    void OnEnter(GameManager gameManager);

    void OnExit(GameManager gameManager);

    void ToState(GameManager gameManager, IGameState targetState);

    void Update(GameManager gameManager);
}

public class GameStateBase : IGameState
{
    public static readonly GameStateRegistration gameStateRegistration;
    public static readonly GameStateLoadout gameStateLoadout;
    public static readonly GameStateBattle gameStateBattle;

    public virtual void OnEnter(GameManager gameManager)
    {

    }

    public virtual void OnExit(GameManager gameManager)
    {

    }

    public virtual void ToState(GameManager gameManager, IGameState targetState)
    {

    }

    public virtual void Update(GameManager gameManager)
    {

    }
}

public class GameStateRegistration : GameStateBase
{
    public override void Update(GameManager gameManager)
    {
        base.Update(gameManager);

        if (PlayerManager.Instance.Players.TrueForAll(x => x.IsReady))
        {
            ToState(gameManager, gameStateLoadout);
        }
    }
}

public class GameStateLoadout : GameStateBase
{
    public override void Update(GameManager gameManager)
    {
        base.Update(gameManager);

        if (PlayerManager.Instance.Players.TrueForAll(x => x.IsReady))
        {
            ToState(gameManager, gameStateBattle);
        }
    }

    //Entering Loadout State
    public override void OnEnter(GameManager gameManager)
    {
        base.OnEnter(gameManager);

        foreach (var player in PlayerManager.Instance.Players)
        {
            player.State.ToState(player, player.playerStateLoadout);
        }
    }
}

public class GameStateBattle : GameStateBase
{

}