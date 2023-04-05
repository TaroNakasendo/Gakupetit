using Com.Nakasendo.Gakupetit.EffectEtc;

namespace Com.Nakasendo.Gakupetit.Effects;

class E000_Fade : EffectBase, IEffect
{
    public E000_Fade(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 0;
    public string[] Names => new[] { "Fade", "フェード" };
    public bool IsBackChecked => true;
    public int DefaultValue => 0;
    public string[] Descriptions => new[] {
        $"Default effect. Adjust background color transparency with the slider.",
        $"既定のエフェクトです。スライダーで背景色への透過度を変更できます。" };
    public Color GetDefaultColor(Color nowColor) => nowColor;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        // 0のときは元画像を返す
        if (v == 0) return srcBitmap;

        Bitmap bmp = new(srcBitmap);
        try
        {
            using var g = Graphics.FromImage(bmp);
            using SolidBrush sb = new(Color.FromArgb(255 * v / SliderMax, color));
            g.FillRectangle(sb, new Rectangle(Point.Empty, bmp.Size));
        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }
        return bmp;
    }
}
