namespace MemoryGame
{
    using System;
    using System.Text;

    public class ConsoleMemoryGameUI
    {
        private const string k_QuitGame = "Q";
        private MemoryGame<char> m_Game;
        private Random m_RandomNumber = new Random();

        public void StartGame()
        {
            buildGame();
            runGame();
        }

        private void buildGame()
        {
            int boardNumberOfRows, boardNumberOfCols;
            string firstPlayerName, secondPlayerName;
            bool isComputerPlaying;

            firstPlayerName = recivePlayerName(1);
            isComputerPlaying = checkIfTheGameIsAgainstTheComputer();
            reciveBoardRowsAndColsSizeFromUser(out boardNumberOfRows, out boardNumberOfCols);
            if (isComputerPlaying == true)
            {
                m_Game = new MemoryGame<char>(boardNumberOfRows, boardNumberOfCols, firstPlayerName);
            }
            else
            {
                secondPlayerName = recivePlayerName(2);
                m_Game = new MemoryGame<char>(boardNumberOfRows, boardNumberOfCols, firstPlayerName, secondPlayerName);         
            }          

            generateCardsOnBoard();
        }

        private void runGame()
        {
            Player currentPlayer = m_Game.PlayerOne;
            
            printBoard();
            while (m_Game.CheckIfTheGameEnd() != true)
            {
                currentPlayerTurn(ref currentPlayer);
            }

            printBoard();
            endOfTheRound();
        }

        private void endOfTheRound()
        {
            bool isPlayerWantsAnotherRound;
            Player theWinner = m_Game.checkWhoIsTheWinner();

            Console.WriteLine("The winner is : " + theWinner.Name);
            isPlayerWantsAnotherRound = checkIfPlayersWantAnotherRound();
            if (isPlayerWantsAnotherRound == true)
            {
                startAnotherRound();
            }
        }

        private void currentPlayerTurn(ref Player io_CurrentPlayer)
        {
            bool isTheCardMatch;
            int firstCardRow, firstCardCol, secondCardRow, secondCardCol;   
            
            choseCardToFlip(ref io_CurrentPlayer, out firstCardRow, out firstCardCol);
            choseCardToFlip(ref io_CurrentPlayer, out secondCardRow, out secondCardCol);
            m_Game.ComputerPlayerAI.SortCardsDataList();
            isTheCardMatch = m_Game.CheckIfTheCardsMatch(firstCardRow, firstCardCol, secondCardRow, secondCardCol);
            if (isTheCardMatch != true)
            {
                cardsWasNotMatch(ref io_CurrentPlayer, firstCardRow, firstCardCol, secondCardRow, secondCardCol);
            }
            else
            {
                cardsWasMatch(ref io_CurrentPlayer, firstCardRow, firstCardCol);
            }
        }

        private void cardsWasNotMatch(ref Player io_CurrentPlayer, int i_FirstCardRow, int i_FirstCardCol, int i_SecondCardRow, int i_SecondCardCol)
        {
            System.Threading.Thread.Sleep(2000);
            m_Game.FlipCard(i_FirstCardRow, i_FirstCardCol);
            m_Game.FlipCard(i_SecondCardRow, i_SecondCardCol);
            printBoard();
            io_CurrentPlayer = m_Game.ChangePlayer(io_CurrentPlayer);
            Console.WriteLine("There is no match!");
        }

        private void cardsWasMatch(ref Player io_CurrentPlayer, int i_FirstCardRow, int i_FirstCardCol)
        {
            io_CurrentPlayer.Score++;
            m_Game.BoardWrapper.AmountOfClosedCards -= 2;
            Console.WriteLine("It's a match! one point to " + io_CurrentPlayer.Name + ". Take another turn.");
            if (m_Game.IsComputerPlaying == true)
            {
                if(io_CurrentPlayer == m_Game.PlayerTwo)
                {
                    System.Threading.Thread.Sleep(2000);
                }

                m_Game.ComputerPlayerAI.UpdateAfterCardMatch(m_Game.BoardWrapper.Board[i_FirstCardRow, i_FirstCardCol].CardValue, i_FirstCardRow, i_FirstCardCol);            
            }
        }

