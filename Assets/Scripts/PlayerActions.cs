using InControl;

public class PlayerActions : PlayerActionSet
{
    public PlayerAction Left;
    public PlayerAction Up;
    public PlayerAction Down;
    public PlayerAction Right;

    public PlayerAction Cross;
    public PlayerAction Circle;
    public PlayerAction Triangle;
    public PlayerAction Square;

    public PlayerActions()
    {
        Left = CreatePlayerAction("Left");
        Up = CreatePlayerAction("Up");
        Down = CreatePlayerAction("Down");
        Right = CreatePlayerAction("Right");

        Cross = CreatePlayerAction("Cross");
        Circle = CreatePlayerAction("Circle");
        Triangle = CreatePlayerAction("Triangle");
        Square = CreatePlayerAction("Square");
    }

    public static PlayerActions CreateWithKeyboardBindings()
    {
        var actions = new PlayerActions();

        actions.Cross.AddDefaultBinding(Key.Q);
        actions.Circle.AddDefaultBinding(Key.E);
        actions.Triangle.AddDefaultBinding(Key.Z);
        actions.Square.AddDefaultBinding(Key.C);

        actions.Up.AddDefaultBinding(Key.UpArrow);
        actions.Down.AddDefaultBinding(Key.DownArrow);
        actions.Left.AddDefaultBinding(Key.LeftArrow);
        actions.Right.AddDefaultBinding(Key.RightArrow);

        return actions;
    }

    public static PlayerActions CreateWithJoystickBindings()
    {
        var actions = new PlayerActions();

        actions.Square.AddDefaultBinding(InputControlType.Action1);
        actions.Circle.AddDefaultBinding(InputControlType.Action2);
        actions.Cross.AddDefaultBinding(InputControlType.Action3);
        actions.Triangle.AddDefaultBinding(InputControlType.Action4);

        actions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
        actions.Down.AddDefaultBinding(InputControlType.LeftStickDown);
        actions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        actions.Right.AddDefaultBinding(InputControlType.LeftStickRight);

        actions.Up.AddDefaultBinding(InputControlType.DPadUp);
        actions.Down.AddDefaultBinding(InputControlType.DPadDown);
        actions.Left.AddDefaultBinding(InputControlType.DPadLeft);
        actions.Right.AddDefaultBinding(InputControlType.DPadRight);

        return actions;
    }
}