using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;

namespace Com.Nakasendo.Gakupetit.Effects;

class E040_MaskingTape(BitmapEffects bitmapEffects) : EffectBase(bitmapEffects), IEffect
{
    public int EffectId => 40;
    public string[] Names => ["Masking Tape", "マスキングテープ"];
    public bool IsBackChecked => true;
    public int DefaultValue => 35;
    public string[] Descriptions => [
        "Adds semi-transparent masking tape to the edges of the image. Adjust transparency and width with the slider.",
        "画像の縁に半透明のマスキングテープを貼ったような枠を追加します。スライダーで太さと透明度を調整します。"
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
            // テープの幅
            float tapeWidth = Math.Max(10.0f, minSide * v / 400.0f);
            
            // 色の設定（半透明にする）
            // 指定色のアルファ値をベースにし、極端に高い場合は少し抑える
            int alpha = color.A;
            if (color.A > 200)
            {
                alpha = 180; // 指定色のアルファが高い場合は固定気味に
            }

            Color tapeColor = Color.FromArgb(alpha, color.R, color.G, color.B);
            using var brush = new SolidBrush(tapeColor);

            Random rnd = new Random(EffectId + v);

            // 4辺にテープを貼る
            // Top
            DrawTapeStrip(g, brush, 0, 0, w, 0, tapeWidth, true, w, h, rnd);
            // Bottom
            DrawTapeStrip(g, brush, 0, h, w, h, tapeWidth, true, w, h, rnd);
            // Left
            DrawTapeStrip(g, brush, 0, 0, 0, h, tapeWidth, false, w, h, rnd);
            // Right
            DrawTapeStrip(g, brush, w, 0, w, h, tapeWidth, false, w, h, rnd);
        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }

        return bmp;
    }

    private static void DrawTapeStrip(Graphics g, Brush brush, float x1, float y1, float x2, float y2, float width, bool isHorizontal, float w, float h, Random rnd)
    {
        // テープの形を多角形（Path）で作成する
        using GraphicsPath path = new GraphicsPath();
        
        float margin = width * 0.1f;
        int segments = 20; // ギザギザの細かさ

        if (isHorizontal)
        {
            float yTop = y1;
            float yBottom = y1 + width;
            if (y1 > 0) { yTop = y1 - width; yBottom = y1; } // 下辺用

            // 左端のギザギザ
            List<PointF> points = new List<PointF>();
            for (int i = 0; i <= segments; i++)
            {
                float py = yTop + (width * i / segments);
                float px = (i == 0 || i == segments) ? 0 : (float)(rnd.NextDouble() * width * 0.15f);
                points.Add(new PointF(px, py));
            }

            // 下辺
            for (int i = 1; i < segments; i++)
            {
                float px = w * i / segments;
                float py = yBottom + (float)(rnd.NextDouble() - 0.5f) * width * 0.05f;
                points.Add(new PointF(px, py));
            }

            // 右端のギザギザ
            for (int i = segments; i >= 0; i--)
            {
                float py = yTop + (width * i / segments);
                float px = w - ((i == 0 || i == segments) ? 0 : (float)(rnd.NextDouble() * width * 0.15f));
                points.Add(new PointF(px, py));
            }

            // 上辺
            for (int i = segments - 1; i > 0; i--)
            {
                float px = w * i / segments;
                float py = yTop + (float)(rnd.NextDouble() - 0.5f) * width * 0.05f;
                points.Add(new PointF(px, py));
            }

            g.FillPolygon(brush, points.ToArray());
        }
        else
        {
            float xLeft = x1;
            float xRight = x1 + width;
            if (x1 > 0) { xLeft = x1 - width; xRight = x1; } // 右辺用

            // 上端のギザギザ
            List<PointF> points = new List<PointF>();
            for (int i = 0; i <= segments; i++)
            {
                float px = xLeft + (width * i / segments);
                float py = (i == 0 || i == segments) ? 0 : (float)(rnd.NextDouble() * width * 0.15f);
                points.Add(new PointF(px, py));
            }

            // 右辺
            for (int i = 1; i < segments; i++)
            {
                float py = h * i / segments;
                float px = xRight + (float)(rnd.NextDouble() - 0.5f) * width * 0.05f;
                points.Add(new PointF(px, py));
            }

            // 下端のギザギザ
            for (int i = segments; i >= 0; i--)
            {
                float px = xLeft + (width * i / segments);
                float py = h - ((i == 0 || i == segments) ? 0 : (float)(rnd.NextDouble() * width * 0.15f));
                points.Add(new PointF(px, py));
            }

            // 左辺
            for (int i = segments - 1; i > 0; i--)
            {
                float py = h * i / segments;
                float px = xLeft + (float)(rnd.NextDouble() - 0.5f) * width * 0.05f;
                points.Add(new PointF(px, py));
            }

            g.FillPolygon(brush, points.ToArray());
        }
    }
}
