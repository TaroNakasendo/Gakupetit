using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using static System.Math;

namespace Com.Nakasendo.Gakupetit.Effects;

class E002_Gauss : EffectBase, IEffect
{
    public E002_Gauss(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 2;
    public string[] Names => new[] { "Blur", "ガウスぼかし" };
    public bool IsBackChecked => true;
    public int DefaultValue => 40;
    public string[] Descriptions => new[] {
        $"It's a Gaussian blur edge effect. Adjust the blur intensity with the slider.",
        $"ふちにガウスぼかしをつけるエフェクトです。スライダーでぼかし具合を変更できます。" };
    public Color GetDefaultColor(Color nowColor) => nowColor;

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
            var outBmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly,
                                                                  PixelFormat.Format32bppArgb);

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
                        else if (mr < 0) // 上
                        {
                            gg = mt;
                        }
                        else // 右上
                        {
                            gg = Sqrt(mr * mr + mt * mt);
                        }
                    }
                    else if (mb > 0)
                    {
                        if (ml > 0) // 左下
                        {
                            gg = Sqrt(ml * ml + mb * mb);
                        }
                        else if (mr < 0) // 下
                        {
                            gg = mb;
                        }
                        else // 右下
                        {
                            gg = Sqrt(mr * mr + mb * mb);
                        }
                    }
                    else // 中央
                    {
                        if (ml > 0) // 左
                        {
                            gg = ml;
                        }
                        else if (mr > 0) // 右
                        {
                            gg = mr;
                        }
                    }

                    var y = (int)((2 * gg - margin) * 1024 / margin + 0.5);
                    if (y >= 1024) y = 1023;
                    if (y < 0) y = 0;
                    gray = SinTable[y];

                    SetPixel(outRgbValues, i * 4 + j * stride, Color.FromArgb(gray, gray, gray));
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
