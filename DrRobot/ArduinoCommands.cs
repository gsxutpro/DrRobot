using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;
using System.Windows.Forms;

namespace DrRobot
{
    #region ArduinoCommands

    /// <summary>
    /// Функции-примитивы Arduino + классы для работы с различными устройствами
    /// </summary>
    public static class ArduinoCommands
    {
        #region Servo

        /// <summary>
        /// Команды для работы с сервами
        /// </summary>
        public static class Servo
        {
            public static void Attach(int id, int pin)
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
            public static void Write(int id, int angle)
            {
                string port = Properties.Settings.Default.Default_port;
                ArduinoCommandBuilder builder = new ArduinoCommandBuilder(CommandType.servoWrite, ParameterType.None);
                builder.AddParameter(id);
                builder.AddParameter(angle);
                ArduinoSerialCommandBroker sender = new ArduinoSerialCommandBroker(port);
                sender.SendCommand(builder.GetByteCommand());
            }
        }

        #endregion

        #region DCMotor
        /// <summary>
        /// Команды для работы с двигателями постоянного тока
        /// </summary>
        public static class DCMotor
        {
            /// <summary>
            /// Установить скорость двигателя
            /// </summary>
            /// <param name="speed">Скорость двигателя в процентах</param>
            public static void SetSpeed(int M, int speed)
            {
                if (speed < 0 && speed > 100)
                    return;

                int realSpeed = (int)(255.0 * (double)speed / 100.0);
                string port = Properties.Settings.Default.Default_port;
                ArduinoCommandBuilder builder = new ArduinoCommandBuilder(CommandType.motorSetSpeed, ParameterType.None);
                builder.AddParameter(M);
                builder.AddParameter(realSpeed);
                ArduinoSerialCommandBroker sender = new ArduinoSerialCommandBroker(port);
                sender.SendCommand(builder.GetByteCommand());
            }

            public static void Run(int M, MotorDirection direction)
            {
                string port = Properties.Settings.Default.Default_port;
                ArduinoCommandBuilder builder = new ArduinoCommandBuilder(CommandType.motorRun, ParameterType.None);
                builder.AddParameter(M);
                builder.AddParameter((int)direction);
                ArduinoSerialCommandBroker sender = new ArduinoSerialCommandBroker(port);
                sender.SendCommand(builder.GetByteCommand());
            }

            public static void Enable()
            {
                string port = Properties.Settings.Default.Default_port;
                ArduinoCommandBuilder builder = new ArduinoCommandBuilder(CommandType.motorEnable, ParameterType.None);
                ArduinoSerialCommandBroker sender = new ArduinoSerialCommandBroker(port);
                sender.SendCommand(builder.GetByteCommand());
            }
        }

        #endregion

        #region Основные команды Arduino

        public static VLevel digitalRead(int pin)
        {
            string port = Properties.Settings.Default.Default_port;
            ArduinoCommandBuilder builder = new ArduinoCommandBuilder(CommandType.digitalRead, ParameterType.Int32);
            builder.AddParameter(pin);
            ArduinoSerialCommandBroker sender = new ArduinoSerialCommandBroker(port);
            int result = (int)sender.SendCommand(builder.GetByteCommand());
            return (VLevel)result;
        }

        public static void digitalWrite(int pin, VLevel level)
        {
            //Нужно сформировать команду и послать ее
            string port = Properties.Settings.Default.Default_port;
            ArduinoCommandBuilder builder = new ArduinoCommandBuilder(CommandType.digitalWrite, ParameterType.None);
            builder.AddParameter(pin);
            builder.AddParameter((int)level);
            ArduinoSerialCommandBroker sender = new ArduinoSerialCommandBroker(port);
            sender.SendCommand(builder.GetByteCommand());
        }

        public static void pinMode(int pin, PinMode mode)
        {
            string port = Properties.Settings.Default.Default_port;
            ArduinoCommandBuilder builder = new ArduinoCommandBuilder(CommandType.pinMode, ParameterType.None);
            builder.AddParameter(pin);
            builder.AddParameter((int)mode);
            ArduinoSerialCommandBroker sender = new ArduinoSerialCommandBroker(port);
            sender.SendCommand(builder.GetByteCommand());
        }

