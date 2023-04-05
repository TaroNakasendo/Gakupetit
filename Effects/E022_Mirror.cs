using Com.Nakasendo.Gakupetit.EffectEtc;

namespace Com.Nakasendo.Gakupetit.Effects;

class E022_Mirror : EffectBase, IEffect
{
    public E022_Mirror(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 22;
    public string[] Names => new[] { "Mirror", "鏡" };
    public bool IsBackChecked => true;
    public int DefaultValue => 6;
    public string[] Descriptions => new[] {
        $"It's a water reflection effect. Use the slider to adjust water level and reflection intensity (2 types).",
        $"水面に鏡面反射するようなエフェクトです。スライダーで水面の位置と反射度合(2種類)を変更できます。" };
    public Color GetDefaultColor(Color nowColor) => nowColor;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        Bitmap bmp = new(srcBitmap);
        Bitmap revBmp = new(srcBitmap);
        revBmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

        try
        {
            var g = Graphics.FromImage(bmp);
            var w = srcBitmap.Width;
            var h = srcBitmap.Height;
            var v2 = 50 + v / 2;
            var h2 = h * v2 / 100;

            Rectangle destRect = new(0, h2, w, h2);
            g.DrawImage(revBmp, destRect, 0, h - h2, w, h2, GraphicsUnit.Pixel);

            if (v % 2 == 0)
            {
                // 暗くするバージョン
                SolidBrush waterBrush = new(Color.FromArgb(100, 0, 10, 30));
                g.FillRectangle(waterBrush, 0, h2, w, h - h2);
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
