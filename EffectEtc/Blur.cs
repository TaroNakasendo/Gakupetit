using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Com.Nakasendo.Gakupetit.EffectEtc;

static class Blur
{
    static public Bitmap BlurMask(Bitmap mask, int w, int h, int v, bool useFade = false)
    {
        if (w < 4 || h < 4) return mask;

        var max = w > h ? h : w;
        v = v * max / 10 / 100 + 1;

        Bitmap bmp = new(mask);
        try
        {
            using Bitmap bmp2 = new(w, h);
            using var g = Graphics.FromImage(bmp2);
            g.Clear(Color.White);

            // bitmapをメモリ上にロックします
            Rectangle rect = new(0, 0, w, h);
            var inBmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            var outBmpData = bmp2.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            // RGB値をbyte列にコピーする
            var inPtr = inBmpData.Scan0;
            var outPtr = outBmpData.Scan0;
            var stride = outBmpData.Stride;
            var total = stride * h;
            var inRgbValues = new byte[total];
            var outRgbValues = new byte[total];

            Marshal.Copy(inPtr, inRgbValues, 0, total);
            Marshal.Copy(outPtr, outRgbValues, 0, total);

            var s = 2 * v + 1;

            // 横ぼかし
            Parallel.For(0, h, j =>
            {
                var js = j * stride;
                int rr = 0, gg = 0, bb = 0;
                // 左端
                for (var k = 0; k < s; k++)
                {
                    bb += inRgbValues[4 * k + 0 + js];
                    gg += inRgbValues[4 * k + 1 + js];
                    rr += inRgbValues[4 * k + 2 + js];
                }
                for (var k = 0; k <= v; k++)
                {
                    var f = useFade ? (double)k / v / s : 1.0 / s;
                    outRgbValues[k * 4 + 0 + js] = (byte)(bb * f + 0.5);
                    outRgbValues[k * 4 + 1 + js] = (byte)(gg * f + 0.5);
                    outRgbValues[k * 4 + 2 + js] = (byte)(rr * f + 0.5);
                }
                for (var i = v + 1; i < w - v; i++)
                {
                    var k1 = (i - (v + 1)) * 4;
                    bb -= inRgbValues[k1 + 0 + js];
                    gg -= inRgbValues[k1 + 1 + js];
                    rr -= inRgbValues[k1 + 2 + js];
                    var k2 = (i + v) * 4;
                    bb += inRgbValues[k2 + 0 + js];
                    gg += inRgbValues[k2 + 1 + js];
                    rr += inRgbValues[k2 + 2 + js];
                    outRgbValues[i * 4 + 0 + js] = (byte)((double)bb / s + 0.5);
                    outRgbValues[i * 4 + 1 + js] = (byte)((double)gg / s + 0.5);
                    outRgbValues[i * 4 + 2 + js] = (byte)((double)rr / s + 0.5);
                }
                // 右端
                bb = gg = rr = 0;
                for (var k = w - s; k < w; k++)
                {
                    bb += inRgbValues[4 * k + 0 + js];
                    gg += inRgbValues[4 * k + 1 + js];
                    rr += inRgbValues[4 * k + 2 + js];
                }
                for (var k = w - v - 1; k < w; k++)
                {
                    var f = useFade ? (double)(w - 1 - k) / v / s : 1.0 / s;
                    outRgbValues[k * 4 + 0 + js] = (byte)(bb * f + 0.5);
                    outRgbValues[k * 4 + 1 + js] = (byte)(gg * f + 0.5);
                    outRgbValues[k * 4 + 2 + js] = (byte)(rr * f + 0.5);
                }
            });
            // 縦ぼかし
            Parallel.For(0, w, j =>
            {
                var js = j * 4;
                int rr = 0, gg = 0, bb = 0;
                // 上端
                for (var k = 0; k < s; k++)
                {
                    bb += outRgbValues[k * stride + 0 + js];
                    gg += outRgbValues[k * stride + 1 + js];
                    rr += outRgbValues[k * stride + 2 + js];
                }
                for (var k = 0; k <= v; k++)
                {
                    var f = useFade ? (double)k / v / s : 1.0 / s;
                    inRgbValues[k * stride + 0 + js] = (byte)(bb * f + 0.5);
                    inRgbValues[k * stride + 1 + js] = (byte)(gg * f + 0.5);
                    inRgbValues[k * stride + 2 + js] = (byte)(rr * f + 0.5);
                }
                for (var i = v + 1; i < h - v; i++)
                {

                    var k1 = (i - (v + 1)) * stride;
                    bb -= outRgbValues[k1 + 0 + js];
                    gg -= outRgbValues[k1 + 1 + js];
                    rr -= outRgbValues[k1 + 2 + js];
                    var k2 = (i + v) * stride;
                    bb += outRgbValues[k2 + 0 + js];
                    gg += outRgbValues[k2 + 1 + js];
                    rr += outRgbValues[k2 + 2 + js];
                    inRgbValues[i * stride + 0 + js] = (byte)((double)bb / s + 0.5);
                    inRgbValues[i * stride + 1 + js] = (byte)((double)gg / s + 0.5);
                    inRgbValues[i * stride + 2 + js] = (byte)((double)rr / s + 0.5);
                }
                // 下端
                for (var k = h - v - 1; k < h; k++)
                {
                    var f = useFade ? (double)(h - 1 - k) / v / s : 1.0 / s;
                    outRgbValues[k * stride + 0 + js] = (byte)(bb * f + 0.5);
                    outRgbValues[k * stride + 1 + js] = (byte)(gg * f + 0.5);
                    outRgbValues[k * stride + 2 + js] = (byte)(rr * f + 0.5);
                }
            });
            Marshal.Copy(inRgbValues, 0, inPtr, total);
            bmp.UnlockBits(inBmpData);
            bmp2.UnlockBits(outBmpData);
        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }

        return bmp;
    }
}
