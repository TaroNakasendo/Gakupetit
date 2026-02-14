using System.Runtime.InteropServices;

namespace Com.Nakasendo.Gakupetit;

public partial class EffectForm : Form
{
    /// <summary>
    /// 現在選択されているものを、あらかじめ選択してフォームを開く
    /// </summary>
    /// <param name="effectNo"></param>
    public EffectForm(int effectNo)
    {
        SelectedEffect = effectNo;
        InitializeComponent();
    }

    public int SelectedEffect { get; private set; }

    /// <summary>
    /// 選択した番号を記憶
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EffectListView_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (effectListView.SelectedItems.Count == 0) return;
        SelectedEffect = effectListView.SelectedItems[0].Index;
    }

    /// <summary>
    /// リストのクリックでフォームを閉じる
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EffectListView_Click(object sender, EventArgs e) => Close();

    /// <summary>
    /// フォームロード時に、エフェクトを適用
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void EffectForm_Load(object sender, EventArgs e)
    {
        var mainForm = (MainForm)Owner!;

        SuspendLayout();

        var backColor = mainForm.GetBackColor();
        if (backColor == Color.Transparent) backColor = Color.White;
        effectListView.Visible = false;
        effectListView.BackColor = backColor;

        // テキストの色
        var textColor = MainForm.IslightColor(backColor) ? SystemColors.ControlText : Color.White;
        effectListView.ForeColor = textColor;

        // 高解像度対応
        var thumbSize = mainForm.ThumBitmap.Size;
        var mag = AutoScaleDimensions.Width / 96;
        thumbSize = new Size((int)(thumbSize.Width * mag) / 2, (int)(thumbSize.Height * mag) / 2);
        ImageList il = new() { ImageSize = thumbSize, ColorDepth = ColorDepth.Depth32Bit };

        try
        {
            for (int id = 0; id < MainForm.EffectNum; id++)
            {
                var (name, img, tooltip) = await mainForm.GetPreviewInfo(id);
                il.Images.Add(img);
                ListViewItem lvi = new(name)
                {
                    ToolTipText = tooltip,
                    ImageIndex = id
                };

                effectListView.Items.Add(lvi);
            }
            Task.WaitAll();
        }
        catch (Exception)
        {
        }

        effectListView.LargeImageList = il;
        effectListView.Items[SelectedEffect].Selected = true;

        // アイコン間隔
        var x = (int)(140 * mag);
        var y = (int)(120 * mag);
        SetIconSpacing(effectListView, x, y);

        // ウィンドウサイズの最適化
        var count = MainForm.EffectNum;
        var screen = Screen.GetWorkingArea(Owner ?? this);

        // 6列くらいがちょうどよい
        var columns = 6;
        var rows = (int)Math.Ceiling((double)count / columns);

        // 画面幅に収まらなければ列を減らす
        var layoutIterationSafety = 0;
        const int MaxLayoutIterations = 1000;
        while (columns * x > screen.Width * 0.9 && columns > 1 && layoutIterationSafety < MaxLayoutIterations)
        {
            columns--;
            rows = (int)Math.Ceiling((double)count / columns);
            layoutIterationSafety++;
        }

        // 画面高さに収まらなければ列を増やす
        layoutIterationSafety = 0;
        while (rows * y > screen.Height * 0.9 && columns * x < screen.Width * 0.9 && layoutIterationSafety < MaxLayoutIterations)
        {
            columns++;
            rows = (int)Math.Ceiling((double)count / columns);
            layoutIterationSafety++;
        }

        // 必要なクライアントサイズ (スクロールバーと境界線の分)
        var w = columns * x + SystemInformation.VerticalScrollBarWidth + 8;
        var h = rows * y + 5;

        // 最大サイズ制限
        w = Math.Min(w, (int)(screen.Width * 0.9));
        h = Math.Min(h, (int)(screen.Height * 0.9));

        ClientSize = new Size(w, h);
        effectListView.Dock = DockStyle.Fill;
        CenterToParent();

        effectListView.Visible = true;
        ResumeLayout();
    }

    /// <summary>
    /// アイコン間隔変更API呼び出し
    /// </summary>
    /// <param name="hWnd"></param>
    /// <param name="msg"></param>
    /// <param name="wParam"></param>
    /// <param name="lParam"></param>
    /// <returns></returns>
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// アイコン間隔の変更
    /// </summary>
    /// <param name="listview"></param>
    /// <param name="cx"></param>
    /// <param name="cy"></param>
    private static void SetIconSpacing(ListView listview, int cx, int cy)
    {
        const int LVM_FIRST = 0x1000;
        const int LVM_SETICONSPACING = LVM_FIRST + 53;
        int lParam = cy << 16 | cx;
        SendMessage(new HandleRef(listview, listview.Handle), LVM_SETICONSPACING, IntPtr.Zero, lParam);
    }

}
