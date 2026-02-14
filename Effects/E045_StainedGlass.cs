using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;

namespace Com.Nakasendo.Gakupetit.Effects;

class E045_StainedGlass(BitmapEffects bitmapEffects) : EffectBase(bitmapEffects), IEffect
{
    public int EffectId => 45;
    public string[] Names => ["Stained Glass", "ステンドグラス"];
    public bool IsBackChecked => false;
    public int DefaultValue => 40;
    public string[] Descriptions => [
        "Adds a beautiful stained glass border. Each piece has unique colors and textures.",
        "美しいステンドグラスの枠を追加します。一枚一枚が異なる色と質感を持っています。"
    ];

    public Color GetDefaultColor(Color _) => Color.FromArgb(200, 100, 50);

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        if (srcBitmap == null) throw new ArgumentNullException(nameof(srcBitmap));
        
        Bitmap bmp = new(srcBitmap);
        if (v == 0) return bmp;

        int w = bmp.Width;
        int h = bmp.Height;

        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // シードを画像サイズと色で固定
        Random rand = new(w ^ h ^ color.ToArgb());

        float borderSize = Math.Min(w, h) * 0.25f * (v / 100f);
        if (borderSize < 5) return bmp;

        DrawStainedGlass(g, w, h, borderSize, color, rand);