        public static int analogRead(int pin)
        {
            int result = -1;
            string port = Properties.Settings.Default.Default_port;
            ArduinoCommandBuilder builder = new ArduinoCommandBuilder(CommandType.analogRead, ParameterType.Int32);
            builder.AddParameter(pin);
            ArduinoSerialCommandBroker sender = new ArduinoSerialCommandBroker(port);
            result = (Int32)sender.SendCommand(builder.GetByteCommand());
            return result;
        }

        public static void analogWrite(int pin, int value)
        {
            if (value < 0 || value > 255) return;
            string port = Properties.Settings.Default.Default_port;
            ArduinoCommandBuilder builder = new ArduinoCommandBuilder(CommandType.analogWrite, ParameterType.None);
            builder.AddParameter(pin);
            builder.AddParameter(value);
            ArduinoSerialCommandBroker sender = new ArduinoSerialCommandBroker(port);
            sender.SendCommand(builder.GetByteCommand());
        }

        #endregion

    }

    #endregion

    #region Delegate Data Recieved
    public delegate void DataRecievedDelegate(byte[] data);
    #endregion

    #region AruinoSerialCommandBroker

    /// <summary>
    /// Посредник, отвечает за посылку и получения команд, результатов и отладочной информации через serialPort
    /// </summary>
    public class ArduinoSerialCommandBroker:IDisposable
    {
        #region Переменные
        private static SerialPort _port;
        #endregion

        public event DataRecievedDelegate DataRecieved;

        #region Конструкторы

        public ArduinoSerialCommandBroker(SerialPort port)
        {
            _port = port;
            if (!_port.IsOpen)
                ActivatePort(_port.PortName);
        }
        public ArduinoSerialCommandBroker(string portname)
        {
            if (_port == null || !_port.IsOpen)
                _port = ActivatePort(portname);
        }

        #endregion

        #region Функции

        public object SendCommand(byte[] command)
        {
            if (_port == null) return null;
            object response = null;
            ParameterType resp_type = (ParameterType)command[command.Length - 2];
            _port.Write(command, 0, command.Length);

            if (resp_type != ParameterType.None)
            {
                //Ждем когда контроллер будет готов к отправке
                System.Threading.Thread.Sleep(25); //25 ms
                _port.ReadTimeout = 10000; // 10 секунд по умолчанию

                int res=0;
                while (_port.BytesToRead <= 0)
                {
                    System.Threading.Thread.Sleep(25);
                }
                if (_port.BytesToRead > 0)
                {
                    res = _port.ReadByte();
                    int csum = res;
                    List<byte> result = new List<byte>();
                    result.Add((byte)res);
                    //Читаем весь ответ. Формат ответа.
                    int resp_len = 0;
                    if (res == (int)ParameterType.Int32 || res == (int)ParameterType.Single || res == (int)ParameterType.Char) //int32
                    {
                        resp_len = 4;
                    }
                    else if (res == (int)ParameterType.Double)
                    {
                        resp_len = 8;
                    }
                    for (int i = 0; i < resp_len; i++) //-1 потому что байт передающий длину тоже считается как часть команды
                    {
                        byte b = (byte)_port.ReadByte();
                        csum = csum ^ b;
                        result.Add(b);
                    }
                    int recieved_csum = _port.ReadByte();
                    if (recieved_csum == csum)
                        response = ConvertResponce(result.ToArray());

                        
                }

            }
            return response;
        }

        private object ConvertResponce(byte[] resp)
        {
            object result = null;
            ParameterType type = (ParameterType)resp[0];
            if (type == ParameterType.Int32)
            {
                result = BitConverter.ToInt32(resp, 1);
            }
            if (type == ParameterType.Double)
            {
                result = BitConverter.ToDouble(resp, 1);
            }
            if (type == ParameterType.Single)
            {
                result = BitConverter.ToSingle(resp, 1);
            }
            return result;
        }

