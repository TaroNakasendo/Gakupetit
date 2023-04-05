using Com.Nakasendo.Gakupetit.Properties;
using ImageMagick;
using System.Drawing.Drawing2D;
using System.Text;

namespace Com.Nakasendo.Gakupetit.EffectEtc;

/// <summary>
/// ビットマップ制御クラス
/// </summary>
public sealed class BitmapEffects
{
    #region private インスタンス

    private Bitmap OrgBitmap { get; set; } = new(1024, 768); // 大元画像用

    #endregion

    #region プロパティ

    /// <summary>
    /// 大元画像サイズ
    /// </summary>
    internal Size OrgSize => OrgBitmap.Size;

    /// <summary>
    /// 画像ファイル名
    /// </summary>
    internal string LongFileName { get; set; } = "";

    /// <summary>
    /// 編集元画像を取得
    /// </summary>
    internal Bitmap srcBitmap = new(1024, 768);
    internal Bitmap SrcBitmap
    {
        get
        {
            return srcBitmap;
        }
        set
        {
            srcBitmap = value;
            SrcSize = value.Size;
        }
    }

    /// <summary>
    /// 編集元画像のサイズ
    /// </summary>
    internal Size SrcSize { get; private set; } = new(1024, 768);

    /// <summary>
    /// サムネイル画像を取得
    /// </summary>
    public Bitmap ThumBitmap { get; set; } = new(256, 192);

    /// <summary>
    /// 撮影日時
    /// </summary>
    public DateTime ShotDateTime { get; set; } = DateTime.Now;

    #endregion

    /// <summary>
    /// 画像の比率を保ったまま、最大サイズに納まるサイズを返す
    /// </summary>
    /// <param name="sourceSize">元の画像のサイズ</param>
    /// <param name="maxSize">最大サイズ, 未指定は256</param>
    /// <returns></returns>
    private static Size ClipSize(Size sourceSize, int maxSize = 256)
    {
        var w = sourceSize.Width;
        var h = sourceSize.Height;

        if (w > h) return new Size(maxSize, Math.Max(1, maxSize * h / w));

        return new Size(Math.Max(1, maxSize * w / h), maxSize);
    }

    /// <summary>
    /// 画像を開く
    /// </summary>
    /// <param name="img">開く画像</param>
    /// <returns>true:成功、false:失敗</returns>
    internal bool OpenImage(Image? image)
    {
        try
        {
            if (image != null)
            {
                OrgBitmap = new(image);
                SrcBitmap = new(image);
                ThumBitmap = new(image, ClipSize(image.Size));
                return true;
            }

            // イメージが指定されなかったときは、画像を読み込んで32bit化              
            if (!File.Exists(LongFileName))
            {
                MessageBox.Show(Resources.BitmapEffectsMessageCantOpenFile, Resources.BitmapEffectsMessageOpenFileError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error,
                     MessageBoxDefaultButton.Button1, 0);
                return false;
            }

            Image newImage;

            if (new[] { ".png", ".bmp", ".jpg", ".jpeg", ".tiff", ".tif", ".gif" }.Contains(Path.GetExtension(LongFileName).ToLower()))
            {
                // .NET標準で開ける形式はそのまま開く
                newImage = Image.FromFile(LongFileName);

                // 日時
                var s = newImage.PropertyItems.Where(s => s.Id == 0x9003 && s.Type == 2).FirstOrDefault()?.Value;
                ShotDateTime = s == null ? DateTime.Now : DateTime.ParseExact(Encoding.ASCII.GetString(s).Trim('\0'), "yyyy:MM:dd HH:mm:ss", null);
            }
            else
            {
                // 未知の形式はImageMagickで開いてみる
                using MagickImage img = new(LongFileName);
                using MemoryStream ms = new();
                img.Write(ms, MagickFormat.Bmp);
                newImage = Image.FromStream(ms);
                var s = img.GetExifProfile()?.Values.Where(v => v.Tag == ExifTag.DateTimeOriginal).FirstOrDefault()?.GetValue().ToString();
                ShotDateTime = s == null ? DateTime.Now : DateTime.ParseExact(s, "yyyy:MM:dd HH:mm:ss", null);
            }


            OrgBitmap = new(newImage);
            SrcBitmap = new(newImage);
            ThumBitmap = new(newImage, ClipSize(newImage.Size));
            newImage.Dispose();
        }
        catch (Exception e)
        {
            var msg = e is MagickMissingDelegateErrorException || e is OutOfMemoryException ?
                Resources.BitmapEffectsMessageFileFormatError : Resources.BitmapEffectsMessageCantOpenFile;

            MessageBox.Show(msg, Resources.BitmapEffectsMessageOpenFileError,
                MessageBoxButtons.OK, MessageBoxIcon.Error,
                 MessageBoxDefaultButton.Button1, 0);
            return false;
        }

        // 読み込み成功
        return true;
    }

    /// <summary>
    /// サイズ変更
    /// </summary>
    /// <param name="width">幅</param>
    /// <param name="height">高さ</param>
    /// <param name="sizeMode">画像モード</param>
    internal void DoSize(int width, int height, byte sizeMode)
    {
        if (sizeMode == 1)
        {
            //オリジナル
            SrcBitmap = new(OrgBitmap!);
            return;
        }

        int w = OrgBitmap.Width;
        int h = OrgBitmap.Height;

        Rectangle r;

        if (sizeMode == 5)
        {
            //正方形切抜き
            if (w > h)
            {
                r = new((w - h) / 2, 0, h, h);
            }
            else
            {
                r = new(0, (h - w) / 2, w, w);
            }
        }
        else //拡大縮小
        {
            r = new(0, 0, w, h);
        }

        // アスペクト比補正
        int calcedHeight = height;
        if (sizeMode == 2 || sizeMode == 3) calcedHeight = width * h / w;
        if (calcedHeight == 0) calcedHeight = 1;

        Bitmap newBitmap = new(width, calcedHeight);
        try
        {
            using var g = Graphics.FromImage(newBitmap);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(OrgBitmap, new Rectangle(0, 0, width, calcedHeight), r, GraphicsUnit.Pixel);
        }
        catch (Exception)
        {
            newBitmap.Dispose();
            throw;
        }
        SrcBitmap = newBitmap;
        ThumBitmap = new(SrcBitmap, ClipSize(SrcBitmap.Size));
    }
}
