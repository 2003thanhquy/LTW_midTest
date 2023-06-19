using System;
using System.Drawing;

namespace Paint.Class_Shape
{
    public class clsTriangle : clsDrawObject
    {
        public override void Draw(Graphics myGp, bool isFill)
        {
            float x_min = Math.Min(lstPoints[0].X, lstPoints[1].X), y_min = Math.Min(lstPoints[0].Y, lstPoints[1].Y);
            float x_max = Math.Max(lstPoints[0].X, lstPoints[1].X), y_max = Math.Max(lstPoints[0].Y, lstPoints[1].Y);
            if (isFill)
                myGp.FillPolygon(myBrush, new PointF[] { new PointF(x_min, y_max), new PointF((int)((x_min +x_max)/2), y_min),
            new PointF(x_max,y_max)});
            else
                myGp.DrawPolygon(myPen, new PointF[] { new PointF(x_min, y_max), new PointF((int)((x_min +x_max)/2), y_min),
            new PointF(x_max,y_max)});
        }
    }
}
