using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;


namespace DrRobot.Devices
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ArduinoDevice
    {
        #region Переменные
        /// <summary>
        /// Используемый номер вывода
        /// </summary>
        protected int _pin;

        #endregion

        #region Конструктор

        public ArduinoDevice(int pin)
        {
            IsConnected = Connect();
            _pin = pin;
        }

        #endregion

        #region Свойства

        public int Pin { get { return _pin; } }
        public bool IsConnected { get; set; }

        #endregion

        #region Функции

        protected virtual bool Connect()
        {
            return true;
        }

        #endregion
    }
}