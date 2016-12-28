using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NextCapture
{
    class MessageWindow : NextWindow
    {
        private string content;
        public string Content
        {
            get { return content; }
            set
            {
                content = value;
                UpdateLayout();
            }
        }
        
        public MessageWindow(string title, string content) : base()
        {
            this.Title = title;
            this.Content = content;

            this.Font = new Font("맑은 고딕", 12);

            //this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;

            var screen = Screen.FromPoint(MousePosition);
            var contentSize = TextRenderer.MeasureText(content, this.Font);

            this.Size = new Size(
                2 + contentSize.Width + 128,
                2 + contentSize.Height + 128);

            this.Location = new Point(
                screen.WorkingArea.Width / 2 - Size.Width / 2,
                screen.WorkingArea.Height / 2 - Size.Height / 2);
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            // resources
            var contentBrushes = new SolidBrush(Config.Color.Disabled);

            // content
            var contentSize = e.Graphics.MeasureString(content, this.Font);
            string[] lines = Regex.Split(content, "\r\n");
            
            for (int i = 0; i < lines.Length; i++)
            {
                var lineSize = e.Graphics.MeasureString(lines[i], this.Font);
                var linePos = new Point(
                    (int)(ContentWidth / 2 - lineSize.Width / 2),
                    (int)(ContentHeight / 2 - contentSize.Height / 2 + (contentSize.Height / lines.Length * i)));

                e.Graphics.DrawString(lines[i], this.Font, contentBrushes, linePos);
            }
            
            // release resources
            contentBrushes.Dispose();
        }
    }
}
