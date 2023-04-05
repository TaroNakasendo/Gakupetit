using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using static System.Math;

namespace Com.Nakasendo.Gakupetit.EffectEtc;

abstract class EffectBase
{
    public EffectBase(BitmapEffects bitmapEffects)
    {
        BitmapEffects = bitmapEffects;
        for (var i = 0; i < 1024; i++)
        {
            SinTable[i] = (int)(255 * Sin(i * PI / (2 * 1024)) + 0.5);
        }
    }

    protected int[] SinTable { get; } = new int[1024]; // ガウスぼかし用テーブル

    protected BitmapEffects BitmapEffects { get; private set; }

    static protected int SliderMax => 100;

    static protected void SetPixel(byte[] ptr, int offset, Color color)
    {
        ptr[offset] = color.B;
        ptr[++offset] = color.G;
        ptr[++offset] = color.R;
    }

    /// <summary>
    /// ぼかし枠用マスク適用処理
    /// </summary>
    /// <param name="v"></param>
    /// <param name="color">背景色</param>
    /// <param name="srcBitmap"></param>
    /// <param name="maskBitmap"></param>
    /// <returns></returns>
    protected virtual Bitmap Masking(int v, Color color, Bitmap srcBitmap, Bitmap maskBitmap)
    {
        Bitmap bmp = new(srcBitmap);

        try
        {
            // bitmapをメモリ上にロックします
            Rectangle rect = new(0, 0, bmp.Width, bmp.Height);
            var inBmpData = maskBitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var outBmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            // RGB値をbyte列にコピーする
            var inPtr = inBmpData.Scan0;
            var outPtr = outBmpData.Scan0;
            var stride = outBmpData.Stride;
            var size = stride * bmp.Height;
            var inRgbValues = new byte[size];
            var outRgbValues = new byte[size];

            Marshal.Copy(inPtr, inRgbValues, 0, size);
            Marshal.Copy(outPtr, outRgbValues, 0, size);

            //4byteずつ進む
            for (var j = 0; j < size; j += 4)
            {
                var b = inRgbValues[j + 0];
                var g = inRgbValues[j + 1];
                var r = inRgbValues[j + 2];

                if (b != 0) outRgbValues[j + 0] += (byte)((color.B - outRgbValues[j + 0]) * b / 255);
                if (g != 0) outRgbValues[j + 1] += (byte)((color.G - outRgbValues[j + 1]) * g / 255);
                if (r != 0) outRgbValues[j + 2] += (byte)((color.R - outRgbValues[j + 2]) * r / 255);
            }

            // byte列をbitmapに復元し、メモリのロックを開放する
            Marshal.Copy(outRgbValues, 0, outPtr, size);
            bmp.UnlockBits(outBmpData);
            maskBitmap.UnlockBits(inBmpData);
        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }

        return bmp;
    }

    /// <summary>
    /// フレームマスク(リニアぼかし)
    /// </summary>
    /// <param name="v">半径</param>
    /// <param name="srcBitmap"></param>
    /// <returns></returns>                    
    internal static Bitmap MakeMask(int v, Bitmap srcBitmap)
    {
        var w = srcBitmap.Width;
        var h = srcBitmap.Height;

        Bitmap bmp = new(srcBitmap);

        try
        {
            using var g = Graphics.FromImage(bmp);

            g.Clear(Color.Black);


            // bitmapをメモリ上にロックします
            Rectangle rect = new(0, 0, bmp.Width, bmp.Height);
            var outBmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            // RGB値をbyte列にコピーする
            var outPtr = outBmpData.Scan0;
            var stride = outBmpData.Stride;
            var size = stride * bmp.Height;
            var outRgbValues = new byte[size];

            Marshal.Copy(outPtr, outRgbValues, 0, size);

            int gray;
            double ml, mt, mr, mb, gg;

            double margin = w < h ? w : h;

            margin *= (double)v / SliderMax / 2;

            for (var j = 0; j < h; j++)
            {
                for (var i = 0; i < w; i++)
                {
                    gg = 0;
                    ml = margin - i;
                    mt = margin - j;
                    mr = margin + i - w;
                    mb = margin + j - h;

                    if (mt <= 0 && mb <= 0 && ml <= 0 && mr <= 0) continue;

                    if (mt > 0) // 上
                    {
                        if (ml > 0) // 左上
                        {
                            gg = Sqrt(ml * ml + mt * mt);
                        }
                        else if (mr > 0) // 右上
                        {
                            gg = Sqrt(mr * mr + mt * mt);
                        }
                        else // 上
                        {
                            gg = mt;
                        }
                    }
                    else if (mb > 0) // 下
                    {
                        if (ml > 0) // 左下
                        {
                            gg = Sqrt(ml * ml + mb * mb);
                        }
                        else if (mr > 0) // 右下
                        {
                            gg = Sqrt(mr * mr + mb * mb);
                        }
                        else // 下
                        {
                            gg = mb;
                        }
                    }
                    else // 中央
                    {
                        if (ml > 0) //左
                        {
                            gg = ml;
                        }
                        else if (mr > 0) //右
                        {
                            gg = mr;
                        }
                    }

                    gray = (int)((2 * gg - margin) * 255 / margin + 0.5);
                    if (gray >= 256) gray = 255;
                    if (gray < 0) gray = 0;

                    SetPixel(outRgbValues, i * 4 + j * stride, Color.FromArgb(gray, gray, gray));
                }
            }

            // byte列をbitmapに復元し、メモリのロックを開放する
            Marshal.Copy(outRgbValues, 0, outPtr, size);
            bmp.UnlockBits(outBmpData);
        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }

        return bmp;
    }
}
