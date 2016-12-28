using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace NextCapture
{
    class InfoWindow : NextWindow
    {
        public InfoWindow() : base()
        {
            this.Size = new Size(230, 310);
            this.ShowInTaskbar = false;
            this.ShowIcon = false;
            this.Title = " ";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // resources
            Bitmap logo = Properties.Resources.logo;

            Font verFont = new Font("맑은 고딕", 12f, FontStyle.Bold);
            Font discFont = new Font("맑은 고딕", 10f);

            var verBrush = new SolidBrush(Config.Color.Accent);
            var verDiscBrush = new SolidBrush(Config.Color.Disabled);
            var infoBrush = new SolidBrush(Color.FromArgb(120, 120, 120));

            string verString = $"Ver. {Config.Version.ToString()}";
            string verStateString = "최신버전입니다."; // TODO: online checking
            string cpString = "오픈소스 저작권 정보";

            int totalTop = 0;

            // logo
            int logoHeight = 76;

            e.Graphics.DrawImage(logo, 
                new RectangleF(
                    new PointF(ToCenterX(logoHeight), 10),
                    new SizeF(logoHeight, logoHeight)));

            totalTop += logoHeight + 30;

            // version
            var verSize = e.Graphics.MeasureString(verString, verFont);
            e.Graphics.DrawString(verString, verFont, verBrush,
                new PointF(ToCenterX(verSize.Width), totalTop));

            totalTop += (int)(verSize.Height + 6);

            // version state
            var verStateSize = e.Graphics.MeasureString(verStateString, discFont);
            e.Graphics.DrawString(verStateString, discFont, verDiscBrush,
                new PointF(ToCenterX(verStateSize.Width), totalTop));

            totalTop += (int)(verStateSize.Height + 6);

            // * bottom title
            var tSize = e.Graphics.MeasureString(Config.AppName, discFont);

            totalTop = (int)(ContentHeight - tSize.Height - 12);

            e.Graphics.DrawString(Config.AppName, discFont, infoBrush,
                new PointF(ToCenterX(tSize.Width), totalTop));

            // copyright
            var cpSize = e.Graphics.MeasureString(cpString, discFont);

            totalTop -= (int)(cpSize.Height + 6);

            e.Graphics.DrawString(cpString, discFont, infoBrush,
                new PointF(ToCenterX(cpSize.Width), totalTop));

            // release resources
            verFont.Dispose();
            discFont.Dispose();
            verBrush.Dispose();
            verDiscBrush.Dispose();
            infoBrush.Dispose();
        }

        private void lblCp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/SteaI/NextCapture");
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            this.Close();
        }
    }
}