        private SerialPort ActivatePort(string portname)
        {
            SerialPort _port = SerialPortCreator.CreatePort(portname);
            _port.DataReceived += new SerialDataReceivedEventHandler(_port_DataReceived);
            return _port;

        }

        public void Dispose()
        {
            if (_port != null)
            {
                _port.Close();
                _port.Dispose();
            }
        }

        #endregion

        #region Обработчики событий

        void _port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //SerialPort port = (SerialPort)sender;
            //string s = port.ReadExisting();
            //List<byte> m = new List<byte>();
            //foreach (char c in s.ToArray())
            //{
            //    m.Add((byte)c);
            //}
            //MessageBox.Show(s);
        }

        #endregion
    }

    #endregion

    #region SerialPortCreator
    /// <summary>
    /// Класс отвечает за создание соединения с Arduino через serialPort
    /// </summary>
    public static class SerialPortCreator
    {
        public static SerialPort CreatePort(string portname)
        {
            SerialPort port = new SerialPort(portname, 38400);
            try
            {
                port.Open();
                if (CheckConnection())
                    return port;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }
            return null;

        }
        private static bool CheckConnection()
        {
            // Реализовать процедуру проверки соединения со стороны Arduino
            //Handshake
            return true;
        }
    }

    #endregion

    #region ArduinoCommandBuilder

    /// <summary>
    /// Формирует команду в виде последовательности байт
    /// </summary>
    public class ArduinoCommandBuilder
    {
        CommandType _cmdType;
        private List<object> _params;
        private ParameterType _result;

        /// <summary>
        /// Параметры комманд
        /// </summary>
        public List<object> Params { get { return _params; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">Тип команды</param>
        /// <param name="result">Тип результата</param>
        public ArduinoCommandBuilder(CommandType type, ParameterType result)
        {
            _cmdType = type;
            _params = new List<object>();
            _result = result;
        }
        /// <summary>
        /// Возвращает команду в виде последовательности байт
        /// </summary>
        /// <returns></returns>
        public byte[] GetByteCommand()
        {
            return GenerateCommand();
        }
        /// <summary>
        /// Добавить новый параметр команды
        /// </summary>
        /// <param name="o"></param>
        public void AddParameter(object o)
        {
            _params.Add(o);
        }

        /// <summary>
        /// Основная функция, создающая команду в виде последовательности байт
        /// </summary>
        private byte[] GenerateCommand()
        {
            // Формат команды
            
            List<byte> result = new List<byte>();

            result.Add((byte)_cmdType); // 2й байт - тип команды

            //Добавляем параметры
            result.Add((byte)_params.Count); //3й байт - число параметров
            foreach (object p in _params)
            {
                result.Add((byte)GetParameterType(p));
                result.AddRange(ConvertToBytes(p));
            }
            //Добавили параметры
            //Возвращаемое значение
            result.Add((byte)_result);
            result.Insert(0, (byte)(result.Count + 2));
            byte xor = result[0];
            for (int i = 1; i < result.Count; i++)
                xor = (byte)(xor ^ result[i]);
            result.Add(xor);
            return result.ToArray();
        }

        private ParameterType GetParameterType(object o)
        {
            switch (o.GetType().Name)
            {
                case "Int32":
                    return ParameterType.Int32;
                case "Double":
                    return ParameterType.Double;
                case "Single":
                    return ParameterType.Single;
                case "Char":
                    return ParameterType.Char;
                default:
                    return 0;
            }
        }

        public static byte[] ConvertToBytes(object o)
        {
            byte[] result = null;
            if (o.GetType().Name == "Int32") result = BitConverter.GetBytes((Int32)o);
            if (o.GetType().Name == "Double") result = BitConverter.GetBytes((Double)o);
            if (o.GetType().Name == "Single") result = BitConverter.GetBytes((Single)o);
            if (o.GetType().Name == "Char") result = BitConverter.GetBytes((Char)o);
            return result;
        }
    }
    #endregion
}
