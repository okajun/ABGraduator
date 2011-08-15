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
        private void MainForm_Shown(object sender, EventArgs e)
        {
            graduatorForm.Show();
            MoveGraduator();
        }

        private void MoveGraduator()
        {
            graduatorForm.Left = Left;
            graduatorForm.Top = this.Bottom + 1;
        }

        private Point beginDragPoint;
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if( e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                beginDragPoint = new Point(e.X, e.Y);
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Left = Left + e.X - beginDragPoint.X;
                Top = Top + e.Y - beginDragPoint.Y;
            }
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            MoveGraduator();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void record_Click(object sender, EventArgs e)
        {
            graduatorForm.RecordPrevPoint();
        }

    }
}
