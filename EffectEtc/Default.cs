using System.Drawing.Drawing2D;

namespace Com.Nakasendo.Gakupetit.EffectEtc;

class Default(BitmapEffects bitmapEffects) : EffectBase(bitmapEffects), IEffect
{
    public int EffectId => -1;
    public string[] Names => ["", ""];
    public bool IsBackChecked => true;
    public int DefaultValue => 10;
    public string[] Descriptions => [
        $"Default effect. Displays the initial screen. Slider values are unused.",
        $"既定のエフェクトです。初期画面を表示します。スライダー値は未使用です。" ];
    public Color GetDefaultColor(Color nowColor) => nowColor;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        // 画像の大きさ
        var w = srcBitmap.Width;
        var h = srcBitmap.Height;

        // 画像を作成
        Bitmap bmp = new(w, h);
        try
        {
            using var g = Graphics.FromImage(bmp);
            using (var bgBrush = new LinearGradientBrush(
                new Rectangle(0, 0, w, h),
                Color.Black,
                Color.FromArgb(10, 12, 48),
                LinearGradientMode.Vertical))
            {
                var blend = new ColorBlend
                {
                    Colors = [Color.Black, Color.FromArgb(10, 12, 48), Color.FromArgb(200, 60, 32)],
                    Positions = new[] { 0f, 0.65f, 1f }
                };
                bgBrush.InterpolationColors = blend;
                g.FillRectangle(bgBrush, 0, 0, w, h);
            }

            // ランダムで模様を描画
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Random rnd = new(DateTime.Now.Second);
            var meteorCount = rnd.Next(6, 10);
            for (var m = 0; m < meteorCount; m++)
            {
                var startX = rnd.Next((int)(w * 0.65f), w);
                var startY = rnd.Next((int)(h * 0.65f), h);
                var endX = rnd.Next(0, (int)(w * 0.25f));
                var endY = rnd.Next(0, (int)(h * 0.25f));
                var steps = rnd.Next(10, 16);
                var baseOuter = rnd.Next(24, 44) * 96 / g.DpiX;
                var baseInner = baseOuter * rnd.Next(30, 55) / 100f;
                var startAlpha = rnd.Next(170, 230);
                var endAlpha = rnd.Next(60, 110);
                var startValue = 1.0f;
                var endValue = 0.65f;
                for (var i = 0; i < steps; i++)
                {
                    var t = i / (float)(steps - 1);
                    var centerX = startX + (endX - startX) * t;
                    var centerY = startY + (endY - startY) * t;
                    var outerRadius = baseOuter * (1f - (t * 0.65f));
                    var innerRadius = baseInner * (1f - (t * 0.65f));
                    var alpha = (int)(startAlpha + (endAlpha - startAlpha) * t);
                    var value = startValue + (endValue - startValue) * t;
                    var stepColor = CreateVividColor(rnd, alpha, value);
                    using Brush stepBrush = new SolidBrush(stepColor);
                    var rotation = (float)(rnd.NextDouble() * Math.PI * 2);
                    var points = CreateStarPoints(centerX, centerY, outerRadius, innerRadius, rotation);
                    g.FillPolygon(stepBrush, points);
                }
            }

            using StringFormat sf = new();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            using Font font = new("Tahoma", 64 * 96 / g.DpiX);

            var version = Application.ProductVersion[0];
            var trademark = $"{Application.ProductName}!{version}";
            g.DrawString(trademark, font, Brushes.White, new Rectangle(0, 0, w, h), sf);

            using Font font2 = new("Arial", 12 * 96 / g.DpiX);
            g.DrawString("The Simple Picture Framing Editor", font2, Brushes.White, new Rectangle(0, 0, w, h / 2), sf);

            var lastYear = File.GetLastWriteTime(Environment.GetCommandLineArgs()[0]).Year;
            var asmcpy = $"©2005 - {lastYear} Seedea Software Development";
            g.DrawString(asmcpy + " - Taro Nakasendo", font2, Brushes.White, new Rectangle(0, h / 2, w, h / 2), sf);
            g.DrawString("https://nakasendo.com/seedea/", font2, Brushes.White, new Rectangle(0, h / 2 + 50, w, h / 2 - 50), sf);

        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }

        // 生成した画像を返す
        return bmp;
    }

    private static PointF[] CreateStarPoints(float centerX, float centerY, float outerRadius, float innerRadius, float rotation)
    {
        const int points = 5;
        var result = new PointF[points * 2];
        var angleStep = MathF.PI / points;
        for (var i = 0; i < result.Length; i++)
        {
            var radius = i % 2 == 0 ? outerRadius : innerRadius;
            var angle = rotation + (i * angleStep);
            result[i] = new PointF(
                centerX + MathF.Cos(angle) * radius,
                centerY + MathF.Sin(angle) * radius);
        }

        return result;
    }

    private static Color CreateVividColor(Random rnd, int alpha, float value)
    {
        var h = rnd.NextSingle();
        var s = 0.9f;
        var v = value;
        var i = (int)(h * 6f);
        var f = (h * 6f) - i;
        var p = v * (1f - s);
        var q = v * (1f - f * s);
        var t = v * (1f - (1f - f) * s);
        var (r, g, b) = (i % 6) switch
        {
            0 => (v, t, p),
            1 => (q, v, p),
            2 => (p, v, t),
            3 => (p, q, v),
            4 => (t, p, v),
            _ => (v, p, q)
        };

        return Color.FromArgb(alpha, (int)(r * 255), (int)(g * 255), (int)(b * 255));
    }
}











