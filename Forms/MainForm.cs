using Com.Nakasendo.Gakupetit.EffectEtc;
using Com.Nakasendo.Gakupetit.Effects;
using Com.Nakasendo.Gakupetit.Forms;
using Com.Nakasendo.Gakupetit.Properties;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

[assembly: CLSCompliant(true)]
namespace Com.Nakasendo.Gakupetit;

internal partial class MainForm : Form
{
    // 余白のサイズ
    private int wakuWidth;
    private int wakuHeight;

    // 一時記憶用
    private string tmpTitleText = "";

    // 連続処理用
    private bool isAbort = false;

    // 時間計測用
    private readonly Stopwatch stopwatch = new();

    // 再入防止用
    private readonly object lockObject = new();

    // 現在の言語のIndex (0:en-US, 1:ja-JP)
    internal int languageIndex = 0;

    // スレッド上限カウント用
    private int doCount = 0;

    /// <summary>
    /// 設定
    /// </summary>
    private readonly SettingManage settingManage = new();
    private readonly Settings appSettings;

    // ビットマップ操作用
    private readonly BitmapEffects bitmapEffects = new();
    private IEffect effect;
    internal Bitmap ThumBitmap => bitmapEffects.ThumBitmap;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    internal MainForm()
    {
        InitializeComponent();

        // 設定の読み出し
        var xmlPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Gakupetit.ini");
        appSettings = settingManage.Read(xmlPath);

        effect = new Default(bitmapEffects);
    }

    /// <summary>
    /// ファイルを複数開いて処理を行う
    /// </summary>
    /// <param name="fileNames">複数のファイル名</param>
    private async void ProcImageFiles(string[] fileNames)
    {
        // 連続処理確認
        StringBuilder sb = new();

        if (fileNames.Length < 10)
        {
            // ファイル数が10個以下の場合      
            fileNames.ToList().ForEach((f) => sb.AppendLine(Path.GetFileName(f)));
            sb.AppendLine(Resources.MainProcImageFiles);
        }
        else
        {
            // ファイル数が多い場合
            sb.AppendLine(string.Format(Resources.MainProcImageManyFiles, fileNames.Length));
        }

        var outFolder = appSettings.SaveFolderType switch
        {
            0 => Path.GetDirectoryName(fileNames[0]),  // 開いたファイルと同じフォルダ
            1 => appSettings.PreviousFolder,           // 前回のフォルダ
            2 => appSettings.TargetFolder,             // 指定したフォルダ
            _ => throw new InvalidOperationException()
        };

        sb.AppendLine();
        sb.AppendLine(Resources.MainOriginalImageFolder + Path.GetDirectoryName(fileNames[0]));
        sb.AppendLine(Resources.MainSaveImageFolder + outFolder);
        sb.AppendLine(Resources.MainFrameColor + appSettings.BackColor);
        sb.AppendLine(Resources.MainFrameType + effect.Names[languageIndex]);
        sb.AppendLine(Resources.MainValue + $"{appSettings.Value}");
        sb.AppendLine(Resources.MainImageSize + $"{appSettings.Width}x{appSettings.Height}");
        sb.AppendLine(Resources.MainResizeType + SizeForm.ResizeTypeToString(appSettings.ResizeType));
        var yesNo = appSettings.IsRenumber ? Resources.Yes : Resources.No;
        sb.AppendLine(Resources.MainRenumber + yesNo);
        sb.AppendLine(Resources.MainPrefix + appSettings.Prefix);
        sb.AppendLine(Resources.MainExtention + appSettings.Extention);
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine(Resources.MainYes);
        sb.AppendLine(Resources.MainNo);
        sb.AppendLine(Resources.MainCancel);

        // 確認ダイアログを表示
        var dialogResult = MessageBox.Show(
            sb.ToString(),
            Resources.MainConfirmProcessing,
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button1,
            0);

        if (dialogResult == DialogResult.Cancel)
        {
            // キャンセル時はファイルを1つだけ開く
            string[] openFileNames = { fileNames[0] };
            OpenFileNames(openFileNames);
            return;
        }
        else if (dialogResult == DialogResult.No)
        {
            // 設定ファイルを開いたあと、処理を継続
            SaveSettingsMenuItem_Click(null, EventArgs.Empty);
            ProcImageFiles(fileNames);
            return;
        }

        // 連続処理コントロールの表示
        stopButton.Visible = true;
        progressBar.Visible = true;
        isAbort = false;
        var max = fileNames.GetLength(0);
        var num = 0;

        // 連続処理実行
        foreach (var fileName in fileNames)
        {
            // 停止ボタンが押されていたら抜ける
            if (isAbort) break;

            try
            {
                bitmapEffects.LongFileName = fileName;
                if (!bitmapEffects.OpenImage(null))
                {
                    var message = string.Format(Resources.MainOutOfMemoryException, fileName);
                    if (MessageBox.Show(message, Resources.MainConfirmProcessing,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                     MessageBoxDefaultButton.Button1, 0) == DialogResult.No)
                    {
                        // 連続処理コントロールの非表示
                        stopButton.Visible = false;
                        progressBar.Visible = false;
                        progressBar.Value = 0;
                        isAbort = false;
                        return;
                    }
                    continue;
                }
            }
            catch (OutOfMemoryException)
            {
                var message = string.Format(Resources.MainOutOfMemoryException, fileName);
                if (MessageBox.Show(message, Resources.MainConfirmProcessing,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                 MessageBoxDefaultButton.Button1, 0) == DialogResult.No)
                {
                    // 連続処理コントロールの非表示
                    stopButton.Visible = false;
                    progressBar.Visible = false;
                    progressBar.Value = 0;
                    isAbort = false;
                    return;
                }
                continue;
            }
            catch (FileNotFoundException)
            {
                var message = string.Format(Resources.MainFileNotFoundException, fileName);
                // 「いいえ」の場合は中止
                if (MessageBox.Show(message, Resources.MainConfirmProcessing,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                 MessageBoxDefaultButton.Button1, 0) == DialogResult.No)
                {
                    // 連続処理コントロールの非表示
                    stopButton.Visible = false;
                    progressBar.Visible = false;
                    progressBar.Value = 0;
                    isAbort = false;
                    return;
                }
                continue;
            }

            // サイズ変更
            DoSize();

            // エフェクト処理
            _ = await MainDoEffect();

            // 画像表示
            progressBar.Value = (++num) * 100 / max;
            Application.DoEvents();

            // 保存処理
            Save(null, EventArgs.Empty);
        }

        // 連続処理コントロールの非表示
        stopButton.Visible = false;
        progressBar.Visible = false;
        progressBar.Value = 0;
        isAbort = false;

        // 完了通知
        var text = string.Format(Resources.MainProcessed, num, appSettings.PreviousFolder);
        MessageBox.Show(text, Resources.Gakupetit, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 0);

        // フォルダを開く
        Process.Start("explorer.exe", appSettings.PreviousFolder);
    }

