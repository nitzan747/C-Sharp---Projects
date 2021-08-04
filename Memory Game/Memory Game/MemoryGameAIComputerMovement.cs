namespace MemoryGame
{
    using System;
    using System.Collections.Generic;

    public class MemoryGameAIComputerMovement<T>
    {
        private const int k_NotFound = -1;
        private List<CardsData> m_CardsDataList;
        private List<CardsData.Point> m_UnFamiliarCards;
        private bool m_IsTheFirstCardWasChosen = false;

        public enum e_NumberOfAppearance
        {
            WAS_MATCH,
            ONE,
            TWO
        }

        public MemoryGameAIComputerMovement(int i_RowAmount, int i_ColAmount)
        {
            m_CardsDataList = new List<CardsData>();
            m_UnFamiliarCards = new List<CardsData.Point>();
            initializeUnFamiliarCardsList(i_RowAmount, i_ColAmount);
        }

        private void initializeUnFamiliarCardsList(int i_RowAmount, int i_ColAmount)
        {
            for (int rowCounter = 0; rowCounter < i_RowAmount; rowCounter++)
            {
                for (int colCounter = 0; colCounter < i_ColAmount; colCounter++)
                {
                    m_UnFamiliarCards.Add(new CardsData.Point(rowCounter, colCounter));
                }
            }
        }

        public void SortCardsDataList()
        {
            int size = m_CardsDataList.Count;
            CardsData temp;

            for (int i = 1; i < size; i++)
            {
                for (int j = 0; j < (size - i); j++)
                {
                    if (m_CardsDataList[j].Appearance > m_CardsDataList[j + 1].Appearance)
                    {
                        temp = m_CardsDataList[j];
                        m_CardsDataList[j] = m_CardsDataList[j + 1];
                        m_CardsDataList[j + 1] = temp;
                    }
                }
            }
        }

        public void ChooseCardToFlip(out int o_RowNum, out int o_ColNum)
        {
            int lastIndex = m_CardsDataList.Count - 1;

            if (m_CardsDataList[lastIndex].Appearance == e_NumberOfAppearance.TWO)
            {
                chooseCardWithTwoAppereance(out o_RowNum, out o_ColNum);
            }
            else
            {
                    chooseRandomlyUnFamiliarCardFromTheList(out o_RowNum, out o_ColNum);
            }

            m_IsTheFirstCardWasChosen = m_IsTheFirstCardWasChosen ? false : true;
        }

        private void chooseRandomlyUnFamiliarCardFromTheList(out int o_RowNum, out int o_ColNum)
        {
            int listLength = m_UnFamiliarCards.Count;
            Random randIndex = new Random();
            int listItemIndex = randIndex.Next(0, listLength);     
            o_RowNum = m_UnFamiliarCards[listItemIndex].X;
            o_ColNum = m_UnFamiliarCards[listItemIndex].Y;
        }

        private void chooseCardWithTwoAppereance(out int o_RowNum, out int o_ColNum)
        {
            int lastIndex = m_CardsDataList.Count - 1;

            if (m_IsTheFirstCardWasChosen == true)
            {
                o_RowNum = m_CardsDataList[lastIndex].FirstAppearance.X;
                o_ColNum = m_CardsDataList[lastIndex].FirstAppearance.Y;
                m_CardsDataList[lastIndex].Appearance = e_NumberOfAppearance.WAS_MATCH;
            }
            else
            {
                o_RowNum = m_CardsDataList[lastIndex].SecondAppearance.X;
                o_ColNum = m_CardsDataList[lastIndex].SecondAppearance.Y;
            }
        }

        public void InsertCardDataToList(T i_CardValue, int i_RowNum, int i_ColNum)
        {
            bool isThePointUnfamiliar = removePointFromUnFamiliarCardsList(i_RowNum, i_ColNum);
            int cardIndexInTheList;
            CardsData.Point cardPosition;

            if (isThePointUnfamiliar == true)
            {
                cardPosition = new CardsData.Point(i_RowNum, i_ColNum);
                cardIndexInTheList = findCardInTheList(i_CardValue, i_RowNum, i_ColNum); 
                if (cardIndexInTheList == k_NotFound)
                {
                    m_CardsDataList.Add(new CardsData(i_CardValue, i_RowNum, i_ColNum));
                }
                else
                {
                    insertSecondApperance(cardIndexInTheList, i_RowNum, i_ColNum);    
                }
            }
        }

        public void insertSecondApperance(int i_CardIndexIntheList, int i_CardRowNum, int i_CardColNum)
        {
            if(m_CardsDataList[i_CardIndexIntheList].Appearance == e_NumberOfAppearance.ONE)
            {            
                if (checkIfPointIsFirstAppearance(i_CardIndexIntheList, i_CardRowNum, i_CardColNum) != true)
                {                  
                    m_CardsDataList[i_CardIndexIntheList].SecondAppearance = new CardsData.Point(i_CardRowNum, i_CardColNum);
                    m_CardsDataList[i_CardIndexIntheList].Appearance = e_NumberOfAppearance.TWO;                  
                }
            }
        }

        private bool checkIfPointIsFirstAppearance(int i_CardIndexIntheList, int i_PointRowNum, int i_PointColNum)
        {
            bool isItTheFirstAppereance = false;

            if (m_CardsDataList[i_CardIndexIntheList].FirstAppearance.X == i_PointRowNum)
            {
                if (m_CardsDataList[i_CardIndexIntheList].FirstAppearance.Y == i_PointColNum)
                {
                    isItTheFirstAppereance = true;
                }
            }

            return isItTheFirstAppereance;
        }

        public void UpdateAfterCardMatch(T i_CardValue, int i_RowNum, int i_ColNum)
        {
            int cardIndexInTheList = findCardInTheList(i_CardValue, i_RowNum, i_ColNum);
            m_CardsDataList[cardIndexInTheList].Appearance = e_NumberOfAppearance.WAS_MATCH;
        }

        private bool removePointFromUnFamiliarCardsList(int i_RowNum, int i_ColNum)
        {
            bool isFound = false;
            int listLength = m_UnFamiliarCards.Count;
            int indexToRemove = k_NotFound;

            for (int itemIndexCounter = 0; itemIndexCounter < listLength && indexToRemove == k_NotFound; itemIndexCounter++)
            {
                if ((m_UnFamiliarCards[itemIndexCounter].X == i_RowNum) && (m_UnFamiliarCards[itemIndexCounter].Y == i_ColNum))
                {
                    indexToRemove = itemIndexCounter;
                    isFound = true;
                }
            }

            if (isFound == true)
            {
                m_UnFamiliarCards.RemoveAt(indexToRemove);
            }

            return isFound;
        }

        private int findCardInTheList(T i_CardValue, int i_RowNum, int i_ColNum)
        {
            int listLength = m_CardsDataList.Count;
            int itemIndex = k_NotFound;

            for (int itemIndexCounter = 0; itemIndexCounter < listLength; itemIndexCounter++)
            {
                if (m_CardsDataList[itemIndexCounter].CardValue.ToString().CompareTo(i_CardValue.ToString()) == 0)
                {
                    itemIndex = itemIndexCounter;
                }
            }

            return itemIndex;
        }

        public class CardsData
        {
            private T m_CardValue;
            private e_NumberOfAppearance m_Appearance;
            private Point m_FirstAppearance;
            private Point? m_SecondAppearance;

            public CardsData(T i_CardValue, int i_AppearanceRowNum, int i_AppearanceColNum)
            {
                m_CardValue = i_CardValue;
                m_Appearance = e_NumberOfAppearance.ONE;
                m_FirstAppearance = new Point(i_AppearanceRowNum, i_AppearanceColNum);
                m_SecondAppearance = null;
            }

            public static int CompareTo(CardsData FirstCardData, CardsData secondCardData)
            {
                return FirstCardData.Appearance.CompareTo(secondCardData.m_Appearance);
            }

            public T CardValue
            {
                get { return m_CardValue; }
                set { m_CardValue = value; }
            }

            public e_NumberOfAppearance Appearance
            {
                get { return m_Appearance; }
                set { m_Appearance = value; }
            }

            public Point FirstAppearance
            {
                get
                {
                    return m_FirstAppearance;
                }

                set
                {
                    m_FirstAppearance = value;
                }
            }

            public Point SecondAppearance
            {
                get
                {
                    if (m_SecondAppearance.HasValue == true)
                    {
                        return m_SecondAppearance.Value;
                    }
                    else
                    {
                        throw new Exception("Second Apperance not initialized");
                    }
                }

                set
                {
                    m_SecondAppearance = value;
                }
            }

            public struct Point
            {
                private int m_X;
                private int m_Y;

                public Point(int i_X, int i_Y)
                {
                    m_X = i_X;
                    m_Y = i_Y;
                }

                public int X
                {
                    get { return m_X; }
                    set { m_X = value; }
                }

                public int Y
                {
                    get { return m_Y; }
                    set { m_Y = value; }
                }
            }
        }
    }
}
