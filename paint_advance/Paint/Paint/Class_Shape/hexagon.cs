using System;
using System.Drawing;

namespace Paint.Class_Shape
{
    public class clsHexagon : clsDrawObject
    {
        public override void Draw(Graphics myGp, bool isFill)
        {
            PointF center = new PointF();
            center.X = (lstPoints[0].X + lstPoints[1].X) / 2;
            center.Y = (lstPoints[0].Y + lstPoints[1].Y) / 2;
            float width = (lstPoints[1].X - lstPoints[0].X) / 2;

            PointF[] points = new PointF[]
            {
                new PointF(center.X,lstPoints[0].Y),
                new PointF(lstPoints[1].X,(float)(center.Y - width/Math.Tan(Math.PI/3))),
                new PointF(lstPoints[1].X,(float)(center.Y - width/Math.Tan(Math.PI*2/3))),
                new PointF(center.X,lstPoints[1].Y),
                new PointF(lstPoints[0].X,(float)(center.Y -width/Math.Tan(Math.PI*2/3))),
                new PointF(lstPoints[0].X,(float)(center.Y -width/Math.Tan(Math.PI/3)))

            };

            if (isFill) myGp.FillPolygon(myBrush, points);
            else myGp.DrawPolygon(myPen, points);
        }
    }
}
