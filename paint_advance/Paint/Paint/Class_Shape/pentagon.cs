using System;
using System.Collections.Generic;
using System.Drawing;


namespace Paint.Class_Shape
{
    class clsPentagon : clsDrawObject
    {
        public override void Draw(Graphics myGp, bool isFill)
        {
            float x = Math.Min(lstPoints[0].X, lstPoints[1].X);
            float y = Math.Min(lstPoints[0].Y, lstPoints[1].Y);
            float width = Math.Abs(lstPoints[1].X - lstPoints[0].X);
            float height = Math.Abs(lstPoints[1].Y - lstPoints[0].Y);

            List<PointF> pentagonPoints = new List<PointF>();
            for (int i = 0; i < 5; i++)
            {
                float angle = (float)(i * 2 * Math.PI / 5);
                float x1 = x + width / 2 + width / 2 * (float)Math.Sin(angle);
                float y1 = y + height / 2 + height / 2 * (float)Math.Cos(angle);
                pentagonPoints.Add(new PointF(x1, y1));
            }
            if(isFill) {
                myGp.FillPolygon(myBrush, pentagonPoints.ToArray());
            }else myGp.DrawPolygon(myPen, pentagonPoints.ToArray());
        }
    }

}
