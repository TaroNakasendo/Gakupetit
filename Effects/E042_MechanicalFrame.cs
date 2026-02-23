using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;

namespace Com.Nakasendo.Gakupetit.Effects;

class E042_MechanicalFrame(BitmapEffects bitmapEffects) : EffectBase(bitmapEffects), IEffect
{
    public int EffectId => 42;
    public string[] Names => ["SF Mechanical Frame", "SFメカニカル枠"];
    public bool IsBackChecked => false;
    public int DefaultValue => 50;
    public string[] Descriptions => [
        "Adds a heavy mechanical monitor frame with random ribs and depressions. No bolts.",
        "リブや窪みが配置された重厚なメカニカル枠を追加します。ボルトはありません。"
    ];
    public Color GetDefaultColor(Color _) => Color.FromArgb(65, 68, 72);

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        ArgumentNullException.ThrowIfNull(srcBitmap);

        Bitmap bmp = new(srcBitmap);
        if (v == 0) return bmp;

        var w = bmp.Width;
        var h = bmp.Height;
        float intensity = v / 100f;
        float fs = Math.Min(w, h) * 0.12f * intensity;
        if (fs < 5) return bmp;

        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = SmoothingMode.AntiAlias;

        Color baseColor = color;
        Color lightColor = ControlPaint.Light(baseColor, 0.4f);
        Color darkColor = ControlPaint.Dark(baseColor, 0.4f);

        // 使用するシード値を画像サイズ等に固定することで、スライダー移動中に装飾が飛ばないようにする
        Random rand = new(w ^ h ^ 42);

        DrawMechanicalPanels(g, w, h, fs, baseColor, lightColor, darkColor, rand);

