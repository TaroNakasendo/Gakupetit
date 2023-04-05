using System.Globalization;

namespace Com.Nakasendo.Gakupetit.EffectEtc;

static class DrawLed
{
    /// <summary>
    /// 撮影日時を挿入
    /// </summary>
    /// <param name="g"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="size"></param>
    /// <param name="color"></param>
    /// <param name="alpha"></param>
    /// <param name="withTime"></param>
    /// <param name="dateTime"></param>
    internal static void DrawDateTime(Graphics g, int width, int height, int size, Color color, int alpha, bool withTime, DateTime dateTime)
    {
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        var margin = 5; // 外枠のマージン
        var space = 5; // 文字間隔

        var dateString = dateTime.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture);
        if (withTime) dateString += dateTime.ToString(" HH:mm", CultureInfo.CurrentCulture);

        // dateString = "0123456789: -a";

        var marginW = width * margin / 100;
        var maxW = (width - marginW * 2) * size / 100;
        var sepW = maxW / (dateString.Length + (dateString.Length - 1) * space / 100f);
        var sepM = sepW * space / 100;
        var sepH = sepW * 285 / 183;

        using SolidBrush sb = new(Color.FromArgb(alpha, color));
        for (var count = 0; count < dateString.Length; count++)
        {
            var c = dateString[dateString.Length - count - 1];
            RectangleF rect = new(width - marginW - sepM * count - sepW * (count + 1), height - marginW - sepH, sepW, sepH);
            DrawByteFlag(g, sb, rect, CharToBitFlag(c));
        }
    }

    private static byte CharToBitFlag(char c)
    {
        return c switch
        {
            '0' => 0x77,
            '1' => 0x24,
            '2' => 0x5d,
            '3' => 0x6d,
            '4' => 0x2e,
            '5' => 0x6b,
            '6' => 0x7b,
            '7' => 0x25,
            '8' => 0x7f,
            '9' => 0x6f,
            ':' => 0x80,
            '-' => 0x08,
            ' ' => 0x00,
            _ => 0x49,// '三'
        };
    }

    private static void DrawByteFlag(Graphics g, Brush brush, RectangleF rect, int bitFlag)
    {
        var x = rect.X;
        var y = rect.Y;
        var c = rect.Width / 183;

        // 中上
        if ((bitFlag & 0x01) != 0)
        {
            g.FillPolygon(brush, new PointF[] {
                    new PointF(x + 66 * c, y +  0 * c),
                    new PointF(x +166 * c, y +  0 * c),
                    new PointF(x +171 * c, y +  7 * c),
                    new PointF(x +139 * c, y + 35 * c),
                    new PointF(x + 80 * c, y + 35 * c),
                    new PointF(x + 59 * c, y +  6 * c) });
        }

        // 左上
        if ((bitFlag & 0x02) != 0)
        {
            g.FillPolygon(brush, new PointF[] {
                    new PointF(x + 50 * c, y + 16 * c),
                    new PointF(x + 73 * c, y + 41 * c),
                    new PointF(x + 60 * c, y +118 * c),
                    new PointF(x + 38 * c, y +136 * c),
                    new PointF(x + 25 * c, y +118 * c),
                    new PointF(x + 41 * c, y + 21 * c) });
        }

        // 右上
        if ((bitFlag & 0x04) != 0)
        {
            g.FillPolygon(brush, new PointF[] {
                    new PointF(x +178 * c, y + 16 * c),
                    new PointF(x +183 * c, y + 22 * c),
                    new PointF(x +167 * c, y +118 * c),
                    new PointF(x +147 * c, y +134 * c),
                    new PointF(x +133 * c, y +118 * c),
                    new PointF(x +146 * c, y + 42 * c) });
        }

        // 中央
        if ((bitFlag & 0x08) != 0)
        {
            g.FillPolygon(brush, new PointF[] {
                    new PointF(x + 64 * c, y +126 * c),
                    new PointF(x +123 * c, y +126 * c),
                    new PointF(x +138 * c, y +143 * c),
                    new PointF(x +118 * c, y +160 * c),
                    new PointF(x + 60 * c, y +160 * c),
                    new PointF(x + 45 * c, y +143 * c) });
        }

        // 左下
        if ((bitFlag & 0x10) != 0)
        {
            g.FillPolygon(brush, new PointF[] {
                    new PointF(x + 36 * c, y +150 * c),
                    new PointF(x + 50 * c, y +167 * c),
                    new PointF(x + 39 * c, y +242 * c),
                    new PointF(x +  5 * c, y +271 * c),
                    new PointF(x +  0 * c, y +262 * c),
                    new PointF(x + 16 * c, y +167 * c) });
        }

        // 右下
        if ((bitFlag & 0x20) != 0)
        {
            g.FillPolygon(brush, new PointF[] {
                    new PointF(x +145 * c, y +150 * c),
                    new PointF(x +158 * c, y +167 * c),
                    new PointF(x +142 * c, y +263 * c),
                    new PointF(x +133 * c, y +269 * c),
                    new PointF(x +110 * c, y +242 * c),
                    new PointF(x +123 * c, y +167 * c) });
        }

        // 中下
        if ((bitFlag & 0x40) != 0)
        {
            g.FillPolygon(brush, new PointF[] {
                    new PointF(x + 43 * c, y +251 * c),
                    new PointF(x +102 * c, y +251 * c),
                    new PointF(x +124 * c, y +277 * c),
                    new PointF(x +115 * c, y +285 * c),
                    new PointF(x + 17 * c, y +285 * c),
                    new PointF(x + 12 * c, y +277 * c) });
        }

        // :
        if ((bitFlag & 0x80) != 0)
        {
            g.FillEllipse(brush, new RectangleF(x + 85 * c, y + 58 * c, 42 * c, 42 * c));
            g.FillEllipse(brush, new RectangleF(x + 63 * c, y + 185 * c, 42 * c, 42 * c));
        }
    }
}
