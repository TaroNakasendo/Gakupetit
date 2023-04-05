using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;
using static System.Math;

namespace Com.Nakasendo.Gakupetit.Effects;

class E010_Rectize : EffectBase, IEffect
{
    public E010_Rectize(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 10;
    public string[] Names => new[] { "Rectangle", "四角" };
    public bool IsBackChecked => true;
    public int DefaultValue => 36;
    public string[] Descriptions => new[] {
        $"Blur the edges with random dots of a single color.  Use the slider to change the area.",
        $"ふちをランダムな単一色のドットでぼかします。スライダーで範囲を変更できます。" };
    public Color GetDefaultColor(Color nowColor) => Color.White;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        var w = srcBitmap.Width;
        var h = srcBitmap.Height;
        Bitmap bmp = new(w, h);
        var m = Min(w, h) * (v * 2) / 3.0f / 480;
        var isLandscape = h < w;

        try
        {
            using var g = Graphics.FromImage(bmp);
            g.Clear(color);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            // 上(または左)から切り取り始める箇所
            var y = isLandscape ? (h * (w - 2 * m) - (h - 2 * m) * w) / (2 * w - 4 * m) : (w * (h - 2 * m) - (w - 2 * m) * h) / (2 * h - 4 * m);
            RectangleF srcRect = isLandscape ? new(0, y, w, h - 2 * y) : new(y, 0, w - 2 * y, h);
            RectangleF desRect = new(m, m, w - 2 * m, h - 2 * m);
            g.DrawImage(srcBitmap, desRect, srcRect, GraphicsUnit.Pixel);

            // 黒枠
            if (v % 2 == 0)
            {
                using Pen pen = new(Color.DarkGray);
                g.SmoothingMode = SmoothingMode.None;
                g.DrawRectangle(pen, 0, 0, w - 1, h - 1);
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
