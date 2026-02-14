using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Com.Nakasendo.Gakupetit.Effects;

class E047_Spotlight(BitmapEffects bitmapEffects) : EffectBase(bitmapEffects), IEffect
{
    public int EffectId => 47;
    public string[] Names => ["Spotlight", "スポットライト"];
    public bool IsBackChecked => false;
    public int DefaultValue => 50;
    public string[] Descriptions => [
        "Lights up the center with a warm bulb color and darkens the surroundings. Adjust the circle size with the slider.",
        "中央を電球色で明るく照らし、周囲を暗くします。スライダーで光の円の大きさを調整します。"
    ];
    public Color GetDefaultColor(Color nowColor) => Color.FromArgb(255, 200, 130);

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        ArgumentNullException.ThrowIfNull(srcBitmap);
        Bitmap bmp = new(srcBitmap);
        int w = bmp.Width;
        int h = bmp.Height;

        try
        {
            float centerX = w / 2f;
            float centerY = h / 2f;
            float shorter = Math.Min(w, h);

            // スライダー値 0-100 を 50-250 に変換して楕円の半径を制御
            float mapped = v * 2f + 50f;
            float radius = shorter * (mapped / 200f);
            float rx = radius;
            float ry = radius * 0.65f;

            // 回転の逆変換用 (-60度)
            float rad = -60f * MathF.PI / 180f;
            float cos = MathF.Cos(rad);
            float sin = MathF.Sin(rad);

            // 選択色の正規化
            float cr = color.R / 255f;
            float cg = color.G / 255f;
            float cb = color.B / 255f;

            // ピクセルデータ取得
            var rect = new Rectangle(0, 0, w, h);
            var bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int stride = bmpData.Stride;
            byte[] pixels = new byte[stride * h];
            Marshal.Copy(bmpData.Scan0, pixels, 0, pixels.Length);

            for (int y = 0; y < h; y++)
            {
                int rowOffset = y * stride;
                for (int x = 0; x < w; x++)
                {
                    // 回転楕円のローカル座標に変換
                    float dx = x - centerX;
                    float dy = y - centerY;
                    float lx = dx * cos - dy * sin;
                    float ly = dx * sin + dy * cos;

                    // 楕円空間での距離 (1.0 = 境界上)
                    float dist = MathF.Sqrt(lx * lx / (rx * rx) + ly * ly / (ry * ry));

                    // 乗算係数 (中心=明るい, 外側=暗い)
                    float factor;
                    if (dist < 0.85f)
                    {
                        factor = 1.2f;
                    }
                    else if (dist < 1.0f)
                    {
                        float t = (dist - 0.85f) / 0.15f;
                        t = t * t * (3f - 2f * t); // smoothstep
                        factor = 1.2f * (1 - t) + 0.4f * t;
                    }
                    else
                    {
                        factor = 0.4f;
                    }

                    // 全体に均一に電球色を乗算
                    float rf = factor * cr;
                    float gf = factor * cg;
                    float bf = factor * cb;

                    int idx = rowOffset + x * 4;
                    pixels[idx + 0] = (byte)Math.Clamp((int)(pixels[idx + 0] * bf), 0, 255); // B
                    pixels[idx + 1] = (byte)Math.Clamp((int)(pixels[idx + 1] * gf), 0, 255); // G
                    pixels[idx + 2] = (byte)Math.Clamp((int)(pixels[idx + 2] * rf), 0, 255); // R
                }
            }

            Marshal.Copy(pixels, 0, bmpData.Scan0, pixels.Length);
            bmp.UnlockBits(bmpData);
        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }

        return bmp;
    }
}
