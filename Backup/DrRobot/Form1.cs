using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace DrRobot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<int, double> polar = new Dictionary<int, double>(64);
            Graphics g = pictureBox1.CreateGraphics();
            Random r = new Random(DateTime.Now.Millisecond);
            // шаг в градусах
            int step = 20;
            double dist = 0;
            
            //Формирование массива
            for (int i = 0; i <= 180; i += 5)
            {
                if (i % step == 0)
                    dist = r.Next(50, 200);

                polar.Add(i, dist);
            }

            DrawInPolar(g, polar);
        }

        private void DrawInPolar(Graphics g, Dictionary<int, double> polar)
        {
            g.Clear(Color.White);

            int cX = pictureBox1.Width / 2;
            int cY = 10;
            // Рисуем
            KeyValuePair<int, double> pr = new KeyValuePair<int, double>(0, 0);
            bool fl = false;
            g.DrawLine(new Pen(Brushes.Blue, 2), new Point(cX - 1, pictureBox1.Height - (cY - 1)), new Point(cX + 1, pictureBox1.Height - (cY + 1)));
            foreach (KeyValuePair<int, double> vp in polar)
            {
                if (!fl) { pr = vp; fl = true; continue; }
                int x, y;

                x = cX + (int)(vp.Value * Math.Cos(vp.Key * Math.PI / 180));
                y = cY + (int)(vp.Value * Math.Sin(vp.Key * Math.PI / 180));
                y = pictureBox1.Height - y;
                Point p1 = new Point(x, y);
                x = cX + (int)(pr.Value * Math.Cos(pr.Key * Math.PI / 180));
                y = cY + (int)(pr.Value * Math.Sin(pr.Key * Math.PI / 180));
                y = pictureBox1.Height - y;
                Point p2 = new Point(x, y);
                g.DrawLine(new Pen(Color.Black), p1, p2);
                g.DrawLine(new Pen(Brushes.Red, 2), new Point(p1.X - 1, p1.Y - 1), new Point(p1.X + 1, p1.Y + 1));
                g.DrawLine(new Pen(Brushes.Red, 2), new Point(p2.X - 1, p2.Y - 1), new Point(p2.X + 1, p2.Y + 1));

                pr = vp;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CommandType type = (CommandType)Enum.Parse(typeof(CommandType), cbCommands.Text);
            double param1 = Double.Parse(tbParam1.Text);
            double param2 = Double.Parse(tbParam2.Text);
            ArduinoCommandBuilder cmd = new ArduinoCommandBuilder(type, ParameterType.None);
            cmd.AddParameter(123.45);
            cmd.AddParameter(456.59);
            byte[] res = cmd.GetByteCommand();
            richTextBox1.AppendText(BytesToString(res));
            ArduinoSerialCommandBroker snd = new ArduinoSerialCommandBroker("COM1");
            snd.SendCommand(res);
        }
        private void GetMove()
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            ArduinoCommandBuilder cb = new ArduinoCommandBuilder(CommandType.delay, ParameterType.None);
            cb.AddParameter(123);
            cb.AddParameter(8904.56f);
            cb.AddParameter(89);
            byte[] b = cb.GetByteCommand();
            
        }

        private void FillCommandBox()
        {
            cbCommands.Items.Clear();
            cbCommands.Items.AddRange(Enum.GetNames(typeof(CommandType)));
            cbCommands.SelectedIndex = 1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FillCommandBox();
        }
        private string BytesToString(byte[] s)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                result.Append(s[i] + ", ");
            }
            return result.ToString();
        }

        private string BytesToChar(byte[] s)
        {
            List<char> res = new List<char>();
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                result.Append((char)s[i]);
                res.Add((char)s[i]);
            }
            return result.ToString();
        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            TestForm test = new TestForm();
            test.Show();
            
        }
    }
}
