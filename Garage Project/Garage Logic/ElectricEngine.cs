namespace Garage_Logic
{
    public class ElectricEngine : Engine
    {
        public ElectricEngine(float i_MaxEnergy) : base(i_MaxEnergy)
        {
        }

        public override string ToString()
        {
            return string.Format("Present Battery Amount: {0}", PresentEnergyAmount);
        }
    }
}
