using System.Xml.Serialization;

namespace Com.Nakasendo.Gakupetit.Forms;

class SettingManage : IDisposable
{

    // シリアライザ
    readonly XmlSerializer serializer;

    // ファイルストリーム
    FileStream? fs;

    // ファイル名
    string fileName = "";

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SettingManage()
    {

        XmlRootAttribute xRoot = new()
        {
            ElementName = "Settings",
            Namespace = "Gakupetit",
            IsNullable = true
        };
        serializer = new(typeof(Settings), xRoot);
    }

    /// <summary>
    /// 書き込み
    /// </summary>
    internal void Write(Settings settings)
    {
        if (File.Exists(fileName))
        {
            fs!.Close();
            fs.Dispose();
            fs = null;
        }

        if (settings.SaveIni)
        {
            // 設定を保存
            fs = new(fileName, FileMode.Create);
            serializer.Serialize(fs, settings);
        }
        else if (File.Exists(fileName))
        {
            // 設定を破棄
            File.Delete(fileName);
        }
    }

    /// <summary>
    /// 読み込み
    /// </summary>
    internal Settings Read(string xmlFileName)
    {
        fileName = xmlFileName;

        // 初回起動
        if (!File.Exists(xmlFileName)) return new Settings();

        // ファイルを開く
        fs = new(xmlFileName, FileMode.Open);

        try
        {
            // 成功
            return (Settings)serializer.Deserialize(fs!)!;
        }
        catch (InvalidOperationException)
        {
            // 読み込み失敗
            return new Settings();
        }
    }

    public void Dispose()
    {
        if (fs != null)
        {
            fs.Close();
            fs.Dispose();
        }
        GC.SuppressFinalize(this);
    }
}
