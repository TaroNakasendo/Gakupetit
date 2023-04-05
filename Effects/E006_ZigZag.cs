using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Com.Nakasendo.Gakupetit.Effects;

class E006_ZigZag : EffectBase, IEffect
{
    public E006_ZigZag(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 6;
    public string[] Names => new[] { "Zigzag", "ジグザグ" };
    public bool IsBackChecked => true;
    public int DefaultValue => 20;
    public string[] Descriptions => new[] {
        $"It's a zigzag frame effect. Adjust frame size with the slider.",
        $"枠をジグザグに切り抜くエフェクトです。スライダーで枠の大きさを変更できます。" };
    public Color GetDefaultColor(Color nowColor) => nowColor;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        Bitmap bmp = new(srcBitmap);

        try
        {
            // bitmapをメモリ上にロックします
            Rectangle rect = new(0, 0, bmp.Width, bmp.Height);
            var outBmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            // RGB値をbyte列にコピーする
            var outPtr = outBmpData.Scan0;
            var stride = outBmpData.Stride;
            var size = stride * bmp.Height;
            var outRgbValues = new byte[size];

            Marshal.Copy(outPtr, outRgbValues, 0, size);

            // 1～Width/8まで可変
            var a1 = v * (bmp.Width - 8) / (SliderMax * 8) + 1;
            var a2 = a1 * 2;

            for (var i = 0; i < bmp.Width; i++)
            {
                var mod = i % a2 - a1;
                for (var j = 0; j < a1; j++) // 上
                {
                    if ((mod > 0) && (mod > j) || (mod <= 0) && (mod < -j))
                    {
                        SetPixel(outRgbValues, j * stride + i * 4, color);
                    }
                }
                for (var j = bmp.Height - a1; j < bmp.Height; j++) // 下
                {
                    int top = bmp.Height - 1 - j;
                    if ((mod > 0) && (mod > top) || (mod <= 0) && (mod < -top))
                    {
                        SetPixel(outRgbValues, j * stride + i * 4, color);
                    }
                }
            }
            for (var j = 0; j < bmp.Height; j++)
            {
                var mod = j % a2 - a1;
                for (var i = 0; i < a1; i++) // 左
                {
                    if ((mod > 0) && (mod > i) || (mod <= 0) && (mod < -i))
                    {
                        SetPixel(outRgbValues, j * stride + i * 4, color);
                    }
                }
                for (var i = bmp.Width - a1; i < bmp.Width; i++) // 右
                {
                    var left = bmp.Width - 1 - i;
                    if ((mod > 0) && (mod > left) || (mod <= 0) && (mod < -left))
                    {
                        SetPixel(outRgbValues, j * stride + i * 4, color);
                    }
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
