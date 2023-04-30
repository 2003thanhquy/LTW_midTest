using System.Drawing;
using System.Drawing.Drawing2D;


namespace System.Windows.Forms
{
    public class RoundButton : Button
    {
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
            this.Region = new Region(graphicsPath);

            using (Brush brush = new SolidBrush(BackColor))
            {
                e.Graphics.FillEllipse(brush, 8, 8, ClientSize.Width - 16, ClientSize.Height - 16);
            }

            base.OnPaint(e);

        }
    }

}