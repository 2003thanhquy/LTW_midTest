using System.Drawing;

namespace Paint.Class_Shape
{
    class clsPen : clsDrawObject
    {
        public override void Draw(Graphics myGp, bool isFill)
        {
            try
            {
                myGp.DrawLines(myPen, lstPoints.ToArray());
            }
            catch { }
            
        }
    }
}
