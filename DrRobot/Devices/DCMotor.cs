using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrRobot.Devices
{
    public class DCMotor
    {
        /// <summary>
        /// номер вывода (М) на плате MotorShield
        /// </summary>
        private int _m;
        private MotorDirection _currentDirection;
        private int speed;

        public int Speed
        {
            get
            {
                return speed;
            }
            set
            {
                if (value >= 0 && value <= 100)
                {
                    SetSpeed(value);
                    speed = value;
                }
            }
        }

        public MotorDirection CurrentDirection { get { return _currentDirection; } }

        public DCMotor(int M)
        {
            this._m = M;
            _currentDirection = MotorDirection.RELEASE;
            ArduinoCommands.DCMotor.Enable();
        }

        public DCMotor(int M, int id)
        {
            this._m = M;
            _currentDirection = MotorDirection.RELEASE;
            ArduinoCommands.DCMotor.Enable();
        }

        public void Forward(int speed)
        {
            ArduinoCommands.DCMotor.Run(this._m, MotorDirection.FORWARD);
            ArduinoCommands.DCMotor.SetSpeed(this._m, speed);
            this.speed = speed;
        }

        public void Backward(int speed)
        {
            ArduinoCommands.DCMotor.Run(this._m, MotorDirection.BACKWARD);
            ArduinoCommands.DCMotor.SetSpeed(this._m, speed);
            this.speed = speed;
        }

        public void Stop()
        {
            ArduinoCommands.DCMotor.Run(this._m, MotorDirection.RELEASE);
            ArduinoCommands.DCMotor.SetSpeed(this._m, 0);
            this.speed = 0;
        }
        private void SetSpeed(int speed)
        {
            if (speed == 0) //Если скорость = 0 полная остановка
                ArduinoCommands.DCMotor.Run(this._m, MotorDirection.RELEASE);
            ArduinoCommands.DCMotor.SetSpeed(this._m, speed);
            this.speed = speed;
        }
    }
}
