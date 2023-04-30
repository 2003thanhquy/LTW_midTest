using System.Drawing;

namespace Paint.Class_Shape
{
    public class clsLine : clsDrawObject
    {
        public override void Draw(Graphics myGp, bool isFill)
        {
            myGp.DrawLine(myPen, lstPoints[0], lstPoints[1]);

        }
    }
}
