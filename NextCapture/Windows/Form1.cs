using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NextCapture
{
    public partial class Form1 : Form
    {
        NotifyIcon notify;

        public Form1()
        {
            InitializeComponent();
            InitializeNotify();

            this.Opacity = 0;
        }

        private void InitializeNotify()
        {
            var ctx = new ContextMenu();
            ctx.MenuItems.Add(new MenuItem("NextCapture 정보", NotifyIcon_Info));
            ctx.MenuItems.Add(new MenuItem("-"));
            ctx.MenuItems.Add(new MenuItem("종료", NotifyIcon_Close));

            notify = new NotifyIcon()
            {
                Icon = Properties.Resources.icon,
                Text = "NextCapture",
                ContextMenu = ctx
            };
            
            notify.Visible = true;
            this.Hide();
        }

        private void NotifyIcon_Info(object sender, EventArgs e)
        {
            
        }

        private void NotifyIcon_Close(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            notify.Visible = false;
            base.OnClosed(e);
        }
    }
}
