using Com.Nakasendo.Gakupetit.Properties;

namespace Com.Nakasendo.Gakupetit;

public partial class SizeForm : Form
{
    /// <summary>
    /// ビットマップの幅
    /// </summary>
    public int BmpWidth { get; set; }

    /// <summary>
    /// ビットマップの高さ
    /// </summary>
    public int BmpHeight { get; set; }

    /// <summary>
    /// リサイズモード
    /// </summary>
    public byte ResizeType { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SizeForm()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 読み込み時
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SizeForm_Load(object sender, EventArgs e)
    {
        oirginalLabel.Text = $"{BmpWidth}x{BmpHeight}";

        if (BmpHeight <= BmpWidth)
        {
            // 横長
            SetTextForLandScape();
        }
        else
        {
            // 縦長
            SetTextForPortlate();
        }

        // 長辺が1280より大きい場合
        var isLarge = 1280 < Math.Max(BmpWidth, BmpHeight);

        // 長辺は、1280を選択、小さい画像の場合は最大値
        listComboBox.SelectedIndex = isLarge ? listComboBox.Items.Count - 7 : 0;
        // 正方形は、800を選択、小さい画像の場合は最大値
        cubicComboBox.SelectedIndex = isLarge ? cubicComboBox.Items.Count - 10 : 0;
    }

    private void OKButton_Click(object sender, EventArgs e)
    {
        // オリジナル 
        ResizeType = 1;

        if (listRadioButton.Checked) // 一覧から選択
        {
            ResizeType = 2;
            var xIndex = listComboBox.Text.IndexOf('x');
            BmpWidth = int.Parse(listComboBox.Text[..xIndex]);
            BmpHeight = int.Parse(listComboBox.Text[(xIndex + 1)..]);
        }
        else if (autoHeightRadioButton.Checked) // 幅で指定
        {
            ResizeType = 3;
            try
            {
                BmpWidth = (int)autoHeightUpDown.Value;
                BmpHeight = int.Parse(autoHeightLabel.Text);
            }
            catch (FormatException)
            {
                BmpWidth = 640;
                BmpHeight = 480;
            }
        }
        else if (freeSizeRadioButton.Checked) // 自由なサイズ
        {
            ResizeType = 4;
            try
            {
                BmpWidth = (int)widthUpDown.Value;
                BmpHeight = (int)heightUpDown.Value;
            }
            catch (FormatException)
            {
                BmpWidth = 640;
                BmpHeight = 480;
            }
        }
        else if (cubicRadioButton.Checked) // 正方形
        {
            ResizeType = 5;
            var xIndex = cubicComboBox.Text.IndexOf('x');
            BmpWidth = int.Parse(cubicComboBox.Text[..xIndex]);
            BmpHeight = int.Parse(cubicComboBox.Text[(xIndex + 1)..]);
        }

        // 閉じる
        DialogResult = DialogResult.OK;
    }

    /// <summary>
    /// 横長用のリスト項目追加
    /// </summary>
    private void SetTextForLandScape()
    {
        var thresholds = new[] { 2560, 1920, 1440, 1280, 1024, 800, 640, 480, 320 };
        foreach (var threshold in thresholds)
        {
            if (BmpWidth > threshold) listComboBox.Items.Add($"{threshold}x{threshold * BmpHeight / BmpWidth}");
        }
        listComboBox.Items.Add($"100x{100 * BmpHeight / BmpWidth}");

        SetTextForCubic(BmpHeight);

        autoHeightUpDown.Value = 300;
        autoHeightLabel.Text = (300 * BmpHeight / BmpWidth).ToString();

        widthUpDown.Value = BmpWidth / 2;
        heightUpDown.Value = BmpHeight / 2;
    }

    /// <summary>
    /// 縦長用のリスト項目追加
    /// </summary>
    private void SetTextForPortlate()
    {
        var thresholds = new[] { 1920, 1600, 1200, 960, 800, 640, 600, 480, 320, 240 };
        foreach (var threshold in thresholds)
        {
            if (BmpHeight > threshold) listComboBox.Items.Add($"{threshold * BmpWidth / BmpHeight}x{threshold}");
        }
        listComboBox.Items.Add($"{100 * BmpWidth / BmpHeight}x100");

        SetTextForCubic(BmpWidth);

        autoHeightUpDown.Value = 300 * BmpWidth / BmpHeight;
        autoHeightLabel.Text = "300";

        widthUpDown.Value = BmpWidth / 2;
        heightUpDown.Value = BmpHeight / 2;
    }

    private void SetTextForCubic(int size)
    {
        cubicComboBox.Items.Add($"{size}x{size}");
        var thresholds = new[] { 2560, 1200, 1024, 800, 640, 600, 480, 300, 256, 180, 128, 64 };
        foreach (var threshold in thresholds)
        {
            if (size > threshold) cubicComboBox.Items.Add($"{threshold}x{threshold}");
        }
        cubicComboBox.Items.Add("32x32");
    }

    /// <summary>
    /// 水平指定の値が変更された場合
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AutoHeightUpDown_ValueChanged(object sender, EventArgs e)
    {
        int value = (int)autoHeightUpDown.Value * BmpHeight / BmpWidth;
        if (value == 0) value = 1;
        autoHeightLabel.Text = value.ToString();
    }


    /// <summary>
    /// 水平指定の値が入力された場合
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AutoHeightUpDown_KeyUp(object sender, KeyEventArgs e)
    {
        AutoHeightUpDown_ValueChanged(sender, e);
    }

    /// <summary>
    /// 高さ自動
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AutoHeightUpDown_Enter(object sender, EventArgs e)
    {
        autoHeightRadioButton.Checked = true;
    }

    /// <summary>
    /// 幅指定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void WidthHeightUpDown_Enter(object sender, EventArgs e)
    {
        freeSizeRadioButton.Checked = true;
    }

    /// <summary>
    /// リストから選択
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ListComboBox_Enter(object sender, EventArgs e)
    {
        listRadioButton.Checked = true;
    }

    /// <summary>
    /// 四角い切り抜きボタン用に、一覧から選択
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CubicComboBox_Enter(object sender, EventArgs e)
    {
        cubicRadioButton.Checked = true;
    }

    /// <summary>
    /// サイズタイプ名を取得します。
    /// </summary>
    /// <param name="sizeType">サイズタイプ</param>
    /// <returns>サイズタイプ名</returns>
    internal static string ResizeTypeToString(byte resizeType)
    {
        return resizeType switch
        {
            1 => Resources.SizeMode1,
            2 => Resources.SizeMode2,
            3 => Resources.SizeMode3,
            4 => Resources.SizeMode4,
            5 => Resources.SizeMode5,
            _ => Resources.SizeMode6,
        };
    }
}