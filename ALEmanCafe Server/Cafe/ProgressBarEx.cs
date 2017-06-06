using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace QuantumConcepts.Common.Forms.UI.Controls
{
    internal class ProgressBarEx : ProgressBar
    {
        public ProgressBarEx()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rec = new Rectangle(0, 0, this.Width, this.Height);

            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, rec);

            LinearGradientBrush brush = null;
            rec.Width = (int)(rec.Width * ((double)base.Value / Maximum)) - 4;
            rec.Height -= 4;
            brush = new LinearGradientBrush(rec, this.ForeColor, this.BackColor, LinearGradientMode.Vertical);
            try
            {
                e.Graphics.FillRectangle(brush, 2, 2, rec.Width, rec.Height);
            }
            catch { }
        }
        
    }
}