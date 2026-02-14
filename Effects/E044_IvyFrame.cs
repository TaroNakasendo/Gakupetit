using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;

namespace Com.Nakasendo.Gakupetit.Effects;

class E044_IvyFrame(BitmapEffects bitmapEffects) : EffectBase(bitmapEffects), IEffect
{
    public int EffectId => 44;
    public string[] Names => ["Ivy Frame", "アイビー枠"];
    public bool IsBackChecked => true;
    public int DefaultValue => 50;
    public string[] Descriptions => [
        "Adds a decorative frame with ivy and vine designs. Adjust the density with the slider.",
        "蔦や草のつるの意匠を凝らした飾り枠を追加します。スライダーで密度を調整します。"
    ];
    public Color GetDefaultColor(Color nowColor) => nowColor;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        if (srcBitmap == null) throw new ArgumentNullException(nameof(srcBitmap));
        if (v == 0) return new Bitmap(srcBitmap);

        Bitmap bmp = new(srcBitmap);
        int w = bmp.Width;
        int h = bmp.Height;

        try
        {
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Random rnd = new(EffectId + v + color.ToArgb());

            // 枠の太さの目安
            float borderSize = Math.Min(w, h) * 0.15f;
            
            // つるの数
            int vineCount = Math.Max(1, v / 10);
            
            using var pen = new Pen(color, borderSize * 0.05f);
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;
            pen.LineJoin = LineJoin.Round;

            // 各辺につるを描画
            DrawVines(g, rnd, 0, 0, w, 0, borderSize, vineCount, color); // Top
            DrawVines(g, rnd, 0, h, w, h, borderSize, vineCount, color); // Bottom
            DrawVines(g, rnd, 0, 0, 0, h, borderSize, vineCount, color); // Left
            DrawVines(g, rnd, w, 0, w, h, borderSize, vineCount, color); // Right
        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }

        return bmp;
    }

    private static void DrawVines(Graphics g, Random rnd, float x1, float y1, float x2, float y2, float borderSize, int count, Color color)
    {
        bool isHorizontal = Math.Abs(y1 - y2) < 0.1;
        float length = isHorizontal ? Math.Abs(x1 - x2) : Math.Abs(y1 - y2);

        for (int i = 0; i < count; i++)
        {
            List<PointF> points = new();
            int segments = (int)(length / (borderSize * 0.4f)) + 3;
            
            // つるの基本となる波打ち
            float waveFreq = (float)(0.5 + rnd.NextDouble() * 1.5);
            float waveAmp = borderSize * 0.4f;
            float phase = (float)(rnd.NextDouble() * Math.PI * 2);

            for (int j = 0; j <= segments; j++)
            {
                float t = (float)j / segments;
                float px = x1 + (x2 - x1) * t;
                float py = y1 + (y2 - y1) * t;
                
                // サイン波でゆらゆらさせる
                float offset = (float)Math.Sin(t * segments * waveFreq + phase) * waveAmp;
                // さらにランダムなノイズ
                offset += (float)(rnd.NextDouble() - 0.5) * (borderSize * 0.2f);
                
                if (isHorizontal) py += offset;
                else px += offset;
                
                points.Add(new PointF(px, py));
            }

            if (points.Count >= 3)
            {
                using var vinePen = new Pen(color, (float)(rnd.NextDouble() * 1.5 + 1.0) * (borderSize * 0.03f));
                g.DrawCurve(vinePen, points.ToArray(), 0.7f);

                // 葉っぱと「巻きひげ」を描画
                for (int j = 1; j < points.Count - 1; j++)
                {
                    PointF p = points[j];
                    
                    // 葉っぱ
                    if (rnd.NextDouble() < 0.6)
                    {
                        // 前後の点から進行方向を計算
                        float dx = points[j+1].X - points[j-1].X;
                        float dy = points[j+1].Y - points[j-1].Y;
                        float baseAngle = (float)(Math.Atan2(dy, dx) * 180 / Math.PI);

                        DrawLeaf(g, rnd, p, borderSize * 0.4f, color, baseAngle);
                    }

                    // 補助的な小さなつる（巻きひげ）
                    if (rnd.NextDouble() < 0.2)
                    {
                        DrawTendril(g, rnd, p, borderSize * 0.5f, color);
                    }
                }
            }
        }
    }

    private static void DrawTendril(Graphics g, Random rnd, PointF p, float length, Color color)
    {
        List<PointF> spiral = new();
        float angle = (float)(rnd.NextDouble() * Math.PI * 2);
        float r = 0;
        int steps = 15;
        for (int i = 0; i < steps; i++)
        {
            r += length / steps;
            angle += 0.8f; // ぐるぐる回る
            spiral.Add(new PointF(
                p.X + (float)Math.Cos(angle) * r,
                p.Y + (float)Math.Sin(angle) * r
            ));
        }
        using var tendrilPen = new Pen(color, 1.0f);
        if (spiral.Count >= 2) g.DrawCurve(tendrilPen, spiral.ToArray());
    }

    private static void DrawLeaf(Graphics g, Random rnd, PointF p, float maxSize, Color color, float baseAngle)
    {
        float size = (float)(rnd.NextDouble() * 0.4 + 0.6) * maxSize;
        // 進行方向に対して左右に振る
        float angle = baseAngle + (rnd.Next(0, 2) == 0 ? 60 : -60) + (float)(rnd.NextDouble() * 20 - 10);
        
        int r = Math.Clamp(color.R + rnd.Next(-20, 21), 0, 255);
        int g_col = Math.Clamp(color.G + rnd.Next(-20, 21), 0, 255);
        int b = Math.Clamp(color.B + rnd.Next(-20, 21), 0, 255);
        
        g.TranslateTransform(p.X, p.Y);
        g.RotateTransform(angle);

        // 葉身の描画 (先端を鋭利にした形状)
        using (GraphicsPath path = new())
        {
            // 葉の根元(0,0)から先端(size, 0)へ
            // 制御点を調整して、先端に向かって急激に細くなるようにする
            path.AddBezier(
                new PointF(0, 0),
                new PointF(size * 0.2f, -size * 0.5f),
                new PointF(size * 0.7f, -size * 0.2f),
                new PointF(size, 0)
            );
            path.AddBezier(
                new PointF(size, 0),
                new PointF(size * 0.7f, size * 0.2f),
                new PointF(size * 0.2f, size * 0.5f),
                new PointF(0, 0)
            );

            // 塗りつぶし
            using var leafBrush = new SolidBrush(Color.FromArgb(color.A, r, g_col, b));
            g.FillPath(leafBrush, path);

            // 葉脈 (少し明るい色 or 暗い色)
            using var veinPen = new Pen(Color.FromArgb(color.A / 2, 
                Math.Clamp(r + 30, 0, 255), 
                Math.Clamp(g_col + 30, 0, 255), 
                Math.Clamp(b + 30, 0, 255)), 0.5f);
            g.DrawLine(veinPen, 0, 0, size * 0.8f, 0);
        }
        
        g.ResetTransform();
    }
}
