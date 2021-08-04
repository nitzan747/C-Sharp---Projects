namespace Garage_Logic
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Garage
    {
        private const int k_VehicleStatusMinValue = 1;
        private const int k_VehicleStatusMaxValue = 3;
        private Dictionary<string, GarageClient> m_Clients;

        public Garage()
        {
            Clients = new Dictionary<string, GarageClient>();
        }

        public enum eVehicleStatus
        {
            InRepair = 1,
            Fixed,
            PaidUp
        }

        public Dictionary<string, GarageClient> Clients
        {
            get { return m_Clients; }
            set { m_Clients = value; }
        }

        public int VehicleStatusMinValue
        {
            get { return k_VehicleStatusMinValue; }
        }

        public int VehicleStatusMaxValue
        {
            get { return k_VehicleStatusMaxValue; }
        }

        public static string GetStringOfEnumValue(Type i_TypeOfEnum)
        {
            StringBuilder listOfEnumValues = new StringBuilder();

            foreach (object valueOfEnum in Enum.GetValues(i_TypeOfEnum))
            {
                listOfEnumValues.Append(string.Format(
                    "{0}. {1}{2}", 
                    (int)valueOfEnum, 
                    valueOfEnum, 
                    Environment.NewLine));
            }

            return listOfEnumValues.ToString();
        }

        public bool CheckIfLicenseNumberExist(string i_licenseNumber)
        {
            return Clients.ContainsKey(i_licenseNumber);
        }

        public void InsertNewVehicle(string i_ClientName, string i_ClientPhone, string i_LicenseNumber, 
            Vehicle i_NewVehicleToInsert)
        {
            GarageClient newClient = new GarageClient(i_ClientName, i_ClientPhone, i_NewVehicleToInsert);
            Clients.Add(i_LicenseNumber, newClient);
        }

        public void ChangeVehicleStatus(string i_LicenseNumber, eVehicleStatus i_VehicleStatus)
        {
            Clients[i_LicenseNumber].VehicleStatus = i_VehicleStatus;
        }

        public List<string> GetVehiclesLicenseListByStatus(int i_VehicleStatus)
        {
            List<string> licenseList;
            eVehicleStatus status;

            if (i_VehicleStatus == 0)
            {
                licenseList = CreateVehiclsLicenseList();
            }
            else
            {
                status = (eVehicleStatus)i_VehicleStatus;
                licenseList = CreateVehiclsLicenseListByStatus(status);
            }

            return licenseList;
        }

        public List<string> CreateVehiclsLicenseListByStatus(eVehicleStatus i_VehicleStatus)
        {
            List<string> licenseList = new List<string>();

            foreach(var item in Clients)
            {
                if (item.Value.VehicleStatus == i_VehicleStatus)
                {
                    licenseList.Add(item.Key);
                }             
            }

            return licenseList;            
        }

        public List<string> CreateVehiclsLicenseList()
        {
            List<string> licenseList = new List<string>();

            foreach(string key in Clients.Keys)
            {
                licenseList.Add(key);
            }

            return licenseList;
        }

        public void FillVehicleToMaxAirPressure(string i_LicenseNumber)
        {
          if(Clients.ContainsKey(i_LicenseNumber) == true)
            {
                Clients[i_LicenseNumber].vehicle.FillAirToWheels();
            }       
        }

        public string FuelRegularVehicle(string i_LicenseNumber, int i_FuelType, int i_ValueToFill)
        {
            string errorMsg = null;
            bool isItTheCorrectFuelType, isTheAmountLowerThenMaxValue;
            FuelEngine engineToCheck = Clients[i_LicenseNumber].vehicle.VehicleEngine as FuelEngine;
                    
            if (engineToCheck != null)
            {
                isItTheCorrectFuelType = checkIfTheFuelTypeIsSame(engineToCheck, i_FuelType);
                isTheAmountLowerThenMaxValue = checkIfTheFuelAmountLowerThenMaxValue(engineToCheck, i_ValueToFill);
                if (isItTheCorrectFuelType != true || isTheAmountLowerThenMaxValue != true)
                {
                    errorMsg = "Can't fill the tank. Not a valid input.";
                }
                else
                {
                    Clients[i_LicenseNumber].vehicle.UpdateEnergyLevel(i_ValueToFill);
                }
            }
            else
            {
                errorMsg = "The vehicle don't have fuel engine.";
            }

            return errorMsg;
        }

        private bool checkIfTheFuelAmountLowerThenMaxValue(FuelEngine i_VehicleFuelEngin, int i_ValueToFill)
        {
            bool isAmountValid;

            if(i_VehicleFuelEngin.PresentEnergyAmount + i_ValueToFill <= i_VehicleFuelEngin.MaxEnergy)
            {
                isAmountValid = true;
            }
            else
            {
                isAmountValid = false;
            }

            return isAmountValid;
        }

        private bool checkIfTheFuelTypeIsSame(FuelEngine i_VehicleFuelEngine, int i_FuelType)
        {
            bool isFuelTypeSame;

            if(i_VehicleFuelEngine.FuelType == (FuelEngine.eFuelType)i_FuelType)
            {
                isFuelTypeSame = true;
            }
            else
            {
                isFuelTypeSame = false;
            }

            return isFuelTypeSame;
        }

        private bool checkIfTheBatteryAmountLowerThenMaxValue(ElectricEngine i_VehicleElectricEngin, 
            float i_ValueToCharge)
        {
            bool isAmountValid;

            if (i_VehicleElectricEngin.PresentEnergyAmount + i_ValueToCharge <= i_VehicleElectricEngin.MaxEnergy)
            {
                isAmountValid = true;
            }
            else
            {
                isAmountValid = false;
            }

            return isAmountValid;
        }

        public string ChargeElectricVehicle(string i_LicenseNumber, float i_MinutsToCharge)
        {
            string errorMsg = null;
            bool isAmountValid;
            ElectricEngine engineToCheck = Clients[i_LicenseNumber].vehicle.VehicleEngine as ElectricEngine;           

            if (Clients[i_LicenseNumber].vehicle.VehicleEngine is ElectricEngine)
            {
                isAmountValid = checkIfTheBatteryAmountLowerThenMaxValue(engineToCheck, i_MinutsToCharge);
                if (isAmountValid == true)
                {
                    Clients[i_LicenseNumber].vehicle.UpdateEnergyLevel(i_MinutsToCharge);
                }
                else
                {
                    errorMsg = "The value out of range.";
                }
            }
            else
            {
                errorMsg = "The vehicle don't have electric engine.";
            }

            return errorMsg;
        }

        public string GetStringOfVehicleDetails(string i_LicenseNumber)
        {
            return Clients[i_LicenseNumber].ToString();
        }

        public class GarageClient
        {
            private string m_ClientName;
            private string m_ClientPhoneNumber;
            private eVehicleStatus m_VehicleStatus;
            private Vehicle m_ClientVehicle;

            public GarageClient(string i_ClientName, string i_ClientPhoneNumber, Vehicle i_ClientVehicle)
            {
                ClientName = i_ClientName;
                ClientPhoneNumber = i_ClientPhoneNumber;
                ClientVehicle = i_ClientVehicle;
                VehicleStatus = eVehicleStatus.InRepair;
            }

            public string ClientName
            {
                get { return m_ClientName; }
                set { m_ClientName = value; }
            }

            public string ClientPhoneNumber
            {
                get { return m_ClientPhoneNumber; }
                set { m_ClientPhoneNumber = value; }
            }

            public Vehicle vehicle
            {
                get { return m_ClientVehicle; }
                set { m_ClientVehicle = value; }
            }

            public eVehicleStatus VehicleStatus
            {
                get { return m_VehicleStatus; }
                set { m_VehicleStatus = value; }
            }

            public Vehicle ClientVehicle
            {
                get { return m_ClientVehicle; }
                set { m_ClientVehicle = value; }
            }

            public override string ToString()
            {
                return string.Format(
@"Client name is {0}, Phone: {1}, Vehicle status: {2}.
Vehicle information: {3}{4}", 
ClientName,
ClientPhoneNumber,
VehicleStatus.ToString(),
Environment.NewLine,
ClientVehicle.ToString());
            }
        }
    }
}
