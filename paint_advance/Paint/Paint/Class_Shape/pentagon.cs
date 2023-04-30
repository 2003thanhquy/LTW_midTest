using System;
using System.Drawing;


namespace Paint.Class_Shape
{
    class clsPentagon : clsDrawObject
    {
        public override void Draw(Graphics myGp, bool isFill)
        {
            int n = 5;
            int radius = 100; // radius of the circumcircle
            Point center = new Point(250, 150); // center of the pentagon
            double theta = Math.PI + 3 * Math.PI / n; // rotation angle

            // Calculate the coordinates of each vertex of the pentagon
            Point[] lstPoints = new Point[n];
            for (int i = 0; i < n; i++)
            {
                double angle = theta + i * 2 * Math.PI / n;
                int x = (int)(center.X + radius * Math.Cos(angle));
                int y = (int)(center.Y + radius * Math.Sin(angle));
                lstPoints[i] = new Point(x, y);
            }

            // Draw the pentagon
            for (int i = 0; i < n; i++)
            {
                myGp.DrawLine(Pens.Black, lstPoints[i], lstPoints[(i + 1) % n]);
            }
        }
    }

}
