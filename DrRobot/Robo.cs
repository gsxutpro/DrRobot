using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrRobot.Devices;

namespace DrRobot
{
    /// <summary>
    /// Класс представляющий робота
    /// </summary>
    class Robo
    {
        //Набор элементов
        DCMotor motorR;
        DCMotor motorL;
        SharpGP2Y0A02YK mRanger;
        SharpGP2Y0A02YK fRanger;
        Servo servo;
        public Robo()
        {
            //Инициализация элементов
            motorL = new DCMotor(2);
            motorR = new DCMotor(1);
            mRanger = new SharpGP2Y0A02YK(0);
            fRanger = new SharpGP2Y0A02YK(1);
            servo = new Servo(1, 1);
        }
        public void MoveForward(int speed)
        {
            motorR.Forward(speed);
            motorL.Forward(speed);
        }
        public void Stop()
        {
            motorR.Stop();
            motorL.Stop();
        }
        public void MoveBackward(int speed)
        {
            motorR.Backward(speed);
            motorL.Backward(speed);
        }
        public void TurnLeft(int speed)
        {
            motorL.Forward(speed);
            motorR.Backward(speed);
        }
        public void TurnRight(int speed)
        {
            motorR.Forward(speed);
            motorL.Backward(speed);
        }
        /// <summary>
        /// Поворот на заданный угол
        /// </summary>
        /// <param name="degree"></param>
        public void SmartTurn(int degree)
        {
            //Принцип работы: 
        }

        public void DoWork()
        {
            
        }

        /// <summary>
        /// Функция возвращает угол, определяющий целевой вектор движения
        /// </summary>
        /// <returns>Угол</returns>
        private int GetAngleToMove()
        {
            //Смотрим
            double[] vis = Observe(5);

            for (int i = 0; i < vis.Length; i++)
            {
                //Ищем области
                
            }
            return 0;
        }

        private double[] Observe(int step)
        {
            List<double> result = new List<double>(180 / step);
            for (int i = 0; i <= 180; i += step)
            {
                result.Add(mRanger.GetDistance());
            }
            return result.ToArray();
        }
        
    }
}
