using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;
using static System.Math;

namespace Com.Nakasendo.Gakupetit.Effects;

class E015_Film : EffectBase, IEffect
{
    public E015_Film(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 15;
    public string[] Names => new[] { "Film", "フィルム" };
    public bool IsBackChecked => true;
    public int DefaultValue => 52;
    public string[] Descriptions => new[] {
        $"It's a photo film-like frame effect. Adjust the frame size with the slider.",
        $"写真フィルムのような枠をつけるエフェクトです。スライダーで枠の大きさを変更できます。" };
    public Color GetDefaultColor(Color nowColor) => nowColor;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        Bitmap bmp = new(srcBitmap);

        try
        {
            var version = Application.ProductVersion[0];

            var s = $"{Application.ProductName.ToUpper()}!{version} FILM        ©2005-{BitmapEffects.ShotDateTime.Year}";

            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            float wPitch = (v + 1) * bmp.Width / 100f;
            float hPitch = (v + 1) * bmp.Height / 100f;

            if (bmp.Height < bmp.Width)
            {
                // 横長
                g.FillRectangle(Brushes.Black, -0.5f, 0, wPitch / 5, bmp.Height);
                g.FillRectangle(Brushes.Black, bmp.Width - wPitch / 5, 0, wPitch / 5, bmp.Height);
                g.FillRectangle(Brushes.Black, 0, -0.5f, bmp.Width, hPitch / 20);
                g.FillRectangle(Brushes.Black, 0, bmp.Height - hPitch / 20, bmp.Width, hPitch / 20);

                float sPitch = wPitch / 20;
                for (float i = -sPitch; i < bmp.Height + sPitch * 2; i += sPitch * 3)
                {
                    RectangleF rect = new(sPitch, i, sPitch * 12 / 5, sPitch * 3 / 2);
                    DrawRoundRect(g, color, rect);
                    rect = new(bmp.Width - sPitch * 7 / 2, i, sPitch * 12 / 5, sPitch * 3 / 2);
                    DrawRoundRect(g, color, rect);
                }

                g.TranslateTransform(sPitch, bmp.Height / 10);
                g.RotateTransform(90);
                DrawString(g, sPitch, s);

                g.ResetTransform();
                g.TranslateTransform(bmp.Width, bmp.Height / 2);
                g.RotateTransform(90);
                s = Path.GetFileNameWithoutExtension(BitmapEffects.LongFileName);
                DrawString(g, sPitch, s);
            }
            else
            {
                // 縦長
                g.FillRectangle(Brushes.Black, 0, -0.5f, bmp.Width, hPitch / 5);
                g.FillRectangle(Brushes.Black, 0, bmp.Height - hPitch / 5, bmp.Width, hPitch / 5);
                g.FillRectangle(Brushes.Black, -0.5f, 0, wPitch / 20, bmp.Height);
                g.FillRectangle(Brushes.Black, bmp.Width - wPitch / 20, 0, wPitch / 20, bmp.Height);

                var sPitch = hPitch / 20;
                for (var i = -sPitch; i < bmp.Width + sPitch * 2; i += sPitch * 3)
                {
                    RectangleF rect = new(i, sPitch, sPitch * 3 / 2, sPitch * 12 / 5);
                    DrawRoundRect(g, color, rect);
                    rect = new(i, bmp.Height - sPitch * 7 / 2, sPitch * 3 / 2, sPitch * 12 / 5);
                    DrawRoundRect(g, color, rect);
                }

                g.TranslateTransform(bmp.Width / 10, 0);
                DrawString(g, sPitch, s);

                g.TranslateTransform(bmp.Width / 2, bmp.Height - sPitch);
                s = Path.GetFileNameWithoutExtension(BitmapEffects.LongFileName);
                DrawString(g, sPitch, s);
            }
        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }
        return bmp;
    }

    static private void DrawString(Graphics g, float sPitch, string s)
    {
        if (sPitch / 2 < 1) sPitch = 1;

        using Font f = new(FontFamily.GenericMonospace, sPitch * 0.7f * 96 / g.DpiX);
        using SolidBrush sb = new(Color.OrangeRed);
        g.DrawString(s, f, sb, new Point(0, 0));
    }

    static private void DrawRoundRect(Graphics g, Color color, RectangleF rect)
    {
        // 小さいほうにあわせる
        var r = Min(rect.Width, rect.Height) / 2;

        // 枠を作成
        using GraphicsPath gp = new(FillMode.Winding);
        gp.StartFigure();
        gp.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90);  // 右下
        gp.AddArc(rect.Left, rect.Bottom - r, r, r, 90, 90);      // 左下
        gp.AddArc(rect.Left, rect.Top, r, r, 180, 90);            // 左上
        gp.AddArc(rect.Right - r, rect.Top, r, r, 270, 90);       // 右上
        gp.CloseFigure();

        // 塗りつぶし
        using SolidBrush sb = new(color);
        g.FillPath(sb, gp);
    }
}
