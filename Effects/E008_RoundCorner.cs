using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;

namespace Com.Nakasendo.Gakupetit.Effects;

class E008_RoundCorner : EffectBase, IEffect
{
    public E008_RoundCorner(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 8;
    public string[] Names => new[] { "Rounded corners", "角丸" };
    public bool IsBackChecked => true;
    public int DefaultValue => 34;
    public string[] Descriptions => new[] {
        $"This is a rounded corner effect. You can change the size of the rounded corners with the slider.",
        $"角丸のエフェクトです。スライダーで角丸の大きさを変更できます。" };
    public Color GetDefaultColor(Color nowColor) => nowColor;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        // 0のときは元画像を返す
        if (v == 0) return srcBitmap;

        Bitmap bmp = new(srcBitmap);

        try
        {
            var w = bmp.Width;
            var h = bmp.Height;
            var d = w > h ? h * v / SliderMax : w * v / SliderMax;
            if (d == 0) d = 1;

            using var g = Graphics.FromImage(bmp);
            g.Clear(color);

            // 枠を作成
            using GraphicsPath gp = new(FillMode.Winding);
            gp.AddRectangle(new Rectangle(0, d / 2, w, h - d));
            gp.AddRectangle(new Rectangle(d / 2, 0, w - d, h));
            gp.AddPie(0, 0, d, d, 180, 90);
            gp.AddPie(w - d - 1, 0, d, d, 270, 90);
            gp.AddPie(w - d - 1, h - d - 1, d, d, 0, 90);
            gp.AddPie(0, h - d - 1, d, d, 90, 90);

            // 枠内を画像ブラシで塗りつぶし
            using TextureBrush tb = new(srcBitmap);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.FillPath(tb, gp);
        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }

        return bmp;
    }
}
