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
        public delegate void ChangedHandler(object o, double degree);
        public event ChangedHandler CursorEvent = delegate(Object s, double degree) { };    //カーソル変更イベント
        public event ChangedHandler LatestFireEvent = delegate(Object s, double degree) { };    //発射イベント
        public event ChangedHandler RecordEvent = delegate(Object s, double degree) { };    //保存イベント

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
                return PointToDegree(latestFire);
            }
        }

        public double RecordDegree
        {
            get
            {
                return PointToDegree(recordFire);
            }
        }

        public void StoreLatestFire()
        {

            recordFire = latestFire;
            RecordEvent(this, RecordDegree);
            Invalidate(); 
        }

        protected override CreateParams CreateParams
        {
            get
            {
                //マウスイベント透過の設定
                System.Windows.Forms.CreateParams cp = base.CreateParams;
                const int WS_EX_LAYERED = 0x00080000;
                const int WS_EX_TRANSPARENT = 0x00000020;

                cp.ExStyle = cp.ExStyle | WS_EX_LAYERED;
                cp.ExStyle = cp.ExStyle | WS_EX_TRANSPARENT;
                return cp;
            }
        }

        //以下、ゲージ描画用パラメータ
        private const int NUM = 24;                     //目盛りのstep値 = 360 / 24 = 15度
        private const float MGN = 2.0f;                 //目盛りとフォームとの間のマージン
        private const float innerCircleRadius = 24.0f;  //鳥捕捉用センターサークル半径

        private Point currentPoint = Point.Empty;       //マウスカーソル位置
        private Point latestFire = Point.Empty;         //最後に発射した位置
        private Point recordFire = Point.Empty;         //保存した位置

        private float radius;                           //目盛りの外周半径
        private float gaugeRadius;                      //短い目盛りの半径
        private PointF center = PointF.Empty;           //目盛り外周円の中心
        private RectangleF innerRect = RectangleF.Empty;//鳥捕捉センターサークルに外接するRectangle

        //Formリサイズ用にメモリ座標の計算
        //今は固定サイズだけれども、念のため。
        private void UpdateGaugeSize()
        {
            radius = (float)(( ((Width > Height) ? Height : Width) - MGN * 2.0f) / 2.0f);
            gaugeRadius = radius - 16.0f;
            center = new PointF(radius + MGN, radius + MGN);
            innerRect = new RectangleF(new PointF(MGN + radius - innerCircleRadius, MGN + radius - innerCircleRadius)
                    , new SizeF(innerCircleRadius * 2, innerCircleRadius * 2));
        }

        //描画
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
                    e.Graphics.DrawArc(gaugePen, innerRect, 0.0f, 360.0f);  //外周円
                    e.Graphics.DrawArc(gaugePen, new RectangleF(MGN, MGN, radius * 2, radius * 2), 0.0f, 360.0f);   //鳥捕捉センターサークル
                    //目盛り描画
                    for (int i = 0; i < NUM; ++i)
                    {
                        var r = 2 * Math.PI * i / NUM;
                        //15度単位で短い目盛り、30度単位で長い目盛り
                        var sx = ((i%2) !=0) ? (float)(center.X + (gaugeRadius * Math.Cos(2 * Math.PI * i / NUM)))
                            : (float)(center.X + (innerCircleRadius * Math.Cos(r)));
                        var sy = ((i%2) !=0) ? (float)(center.Y + (gaugeRadius * Math.Sin(r)))
                            : (float)(center.Y + (innerCircleRadius * Math.Sin(r)));
                        var ex = (float)(center.X + (radius * Math.Cos(r)));
                        var ey = (float)(center.Y + (radius * Math.Sin(r)));
                        e.Graphics.DrawLine(gaugePen, new PointF(sx, sy), new PointF(ex, ey));
                    }
                    if (!recordFire.IsEmpty)
                    {
                        //保存した角度描画
                        drawDivideCircle(center, innerCircleRadius, radius, recordFire, e.Graphics, recordPen);
                    }
                    if (!latestFire.IsEmpty)
                    {
                        //最後の発射角度描画
                        drawDivideCircle(center, innerCircleRadius, radius, latestFire, e.Graphics, latestPen);
                    }
                    if (!currentPoint.IsEmpty)
                    {
                        //カーソル位置の角度描画
                        drawDivideCircle(center, innerCircleRadius, radius, currentPoint, e.Graphics, currentPen);
                    }
                }
            }
        }

        //クライアント位置->角度計算
        private double PointToDegree(PointF p)
        {
            if (p.IsEmpty)
            {
                return double.NaN;
            }
            double r = Math.Atan2(p.Y - center.Y, -(p.X - center.X));
            return r * 180 / Math.PI;
        }

        //角度描画
        private void drawDivideCircle(PointF center, float innerRadius, float radius, Point containsPoint, Graphics g, Pen pen)
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
            //グローバルフックによるカーソルイベントの捕捉設定
            //Global Mouse and Keyboard Library を利用
            // http://www.codeproject.com/KB/system/globalmousekeyboardlib.aspx
            mouseHook.MouseMove += new MouseEventHandler(mouseHook_MouseMove);
            mouseHook.MouseDown += new MouseEventHandler(mouseHook_MouseDown);
            mouseHook.MouseUp += new MouseEventHandler(mouseHook_MouseUp);
            //マウスフック実行
            mouseHook.Start();
        }

        //マウスボタンアップ
        private void mouseHook_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
                exceptClickTimer.Stop();
                if (250 > exceptClickTimer.ElapsedMilliseconds)
                {
                    //押下時間が250ms未満の場合は無視
                    return;
                }
                latestFire = PointToClient(e.Location);
                LatestFireEvent(this, LatestDegree);
                Invalidate();
            }
        }

        private bool mouseDown = false;
        private System.Diagnostics.Stopwatch exceptClickTimer = new System.Diagnostics.Stopwatch();
        //マウスボタンダウン
        private void mouseHook_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                //if (ClientRectangle.Contains(PointToClient(e.Location)))
                if (innerRect.Contains(PointToClient(e.Location)))
                {
                    exceptClickTimer.Reset();
                    exceptClickTimer.Start();
                    mouseDown = true;
                }
            }
        }

        //マウスカーソル移動
        private void mouseHook_MouseMove(object sender, MouseEventArgs e)
        {
            currentPoint = PointToClient(e.Location);
            CursorEvent(this, CurrentDegree);
            Invalidate();
        }

        private void GraduatorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //マウスフック終了
            mouseHook.Stop();
        }

        private void GraduatorForm_Resize(object sender, EventArgs e)
        {
            //目盛り描画諸元の計算
            UpdateGaugeSize();
        }

    }
}
