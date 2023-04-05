using Com.Nakasendo.Gakupetit.EffectEtc;
using static System.Math;

namespace Com.Nakasendo.Gakupetit.Effects;

class E016_UnderConstruction : EffectBase, IEffect
{
    public E016_UnderConstruction(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 16;
    public string[] Names => new[] { "Under construction", "工事中" };
    public bool IsBackChecked => false;
    public int DefaultValue => 40;
    public string[] Descriptions => new[] {
        $"It's a construction frame effect. Adjust frame size with the slider.",
        $"工事中の枠をつけるエフェクトです。スライダーで枠の大きさを変更できます。" };
    public Color GetDefaultColor(Color nowColor) => Color.Gold;
    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        Bitmap bmp = new(srcBitmap);

        try
        {
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);

            var maxSize = Max(bmp.Width, bmp.Height);
            var pitch = maxSize * (v * 0.99f + 1) / 1000;

            using Pen pen = new(color, pitch);
            var hPitch = (float)Sqrt(2) * pitch;
            // 左下から右上へ黄色で描画するのを、左端から右端まで繰り返す
            for (var i = -(float)maxSize + hPitch; i < maxSize; i += 2 * hPitch)
            {
                g.DrawLine(pen, i, maxSize, i + maxSize + pitch, -pitch);
            }
            var margin = (v + 1) * maxSize / 8f / 100;
            RectangleF marginRect = new(margin, margin, bmp.Width - 2 * margin - 1, bmp.Height - 2 * margin - 1);
            // クリップ領域を設定してから、画像を描画する
            g.SetClip(marginRect);
            g.DrawImage(srcBitmap, 0, 0);
        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }

        return bmp;
    }
}
