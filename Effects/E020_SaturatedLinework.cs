using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;

namespace Com.Nakasendo.Gakupetit.Effects;

class E020_SaturatedLinework : EffectBase, IEffect
{
    public E020_SaturatedLinework(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 20;
    public string[] Names => new[] { "Saturated linework", "集中線" };
    public bool IsBackChecked => false;
    public int DefaultValue => 80;
    public string[] Descriptions => new[] {
        $"Concentration line effect. The number of concentration lines can be changed with the slider.",
        $"集中線のエフェクトです。スライダーで集中戦の本数を変更できます。" };
    public Color GetDefaultColor(Color nowColor) => Color.White;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        Bitmap bmp = new(srcBitmap);

        try
        {
            var w = bmp.Width;
            var h = bmp.Height;
            var d = (int)Math.Sqrt(w * w + h * h);

            var span = 60 + v * (360 - 60) / 100; // 30～360まで
            var r = span / 30;

            using var g = Graphics.FromImage(bmp);
            // なめらかにする
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Random rnd = new(1);
            using SolidBrush sb = new(color);
            for (var i = 0; i <= 360; i += r)
            {
                var rv = (250 + rnd.Next(100)) / 300.0f;
                var rv2 = (10 + rnd.Next(100)) / 100.0f;
                var offset = v * rv2 / 40.0f + 3;
                var x = -(d - w) / 2 + (int)((d / offset) * Math.Cos(Math.PI * i / 180));
                var y = -(d - h) / 2 + (int)((d / offset) * Math.Sin(Math.PI * i / 180));


                g.FillPie(sb, new Rectangle(x, y, d, d), i, r * rv2 / 2.0f + 1);
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
