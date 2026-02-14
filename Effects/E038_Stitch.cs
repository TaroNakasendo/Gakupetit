using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;

namespace Com.Nakasendo.Gakupetit.Effects;

class E038_Stitch(BitmapEffects bitmapEffects) : EffectBase(bitmapEffects), IEffect
{
    public int EffectId => 38;
    public string[] Names => ["Stitch", "ステッチ"];
    public bool IsBackChecked => true;
    public int DefaultValue => 30;
    public string[] Descriptions => [
        "Draws a rough, yarn-like stitch frame. Adjust the stitch length and roughness with the slider.",
        "毛糸のようなラフなタッチの縫い目で枠を描きます。スライダーで縫い目の長さと不揃いさを調整します。"
    ];
    public Color GetDefaultColor(Color nowColor) => nowColor;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        if (srcBitmap == null) throw new ArgumentNullException(nameof(srcBitmap));
        if (v == 0) return new(srcBitmap);

        Bitmap bmp = new(srcBitmap);
        var w = bmp.Width;
        var h = bmp.Height;

        try
        {
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // パラメータ調整
            var minSide = Math.Min(w, h);
            // 毛糸っぽさを出すため、少し太めにする
            float penWidth = Math.Max(3.0f, minSide / 200.0f); 
            float margin = minSide / 20.0f;

            // vの値に応じて、縫い目の長さと不揃いさを変える
            float step = Math.Max(penWidth * 1.5f, penWidth + v / 2.0f);
            float jitter = Math.Max(2.0f, penWidth * 0.5f * v / 50.0f); // 乱れ具合

            // ランダムシード（再描画でチラつかないように固定してもよいが、今回は動きを楽しむため変動させるか、ID依存にする）
            Random rnd = new Random(EffectId + v);

            // ステッチ（毛糸）の色ペン
            using var pen = new Pen(color, penWidth);
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;
            
            // 影ペン（少し太く、薄く）
            using var shadowPen = new Pen(Color.FromArgb(80, 0, 0, 0), penWidth * 1.2f);
            shadowPen.StartCap = LineCap.Round;
            shadowPen.EndCap = LineCap.Round;

            // 四角形の頂点
            float x1 = margin;
            float y1 = margin;
            float x2 = w - margin;
            float y2 = h - margin;

            // 4辺を描画する
            DrawYarnLine(g, pen, shadowPen, x1, y1, x2, y1, step, jitter, rnd); // Top
            DrawYarnLine(g, pen, shadowPen, x2, y1, x2, y2, step, jitter, rnd); // Right
            DrawYarnLine(g, pen, shadowPen, x2, y2, x1, y2, step, jitter, rnd); // Bottom
            DrawYarnLine(g, pen, shadowPen, x1, y2, x1, y1, step, jitter, rnd); // Left
        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }

        return bmp;
    }

    /// <summary>
    /// 毛糸っぽいラフな破線を描く
    /// </summary>
    private static void DrawYarnLine(Graphics g, Pen pen, Pen shadowPen, float x1, float y1, float x2, float y2, float step, float jitter, Random rnd)
    {
        // 線のベクトルと距離
        float dx = x2 - x1;
        float dy = y2 - y1;
        float dist = (float)Math.Sqrt(dx * dx + dy * dy);
        
        // 正規化
        if (dist == 0) return;
        float nx = dx / dist;
        float ny = dy / dist;
        
        // 直交ベクトル（揺らぎ用）
        float px = -ny;
        float py = nx;

        // ステップごとに線を描く
        float currentDist = 0;

        // 一つ一つの縫い目は独立した線分として描画し、角度や位置を少しずらす
        while (currentDist < dist)
        {
            // 今回の縫い目の長さ（少しランダムにする）
            float stitchLen = step * 0.6f + (float)(rnd.NextDouble() * step * 0.4f);
            
            // 隙間（次の開始位置まで）
            float gapLen = step * 0.3f + (float)(rnd.NextDouble() * step * 0.2f);

            // 終了判定
            if (currentDist + stitchLen > dist) stitchLen = dist - currentDist;

            // 基準始点
            float sx = x1 + nx * currentDist;
            float sy = y1 + ny * currentDist;

            // 基準終点
            float ex = x1 + nx * (currentDist + stitchLen);
            float ey = y1 + ny * (currentDist + stitchLen);

            // 座標をジッター（乱れ）させる
            // 始点と終点を少しずらして「手で縫った感」を出す
            float j1 = (float)(rnd.NextDouble() - 0.5) * jitter;
            float j2 = (float)(rnd.NextDouble() - 0.5) * jitter;

            // 直交方向のズレ（縫い目が一直線にならないように）
            float pShift1 = (float)(rnd.NextDouble() - 0.5) * jitter * 0.5f;
            float pShift2 = (float)(rnd.NextDouble() - 0.5) * jitter * 0.5f;

            PointF pStart = new PointF(sx + nx * j1 + px * pShift1, sy + ny * j1 + py * pShift1);
            PointF pEnd   = new PointF(ex + nx * j2 + px * pShift2, ey + ny * j2 + py * pShift2);

            // 影を描画（少し右下にずらす）
            float shadowOffset = pen.Width * 0.3f;
            g.DrawLine(shadowPen, pStart.X + shadowOffset, pStart.Y + shadowOffset, pEnd.X + shadowOffset, pEnd.Y + shadowOffset);

            // 本体を描画 (少し膨らみを持たせてもいいが、今回はDrawLineで端点丸め)
            g.DrawLine(pen, pStart, pEnd);

            currentDist += stitchLen + gapLen;
        }
    }
}
