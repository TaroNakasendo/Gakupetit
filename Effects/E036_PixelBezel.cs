using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Com.Nakasendo.Gakupetit.Effects;

class E036_PixelBezel(BitmapEffects bitmapEffects) : EffectBase(bitmapEffects), IEffect
{
    public int EffectId => 36;
    public string[] Names => ["Pixel Bezel", "ピクセル枠"];
    public bool IsBackChecked => true;
    public int DefaultValue => 20;
    public string[] Descriptions => [
        "Creates a chunky pixelated bezel around the image. Adjust the bezel size with the slider.",
        "画像の周囲に太めのピクセル風枠を作ります。スライダーで枠のサイズを調整します。"
    ];
    public Color GetDefaultColor(Color _) => Color.White;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        if (srcBitmap == null) throw new ArgumentNullException(nameof(srcBitmap));
        if (v == 0) return new(srcBitmap);

        var w = srcBitmap.Width;
        var h = srcBitmap.Height;

        Bitmap bmp = new(srcBitmap);
        try
        {
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.None;

            // bezel width as fraction of the shorter side
            var min = Math.Min(w, h);
            var bezel = Math.Max(1, min * v / 200);
            var block = Math.Max(2, bezel / 8);

            // 高速化: LockBitsでピクセルデータを直接取得
            BitmapData srcData = srcBitmap.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, srcBitmap.PixelFormat);
            try
            {
                unsafe
                {
                    byte* srcPtr = (byte*)srcData.Scan0;
                    int bpp = Image.GetPixelFormatSize(srcBitmap.PixelFormat) / 8;
                    int stride = srcData.Stride;

                    for (int y = 0; y < h; y += block)
                    {
                        for (int x = 0; x < w; x += block)
                        {
                            bool inBorder = x < bezel || y < bezel || x + block > w - bezel || y + block > h - bezel;
                            if (!inBorder) continue;

                            var cx = Math.Min(w - 1, x + block / 2);
                            var cy = Math.Min(h - 1, y + block / 2);
                            Color sample;

                            // バッファオーバーラン防止: ポインタアクセス前に範囲チェック
                            if (cx >= 0 && cx < w && cy >= 0 && cy < h)
                            {
                                byte* p = srcPtr + cy * stride + cx * bpp;
                                if (bpp == 4)
                                    sample = Color.FromArgb(p[3], p[2], p[1], p[0]);
                                else if (bpp == 3)
                                    sample = Color.FromArgb(p[2], p[1], p[0]);
                                else
                                    sample = color;
                            }
                            else
                            {
                                sample = color;
                            }
                            using var sb = new SolidBrush(sample);
                            g.FillRectangle(sb, x, y, block, block);
                        }
                    }
                }
            }
            finally
            {
                srcBitmap.UnlockBits(srcData);
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
