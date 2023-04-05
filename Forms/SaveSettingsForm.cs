using Com.Nakasendo.Gakupetit.Forms;
using Com.Nakasendo.Gakupetit.Properties;
using System.Diagnostics;

namespace Com.Nakasendo.Gakupetit;

/// <summary>
/// 保存設定画面
/// </summary>
public partial class SaveSettingsForm : Form
{
    /// <summary>
    /// 設定
    /// </summary>
    private readonly Settings appSettings;


    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SaveSettingsForm(Settings appSettings)
    {
        InitializeComponent();
        this.appSettings = appSettings;
    }

    /// <summary>
    /// フォルダ指定ボタン
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TargetFolderButton_Click(object sender, EventArgs e)
    {
        // フォルダ指定ダイアログを開く
        if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;

        targetFolderRadioButton.Checked = true;
        targetFolderLabel.Text = folderBrowserDialog.SelectedPath;
        okButton.Enabled = true;
    }

    /// <summary>
    /// プレフィックスの指定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Prefix_Changed(object? sender, EventArgs e)
    {
        // ファイル名のチェック
        prefixTextBox.Text = ValidFileName(prefixTextBox.Text);

        // 番号を振りなおす場合
        if (isRenumberCheckBox.Checked)
        {
            exLabel.Text = Resources.SaveEx + prefixTextBox.Text + "1".PadLeft((int)digitNumUpDown.Value, '0') + extensionComboBox.Text;
        }
        else
        {
            exLabel.Text = Resources.SaveEx + prefixTextBox.Text + "(InputFileName)" + extensionComboBox.Text;
        }
        digitNumUpDown.Enabled = isRenumberCheckBox.Checked;
    }

    /// <summary>
    /// ファイル名をチェックし、置き換える。
    /// </summary>
    /// <param name="prefix">プレフィックス</param>
    /// <returns>変更済みプレフィックス</returns>
    private static string ValidFileName(string prefix)
    {

        var valid = prefix;
        var invalidch = Path.GetInvalidFileNameChars();

        foreach (var c in invalidch)
        {
            if (valid.Contains(c.ToString()))
            {
                var message = string.Format(Resources.SaveFileNameCheckMessage, c.ToString());
                MessageBox.Show(message, Resources.SaveFileNameCheckTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1, 0);
            }
            valid = valid.Replace(c, '_');
        }
        return valid;
    }

