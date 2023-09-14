namespace _2048.Models
{
    public class Tile
    {
        public int Value { get; set; }
        public bool HasMerged { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public Tile(int value, int row, int column)
        {
            Value = value;
            Row = row;
            Column = column;
            HasMerged = false;
        }

        public void SetMergeStatus(bool status)
        {
            HasMerged = status;
        }
    }
}