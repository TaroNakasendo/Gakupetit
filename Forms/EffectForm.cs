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
