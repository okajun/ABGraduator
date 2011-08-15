﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MouseKeyboardLibrary;

namespace ABGraduator
{
    public partial class GraduatorForm : Form
    {
        public GraduatorForm()
        {
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                System.Windows.Forms.CreateParams cp = base.CreateParams;

                const int WS_EX_LAYERED = 0x00080000;
                const int WS_EX_TRANSPARENT = 0x00000020;

                cp.ExStyle = cp.ExStyle | WS_EX_LAYERED;
                cp.ExStyle = cp.ExStyle | WS_EX_TRANSPARENT;
                return cp;
            }
        }

        public void RecordPrevPoint()
        {
            recordFire = prevFire;
            Invalidate();
        }

        private Point currentPoint = Point.Empty;
        private Point prevFire = Point.Empty;
        private Point recordFire = Point.Empty;
        private void GraduatorForm_Paint(object sender, PaintEventArgs e)
        {
            const int NUM = 12;
            const float mgn = 2.0f;
            var radius = (float)((Width - mgn*2.0f) / 2.0f);
            var center = new PointF(radius + mgn, radius + mgn);
            var innerCircleRadius = 16.0f;
            var innerRect = new RectangleF(new PointF(mgn + radius - innerCircleRadius, mgn + radius - innerCircleRadius)
                    , new SizeF(innerCircleRadius*2,innerCircleRadius*2));

            using (SolidBrush beforeBrush = new SolidBrush(Color.FromArgb(142, Color.RosyBrown))
                    , currentBrush = new SolidBrush(Color.FromArgb(142, Color.Green)))
            {
                using (Pen gridPen = new Pen(Brushes.Black, 2)
                        , recordPen = new Pen(Brushes.Red, 8)
                        , beforePen = new Pen(beforeBrush, 8)
                        , currentPen = new Pen(currentBrush, 8)
                        )
                {
                    e.Graphics.DrawArc(gridPen, innerRect, 0.0f, 360.0f);
                    for (int i = 0; i < NUM; i++)
                    {
                        var sx = (float)(center.X + (innerCircleRadius * Math.Cos(2 * Math.PI * i / NUM)));
                        var sy = (float)(center.Y + (innerCircleRadius * Math.Sin(2 * Math.PI * i / NUM)));
                        var ex = (float)(center.X + (radius * Math.Cos(2 * Math.PI * i / NUM)));
                        var ey = (float)(center.Y + (radius * Math.Sin(2 * Math.PI * i / NUM)));
                        e.Graphics.DrawLine(gridPen, new PointF(sx, sy), new PointF(ex, ey));
                        e.Graphics.DrawArc(gridPen, new RectangleF(mgn, mgn, radius * 2, radius * 2), 0.0f, 360.0f);
                    }
                    if (!recordFire.IsEmpty)
                    {
                        drawDivideCircle(center, innerCircleRadius, radius, recordFire, e.Graphics, recordPen);
                    }
                    if (!prevFire.IsEmpty)
                    {
                        drawDivideCircle(center, innerCircleRadius, radius, prevFire, e.Graphics, beforePen);
                    }
                    if (!currentPoint.IsEmpty)
                    {
                        drawDivideCircle(center, innerCircleRadius, radius, currentPoint, e.Graphics, currentPen);
                    }
                }
            }
        }

        void drawDivideCircle(PointF center, float innerRadius, float radius, Point containsPoint, Graphics g, Pen pen)
        {
            double r = Math.Atan2(containsPoint.Y - center.Y, containsPoint.X - center.X);
            var ss = new SizeF((float)(innerRadius * Math.Cos(r)), (float)(innerRadius * Math.Sin(r)));
            var es = new SizeF((float)(radius * Math.Cos(r)), (float)(radius * Math.Sin(r)));

            g.DrawLine(pen, center + ss, center + es);
            g.DrawLine(pen, center - ss, center - es);
        }

        MouseHook mouseHook = new MouseHook();
        private void GraduatorForm_Load(object sender, EventArgs e)
        {
            mouseHook.MouseMove += new MouseEventHandler(mouseHook_MouseMove);
            mouseHook.MouseDown += new MouseEventHandler(mouseHook_MouseDown);
            mouseHook.MouseUp += new MouseEventHandler(mouseHook_MouseUp);
            mouseHook.Start();
        }

        void mouseHook_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
                exceptClickTimer.Stop();
                System.Diagnostics.Debug.WriteLine("Timer : " + exceptClickTimer.ElapsedMilliseconds);
                if (250 > exceptClickTimer.ElapsedMilliseconds)
                {
                    return;
                }
                if (StorePoint(e.Location, ref prevFire))
                {
                    Invalidate();
                }
            }
        }

        private bool mouseDown = false;
        private System.Diagnostics.Stopwatch exceptClickTimer = new System.Diagnostics.Stopwatch();
        void mouseHook_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (ClientRectangle.Contains(PointToClient(e.Location)))
                {
                    exceptClickTimer.Reset();
                    exceptClickTimer.Start();
                    mouseDown = true;
                }
            }
        }

        private void mouseHook_MouseMove(object sender, MouseEventArgs e)
        {
            currentPoint = PointToClient(e.Location);
            Invalidate();
        }

        private bool StorePoint(Point p, ref Point tp)
        {
            p = PointToClient(p);
            if (ClientRectangle.Contains(p))
            {
                tp = p;
                return true;
            }
            return false;
        }

        private void GraduatorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            mouseHook.Stop();
        }

    }
}