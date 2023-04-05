using Com.Nakasendo.Gakupetit.EffectEtc;

namespace Com.Nakasendo.Gakupetit.Effects;

class E023_Checker : EffectBase, IEffect
{
    public E023_Checker(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 23;
    public string[] Names => new[] { "Checker", "チェッカー" };
    public bool IsBackChecked => true;
    public int DefaultValue => 8;
    public string[] Descriptions => new[] {
        $"It adds checkered frames. Adjust size and type (even: inner/outer, odd: outer only) with the slider.",
        $"チェック模様の枠を付加します。スライダーで大きさ、タイプ(偶数:内外、奇数:外側のみ)を変更できます。" };
    public Color GetDefaultColor(Color nowColor) => nowColor;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        Bitmap bmp = new(srcBitmap);

        try
        {
            var w = bmp.Width;
            var h = bmp.Height;

            var num = 5 + v; // 5～105まで
            var d = h / (2 * num - 1.0f);

            using var g = Graphics.FromImage(bmp);

            using SolidBrush sb = new(color);
            // 左右
            for (var i = 0; i <= num; i++)
            {
                var t = i * 2 * d;
                // 左
                g.FillRectangle(sb, 0, t, d, d);
                // 右
                //if (d <= t && t < h - d) 
                g.FillRectangle(sb, w - d, t, d, d);

                // 内側
                if (v % 2 == 0)
                {
                    // 左
                    g.FillRectangle(sb, d, t + d, d, d);
                    // 右
                    if (d <= t && t < h - 2 * d) g.FillRectangle(sb, w - 2 * d, t - d, d, d);
                }
            }

            // 上下
            for (var i = 0; i <= num * w / h; i++)
            {
                var t = i * 2 * d;
                // 上
                g.FillRectangle(sb, t, 0, d, d);
                // 下
                g.FillRectangle(sb, t, h - d, d, d);

                // 内側
                if (v % 2 == 0)
                {
                    if (d <= t && t < w - 2 * d)
                    {
                        // 上
                        g.FillRectangle(sb, t + d, d, d, d);
                        // 下
                        g.FillRectangle(sb, t + d, h - 2 * d, d, d);
                    }
                }
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
