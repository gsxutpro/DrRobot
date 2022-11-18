using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrRobot
{
    /// <summary>
    /// Тип команды
    /// </summary>
    public enum CommandType
    {
        digitalRead = 1,
        digitalWrite = 2,
        delay = 3,
        pinMode = 4,
        analogRead = 5,
        servoAttach = 6,
        servoWrite = 7,
        motorSetSpeed = 8,
        motorRun = 9,
        analogWrite = 10,
        motorEnable = 11
    }
    /// <summary>
    /// Тип параметра команды
    /// </summary>
    public enum ParameterType
    {
        None = 0,
        Int32 = 1,
        Double = 2,
        Single = 3,
        Char = 4,
        CharArray = 5
    }
    // Уровни передаются как INT
    public enum VLevel
    {
        HIGH = 1,
        LOW = 0
    }

    /// <summary>
    /// Режим работы выхода
    /// </summary>
    public enum PinMode
    {
        INPUT = 1,
        OUTPUT = 0
    }

    /// <summary>
    /// Направление вращения двигателя
    /// </summary>
    public enum MotorDirection
    {
        FORWARD = 0,
        RELEASE = 1,
        BACKWARD = 2
    }

    public static class ByteArrayConverter
    {
        public static int byteArrayToInt(byte[] b, int start, int length)
        {
            int dt = 0;
            if ((b[start] & 0x80) != 0)
                dt = Int32.MaxValue;
            for (int i = 0; i < length; i++)
                dt = (dt << 8) + (b[start++] & 255);
            return dt;
        }

        public static byte[] intToByteArray(int n, int byteCount)
        {
            byte[] res = new byte[byteCount];
            for (int i = 0; i < byteCount; i++)
                res[byteCount - i - 1] = (byte)((n >> i * 8) & 255);
            return res;
        }
    }
}
