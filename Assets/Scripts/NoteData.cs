using System.Collections.Generic;

[System.Serializable]
public class NoteData
{
    public float timestamp;
    public char left;
    public char right;
    public char up;
    public char down;
    public List<char> Chars { get; set; }

    public bool IsEmpty
    {
        get
        {
            return left == '0' && right == '0' && up == '0' && down == '0';
        }
    }

    public NoteData(char left, char right, char up, char down)
    {
        this.left = left;
        this.right = right;
        this.up = up;
        this.down = down;

        Chars = new List<char>() { this.left, this.right, this.up, this.down };
    }

    public NoteData(string noteLine) : this(noteLine[0], noteLine[1], noteLine[2], noteLine[3])
    {

    }

    public override string ToString()
    {
        return left.ToString() + up.ToString() + right.ToString() + down.ToString() + " at " + timestamp;
    }
}
