using Com.Nakasendo.Gakupetit.EffectEtc;

namespace Com.Nakasendo.Gakupetit.Effects;

class E012_Flower : EffectBase, IEffect
{
    public E012_Flower(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 12;
    public string[] Names => new[] { "Flower", "フラワー" };
    public bool IsBackChecked => true;
    public int DefaultValue => 5;
    public string[] Descriptions => new[] {
        $"It's a flower-shaped cutout effect. Adjust the number of petals with the slider.",
        $"花ような切り抜きをつけるエフェクトです。スライダーで花びらの数を変更できます。" };
    public Color GetDefaultColor(Color nowColor) => nowColor;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        var w = srcBitmap.Width;
        var h = srcBitmap.Height;

        Bitmap bmp = new(srcBitmap);

        try
        {
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TranslateTransform(w / 2, h / 2);

            var z = v + 1;

            // 対角線角度
            var theta = Math.Atan2(h / 2, w / 2) * 180 / Math.PI;
            var hh = h / 2.0f * (float)Math.Pow(z, -0.5) / 1.2f;

            for (var i = 0; i < z + 1; i++)
            {
                var angle = 360f / (z + 1);
                var anglei = angle * i;


                var ww = 0.96f / 2;

                if (anglei < theta || (anglei > 180 - theta && anglei < 180 + theta) || anglei > 360 - theta)
                {
                    ww *= w / (float)Math.Abs(Math.Cos(anglei * Math.PI / 180));
                }
                else
                {
                    ww *= h / (float)Math.Abs(Math.Sin(anglei * Math.PI / 180));
                }
                g.FillEllipse(Brushes.Black, -ww, -hh, 2 * ww, 2 * hh);
                g.RotateTransform(angle);
            }

            bmp = Masking(v, color, srcBitmap, bmp);
        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }

        return bmp;
    }
}
