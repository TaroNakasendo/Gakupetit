using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;

namespace Com.Nakasendo.Gakupetit.Effects;

class E043_PolygonScatter(BitmapEffects bitmapEffects) : EffectBase(bitmapEffects), IEffect
{
    public int EffectId => 43;
    public string[] Names => ["Polygon Scatter", "ポリゴン散布"];
    public bool IsBackChecked => true;
    public int DefaultValue => 50;
    public string[] Descriptions => [
        "Adds a frame by scattering random semi-transparent polygons of various sizes. Adjust the amount with the slider.",
        "様々な大きさの半透明な多角形をランダムにちりばめた枠を追加します。スライダーで量を調整します。"
    ];
    public Color GetDefaultColor(Color nowColor) => Color.FromArgb(255, nowColor);

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        ArgumentNullException.ThrowIfNull(srcBitmap);
        if (v == 0) return new Bitmap(srcBitmap);

        Bitmap bmp = new(srcBitmap);
        int w = bmp.Width;
        int h = bmp.Height;

        try
        {
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // シードを固定して、パラメータ変更時に同じ配置にならないように工夫
            Random rnd = new(EffectId + v + color.ToArgb());

            // 描画する多角形の数
            int count = v * 3;
            
            // 枠の範囲 (少し狭める)
            float borderSize = Math.Min(w, h) * 0.2f;

            for (int i = 0; i < count; i++)
            {
                // 多角形の頂点数 (3〜5)
                int pointsCount = rnd.Next(3, 6);
                PointF[] points = new PointF[pointsCount];

                // 位置の決定 (枠の外縁に強く寄せる)
                float cx, cy;
                double edgeSelector = rnd.NextDouble();
                
                // 距離の分布を調整 (2乗することで端に集中させる)
                // 少しマイナス値（画像の外側）も許容して、外側から重なるようにする
                float dist = (float)(Math.Pow(rnd.NextDouble(), 2.0) * borderSize) - (borderSize * 0.1f);

                if (edgeSelector < 0.25) // Top
                {
                    cx = (float)rnd.NextDouble() * w;
                    cy = dist;
                }
                else if (edgeSelector < 0.5) // Bottom
                {
                    cx = (float)rnd.NextDouble() * w;
                    cy = h - dist;
                }
                else if (edgeSelector < 0.75) // Left
                {
                    cx = dist;
                    cy = (float)rnd.NextDouble() * h;
                }
                else // Right
                {
                    cx = w - dist;
                    cy = (float)rnd.NextDouble() * h;
                }

                // サイズ
                float baseSize = borderSize * 0.6f;
                float size = (float)(rnd.NextDouble() * baseSize * 0.7 + baseSize * 0.3);

                // 回転
                double startAngle = rnd.NextDouble() * Math.PI * 2;

                for (int j = 0; j < pointsCount; j++)
                {
                    double angle = startAngle + 2 * Math.PI * j / pointsCount;
                    // 少し歪ませる
                    float r = size * (float)(0.7 + rnd.NextDouble() * 0.3);
                    points[j] = new PointF(
                        cx + (float)(Math.Cos(angle) * r),
                        cy + (float)(Math.Sin(angle) * r)
                    );
                }

                // 透明度と色のゆらぎ
                int alpha = (int)(color.A * (0.3 + rnd.NextDouble() * 0.7));
                // 色を少し変える
                int r_col = Math.Clamp(color.R + rnd.Next(-20, 21), 0, 255);
                int g_col = Math.Clamp(color.G + rnd.Next(-20, 21), 0, 255);
                int b_col = Math.Clamp(color.B + rnd.Next(-20, 21), 0, 255);

                using var brush = new SolidBrush(Color.FromArgb(alpha, r_col, g_col, b_col));
                g.FillPolygon(brush, points);
            }
        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }

        return bmp;
    }
}
