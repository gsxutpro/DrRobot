using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

using DrRobot.Devices;

namespace DrRobot
{
    class Arduino
    {

        ArduinoSerialCommandBroker _broker;
        public Arduino(string SerialPort)
        {
            _broker = new ArduinoSerialCommandBroker(SerialPort);
        }
        public List<ArduinoDevice> Devices { get; set; }

    }
}
