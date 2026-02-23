using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Com.Nakasendo.Gakupetit.Effects;

class E041_Glitch(BitmapEffects bitmapEffects) : EffectBase(bitmapEffects), IEffect
{
    public int EffectId => 41;
    public string[] Names => ["Glitch", "グリッチ"];
    public bool IsBackChecked => true;
    public int DefaultValue => 30;
    public string[] Descriptions => [
        "Adds digital glitch effects like horizontal shifts and color bleeding. Adjust intensity with the slider.",
        "横方向のズレや色の滲みなど、デジタルノイズのようなグリッチ効果を追加します。スライダーで強さを調整します。"
    ];
    public Color GetDefaultColor(Color nowColor) => nowColor;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        if (srcBitmap == null) throw new ArgumentNullException(nameof(srcBitmap));
        if (v == 0) return new(srcBitmap);

        var w = srcBitmap.Width;
        var h = srcBitmap.Height;
        Bitmap bmp = new(w, h);
        Random rundomGenerator = new(EffectId + v);

        float intensity = v / 100f;

        try
        {
            // 1. 枠用の中央を抜いたマスクを作成
            // vの値に応じて枠の太さを決める（スライダーの値の20%程度を最大幅とする）
            int borderSize = (int)(Math.Min(w, h) * 0.15f * intensity) + 10;
            using Bitmap maskBmp = new(w, h);
            using (var gMask = Graphics.FromImage(maskBmp))
            {
                gMask.Clear(Color.White); // 全体を白（エフェクト適用外）にする
                // 中央を黒（エフェクト適用対象）で塗りつぶす
                // 実際には「枠だけ」にしたいので、中央を白く残し、外側を黒くする
                gMask.FillRectangle(Brushes.Black, 0, 0, w, h);
                gMask.FillRectangle(Brushes.White, borderSize, borderSize, w - borderSize * 2, h - borderSize * 2);
            }

            using var g = Graphics.FromImage(bmp);
            // 元画像を背景として描画
            g.DrawImage(srcBitmap, 0, 0, w, h);

            // エフェクト用の一時バッファ
            using Bitmap effectBmp = new(w, h);
            using (var gEff = Graphics.FromImage(effectBmp))
            {
                gEff.Clear(color);
                gEff.SmoothingMode = SmoothingMode.None;
                gEff.InterpolationMode = InterpolationMode.NearestNeighbor;

                // 水平方向のスライスズレ
                int numSlices = 10 + (int)(intensity * 30);
                int currentY = 0;
                while (currentY < h)
                {
                    int sliceHeight = rundomGenerator.Next(2, Math.Max(3, h / 10));
                    if (currentY + sliceHeight > h) sliceHeight = h - currentY;

                    int offsetX = 0;
                    double r = rundomGenerator.NextDouble();
                    if (r < 0.15 * intensity) offsetX = (int)((rundomGenerator.NextDouble() - 0.5) * w * 0.2 * intensity);
                    else if (r < 0.4 * intensity) offsetX = (int)((rundomGenerator.NextDouble() - 0.5) * w * 0.05 * intensity);

                    gEff.DrawImage(srcBitmap,
                        new Rectangle(offsetX, currentY, w, sliceHeight),
                        new Rectangle(0, currentY, w, sliceHeight),
                        GraphicsUnit.Pixel);

                    currentY += sliceHeight;
                }

                // Chromatic Aberration
                using Bitmap shiftedBmp = new(effectBmp);
                float shiftAmount = w * 0.015f * intensity;
                DrawColorChannel(gEff, shiftedBmp, shiftAmount, 0, 1, 0, 0, 0.4f);
                DrawColorChannel(gEff, shiftedBmp, -shiftAmount, 0, 0, 0, 1, 0.4f);

                // ノイズライン
                if (intensity > 0.3)
                {
                    using var pen = new Pen(Color.FromArgb((int)(40 * intensity), 0, 0, 0));
                    for (int y = 0; y < h; y += 4) gEff.DrawLine(pen, 0, y, w, y);
                }
            }

            // 2. マスクを使用して、枠の部分だけエフェクト画像を合成
            // EffectBase.Masking を利用して、maskBmpが黒い部分にeffectBmpを、白い部分に元のままを適用する
            // Maskingの実装は「maskが0(黒)でない場合にcolorに近づける」ロジックなので、
            // ここでは自前でマスク合成を行うか、Maskingの仕組みに合わせる

            // シンプルに枠の部分だけをg.DrawImageで上書きする（マスクとして透過処理）
            using (ImageAttributes ia = new ImageAttributes())
            {
                // マスク画像をアルファチャネルとして使用して合成
                // (GDI+で直接マスク合成は面倒なので、ビット操作を行うか、手動で合成)
                Rectangle rect = new(0, 0, w, h);
                var outData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                var effData = effectBmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                var maskData = maskBmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                int size = outData.Stride * h;
                byte[] outRgb = new byte[size];
                byte[] effRgb = new byte[size];
                byte[] maskRgb = new byte[size];

                Marshal.Copy(outData.Scan0, outRgb, 0, size);
                Marshal.Copy(effData.Scan0, effRgb, 0, size);
                Marshal.Copy(maskData.Scan0, maskRgb, 0, size);

                Parallel.For(0, h, y =>
                {
                    int offset = y * outData.Stride;
                    for (int x = 0; x < w; x++)
                    {
                        int i = offset + x * 4;
                        // マスクが黒(0,0,0)に近いほどエフェクトを強くかける
                        float m = 1.0f - (maskRgb[i] / 255.0f);
                        if (m > 0)
                        {
                            outRgb[i + 0] = (byte)(outRgb[i + 0] * (1 - m) + effRgb[i + 0] * m);
                            outRgb[i + 1] = (byte)(outRgb[i + 1] * (1 - m) + effRgb[i + 1] * m);
                            outRgb[i + 2] = (byte)(outRgb[i + 2] * (1 - m) + effRgb[i + 2] * m);
                        }
                    }
                });

                Marshal.Copy(outRgb, 0, outData.Scan0, size);
                bmp.UnlockBits(outData);
                effectBmp.UnlockBits(effData);
                maskBmp.UnlockBits(maskData);
            }
        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }

        return bmp;
    }

    private static void DrawColorChannel(Graphics g, Bitmap src, float dx, float dy, float r, float g_val, float b, float alpha)
    {
        using ImageAttributes ia = new ImageAttributes();
        ColorMatrix cm = new ColorMatrix(new float[][] {
            new float[] {r, 0, 0, 0, 0},
            new float[] {0, g_val, 0, 0, 0},
            new float[] {0, 0, b, 0, 0},
            new float[] {0, 0, 0, alpha, 0},
            new float[] {0, 0, 0, 0, 1}
        });
        ia.SetColorMatrix(cm);

        g.DrawImage(src,
            new Rectangle((int)dx, (int)dy, src.Width, src.Height),
            0, 0, src.Width, src.Height,
            GraphicsUnit.Pixel, ia);
    }
}
