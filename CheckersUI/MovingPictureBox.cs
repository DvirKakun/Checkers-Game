using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace CheckersUI
{
    public class MovingPictureBox : PictureBox
    {
        private Button m_FromButton = null;
        private Button m_ToButton = null;
        private int m_XStep;
        private int m_YStep;
        private readonly Timer r_AnimationTimer;
        public event Action AnimationStopped;
        

        public MovingPictureBox(int i_PictureBoxSize, Color i_BackColor)
        {
            this.Size = new Size(i_PictureBoxSize, i_PictureBoxSize);
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.BackColor = i_BackColor;
            this.makePictureBoxRounded();
            this.Visible = false;
            this.r_AnimationTimer = new Timer();
            this.r_AnimationTimer.Interval = 1;
            this.r_AnimationTimer.Tick += r_AnimationTimer_Tick;
        }

        private void makePictureBoxRounded()
        {
            GraphicsPath path = new GraphicsPath();

            path.AddArc(0, 0, this.Size.Width, this.Size.Height, 0, 360);
            path.CloseAllFigures();
            this.Region = new Region(path);
        }

        public void StartAnimation(Button i_FromButton, Button i_ToButton, Image i_MoveImage)
        {
            this.m_FromButton = i_FromButton;
            this.m_ToButton = i_ToButton;
            this.Image = i_MoveImage;
            this.Location = this.m_FromButton.Location;
            this.Visible = true;
            this.m_XStep = this.m_FromButton.Location.X < this.m_ToButton.Location.X ? 2 : -2;
            this.m_YStep = this.m_FromButton.Location.Y < this.m_ToButton.Location.Y ? 2 : -2;
            this.r_AnimationTimer.Start();
        }

        private void r_AnimationTimer_Tick(object sender, EventArgs e)
        {
            this.Left += this.m_XStep;
            this.Top += this.m_YStep;

            if(this.Location.Equals(this.m_ToButton.Location))
            {
                this.r_AnimationTimer.Stop();
                this.Visible = false;
                this.AnimationStopped?.Invoke();
            }
        }
    }
}