        return bmp;
    }

    private static void DrawStainedGlass(Graphics g, int w, int h, float bs, Color baseColor, Random rand)
    {
        // 4つの矩形領域（上、下、左、右）に分けて描画
        // 重なりを考慮して領域を定義
        RectangleF top = new(0, 0, w, bs);
        RectangleF bottom = new(0, h - bs, w, bs);
        RectangleF left = new(0, bs, bs, h - bs * 2);
        RectangleF right = new(w - bs, bs, bs, h - bs * 2);

        DrawGlassStrip(g, top, true, bs, baseColor, rand);
        DrawGlassStrip(g, bottom, true, bs, baseColor, rand);
        DrawGlassStrip(g, left, false, bs, baseColor, rand);
        DrawGlassStrip(g, right, false, bs, baseColor, rand);
    }

    private static void DrawGlassStrip(Graphics g, RectangleF rect, bool isHorizontal, float bs, Color baseColor, Random rand)
    {
        int divisions = isHorizontal ? Math.Max(3, (int)(rect.Width / bs * 1.5f)) : Math.Max(3, (int)(rect.Height / bs * 1.5f));
        float step = isHorizontal ? rect.Width / divisions : rect.Height / divisions;

        // グリッドの交点を生成（揺らぎを加える）
        PointF[,] points = new PointF[divisions + 1, 3];
        float jitter = bs * 0.2f;

        for (int i = 0; i <= divisions; i++)
        {
            float pos = i * step;
            for (int j = 0; j < 3; j++)
            {
                float offset = j * (bs / 2);
                float x, y;
                if (isHorizontal)
                {
                    x = rect.X + pos + (i == 0 || i == divisions ? 0 : (float)(rand.NextDouble() - 0.5) * jitter);
                    y = rect.Y + offset + (j == 0 || j == 2 ? 0 : (float)(rand.NextDouble() - 0.5) * jitter);
                }
                else
                {
                    x = rect.X + offset + (j == 0 || j == 2 ? 0 : (float)(rand.NextDouble() - 0.5) * jitter);
                    y = rect.Y + pos + (i == 0 || i == divisions ? 0 : (float)(rand.NextDouble() - 0.5) * jitter);
                }
                points[i, j] = new PointF(x, y);
            }
        }

        // 各セルを多角形として描画
        for (int i = 0; i < divisions; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                PointF[] piece = [
                    points[i, j],
                    points[i + 1, j],
                    points[i + 1, j + 1],
                    points[i, j + 1]
                ];
                DrawGlassPiece(g, piece, baseColor, rand);
            }
        }
    }

    private static void DrawGlassPiece(Graphics g, PointF[] points, Color baseColor, Random rand)
    {
        // 色のランダム化（色相を少しずらす）
        float h, s, v;
        ColorToHSV(baseColor, out h, out s, out v);
        h = (h + rand.Next(-30, 31) + 360) % 360;
        s = Math.Clamp(s + (float)(rand.NextDouble() - 0.5) * 0.4f, 0.2f, 1.0f);
        v = Math.Clamp(v + (float)(rand.NextDouble() - 0.5) * 0.4f, 0.3f, 0.9f);
        Color pieceColor = ColorFromHSV(h, s, v);
        pieceColor = Color.FromArgb(200, pieceColor);

        using GraphicsPath path = new();
        path.AddPolygon(points);

        // ガラスの質感を出すためのグラデーション
        var bounds = path.GetBounds();
        if (bounds.Width < 1 || bounds.Height < 1) return;

        using (PathGradientBrush pgb = new(path))
        {
            pgb.CenterColor = Color.White;
            pgb.SurroundColors = [pieceColor];
            pgb.CenterPoint = new PointF(bounds.X + bounds.Width * 0.3f, bounds.Y + bounds.Height * 0.3f);
            g.FillPath(pgb, path);
        }

        // 表面の気泡やムラをシミュレート
        if (rand.NextDouble() < 0.3)
        {
            using var hatch = new HatchBrush(HatchStyle.Percent10, Color.FromArgb(40, Color.White), Color.Transparent);
            g.FillPath(hatch, path);
        }

        // 鉛の枠線 (Lead Cames)
        using var pen = new Pen(Color.FromArgb(200, 30, 30, 30), 2.5f);
        pen.LineJoin = LineJoin.Round;
        g.DrawPath(pen, path);

        // 内側のハイライト
        using var lightPen = new Pen(Color.FromArgb(100, Color.White), 1f);
        PointF[] shrinked = ShrinkPolygon(points, 2f);
        if (shrinked.Length > 2)
        {
            g.DrawLines(lightPen, [shrinked[0], shrinked[1], shrinked[3]]);
        }
    }

    private static PointF[] ShrinkPolygon(PointF[] points, float amount)
    {
        PointF center = new(0, 0);
        foreach (var p in points) { center.X += p.X; center.Y += p.Y; }
        center.X /= points.Length;
        center.Y /= points.Length;

        PointF[] result = new PointF[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            float dx = center.X - points[i].X;
            float dy = center.Y - points[i].Y;
            float len = (float)Math.Sqrt(dx * dx + dy * dy);
            if (len > amount)
            {
                result[i] = new PointF(points[i].X + dx / len * amount, points[i].Y + dy / len * amount);
            }
            else
            {
                result[i] = points[i];
            }
        }
        return result;
    }

    // Helper methods for HSV conversion
    private static void ColorToHSV(Color color, out float hue, out float saturation, out float value)
    {
        int max = Math.Max(color.R, Math.Max(color.G, color.B));
        int min = Math.Min(color.R, Math.Min(color.G, color.B));

        hue = color.GetHue();
        saturation = (max == 0) ? 0 : 1f - (1f * min / max);
        value = max / 255f;
    }

    private static Color ColorFromHSV(float hue, float saturation, float value)
    {
        int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
        float f = hue / 60 - (float)Math.Floor(hue / 60);

        value = value * 255;
        int v = Convert.ToInt32(value);
        int p = Convert.ToInt32(value * (1 - saturation));
        int q = Convert.ToInt32(value * (1 - f * saturation));
        int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

        if (hi == 0) return Color.FromArgb(255, v, t, p);
        if (hi == 1) return Color.FromArgb(255, q, v, p);
        if (hi == 2) return Color.FromArgb(255, p, v, t);
        if (hi == 3) return Color.FromArgb(255, p, q, v);
        if (hi == 4) return Color.FromArgb(255, t, p, v);
        return Color.FromArgb(255, v, p, q);
    }
}