    /// <summary>
    /// 枠メニューを動的に作成
    /// </summary>
    private async void AddEffectMenu()
    {
        for (int id = 0; id < EffectNum; id++)
        {
            var (name, img, tooltip) = await GetPreviewInfo(id, forMenu: true);

            // アクセラレータ
            var acc = id < 10 ? id.ToString() : ((char)(id + 55)).ToString();
            ToolStripMenuItem tsmItem = new(name + "(&" + acc + ")", img)
            {
                Tag = id,
            };
            tsmItem.Click += EffectMenuItem_Click!;
            effectMenuItem.DropDownItems.Add(tsmItem);
        }
        Task.WaitAll();
    }

    /// <summary>
    /// メインフォームが読み込まれたとき
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MainForm_Load(object sender, EventArgs e)
    {

        // カラーピッカのカーソル設定
        var assembly = Assembly.GetExecutingAssembly();
        pickupPictureBox.Cursor =
            new Cursor(assembly.GetManifestResourceStream("Com.Nakasendo.Gakupetit.Forms.cursor.cur")!);

        // コマンドライン引数
        var fileNames = Environment.GetCommandLineArgs()[1..];

        // ファイルを開く
        OpenFileNames(fileNames);

        // バックカラー
        backCheckBox.Checked = appSettings.DispBackColor;

        // DPI変更対応
        SetItemsForDpi();


        // 色を変更
        SetColor(appSettings.DefaultColor);

        // 言語を取得
        languageIndex = CultureInfo.CurrentUICulture.ToString() == "ja-JP" ? 1 : 0;

        // メニュー追加
        AddEffectMenu();
    }

    /// <summary>
    /// フォームのDPIが変更されば場合のイベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MainForm_DpiChanged(object sender, DpiChangedEventArgs e) => SetItemsForDpi();

    private void SetItemsForDpi()
    {
        MinimumSize = new Size(707 * DeviceDpi / 96, 300 * DeviceDpi / 96);

        // 高解像度対応
        List<Button> buttons = new() { openButton, sizeButton, effectButton, colorButton, saveButton };

        var s = (int)(DeviceDpi / 2.5f);

        SuspendLayout();
        for (int i = 0; i < buttons.Count; i++)
        {
            var bmp = new Bitmap(HighDpiButtonImageList.Images[i], s, s);
            buttons[i].Image = bmp;
        }

        ResumeLayout();

        // 変更後の倍率
        var nextMag = DeviceDpi / AutoScaleDimensions.Width;
        wakuWidth = (int)((Width - mainPictureBox.Width) * nextMag);
        wakuHeight = (int)((Height - mainPictureBox.Height) * nextMag);
        // 微調整 (まだ、何度も繰り返すとずれてくる)
        wakuWidth += nextMag < 1 ? -2 : 1 < nextMag ? 5 : 0;
        wakuHeight += nextMag < 1 ? -2 : 1 < nextMag ? 5 : 0;
        SetTitle();
    }

