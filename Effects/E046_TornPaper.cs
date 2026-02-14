using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;

namespace Com.Nakasendo.Gakupetit.Effects;

class E046_TornPaper(BitmapEffects bitmapEffects) : EffectBase(bitmapEffects), IEffect
{
    public int EffectId => 46;
    public string[] Names => ["Torn Paper", "破れ紙"];
    public bool IsBackChecked => true;
    public int DefaultValue => 30;
    public string[] Descriptions => [
        "Adds a torn paper edge effect around the image. Adjust the roughness and border width with the slider.",
        "画像の縁を紙を破ったようなギザギザの枠にします。スライダーで破れの荒さと枠幅を調整します。"
    ];
    public Color GetDefaultColor(Color _) => Color.FromArgb(245, 240, 230);

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        if (srcBitmap == null) throw new ArgumentNullException(nameof(srcBitmap));
        if (v == 0) return new(srcBitmap);

        Bitmap bmp = new(srcBitmap);
        int w = bmp.Width;
        int h = bmp.Height;

        try
        {
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int minSide = Math.Min(w, h);
            float borderWidth = Math.Max(8f, minSide * v / 300f);
            float tearDepth = borderWidth * 0.6f;

            Random rnd = new(w ^ h ^ EffectId);

            // 破れた内側の輪郭を生成
            var innerContour = GenerateTornContour(w, h, borderWidth, tearDepth, rnd);

            // 背景色で枠を塗る（破れた紙の部分）
            using var borderPath = new GraphicsPath();
            // 外枠の矩形
            borderPath.AddRectangle(new RectangleF(-1, -1, w + 2, h + 2));
            // 内側の破れた輪郭（反転で穴を空ける）
            borderPath.AddPolygon(innerContour);
            borderPath.FillMode = FillMode.Alternate;

            // 紙の色で塗りつぶし
            using var paperBrush = new SolidBrush(color);
            g.FillPath(paperBrush, borderPath);

            // 紙の繊維テクスチャ（薄い線）
            DrawPaperTexture(g, borderPath, borderWidth, color, rnd, w, h);

            // 破れ目の影（内側に薄い影を落とす）
            DrawTearShadow(g, innerContour, tearDepth);

            // 破れ目のハイライト（紙の断面の白い部分）
            DrawTearHighlight(g, innerContour, color);
        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }

        return bmp;
    }

    /// <summary>
    /// 破れた輪郭を生成する
    /// </summary>
    private static PointF[] GenerateTornContour(int w, int h, float border, float tearDepth, Random rnd)
    {
        List<PointF> points = [];
        int segmentsH = Math.Max(20, w / 8);
        int segmentsV = Math.Max(20, h / 8);

        // 上辺（左→右）
        for (int i = 0; i <= segmentsH; i++)
        {
            float x = w * i / (float)segmentsH;
            float baseY = border;
            float tear = (i == 0 || i == segmentsH) ? 0 : (float)(rnd.NextDouble() * 2 - 1) * tearDepth;
            // 大きな破れを時々入れる
            if (rnd.NextDouble() < 0.1 && i > 1 && i < segmentsH - 1)
                tear += (float)(rnd.NextDouble() - 0.5) * tearDepth * 1.5f;
            points.Add(new PointF(x, baseY + tear));
        }

        // 右辺（上→下）
        for (int i = 1; i < segmentsV; i++)
        {
            float y = h * i / (float)segmentsV;
            float baseX = w - border;
            float tear = (float)(rnd.NextDouble() * 2 - 1) * tearDepth;
            if (rnd.NextDouble() < 0.1)
                tear += (float)(rnd.NextDouble() - 0.5) * tearDepth * 1.5f;
            points.Add(new PointF(baseX + tear, y));
        }

        // 下辺（右→左）
        for (int i = segmentsH; i >= 0; i--)
        {
            float x = w * i / (float)segmentsH;
            float baseY = h - border;
            float tear = (i == 0 || i == segmentsH) ? 0 : (float)(rnd.NextDouble() * 2 - 1) * tearDepth;
            if (rnd.NextDouble() < 0.1 && i > 1 && i < segmentsH - 1)
                tear += (float)(rnd.NextDouble() - 0.5) * tearDepth * 1.5f;
            points.Add(new PointF(x, baseY + tear));
        }

        // 左辺（下→上）
        for (int i = segmentsV - 1; i > 0; i--)
        {
            float y = h * i / (float)segmentsV;
            float baseX = border;
            float tear = (float)(rnd.NextDouble() * 2 - 1) * tearDepth;
            if (rnd.NextDouble() < 0.1)
                tear += (float)(rnd.NextDouble() - 0.5) * tearDepth * 1.5f;
            points.Add(new PointF(baseX + tear, y));
        }

        return [.. points];
    }

    /// <summary>
    /// 紙の繊維テクスチャを描画
    /// </summary>
    private static void DrawPaperTexture(Graphics g, GraphicsPath clipPath, float borderWidth, Color paperColor, Random rnd, int w, int h)
    {
        var state = g.Save();
        g.SetClip(clipPath);

        // 繊維の線を薄く描画
        int fiberCount = (w + h) / 4;
        int darker = Math.Max(0, (int)(paperColor.GetBrightness() * 255) - 15);
        using var fiberPen = new Pen(Color.FromArgb(30, darker, darker, darker), 0.5f);

        for (int i = 0; i < fiberCount; i++)
        {
            float x1 = (float)(rnd.NextDouble() * w);
            float y1 = (float)(rnd.NextDouble() * h);
            float angle = (float)(rnd.NextDouble() * Math.PI);
            float len = 3 + (float)(rnd.NextDouble() * 8);
            float x2 = x1 + (float)Math.Cos(angle) * len;
            float y2 = y1 + (float)Math.Sin(angle) * len;
            g.DrawLine(fiberPen, x1, y1, x2, y2);
        }

        g.Restore(state);
    }

    /// <summary>
    /// 破れ目の影を描画
    /// </summary>
    private static void DrawTearShadow(Graphics g, PointF[] contour, float tearDepth)
    {
        float shadowOffset = Math.Max(1f, tearDepth * 0.15f);

        // 影を内側にずらして描画
        PointF[] shadowContour = new PointF[contour.Length];
        PointF center = new(0, 0);
        foreach (var p in contour) { center.X += p.X; center.Y += p.Y; }
        center.X /= contour.Length;
        center.Y /= contour.Length;

        for (int i = 0; i < contour.Length; i++)
        {
            float dx = center.X - contour[i].X;
            float dy = center.Y - contour[i].Y;
            float len = (float)Math.Sqrt(dx * dx + dy * dy);
            if (len > 0)
            {
                shadowContour[i] = new PointF(
                    contour[i].X + dx / len * shadowOffset,
                    contour[i].Y + dy / len * shadowOffset);
            }
            else
            {
                shadowContour[i] = contour[i];
            }
        }

        using var shadowPen = new Pen(Color.FromArgb(40, 0, 0, 0), shadowOffset * 2);
        shadowPen.LineJoin = LineJoin.Round;
        g.DrawPolygon(shadowPen, shadowContour);
    }

    /// <summary>
    /// 破れ目のハイライトを描画（紙の断面）
    /// </summary>
    private static void DrawTearHighlight(Graphics g, PointF[] contour, Color paperColor)
    {
        Color highlight = ControlPaint.Light(paperColor, 0.5f);
        using var highlightPen = new Pen(Color.FromArgb(120, highlight), 1.2f);
        highlightPen.LineJoin = LineJoin.Round;
        g.DrawPolygon(highlightPen, contour);
    }
}
