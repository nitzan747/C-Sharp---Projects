namespace Garage_Logic
{
    using System;
    using System.Collections.Generic;

    public class Car : Vehicle
    {
        public const int k_ColorMinValue = 1;
        public const int k_ColorMaxValue = 4;
        public const int k_DoorsAmountMinValue = 2;
        public const int k_DoorsAmountMaxValue = 5;
        private eColor m_Color;
        private eDoorsAmount m_Doors;

        public Car(string i_LicenseNumber, Engine i_Engine) : base(i_LicenseNumber, eWheelsAmount.Four, 32, i_Engine)
        {
        }

        public enum eColor
        {
            Red = 1,
            White,
            Black,
            Silver
        }

        public enum eDoorsAmount
        {
            Two = 2,
            Three,
            Four,
            Five
        }

        public eColor Color
        {
            get { return m_Color; }
            set
            {
                if ((int)value < k_ColorMinValue || (int)value > k_ColorMaxValue)
                {
                    throw new ValueOutOfRangeException(k_ColorMinValue, k_ColorMaxValue, "Car color");
                }
                else
                {
                    m_Color = value;
                }
            }
        }

        public eDoorsAmount Doors
        {
            get { return m_Doors; }
            set
            {
                if ((int)value < k_DoorsAmountMinValue || (int)value > k_DoorsAmountMaxValue)
                {
                    throw new ValueOutOfRangeException(k_DoorsAmountMinValue, k_DoorsAmountMaxValue, "Car doors amount");
                }
                else
                {
                    m_Doors = value;
                }
            }
        }

        public override List<QuestionForVehicleInfo> GetGeneralVehicleInfo()
        {
            List<QuestionForVehicleInfo> questionsToUser = new List<QuestionForVehicleInfo>();
            string msgToChooseColor = string.Format(
                "Please insert car's color:{0}{1}", 
                Environment.NewLine, 
                Garage.GetStringOfEnumValue(typeof(eColor)));
            string msgToChooseDoorsAmount = string.Format(
                "Please insert car's doors amount:{0}{1}", 
                Environment.NewLine, 
                Garage.GetStringOfEnumValue(typeof(eDoorsAmount)));
            
            questionsToUser.AddRange(base.GetGeneralVehicleInfo());
            questionsToUser.Add(new QuestionForVehicleInfo(QuestionForVehicleInfo.eQuestionType.ValueBetweenRange, 
                msgToChooseColor, k_ColorMinValue, k_ColorMaxValue));
            questionsToUser.Add(new QuestionForVehicleInfo(QuestionForVehicleInfo.eQuestionType.ValueBetweenRange, 
                msgToChooseDoorsAmount, k_DoorsAmountMinValue, k_DoorsAmountMaxValue));

            return questionsToUser;
        }

        public override void SetVehicleInfo(List<string> i_VehicleInfo)
        {
            base.SetVehicleInfo(i_VehicleInfo);
            base.SetVehicleInfo(i_VehicleInfo);
            int colorConvertor, doorsConvertor, index = i_VehicleInfo.Count - 2;

            colorConvertor = int.Parse(i_VehicleInfo[index]);
            doorsConvertor = int.Parse(i_VehicleInfo[index + 1]);
            Color = (eColor)colorConvertor;
            Doors = (eDoorsAmount)doorsConvertor;
        }   

        public override string ToString()
        {
            return string.Format(
                "{0} color : {1}, doors amount is: {2}.", 
                base.ToString(), 
                Color.ToString(), 
                Doors);
        }
    }
}