    /// <summary>
    /// ドラッグ受付処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MainForm_DragEnter(object sender, DragEventArgs e)
    {
        // ファイルのみコピーを行う
        var isFile = e.Data!.GetDataPresent(DataFormats.FileDrop);
        e.Effect = isFile ? DragDropEffects.Copy : DragDropEffects.None;
    }

    /// <summary>
    /// ドラッグアンドドロップ処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MainForm_DragDrop(object sender, DragEventArgs e)
    {
        // ファイル名を取得
        var fileNames = (string[])e.Data!.GetData(DataFormats.FileDrop, false)!;

        // ファイルを開く
        OpenFileNames(fileNames);
    }

    /// <summary>
    /// 画像を開く
    /// </summary>
    private async void OpenImage(Image? image)
    {
        // ビットマップを開く
        if (!bitmapEffects.OpenImage(image)) return;

        appSettings.Width = bitmapEffects.OrgSize.Width;
        appSettings.Height = bitmapEffects.OrgSize.Height;

        // フォームサイズ変更
        ChangeFormSize();

        // フォームタイトルを設定
        SetTitle();

        // エフェクトを実行
        _ = await MainDoEffect();
    }

    /// <summary>
    /// フォームの高さと幅をビットマップに合わせて変更します
    /// </summary>
    private void ChangeFormSize()
    {
        // 画面に表示できる最大サイズ
        var margin = 20;

        var maxWidth = Screen.GetWorkingArea(this).Right - Left - wakuWidth - margin;
        var maxHeight = Screen.GetWorkingArea(this).Bottom - Top - wakuHeight - margin;

        // 元の画像サイズ
        var originalWidth = bitmapEffects.SrcSize.Width;
        var originalHeight = bitmapEffects.SrcSize.Height;

        // 表示サイズ
        var dispWidth = originalWidth;
        var dispHeight = originalHeight;

        // 表示サイズを決定
        if (originalWidth > originalHeight)
        {
            // 横長
            if (maxWidth <= originalWidth)
            {
                // 横長オーバー
                dispWidth = maxWidth;
                dispHeight = maxWidth * originalHeight / originalWidth;
            }

            if (maxHeight <= dispHeight)
            {
                // 縦長オーバー
                dispHeight = maxHeight;
                dispWidth = dispHeight * originalWidth / originalHeight;
            }
        }
        else
        {
            // 縦長
            if (maxHeight <= originalHeight)
            {
                // 縦長オーバー
                dispHeight = maxHeight;
                dispWidth = maxHeight * originalWidth / originalHeight;
            }

            if (maxWidth <= dispWidth)
            {
                // 横長オーバー
                dispWidth = maxWidth;
                dispHeight = dispWidth * originalHeight / originalWidth;
            }

        }

        SuspendLayout();
        lock (lockObject)
        {
            mainPictureBox.Image = new Bitmap(bitmapEffects.SrcBitmap!);
        }

        // ウィンドウサイズ調整
        Size = new(dispWidth + wakuWidth, dispHeight + wakuHeight);

        ResumeLayout();
    }

