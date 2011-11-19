using System;
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
        public delegate void ChangedHandler(object o, double degree);   //event handler
        public event ChangedHandler CursorEvent = delegate(Object s, double degree) { };        //mouse cursor evnet
        public event ChangedHandler LatestShotEvent = delegate(Object s, double degree) { };    //shot event
        public event ChangedHandler MarkEvent = delegate(Object s, double degree) { };          //mark event

        public GraduatorForm()
        {
            InitializeComponent();
        }

        public double CurrentDegree
        {
            get
            {
                return PointToDegree(currentPoint);
            }
        }

        public double LatestDegree
        {
            get
            {
                return PointToDegree(latestShot);
            }
        }

        public double RecordDegree
        {
            get
            {
                return PointToDegree(markShot);
            }
        }

        public void StoreLatestShot()
        {

            markShot = latestShot;
            MarkEvent(this, RecordDegree);
            Invalidate(); 
        }

        protected override CreateParams CreateParams
        {
            get
            {
                //Set up transparent window.
                System.Windows.Forms.CreateParams cp = base.CreateParams;
                const int WS_EX_LAYERED = 0x00080000;
                const int WS_EX_TRANSPARENT = 0x00000020;

                cp.ExStyle = cp.ExStyle | WS_EX_LAYERED;        // Layered Windows
                cp.ExStyle = cp.ExStyle | WS_EX_TRANSPARENT;    // Don't hittest this window
                return cp;
            }
        }

        private const int NUM = 24;                     //interval graduations 360 / 24 = 15degree
        private const float MGN = 2.0f;                 //margin for between the graduator with the border
        private const float centerCircleRadius = 24.0f;  //radius for center circle 

        private Point currentPoint = Point.Empty;       //mouse cursor point
        private Point latestShot = Point.Empty;         //latest shot point
        private Point markShot = Point.Empty;           //mark point

        private float radius;                           //radius for outer circle 
        private float innerRadius;                      //radius for inner circle
        private PointF center = PointF.Empty;           //center point
        private RectangleF centerRect = RectangleF.Empty;//bounding rectangle for center circle

        //To calculate size of graduator.
        private void UpdateGraduatorSize()
        {
            radius = (float)(( ((Width > Height) ? Height : Width) - MGN * 2.0f) / 2.0f);
            innerRadius = radius - 16.0f;
            center = new PointF(radius + MGN, radius + MGN);
            centerRect = new RectangleF(new PointF(MGN + radius - centerCircleRadius, MGN + radius - centerCircleRadius)
                    , new SizeF(centerCircleRadius * 2, centerCircleRadius * 2));
        }

        //draw graduator
        private void GraduatorForm_Paint(object sender, PaintEventArgs e)
        {
            using (SolidBrush latestBrush = new SolidBrush(Color.FromArgb(142, Color.RosyBrown))
                    , currentBrush = new SolidBrush(Color.FromArgb(142, Color.Green)))
            {
                using (Pen gaugePen = new Pen(Brushes.Black, 2)
                        , recordPen = new Pen(Brushes.Red, 8)
                        , latestPen = new Pen(latestBrush, 8)
                        , currentPen = new Pen(currentBrush, 8)
                        )
                {
                    e.Graphics.DrawArc(gaugePen, centerRect, 0.0f, 360.0f);  //draw outer circle
                    e.Graphics.DrawArc(gaugePen, new RectangleF(MGN, MGN, radius * 2, radius * 2), 0.0f, 360.0f);   //draw center circle
                    //draw graduations
                    for (int i = 0; i < NUM; ++i)
                    {
                        var r = 2 * Math.PI * i / NUM;
                        //for each 15 degree short line and 30 degree long line
                        var sx = ((i%2) !=0) ? (float)(center.X + (innerRadius * Math.Cos(2 * Math.PI * i / NUM)))
                            : (float)(center.X + (centerCircleRadius * Math.Cos(r)));
                        var sy = ((i%2) !=0) ? (float)(center.Y + (innerRadius * Math.Sin(r)))
                            : (float)(center.Y + (centerCircleRadius * Math.Sin(r)));
                        var ex = (float)(center.X + (radius * Math.Cos(r)));
                        var ey = (float)(center.Y + (radius * Math.Sin(r)));
                        e.Graphics.DrawLine(gaugePen, new PointF(sx, sy), new PointF(ex, ey));
                    }
                    if (!markShot.IsEmpty)
                    {
                        //draw marked line
                        drawDivideCircleLine(center, centerCircleRadius, radius, markShot, e.Graphics, recordPen);
                    }
                    if (!latestShot.IsEmpty)
                    {
                        //draw latest line
                        drawDivideCircleLine(center, centerCircleRadius, radius, latestShot, e.Graphics, latestPen);
                    }
                    if (!currentPoint.IsEmpty)
                    {
                        //draw current line
                        drawDivideCircleLine(center, centerCircleRadius, radius, currentPoint, e.Graphics, currentPen);
                    }
                }
            }
        }
         
        //To calculate the angle from the center point of the graduator.
        private double PointToDegree(PointF p)
        {
            if (p.IsEmpty)
            {
                return double.NaN;
            }
            double r = Math.Atan2(p.Y - center.Y, -(p.X - center.X));
            return r * 180 / Math.PI;
        }

        //Use to current, mark and latest line.
        private void drawDivideCircleLine(PointF center, float innerRadius, float radius, Point containsPoint, Graphics g, Pen pen)
        {
            double r = Math.Atan2(containsPoint.Y - center.Y, containsPoint.X - center.X);
            var ss = new SizeF((float)(innerRadius * Math.Cos(r)), (float)(innerRadius * Math.Sin(r)));
            var es = new SizeF((float)(radius * Math.Cos(r)), (float)(radius * Math.Sin(r)));

            g.DrawLine(pen, center + ss, center + es);
            g.DrawLine(pen, center - ss, center - es);
        }

        private MouseHook mouseHook = new MouseHook();
        private void GraduatorForm_Load(object sender, EventArgs e)
        {
            //Set up the global hook.
            // Global Mouse and Keyboard Library
            // http://www.codeproject.com/KB/system/globalmousekeyboardlib.aspx
            mouseHook.MouseMove += new MouseEventHandler(mouseHook_MouseMove);
            mouseHook.MouseDown += new MouseEventHandler(mouseHook_MouseDown);
            mouseHook.MouseUp += new MouseEventHandler(mouseHook_MouseUp);

            mouseHook.Start();
        }

        //Mouse up on the global hook.
        private void mouseHook_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown) // did mouse down?
            {
                mouseDown = false;
                exceptClickTimer.Stop();
                if (250 > exceptClickTimer.ElapsedMilliseconds)
                {
                    //ignore less than 250ms
                    return;
                }
                //Assume shooting the bird.
                latestShot = PointToClient(e.Location);
                LatestShotEvent(this, LatestDegree);
                Invalidate();
            }
        }

        private bool mouseDown = false;
        private System.Diagnostics.Stopwatch exceptClickTimer = new System.Diagnostics.Stopwatch();
        //Mouse down on the global hook.
        private void mouseHook_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                //if (ClientRectangle.Contains(PointToClient(e.Location)))
                if (centerRect.Contains(PointToClient(e.Location)))
                {
                    //If the location is contained center circle of graduator, assume catching the bird.
                    exceptClickTimer.Reset();
                    exceptClickTimer.Start();
                    mouseDown = true;
                }
            }
        }

        //Mouse move on the global hook.
        private void mouseHook_MouseMove(object sender, MouseEventArgs e)
        {
            currentPoint = PointToClient(e.Location);
            CursorEvent(this, CurrentDegree);   // cursor event shot
            Invalidate();
        }

        private void GraduatorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            mouseHook.Stop();
        }

        private void GraduatorForm_Resize(object sender, EventArgs e)
        {
            UpdateGraduatorSize();
        }

    }
}
