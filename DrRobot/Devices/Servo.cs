using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrRobot.Devices
{
    public class Servo 
    {
        private int id;
        private int angle = 90;
        private int pin;
        public bool IsConnected { get; set; }
        public Servo(int id, int pin)
        {
            this.id = id;
            this.pin = pin;
            IsConnected = Connect();
        }
        public int Angle
        {
            get
            {
                return angle;
            }
            set
            {
                SetAngle(value);
            }
        }
        protected bool Connect()
        {
            Attach(this.id, this.pin);
            return true;
        }

        /// <summary>
        /// Задать угол поворота
        /// </summary>
        /// <param name="angle">угол от 0 до 180</param>
        public void SetAngle(int angle)
        {
            if (angle >= 0 && angle <= 180)
                Write(this.id, angle);
            this.angle = angle;
        }

        private void Attach(int id, int pin)
        {
            string port = Properties.Settings.Default.Default_port;
            ArduinoCommandBuilder builder = new ArduinoCommandBuilder(CommandType.servoAttach, ParameterType.None);
            builder.AddParameter(id);
            builder.AddParameter(pin);
            ArduinoSerialCommandBroker sender = new ArduinoSerialCommandBroker(port);
            sender.SendCommand(builder.GetByteCommand());
        }

        /// <summary>
        /// Установка угла
        /// </summary>
        /// <param name="angle">Угол от 0 до 180</param>
        private void Write(int id, int angle)
        {
            string port = Properties.Settings.Default.Default_port;
            ArduinoCommandBuilder builder = new ArduinoCommandBuilder(CommandType.servoWrite, ParameterType.None);
            builder.AddParameter(id);
            builder.AddParameter(angle);
            ArduinoSerialCommandBroker sender = new ArduinoSerialCommandBroker(port);
            sender.SendCommand(builder.GetByteCommand());
        }
    }
}
