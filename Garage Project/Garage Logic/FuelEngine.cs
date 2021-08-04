namespace Garage_Logic
{

    public class FuelEngine : Engine
    {
        public const int k_MinFuelTypeValue = 1;
        public const int k_MaxFuelTypeValue = 4;

        private readonly eFuelType r_FuelType;

        public FuelEngine(eFuelType i_FuelType, float i_MaxEnergy) : base(i_MaxEnergy)
        {
            r_FuelType = i_FuelType;
        }

        public enum eFuelType
        {
            Soler = 1,
            Octan95,
            Octan96,
            Octan98
        }

        public eFuelType FuelType
        {
            get { return r_FuelType; }
        }

        public void BatteryCharging(float i_BatteryTimeToAdd)
        {
            FillEnergy(i_BatteryTimeToAdd);
        }

        public override string ToString()
        {
            return string.Format("Fuel Type is {0}, present Fuel Amount: {1}", FuelType, PresentEnergyAmount);
        }
    }
}
