﻿using System.Reflection;
using System.Windows.Forms;

namespace Paint
{
    public static class SolvingFlicker
    {
        public static void SetDoubleBuffered(this Panel panel)
        {
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance |
                BindingFlags.SetProperty, null, panel, new object[] { true });
        }
    }
}
