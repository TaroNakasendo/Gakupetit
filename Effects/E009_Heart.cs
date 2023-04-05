using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;
using static System.Math;

namespace Com.Nakasendo.Gakupetit.Effects;

class E009_Heart : EffectBase, IEffect
{
    public E009_Heart(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 9;
    public string[] Names => new[] { "Heart", "ハート" };
    public bool IsBackChecked => true;
    public int DefaultValue => 20;
    public string[] Descriptions => new[] {
        $"It's a heart-shaped center cutout effect. Adjust the heart blur intensity with the slider.",
        $"中央をハート形に切り抜くエフェクトです。スライダーでハートのぼかし具合を変更できます。" };
    public Color GetDefaultColor(Color nowColor) => Color.FromArgb(255, 245, 240);

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        var w = srcBitmap.Width;
        var h = srcBitmap.Height;
        var isWide = (w > h);
        var heartW = Min(h, w); // ハート幅・高さ
        var heartL = isWide ? (w - h) / 2 : 0; // ハート左座標
        var heartT = isWide ? 0 : (h - w) / 2; // ハート上座標


        Bitmap bmp = new(srcBitmap);
        try
        {
            using var g = Graphics.FromImage(bmp);

            g.Clear(Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var max = w > h ? h : w;
            var scale = (max - 4 * (v * max / 10 / 100 + 1)) / (float)max;
            g.TranslateTransform(-w * scale / 2 + w / 2, -h * scale / 2 + h / 2);
            g.ScaleTransform(scale, scale);

            using GraphicsPath gp = new();
            gp.AddBeziers( // 右半分
                new Point(w / 2, heartW * 205 / 1024 + heartT), // 中央上の点
                new Point(heartL + heartW * 667 / 1024, -heartW * 19 / 1024 + heartT), // 中央上からのコントロールポイント
                new Point(heartL + heartW - 1, heartW * 42 / 1024 + heartT), // 右中央からのコントロールポイント
                new Point(heartL + heartW - 1, heartW * 333 / 1024 + heartT), // 右中央の点
                new Point(heartL + heartW - 1, heartW * 595 / 1024 + heartT), // 右中央からのコントロールポイント2
                new Point(heartL + heartW * 835 / 1024, heartW * 730 / 1024 + heartT), // 中央下からのコントロールポイント
                new Point(w / 2, heartW * 996 / 1024 + heartT)); // 中央下の点　
            gp.AddBeziers(
                new Point(w / 2, heartW * 205 / 1024 + heartT),
                new Point(heartL + heartW * 356 / 1024, -heartW * 19 / 1024 + heartT),
                new Point(heartL, heartW * 42 / 1024 + heartT),
                new Point(heartL, heartW * 333 / 1024 + heartT),
                new Point(heartL, heartW * 595 / 1024 + heartT),
                new Point(heartL + heartW * 188 / 1024, heartW * 730 / 1024 + heartT),
                new Point(w / 2, heartW * 996 / 1024 + heartT));

            g.FillPath(Brushes.Black, gp);

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
