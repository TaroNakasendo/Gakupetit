using Com.Nakasendo.Gakupetit.EffectEtc;

namespace Com.Nakasendo.Gakupetit.Effects;

class E007_Border : EffectBase, IEffect
{
    public E007_Border(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 7;
    public string[] Names => new[] { "Stripe", "ボーダー" };
    public bool IsBackChecked => true;
    public int DefaultValue => 37;
    public string[] Descriptions => new[] {
        $"It adds vintage TV scanlines. Adjust intensity (10 levels) and stripe width (in 10s) with the slider.",
        $"昔のアナログテレビのような走査線を付加します。スライダーで濃さ(10段階)と縞々の幅(10ごと)を変更できます。" };
    public Color GetDefaultColor(Color nowColor) => nowColor;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        // 0のときは元画像を返す
        if (v == 0) return srcBitmap;

        Bitmap bmp = new(srcBitmap);
        try
        {
            using var g = Graphics.FromImage(bmp);
            var newV = 255 * (((v - 1) % 10) + 1) * 10 / SliderMax;
            var interval = v / 10 + 1;
            using Pen p = new(Color.FromArgb(newV, color), interval);

            // interval行おきに色をつける
            for (int j = interval * 3 / 2; j < bmp.Height; j += interval * 2)
            {
                g.DrawLine(p, 0, j, bmp.Width, j);
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
