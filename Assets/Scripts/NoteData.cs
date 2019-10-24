using System.Collections.Generic;

[System.Serializable]
public class NoteData
{
    public float Timestamp { get; set; }
    public char Left { get; set; }
    public char Right { get; set; }
    public char Up { get; set; }
    public char Down { get; set; }
    public List<char> Chars { get; set; }

    public bool IsEmpty
    {
        get
        {
            return Left == '0' && Right == '0' && Up == '0' && Down == '0';
        }
    }

    public NoteData(char left, char right, char up, char down)
    {
        Left = left;
        Right = right;
        Up = up;
        Down = down;

        Chars = new List<char>() { Left, Right, Up, Down };
    }

    public NoteData(string noteLine) : this(noteLine[0], noteLine[1], noteLine[2], noteLine[3])
    {

    }

    public override string ToString()
    {
        return Left.ToString() + Up.ToString() + Right.ToString() + Down.ToString() + " at " + Timestamp;
    }
}
