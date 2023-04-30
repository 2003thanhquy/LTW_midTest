﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Paint.Class_Shape
{
    public class clsDrawObject
    {
        public List<Point> lstPoints = new List<Point>();
        public bool isFill;
        public int numOfPoints = 0;
        public Pen myPen;//dash style, width ,color
        public Color myColor;
        public Brush myBrush;
        public Point[] recResize = new Point[8];
        List<clsDrawObject> lstGroups = new List<clsDrawObject>();


        public virtual void Draw(Graphics myGp, bool isFill) { return; }

        public void updatePoints()
        {
            int x_min = Math.Min(lstPoints[0].X, lstPoints[1].X), y_min = Math.Min(lstPoints[0].Y, lstPoints[1].Y);
            int x_max = Math.Max(lstPoints[0].X, lstPoints[1].X), y_max = Math.Max(lstPoints[0].Y, lstPoints[1].Y);
            this.lstPoints[0] = new Point(x_min, y_min);
            this.lstPoints[1] = new Point(x_max, y_max);

        }
        public (Point, Point) getStartAndEndPoints()
        {
            int x_min = lstPoints[0].X, x_max = lstPoints[0].X;
            int y_min = lstPoints[0].Y, y_max = lstPoints[0].Y;
            foreach (var p in lstPoints)
            {
                if (p.X < x_min) x_min = p.X;
                if (p.X > x_max) x_max = p.X;
                if (p.Y < y_min) y_min = p.Y;
                if (p.Y > y_max) y_max = p.Y;
            }
            Point pointStart = new Point(x_min, y_min);
            Point pointEnd = new Point(x_max, y_max);
            return (pointStart, pointEnd);
        }
        public void calculateResizePoints()
        {
            var p = getStartAndEndPoints();

            recResize[0] = new Point(p.Item1.X - 9, p.Item1.Y - 9);
            recResize[1] = new Point((int)((p.Item1.X + p.Item2.X) / 2 - 9), p.Item1.Y - 9);
            recResize[2] = new Point(p.Item2.X + 1, p.Item1.Y - 9);
            recResize[3] = new Point(p.Item2.X + 1, (int)((p.Item1.Y + p.Item2.Y) / 2) - 9);
            recResize[4] = new Point(p.Item2.X + 1, p.Item2.Y + 1);
            recResize[5] = new Point((int)((p.Item1.X + p.Item2.X) / 2) - 9, p.Item2.Y + 1);
            recResize[6] = new Point(p.Item1.X - 9, p.Item2.Y + 1);
            recResize[7] = new Point(p.Item1.X - 9, (int)((p.Item1.Y + p.Item2.Y) / 2) - 9);
        }
        public void DrawResizableRectangle(Graphics gp, Rectangle r)
        {
            for (int i = 0; i < 8; i++)
            {
                ControlPaint.DrawBorder(gp, r, Color.Blue, ButtonBorderStyle.Dotted);
                gp.FillRectangle(new SolidBrush(Color.Black), new Rectangle(recResize[i].X, recResize[i].Y, 8, 8));
            }
        }
        public List<clsDrawObject> getGroup()
        {
            return lstGroups;
        }
        public void CreateGroupObject(List<clsDrawObject> lstObjSelected)
        {
            //remove nhung group duoc chon khoi group 
            //tao new group 
            this.lstGroups.AddRange(lstObjSelected);
        }
        public List<clsDrawObject> UngroupObjects(List<clsDrawObject> lstObjSelected)
        {
            //add  lai day  sach group 
            //moi phan  tu trong list la mot group con  
            List<clsDrawObject> lstObj = new List<clsDrawObject>();
            lstObjSelected.ForEach(x =>
            {

                if (x.getGroup().Count == 0)
                    lstObj.Add(x);
                else lstObj.AddRange(x.lstGroups);
            });
            return lstObj;
        }

    }
}