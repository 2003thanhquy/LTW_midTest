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
        Point pointStart;
        float zoom = 1f;

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
            this.pnlMain.SetDoubleBuffered();
        }
        public clsDrawObject Init_Object(clsDrawObject drawOject)
        {
            drawOject.myPen = new Pen(ptbColor.BackColor, Int32.Parse(txtWidth.Text));
            drawOject.myPen.DashStyle = dashStyle;
            drawOject.myColor = ptbColor.BackColor;
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

        private void moveCoordAllOfOgroup(clsDrawObject clsObj, int x, int y)
        {
            for (int i = 0; i < clsObj.lstPoints.Count; i++)
            {
                Point p = clsObj.lstPoints[i];
                clsObj.lstPoints[i] = new Point(p.X + x, p.Y + y);
            }
            clsObj.getGroup().ForEach(obj =>
            {
                moveCoordAllOfOgroup(obj, x, y);
            });
        }
        private clsDrawObject moveObject(clsDrawObject clsObj, Point start, Point end)
        {
            this.pointStart = end;
            int x = end.X - start.X;
            int y = end.Y - start.Y;
            moveCoordAllOfOgroup(clsObj, x, y);
            clsObj.calculateResizePoints();
            return clsObj;
        }
        private void resizeObject(clsDrawObject clsObj, int ind, int x, int y)
        {
            switch (ind)
            {
                case 0:
                    clsObj.lstPoints[0] = new Point(clsObj.lstPoints[0].X + x, clsObj.lstPoints[0].Y + y);
                    break;
                case 4:
                    clsObj.lstPoints[1] = new Point(clsObj.lstPoints[1].X + x, clsObj.lstPoints[1].Y + y);
                    break;
                case 2:
                    clsObj.lstPoints[0] = new Point(clsObj.lstPoints[0].X, clsObj.lstPoints[0].Y + y);
                    clsObj.lstPoints[1] = new Point(clsObj.lstPoints[1].X + x, clsObj.lstPoints[1].Y);
                    break;
                case 6:
                    clsObj.lstPoints[0] = new Point(clsObj.lstPoints[0].X + x, clsObj.lstPoints[0].Y);
                    clsObj.lstPoints[1] = new Point(clsObj.lstPoints[1].X, clsObj.lstPoints[1].Y + y);
                    break;
                case 1:
                    clsObj.lstPoints[0] = new Point(clsObj.lstPoints[0].X, clsObj.lstPoints[0].Y + y);
                    break;
                case 5:
                    clsObj.lstPoints[1] = new Point(clsObj.lstPoints[1].X, clsObj.lstPoints[1].Y + y);
                    break;
                case 7:
                    clsObj.lstPoints[0] = new Point(clsObj.lstPoints[0].X + x, clsObj.lstPoints[0].Y);
                    break;
                case 3:
                    clsObj.lstPoints[1] = new Point(clsObj.lstPoints[1].X + x, clsObj.lstPoints[1].Y);
                    break;
                default: break;
            }
            clsObj.calculateResizePoints();
            clsObj.getGroup().ForEach(obj =>
            {
                resizeObject(obj, ind, x, y);
            });

        }
        private void resizeObjectAllOfGroup(clsDrawObject clsObj, int ind, Point start, Point end)
        {
            this.pointStart = end;
            int x = end.X - start.X;
            int y = end.Y - start.Y;
            resizeObject(clsObj, ind, x, y);

        }
        public void searchLocationResize(Point e)
        {
            this.lstSelected.ForEach(x =>
            {
                for (int i = 0; i < 8; i++)
                {
                    Rectangle r = new Rectangle(x.recResize[i].X, x.recResize[i].Y, 8, 8);
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
                btnPen_Click(null, null); ;
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
                Point p1 = this.lstObject[this.lstObject.Count - 1].lstPoints[0];
                Point p2 = this.lstObject[this.lstObject.Count - 1].lstPoints[1];

                this.lstObject.RemoveAt(this.lstObject.Count - 1);
                isSelected = false;
                dashStyle = DashStyle.Solid;
                isObjSelected = true;
                this.pnlMain.Refresh();

                Rectangle r = new Rectangle(p1.X,p1.Y,p2.X-p1.X,p2.Y-p1.Y);    
                this.lstSelected = this.lstObject.Where(x =>
                {
                    var p = x.getStartAndEndPoints();
                    Rectangle r_curr = new Rectangle(p.Item1.X, p.Item1.Y, p.Item2.X - p.Item1.X, p.Item2.Y - p.Item2.Y);
                    return r.Contains(r_curr);
                }).ToList();
                this.pnlMain.Refresh();
            }

        }
        private void drawObject(clsDrawObject objDraw, Graphics e)
        {
            if (objDraw.getGroup().Count == 0)
            {
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
           
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.ScaleTransform(zoom, zoom);
            lstObject.ForEach(obj =>
            {
                
                drawObject(obj, g);
            });
            if (isObjSelected)
            {

                this.lstSelected.ForEach(obj =>
                {
                    var p = obj.getStartAndEndPoints();
                    obj.calculateResizePoints();

                    Rectangle r = new Rectangle(p.Item1.X - 5, p.Item1.Y - 5, p.Item2.X - p.Item1.X + 10, p.Item2.Y - p.Item1.Y + 10);
                    obj.DrawResizableRectangle(g, r);

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

        private void btnSelect_Click(object sender, EventArgs e)
        {
            resetVar();
            if (isFill == true) btnFill_Click(null, null);
            isSelected = true;
            objectCurr = new clsRectangle();
            dashStyle = DashStyle.Dot;

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

        private void btnTriangle_Click(object sender, EventArgs e)
        {
            resetVar();
            objectCurr = new clsTriangle();
        }

        private void btnGroup_Click(object sender, EventArgs e)
        {
            if (lstSelected.Count > 1)
            {
                clsDrawObject objGroup = new clsDrawObject();
                objGroup.CreateGroupObject(lstSelected);
                int x_min = lstSelected.Min(x => x.lstPoints.Min(p => p.X));
                int y_min = lstSelected.Min(y => y.lstPoints.Min(p => p.Y));
                int x_max = lstSelected.Max(x => x.lstPoints.Max(p => p.X));
                int y_max = lstSelected.Max(y => y.lstPoints.Max(p => p.Y));
                lstSelected.ForEach(obj =>
                {
                    lstObject.Remove(obj);
                });
                objGroup.lstPoints.Add(new Point(x_min, y_min));
                objGroup.lstPoints.Add(new Point(x_max, y_max));
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

        private void cMSBrush_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            int index = this.cMSBrush.Items.IndexOf(e.ClickedItem);
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

        private void btnZoomin_Click(object sender, EventArgs e)
        {
            pnlMain.Scale(new SizeF(2.0f, 2.0f));
            zoom *= 2;
            //pnlMain.Location = new Point(0, 0);
            pnlMain.Refresh();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            pnlMain.Scale(new SizeF(0.5f, 0.5f));
            zoom /= 2;
            pnlMain.Refresh();
        }
    }
}

//zoom in , zoom out la tang  kich thuoc cua panel // scroll la ok
//https://stackoverflow.com/questions/32204274/panel-drawing-zoom-in-c-sharp