namespace Garage_Logic
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class MotorCycle : Vehicle
    {
        private const int k_LicenseTypeMinValue = 1;
        private const int k_LicenseTypeMaxValue = 4;      
        private eLicenseType m_LicenseType;
        private int m_EngineCapacity;

        public MotorCycle(string i_LicenseNumber, Engine i_Engine) :
            base(i_LicenseNumber, eWheelsAmount.Two, 30, i_Engine)
        {
        }

        public enum eLicenseType
        {
            A = 1,
            A1,
            AA,
            B
        }

        public eLicenseType LicenseType
        {
            get { return m_LicenseType; }
            set
            {
                if ((int)value < k_LicenseTypeMinValue || (int)value > k_LicenseTypeMaxValue)
                {
                    throw new ValueOutOfRangeException(k_LicenseTypeMinValue, k_LicenseTypeMaxValue, "License type");
                }
                else
                {
                    m_LicenseType = value;
                }
            }
        }

        public int EngineCapacity
        {
            get { return m_EngineCapacity; }
            set
            {
                if (value > 0)
                {
                    m_EngineCapacity = value;
                }
                else
                {
                    throw new ArgumentException("Error: Engine capacity must be greater than 0");
                }
            }
        }

        public override void SetVehicleInfo(List<string> i_VehicleInfo)
        {
            base.SetVehicleInfo(i_VehicleInfo);
            int licenseConverter, index = i_VehicleInfo.Count - 2;
            licenseConverter = int.Parse(i_VehicleInfo[index].ToString());
            
            LicenseType = (eLicenseType)licenseConverter;
            EngineCapacity = int.Parse(i_VehicleInfo[index + 1].ToString());
        }
        
        public override List<QuestionForVehicleInfo> GetGeneralVehicleInfo()
        {
            List<QuestionForVehicleInfo> questionsToUser = new List<QuestionForVehicleInfo>();
            string msgToChooseLicenceType = string.Format(
                "Please insert motorcycle's license type:{0}{1}", 
                Environment.NewLine, 
                Garage.GetStringOfEnumValue(typeof(eLicenseType)));
            
            questionsToUser.AddRange(base.GetGeneralVehicleInfo());
            questionsToUser.Add(new QuestionForVehicleInfo(QuestionForVehicleInfo.eQuestionType.ValueBetweenRange, 
                msgToChooseLicenceType, k_LicenseTypeMinValue, k_LicenseTypeMaxValue));
            questionsToUser.Add(new QuestionForVehicleInfo(QuestionForVehicleInfo.eQuestionType.StringNotEmpty, 
                "Please insert motorcycle's engine capacity"));

            return questionsToUser;
        }

        public override string ToString()
        {
            return string.Format(
                "{0} license Type: {1}, Engine Capacity: {2}.", 
                base.ToString(), 
                LicenseType.ToString(), 
                EngineCapacity);
        }
    }
}
