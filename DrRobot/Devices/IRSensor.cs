using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrRobot.Devices
{
    public class IRSensor : ArduinoDevice
    {
        public IRSensor(int pin)
            : base(pin)
        {

        }

        protected override bool Connect()
        {
            ArduinoCommands.pinMode(_pin, PinMode.OUTPUT);
            return true;
        }

        public double GetDistance()
        {
            //Нужно получить расстояние
            //1. Получить данные с ЦАП
            double r1 = double.MinValue;
            double r2 = double.MaxValue;
            while (Math.Abs(r1 - r2) > 20.0)
            {
                int data1 = ArduinoCommands.analogRead(_pin);
                int data2 = ArduinoCommands.analogRead(_pin);
                //2. Пересчитать в расстояние
                r1 = ConvertToDistance(data1);
                r2 = ConvertToDistance(data2);
            }
            return (r1 + r2) / 2.0;
        }

        protected virtual double ConvertToDistance(int data)
        {

            return data;
        }
    }
}
