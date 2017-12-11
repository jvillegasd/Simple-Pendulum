using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;

namespace Pendulum
{
    public partial class Form : System.Windows.Forms.Form
    {
        Pendulum pendulum;
        Pen penString, penBall;
        Thread engine;
        bool engineOn = false, createEngine = false, start = false, firstTime = true, waitThread = false;
        private int thetaA, l;

        private void button1_Click(object sender, EventArgs e)
        {
            thetaA = thetaATB.Value;
            l = longTB.Value;
            if (firstTime) createEngine = true;
            if (!start)
            {
                pendulum.setL(l);
                pendulum.setThetaA(thetaA);
                start = true;
                button1.Text = "Start";
                waitThread = false;
                panel1.Refresh();
            }
            else
            {
                button1.Text = "Reset";
                start = false;
                waitThread = true;
            }
        }

        public Form()
        {
            InitializeComponent();
            penString = new Pen(Color.Black);
            penBall = new Pen(Color.Blue);
            pendulum = new Pendulum(penString, penBall);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (createEngine)
            {
                engine = new Thread(() => initEngine());
                engine.Start();
                engineOn = true;
                createEngine = firstTime = false;
            }
        }

        void initEngine()
        {
            Graphics g = panel1.CreateGraphics();
            while (engineOn)
            {
                pendulum.paintMovement(panel1.Width, panel1.Height, g);
                Thread.Sleep(60);
                while (waitThread)
                {
                    Thread.SpinWait(1);
                }
            }
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                engine.Abort();
            }
            catch (Exception) {}
        }
    }
}
