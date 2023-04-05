using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;
using static System.Math;

namespace Com.Nakasendo.Gakupetit.Effects;

class E019_Slope : EffectBase, IEffect
{
    public E019_Slope(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 19;
    public string[] Names => new[] { "Tilt", "斜め" };
    public bool IsBackChecked => true;
    public int DefaultValue => 23;
    public string[] Descriptions => new[] {
        $"It's a diagonal line frame effect. Adjust line thickness and count with the slider.",
        $"斜め線で枠を装飾するエフェクトです。スライダーで斜め線の太さと本数を変更できます。" };
    public Color GetDefaultColor(Color nowColor) => Color.White;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        Bitmap bmp = new(srcBitmap);
        var w = bmp.Width;
        var h = bmp.Height;
        var l = Min(w, h) * (10 + v) / 4000.0f;
        var p = v % 16;
        var a = 255 - p * 10;
        var m = l / (2 * p + 10);
        try
        {
            using var g = Graphics.FromImage(bmp);
            using Pen pen = new(Color.FromArgb(a, color), l);
            g.SmoothingMode = SmoothingMode.HighQuality;

            for (int i = 0; i <= p + 1; i++)
            {
                Console.WriteLine(m.ToString());
                // 右下がり
                g.TranslateTransform(w / 2, h / 2);
                g.RotateTransform(i * 0.2f);
                g.TranslateTransform(-w / 2, -h / 2);
                g.DrawRectangle(pen, m * 2, m * 2, w - 1 - m * 4, h - 1 - m * 4);
                g.TranslateTransform(w / 2, h / 2);
                g.RotateTransform(i * 0.2f);
                g.TranslateTransform(-w / 2, -h / 2);
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
