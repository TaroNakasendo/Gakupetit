using System.ComponentModel;
using System.Xml.Serialization;

namespace Com.Nakasendo.Gakupetit.Forms;

/// <summary>
/// 設定保存用クラス
/// </summary>
public class Settings
{
    /// <summary>
    /// 枠の種類
    /// </summary>
    public int Effect { get; set; } = 2; // ガウスぼかし

    /// <summary>
    /// 設定値
    /// </summary>
    public int Value { get; set; } = 40;

    /// <summary>
    /// 幅
    /// </summary>
    public int Width { get; set; } = 1024;

    /// <summary>
    /// 高さ
    /// </summary>
    public int Height { get; set; } = 768;

    /// <summary>
    /// デフォルト背景色
    /// </summary>
    [XmlIgnore]
    public Color DefaultColor { get; set; } = Color.White;

    /// <summary>
    /// デフォルト背景色(保存用)
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [XmlElement("DefaultBackColor")]
    public string DefaultBackColor { get { return ColorTranslator.ToHtml(DefaultColor); } set { DefaultColor = ColorTranslator.FromHtml(value); } }

    /// <summary>
    /// 背景色
    /// </summary>
    [XmlIgnore]
    public Color Color { get; set; } = Color.White;

    /// <summary>
    /// 背景色(保存用)
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [XmlElement("BackColor")]
    public string BackColor { get { return ColorTranslator.ToHtml(Color); } set { Color = ColorTranslator.FromHtml(value); } }

    /// <summary>
    /// リサイズの種類
    /// </summary>
    public byte ResizeType { get; set; } = 1;

    /// <summary>
    /// 保存フォルダの種類
    /// </summary>
    public byte SaveFolderType { get; set; } = 0;

    /// <summary>
    /// 前回の保存フォルダ
    /// </summary>
    public string PreviousFolder { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

    /// <summary>
    /// 指定した保存フォルダ
    /// </summary>
    public string TargetFolder { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

    /// <summary>
    /// ﾕｰｻﾞｰ定義/絵画用画像フォルダ
    /// </summary>
    public string ImagesFolder { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

    /// <summary>
    /// リナンバーするか？
    /// </summary>
    public bool IsRenumber { get; set; } = false;

    /// <summary>
    /// プレフィックス
    /// </summary>
    public string Prefix { get; set; } = "_";

    /// <summary>
    /// ファイル名の数値桁数
    /// </summary>
    public int DigitNum { get; set; } = 4;

    /// <summary>
    /// 拡張子
    /// </summary>
    public string Extention { get; set; } = ".jpg";

    /// <summary>
    /// JPEG画質
    /// </summary>
    public byte JpegQuality { get; set; } = 85;

    /// <summary>
    /// 背景色の表示
    /// </summary>
    public bool DispBackColor { get; set; } = true;

    /// <summary>
    /// 設定ファイルの保存
    /// </summary>
    public bool SaveIni { get; set; } = false;
}
