using Paint.Class_Shape;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Paint
{
    public partial class Form1 : Form
    {

        bool isFill = false;// lap day doi tuong
        bool isPress = false;// nhan khi ve hinh
        bool isSelected = false;//danh sach chon
        bool isObjSelected = false;//doi tuong duoc chon
        bool isObjMove = false; // duy chuyen doi tuong
        bool isPolygon = false;
        bool isPen = false;
        PointF pointStart;
        //float zoom = 1f;

        DashStyle dashStyle = DashStyle.Solid;
        Brush brushStyle = new SolidBrush(Color.Black);
        List<clsDrawObject> lstSelected = new List<clsDrawObject>(); // damh sach duoc chon


        int indSelected;
        clsDrawObject objectCurr = null;
        clsDrawObject objSelected = null;
        //brush 
        List<clsDrawObject> lstObject = new List<clsDrawObject>();
        public Form1()
        {
            InitializeComponent();
            this.cMSBrush.ItemClicked += (sender, e) =>
            {
                int index = -1;
                cMSBrush_ItemClicked(sender, e, index);
            };
            this.pnlMain.SetDoubleBuffered();
        }
        public clsDrawObject Init_Object(clsDrawObject drawOject)
        {
            if (isSelected)
            {
                drawOject.myPen = new Pen(Color.Black, 3);
                drawOject.myPen.DashStyle = DashStyle.Dot;
                return drawOject;
            }
            drawOject.myPen = new Pen(ptbColor.BackColor, Int32.Parse(txtWidth.Text));

            drawOject.myPen.DashStyle = dashStyle;
            drawOject.myColor = ptbColor.BackColor;
            if (brushStyle is SolidBrush) cMSBrush_ItemClicked(null, null, 0);
            if ( brushStyle is HatchBrush ) cMSBrush_ItemClicked(null, null, 1);
            if (brushStyle is LinearGradientBrush) cMSBrush_ItemClicked(null, null, 2);
            drawOject.myBrush = brushStyle; 
            drawOject.isFill = isFill;
            return drawOject;
        }
        private void resetVar()
        {
            // isFill = false;
            indSelected = -1;
            isPress = false;
            isSelected = false;//bat che do chon hinh
            isObjSelected = false; //chon duoc doi tuong co the resize
            isSelected = false;// bien lua chon hinh
            lstSelected.Clear();
            isObjMove = false;//
            objectCurr = null; // doi tuong create tao  hien tai
            objSelected = null;// doi tuong duoc chon 
            isPen = false; //but ve
            isPolygon = false;
           // zoom = 1f; // dat lai trang thai ban dau
            lstSelected.Clear();
        }
        private void btnPenDStyle_Click(object sender, EventArgs e)
        {
            cMSDStyle.Show(btnPenDStyle, new Point(0, btnPenDStyle.Height));
        }

        private void moveCoordAllOfOgroup(clsDrawObject clsObj, float x, float y)
        {
            for (int i = 0; i < clsObj.lstPoints.Count; i++)
            {
                PointF p = clsObj.lstPoints[i];
                clsObj.lstPoints[i] = new PointF(p.X + x, p.Y + y);
            }
            clsObj.getGroup().ForEach(obj =>
            {
                moveCoordAllOfOgroup(obj, x, y);
            });
        }
        private clsDrawObject moveObject(clsDrawObject clsObj, PointF start, PointF end)
        {
            this.pointStart = end;
            float x = end.X - start.X;
            float y = end.Y - start.Y;
            moveCoordAllOfOgroup(clsObj, x, y);
            clsObj.calculateResizePoints();
            return clsObj;
        }
        private void resizeObject(clsDrawObject clsObj, int ind, float x, float y)
        {
            switch (ind)
            {
                case 0:
                    clsObj.lstPoints[0] = new PointF(clsObj.lstPoints[0].X + x, clsObj.lstPoints[0].Y + y);
                    break;
                case 4:
                    clsObj.lstPoints[1] = new PointF(clsObj.lstPoints[1].X + x, clsObj.lstPoints[1].Y + y);
                    break;
                case 2:
                    clsObj.lstPoints[0] = new PointF(clsObj.lstPoints[0].X, clsObj.lstPoints[0].Y + y);
                    clsObj.lstPoints[1] = new PointF(clsObj.lstPoints[1].X + x, clsObj.lstPoints[1].Y);
                    break;
                case 6:
                    clsObj.lstPoints[0] = new PointF(clsObj.lstPoints[0].X + x, clsObj.lstPoints[0].Y);
                    clsObj.lstPoints[1] = new PointF(clsObj.lstPoints[1].X, clsObj.lstPoints[1].Y + y);
                    break;
                case 1:
                    clsObj.lstPoints[0] = new PointF(clsObj.lstPoints[0].X, clsObj.lstPoints[0].Y + y);
                    break;
                case 5:
                    clsObj.lstPoints[1] = new PointF(clsObj.lstPoints[1].X, clsObj.lstPoints[1].Y + y);
                    break;
                case 7:
                    clsObj.lstPoints[0] = new PointF(clsObj.lstPoints[0].X + x, clsObj.lstPoints[0].Y);
                    break;
                case 3:
                    clsObj.lstPoints[1] = new PointF(clsObj.lstPoints[1].X + x, clsObj.lstPoints[1].Y);
                    break;
                default: break;
            }
            clsObj.calculateResizePoints();
            clsObj.getGroup().ForEach(obj =>
            {
                resizeObject(obj, ind, x, y);
            });

        }
        private void resizeObjectAllOfGroup(clsDrawObject clsObj, int ind, PointF start, PointF end)
        {
            this.pointStart = end;
            float x = end.X - start.X;
            float y = end.Y - start.Y;
            resizeObject(clsObj, ind, x, y);

        }
        public void searchLocationResize(PointF e)
        {
            this.lstSelected.ForEach(x =>
            {
                for (int i = 0; i < 8; i++)
                {
                    RectangleF r = new RectangleF(x.recResize[i].X, x.recResize[i].Y, 8, 8);
                    if (r.Contains(e))
                    {
                        this.pointStart = e;
                        this.indSelected = i;
                        this.objSelected = x;
                        return;
                    }
                }
            });

        }

        private clsDrawObject checkCursorCurr(Point p)
        {
            return lstObject.LastOrDefault(obj =>
            {
                var (start, end) = obj.getStartAndEndPoints();
                return p.X >= start.X && p.Y >= start.Y && p.X <= end.X && p.Y <= end.Y;
            });
        }
        private void pnlMain_MouseDown(object sender, MouseEventArgs e)
        {

            if (isPress)
            {
                objectCurr.lstPoints.Add(e.Location);
                if (e.Button == MouseButtons.Right)
                {
                    objectCurr = null;
                    isPress = false;
                }
                return;
            }

            if (objectCurr != null)
            {
                this.isPress = true;
                objectCurr = Init_Object(objectCurr);
                objectCurr.lstPoints.Add(e.Location);
                this.lstObject.Add(objectCurr);
                return;
            }


            //chon hinh vuong de di chuyen
            searchLocationResize(e.Location);
            if (this.objSelected == null && indSelected == -1)
            {
                this.objSelected = checkCursorCurr(e.Location);
                if (this.objSelected != null)
                {
                    isObjMove = true;
                    pointStart = e.Location;
                    return;

                }
                resetVar();
                pnlMain.Refresh();
            }
           
        }
        private void pnlMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPen && isPress)
            {
                this.lstObject[this.lstObject.Count - 1].lstPoints.Add(e.Location);

                pnlMain.Refresh();
                return;
            }
            if (this.isPress)
            {
                var obj = this.lstObject[this.lstObject.Count - 1].lstPoints;
                if (obj.Count < 2)
                    obj.Add(e.Location);
                else obj[obj.Count - 1] = e.Location;
                this.pnlMain.Refresh();
                return;
            }
            //bat con tro chuot  sizeAll
            if (checkCursorCurr(e.Location) != null)
            {
                this.Cursor = Cursors.SizeAll;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
            // thay doi size
            if (indSelected >= 0 && this.objSelected != null)
            {
                resizeObjectAllOfGroup(this.objSelected, indSelected, pointStart, e.Location);
                this.pnlMain.Refresh();
            }
            else
            {
                if (isObjMove)
                {
                    moveObject(this.objSelected, pointStart, e.Location);
                    this.pnlMain.Refresh();
                }

            }
        }
        private void pnlMain_MouseUp(object sender, MouseEventArgs e)
        {
            //reset cac gi tri
            this.objSelected = null;
            isObjMove = false;
            this.objSelected = null;
            indSelected = -1;
            if (isPolygon) return;
            if (isPen)
            {
                this.lstObject[this.lstObject.Count - 1].lstPoints.Add(e.Location);
                isPen = false;
                objectCurr = null;
                isPress = false;
                return;
            }
            
            if (this.isPress)
            {
                this.isPress = false;
                this.lstObject[this.lstObject.Count - 1].lstPoints[1] = e.Location;
                this.lstObject[this.lstObject.Count - 1].updatePoints();
                objectCurr = null;
                this.pnlMain.Refresh();
            }
            if (isSelected)
            {
                PointF p1 = this.lstObject[this.lstObject.Count - 1].lstPoints[0];
                PointF p2 = this.lstObject[this.lstObject.Count - 1].lstPoints[1];

                this.lstObject.RemoveAt(this.lstObject.Count - 1);
                isSelected = false;
                dashStyle = DashStyle.Solid;
                isObjSelected = true;
                this.pnlMain.Refresh();

                RectangleF r = new RectangleF(p1.X,p1.Y,p2.X-p1.X,p2.Y-p1.Y);    
                this.lstSelected = this.lstObject.Where(x =>
                {
                    var p = x.getStartAndEndPoints();
                    RectangleF r_curr = new RectangleF(p.Item1.X, p.Item1.Y, p.Item2.X - p.Item1.X, p.Item2.Y - p.Item2.Y);
                    return r.Contains(r_curr);
                }).ToList();
                this.pnlMain.Refresh();
            }

        }
        private void drawObject(clsDrawObject objDraw, Graphics e)
        {
           
            if (objDraw.getGroup().Count == 0)
            {
                e.SmoothingMode = SmoothingMode.AntiAlias;         
                objDraw.Draw(e, objDraw.isFill);
                return;
            }
            objDraw.getGroup().ForEach(obj =>
            {
                drawObject(obj, e);
            });
        }
        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {
           
            lstObject.ForEach(obj =>
            {              
                drawObject(obj, e.Graphics);
            });
            if (isObjSelected)
            {

                this.lstSelected.ForEach(obj =>
                {
                    var p = obj.getStartAndEndPoints();
                    obj.calculateResizePoints();

                    RectangleF r = new RectangleF(p.Item1.X - 5, p.Item1.Y - 5, p.Item2.X - p.Item1.X + 10, p.Item2.Y - p.Item1.Y + 10);
                    obj.DrawResizableRectangle(e.Graphics, r);

                });

            }
        }

        private void btnFill_Click(object sender, EventArgs e)
        {
            isFill = !isFill;
            btnFill.FlatStyle = FlatStyle.Standard;
            btnFill.FlatAppearance.BorderColor = SystemColors.ControlDarkDark;
            if (isFill)
            {
                btnFill.FlatStyle = FlatStyle.Flat;
                btnFill.FlatAppearance.BorderColor = Color.Orange;
            }

        }

        private void btnEllipse_Click(object sender, EventArgs e)
        {
            resetVar();
            objectCurr = new clsEllipse();
        }
        private void btnRectangle_Click(object sender, EventArgs e)
        {
            resetVar();
            objectCurr = new clsRectangle();
        }
        private void btnTriangle_Click(object sender, EventArgs e)
        {
            resetVar();
            objectCurr = new clsTriangle();
        }
        private void btnRhombus_Click(object sender, EventArgs e)
        {
            resetVar();
            objectCurr = new clsRhombus();
        }
        private void btnPentagon_Click(object sender, EventArgs e)
        {
            resetVar();
            objectCurr = new clsPentagon();
        }
        private void btnHexagon_Click(object sender, EventArgs e)
        {
            resetVar();
            objectCurr = new clsHexagon();
        }
        private void btnPolygon_Click(object sender, EventArgs e)
        {
            resetVar();
            isPolygon = true;
            objectCurr = new clsPolygon();
        }
        private void btnArc_Click(object sender, EventArgs e)
        {
            resetVar();
            objectCurr = new clsArc();
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            resetVar();
            isSelected = true;
            objectCurr = new clsRectangle();  
        }

        private void cMSDStyle_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            int index = this.cMSDStyle.Items.IndexOf(e.ClickedItem);

            switch (index)
            {

                case 0:
                    dashStyle = DashStyle.Solid; break;
                case 1:
                    dashStyle = DashStyle.Dash; break;
                case 2:
                    dashStyle = DashStyle.Dot; break;
                case 3:
                    dashStyle = DashStyle.DashDot; break;
                case 4:
                    dashStyle = DashStyle.DashDotDot; break;
                case 5:
                    dashStyle = DashStyle.Custom; break;
                default: break;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int val = Int32.Parse(txtWidth.Text);
            if (val < 50)
                val += 1;
            txtWidth.Text = val.ToString();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int val = Int32.Parse(txtWidth.Text);
            if (val > 1)
                val -= 1;
            txtWidth.Text = val.ToString();
        }

        private void pnlMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control && e.Button == MouseButtons.Left)
            {
                clsDrawObject cls = checkCursorCurr(e.Location);
                if (cls != null)
                {
                    isObjSelected = true;
                    if (!lstSelected.Contains(cls))
                        this.lstSelected.Add(cls);
                    this.pnlMain.Refresh();
                }
            }
        }

        private void btnGroup_Click(object sender, EventArgs e)
        {
            if (lstSelected.Count > 1)
            {
                clsDrawObject objGroup = new clsDrawObject();
                objGroup.CreateGroupObject(lstSelected);
                float x_min = lstSelected.Min(x => x.lstPoints.Min(p => p.X));
                float y_min = lstSelected.Min(y => y.lstPoints.Min(p => p.Y));
                float x_max = lstSelected.Max(x => x.lstPoints.Max(p => p.X));
                float y_max = lstSelected.Max(y => y.lstPoints.Max(p => p.Y));
                lstSelected.ForEach(obj =>
                {
                    lstObject.Remove(obj);
                });
                objGroup.lstPoints.Add(new PointF(x_min, y_min));
                objGroup.lstPoints.Add(new PointF(x_max, y_max));
                lstObject.Add(objGroup);
                lstSelected.Clear();
                lstSelected.Add(objGroup);
                this.pnlMain.Refresh();

            }
        }

        private void btnUngroup_Click(object sender, EventArgs e)
        {
            if (lstSelected.Count > 0)
            {
                List<clsDrawObject> lstObjGroup = new List<clsDrawObject>();
                clsDrawObject objGroup = new clsDrawObject();
                lstObjGroup = objGroup.UngroupObjects(lstSelected);
                if (lstObjGroup.Count == 0) return;
                lstSelected.ForEach(obj =>
                {
                    lstObject.Remove(obj);
                });

                lstSelected.Clear();
                lstSelected.AddRange(lstObjGroup);
                lstObject.AddRange(lstObjGroup);
                this.pnlMain.Refresh();
            }
        }


        private void btnOpenColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color color = colorDialog1.Color;
                ptbColor.BackColor = color;
            }
        }
        private void color_Click(object sender, EventArgs e)
        {
            PictureBox clr = (PictureBox)sender;
            ptbColor.BackColor = clr.BackColor;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (var x in lstSelected)
            {
                lstObject.Remove(x);
            }
            lstSelected.Clear();
            this.pnlMain.Refresh();
        }

        private void btnPen_Click(object sender, EventArgs e)
        {
            resetVar();
            isPen = true;
            objectCurr = new clsPen();
            
        }
        private void btnEraser_Click(object sender, EventArgs e)
        {

        }

        private void btnLine_Click(object sender, EventArgs e)
        {
            resetVar();
            objectCurr = new clsLine();
        }

        private void btnBrushStyle_Click(object sender, EventArgs e)
        {
            cMSBrush.Show(btnBrushStyle, new Point(0, btnBrushStyle.Height));
        }

        private void cMSBrush_ItemClicked(object sender, ToolStripItemClickedEventArgs e,int index = -1)
        {
            if(index ==-1)
                index = this.cMSBrush.Items.IndexOf(e.ClickedItem);
            switch (index)
            {
                case 0:
                    brushStyle = new SolidBrush(ptbColor.BackColor);
                    break;
                case 1:
                    brushStyle = new HatchBrush(
                   HatchStyle.Horizontal,
                   Color.Red,
                   ptbColor.BackColor);
                    break;
                case 2:
                    brushStyle = new LinearGradientBrush(new Point(0, 0),
                        new Point(100, 100),
                        ptbColor.BackColor,
                        Color.Blue);
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.lstObject.Clear();
            this.lstSelected.Clear();
            this.pnlMain.Refresh();

        }
        private void zoom(clsDrawObject obj, float factor)
        {

            if (obj.getGroup().Count ==0)
            {
                obj.myPen.Width *= factor;
                var lstPoint = obj.lstPoints;
                for (int i = 0; i < lstPoint.Count(); i++)
                {
                    lstPoint[i] = new PointF(lstPoint[i].X * factor, lstPoint[i].Y * factor);
                }
                return;
            }
            foreach(var x in obj.getGroup())
                zoom(x,factor);
            

        }
        private void btnZoomin_Click(object sender, EventArgs e)
        {
            pnlMain.Scale(new SizeF(2.0f, 2.0f));
            foreach (var obj in lstObject)
            {
                zoom(obj, 2.0f);
                if (obj.getGroup().Count !=0)
                {
                    obj.lstPoints[0] = new PointF(obj.lstPoints[0].X * 2, obj.lstPoints[0].Y * 2);
                    obj.lstPoints[1] = new PointF(obj.lstPoints[1].X * 2, obj.lstPoints[1].Y * 2);

                }
            }
            pnlMain.Refresh();
        }
        private void btnZoomout_Click(object sender, EventArgs e)
        {
            pnlMain.Scale(new SizeF(0.5f, 0.5f));
            foreach (var obj in lstObject)
            {
                zoom(obj, 0.5f);
                if (obj.getGroup().Count != 0)
                {
                    obj.lstPoints[0] = new PointF(obj.lstPoints[0].X / 2, obj.lstPoints[0].Y / 2);
                    obj.lstPoints[1] = new PointF(obj.lstPoints[1].X / 2, obj.lstPoints[1].Y / 2);

                }

            }
            pnlMain.Refresh();
        }

    }
}
