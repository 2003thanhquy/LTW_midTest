using System.Drawing;

namespace Paint.Class_Shape
{
    public class clsRhombus : clsDrawObject
    {
        public override void Draw(Graphics myGp, bool isFill)
        {
            int x = (int)((lstPoints[1].X - lstPoints[0].X) / 2);
            int y = (int)((lstPoints[1].Y - lstPoints[0].Y) / 2);
            Point[] points = new Point[]{new Point(lstPoints[0].X + x, lstPoints[0].Y),
                new Point(lstPoints[1].X, lstPoints[1].Y -y),
                new Point(lstPoints[1].X -x, lstPoints[1].Y),
                new Point(lstPoints[0].X, lstPoints[1].Y -y)
            };
            if (isFill)
            {
                myGp.FillPolygon(myBrush, points);
            }
            else myGp.DrawPolygon(myPen, points);
        }
    }
}
