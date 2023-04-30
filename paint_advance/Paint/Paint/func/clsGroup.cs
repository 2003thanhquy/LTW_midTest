using Paint.Class_Shape;
using System.Collections.Generic;
using System.Drawing;

namespace Paint.func
{
    class clsGroup
    {
        public Point p_min;//== khi mouse up , update lai gia tri p1 = min , p2  =max
        public Point p_max;// == p2 ||
        public clsDrawObject objGrouped = null;// chi khi ve mot doi tuong 
        public List<clsGroup> gr = new List<clsGroup>();
        public void updateRegion()
        {
            int x_min = int.MaxValue, y_min = int.MaxValue;
            int x_max = int.MinValue, y_max = int.MinValue;
            gr.ForEach(x =>
            {
                if (x_min > x.p_min.X) x_min = x.p_min.X;
                if (x_max < x.p_max.X) y_max = x.p_max.X;
                if (y_min > x.p_min.Y) x_min = x.p_min.Y;
                if (y_max < x.p_max.Y) y_max = x.p_max.Y;


            });
            this.p_min = new Point(x_min, y_min);
            this.p_max = new Point(y_max, x_max);
        }
        public void group(List<clsGroup> lstObjSelected)
        {
            //remove nhung group duoc chon khoi group 
            //tao new group 
            this.gr.AddRange(lstObjSelected);
        }
        public List<clsGroup> ungroup(List<clsGroup> lstObjSelected)
        {
            //add  lai day  sach group 
            //moi phan  tu trong list la mot group con  
            List<clsGroup> lstObj = new List<clsGroup>();
            lstObjSelected.ForEach(x =>
            {
                lstObj.AddRange(x.gr);
            });
            return lstObj;
        }
    }
}