    /// <summary>
    /// 存在するパスを返す
    /// </summary>
    /// <param name="path">確認するパス</param>
    /// <returns>存在するパス</returns>
    private string GetExistPath(string path)
    {
        // 存在する場合はそのまま返す
        if (Directory.Exists(path)) return path;

        // 存在しない場合は、フォルダ選択ダイアログを開く
        if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
        {
            // 保存パスに設定
            appSettings.TargetFolder = appSettings.PreviousFolder = folderBrowserDialog.SelectedPath;

        }
        else
        {
            // キャンセルしたときはデスクトップ
            appSettings.TargetFolder = appSettings.PreviousFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        return appSettings.TargetFolder;
    }

    /// <summary>
    /// 保存実行
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Save(object? sender, EventArgs e)
    {
        string pathName = "";
        switch (appSettings.SaveFolderType)
        {
            case 0: // 画像と同じフォルダ                    
                if (string.IsNullOrEmpty(bitmapEffects.LongFileName))
                {
                    // ファイル以外はデスクトップ
                    pathName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }
                else
                {
                    // 画像と同じフォルダ
                    pathName = Path.GetDirectoryName(bitmapEffects.LongFileName)!;
                }
                break;
            case 1: // 前回の保存フォルダ                    
                pathName = GetExistPath(appSettings.PreviousFolder);
                break;
            case 2: // 指定したフォルダ                    
                pathName = GetExistPath(appSettings.TargetFolder);
                break;
        }

        // 保存ダイアログの初期ディレクトリをセット
        saveFileDialog.InitialDirectory = pathName;

        // 保存ファイル名を生成
        var fileName = appSettings.Prefix;
        if (appSettings.IsRenumber)
        {
            // 番号を振りなおす場合
            var i = 1;
            string numFileName;
            do
            {
                // 保存ファイル名をインクリメント
                numFileName = fileName + $"{i}".PadLeft((int)appSettings.DigitNum, '0') + appSettings.Extention;
                i++;
            } while (File.Exists(pathName + "\\" + numFileName));
            fileName = numFileName;
        }
        else
        {
            fileName += Path.GetFileNameWithoutExtension(bitmapEffects.LongFileName) + appSettings.Extention;
        }
        saveFileDialog.FileName = fileName;


        // フィルタの自動選択
        Dictionary<string, int> extDictionary = new() { { ".jpg", 1 }, { ".bmp", 2 }, { ".png", 3 }, { ".gif", 4 }, { ".tiff", 5 } };

        int index = 6;
        if (extDictionary.ContainsKey(appSettings.Extention))
        {
            index = extDictionary[appSettings.Extention];
        }
        saveFileDialog.FilterIndex = index;

        if (sender == null)
        {
            // 自動処理の場合
            fileName = saveFileDialog.InitialDirectory + "\\" + saveFileDialog.FileName;
        }
        else
        {
            // 保存ダイアログを表示し、OK以外なら戻る
            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            fileName = saveFileDialog.FileName;
        }

        // 画像の保存
        SaveImage(fileName);

        // 設定の保存
        appSettings.PreviousFolder = Path.GetDirectoryName(fileName)!;
    }

    /// <summary>
    /// 画像の保存
    /// </summary>
    /// <param name="fileName"></param>
    private void SaveImage(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToUpperInvariant();
        using Bitmap bmp = new(mainPictureBox.Image, bitmapEffects.SrcSize);
        switch (ext)
        {
            case ".JPG":
            case ".JPEG":
                SaveJpeg(bmp, fileName, appSettings.JpegQuality);
                break;
            case ".GIF":
                bmp.Save(fileName, ImageFormat.Gif);
                break;
            case ".PNG":
                bmp.Save(fileName, ImageFormat.Png);
                break;
            case ".TIF":
            case ".TIFF":
                bmp.Save(fileName, ImageFormat.Tiff);
                break;
            case ".BMP":
                bmp.Save(fileName, ImageFormat.Bmp);
                break;
            default:
                SaveJpeg(bmp, fileName + ".jpg", appSettings.JpegQuality);
                break;
        }
    }

    /// <summary>
    /// 画像の保存
    /// </summary>
    /// <param name="image">イメージ</param>
    /// <param name="fileName">ファイル名</param>
    /// <param name="quality">画質</param>
    private static void SaveJpeg(Image image, string fileName, int quality)
    {
        // エンコードパラメータ
        using EncoderParameters encParams = new(1);
        encParams.Param[0] = new(System.Drawing.Imaging.Encoder.Quality, quality);

        // イメージコーデック
        var encoders = ImageCodecInfo.GetImageEncoders();
        var index = 0;
        foreach (var encoder in encoders)
        {
            if (encoder.MimeType!.Equals("image/jpeg")) break;
            index++;
        }

        // 保存する
        image.Save(fileName, encoders[index], encParams);
    }

    private void PicLogo_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://nakasendo.com/seedea/") { UseShellExecute = true });
    }

    /// <summary>
    /// エフェクトを実行
    /// </summary>
    private async Task<bool> MainDoEffect()
    {
        stopwatch.Reset();
        stopwatch.Start();
        timeLabel.Text += "*";
        Cursor.Current = Cursors.WaitCursor;

        var color = pickupPictureBox.BackColor;

        // エフェクト実行
        await Task.Run(() =>
        {
            lock (lockObject)
            {
                var v = appSettings.Value;
                mainPictureBox.Image = effect.DoEffect(v, color, bitmapEffects.SrcBitmap);
            }
        });

        Cursor.Current = Cursors.Default;
        valueUpDown.Value = valueTrackBar.Value;
        stopwatch.Stop();
        timeLabel.Text = string.Format(Resources.MainElapsedTime, stopwatch.ElapsedMilliseconds);
        return true;
    }

    /// <summary>
    /// サイズボタン
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void SizeButton_Click(object sender, EventArgs e)
    {
        using SizeForm sizeForm = new()
        {
            BmpWidth = bitmapEffects.OrgSize.Width,
            BmpHeight = bitmapEffects.OrgSize.Height
        };

        if (sizeForm.ShowDialog(this) != DialogResult.OK) return;

        // 設定の保存
        appSettings.Width = sizeForm.BmpWidth;
        appSettings.Height = sizeForm.BmpHeight;
        appSettings.ResizeType = sizeForm.ResizeType;

        // サイズ処理メソッド
        DoSize();

        _ = await MainDoEffect();
    }

