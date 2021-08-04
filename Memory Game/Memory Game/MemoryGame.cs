namespace MemoryGame
{
    using System;

    public class MemoryGame<T>
    {
        private readonly bool r_IsComputerPlaying;
        private MemoryGameBoard m_BoardWrapper;
        private Player m_Firstplayer;
        private Player m_SecondPlayer;
        private MemoryGameAIComputerMovement<T> m_ComputerPlayerAI;

        public MemoryGame(int i_AmountOfRows, int i_AmountOfCols, string i_FirstPlayerName, string i_SecondPlayerName)
        {
            m_BoardWrapper = new MemoryGameBoard(i_AmountOfRows, i_AmountOfCols);
            m_Firstplayer = new Player(i_FirstPlayerName);
            m_SecondPlayer = new Player(i_SecondPlayerName);
            r_IsComputerPlaying = false;
    }

        public MemoryGame(int i_AmountOfRows, int i_AmountOfCols, string i_FirstPlayerName)
        {
            m_BoardWrapper = new MemoryGameBoard(i_AmountOfRows, i_AmountOfCols);
            m_Firstplayer = new Player(i_FirstPlayerName);
            m_SecondPlayer = new Player("Computer");
            r_IsComputerPlaying = true;
            m_ComputerPlayerAI = new MemoryGameAIComputerMovement<T>(i_AmountOfRows, i_AmountOfCols);
        }

        public Player PlayerOne
        {
            get { return m_Firstplayer; }
        }

        public Player PlayerTwo
        {
            get { return m_SecondPlayer; }
        }

        public bool IsComputerPlaying
        {
            get { return r_IsComputerPlaying; }
        }

        public MemoryGameAIComputerMovement<T> ComputerPlayerAI
        {
            get { return m_ComputerPlayerAI; }
            set { m_ComputerPlayerAI = value; }
        }

        public MemoryGameBoard BoardWrapper
        {
            get { return m_BoardWrapper; }
            set { m_BoardWrapper = value; }
        }

        public Player ChangePlayer(Player io_CurrentPlayer)
        {
            Player nextPlayer;

            if(io_CurrentPlayer == m_Firstplayer)
            {
                nextPlayer = PlayerTwo;
            }
            else
            {
                nextPlayer = PlayerOne;
            }

            return nextPlayer;
        }

        public void FlipCard(int i_row, int i_col)
        {
            BoardWrapper.Board[i_row, i_col].IsOpen = !BoardWrapper.Board[i_row, i_col].IsOpen;
        }

        public bool CheckIfTheCardsMatch(int i_firstCardRow, int i_firstCardCol, int i_secondCardRow, int i_secondCardCol)
        {
            bool isCardsMatch;

            if(BoardWrapper.Board[i_firstCardRow, i_firstCardCol].CardValue.Equals(BoardWrapper.Board[i_secondCardRow, i_secondCardCol].CardValue))
            {
                isCardsMatch = true;
            }
            else
            {
                isCardsMatch = false;
            }

            return isCardsMatch;
        }

        public void CheckIfCardPositonIsValid(string i_InputMsg, int i_row, int i_col, out bool o_IsPositionInRange, 
            out bool o_IsCardNotAllreadyFlipped, out bool o_IsInputValid)
        {
            o_IsInputValid = i_InputMsg.Length <= 2 && i_InputMsg.Length >= 1 ? true : false;
            o_IsPositionInRange = BoardWrapper.CheckCardPositionInRange(i_row, i_col);

            if (o_IsPositionInRange != true)
            {
                o_IsCardNotAllreadyFlipped = false;
            }
            else
            {
                o_IsCardNotAllreadyFlipped = BoardWrapper.CheckIfCardIsNotAllreadyFlipped(i_row, i_col);
            }
        }

        public bool CheckIfTheGameEnd()
        {
            bool isGameOver;

            if(BoardWrapper.AmountOfClosedCards == 0)
            {
                isGameOver = true;
            }
            else
            {
                isGameOver = false;
            }

            return isGameOver;
        }

        public Player checkWhoIsTheWinner()
        {
            Player theWinner;

            if(PlayerOne.Score > PlayerTwo.Score)
            {
                theWinner = PlayerOne;
            }
            else
            {
                theWinner = PlayerTwo;
            }

            return theWinner;
        }

        public void InitializeGameForRematch(int i_RowsNumber, int i_ColsNumber)
        {
            BoardWrapper = new MemoryGameBoard(i_RowsNumber, i_ColsNumber);
            m_Firstplayer.Score = 0;
            m_SecondPlayer.Score = 0;
        }

        public class MemoryGameBoard
        {
            private const int k_MinBoardSize = 4;
            private const int k_MaxBoardSize = 6;
            private MemoryGameCard<T>[,] m_Board;
            private int m_AmountOfRows;
            private int m_AmountOfCols;
            private int m_AmountOfClosedCards;
           
            public MemoryGameBoard(int i_AmountOfRows, int i_AmountOfCols)
            {
                m_AmountOfRows = i_AmountOfRows;
                m_AmountOfCols = i_AmountOfCols;
                m_Board = new MemoryGameCard<T>[i_AmountOfRows, i_AmountOfCols];
                m_AmountOfClosedCards = i_AmountOfRows * i_AmountOfCols;
            }

            public int Rows
            {
                get { return m_AmountOfRows; }
            }

            public int Cols
            {
                get { return m_AmountOfCols; }
            }

            public int AmountOfClosedCards
            {
                get { return m_AmountOfClosedCards; }
                set { m_AmountOfClosedCards = value; }
            }

            public MemoryGameCard<T> this[int row, int col]
            {
                get { return m_Board[row, col]; }
                set { m_Board[row, col] = value; }
            }
            
            public MemoryGameCard<T>[,] Board
            {
                get { return m_Board; }
            }

            public static bool CheckBoardSizeInRange(int i_Rows, int i_Cols)
            {
                bool isSizeValueInRange = true;

                if (i_Rows > k_MaxBoardSize || i_Rows < k_MinBoardSize)
                {
                    isSizeValueInRange = false;
                }
                else if(i_Cols > k_MaxBoardSize || i_Cols < k_MinBoardSize)
                {
                    isSizeValueInRange = false;
                }

                return isSizeValueInRange;
            }

           public static bool CheckBoardSizeIsPair(int i_RowBoard, int i_ColBoard)
            {
                bool isBoardPair;

                if ((i_RowBoard * i_ColBoard) % 2 != 0)
                {
                    isBoardPair = false;
                }
                else
                {
                    isBoardPair = true;
                }

                return isBoardPair;
            }

            public void InitializeCard(int row, int col, T value)
            {
                if (m_Board[row, col] == null)
                {
                    m_Board[row, col] = new MemoryGameCard<T>(value);
                }
                else
                {
                    throw new Exception("The card has allready been initialized!");
                }
            }

            public bool CheckCardPositionInRange(int row, int col)
            {
                int rowTrueIndexOnBoard = row + 1;
                bool isInRange = true;

                if (rowTrueIndexOnBoard < 0 || rowTrueIndexOnBoard > m_AmountOfRows)
                {
                    isInRange = false;
                }
                else if (col < 0 || col > m_AmountOfCols)
                {
                    isInRange = false;
                }

                return isInRange;
            }

            public bool CheckIfCardIsNotAllreadyFlipped(int row, int col)
            {
                bool isNotAllreadyFlipped;

                if (Board[row, col].IsOpen == true)
                {
                    isNotAllreadyFlipped = false;
                }
                else
                {
                    isNotAllreadyFlipped = true;
                }

                return isNotAllreadyFlipped;
            }
        }
    }
}
