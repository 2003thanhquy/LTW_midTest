using System.Drawing;

namespace Paint.Class_Shape
{
    public class clsRhombus : clsDrawObject
    {
        public override void Draw(Graphics myGp, bool isFill)
        {
            float x = ((lstPoints[1].X - lstPoints[0].X) / 2);
            float y = ((lstPoints[1].Y - lstPoints[0].Y) / 2);
            PointF[] points = new PointF[]{new PointF(lstPoints[0].X + x, lstPoints[0].Y),
                new PointF(lstPoints[1].X, lstPoints[1].Y -y),
                new PointF(lstPoints[1].X -x, lstPoints[1].Y),
                new PointF(lstPoints[0].X, lstPoints[1].Y -y)
            };
            if (isFill)
            {
                myGp.FillPolygon(myBrush, points);
            }
            else myGp.DrawPolygon(myPen, points);
        }
    }
}
