using System.Drawing.Drawing2D;

namespace Com.Nakasendo.Gakupetit.EffectEtc;

class Default : EffectBase, IEffect
{
    public Default(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId => -1;
    public string[] Names => new[] { "", "" };
    public bool IsBackChecked => true;
    public int DefaultValue => 10;
    public string[] Descriptions => new[] {
        $"Default effect. Displays the initial screen. Slider values are unused.",
        $"既定のエフェクトです。初期画面を表示します。スライダー値は未使用です。" };
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
            g.Clear(Color.Black);

            // ランダムで模様を描画
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Random rnd = new(DateTime.Now.Second);
            for (var i = 0; i < 70; i++)
            {
                var rndColor = Color.FromArgb(48, rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                using Pen p = new(rndColor, rnd.Next(10, 30) * 96 / g.DpiX);
                var l = rnd.Next(0, w);
                g.DrawLine(p, l, -1, l, h + 2);
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
}
