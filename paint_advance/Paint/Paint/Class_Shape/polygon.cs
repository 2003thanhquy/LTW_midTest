using System.Drawing;

namespace Paint.Class_Shape
{
    public class clsPolygon : clsDrawObject
    {

        public override void Draw(Graphics myGp, bool isFill)
        {
            if (isFill) myGp.FillPolygon(myBrush, lstPoints.ToArray());
            else myGp.DrawPolygon(myPen, lstPoints.ToArray());
        }
    }
}
