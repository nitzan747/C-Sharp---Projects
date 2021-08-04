namespace MemoryGame
{
    public class MemoryGameCard<T> 
    {
        private readonly T r_CardValue;
        private bool m_IsOpen;

        public MemoryGameCard(T i_CardValue)
        {
            r_CardValue = i_CardValue;
            m_IsOpen = false;
        }

        public string CardValueToString()
        {
            string cardApperance;

            if(m_IsOpen == true)
            {
                cardApperance = r_CardValue.ToString();
            }
            else
            {
                cardApperance = " ";
            }

            return cardApperance;
        }       
        
        public bool IsOpen
        {
            get { return m_IsOpen; }
            set { m_IsOpen = value; }
        }

        public T CardValue
        {
            get { return r_CardValue; }
        }
    }
}