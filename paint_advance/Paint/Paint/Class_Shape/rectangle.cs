using System;
using System.Drawing;

namespace Paint.Class_Shape
{
    public class clsRectangle : clsDrawObject
    {
        public override void Draw(Graphics myGp, bool isFill)
        {
            float x_min = Math.Min(lstPoints[0].X, lstPoints[1].X), y_min = Math.Min(lstPoints[0].Y, lstPoints[1].Y);
            float x_max = Math.Max(lstPoints[0].X, lstPoints[1].X), y_max = Math.Max(lstPoints[0].Y, lstPoints[1].Y);
            RectangleF r = new RectangleF(x_min, y_min, x_max - x_min, y_max - y_min);
            Rectangle rect = Rectangle.Round(r);
            if (isFill)
                myGp.FillRectangle(myBrush, r);
            else myGp.DrawRectangle(myPen, rect);

        }
    }
}
