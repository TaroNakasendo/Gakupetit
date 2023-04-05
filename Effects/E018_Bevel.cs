using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using static System.Math;

namespace Com.Nakasendo.Gakupetit.Effects;

class E018_Bevel : EffectBase, IEffect
{
    public E018_Bevel(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => 18;
    public string[] Names => new[] { "Bevel", "べベル" };
    public bool IsBackChecked => true;
    public int DefaultValue => 10;
    public string[] Descriptions => new[] {
        $"It's a semi-transparent bevel effect. Adjust bevel width with the slider.",
        $"半透明で立体的ななベベルを付加します。スライダーでベベル幅を変更できます。" };
    public Color GetDefaultColor(Color nowColor) => nowColor;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        // 0のときは元画像を返す
        if (v == 0) return srcBitmap;

        Bitmap bmp = new(srcBitmap);
        int w = bmp.Width;
        int h = bmp.Height;
        int m = Min(w, h) / 2 * v / 100;

        try
        {
            using var g = Graphics.FromImage(bmp); // 左
            using GraphicsPath gp = new(FillMode.Winding);
            gp.StartFigure();
            gp.AddLines(new Point[] { new Point(0, 0), new Point(0, h), new Point(m, h - m), new Point(m, m) });
            gp.CloseFigure();
            using SolidBrush sb = new(Color.FromArgb((byte)((color.R + 255) / 2), (byte)((color.G + 255) / 2), (byte)((color.B + 255) / 2)));
            g.FillPath(sb, gp);
            g.Clip = new(gp);
            ColorMatrix cm = new() { Matrix00 = 1, Matrix11 = 1, Matrix22 = 1, Matrix33 = 0.4f, Matrix44 = 1 };
            ImageAttributes ia = new();
            ia.SetColorMatrix(cm);
            g.DrawImage(srcBitmap, new Rectangle(0, 0, w, h), 0, 0, w, h, GraphicsUnit.Pixel, ia);

            using var g2 = Graphics.FromImage(bmp); // 上
            using GraphicsPath gp2 = new(FillMode.Winding);
            gp2.StartFigure();
            gp2.AddLines(new Point[] { new Point(0, 0), new Point(m, m), new Point(w - m, m), new Point(w, 0) });
            gp2.CloseFigure();
            using SolidBrush sb2 = new(Color.FromArgb((byte)((color.R + 255) / 2), (byte)((color.G + 255) / 2), (byte)((color.B + 255) / 2)));
            g2.FillPath(sb2, gp2);
            g2.Clip = new(gp2);
            ColorMatrix cm2 = new() { Matrix00 = 1, Matrix11 = 1, Matrix22 = 1, Matrix33 = 0.6f, Matrix44 = 1 };
            ImageAttributes ia2 = new();
            ia2.SetColorMatrix(cm2);
            g2.DrawImage(srcBitmap, new Rectangle(0, 0, w, h), 0, 0, w, h, GraphicsUnit.Pixel, ia2);

            using var g3 = Graphics.FromImage(bmp); // 下
            using GraphicsPath gp3 = new(FillMode.Winding);
            gp3.StartFigure();
            gp3.AddLines(new Point[] { new Point(0, h), new Point(m, h - m), new Point(w - m, h - m), new Point(w, h) });
            gp3.CloseFigure();
            using SolidBrush sb3 = new(Color.FromArgb(127, color.R / 2, color.G / 2, color.B / 2));
            g3.FillPath(sb3, gp3);

            using var g4 = Graphics.FromImage(bmp); // 右
            using GraphicsPath gp4 = new(FillMode.Winding);
            gp4.StartFigure();
            gp4.AddLines(new Point[] { new Point(w - m, h - m), new Point(w - m, m), new Point(w, 0), new Point(w, h) });
            gp4.CloseFigure();
            using SolidBrush sb4 = new(Color.FromArgb(160, color.R / 3, color.G / 3, color.B / 3));
            g4.FillPath(sb4, gp4);
        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }
        return bmp;
    }
}
