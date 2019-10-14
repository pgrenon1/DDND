[System.Serializable]
public class Note
{
    public float Timestamp { get; set; }
    public char Left { get; set; }
    public char Up { get; set; }
    public char Right { get; set; }
    public char Down { get; set; }
    public bool IsEmpty
    {
        get
        {
            return Left == '0' && Right == '0' && Up == '0' && Down == '0';
        }
    }

    public Note(char left, char up, char right, char down)
    {
        Left = left;
        Up = up;
        Right = right;
        Down = down;
    }

    public Note(string noteLine)
    {
        Left = noteLine[0];
        Up = noteLine[1];
        Right = noteLine[2];
        Down = noteLine[3];
    }

    public override string ToString()
    {
        return Left.ToString() + Up.ToString() + Right.ToString() + Down.ToString() + " at " + Timestamp;
    }
}
