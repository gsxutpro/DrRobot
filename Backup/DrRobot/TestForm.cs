using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DrRobot
{
    public partial class TestForm : Form
    {
        Servo servo1;
        DCMotor motor1 = new DCMotor(1);
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
                servo1 = new Servo(1, 0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            motor1.Forward(5);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            motor1.Speed += 10;
        }
    }
}
