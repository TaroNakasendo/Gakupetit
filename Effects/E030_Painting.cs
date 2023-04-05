using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Com.Nakasendo.Gakupetit.Effects;

class E030_Painting : EffectBase, IEffect
{
    public E030_Painting(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId { get; set; } = 30;
    public string[] Names => new[] { $"Green back {PaintingId}", $"絵画 {PaintingId}" };
    public bool IsBackChecked => false;
    public int DefaultValue { get; set; } = 0;
    public string[] Descriptions => new[] {
        $"This is effect No. {PaintingId}, which looks like a picture frame. You can change the color range of transparency with the slider.",
        $"絵画の額縁のようなエフェクトその{PaintingId}です。スライダーで透過の色範囲を変更できます。" };
    public Color GetDefaultColor(Color nowColor) => Color.FromArgb(255, 255, 255);
    public string ImagesFolder { get; set; } = @".\Images";

    private int PaintingId => EffectId - 30 + 1;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        var w = srcBitmap.Width;
        var h = srcBitmap.Height;


        Bitmap bmp = new(srcBitmap);
        try
        {
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            // 境界色が残るのを防止
            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            var maskFile = $@"{ImagesFolder}\Painting{PaintingId}.png";
            if (!File.Exists(maskFile))
            {
                maskFile = $@".\Images\Painting{PaintingId}.png";
            }
            if (File.Exists(maskFile))
            {
                using var imageMask = Image.FromFile(maskFile);
                g.DrawImage(imageMask, 0, 0, w, h);
            }
            else
            {
                g.Clear(Color.White);
                using Pen p = new(Color.Red, 10);
                g.DrawLine(p, 0, 0, w, h);
                g.DrawLine(p, 0, h, w, 0);
                using Font font = new("Tahoma", 14);
                g.DrawString($"Image not found.", font, Brushes.Red, 0, 0);
            }

            bmp = Masking(v, color, srcBitmap, bmp);
        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }

        return bmp;
    }


    /// <summary>
    /// Painting用
    /// </summary>
    /// <param name="color">透過させる色</param>
    /// <returns>ビットマップ</returns>
    protected override Bitmap Masking(int v, Color color, Bitmap srcBitmap, Bitmap maskBitmap)
    {
        Bitmap bmp = new(srcBitmap);

        try
        {
            // bitmapをメモリ上にロックします
            var w = bmp.Width;
            var h = bmp.Height;
            Rectangle rect = new(0, 0, w, h);
            var inBmpData = maskBitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var outBmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            // RGB値をbyte列にコピーする
            var inPtr = inBmpData.Scan0;
            var outPtr = outBmpData.Scan0;
            var stride = outBmpData.Stride;
            var size = stride * h;
            var inRgbValues = new byte[size];
            var outRgbValues = new byte[size];

            Marshal.Copy(inPtr, inRgbValues, 0, size);
            Marshal.Copy(outPtr, outRgbValues, 0, size);

            var v2 = v * v * 255 / 10000;
            //4byteずつ進む
            Parallel.For(0, h, j =>
            {
                for (int i = 0; i < stride; i += 4)
                {
                    var b = inRgbValues[i + j * stride + 0];
                    var g = inRgbValues[i + j * stride + 1];
                    var r = inRgbValues[i + j * stride + 2];
                    var a = inRgbValues[i + j * stride + 3];
                    if (color.B - v2 <= b && b <= color.B + v2 &&
                        color.G - v2 <= g && g <= color.G + v2 &&
                        color.R - v2 <= r && r <= color.R + v2) continue; // 指定色に近い範囲は透過
                    outRgbValues[i + j * stride + 0] = b; // B
                    outRgbValues[i + j * stride + 1] = g; // G
                    outRgbValues[i + j * stride + 2] = r; // R
                    outRgbValues[i + j * stride + 3] = a; // A
                }
            });

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
