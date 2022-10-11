namespace BingoCards.Models
{
    public class BingoGame
    {
        public int Id { get; set; } 
        public string? Name { get; set; } = null;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; }=DateTime.Now;
        public bool IsWinner { get { return CheckNormalWin(); } }
        public bool IsBlackout { get { return CheckBlackout(); } }
        public bool HasFreeSpace { get; set; } = true;
        public bool IsReady { get; set; } = false;
        public List<BingoSquare>? Squares { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private List<string> Words {  get; set; }

        public BingoGame()
        {
            Words = new List<string>() { "You", "forgot", "to", "setup", "your", "bingo", "game", "my", "friend" };
        }
        public BingoGame(List<string> wordlist, string? name = "dude", string? freespacelabel = "FREE SPACE")
        {
            Words = wordlist ?? throw new ArgumentNullException(nameof(wordlist));
            if (!IsSaneGame()) throw new Exception("Word list should be 9, 16 or 25 entries");
            double sides = Math.Sqrt(Words.Count);

            Squares = SetupSquares(sides, freespacelabel);
            Rows = Convert.ToInt32(sides);
            Columns = Convert.ToInt32(sides);
            Name = name;
        }



        public bool CheckBlackout()
        {
            if(Squares == null) return false;
            if (Squares.Count(x => x.Selected) == Squares.Count)
            {
                return true;
            }
            return false;
        }

        public bool CheckNormalWin()
        {
            if (Squares == null) return false;

            return CheckHorizontalWin() || CheckVerticalWin() || CheckTopDownDiagonalWin() || CheckBottomUpDiagonalWin();
        }

        private bool CheckTopDownDiagonalWin()
        {
            if (Squares == null) return false;
            for (int i = 0; i < Rows; i++)
            {
                var sq = Squares.Where(x => x.Row == x.Column && x.Selected);
                if (sq.Count() == Rows)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckBottomUpDiagonalWin()
        {
            if (Squares == null) return false;
            int CheckColumn = 0;
            for (var j = (Rows-1); j >= 0; j--)
            {
                var sq = Squares.Where(x => x.Row == j && x.Column == CheckColumn && x.Selected).Count();
                if (sq <= 0)
                {
                    return false;
                }
                CheckColumn++;
            }
            return true;
        }

        private bool CheckHorizontalWin()
        {
            if (Squares == null) return false;
            for(int i=0; i<Rows; i++)
            {
                var sq = Squares.Where(x => x.Row == i && x.Selected);
                if (sq.Count() == Rows)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckVerticalWin()
        {
            if (Squares == null) return false;
            for (int i = 0; i < Columns; i++)
            {
                var sq = Squares.Where(y => y.Column == i && y.Selected);
                if (sq.Count() == Columns)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsSaneGame()
        {
            //I didn't write this, ye old auto formatter suggested it. 
            // I had and was happy with a plain of switch statement..
            return Words.Count switch
            {
                9 or 16 or 25 => true,
                _ => false,
            };
        }

        private List<BingoSquare> SetupSquares(double sides, string freespacelabel)
        {
            List<BingoSquare> gamesquares = new();
            int i = 0;
            for (int rows = 0; rows < sides; rows++)
            {
                for (int cols = 0; cols < sides; cols++)
                {
                    var square = new BingoSquare {Id = i,  Row = rows, Column = cols, FreeSpace = false, Selected = false, Value = Words[i++] };
                    if(square.Value == "FREE SPACE")
                    {
                        square.Value = freespacelabel;
                        square.FreeSpace = true;
                        square.Selected = true;
                    }
                    gamesquares.Add(square);
                }
            }
            return gamesquares;
        }
    }

}
