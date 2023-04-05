using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using static System.Math;

namespace Com.Nakasendo.Gakupetit.Effects;

class E003_Circle : EffectBase, IEffect
{
    public E003_Circle(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 3;
    public string[] Names => new[] { "Circle", "まんまる" };
    public bool IsBackChecked => true;
    public int DefaultValue => 34;
    public string[] Descriptions => new[] {
        $"It crops a circular center. Adjust blur with the slider.",
        $"画像中央を正円に切り抜きます。スライダーでぼけ具合を変更できます。" };
    public Color GetDefaultColor(Color nowColor) => nowColor;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        int w = srcBitmap.Width;
        int h = srcBitmap.Height;

        Bitmap bmp = new(srcBitmap);

        try
        {
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            // bitmapをメモリ上にロックします
            Rectangle rect = new(0, 0, w, h);
            var outBmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            // RGB値をbyte列にコピーする
            var outPtr = outBmpData.Scan0;
            var stride = outBmpData.Stride;
            var size = stride * h;
            var outRgbValues = new byte[size];

            Marshal.Copy(outPtr, outRgbValues, 0, size);

            int gray;
            double mh, mv, mlength;
            var mround = (w > h) ? h / 2.0 : w / 2.0;
            var threshold = (v == 0) ? 0.99 : 1 - (double)v / SliderMax;

            for (var j = 0; j < h; j++)
            {
                mv = Abs(h / 2.0 - j);
                if (mv > mround) continue;

                for (int i = 0; i < w; i++)
                {
                    mh = Abs(w / 2.0 - i);
                    if (mh > mround) continue;

                    // 円の外側
                    mlength = Sqrt(mh * mh + mv * mv);
                    if (mlength > mround) continue;

                    if (mlength < mround * threshold)
                    {
                        // 円の中
                        SetPixel(outRgbValues, j * stride + i * 4, Color.Black);
                    }
                    else
                    {
                        // 円の中間
                        int y = (int)(((mlength - mround * threshold) * 1024 / (mround * (1 - threshold))) + 0.5);
                        if (y >= 1024) y = 1023;
                        if (y < 0) y = 0;
                        gray = SinTable[y];
                        SetPixel(outRgbValues, j * stride + i * 4, Color.FromArgb(gray, gray, gray));
                    }
                }
            }

            // byte列をbitmapに復元し、メモリのロックを開放する
            Marshal.Copy(outRgbValues, 0, outPtr, size);
            bmp.UnlockBits(outBmpData);
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
