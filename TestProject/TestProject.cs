using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace TestProject
{
    [TestClass]
    public class TestProject
    {
        [TestMethod]
        public void TestArduinoCommands()
        {
            DrRobot.ArduinoCommands.analogWrite(1, 1);

        }
    }
}