        return bmp;
    }

    private static void DrawMechanicalPanels(Graphics g, int w, int h, float fs, Color baseCol, Color lightCol, Color darkCol, Random rand)
    {
        // 水平切りに変更（上下が全幅）
        PointF[][] panels =
        [
            // 上部パネル（全幅）
            [
                new(0, 0), new(w, 0),
                new(w, fs),
                new(w * 0.7f, fs), new(w * 0.65f, fs * 1.3f),
                new(w * 0.35f, fs * 1.3f), new(w * 0.3f, fs),
                new(0, fs)
            ],
            // 下部パネル（全幅）
            [
                new(0, h - fs),
                new(w * 0.3f, h - fs), new(w * 0.35f, h - fs * 1.3f),
                new(w * 0.65f, h - fs * 1.3f), new(w * 0.7f, h - fs),
                new(w, h - fs),
                new(w, h), new(0, h)
            ],
            // 左部パネル（上下パネルの間）
            [
                new(0, fs), new(fs, fs),
                new(fs, h * 0.4f), new(fs * 1.25f, h * 0.45f),
                new(fs * 1.25f, h * 0.55f), new(fs, h * 0.6f),
                new(fs, h - fs), new(0, h - fs)
            ],
            // 右部パネル（上下パネルの間）
            [
                new(w, fs), new(w - fs, fs),
                new(w - fs, h * 0.4f), new(w - fs * 1.25f, h * 0.45f),
                new(w - fs * 1.25f, h * 0.55f), new(w - fs, h * 0.6f),
                new(w - fs, h - fs), new(w, h - fs)
            ],
        ];

        // 接合部の線（上下パネルの 2, 6-7, 左右パネルの 0, 6）を描画スキップ
        DrawPanelWithGreebles(g, panels[0], baseCol, lightCol, darkCol, fs, rand, 2, 7);
        DrawPanelWithGreebles(g, panels[1], baseCol, lightCol, darkCol, fs, rand, 0, 5);
        DrawPanelWithGreebles(g, panels[2], baseCol, lightCol, darkCol, fs, rand, 0, 6);
        DrawPanelWithGreebles(g, panels[3], baseCol, lightCol, darkCol, fs, rand, 0, 6);
    }

    private static void DrawPanelWithGreebles(Graphics g, PointF[] points, Color baseCol, Color lightCol, Color darkCol, float fs, Random rand, int skip1 = -1, int skip2 = -1)
    {
        using GraphicsPath path = new();
        path.AddPolygon(points);

        var bounds = GetBounds(points);
        // より輝くメタリック質感（鏡面反射をシミュレート）
        using (var br = new LinearGradientBrush(bounds, darkCol, lightCol, 60f))
        {
            Color shine = Color.FromArgb(235, Color.White);
            ColorBlend cb = new()
            {
                Positions = [0f, 0.15f, 0.3f, 0.5f, 0.7f, 0.85f, 1f],
                Colors = [darkCol, baseCol, shine, lightCol, shine, baseCol, darkCol]
            };
            br.InterpolationColors = cb;
            g.FillPath(br, path);
        }

        // ヘアライン模様の追加
        DrawHairlines(g, path, bounds);

        DrawPanelEdges(g, points, lightCol, darkCol, skip1, skip2);

        var state = g.Save();
        g.SetClip(path);

        // 重ならないようにセグメント分割
        bool isVertical = bounds.Height > bounds.Width;
        int segments = rand.Next(3, 5);
        float padding = Math.Max(5.0f, fs * 0.25f);
        float itemThick = fs * 0.45f; // 装飾の厚み

        for (int i = 0; i < segments; i++)
        {
            RectangleF slot;
            // 枠の外側と内側の中央（fs*0.5付近）に配置されるように厳密に計算
            if (isVertical)
            {
                float segH = (bounds.Height - padding * 2) / segments;
                float x = (bounds.Left < fs) ? (fs * 0.5f - itemThick * 0.5f) : (bounds.Right - fs * 0.5f - itemThick * 0.5f);
                slot = new RectangleF(x, bounds.Y + padding + i * segH, itemThick, segH);
            }
            else
            {
                float segW = (bounds.Width - padding * 2) / segments;
                float y = (bounds.Top < fs) ? (fs * 0.5f - itemThick * 0.5f) : (bounds.Bottom - fs * 0.5f - itemThick * 0.5f);
                slot = new RectangleF(bounds.X + padding + i * segW, y, segW, itemThick);
            }

            if (rand.NextDouble() < 0.2) continue;

            if (rand.NextDouble() < 0.5)
                DrawRibsInSlot(g, slot, fs, lightCol, darkCol, rand);
            else
                DrawLCDPanelInSlot(g, slot, fs, darkCol, rand);
        }

        g.Restore(state);
    }

    private static void DrawHairlines(Graphics g, GraphicsPath path, RectangleF bounds)
    {
        var state = g.Save();
        g.SetClip(path);
        using var hPen = new Pen(Color.FromArgb(40, Color.Black), 1f);
        using var hPenL = new Pen(Color.FromArgb(30, Color.White), 1f);

        float step = 2.0f;
        for (float y = bounds.Top; y < bounds.Bottom; y += step)
        {
            g.DrawLine(hPen, bounds.Left, y, bounds.Right, y);
            g.DrawLine(hPenL, bounds.Left, y + 1.0f, bounds.Right, y + 1.0f);
        }
        g.Restore(state);
    }

    private static void DrawPanelEdges(Graphics g, PointF[] points, Color lightCol, Color darkCol, int skip1, int skip2)
    {
        using var pLight = new Pen(Color.FromArgb(200, Color.White), 1.5f); // 縁も輝かせる
        using var pDark = new Pen(darkCol, 1.5f);
        for (int i = 0; i < points.Length; i++)
        {
            if (i == skip1 || i == skip2) continue;

            var p1 = points[i];
            var p2 = points[(i + 1) % points.Length];
            float angle = (float)(Math.Atan2(p2.Y - p1.Y, p2.X - p1.X) * 180 / Math.PI);
            if (angle < 0) angle += 360;

            if (angle >= 135 && angle < 315) g.DrawLine(pLight, p1, p2);
            else g.DrawLine(pDark, p1, p2);
        }
    }

    private static void DrawRibsInSlot(Graphics g, RectangleF slot, float fs, Color lightCol, Color darkCol, Random rand)
    {
        bool isVertical = slot.Height > slot.Width;
        int count = rand.Next(2, 5);
        float ribThick = fs * 0.12f;
        float gap = fs * 0.06f;
        float totalSize = count * ribThick + (count - 1) * gap;

        if (isVertical)
        {
            float w = slot.Width;
            float x = slot.Left;
            float y = slot.Top + (slot.Height - totalSize) / 2;
            for (int i = 0; i < count; i++)
                DrawBeveledRect(g, new RectangleF(x, y + i * (ribThick + gap), w, ribThick), lightCol, darkCol, true);
        }
        else
        {
            float h = slot.Height;
            float x = slot.Left + (slot.Width - totalSize) / 2;
            float y = slot.Top;
            for (int i = 0; i < count; i++)
                DrawBeveledRect(g, new RectangleF(x + i * (ribThick + gap), y, ribThick, h), lightCol, darkCol, true);
        }
    }

    private static void DrawLCDPanelInSlot(Graphics g, RectangleF slot, float fs, Color darkCol, Random rand)
    {
        float dw, dh;
        if (slot.Height > slot.Width)
        {
            dw = slot.Width * 0.85f;
            dh = Math.Min(slot.Height * 0.85f, fs * 1.5f);
        }
        else
        {
            dw = Math.Min(slot.Width * 0.85f, fs * 1.5f);
            dh = slot.Height * 0.85f;
        }

        float x = slot.Left + (slot.Width - dw) / 2;
        float y = slot.Top + (slot.Height - dh) / 2;
        RectangleF r = new(x, y, dw, dh);
        float chamfer = Math.Min(dw, dh) * 0.2f;

        using GraphicsPath p = new();
        p.AddLine(r.Left + chamfer, r.Top, r.Right - chamfer, r.Top);
        p.AddLine(r.Right - chamfer, r.Top, r.Right, r.Top + chamfer);
        p.AddLine(r.Right, r.Top + chamfer, r.Right, r.Bottom - chamfer);
        p.AddLine(r.Right, r.Bottom - chamfer, r.Right - chamfer, r.Bottom);
        p.AddLine(r.Right - chamfer, r.Bottom, r.Left + chamfer, r.Bottom);
        p.AddLine(r.Left + chamfer, r.Bottom, r.Left, r.Bottom - chamfer);
        p.AddLine(r.Left, r.Bottom - chamfer, r.Left, r.Top + chamfer);
        p.CloseAllFigures();

        Color lcdBg = Color.FromArgb(255, 10, 30, 25);
        using (var br = new LinearGradientBrush(r, lcdBg, Color.Black, 90f))
            g.FillPath(br, p);

        DrawLCDContent(g, p, r, fs, rand);

        using var pL = new Pen(Color.FromArgb(120, Color.Cyan), 1f);
        using var pD = new Pen(Color.FromArgb(150, Color.Black), 1.2f);
        PointF[] pts = p.PathPoints;
        for (int i = 0; i < pts.Length; i++)
        {
            var p1 = pts[i];
            var p2 = pts[(i + 1) % pts.Length];
            float angle = (float)(Math.Atan2(p2.Y - p1.Y, p2.X - p1.X) * 180 / Math.PI);
            if (angle < 0) angle += 360;

            if (angle > 180 && angle < 360) g.DrawLine(pD, p1, p2);
            else g.DrawLine(pL, p1, p2);
        }
    }

    private static void DrawLCDContent(Graphics g, GraphicsPath p, RectangleF r, float fs, Random rand)
    {
        var state = g.Save();
        g.SetClip(p);

        using var gPen = new Pen(Color.FromArgb(30, 0, 255, 150), 0.5f);
        for (float lx = r.Left; lx < r.Right; lx += fs * 0.1f) g.DrawLine(gPen, lx, r.Top, lx, r.Bottom);
        for (float ly = r.Top; ly < r.Bottom; ly += fs * 0.1f) g.DrawLine(gPen, r.Left, ly, r.Right, ly);

        using var tBr = new SolidBrush(Color.FromArgb(200, 0, 255, 150));
        // ハードコードされたラベルを定数化し、保守性・ローカライズ性を向上
        string[] labels = LcdPanelLabels;
        string label = labels[rand.Next(labels.Length)];

        using Font font = new("Consolas", fs * 0.16f, FontStyle.Bold);
        g.DrawString(label, font, tBr, r.Left + 1, r.Top + 1);

        float barW = r.Width * 0.6f;
        float barH = fs * 0.05f;
        g.FillRectangle(tBr, r.Left + 2, r.Bottom - barH - 2, barW * (float)rand.NextDouble(), barH);

        g.Restore(state);
    }

    private static void DrawBeveledRect(Graphics g, RectangleF r, Color lightCol, Color darkCol, bool raised)
    {
        Color topColor = raised ? Color.FromArgb(220, Color.White) : darkCol;
        Color bottomColor = raised ? darkCol : Color.FromArgb(220, Color.White);

        using var pTop = new Pen(topColor, 1.5f);
        using var pBottom = new Pen(bottomColor, 1.5f);

        g.DrawLine(pTop, r.Left, r.Top, r.Right, r.Top);
        g.DrawLine(pTop, r.Left, r.Top, r.Left, r.Bottom);
        g.DrawLine(pBottom, r.Right, r.Top, r.Right, r.Bottom);
        g.DrawLine(pBottom, r.Left, r.Bottom, r.Right, r.Bottom);
    }

    private static RectangleF GetBounds(PointF[] points)
    {
        float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
        foreach (var p in points)
        {
            if (p.X < minX) minX = p.X; if (p.X > maxX) maxX = p.X;
            if (p.Y < minY) minY = p.Y; if (p.Y > maxY) maxY = p.Y;
        }
        return new RectangleF(minX, minY, Math.Max(1, maxX - minX), Math.Max(1, maxY - minY));
    }

    // LCDパネルラベルを定数化（将来的なローカライズや保守性向上のため）
    private static readonly string[] LcdPanelLabels = ["SYS:OK", "LINK:HI", "PWR:ON", "DATA:RW"];
}

