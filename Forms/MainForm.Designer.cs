namespace Com.Nakasendo.Gakupetit;

partial class MainForm
{
    /// <summary>
    /// 必要なデザイナ変数です。
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// 使用中のリソースをすべてクリーンアップします。
    /// </summary>
    /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        this.settingManage.Dispose();
        base.Dispose(disposing);
    }

    #region Windows フォーム デザイナで生成されたコード

    /// <summary>
    /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
    /// コード エディタで変更しないでください。
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        valueTrackBar = new TrackBar();
        label5 = new Label();
        menuStrip = new MenuStrip();
        fileMenuItem = new ToolStripMenuItem();
        openMenuItem = new ToolStripMenuItem();
        reOpenMenuItem = new ToolStripMenuItem();
        loopMenuItem = new ToolStripMenuItem();
        toolStripMenuItem1 = new ToolStripSeparator();
        saveMenuItem = new ToolStripMenuItem();
        toolStripMenuItem2 = new ToolStripSeparator();
        exitMenuItem = new ToolStripMenuItem();
        editMenuItem = new ToolStripMenuItem();
        copyMenuItem = new ToolStripMenuItem();
        pasteMenuItem = new ToolStripMenuItem();
        effectMenuItem = new ToolStripMenuItem();
        viewMenuItem = new ToolStripMenuItem();
        colorMenuItem = new ToolStripMenuItem();
        defaultColorMenuItem = new ToolStripMenuItem();
        backMenuItem = new ToolStripMenuItem();
        toolStripMenuItem4 = new ToolStripSeparator();
        sizeMenuItem = new ToolStripMenuItem();
        equalMenuItem = new ToolStripMenuItem();
        settingsMenuItem = new ToolStripMenuItem();
        saveSettingsMenuItem = new ToolStripMenuItem();
        openFolderMenuItem = new ToolStripMenuItem();
        helpMenuItem = new ToolStripMenuItem();
        gakupetitMenuItem = new ToolStripMenuItem();
        privacyPolicyMenuItem = new ToolStripMenuItem();
        timeLabel = new Label();
        backCheckBox = new CheckBox();
        saveFileDialog = new SaveFileDialog();
        colorDialog = new ColorDialog();
        toolTip = new ToolTip(components);
        valueUpDown = new NumericUpDown();
        saveButton = new Button();
        buttonImageList = new ImageList(components);
        mainPictureBox = new PictureBox();
        spuitPictureBox = new PictureBox();
        pickupPictureBox = new PictureBox();
        openButton = new Button();
        colorButton = new Button();
        sizeButton = new Button();
        effectButton = new Button();
        logoPicture = new PictureBox();
        equalButton = new Button();
        stopButton = new Button();
        progressBar = new ProgressBar();
        contextMenuStrip = new ContextMenuStrip(components);
        openContextMenuItem = new ToolStripMenuItem();
        saveContextMenuItem = new ToolStripMenuItem();
        toolStripMenuItem3 = new ToolStripSeparator();
        copyContextMenuItem = new ToolStripMenuItem();
        pasteContextMenuItem = new ToolStripMenuItem();
        backPictureBox = new PictureBox();
        lblMagnify = new Label();
        openFileDialog = new OpenFileDialog();
        folderBrowserDialog = new FolderBrowserDialog();
        HighDpiButtonImageList = new ImageList(components);
        ((System.ComponentModel.ISupportInitialize)valueTrackBar).BeginInit();
        menuStrip.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)valueUpDown).BeginInit();
        ((System.ComponentModel.ISupportInitialize)mainPictureBox).BeginInit();
        ((System.ComponentModel.ISupportInitialize)spuitPictureBox).BeginInit();
        ((System.ComponentModel.ISupportInitialize)pickupPictureBox).BeginInit();
        ((System.ComponentModel.ISupportInitialize)logoPicture).BeginInit();
        contextMenuStrip.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)backPictureBox).BeginInit();
        SuspendLayout();
        // 
        // valueTrackBar
        // 
        resources.ApplyResources(valueTrackBar, "valueTrackBar");
        valueTrackBar.Maximum = 100;
        valueTrackBar.Name = "valueTrackBar";
        valueTrackBar.TickStyle = TickStyle.None;
        toolTip.SetToolTip(valueTrackBar, resources.GetString("valueTrackBar.ToolTip"));
        valueTrackBar.Scroll += ValueTrackBar_Scroll;
        // 
        // label5
        // 
        resources.ApplyResources(label5, "label5");
        label5.BackColor = Color.Transparent;
        label5.Name = "label5";
        // 
        // menuStrip
        // 
        menuStrip.ImageScalingSize = new Size(24, 24);
        menuStrip.Items.AddRange(new ToolStripItem[] { fileMenuItem, editMenuItem, effectMenuItem, viewMenuItem, settingsMenuItem, helpMenuItem });
        resources.ApplyResources(menuStrip, "menuStrip");
        menuStrip.Name = "menuStrip";
        // 
        // fileMenuItem
        // 
        fileMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openMenuItem, reOpenMenuItem, loopMenuItem, toolStripMenuItem1, saveMenuItem, toolStripMenuItem2, exitMenuItem });
        fileMenuItem.Name = "fileMenuItem";
        resources.ApplyResources(fileMenuItem, "fileMenuItem");
        // 
        // openMenuItem
        // 
        resources.ApplyResources(openMenuItem, "openMenuItem");
        openMenuItem.Name = "openMenuItem";
        openMenuItem.Click += OpenButton_Click;
        // 
        // reOpenMenuItem
        // 
        reOpenMenuItem.Name = "reOpenMenuItem";
        resources.ApplyResources(reOpenMenuItem, "reOpenMenuItem");
        reOpenMenuItem.Click += ReOpenMenuItem_Click;
        // 
        // loopMenuItem
        // 
        resources.ApplyResources(loopMenuItem, "loopMenuItem");
        loopMenuItem.Name = "loopMenuItem";
        loopMenuItem.Click += LoopMenuItem_Click;
        // 
        // toolStripMenuItem1
        // 
        toolStripMenuItem1.Name = "toolStripMenuItem1";
        resources.ApplyResources(toolStripMenuItem1, "toolStripMenuItem1");
        // 
        // saveMenuItem
        // 
        resources.ApplyResources(saveMenuItem, "saveMenuItem");
        saveMenuItem.Name = "saveMenuItem";
        saveMenuItem.Click += Save;
        // 
        // toolStripMenuItem2
        // 
        toolStripMenuItem2.Name = "toolStripMenuItem2";
        resources.ApplyResources(toolStripMenuItem2, "toolStripMenuItem2");
        // 
        // exitMenuItem
        // 
        exitMenuItem.Name = "exitMenuItem";
        resources.ApplyResources(exitMenuItem, "exitMenuItem");
        exitMenuItem.Click += ExitMenuItem_Click;
        // 
        // editMenuItem
        // 
        editMenuItem.DropDownItems.AddRange(new ToolStripItem[] { copyMenuItem, pasteMenuItem });
        editMenuItem.Name = "editMenuItem";
        resources.ApplyResources(editMenuItem, "editMenuItem");
        // 
        // copyMenuItem
        // 
        resources.ApplyResources(copyMenuItem, "copyMenuItem");
        copyMenuItem.Name = "copyMenuItem";
        copyMenuItem.Click += Copy;
        // 
        // pasteMenuItem
        // 
        resources.ApplyResources(pasteMenuItem, "pasteMenuItem");
        pasteMenuItem.Name = "pasteMenuItem";
        pasteMenuItem.Click += Paste;
        // 
        // effectMenuItem
        // 
        effectMenuItem.Name = "effectMenuItem";
        resources.ApplyResources(effectMenuItem, "effectMenuItem");
        // 
        // viewMenuItem
        // 
        viewMenuItem.DropDownItems.AddRange(new ToolStripItem[] { colorMenuItem, defaultColorMenuItem, backMenuItem, toolStripMenuItem4, sizeMenuItem, equalMenuItem });
        viewMenuItem.Name = "viewMenuItem";
        resources.ApplyResources(viewMenuItem, "viewMenuItem");
        // 
        // colorMenuItem
        // 
        resources.ApplyResources(colorMenuItem, "colorMenuItem");
        colorMenuItem.Name = "colorMenuItem";
        colorMenuItem.Click += ColorButton_Click;
        // 
        // defaultColorMenuItem
        // 
        defaultColorMenuItem.Name = "defaultColorMenuItem";
        resources.ApplyResources(defaultColorMenuItem, "defaultColorMenuItem");
        defaultColorMenuItem.Click += SpuitPictureBox_Click;
        // 
        // backMenuItem
        // 
        backMenuItem.Checked = true;
        backMenuItem.CheckOnClick = true;
        backMenuItem.CheckState = CheckState.Checked;
        backMenuItem.Name = "backMenuItem";
        resources.ApplyResources(backMenuItem, "backMenuItem");
        backMenuItem.Click += BackMenuItem_Click;
        // 
        // toolStripMenuItem4
        // 
        toolStripMenuItem4.Name = "toolStripMenuItem4";
        resources.ApplyResources(toolStripMenuItem4, "toolStripMenuItem4");
        // 
        // sizeMenuItem
        // 
        resources.ApplyResources(sizeMenuItem, "sizeMenuItem");
        sizeMenuItem.Name = "sizeMenuItem";
        sizeMenuItem.Click += SizeButton_Click;
        // 
        // equalMenuItem
        // 
        equalMenuItem.Name = "equalMenuItem";
        resources.ApplyResources(equalMenuItem, "equalMenuItem");
        equalMenuItem.Click += EqualButton_Click;
        // 
        // settingsMenuItem
        // 
        settingsMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveSettingsMenuItem, openFolderMenuItem });
        settingsMenuItem.Name = "settingsMenuItem";
        resources.ApplyResources(settingsMenuItem, "settingsMenuItem");
        // 
        // saveSettingsMenuItem
        // 
        resources.ApplyResources(saveSettingsMenuItem, "saveSettingsMenuItem");
        saveSettingsMenuItem.Name = "saveSettingsMenuItem";
        saveSettingsMenuItem.Click += SaveSettingsMenuItem_Click;
        // 
        // openFolderMenuItem
        // 
        resources.ApplyResources(openFolderMenuItem, "openFolderMenuItem");
        openFolderMenuItem.Name = "openFolderMenuItem";
        openFolderMenuItem.Click += FolderOpenMenuItem_Click;
        // 
        // helpMenuItem
        // 
        helpMenuItem.DropDownItems.AddRange(new ToolStripItem[] { gakupetitMenuItem, privacyPolicyMenuItem });
        helpMenuItem.Name = "helpMenuItem";
        resources.ApplyResources(helpMenuItem, "helpMenuItem");
        // 
        // gakupetitMenuItem
        // 
        resources.ApplyResources(gakupetitMenuItem, "gakupetitMenuItem");
        gakupetitMenuItem.Name = "gakupetitMenuItem";
        gakupetitMenuItem.Click += PicLogo_Click;
        // 
        // privacyPolicyMenuItem
        // 
        privacyPolicyMenuItem.Name = "privacyPolicyMenuItem";
        resources.ApplyResources(privacyPolicyMenuItem, "privacyPolicyMenuItem");
        privacyPolicyMenuItem.Click += PrivacyPolicyMenuItem_Click;
        // 
        // timeLabel
        // 
        resources.ApplyResources(timeLabel, "timeLabel");
        timeLabel.BackColor = SystemColors.ControlDark;
        timeLabel.Name = "timeLabel";
        // 
        // backCheckBox
        // 
        resources.ApplyResources(backCheckBox, "backCheckBox");
        backCheckBox.BackColor = SystemColors.ControlDark;
        backCheckBox.Checked = true;
        backCheckBox.CheckState = CheckState.Checked;
        backCheckBox.FlatAppearance.BorderSize = 0;
        backCheckBox.Name = "backCheckBox";
        backCheckBox.UseVisualStyleBackColor = false;
        backCheckBox.CheckedChanged += BackCheckBox_CheckedChanged;
        // 
        // saveFileDialog
        // 
        saveFileDialog.DefaultExt = "jpg";
        resources.ApplyResources(saveFileDialog, "saveFileDialog");
        // 
        // colorDialog
        // 
        colorDialog.AnyColor = true;
        colorDialog.FullOpen = true;
        // 
        // toolTip
        // 
        toolTip.AutoPopDelay = 5000;
        toolTip.InitialDelay = 100;
        toolTip.ReshowDelay = 100;
        // 
        // valueUpDown
        // 
        resources.ApplyResources(valueUpDown, "valueUpDown");
        valueUpDown.Name = "valueUpDown";
        toolTip.SetToolTip(valueUpDown, resources.GetString("valueUpDown.ToolTip"));
        valueUpDown.ValueChanged += ValueUpDown_ValueChanged;
        // 
        // saveButton
        // 
        resources.ApplyResources(saveButton, "saveButton");
        saveButton.BackColor = Color.Transparent;
        saveButton.FlatAppearance.BorderSize = 0;
        saveButton.ImageList = buttonImageList;
        saveButton.Name = "saveButton";
        toolTip.SetToolTip(saveButton, resources.GetString("saveButton.ToolTip"));
        saveButton.UseVisualStyleBackColor = false;
        saveButton.Click += Save;
        // 
        // buttonImageList
        // 
        buttonImageList.ColorDepth = ColorDepth.Depth8Bit;
        buttonImageList.ImageStream = (ImageListStreamer)resources.GetObject("buttonImageList.ImageStream");
        buttonImageList.TransparentColor = Color.Transparent;
        buttonImageList.Images.SetKeyName(0, "Open_64x.png");
        buttonImageList.Images.SetKeyName(1, "Resize_64x.png");
        buttonImageList.Images.SetKeyName(2, "Image_64x.png");
        buttonImageList.Images.SetKeyName(3, "Color_64x.png");
        buttonImageList.Images.SetKeyName(4, "Save_64x.png");
        // 
        // mainPictureBox
        // 
        resources.ApplyResources(mainPictureBox, "mainPictureBox");
        mainPictureBox.BackColor = SystemColors.Control;
        mainPictureBox.Name = "mainPictureBox";
        mainPictureBox.TabStop = false;
        toolTip.SetToolTip(mainPictureBox, resources.GetString("mainPictureBox.ToolTip"));
        // 
        // spuitPictureBox
        // 
        spuitPictureBox.BackColor = Color.Transparent;
        resources.ApplyResources(spuitPictureBox, "spuitPictureBox");
        spuitPictureBox.Name = "spuitPictureBox";
        spuitPictureBox.TabStop = false;
        toolTip.SetToolTip(spuitPictureBox, resources.GetString("spuitPictureBox.ToolTip"));
        spuitPictureBox.Click += SpuitPictureBox_Click;
        // 
        // pickupPictureBox
        // 
        pickupPictureBox.BackColor = Color.White;
        pickupPictureBox.BorderStyle = BorderStyle.FixedSingle;
        resources.ApplyResources(pickupPictureBox, "pickupPictureBox");
        pickupPictureBox.Name = "pickupPictureBox";
        pickupPictureBox.TabStop = false;
        toolTip.SetToolTip(pickupPictureBox, resources.GetString("pickupPictureBox.ToolTip"));
        pickupPictureBox.MouseDown += PickupPictureBox_MouseDown;
        pickupPictureBox.MouseMove += PickupPictureBox_MouseMove;
        pickupPictureBox.MouseUp += PickupPictureBox_MouseUp;
        // 
        // openButton
        // 
        openButton.BackColor = Color.Transparent;
        openButton.FlatAppearance.BorderSize = 0;
        resources.ApplyResources(openButton, "openButton");
        openButton.ImageList = buttonImageList;
        openButton.Name = "openButton";
        toolTip.SetToolTip(openButton, resources.GetString("openButton.ToolTip"));
        openButton.UseVisualStyleBackColor = false;
        openButton.Click += OpenButton_Click;
        // 
        // colorButton
        // 
        colorButton.BackColor = Color.Transparent;
        colorButton.FlatAppearance.BorderSize = 0;
        resources.ApplyResources(colorButton, "colorButton");
        colorButton.ImageList = buttonImageList;
        colorButton.Name = "colorButton";
        toolTip.SetToolTip(colorButton, resources.GetString("colorButton.ToolTip"));
        colorButton.UseVisualStyleBackColor = false;
        colorButton.Click += ColorButton_Click;
        // 
        // sizeButton
        // 
        sizeButton.BackColor = Color.Transparent;
        sizeButton.FlatAppearance.BorderSize = 0;
        resources.ApplyResources(sizeButton, "sizeButton");
        sizeButton.ImageList = buttonImageList;
        sizeButton.Name = "sizeButton";
        toolTip.SetToolTip(sizeButton, resources.GetString("sizeButton.ToolTip"));
        sizeButton.UseVisualStyleBackColor = false;
        sizeButton.Click += SizeButton_Click;
        // 
        // effectButton
        // 
        effectButton.BackColor = Color.Transparent;
        effectButton.FlatAppearance.BorderSize = 0;
        resources.ApplyResources(effectButton, "effectButton");
        effectButton.ImageList = buttonImageList;
        effectButton.Name = "effectButton";
        toolTip.SetToolTip(effectButton, resources.GetString("effectButton.ToolTip"));
        effectButton.UseVisualStyleBackColor = false;
        effectButton.Click += EffectButton_Click;
        // 
        // logoPicture
        // 
        resources.ApplyResources(logoPicture, "logoPicture");
        logoPicture.BackColor = Color.White;
        logoPicture.Cursor = Cursors.Hand;
        logoPicture.Name = "logoPicture";
        logoPicture.TabStop = false;
        toolTip.SetToolTip(logoPicture, resources.GetString("logoPicture.ToolTip"));
        logoPicture.Click += PicLogo_Click;
        // 
        // equalButton
        // 
        equalButton.FlatAppearance.BorderSize = 0;
        resources.ApplyResources(equalButton, "equalButton");
        equalButton.Name = "equalButton";
        toolTip.SetToolTip(equalButton, resources.GetString("equalButton.ToolTip"));
        equalButton.UseVisualStyleBackColor = false;
        equalButton.Click += EqualButton_Click;
        // 
        // stopButton
        // 
        stopButton.FlatAppearance.BorderSize = 0;
        resources.ApplyResources(stopButton, "stopButton");
        stopButton.Name = "stopButton";
        toolTip.SetToolTip(stopButton, resources.GetString("stopButton.ToolTip"));
        stopButton.UseVisualStyleBackColor = false;
        stopButton.Click += StopButton_Click;
        // 
        // progressBar
        // 
        resources.ApplyResources(progressBar, "progressBar");
        progressBar.Name = "progressBar";
        progressBar.Step = 1;
        toolTip.SetToolTip(progressBar, resources.GetString("progressBar.ToolTip"));
        // 
        // contextMenuStrip
        // 
        contextMenuStrip.ImageScalingSize = new Size(24, 24);
        contextMenuStrip.Items.AddRange(new ToolStripItem[] { openContextMenuItem, saveContextMenuItem, toolStripMenuItem3, copyContextMenuItem, pasteContextMenuItem });
        contextMenuStrip.Name = "contextMenuStrip";
        resources.ApplyResources(contextMenuStrip, "contextMenuStrip");
        // 
        // openContextMenuItem
        // 
        resources.ApplyResources(openContextMenuItem, "openContextMenuItem");
        openContextMenuItem.Name = "openContextMenuItem";
        openContextMenuItem.Click += OpenContextMenuItem_Click;
        // 
        // saveContextMenuItem
        // 
        resources.ApplyResources(saveContextMenuItem, "saveContextMenuItem");
        saveContextMenuItem.Name = "saveContextMenuItem";
        saveContextMenuItem.Click += SaveContextMenuItem_Click;
        // 
        // toolStripMenuItem3
        // 
        toolStripMenuItem3.Name = "toolStripMenuItem3";
        resources.ApplyResources(toolStripMenuItem3, "toolStripMenuItem3");
        // 
        // copyContextMenuItem
        // 
        resources.ApplyResources(copyContextMenuItem, "copyContextMenuItem");
        copyContextMenuItem.Name = "copyContextMenuItem";
        copyContextMenuItem.Click += CopyContextMenuItem_Click;
        // 
        // pasteContextMenuItem
        // 
        resources.ApplyResources(pasteContextMenuItem, "pasteContextMenuItem");
        pasteContextMenuItem.Name = "pasteContextMenuItem";
        pasteContextMenuItem.Click += PasteContextMenuItem_Click;
        // 
        // backPictureBox
        // 
        resources.ApplyResources(backPictureBox, "backPictureBox");
        backPictureBox.BackColor = SystemColors.ControlDark;
        backPictureBox.Name = "backPictureBox";
        backPictureBox.TabStop = false;
        // 
        // lblMagnify
        // 
        resources.ApplyResources(lblMagnify, "lblMagnify");
        lblMagnify.BackColor = SystemColors.ControlDark;
        lblMagnify.Name = "lblMagnify";
        // 
        // openFileDialog
        // 
        resources.ApplyResources(openFileDialog, "openFileDialog");
        openFileDialog.FilterIndex = 0;
        openFileDialog.Multiselect = true;
        // 
        // folderBrowserDialog
        // 
        resources.ApplyResources(folderBrowserDialog, "folderBrowserDialog");
        // 
        // HighDpiButtonImageList
        // 
        HighDpiButtonImageList.ColorDepth = ColorDepth.Depth8Bit;
        HighDpiButtonImageList.ImageStream = (ImageListStreamer)resources.GetObject("HighDpiButtonImageList.ImageStream");
        HighDpiButtonImageList.TransparentColor = Color.Transparent;
        HighDpiButtonImageList.Images.SetKeyName(0, "Open_64x.png");
        HighDpiButtonImageList.Images.SetKeyName(1, "Resize_64x.png");
        HighDpiButtonImageList.Images.SetKeyName(2, "Image_64x.png");
        HighDpiButtonImageList.Images.SetKeyName(3, "Color_64x.png");
        HighDpiButtonImageList.Images.SetKeyName(4, "Save_64x.png");
        // 
        // MainForm
        // 
        AllowDrop = true;
        resources.ApplyResources(this, "$this");
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = SystemColors.Control;
        ContextMenuStrip = contextMenuStrip;
        Controls.Add(valueUpDown);
        Controls.Add(saveButton);
        Controls.Add(lblMagnify);
        Controls.Add(progressBar);
        Controls.Add(stopButton);
        Controls.Add(timeLabel);
        Controls.Add(equalButton);
        Controls.Add(backCheckBox);
        Controls.Add(mainPictureBox);
        Controls.Add(spuitPictureBox);
        Controls.Add(backPictureBox);
        Controls.Add(pickupPictureBox);
        Controls.Add(valueTrackBar);
        Controls.Add(label5);
        Controls.Add(openButton);
        Controls.Add(colorButton);
        Controls.Add(sizeButton);
        Controls.Add(menuStrip);
        Controls.Add(effectButton);
        Controls.Add(logoPicture);
        MainMenuStrip = menuStrip;
        Name = "MainForm";
        FormClosing += MainForm_FormClosing;
        Load += MainForm_Load;
        DpiChanged += MainForm_DpiChanged;
        SizeChanged += MainForm_SizeChanged;
        DragDrop += MainForm_DragDrop;
        DragEnter += MainForm_DragEnter;
        ((System.ComponentModel.ISupportInitialize)valueTrackBar).EndInit();
        menuStrip.ResumeLayout(false);
        menuStrip.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)valueUpDown).EndInit();
        ((System.ComponentModel.ISupportInitialize)mainPictureBox).EndInit();
        ((System.ComponentModel.ISupportInitialize)spuitPictureBox).EndInit();
        ((System.ComponentModel.ISupportInitialize)pickupPictureBox).EndInit();
        ((System.ComponentModel.ISupportInitialize)logoPicture).EndInit();
        contextMenuStrip.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)backPictureBox).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.TrackBar valueTrackBar;
    private System.Windows.Forms.Button colorButton;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Button openButton;
    private System.Windows.Forms.Button saveButton;
    private System.Windows.Forms.Button sizeButton;
    private System.Windows.Forms.PictureBox logoPicture;
    private System.Windows.Forms.MenuStrip menuStrip;
    private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
    private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
    private System.Windows.Forms.ToolStripMenuItem editMenuItem;
    private System.Windows.Forms.ToolStripMenuItem copyMenuItem;
    private System.Windows.Forms.ToolStripMenuItem pasteMenuItem;
    private System.Windows.Forms.ToolStripMenuItem backMenuItem;
    private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
    private System.Windows.Forms.ToolStripMenuItem gakupetitMenuItem;
    private System.Windows.Forms.PictureBox mainPictureBox;
    private System.Windows.Forms.PictureBox backPictureBox;
    private System.Windows.Forms.Label timeLabel;
    private System.Windows.Forms.CheckBox backCheckBox;
    private System.Windows.Forms.SaveFileDialog saveFileDialog;
    private System.Windows.Forms.ColorDialog colorDialog;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.OpenFileDialog openFileDialog;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem openContextMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveContextMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
    private System.Windows.Forms.ToolStripMenuItem copyContextMenuItem;
    private System.Windows.Forms.ToolStripMenuItem pasteContextMenuItem;
    private System.Windows.Forms.ToolStripMenuItem settingsMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveSettingsMenuItem;
    private System.Windows.Forms.PictureBox pickupPictureBox;
    private System.Windows.Forms.ToolStripMenuItem loopMenuItem;
    private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    private System.Windows.Forms.PictureBox spuitPictureBox;
    private System.Windows.Forms.ToolStripMenuItem reOpenMenuItem;
    private System.Windows.Forms.Button effectButton;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
    private System.Windows.Forms.ToolStripMenuItem defaultColorMenuItem;
    private System.Windows.Forms.ToolStripMenuItem viewMenuItem;
    private System.Windows.Forms.ToolStripMenuItem colorMenuItem;
    private System.Windows.Forms.ToolStripMenuItem equalMenuItem;
    private System.Windows.Forms.Button equalButton;
    private System.Windows.Forms.NumericUpDown valueUpDown;
    private System.Windows.Forms.Button stopButton;
    private System.Windows.Forms.ProgressBar progressBar;
    private System.Windows.Forms.ToolStripMenuItem sizeMenuItem;
    private System.Windows.Forms.ToolStripMenuItem effectMenuItem;
    private System.Windows.Forms.ToolStripMenuItem privacyPolicyMenuItem;
    private System.Windows.Forms.Label lblMagnify;
    private ImageList buttonImageList;
    private ImageList HighDpiButtonImageList;
    private ToolStripMenuItem openFolderMenuItem;
}