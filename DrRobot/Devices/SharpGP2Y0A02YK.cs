using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrRobot.Devices
{
    public class SharpGP2Y0A02YK : IRSensor
    {
        private const double _A = 0.008271;
        private const double _B = 939.6;
        private const double _C = -3.398;
        private const double _D = 17.339;

        public SharpGP2Y0A02YK(int pin)
            : base(pin)
        {

        }
        protected override double ConvertToDistance(int data)
        {
            //data - результат с АЦП от 0 до 1023
            double result = data;
            result = result * 5.0 / 1024.0; // Нормализация

            result = (_A + _B * result) / (1 + _C * result + _D * result * result);
            return result;
        }
    }
}
