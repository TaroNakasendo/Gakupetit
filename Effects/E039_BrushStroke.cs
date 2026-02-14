using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;

namespace Com.Nakasendo.Gakupetit.Effects;

class E039_BrushStroke(BitmapEffects bitmapEffects) : EffectBase(bitmapEffects), IEffect
{
    public int EffectId => 39;
    public string[] Names => ["Brush Stroke", "筆あと"];
    public bool IsBackChecked => true;
    public int DefaultValue => 40;
    public string[] Descriptions => [
        "Adds frame with thick, painterly brush strokes. Adjust thickness and dry-brush effect with the slider.",
        "太い筆で塗ったような枠を追加します。スライダーで太さとかすれ具合を調整します。"
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

            var minSide = Math.Min(w, h);
            // 筆の基本の太さ
            float baseWidth = Math.Max(5.0f, minSide * v / 500.0f);
            float margin = baseWidth * 0.5f;

            Random rnd = new Random(EffectId + v);

            // 四辺の矩形
            float x1 = margin;
            float y1 = margin;
            float x2 = w - margin;
            float y2 = h - margin;

            // 各辺を筆で塗る
            DrawBrushLine(g, color, x1, y1, x2, y1, baseWidth, rnd); // Top
            DrawBrushLine(g, color, x2, y1, x2, y2, baseWidth, rnd); // Right
            DrawBrushLine(g, color, x2, y2, x1, y2, baseWidth, rnd); // Bottom
            DrawBrushLine(g, color, x1, y2, x1, y1, baseWidth, rnd); // Left
        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }

        return bmp;
    }

    private static void DrawBrushLine(Graphics g, Color color, float x1, float y1, float x2, float y2, float width, Random rnd)
    {
        float dx = x2 - x1;
        float dy = y2 - y1;
        float length = (float)Math.Sqrt(dx * dx + dy * dy);
        if (length < 1) return;

        float nx = dx / length;
        float ny = dy / length;
        float px = -ny; // 法線
        float py = nx;

        // 1本の太い線ではなく、何本かの細い線の集合（ bristles ）として描画して「かすれ」を出す
        int bristles = 15;
        for (int i = 0; i < bristles; i++)
        {
            // 中心からのオフセット (-0.5 to 0.5)
            float offsetFactor = (float)(i / (float)(bristles - 1) - 0.5);
            float offset = offsetFactor * width;

            // 各毛（bristle）の透明度を変えることで「かすれ」を表現
            int alpha = 100 + rnd.Next(155);
            using var pen = new Pen(Color.FromArgb(alpha, color), 1.0f + (float)rnd.NextDouble() * 2.0f);
            
            // 毛ごとに長さを微妙に変える（はみ出しを表現）
            float overshootStart = (float)(rnd.NextDouble() - 0.5) * width * 0.8f;
            float overshootEnd   = (float)(rnd.NextDouble() - 0.5) * width * 0.8f;

            // 基準点にオフセットを加える
            float sx = x1 + px * offset + nx * overshootStart;
            float sy = y1 + py * offset + ny * overshootStart;
            float ex = x2 + px * offset + nx * overshootEnd;
            float ey = y2 + py * offset + ny * overshootEnd;

            // 途中で筆圧が変わるように、短いセグメントに分割して描画（少しうねらせる）
            DrawWavyLine(g, pen, sx, sy, ex, ey, width * 0.05f, rnd);
        }
    }

    private static void DrawWavyLine(Graphics g, Pen pen, float x1, float y1, float x2, float y2, float wave, Random rnd)
    {
        float dx = x2 - x1;
        float dy = y2 - y1;
        float dist = (float)Math.Sqrt(dx * dx + dy * dy);
        
        int segments = 4;
        float sx = x1;
        float sy = y1;

        for (int i = 1; i <= segments; i++)
        {
            float t = i / (float)segments;
            float tx = x1 + dx * t;
            float ty = y1 + dy * t;

            // 垂直方向に少しゆらす
            float nx = -(y2 - y1) / dist;
            float ny = (x2 - x1) / dist;
            
            if (i < segments)
            {
                tx += nx * (float)(rnd.NextDouble() - 0.5) * wave;
                ty += ny * (float)(rnd.NextDouble() - 0.5) * wave;
            }

            g.DrawLine(pen, sx, sy, tx, ty);
            sx = tx;
            sy = ty;
        }
    }
}
