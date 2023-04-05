using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Com.Nakasendo.Gakupetit.Effects;

class E004_RandomDot : EffectBase, IEffect
{
    public E004_RandomDot(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 4;
    public string[] Names => new[] { "Random dots", "ランダムドット" };
    public bool IsBackChecked => true;
    public int DefaultValue => 20;
    public string[] Descriptions => new[] {
        $"Blur the edges with random dots of a single color.  Use the slider to change the area.",
        $"ふちをランダムな単一色のドットでぼかします。スライダーで範囲を変更できます。" };
    public Color GetDefaultColor(Color nowColor) => nowColor;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        var bmp = MakeMask(v, srcBitmap);
        return Masking(v, color, srcBitmap, bmp);
    }

    /// <summary>
    /// ランダムドット用
    /// </summary>
    /// <param name="color">背景色</param>
    /// <returns>ビットマップ</returns>
    protected override Bitmap Masking(int v, Color color, Bitmap srcBitmap, Bitmap maskBitmap)
    {
        Random rnd = new(1000);
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
                var r = inRgbValues[j + 2];
                if (r == 0) continue;
                if (rnd.Next(0, 255) < r) SetPixel(outRgbValues, j, color);
            }

            // byte列をbitmapに復元し、メモリのロックを開放する
            Marshal.Copy(outRgbValues, 0, outPtr, size);
            maskBitmap.UnlockBits(inBmpData);
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
