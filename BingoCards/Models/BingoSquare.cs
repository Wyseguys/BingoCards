namespace BingoCards.Models
{
    public class BingoSquare
    {
        public int Id { get; set; }
        public string? Value { get; set; }
        public bool Selected { get; set; }
        public bool FreeSpace { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
