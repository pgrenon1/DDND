[System.Serializable]
public class Bar
{
    public char Left { get; set; }
    public char Up { get; set; }
    public char Right { get; set; }
    public char Down { get; set; }

    public Bar(char left, char up, char right, char down)
    {
        Left = left;
        Up = up;
        Right = right;
        Down = down;
    }
}