    /// <summary>
    /// サイズ変更処理
    /// </summary>
    private void DoSize()
    {
        bitmapEffects.DoSize(appSettings.Width, appSettings.Height, appSettings.ResizeType);
        ChangeFormSize();
        SetTitle();
    }

    private void SetTitle()
    {
        if (appSettings == null) return;
        Text = Resources.Gakupetit + $" : {Path.GetFileName(bitmapEffects.LongFileName)} ({effect.Names[languageIndex]} - {appSettings.Width}x{appSettings.Height})";

        var srcAspect = bitmapEffects.SrcSize.Width / (double)bitmapEffects.SrcSize.Height;
        var picAspect = mainPictureBox.Width / (double)mainPictureBox.Height;
        double size;
        if (srcAspect < picAspect)
        {
            size = mainPictureBox.Height * 10 / bitmapEffects.SrcSize.Height;
        }
        else
        {
            size = mainPictureBox.Width * 10 / bitmapEffects.SrcSize.Width;
        }
        size /= 10.0;
        lblMagnify.Text = $"(x{size:F1})";
    }

    /// <summary>
    /// 開くボタン
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OpenButton_Click(object sender, EventArgs e)
    {
        // フルパスが指定されていた場合は、フォルダのみに変更
        if (!string.IsNullOrEmpty(openFileDialog.FileName))
        {
            string path = Path.GetDirectoryName(openFileDialog.FileName)!;
            openFileDialog.InitialDirectory = path;
            openFileDialog.FileName = string.Empty;
        }

        openFileDialog.Title = Resources.MainImageFileMessage;

        // ファイルダイアログを開き、OKでなかったら戻る
        if (openFileDialog.ShowDialog() != DialogResult.OK) return;

        // ファイル名を開く
        if (openFileDialog.FileNames.Length > 1)
        {
            if (MessageBox.Show(Resources.MainConfirmMultipleFile, Resources.MainConfirmProcessing,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1, 0) == DialogResult.Yes)
            {
                // 連続モードで開く
                OpenFileNames(openFileDialog.FileNames);
                return;
            }
        }

        // 単独で開く
        string[] file = { openFileDialog.FileNames[0] };
        OpenFileNames(file);
    }

    /// <summary>
    /// ファイル名を開く
    /// </summary>
    /// <param name="fileNames">１つまたは複数のファイル名</param>
    private void OpenFileNames(string[] fileNames)
    {
        // 処理中なら待つ
        while (stopwatch.IsRunning)
        {
            Application.DoEvents();
        }

        if (fileNames.Length == 1)
        {
            // ファイルが１つのときは、通常モードでオープン
            bitmapEffects.LongFileName = fileNames[0];
            OpenImage(null);
        }
        else if (fileNames.Length > 1)
        {
            // ファイルが２つ以上のときは、複数自動処理モード
            ProcImageFiles(fileNames);
        }
        else
        {
            // 引数がない場合は、初期画面を作成
            effect = new Default(bitmapEffects);
            var bmp = effect.DoEffect(0, Color.White, new Bitmap(appSettings.Width, appSettings.Height));
            bitmapEffects.OpenImage(bmp);

            // ちょっと待つ
            Thread.Sleep(10);

            // デフォルトのエフェクトを実行
            ChangeEffectType();
        }
    }

    /// <summary>
    /// カラーを変更
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ColorButton_Click(object sender, EventArgs e)
    {
        // カラーダイアログ表示
        colorDialog.CustomColors = new int[] { ColorTranslator.ToWin32(pickupPictureBox.BackColor) };
        colorDialog.Color = pickupPictureBox.BackColor;

        if (colorDialog.ShowDialog() != DialogResult.OK) return;

        // 色を変更
        SetColor(colorDialog.Color);
    }

    /// <summary>
    /// トラックバーの移動時
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void ValueTrackBar_Scroll(object sender, EventArgs e)
    {
        // 設定を保存
        appSettings.Value = valueTrackBar.Value;

        // 実行バッファは1つ
        if (1 < doCount) return;

        doCount++;
        _ = await MainDoEffect();
        doCount--;
    }

