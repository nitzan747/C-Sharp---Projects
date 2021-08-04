namespace Garage_Logic
{
    using System.Collections.Generic;

    public abstract class Engine
    {
        private readonly float m_MaxEnergy;
        private float m_PresentEnergyAmount;
       
        public Engine(float i_MaxEnergy)
        {
            m_MaxEnergy = i_MaxEnergy;
        }
        
        public virtual List<Vehicle.QuestionForVehicleInfo> GetEngineInfo()
        {
            string msgForEnergy = string.Format("Insert engine's present energy amount (0 to {0})", MaxEnergy);
            List<Vehicle.QuestionForVehicleInfo> questionsToUser = new List<Vehicle.QuestionForVehicleInfo>();
            questionsToUser.Add(new Vehicle.QuestionForVehicleInfo(Vehicle.QuestionForVehicleInfo.eQuestionType.ValueBetweenRange, 
                msgForEnergy, 0, MaxEnergy));

            return questionsToUser;
        }

        public float MaxEnergy
        {
            get { return m_MaxEnergy; }            
        }

        public float PresentEnergyAmount
        {
            get { return m_PresentEnergyAmount; }
            set
            {
                if (value >= 0 && value <= m_MaxEnergy)
                {
                    m_PresentEnergyAmount = value;
                }
                else
                {    
                    throw new ValueOutOfRangeException(0, MaxEnergy, "Energy level");
                } 
            }
        }

        public virtual void FillEnergy(float i_EnergyToAdd)
        {
            PresentEnergyAmount += i_EnergyToAdd;
        }

        public override string ToString()
        {
            return string.Format("Present Energy Amount: {0}", m_PresentEnergyAmount);
        }
    }
}
