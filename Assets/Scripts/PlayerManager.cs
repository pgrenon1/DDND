using InControl;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    public Player playerPrefab;
    public List<PlayerParent> playerParents = new List<PlayerParent>(_maxPlayers);

    public List<Player> Players { get; set; } = new List<Player>(_maxPlayers);

    private const int _maxPlayers = 4;
    private PlayerActions _keyboardListener;
    private PlayerActions _joystickListener;

    private void OnEnable()
    {
        InputManager.OnDeviceDetached += OnDeviceDetached;
        _keyboardListener = PlayerActions.CreateWithKeyboardBindings();
        _joystickListener = PlayerActions.CreateWithJoystickBindings();
    }

    private void OnDisable()
    {
        InputManager.OnDeviceDetached -= OnDeviceDetached;
        _joystickListener.Destroy();
        _keyboardListener.Destroy();
    }

    private void Update()
    {
        if (JoinButtonWasPressedOnListener(_joystickListener))
        {
            var inputDevice = InputManager.ActiveDevice;

            if (ThereIsNoPlayerUsingJoystick(inputDevice))
            {
                CreatePlayer(inputDevice);
            }
        }

        if (JoinButtonWasPressedOnListener(_keyboardListener))
        {
            if (ThereIsNoPlayerUsingKeyboard())
            {
                CreatePlayer(null);
            }
        }
    }

    private Player CreatePlayer(InputDevice inputDevice)
    {
        if (Players.Count < _maxPlayers)
        {
            // Pop a position off the list. We'll add it back if the player is removed.
            var playerParent = playerParents[0];
            playerParents.RemoveAt(0);

            var player = Instantiate(playerPrefab, playerParent.transform);

            playerParent.regristrationPrompt.SetActive(false);

            if (inputDevice == null)
            {
                // We could create a new instance, but might as well reuse the one we have
                // and it lets us easily find the keyboard player.
                player.Actions = _keyboardListener;
            }
            else
            {
                // Create a new instance and specifically set it to listen to the
                // given input device (joystick).
                var actions = PlayerActions.CreateWithJoystickBindings();
                actions.Device = inputDevice;

                player.Actions = actions;
            }

            Players.Add(player);

            return player;
        }

        return null;
    }

    private bool ThereIsNoPlayerUsingKeyboard()
    {
        return FindPlayerUsingKeyboard() == null;
    }

    private Player FindPlayerUsingKeyboard()
    {
        var playerCount = Players.Count;
        for (var i = 0; i < playerCount; i++)
        {
            var player = Players[i];
            if (player.Actions == _keyboardListener)
            {
                return player;
            }
        }

        return null;
    }

    private bool ThereIsNoPlayerUsingJoystick(InputDevice inputDevice)
    {
        return FindPlayerUsingJoystick(inputDevice) == null;
    }

    private Player FindPlayerUsingJoystick(InputDevice inputDevice)
    {
        var playerCount = Players.Count;
        for (var i = 0; i < playerCount; i++)
        {
            var player = Players[i];
            if (player.Actions.Device == inputDevice)
            {
                return player;
            }
        }

        return null;
    }

    private bool JoinButtonWasPressedOnListener(PlayerActions actions)
    {
        return actions.Device.AnyButton.WasPressed;
    }

    private void OnDeviceDetached(InputDevice inputDevice)
    {
        var player = FindPlayerUsingJoystick(inputDevice);
        if (player != null)
        {
            RemovePlayer(player);
        }
    }

    private void RemovePlayer(Player player)
    {
        player.PlayerParent.regristrationPrompt.SetActive(true);
        playerParents.Insert(0, player.PlayerParent);
        Players.Remove(player);
        player.Actions = null;
        Destroy(player.gameObject);
    }
}