        private void startAnotherRound()
        {
            int boardNumberOfRows, boardNumberOfCols;

            reciveBoardRowsAndColsSizeFromUser(out boardNumberOfRows, out boardNumberOfCols);      
            m_Game.InitializeGameForRematch(boardNumberOfRows, boardNumberOfCols);
            if(m_Game.IsComputerPlaying == true)
            {
                m_Game.ComputerPlayerAI = new MemoryGameAIComputerMovement<char>(boardNumberOfRows, boardNumberOfCols);
            }

            generateCardsOnBoard();
            runGame();
        }

        private string recivePlayerName(int i_PlayerNumber)
        {
            string playerName;

            Console.WriteLine("please enter the " + i_PlayerNumber + "'s player name:");
            playerName = Console.ReadLine();
            while(playerName.Length == 0)
            {
                Console.WriteLine("Must choose a name ");
                playerName = Console.ReadLine();
            }

            return playerName;
        }

        private bool checkIfTheGameIsAgainstTheComputer()
        {
            string computerOrPlayer;
            bool isComputerPlaying;

            Console.WriteLine("Would you like to play againts the computer? (y/n)");
            computerOrPlayer = Console.ReadLine();
            while (computerOrPlayer != "y" && computerOrPlayer != "n")
            {
                Console.WriteLine("Must answer with the following - y or n");
                computerOrPlayer = Console.ReadLine();
            }

            isComputerPlaying = computerOrPlayer == "y";

            return isComputerPlaying;
        }

        private void reciveBoardRowsAndColsSizeFromUser(out int o_BoardRows, out int o_BoardCols)
        {
            o_BoardRows = reciveBoardSizeFromUser("rows");
            o_BoardCols = reciveBoardSizeFromUser("cols");
            while (checkIfBoardSizeValidate(o_BoardRows, o_BoardCols) != true)
            {
                o_BoardRows = reciveBoardSizeFromUser("rows");
                o_BoardCols = reciveBoardSizeFromUser("cols");
            }
        }

        private int reciveBoardSizeFromUser(string i_RowsOrColsToInsert)
        {
            int SizeNumber;
            string SizeNumberByString;
            char SizeNumberByChar;

            Console.WriteLine("Please enter the board " + i_RowsOrColsToInsert + " number");
            SizeNumberByString = Console.ReadLine();
            SizeNumberByChar = char.Parse(SizeNumberByString);
            while (SizeNumberByString.Length != 1 || char.IsDigit(SizeNumberByChar) != true)
            {
                Console.WriteLine("Please enter a number between 4 to 6 for the " + i_RowsOrColsToInsert + " number");
                SizeNumberByString = Console.ReadLine();
                SizeNumberByChar = char.Parse(SizeNumberByString);
            }

            SizeNumber = int.Parse(SizeNumberByString);
            return SizeNumber;
        }

        private bool checkIfBoardSizeValidate(int i_BoardRows, int i_BoardCols)
        {
            bool isBoardSizeInRange, isBoardSizePair;

            isBoardSizePair = checkBoardSizeIsPair(i_BoardRows, i_BoardCols);
            isBoardSizeInRange = checkBoardSizeInRange(i_BoardRows, i_BoardCols);
            return isBoardSizeInRange && isBoardSizePair;
        }

        private bool checkBoardSizeInRange(int i_BoardRows, int i_BoardCols)
        {
            bool isBoardSizeInRange;

            if (MemoryGame<char>.MemoryGameBoard.CheckBoardSizeInRange(i_BoardRows, i_BoardCols) != true)
            {
                Console.WriteLine("Try again, rows and cols must be between 4 to 6.");
                isBoardSizeInRange = false;
            }
            else
            {
                isBoardSizeInRange = true;
            }

            return isBoardSizeInRange;
        }

