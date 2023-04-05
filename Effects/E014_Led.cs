using Com.Nakasendo.Gakupetit.EffectEtc;

namespace Com.Nakasendo.Gakupetit.Effects;

class E014_Led : EffectBase, IEffect
{
    public E014_Led(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 14;
    public string[] Names => new[] { "Date and time", "撮影日時" };
    public bool IsBackChecked => false;
    public int DefaultValue => 69;
    public string[] Descriptions => new[] {
        $"It's a date and time display effect. Use the slider to adjust display size and toggle time visibility (alt by 1).",
        $"日時表示を付加するエフェクトです。スライダーで表示サイズと時刻の有無(1ずつ交互)を変更できます。" };
    public Color GetDefaultColor(Color nowColor) => Color.Red;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        Bitmap bmp = new(srcBitmap);

        try
        {
            var size = (v == 100) ? 100 : (100 - 10) * (v / 10) / 10 + 10;
            var alpha = (v == 100) ? 255 : (v % 5 + 1) * 51;
            var withTime = v != 100 && (v % 10 < 5);

            using var g = Graphics.FromImage(bmp);
            DrawLed.DrawDateTime(g, bmp.Width, bmp.Height, size, color, alpha, withTime, BitmapEffects.ShotDateTime);
        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }
        return bmp;
    }
}
