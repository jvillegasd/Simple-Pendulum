using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace Pendulum
{
    class Pendulum
    {
        const double G = 9.80665;
        private double l, theta, t, thetaA, w;
        private Pen penString, penBall;
        private SolidBrush brush;
        private object thisLock = new object(); //Lock object for thread use

        public Pendulum(Pen penString, Pen penBall)
        {
            this.penString = penString;
            this.penBall = penBall;
            brush = new SolidBrush(penBall.Color);
        }

        public double T { get => t; set => t = value; }

        public void setThetaA(double angle)
        {
            thetaA = degToRad(angle);
        }

        public void setL(double l)
        {
            this.l = l;
            w = Math.Sqrt(G/l);
        }

        public void paintMovement(int wi, int he, Graphics g)
        {
            lock (thisLock)
            {
                g.Clear(Color.White);
                t+=0.7;
                theta = thetaA * Math.Cos(w * t);
                double x = wi / 2 + (l * Math.Sin(theta));
                double y = he / 4 + (l * Math.Cos(theta));
                g.DrawLine(penString, wi / 2, he / 4, (int)x, (int)y);
                Rectangle circle = new Rectangle((int)x - 5,(int)y,10,10);
                g.DrawEllipse(penBall, circle);
                g.FillEllipse(brush, circle);
            }
        }

        private double degToRad(double angle) { return angle * Math.PI / 180; }  
    }
}
