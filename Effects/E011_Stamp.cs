using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;

namespace Com.Nakasendo.Gakupetit.Effects;

class E011_Stamp : EffectBase, IEffect
{
    public E011_Stamp(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 11;
    public string[] Names => new[] { "Stamp", "切手" };
    public bool IsBackChecked => true;
    public int DefaultValue => 100;
    public string[] Descriptions => new[] {
        $"It's a stamp-like frame effect. Adjust frame size with the slider.",
        $"切手のような枠をつけるエフェクトです。スライダーで枠の大きさを変更できます。" };
    public Color GetDefaultColor(Color nowColor) => nowColor;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        Bitmap bmp = new(srcBitmap);

        try
        {
            var w = bmp.Width;
            var h = bmp.Height;

            var shortLen = (w > h) ? h : w;

            var span = shortLen / (SliderMax - v + 10); // 10～110まで
            if (span < 3) span = 3;
            var r = span / 3;
            var d = span * 2 / 3;

            using var g = Graphics.FromImage(bmp);
            // なめらかにする
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 白枠
            using Pen whitePen = new(Color.FromArgb(245, 245, 245), d * 2);
            g.DrawRectangle(whitePen, d, d, w - d * 2 - 1, h - d * 2 - 1);

            using SolidBrush sb = new(color);
            using Pen blackPen = new(Color.Black);
            g.DrawRectangle(blackPen, 0, 0, w - 1, h - 1);
            // 左右
            for (var j = 0; j <= h; j += span)
            {
                // 影
                g.DrawEllipse(blackPen, -r, j - r, d, d);
                g.DrawEllipse(blackPen, w - 1 - r, j - r, d, d);
                // 穴
                g.FillEllipse(sb, -r, j - r, d, d);
                g.FillEllipse(sb, w - 1 - r, j - r, d, d);
            }

            // 上下
            for (var i = 0; i <= w; i += span)
            {
                // 影
                g.DrawEllipse(blackPen, i - r, -r, d, d);
                g.DrawEllipse(blackPen, i - r, h - 1 - r, d, d);
                // 穴
                g.FillEllipse(sb, i - r, -r, d, d);
                g.FillEllipse(sb, i - r, h - 1 - r, d, d);
            }
        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }

        return bmp;
    }
}
