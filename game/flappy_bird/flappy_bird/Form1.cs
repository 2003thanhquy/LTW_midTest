using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace flappy_bird
{
    public partial class Form1 : Form
    {
        int flying = 15;
        int pipleSpeed = 8;
        public Form1()
        {
            InitializeComponent();
     
            this.KeyDown += pnlMain_KeyDown;
            this.KeyUp += pnlMain_KeyUp;
        }
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            flappyBird.Top += flying;
            pipeBottom.Left -= 10;
            pipeTop.Left -= 10;  
            if(pipeBottom.Left < -pipeBottom.Width)
            {
                pipeBottom.Left += pnlMain.Width +50;
            }
            if (pipeTop.Left < -pipeTop.Width)
            {
                pipeTop.Left += pnlMain.Width +50;
            }
            // kiem tra flappyBird ket thuc
            // endGame
        }
        private void pnlMain_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space)
            {
                flying = -15;
            }
            
        }
        private void pnlMain_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space)
            {
                flying = 15;
            }
            
        }
        private void endGame()
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.pnlMain.Focus();
        }
    }
}
