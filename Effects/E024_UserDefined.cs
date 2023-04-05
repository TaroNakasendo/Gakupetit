using Com.Nakasendo.Gakupetit.EffectEtc;
using System.Drawing.Drawing2D;

namespace Com.Nakasendo.Gakupetit.Effects;

class E024_UserDefined : EffectBase, IEffect
{
    public E024_UserDefined(BitmapEffects bitmapEffects) : base(bitmapEffects) { }

    public int EffectId { get; set; } = 24;
    public string[] Names => new[] { $"User defined {CustomId}", $"ﾕｰｻﾞｰ定義 {CustomId}" };
    public bool IsBackChecked => true;
    public int DefaultValue { get; set; } = 0;
    public string[] Descriptions => new[] {
        $"It's user custom frame effect No. {CustomId}. Adjust image blur with the slider.",
        $"ユーザーが画像で枠を定義できるエフェクトその{CustomId}です。スライダーで画像のぼけ具合を変更できます。" };
    public Color GetDefaultColor(Color nowColor) => (EffectId == 25) ? Color.White : nowColor;
    public string ImagesFolder { get; set; } = @".\Images";

    private int CustomId => EffectId - 24 + 1;

    public Bitmap DoEffect(int v, Color color, Bitmap srcBitmap)
    {
        var w = srcBitmap.Width;
        var h = srcBitmap.Height;

        Bitmap bmp = new(srcBitmap);

        try
        {
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var max = w > h ? h : w;
            var scale = (max - 4 * (v * max / 10 / 100 + 1)) / (float)max;
            g.TranslateTransform(-w * scale / 2 + w / 2, -h * scale / 2 + h / 2);
            g.ScaleTransform(scale, scale);

            var maskFile = $@"{ImagesFolder}\UserDefined{CustomId}.png";
            if (!File.Exists(maskFile))
            {
                maskFile = $@".\Images\UserDefined{CustomId}.png";
            }
            if (File.Exists(maskFile))
            {
                using var imageMask = Image.FromFile(maskFile);
                g.DrawImage(imageMask, 0, 0, w, h);
            }
            else
            {
                g.Clear(Color.White);
                using Pen p = new(Color.Red, 10);
                g.DrawLine(p, 0, 0, w, h);
                g.DrawLine(p, 0, h, w, 0);
                using Font font = new("Tahoma", 14);
                g.DrawString($"Image not found.", font, Brushes.Red, 0, 0);
            }

            // ガウスぼかしとする
            bmp = Blur.BlurMask(bmp, w, h, v);
            bmp = Masking(v, color, srcBitmap, bmp);
        }
        catch (Exception)
        {
            bmp.Dispose();
            throw;
        }

        return bmp;
    }
}
