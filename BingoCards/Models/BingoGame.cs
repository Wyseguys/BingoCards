namespace BingoCards.Models
{
    public class BingoGame
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; } = null;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;
        public bool IsWinner { get { return CheckNormalWin(); } }
        public bool IsBlackout { get { return CheckBlackout(); } }
        public bool HasFreeSpace { get; set; } = true;
        public List<BingoSquare>? Squares { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private List<string> Words {  get; set; }

        public BingoGame()
        {
            Words = new List<string>() { "You", "forgot", "to", "setup", "your", "bingo", "game", "my", "friend" };
        }
        public BingoGame(List<string> wordlist, string name = "dude", string freespacelabel = "FREE SPACE")
        {
            try
            {
                Words = wordlist ?? throw new ArgumentNullException(nameof(wordlist));
                if (!IsSaneGame()) throw new Exception("Word list should be 9, 16 or 25 entries");
                double sides = Math.Sqrt(Words.Count);

                Squares = SetupSquares(sides, freespacelabel);
                Rows = Convert.ToInt32(sides);
                Columns = Convert.ToInt32(sides);
                Name = name;
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot create new bingo card: " + ex.Message);
            }

        }

        /// <summary>
        /// Check if every square is marked on the bingo card
        /// </summary>
        /// <returns>bool (true or false)</returns>
        public bool CheckBlackout()
        {
            if(Squares == null) return false;
            if (Squares.Count(x => x.Selected) == Squares.Count)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if there is a full row, column or diagonal win
        /// </summary>
        /// <returns>bool (true or false)</returns>
        public bool CheckNormalWin()
        {
            if (Squares == null) return false;

            return CheckHorizontalWin() || CheckVerticalWin() || CheckTopDownDiagonalWin() || CheckBottomUpDiagonalWin();
        }

        /// <summary>
        /// See if the diagonal, top left towards bottom right win condition is met
        /// </summary>
        /// <returns>bool (true or false)</returns>
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

        /// <summary>
        /// See if the diagonal, bottom left up towards the top right win condition is met
        /// </summary>
        /// <returns>bool (true or false)</returns>
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

        /// <summary>
        /// Check if all the columnns in any of the rows have met the win condition
        /// </summary>
        /// <returns>bool (true or false)</returns>
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

        /// <summary>
        /// Check if all the rows in any of the columns have met the win condition
        /// </summary>
        /// <returns>bool (true or false)</returns>
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

        /// <summary>
        /// Confirm this game is either 9, 16 or 25 squares.  Reject anything else
        /// </summary>
        /// <returns>bool (true or false)</returns>
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