        private bool checkBoardSizeIsPair(int i_BoardRows, int i_BoardCols)
        {
            bool isBoardSizePair;

            if (MemoryGame<char>.MemoryGameBoard.CheckBoardSizeIsPair(i_BoardRows, i_BoardCols) != true)
            {
                Console.WriteLine("Try again, must have pair cards numberboard size must be pair.");
                isBoardSizePair = false;
            }
            else
            {
                isBoardSizePair = true;
            }

            return isBoardSizePair;
        }

        private void generateCardsOnBoard()
        {
            int charsCount = m_Game.BoardWrapper.Rows * m_Game.BoardWrapper.Cols;
            char currCharToInsertTheString = 'A';
            StringBuilder charsToInsertTheBoard = new StringBuilder();
            int randCharIndex = m_RandomNumber.Next(charsToInsertTheBoard.Length);

            for (int charIndex = 0; charIndex < charsCount; charIndex += 2)
            {
                charsToInsertTheBoard.Append(currCharToInsertTheString, 2);
                currCharToInsertTheString++;
            }

            for (int rowCounter = 0; rowCounter < m_Game.BoardWrapper.Rows; rowCounter++)
            {
                for (int colCounter = 0; colCounter < m_Game.BoardWrapper.Cols; colCounter++)
                {
                    m_Game.BoardWrapper.InitializeCard(rowCounter, colCounter, charsToInsertTheBoard[randCharIndex]);
                    charsToInsertTheBoard.Remove(randCharIndex, 1);
                    randCharIndex = m_RandomNumber.Next(charsToInsertTheBoard.Length);
                }
            }
        }

        private void printBoard() 
        {
            Ex02.ConsoleUtils.Screen.Clear();
            StringBuilder boardByString = new StringBuilder();
            string lettersLine = createLettersString(m_Game.BoardWrapper.Cols);
            string line = new string('=', lettersLine.Length);

            line += boardByString.Append(Environment.NewLine);
            boardByString.Append(lettersLine);
            boardByString.Append(line);
            boardToStringBuilder(ref boardByString, line);
            Console.WriteLine(boardByString);
        }

        private void boardToStringBuilder(ref StringBuilder io_BoardByString, string i_Line)
        {
            for (int rowCounter = 0; rowCounter < m_Game.BoardWrapper.Rows; rowCounter++)
            {
                for (int colCounter = 0; colCounter < m_Game.BoardWrapper.Cols; colCounter++)
                {
                    if (colCounter == 0)
                    {
                        io_BoardByString.Append((rowCounter + 1) + " | ");
                    }

                    io_BoardByString.Append(m_Game.BoardWrapper.Board[rowCounter, colCounter].CardValueToString() + " | ");
                }

                io_BoardByString.Append(Environment.NewLine);
                io_BoardByString.Append(i_Line);
            }
        }

        private string createLettersString(int i_BoardCols)
        {
            char temp = 'B';
            StringBuilder lineOfletters = new StringBuilder("    A");

            for (int colCounter = 2; colCounter <= i_BoardCols; colCounter++)
            {
                lineOfletters.Append("   " + temp);
                temp++;
            }

            lineOfletters.Append(Environment.NewLine);

            return lineOfletters.ToString();
        }

        private void choseCardToFlip(ref Player i_CurrentPlayer, out int o_Row, out int o_Col)
        {
            Console.WriteLine(i_CurrentPlayer.Name + "'s turn.");
            if(m_Game.IsComputerPlaying == true && i_CurrentPlayer == m_Game.PlayerTwo)
            {
                m_Game.ComputerPlayerAI.ChooseCardToFlip(out o_Row, out o_Col);
            }
            else
            {
                reciveCardPositionFromUser(i_CurrentPlayer, out o_Row, out o_Col);
            }

            m_Game.FlipCard(o_Row, o_Col);
            if (m_Game.IsComputerPlaying == true)
            {
                m_Game.ComputerPlayerAI.InsertCardDataToList(m_Game.BoardWrapper.Board[o_Row, o_Col].CardValue, o_Row, o_Col);
            }

            printBoard();
        }

