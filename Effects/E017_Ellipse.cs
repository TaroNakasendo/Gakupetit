using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;

namespace Com.Nakasendo.Gakupetit.Effects;

class E017_Ellipse : EffectBase, IEffect
{
    public E017_Ellipse(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 17;
    public string[] Names => new[] { "Oval", "楕円" };
    public bool IsBackChecked => true;
    public int DefaultValue => 50;
    public string[] Descriptions => new[] {
        $"It's an oval cropping effect. Adjust edge blur with the slider.",
        $"楕円に切りぬくエフェクトです。スライダーで楕円のふちのぼかし具合を変更できます。" };
    public Color GetDefaultColor(Color nowColor) => nowColor;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        var w = srcBitmap.Width;
        var h = srcBitmap.Height;

        Bitmap bmp = new(srcBitmap);

        try
        {
            var max = w > h ? h : w;
            var scale = 2 * v * max / 10 / 100 + 1;

            Rectangle rect = new(scale, scale, w - 2 * scale, h - 2 * scale);

            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using SolidBrush sb = new(Color.Black);
            g.FillEllipse(sb, rect);

            // ガウスぼかしとする
            bmp = Blur.BlurMask(bmp, w, h, v);
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
