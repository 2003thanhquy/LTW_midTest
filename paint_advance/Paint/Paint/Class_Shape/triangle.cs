using System;
using System.Drawing;

namespace Paint.Class_Shape
{
    public class clsTriangle : clsDrawObject
    {
        public override void Draw(Graphics myGp, bool isFill)
        {
            int x_min = Math.Min(lstPoints[0].X, lstPoints[1].X), y_min = Math.Min(lstPoints[0].Y, lstPoints[1].Y);
            int x_max = Math.Max(lstPoints[0].X, lstPoints[1].X), y_max = Math.Max(lstPoints[0].Y, lstPoints[1].Y);
            //Rectangle r = new Rectangle(x_min, y_min, x_max - x_min, y_max - y_min);
            //myGp.FillPolygon(myBrush, points);
            if (isFill)
                myGp.FillPolygon(myBrush, new Point[] { new Point(x_min, y_max), new Point((int)((x_min +x_max)/2), y_min),
            new Point(x_max,y_max)});
            else
                myGp.DrawPolygon(myPen, new Point[] { new Point(x_min, y_max), new Point((int)((x_min +x_max)/2), y_min),
            new Point(x_max,y_max)});
        }
    }
}
