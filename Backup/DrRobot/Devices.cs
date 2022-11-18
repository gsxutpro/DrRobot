using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;


namespace DrRobot
{
    public class ArduinoDevice
    {
        protected int _pin;
        public ArduinoDevice(int pin)
        {
            IsConnected = Connect();
            _pin = pin;
        }
        public int Pin { get { return _pin; } }
        public bool IsConnected { get; set; }
        protected virtual bool Connect()
        {
            return true;
        }
    }
    public class IRSensor : ArduinoDevice
    {
        public IRSensor(int pin) : base(pin)
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
            int data = ArduinoCommands.analogRead(_pin);
            //2. Пересчитать в расстояние
            return ConvertToDistance(data);
        }
        
        private double ConvertToDistance(int data)
        {
            
            return data;
        }
    }

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
        protected  bool Connect()
        {
            ArduinoCommands.Servo.Attach(this.id, this.pin);
            return true;
        }
        /// <summary>
        /// Задать угол
        /// </summary>
        /// <param name="angle">угол от 0 до 180</param>
        public void SetAngle(int angle)
        {
            if (angle >= 0 && angle <= 180)
                ArduinoCommands.Servo.Write(this.id, angle);
            this.angle = angle;
        }
    }

    public class DCMotor 
    {
        private int M;
        private MotorDirection currentDirection;
        private int speed;
        public int Speed
        {
            get
            {
                return speed;
            }
            set
            {
                if (value >= 0 && value <= 180)
                    SetSpeed(value);
                speed = value;
            }
        }

        public MotorDirection CurrentDirection { get { return currentDirection; } }

        public DCMotor(int M)
        {
            this.M = M;
            currentDirection = MotorDirection.RELEASE;
        }

        public DCMotor(int M, int id)
        {
            this.M = M;
        }

        public void Forward(int speed)
        {
            ArduinoCommands.DCMotor.Run(this.M, MotorDirection.FORWARD);
            ArduinoCommands.DCMotor.SetSpeed(this.M, speed);
        }

        public void Backward(int speed)
        {
            ArduinoCommands.DCMotor.Run(this.M, MotorDirection.BACKWARD);
            ArduinoCommands.DCMotor.SetSpeed(this.M, speed);
        }

        public void Stop()
        {
            ArduinoCommands.DCMotor.Run(this.M, MotorDirection.RELEASE);
            ArduinoCommands.DCMotor.SetSpeed(this.M, 0);
        }
        private void SetSpeed(int speed)
        {
            if (speed == 0) //Если скорость = 0 полная остановка
                ArduinoCommands.DCMotor.Run(this.M, MotorDirection.RELEASE);
            ArduinoCommands.DCMotor.SetSpeed(this.M, speed);
        }
    }

}