        private void reciveCardPositionFromUser(Player i_CurrentPlayer, out int o_row, out int o_col)
        {
            bool isCardValid, isPositionInRange, isCardNotAllreadyFlipped, isInputValid;
            string i_InputMsg;

            Console.WriteLine("Please choose a card: ");
            i_InputMsg = Console.ReadLine();
            playersDecisionQuit(i_InputMsg);
            convertColAndRowIndexToint(i_InputMsg[1], i_InputMsg[0], out o_row, out o_col);
            m_Game.CheckIfCardPositonIsValid(i_InputMsg, o_row, o_col, out isPositionInRange, out isCardNotAllreadyFlipped, out isInputValid);
            isCardValid = isInputValid && isPositionInRange && isCardNotAllreadyFlipped;
            while (isCardValid != true)
            {
                errorCardChoice(isPositionInRange, isCardNotAllreadyFlipped, isInputValid);              
                Console.WriteLine("Please choose a card: ");
                i_InputMsg = Console.ReadLine();
                playersDecisionQuit(i_InputMsg);
                convertColAndRowIndexToint(i_InputMsg[1], i_InputMsg[0], out o_row, out o_col);
                m_Game.CheckIfCardPositonIsValid(i_InputMsg, o_row, o_col, out isPositionInRange, out isCardNotAllreadyFlipped, out isInputValid);
                isCardValid = isInputValid && isPositionInRange && isCardNotAllreadyFlipped;
            }
        }

        private void playersDecisionQuit(string i_InputMsg)
        {
            if (i_InputMsg == k_QuitGame)
            {
                Console.WriteLine("You choose to end the game. Bye bye");
                Environment.Exit(-1);
            }
        }

        private void convertColAndRowIndexToint(char i_RowIndex, char i_ColIndex, out int o_CardRow, out int o_CardCol)
        {
            o_CardRow = Convert.ToInt32(i_RowIndex);
            o_CardRow -= Convert.ToInt32('1');
            o_CardCol = Convert.ToInt32(i_ColIndex);
            o_CardCol -= Convert.ToInt32('A');
        }

        private void errorCardChoice(bool i_IsPositionInRange, bool i_IsCardNotAllreadyFlipped, bool i_InputValid)
        {
            if (i_IsPositionInRange != true)
            {
                printCardOutOfBoardLimits(m_Game.BoardWrapper.Rows, m_Game.BoardWrapper.Cols);
            }
            else if (i_IsCardNotAllreadyFlipped != true)
            {
                Console.WriteLine("Try again, You must flip a card which is not allready flipped!");
            }
            else
            {
                Console.WriteLine("Try again, cant be more then 2 digits");
            }
        }

        private void printCardOutOfBoardLimits(int i_MaxRowNumber, int i_MaxColNumber)
        {
            char maxCol = 'A';
            string message;

            maxCol += Convert.ToChar(i_MaxColNumber - 1);
            message = string.Format("Need to choose col between A to {0} and row between 1 to {1}.", maxCol.ToString(), i_MaxRowNumber);
            Console.WriteLine(message);
        }

        private bool checkIfPlayersWantAnotherRound()
        {
            bool isPlayingAnotherRound;
            string anotherRound;

            Console.WriteLine("Would you like to play another round? (y or n)");
            anotherRound = Console.ReadLine();
            while (anotherRound != "n" && anotherRound != "y")
            {
                Console.WriteLine("Must answer with the following - y or n");
                anotherRound = Console.ReadLine();
            }

            if (anotherRound == "n")
            {
                isPlayingAnotherRound = false;
            }
            else
            {
                isPlayingAnotherRound = true;
            }

            return isPlayingAnotherRound;
        }
    }
}
