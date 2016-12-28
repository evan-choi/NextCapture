using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NextCapture
{
    class NextWindow : AeroWindow
    {
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                this.Text = value;

                UpdateLayout();
            }
        }

        protected int TitleHeight { get; set; }

        public int ContentWidth { get { return this.Width - 2; } }
        public int ContentHeight { get { return this.Height - TitleHeight  - 2; } }

        private bool isActivated = false;
        private Rectangle closeRect;

        public NextWindow() : base()
        {
            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Icon = Properties.Resources.icon;
        }

        protected int ToCenterX(int width)
        {
            return ContentWidth / 2 - width / 2;
        }

        protected float ToCenterX(float width)
        {
            return ContentWidth / 2f - width / 2f;
        }

        protected void SetContentSize(Size size)
        {
            this.Size = size + new Size(2, 2 + TitleHeight);
        }

        public void UpdateLayout()
        {
            this.Invalidate();
        }

        protected T SelectActivate<T>(T activated, T deactivated)
        {
            return isActivated ? activated : deactivated;
        }
        
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (closeRect.Contains(e.Location))
                Cursor = Cursors.Hand;
            else
                Cursor = Cursors.Default;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            var mouseArgs = e as MouseEventArgs;

            if (closeRect.Contains(mouseArgs.Location))
                this.Close();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            UpdateLayout();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            isActivated = true;
            UpdateLayout();
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);

            isActivated = false;
            UpdateLayout();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int blank = 6;

            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;

            // resources
            Bitmap closeIcon = Properties.Resources.close;

            var titleFont = new Font("맑은 고딕", 10);
            var textBrushes = new SolidBrush(
                SelectActivate(
                    Config.Color.Disabled,
                    Color.FromArgb(200, Config.Color.Disabled)));

            // title
            var titleSize = e.Graphics.MeasureString(title, titleFont);
            TitleHeight = (int)(titleSize.Height + blank * 2);

            e.Graphics.DrawString(title, titleFont, textBrushes, new Point(blank, blank));

            // close
            int iconBlank = Math.Max(blank, TitleHeight / 2 - closeIcon.Height / 2);

            closeRect = new Rectangle(
                ContentWidth - closeIcon.Width - iconBlank, iconBlank,
                closeIcon.Width, closeIcon.Height);

            e.Graphics.DrawImage(closeIcon, closeRect);
            closeRect.Inflate(1, 1);

            AddHitVisibleBounds("Close", closeRect);

            // Border
            using (var brush = new SolidBrush(SelectActivate(Config.Color.Accent, Config.Color.Disabled)))
            {
                using (var pen = new Pen(brush))
                {
                    e.Graphics.DrawRectangle(pen,
                        new Rectangle(
                            Point.Empty,
                            new Size(Width - 1, Height - 1)));
                }
            }

            // release resources
            titleFont.Dispose();
            textBrushes.Dispose();
            
            // clip
            e.Graphics.TranslateTransform(1, 1 + TitleHeight);
            e.Graphics.SetClip(new Rectangle(0, 0, ContentWidth, ContentHeight));
        }
    }
}