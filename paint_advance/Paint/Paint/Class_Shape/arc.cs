using System.Drawing;

namespace Paint.Class_Shape
{
    public class clsArc : clsDrawObject
    {
        public override void Draw(Graphics myGp, bool isFill)
        {
            Rectangle r = new Rectangle(lstPoints[0].X, lstPoints[0].Y, lstPoints[1].X - lstPoints[0].X, lstPoints[1].Y - lstPoints[0].Y);
            myGp.DrawArc(myPen, r, 90, 180);
        }
    }
}