    /// <summary>
    /// 保存設定画面が開いたとき
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SaveSettingsForm_Load(object sender, EventArgs e)
    {
        // 設定を呼び戻す
        isRenumberCheckBox.Checked = appSettings.IsRenumber;
        prefixTextBox.Text = appSettings.Prefix;

        // 前回の保存フォルダ名を設定
        if (Directory.Exists(appSettings.PreviousFolder))
        {
            previousFolderLabel.Text = appSettings.PreviousFolder;
        }
        else
        {
            // なければデスクトップ
            previousFolderLabel.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        // 指定したフォルダ名を設定
        if (Directory.Exists(appSettings.TargetFolder))
        {
            targetFolderLabel.Text = appSettings.TargetFolder;
        }
        else
        {
            // なければマイピクチャ
            targetFolderLabel.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        }

        // ﾕｰｻﾞｰ定義/絵画用画像フォルダ名を設定
        if (Directory.Exists(appSettings.ImagesFolder))
        {
            imagesFolderLabel.Text = appSettings.ImagesFolder;
        }
        else
        {
            // なければExeの下
            imagesFolderLabel.Text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
        }

        // 前回のラジオボタンの設定
        switch (appSettings.SaveFolderType)
        {
            case 0:
                // 同じフォルダ
                sameFolderRadioButton.Checked = true;
                break;
            case 1:
                // 前回のフォルダ
                sameFolderRadioButton.Checked = true;
                break;
            case 2:
                // 指定したフォルダ
                targetFolderRadioButton.Checked = true;
                break;
        }

        extensionComboBox.SelectedItem = appSettings.Extention;

        // 例の反映
        Prefix_Changed(null, EventArgs.Empty);

        jpegQualityUpDown.Value = appSettings.JpegQuality;

        saveIniCheckBox.Checked = appSettings.SaveIni;

        defaultBackColorPictureBox.BackColor = appSettings.DefaultColor;

        digitNumUpDown.Value = appSettings.DigitNum;
    }

    /// <summary>
    /// 同じフォルダラジオボタンを選択
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SameFolderRadioButton_CheckedChanged(object sender, EventArgs e)
    {
        // 選択されなかったときは戻る
        if (!sameFolderRadioButton.Checked) return;

        // 設定を保存
        okButton.Enabled = true;
    }

    /// <summary>
    /// 以前のフォルダラジオボタンを選択
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PreviousFolderRadioButton_CheckedChanged(object sender, EventArgs e)
    {
        // 選択されなかったときは戻る
        if (!previousFolderRadioButton.Checked) return;

        // 設定を保存
        appSettings.SaveFolderType = 1;
        okButton.Enabled = true;
    }

    /// <summary>
    /// 指定したフォルダ
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TargetFolderRadioButton_CheckedChanged(object sender, EventArgs e)
    {
        // 選択されなかったときは戻る
        if (!targetFolderRadioButton.Checked) return;

        // 設定を保存
        appSettings.SaveFolderType = 2;
        okButton.Enabled = Directory.Exists(targetFolderLabel.Text);
    }

    /// <summary>
    /// 指定したフォルダを保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TargetFolderLabel_TextChanged(object sender, EventArgs e)
    {
        appSettings.TargetFolder = targetFolderLabel.Text;
    }

    /// <summary>
    /// OKボタンのクリック
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OkButton_Click(object sender, EventArgs e)
    {
        // 設定の保存
        if (sameFolderRadioButton.Checked) appSettings.SaveFolderType = 0;
        if (previousFolderRadioButton.Checked) appSettings.SaveFolderType = 1;
        if (targetFolderRadioButton.Checked) appSettings.SaveFolderType = 2;

        appSettings.PreviousFolder = previousFolderLabel.Text;
        appSettings.TargetFolder = targetFolderLabel.Text;
        appSettings.ImagesFolder = imagesFolderLabel.Text;
        appSettings.DigitNum = (int)digitNumUpDown.Value;
        appSettings.Prefix = prefixTextBox.Text;
        appSettings.Extention = extensionComboBox.Text;
        appSettings.IsRenumber = isRenumberCheckBox.Checked;
        appSettings.JpegQuality = (byte)jpegQualityUpDown.Value;
        appSettings.SaveIni = saveIniCheckBox.Checked;
    }

    /// <summary>
    /// デフォルトボタン
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DefaultButton_Click(object sender, EventArgs e)
    {
        isRenumberCheckBox.Checked = false;
        sameFolderRadioButton.Checked = true;
        digitNumUpDown.Value = 4;
        prefixTextBox.Text = "_";
        extensionComboBox.SelectedIndex = 0; // .jpg
        previousFolderLabel.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        targetFolderLabel.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        imagesFolderLabel.Text = AppDomain.CurrentDomain.BaseDirectory + @"\Images";
        jpegQualityUpDown.Value = 85;
        saveIniCheckBox.Checked = false;
        defaultBackColorPictureBox.BackColor = Color.White;
    }

    private void ColorButton_Click(object sender, EventArgs e)
    {
        // カラーダイアログ表示
        colorDialog.CustomColors = new int[] { ColorTranslator.ToWin32(defaultBackColorPictureBox.BackColor) };
        colorDialog.Color = defaultBackColorPictureBox.BackColor;

        if (colorDialog.ShowDialog() != DialogResult.OK) return;

        defaultBackColorPictureBox.BackColor = colorDialog.Color;
        appSettings.DefaultColor = colorDialog.Color;
    }

    private void DigitNumUpDown_ValueChanged(object sender, EventArgs e)
    {
        Prefix_Changed(sender, e);
    }

    private void ImagesFolderButton_Click(object sender, EventArgs e)
    {
        // フォルダ指定ダイアログを開く
        folderBrowserDialog.SelectedPath = appSettings.ImagesFolder;
        if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;

        // デフォルトの画像コピー確認
        var message = string.Format(Resources.SaveFileCopyCheckMessage, folderBrowserDialog.SelectedPath);
        var ret = MessageBox.Show(message, Resources.SaveFileCopyCheckTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        if (ret == DialogResult.OK)
        {
            try
            {
                foreach (var imageFilePath in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"\Images"))
                {
                    File.Copy(imageFilePath, $"{folderBrowserDialog.SelectedPath}\\{Path.GetFileName(imageFilePath)}", true);
                }
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format(Resources.SaveFileCopyErrorMessage, ex.Message);
                MessageBox.Show(errorMessage, Resources.SaveFIleCopyErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        imagesFolderLabel.Text = folderBrowserDialog.SelectedPath;
        appSettings.ImagesFolder = folderBrowserDialog.SelectedPath;
    }

    private void BtnOpen_Click(object sender, EventArgs e)
    {
        Process.Start("explorer.exe", imagesFolderLabel.Text);
    }
}