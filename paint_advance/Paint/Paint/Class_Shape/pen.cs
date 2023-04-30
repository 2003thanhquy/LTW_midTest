using System.Drawing;

namespace Paint.Class_Shape
{
    class clsPen : clsDrawObject
    {
        public override void Draw(Graphics myGp, bool isFill)
        {
            myGp.DrawLines(myPen, lstPoints.ToArray());
        }
    }
}
