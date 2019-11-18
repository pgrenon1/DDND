using System.Collections.Generic;

[System.Serializable]
public class NoteData
{
    public float timestamp;
    public char left;
    public char up;
    public char down;
    public char right;
    public List<char> Chars = new List<char>();

    public bool IsEmpty
    {
        get
        {
            return left == '0' && up == '0' && down == '0' && right == '0';
        }
    }

    public NoteData(char left, char up, char down, char right)
    {
        this.left = left;
        this.up = up;
        this.down = down;
        this.right = right;

        Chars = new List<char>() { this.left, this.up, this.down, this.right };
    }

    public NoteData(string noteLine) : this(noteLine[0], noteLine[1], noteLine[2], noteLine[3]) { }

    public override string ToString()
    {
        return left.ToString() + up.ToString() + right.ToString() + down.ToString() + " at " + timestamp;
    }
}
