using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using static System.Math;

namespace Com.Nakasendo.Gakupetit.Effects;

class E001_Transparent : EffectBase, IEffect
{
    public E001_Transparent(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 1;
    public string[] Names => new[] { "Transparent", "透明" };
    public bool IsBackChecked => true;
    public int DefaultValue => 70;
    public string[] Descriptions => new[] {
        $"It's a transparent blur effect. Adjust blur with the slider. Output must be in PNG.",
        $"透明ぼかしをつけるエフェクトです。スライダーで枠のボケ具合を変更できます。出力はPNGにする必要があります。" };
    public Color GetDefaultColor(Color nowColor) => Color.Transparent;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        int w = srcBitmap.Width;
        int h = srcBitmap.Height;

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

            double margin = (w < h) ? w : h;

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

                    SetPixel(outRgbValues, i * 4 + j * stride, Color.FromArgb(gray, gray, gray, gray));
                }
            }

            // byte列をbitmapに復元し、メモリのロックを開放する
            Marshal.Copy(outRgbValues, 0, outPtr, size);
            bmp.UnlockBits(outBmpData);
            bmp = AlphaMasking(srcBitmap, bmp);

        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }

        return bmp;
    }

    /// <summary>
    /// アルファチャンネルマスク適用処理
    /// </summary>
    /// <param name="color">背景色</param>
    /// <param name="srcBitmap"></param>
    /// <param name="maskBitmap"></param>
    /// <returns></returns>
    private static Bitmap AlphaMasking(Bitmap srcBitmap, Bitmap maskBitmap)
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
                outRgbValues[j + 3] = (byte)(255 - inRgbValues[j + 0]);
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
}
