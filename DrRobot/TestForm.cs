using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using DrRobot.Devices;

namespace DrRobot
{
    public partial class TestForm : Form
    {
        Servo servo1;
        DCMotor motor1 = new DCMotor(2);
        Robo robot = new Robo();
        public TestForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            servo1.SetAngle(trackBar1.Value);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            label1.Text = trackBar1.Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (servo1 == null)
                servo1 = new Servo(1, 7);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            motor1.Forward(60);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            motor1.Speed += 10;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //ArduinoCommands.pinMode(13, PinMode.OUTPUT);
            //ArduinoCommands.digitalWrite(13, VLevel.HIGH);
            //System.Threading.Thread.Sleep(5000);
            //ArduinoCommands.digitalWrite(13, VLevel.LOW);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            motor1.Forward(100);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            motor1.Stop();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            motor1.Backward(100);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            int mid0 = 0;
            int mid1 = 0;
            for (int i = 0; i < 10; i++)
            {
                int res0 = ArduinoCommands.analogRead(0);
                int res1 = ArduinoCommands.analogRead(1);
                System.Threading.Thread.Sleep(100);
                mid0 += res0;
                mid1 += res1;
                listBox1.Items.Add(res0);
                listBox2.Items.Add(res1);
            }
            textBox1.Text = (mid0 / 10.0).ToString();
            textBox2.Text = (mid1 / 10.0).ToString();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            SharpGP2Y0A02YK mIR = new SharpGP2Y0A02YK(0);
            SharpGP2Y0A02YK fIR = new SharpGP2Y0A02YK(1);
            List<double> dist = new List<double>();
            DCMotor motorL = new DCMotor(1);
            DCMotor motorR = new DCMotor(2);
            if (servo1 == null) return;
            for (int i = 0; i <= 180; i += 5)
            {
                servo1.SetAngle(i);
                System.Threading.Thread.Sleep(50);
                dist.Add(mIR.GetDistance());
            }
            int ang = 0;
            double min = double.MaxValue;
            for (int i = 0; i < dist.Count; i++)
            {
                if (dist[i] < min)
                {
                    min = dist[i];
                    ang = i * 5;
                }
            }
            servo1.SetAngle(ang);
            if (ang <= 90)
            {
                motorL.Forward(70);
                while (true)
                {
                    motorL.Forward(70);
                    System.Threading.Thread.Sleep(50);
                    //motorL.Stop();
                    double ds = fIR.GetDistance();
                    Debug.WriteLine(ds);
                    if (Math.Abs(ds - (min - 4)) <= 10)
                    {
                        motorL.Stop();
                        break;
                    }
                }
            }
            else
            {
                motorR.Forward(70);
                while (true)
                {
                    motorR.Forward(70);
                    System.Threading.Thread.Sleep(50);
                    //motorL.Stop();
                    double ds = fIR.GetDistance();
                    Debug.WriteLine(ds);
                    if (Math.Abs(ds - (min - 4)) <= 10)
                    {
                        motorR.Stop();
                        break;
                    }
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            
        }

        private void button11_MouseDown(object sender, MouseEventArgs e)
        {
            robot.MoveForward(100);
        }

        private void button11_MouseUp(object sender, MouseEventArgs e)
        {
            robot.Stop();
        }

        private void button12_MouseDown(object sender, MouseEventArgs e)
        {
            robot.MoveBackward(100);
        }

        private void button12_MouseUp(object sender, MouseEventArgs e)
        {
            robot.Stop();
        }

        private void button14_Click(object sender, EventArgs e)
        {

        }

        private void button14_MouseDown(object sender, MouseEventArgs e)
        {
            robot.TurnRight(70);
        }

        private void button14_MouseUp(object sender, MouseEventArgs e)
        {
            robot.Stop();
        }

        private void button13_MouseDown(object sender, MouseEventArgs e)
        {
            robot.TurnLeft(70);
        }

        private void button13_MouseUp(object sender, MouseEventArgs e)
        {
            robot.Stop();
        }

        private void button13_Click(object sender, EventArgs e)
        {

        }
    }
}
