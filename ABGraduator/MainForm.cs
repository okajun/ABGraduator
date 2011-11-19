using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ABGraduator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        GraduatorForm graduatorForm = new GraduatorForm();
        private void MainForm_Load(object sender, EventArgs e)
        {
            //register mouse cursor event
            graduatorForm.CursorEvent += new GraduatorForm.ChangedHandler(graduatorForm_CurrentEvent);
            //register shot event
            graduatorForm.LatestShotEvent += new GraduatorForm.ChangedHandler(graduatorForm_LatestEvent);
            //register mark event
            graduatorForm.MarkEvent += new GraduatorForm.ChangedHandler(graduatorForm_MarkEvent);
        }

        void graduatorForm_MarkEvent(object o, double degree)
        {
            //display mark angle
            labelStore.Text = degree.ToString("0.000");
        }

        void graduatorForm_LatestEvent(object o, double degree)
        {
            //display latest angle
            labelLatest.Text = degree.ToString("0.000");
        }

        void graduatorForm_CurrentEvent(object o, double degree)
        {
            //display current angle
            labelCurrent.Text = degree.ToString("0.000");
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            //show graduator
            graduatorForm.Show();
            MoveGraduator();
        }

        private void MoveGraduator()
        {
            //move graduator to below this form
            graduatorForm.Left = Left;
            graduatorForm.Top = this.Bottom + 1;
        }

        private Point beginDragPoint;
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {   
            //To move this form when mouse down event
            if( e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                beginDragPoint = new Point(e.X, e.Y);
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            //To move this form when mouse move event
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Left = Left + e.X - beginDragPoint.X;
                Top = Top + e.Y - beginDragPoint.Y;
            }
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            //When the movement of this form, graduator form to follow suit.
            MoveGraduator();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //Because there are no title bar, and explicitly terminated
            Close();
        }

        private void mark_Click(object sender, EventArgs e)
        {
            //mark current angle
            graduatorForm.StoreLatestShot();
        }
    }
}