    /// <summary>
    /// 背景色を表示するか？
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BackCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        SetBackColor(pickupPictureBox.BackColor);
    }

    /// <summary>
    /// ファイル - 終了 メニュー
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ExitMenuItem_Click(object sender, EventArgs e)
    {
        Close();
    }

    /// <summary>
    /// コピー
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Copy(object? sender, EventArgs e)
    {
        //画像データをクリップボードにコピーする
        Clipboard.SetImage(mainPictureBox.Image);
    }

    /// <summary>
    /// 貼り付け
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Paste(object? sender, EventArgs e)
    {
        // クリップボードからイメージを取得してnullでなかったら開く
        var image = Clipboard.GetImage();
        if (image != null) OpenImage(image);
    }

    /// <summary>
    /// 背景色で表示
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BackMenuItem_Click(object sender, EventArgs e)
    {
        backCheckBox.Checked = backMenuItem.Checked;
    }

    /// <summary>
    /// フォームを閉じる際に、設定を保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        // 連続処理中は閉じさせない
        if (stopButton.Visible)
        {
            var result = MessageBox.Show(Resources.MainNowOnProcessing, Resources.MainConfirmProcessing,
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1, 0);
            if (result == DialogResult.Yes) isAbort = true;
            e.Cancel = true;
        }

        // 設定を保存
        settingManage.Write(appSettings);
    }

    /// <summary>
    /// 連続処理設定...
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SaveSettingsMenuItem_Click(object? sender, EventArgs e)
    {
        using SaveSettingsForm saveSettingsForm = new(appSettings);
        saveSettingsForm.ShowDialog();
    }

    private void PickupPictureBox_MouseDown(object sender, MouseEventArgs e)
    {
        // スポイトで色の取得を開始
        pickupPictureBox.Capture = true;
        tmpTitleText = Text;
        Text = Resources.MainDragColor;
    }

    // マウスの下の色の取得
    private void PickupPictureBox_MouseMove(object sender, MouseEventArgs e)
    {
        if (!pickupPictureBox.Capture) return;

        CursorPoint p = new();

        if (Environment.OSVersion.Version.Major < 6)
        {
            // 96dpiのみ対応
            p.PosX = Cursor.Position.X;
            p.PosY = Cursor.Position.Y;
        }
        else
        {
            // Vista以上
            SafeNativeMethods.GetPhysicalCursorPos(ref p);
        }

        using Bitmap img = new(1, 1);
        using var g = Graphics.FromImage(img);
        g.CopyFromScreen(p.PosX, p.PosY, 0, 0, new Size(1, 1), CopyPixelOperation.SourceCopy);
        Text = Resources.MainDragColor + $" ({p.PosX + 1} x {p.PosY + 1})";

        Rectangle rect = new(0, 0, 1, 1);
        var inBmpData = img.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

        // RGB値をbyte列にコピーする
        var inPtr = inBmpData.Scan0;
        var inRgbValues = new byte[4];
        Marshal.Copy(inPtr, inRgbValues, 0, 4);
        // 色を変更
        pickupPictureBox.BackColor = Color.FromArgb(inRgbValues[2], inRgbValues[1], inRgbValues[0]);

        // byte列をbitmapに復元し、メモリのロックを開放する
        img.UnlockBits(inBmpData);
    }

    private void PickupPictureBox_MouseUp(object sender, MouseEventArgs e)
    {
        // スポイトでの色の取得終了
        pickupPictureBox.Capture = false;
        Text = tmpTitleText;

        // 色を変更
        SetColor(pickupPictureBox.BackColor);
    }

    /// <summary>
    /// フォームの色を設定
    /// </summary>
    /// <param name="color"></param>
    private async void SetColor(Color color)
    {
        // 色を変更
        appSettings.Color = color;
        pickupPictureBox.BackColor = color;
        SetBackColor(color);

        // 画像を処理
        _ = await MainDoEffect();
    }

    // 背景色を取得
    internal Color GetBackColor()
    {
        return backCheckBox.Checked ? backPictureBox.BackColor : SystemColors.ControlDark;
    }

    // 背景色を設定
    private void SetBackColor(Color color)
    {
        // 背景色を設定
        var backColor = backCheckBox.Checked ? color : SystemColors.ControlDark;
        mainPictureBox.BackColor = backColor;
        backPictureBox.BackColor = backColor;
        backCheckBox.BackColor = backColor;

        if (color == Color.Transparent)
        {
            Bitmap backBmp = new(Width, Height);

            try
            {
                using var g = Graphics.FromImage(backBmp);
                var top = backPictureBox.Top;
                g.FillRectangle(Brushes.White, new Rectangle(0, top, Width, Height - top));
                var rSize = 50;
                for (int j = top; j < Height; j += rSize)
                {
                    for (int i = 0; i < Width; i += rSize * 2)
                    {
                        g.FillRectangle(Brushes.LightGray, new Rectangle(i + (j - top) % (rSize * 2), j, rSize, rSize));
                    }
                }
            }
            catch (Exception)
            {
                backBmp.Dispose();
                throw;
            }
            BackgroundImage = backBmp;
        }
        else
        {
            BackgroundImage = null;
        }

        // テキストの背景色
        timeLabel.BackColor = backColor;
        lblMagnify.BackColor = backColor;

        // テキスト色を設定
        var textColor = IslightColor(backColor) ? SystemColors.ControlText : Color.White;
        backCheckBox.ForeColor = textColor;
        timeLabel.ForeColor = textColor;
        lblMagnify.ForeColor = textColor;
    }

    /// <summary>
    /// 明るい色か？
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    internal static bool IslightColor(Color color)
    {
        var r = color.R;
        var g = color.G;
        var b = color.B;

        var y = r * 3 + g * 6 + b;

        // 輝度を10倍して1000以上だったら明るい
        return (y > 1000);
    }

    /// <summary>
    /// 連続処理...
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LoopMenuItem_Click(object sender, EventArgs e)
    {
        // ファイル名の初期化
        openFileDialog.FileName = string.Empty;

        // 初期ディレクトリはデスクトップ
        openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        openFileDialog.Title = Resources.MainSpecifyMultipleFile;

        // OK以外は戻る
        if (openFileDialog.ShowDialog() != DialogResult.OK) return;

        if (openFileDialog.FileNames.Length == 1)
        {
            MessageBox.Show(Resources.MainMassageMultipleFile, Resources.MainTitleMultipleFile,
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1, 0);
        }

        // ファイルを開く
        OpenFileNames(openFileDialog.FileNames);
    }

    /// <summary>
    /// コンテキストメニュー：開く...
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OpenContextMenuItem_Click(object sender, EventArgs e)
    {
        // 開く
        OpenButton_Click(sender, e);
    }

    /// <summary>
    /// コンテキストメニュー：保存...
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SaveContextMenuItem_Click(object sender, EventArgs e)
    {
        // 保存
        Save(sender, e);
    }

    /// <summary>
    /// コンテキストメニュー：コピー
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CopyContextMenuItem_Click(object sender, EventArgs e)
    {
        // コピー
        Copy(sender, e);
    }

    /// <summary>
    /// コンテキストメニュー：貼り付け
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PasteContextMenuItem_Click(object sender, EventArgs e)
    {
        // 貼り付け
        Paste(sender, e);
    }

    /// <summary>
    /// 再読み込み
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ReOpenMenuItem_Click(object sender, EventArgs e)
    {
        Copy(null, EventArgs.Empty);
        Paste(null, EventArgs.Empty);
    }

    /// <summary>
    /// 枠の種類ボタンをクリック
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EffectButton_Click(object sender, EventArgs e)
    {
        // 枠の種類選択ダイアログを表示
        using EffectForm effectForm = new(appSettings.Effect);
        effectForm.ShowDialog(this);

        // 設定の保存
        appSettings.Effect = effectForm.SelectedEffect;

        ChangeEffectType();
    }

    /// <summary>
    /// 枠の種類メニューを選択
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EffectMenuItem_Click(object sender, EventArgs e)
    {
        var tsi = (ToolStripItem)sender;

        // 選択したメニューに設定されているTagを取得
        var effectNo = int.Parse(tsi.Tag.ToString()!, CultureInfo.CurrentCulture);

        // 設定の保存
        appSettings.Effect = effectNo;

        ChangeEffectType();
    }

    /// <summary>
    /// デフォルト色
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SpuitPictureBox_Click(object sender, EventArgs e)
    {
        // 色をデフォルトに戻す
        pickupPictureBox.BackColor = appSettings.DefaultColor;
        SetColor(pickupPictureBox.BackColor);
    }

    /// <summary>
    /// 等倍表示
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EqualButton_Click(object sender, EventArgs e)
    {
        var scrRect = Screen.GetWorkingArea(this);
        var realSize = mainPictureBox.Image.Size;
        realSize += new Size(wakuWidth, wakuHeight);
        Size canSize = new(scrRect.Right - scrRect.X, scrRect.Bottom - scrRect.Y);

        SuspendLayout();

        WindowState = FormWindowState.Normal;
        Width = (realSize.Width > canSize.Width) ? canSize.Width : realSize.Width;
        Height = (realSize.Height > canSize.Height) ? canSize.Height : realSize.Height;

        if (Left + Width > canSize.Width) Left = canSize.Width - Width + scrRect.X;
        if (Top + Height > canSize.Height) Top = canSize.Height - Height + scrRect.Y;

        ResumeLayout();
        SetTitle();
    }

    private async void ValueUpDown_ValueChanged(object sender, EventArgs e)
    {
        appSettings.Value = (int)valueUpDown.Value;

        // トラックバーの値が異なるときは、セットしなおして実行 
        if (valueTrackBar.Value != appSettings.Value)
        {
            valueTrackBar.Value = appSettings.Value;
            _ = await MainDoEffect();
        }
    }

    private void StopButton_Click(object sender, EventArgs e)
    {
        isAbort = true;
    }

    /// <summary>
    /// IDで指定されたエフェクトを使えるようにする
    /// </summary>
    /// <param name="bitmapEffects"></param>
    /// <param name="id"></param>
    /// <returns>エフェクト</returns>
    internal IEffect GetEffect(BitmapEffects bitmapEffects, int id)
    {
        return id switch
        {
            0 => new E000_Fade(bitmapEffects),
            1 => new E001_Transparent(bitmapEffects),
            2 => new E002_Gauss(bitmapEffects),
            3 => new E003_Circle(bitmapEffects),
            4 => new E004_RandomDot(bitmapEffects),
            5 => new E005_ColorDot(bitmapEffects),
            6 => new E006_ZigZag(bitmapEffects),
            7 => new E007_Border(bitmapEffects),
            8 => new E008_RoundCorner(bitmapEffects),
            9 => new E009_Heart(bitmapEffects),
            10 => new E010_Rectize(bitmapEffects),
            11 => new E011_Stamp(bitmapEffects),
            12 => new E012_Flower(bitmapEffects),
            13 => new E013_Halftone(bitmapEffects),
            14 => new E014_Led(bitmapEffects),
            15 => new E015_Film(bitmapEffects),
            16 => new E016_UnderConstruction(bitmapEffects),
            17 => new E017_Ellipse(bitmapEffects),
            18 => new E018_Bevel(bitmapEffects),
            19 => new E019_Slope(bitmapEffects),
            20 => new E020_SaturatedLinework(bitmapEffects),
            21 => new E021_Crt(bitmapEffects),
            22 => new E022_Mirror(bitmapEffects),
            23 => new E023_Checker(bitmapEffects),
            24 or 25 or 26 or 27 or 28 or 29 => new E024_UserDefined(bitmapEffects) { ImagesFolder = appSettings.ImagesFolder, EffectId = id },
            30 or 31 or 32 or 33 or 34 or 35 => new E030_Painting(bitmapEffects) { ImagesFolder = appSettings.ImagesFolder, EffectId = id },

            // 新規エフェクトはここに追加すること(下記EffectNumも更新すること)
            _ => new Default(bitmapEffects),
        };
    }

    /// <summary>
    /// エフェクトの数
    /// </summary>
    internal static int EffectNum => 36;

    /// <summary>
    /// メニュー用の黒ビットマップを作成する
    /// </summary>
    /// <returns>16x16のビットマップ</returns>
    private static Bitmap MakeIconBitmap()
    {
        Bitmap b = new(16, 16);
        using var g = Graphics.FromImage(b);
        g.Clear(Color.Black);
        return b;
    }

    /// <summary>
    /// エフェクト選択用の画像と説明を取得する
    /// </summary>
    /// <param name="id"></param>
    /// <param name="forMenu">メニュー用か？(default:false)</param>
    /// <returns>(名前,プレビュー, 説明)</returns>
    internal async Task<(string, Bitmap, string)> GetPreviewInfo(int id, bool forMenu = false)
    {
        var e = GetEffect(bitmapEffects, id);
        var orgImage = forMenu ? MakeIconBitmap() : bitmapEffects.ThumBitmap!;
        return (e.Names[languageIndex], await Task.Run(() => e.DoEffect(e.DefaultValue, e.GetDefaultColor(pickupPictureBox.BackColor), orgImage)), e.Descriptions[languageIndex]);
    }

    /// <summary>
    /// ライブエフェクト用の背景色
    /// </summary>
    internal Color EffectColor => (pickupPictureBox.BackColor == Color.Transparent) ? Color.White : pickupPictureBox.BackColor;

    /// <summary>
    /// エフェクトごとのデフォルト設定にする
    /// </summary>
    private async void ChangeEffectType()
    {
        var id = appSettings.Effect;

        effect = GetEffect(bitmapEffects, id);

        // 透明の場合は警告表示
        if (id == 1) MessageBox.Show(Resources.MainMessageTransparent);

        // デフォルト値を設定し、パラメータを保存
        valueTrackBar.Value = appSettings.Value = valueTrackBar.Value = effect.DefaultValue;

        // デフォルトは背景色を表示するか？
        backCheckBox.Checked = effect.IsBackChecked;

        // デフォルトの背景色
        var color = effect.GetDefaultColor(pickupPictureBox.BackColor);
        if (color == Color.Transparent && id != 1) color = Color.White;
        SetColor(color);
        SetTitle();

        // エフェクトを実行
        _ = await MainDoEffect();
    }

    // プライバシーポリシーの表示
    private void PrivacyPolicyMenuItem_Click(object sender, EventArgs e) => MessageBox.Show(Resources.MainPrivacyPoricyText, Resources.MainPrivacyPolicyTitle);

    /// <summary>
    /// :フォームサイズが変更されたらタイトルも更新
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MainForm_SizeChanged(object sender, EventArgs e) => SetTitle();

    /// <summary>
    /// imagesフォルダを開く
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void FolderOpenMenuItem_Click(object sender, EventArgs e)
    {
        var myAssembly = Assembly.GetEntryAssembly()!;
        var exeDir = Path.GetDirectoryName(myAssembly.Location)!;
        var imgDir = Path.Combine(exeDir, "images");
        Process.Start("EXPLORER.EXE", $"/e,{imgDir}");
    }
}