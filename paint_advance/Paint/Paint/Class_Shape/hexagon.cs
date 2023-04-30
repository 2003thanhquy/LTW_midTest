using System;
using System.Drawing;

namespace Paint.Class_Shape
{
    public class clsHexagon : clsDrawObject
    {
        public override void Draw(Graphics myGp, bool isFill)
        {
            Point center = new Point();
            center.X = (lstPoints[0].X + lstPoints[1].X) / 2;
            center.Y = (lstPoints[0].Y + lstPoints[1].Y) / 2;
            int width = (lstPoints[1].X - lstPoints[0].X) / 2;

            Point[] points = new Point[]
            {
                new Point(center.X,lstPoints[0].Y),
                new Point(lstPoints[1].X,(int)(center.Y - width/Math.Tan(Math.PI/3))),
                new Point(lstPoints[1].X,(int)(center.Y - width/Math.Tan(Math.PI*2/3))),
                new Point(center.X,lstPoints[1].Y),
                new Point(lstPoints[0].X,(int)(center.Y -width/Math.Tan(Math.PI*2/3))),
                new Point(lstPoints[0].X,(int)(center.Y -width/Math.Tan(Math.PI/3)))

            };

            if (isFill) myGp.FillPolygon(myBrush, points);
            else myGp.DrawPolygon(myPen, points);
        }
    }
}
