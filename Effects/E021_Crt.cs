using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Com.Nakasendo.Gakupetit.Effects;

class E021_Crt : EffectBase, IEffect
{
    public E021_Crt(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 21;
    public string[] Names => new[] { "CRT", "ブラウン管" };
    public bool IsBackChecked => true;
    public int DefaultValue => 42;
    public string[] Descriptions => new[] {
        $"The effect is as if projected on a narlog TV. You can change the brightness with the slider.",
        $"アナログテレビで映し出されているようなエフェクトです。スライダーで明るさを変更できます。" };
    public Color GetDefaultColor(Color nowColor) => nowColor;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        var w = srcBitmap.Width;
        var h = srcBitmap.Height;

        // 出力用ビットマップ
        Bitmap retBmp = new(srcBitmap);
        try
        {
            // ブラウン管型の切り取りマスク(中央が黒)
            Bitmap clopMask = new(srcBitmap);

            using var g = Graphics.FromImage(clopMask);
            g.Clear(Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var ww = w / 1600.0f;
            var hh = h / 1200.0f;
            var scale = 0.9f;
            g.TranslateTransform(-w * scale / 2 + w / 2, -h * scale / 2 + h / 2);
            g.ScaleTransform(scale, scale);

            using GraphicsPath gp = new();
            gp.AddBeziers(new PointF[] {
                        new PointF(ww * 115, hh * 115), // 左上の点
                        new PointF(ww * (115 + 140), hh * (115 - 140)), // 左上コントロールポイント
                        new PointF(ww * 1600 - (ww * (115 + 140)), hh * (115 - 140)), // 右上コントロールポイント
                        new PointF(ww * 1600 - ww * 115, hh * 115), // 右上の点
                        new PointF(ww * 1600 - (ww * (115 - 140)), hh * (115 + 140)), // 右上コントロールポイント2
                        new PointF(ww * 1600 - (ww * (115 - 140)), hh * 1200 - (hh * (115 + 140))), // 右下コントロールポイント
                        new PointF(ww * 1600 - ww * 115, hh * 1200 - hh * 115), // 右下の点
                        new PointF(ww * 1600 - (ww * (115 + 140)), hh * 1200 - hh * (115 - 140)), // 右下コントロールポイント2
                        new PointF(ww * (115 + 140), hh * 1200 - hh * (115 - 140)), // 左下コントロールポイント
                        new PointF(ww * 115, hh * 1200 - hh * 115), // 左下の点
                        new PointF(ww * (115 - 140), hh * 1200 - (hh * (115 + 140))), // 左下コントロールポイント2
                        new PointF(ww * (115 - 140), hh * (115 + 140)), // 左上コントロールポイント2
                        new PointF(ww * 115, hh * 115), // 左上の点
                        }); // 中央下の点　

            g.FillPath(Brushes.Black, gp);

            // ブラウン管の左上影用マスク(中央が白)
            var negaLeftTopClopMask = CreateNegativeMask(clopMask, true);
            negaLeftTopClopMask = Blur.BlurMask(negaLeftTopClopMask, w, h, 40, true);
            // 反転マスク(中央が白)
            var negaClopMask = CreateNegativeMask(clopMask);
            // 左上のふちが白っぽいもの(枠の凹み表現用)
            negaClopMask = base.Masking(v, Color.Black, negaLeftTopClopMask, negaClopMask);
            // 凹みの右下
            Bitmap rotatedNegaClopMask = new(negaClopMask);
            rotatedNegaClopMask.RotateFlip(RotateFlipType.Rotate180FlipNone);

            // ブラウン管をぼかしたマスク(ブラウン管の周りに沿って暗くする用)
            var bluredClopMask = Blur.BlurMask(clopMask, w, h, 50);

            // 周りが暗くなっている画像(CRT用独自に定義されているマスク効果)
            var shadedBmp = Masking(v * 7 / 10, color, srcBitmap, srcBitmap);
            // さらにブラウン管の周りに沿って暗くする
            shadedBmp = base.Masking(v, Color.Black, shadedBmp, bluredClopMask);

            // ブラウン管に沿って切り取り
            var clopedBmp = base.Masking(v, color, shadedBmp, clopMask);
            // 凹みをつける(左上)
            clopedBmp = base.Masking(v, Color.Black, clopedBmp, negaClopMask);
            // 凹みをつける(右下)
            Color whityColor = Color.FromArgb((color.R * 2 + 255) / 3, (color.G * 2 + 255) / 3, (color.B * 2 + 255) / 3);
            var dentBmp = base.Masking(v, whityColor, clopedBmp, rotatedNegaClopMask);

            // 左上の反射
            Bitmap shiftedClopMask = new(w, h);
            using var g2 = Graphics.FromImage(shiftedClopMask);
            var ww2 = w / 1600.0f;
            var hh2 = h / 1200.0f;
            g2.Clear(Color.Black);

            var negaMask = CreateNegativeMask(clopMask);
            var m = Math.Max(w, h);
            g2.DrawImage(negaMask, m / 40, m / 40, w - m * 2 / 40, h - m * 2 / 40);
            LinearGradientBrush gb = new(new RectangleF(m / 20, 0, w / 2 - m / 20, h), Color.FromArgb(v * 255 / 100, 0, 0, 0), Color.Black, LinearGradientMode.Horizontal);
            g2.FillRectangle(gb, g2.VisibleClipBounds);
            g2.FillRectangle(Brushes.Black, new RectangleF(w / 2, 0, w / 2, h));
            gb = new(new RectangleF(0, m / 20, w, h / 2 - m / 20), Color.FromArgb(v * 255 / 100, 0, 0, 0), Color.Black, LinearGradientMode.Vertical);
            g2.FillRectangle(gb, g2.VisibleClipBounds);
            g2.FillRectangle(Brushes.Black, new RectangleF(0, h / 2, w, h / 2));

            // 効果を高めるため、自分で自分にマスク
            shiftedClopMask = Masking(v * 7 / 10, color, shiftedClopMask, shiftedClopMask);
            retBmp = base.Masking(v, Color.White, dentBmp, shiftedClopMask);
        }
        catch (Exception)
        {
            retBmp.Dispose();
            throw;
        }

        return retBmp;
    }

    private static Bitmap CreateNegativeMask(Image img, bool useOnlyLeftTop = false)
    {
        // ネガティブイメージの描画先となるImageオブジェクトを作成
        Bitmap negaImg = new(img.Width, img.Height);

        // negaImgのGraphicsオブジェクトを取得
        using var g = Graphics.FromImage(negaImg);

        // ColorMatrixオブジェクトの作成 行列の値を変更して、色が反転されるようにする
        ColorMatrix cm = new() { Matrix00 = -1, Matrix11 = -1, Matrix22 = -1, Matrix33 = 1, Matrix40 = 1, Matrix41 = 1, Matrix42 = 1, Matrix44 = 1 };

        // ImageAttributesオブジェクトの作成
        ImageAttributes ia = new();

        //C olorMatrixを設定する
        ia.SetColorMatrix(cm);

        // ImageAttributesを使用して色が反転した画像を描画
        g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);

        // 右下の不要な部分を切り取り
        if (useOnlyLeftTop)
        {
            g.FillPie(Brushes.Black, new Rectangle(img.Width / 8, img.Height / 8, img.Width * 14 / 8, img.Height * 14 / 8), 90, 180);
        }

        return negaImg;
    }

    /// <summary>
    /// CRT用の右下中心ビネット
    /// </summary>
    /// <param name="color">背景色</param>
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

            //4byteずつ進む
            Parallel.For(0, h, j =>
            {
                for (int i = 0; i < stride; i += 4)
                {
                    // 右下の点からの距離
                    var sub_r = Math.Max(0, 1.5 - v / 50.0 * (Math.Sqrt(Math.Pow((i / 4 - w * 3 / 4), 2) + Math.Pow((j - h * 3 / 4), 2)) / Math.Min(w, h)));
                    outRgbValues[i + j * stride + 0] = (byte)(Math.Min(inRgbValues[i + j * stride + 0] * sub_r, 255)); // B
                    outRgbValues[i + j * stride + 1] = (byte)(Math.Min(inRgbValues[i + j * stride + 1] * sub_r, 255)); // G
                    outRgbValues[i + j * stride + 2] = (byte)(Math.Min(inRgbValues[i + j * stride + 2] * sub_r, 255)); // R
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
