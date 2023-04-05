using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;

namespace Com.Nakasendo.Gakupetit.Effects;

class E013_Halftone : EffectBase, IEffect
{
    public E013_Halftone(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 13;
    public string[] Names => new[] { "Halftone", "網点" };
    public bool IsBackChecked => true;
    public int DefaultValue => 20;
    public string[] Descriptions => new[] {
        $"It's a polka dot border effect. Adjust the dot size with the slider.",
        $"ふちを水玉模様で彩るエフェクトです。スライダーで水玉の大きさを変更できます。" };
    public Color GetDefaultColor(Color nowColor) => nowColor;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        Bitmap bmp = new(srcBitmap);

        try
        {
            var w = bmp.Width;
            var h = bmp.Height;


            using var g = Graphics.FromImage(bmp);
            // なめらかにする
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var span = (int)((4 + v * (128 - 4) / 100) * g.DpiX / 96); // 4～128まで
            var r = span / 3f;
            var d = 2 * r;


            using SolidBrush sb = new(color);
            // 左右
            for (var j = 0; j <= h; j += span)
            {
                // 左
                g.FillEllipse(sb, -r, j - r, d, d);
                g.FillEllipse(sb, r, j + r, r, r);
                g.FillEllipse(sb, 3 * r / 2, j - r / 3, d / 3, d / 3);
                // 右
                g.FillEllipse(sb, w - 1 - r, j - r, d, d);
                g.FillEllipse(sb, w - 1 - 2 * r, j + r, r, r);
                g.FillEllipse(sb, w - 1 - 3 * r / 2 - d / 3, j - r / 3, d / 3, d / 3);
            }

            // 上下
            for (var i = 0; i <= w; i += span)
            {
                // 上
                g.FillEllipse(sb, i - r, -r, d, d);
                g.FillEllipse(sb, i + r, r, r, r);
                g.FillEllipse(sb, i - r / 3, 3 * r / 2, d / 3, d / 3);
                // 下
                g.FillEllipse(sb, i - r, h - 1 - r, d, d);
                g.FillEllipse(sb, i + r, h - 1 - 2 * r, r, r);
                g.FillEllipse(sb, i - r / 3, h - 1 - 3 * r / 2 - d / 3, d / 3, d / 3);
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
