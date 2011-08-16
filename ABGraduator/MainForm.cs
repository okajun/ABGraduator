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
            //カーソル、発射イベントを登録
            graduatorForm.CursorEvent += new GraduatorForm.ChangedHandler(graduatorForm_CurrentEvent);
            graduatorForm.LatestFireEvent += new GraduatorForm.ChangedHandler(graduatorForm_LatestEvent);
            graduatorForm.RecordEvent += new GraduatorForm.ChangedHandler(graduatorForm_RecordEvent);
        }

        void graduatorForm_RecordEvent(object o, double degree)
        {
            //保存した角度を表示
            labelStore.Text = degree.ToString("0.000");
        }

        void graduatorForm_LatestEvent(object o, double degree)
        {
            //最後の発射角度を表示
            labelLatest.Text = degree.ToString("0.000");
        }

        void graduatorForm_CurrentEvent(object o, double degree)
        {
            //カーソル角度を表示
            labelCurrent.Text = degree.ToString("0.000");
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            //目盛りフォームの表示
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
            //タイトルバーがないので、自前のウィンドウ移動処理
            if( e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                beginDragPoint = new Point(e.X, e.Y);
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            //タイトルバーがないので、自前のウィンドウ移動処理
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Left = Left + e.X - beginDragPoint.X;
                Top = Top + e.Y - beginDragPoint.Y;
            }
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            //目盛りフォームはMainFormに追従して動く
            MoveGraduator();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //タイトルバーがないので自前の終了処理
            Close();
        }

        private void record_Click(object sender, EventArgs e)
        {
            //角度の保存処理
            graduatorForm.StoreLatestFire();
        }
    }
}
