namespace Com.Nakasendo.Gakupetit;

partial class SaveSettingsForm
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
        base.Dispose(disposing);
    }

    #region Windows フォーム デザイナで生成されたコード

    /// <summary>
    /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
    /// コード エディタで変更しないでください。
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveSettingsForm));
        okButton = new Button();
        cancelButton = new Button();
        saveFolderGroupBox = new GroupBox();
        targetFolderButton = new Button();
        targetFolderLabel = new Label();
        previousFolderLabel = new Label();
        targetFolderRadioButton = new RadioButton();
        previousFolderRadioButton = new RadioButton();
        sameFolderRadioButton = new RadioButton();
        folderBrowserDialog = new FolderBrowserDialog();
        prefixGroupBox = new GroupBox();
        digitNumUpDown = new NumericUpDown();
        lblDigitNum = new Label();
        extensionLabel = new Label();
        extensionComboBox = new ComboBox();
        isRenumberCheckBox = new CheckBox();
        exLabel = new Label();
        prefixTextBox = new TextBox();
        prefixLabel = new Label();
        infoLabel = new Label();
        defaultButton = new Button();
        jpegQualityGroupBox = new GroupBox();
        jpegQualityLabel = new Label();
        jpegQualityUpDown = new NumericUpDown();
        saveIniGroupBox = new GroupBox();
        saveIniCheckBox = new CheckBox();
        defaultBackColorGroupBox = new GroupBox();
        defaultBackColorPictureBox = new PictureBox();
        colorButton = new Button();
        colorDialog = new ColorDialog();
        UserDefinedImagesFolderGroupBox = new GroupBox();
        imagesFolderLabel = new Label();
        btnOpen = new Button();
        imagesFolderButton = new Button();
        saveFolderGroupBox.SuspendLayout();
        prefixGroupBox.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)digitNumUpDown).BeginInit();
        jpegQualityGroupBox.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)jpegQualityUpDown).BeginInit();
        saveIniGroupBox.SuspendLayout();
        defaultBackColorGroupBox.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)defaultBackColorPictureBox).BeginInit();
        UserDefinedImagesFolderGroupBox.SuspendLayout();
        SuspendLayout();
        // 
        // okButton
        // 
        resources.ApplyResources(okButton, "okButton");
        okButton.DialogResult = DialogResult.OK;
        okButton.Name = "okButton";
        okButton.UseVisualStyleBackColor = true;
        okButton.Click += OkButton_Click;
        // 
        // cancelButton
        // 
        resources.ApplyResources(cancelButton, "cancelButton");
        cancelButton.DialogResult = DialogResult.Cancel;
        cancelButton.Name = "cancelButton";
        cancelButton.UseVisualStyleBackColor = true;
        // 
        // saveFolderGroupBox
        // 
        resources.ApplyResources(saveFolderGroupBox, "saveFolderGroupBox");
        saveFolderGroupBox.Controls.Add(targetFolderButton);
        saveFolderGroupBox.Controls.Add(targetFolderLabel);
        saveFolderGroupBox.Controls.Add(previousFolderLabel);
        saveFolderGroupBox.Controls.Add(targetFolderRadioButton);
        saveFolderGroupBox.Controls.Add(previousFolderRadioButton);
        saveFolderGroupBox.Controls.Add(sameFolderRadioButton);
        saveFolderGroupBox.Name = "saveFolderGroupBox";
        saveFolderGroupBox.TabStop = false;
        // 
        // targetFolderButton
        // 
        resources.ApplyResources(targetFolderButton, "targetFolderButton");
        targetFolderButton.Name = "targetFolderButton";
        targetFolderButton.UseVisualStyleBackColor = true;
        targetFolderButton.Click += TargetFolderButton_Click;
        // 
        // targetFolderLabel
        // 
        resources.ApplyResources(targetFolderLabel, "targetFolderLabel");
        targetFolderLabel.AutoEllipsis = true;
        targetFolderLabel.Name = "targetFolderLabel";
        targetFolderLabel.TextChanged += TargetFolderLabel_TextChanged;
        // 
        // previousFolderLabel
        // 
        resources.ApplyResources(previousFolderLabel, "previousFolderLabel");
        previousFolderLabel.AutoEllipsis = true;
        previousFolderLabel.Name = "previousFolderLabel";
        // 
        // targetFolderRadioButton
        // 
        resources.ApplyResources(targetFolderRadioButton, "targetFolderRadioButton");
        targetFolderRadioButton.Name = "targetFolderRadioButton";
        targetFolderRadioButton.TabStop = true;
        targetFolderRadioButton.UseVisualStyleBackColor = true;
        targetFolderRadioButton.CheckedChanged += TargetFolderRadioButton_CheckedChanged;
        // 
        // previousFolderRadioButton
        // 
        resources.ApplyResources(previousFolderRadioButton, "previousFolderRadioButton");
        previousFolderRadioButton.Name = "previousFolderRadioButton";
        previousFolderRadioButton.TabStop = true;
        previousFolderRadioButton.UseVisualStyleBackColor = true;
        previousFolderRadioButton.CheckedChanged += PreviousFolderRadioButton_CheckedChanged;
        // 
        // sameFolderRadioButton
        // 
        resources.ApplyResources(sameFolderRadioButton, "sameFolderRadioButton");
        sameFolderRadioButton.Checked = true;
        sameFolderRadioButton.Name = "sameFolderRadioButton";
        sameFolderRadioButton.TabStop = true;
        sameFolderRadioButton.UseVisualStyleBackColor = true;
        sameFolderRadioButton.CheckedChanged += SameFolderRadioButton_CheckedChanged;
        // 
        // prefixGroupBox
        // 
        resources.ApplyResources(prefixGroupBox, "prefixGroupBox");
        prefixGroupBox.Controls.Add(digitNumUpDown);
        prefixGroupBox.Controls.Add(lblDigitNum);
        prefixGroupBox.Controls.Add(extensionLabel);
        prefixGroupBox.Controls.Add(extensionComboBox);
        prefixGroupBox.Controls.Add(isRenumberCheckBox);
        prefixGroupBox.Controls.Add(exLabel);
        prefixGroupBox.Controls.Add(prefixTextBox);
        prefixGroupBox.Controls.Add(prefixLabel);
        prefixGroupBox.Name = "prefixGroupBox";
        prefixGroupBox.TabStop = false;
        // 
        // digitNumUpDown
        // 
        resources.ApplyResources(digitNumUpDown, "digitNumUpDown");
        digitNumUpDown.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
        digitNumUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        digitNumUpDown.Name = "digitNumUpDown";
        digitNumUpDown.Value = new decimal(new int[] { 4, 0, 0, 0 });
        digitNumUpDown.ValueChanged += DigitNumUpDown_ValueChanged;
        // 
        // lblDigitNum
        // 
        resources.ApplyResources(lblDigitNum, "lblDigitNum");
        lblDigitNum.Name = "lblDigitNum";
        // 
        // extensionLabel
        // 
        resources.ApplyResources(extensionLabel, "extensionLabel");
        extensionLabel.Name = "extensionLabel";
        // 
        // extensionComboBox
        // 
        extensionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        extensionComboBox.FormattingEnabled = true;
        extensionComboBox.Items.AddRange(new object[] { resources.GetString("extensionComboBox.Items"), resources.GetString("extensionComboBox.Items1"), resources.GetString("extensionComboBox.Items2"), resources.GetString("extensionComboBox.Items3"), resources.GetString("extensionComboBox.Items4") });
        resources.ApplyResources(extensionComboBox, "extensionComboBox");
        extensionComboBox.Name = "extensionComboBox";
        extensionComboBox.SelectedIndexChanged += Prefix_Changed;
        // 
        // isRenumberCheckBox
        // 
        resources.ApplyResources(isRenumberCheckBox, "isRenumberCheckBox");
        isRenumberCheckBox.Name = "isRenumberCheckBox";
        isRenumberCheckBox.UseVisualStyleBackColor = true;
        isRenumberCheckBox.CheckedChanged += Prefix_Changed;
        // 
        // exLabel
        // 
        resources.ApplyResources(exLabel, "exLabel");
        exLabel.Name = "exLabel";
        // 
        // prefixTextBox
        // 
        resources.ApplyResources(prefixTextBox, "prefixTextBox");
        prefixTextBox.Name = "prefixTextBox";
        prefixTextBox.TextChanged += Prefix_Changed;
        // 
        // prefixLabel
        // 
        resources.ApplyResources(prefixLabel, "prefixLabel");
        prefixLabel.Name = "prefixLabel";
        // 
        // infoLabel
        // 
        resources.ApplyResources(infoLabel, "infoLabel");
        infoLabel.Name = "infoLabel";
        // 
        // defaultButton
        // 
        resources.ApplyResources(defaultButton, "defaultButton");
        defaultButton.Name = "defaultButton";
        defaultButton.UseVisualStyleBackColor = true;
        defaultButton.Click += DefaultButton_Click;
        // 
        // jpegQualityGroupBox
        // 
        jpegQualityGroupBox.Controls.Add(jpegQualityLabel);
        jpegQualityGroupBox.Controls.Add(jpegQualityUpDown);
        resources.ApplyResources(jpegQualityGroupBox, "jpegQualityGroupBox");
        jpegQualityGroupBox.Name = "jpegQualityGroupBox";
        jpegQualityGroupBox.TabStop = false;
        // 
        // jpegQualityLabel
        // 
        resources.ApplyResources(jpegQualityLabel, "jpegQualityLabel");
        jpegQualityLabel.Name = "jpegQualityLabel";
        // 
        // jpegQualityUpDown
        // 
        resources.ApplyResources(jpegQualityUpDown, "jpegQualityUpDown");
        jpegQualityUpDown.Name = "jpegQualityUpDown";
        jpegQualityUpDown.Value = new decimal(new int[] { 85, 0, 0, 0 });
        // 
        // saveIniGroupBox
        // 
        saveIniGroupBox.Controls.Add(saveIniCheckBox);
        saveIniGroupBox.Controls.Add(infoLabel);
        resources.ApplyResources(saveIniGroupBox, "saveIniGroupBox");
        saveIniGroupBox.Name = "saveIniGroupBox";
        saveIniGroupBox.TabStop = false;
        // 
        // saveIniCheckBox
        // 
        resources.ApplyResources(saveIniCheckBox, "saveIniCheckBox");
        saveIniCheckBox.Name = "saveIniCheckBox";
        saveIniCheckBox.UseVisualStyleBackColor = true;
        // 
        // defaultBackColorGroupBox
        // 
        defaultBackColorGroupBox.Controls.Add(defaultBackColorPictureBox);
        defaultBackColorGroupBox.Controls.Add(colorButton);
        resources.ApplyResources(defaultBackColorGroupBox, "defaultBackColorGroupBox");
        defaultBackColorGroupBox.Name = "defaultBackColorGroupBox";
        defaultBackColorGroupBox.TabStop = false;
        // 
        // defaultBackColorPictureBox
        // 
        defaultBackColorPictureBox.BackColor = Color.White;
        defaultBackColorPictureBox.BorderStyle = BorderStyle.Fixed3D;
        resources.ApplyResources(defaultBackColorPictureBox, "defaultBackColorPictureBox");
        defaultBackColorPictureBox.Name = "defaultBackColorPictureBox";
        defaultBackColorPictureBox.TabStop = false;
        // 
        // colorButton
        // 
        resources.ApplyResources(colorButton, "colorButton");
        colorButton.Name = "colorButton";
        colorButton.UseVisualStyleBackColor = true;
        colorButton.Click += ColorButton_Click;
        // 
        // colorDialog
        // 
        colorDialog.AnyColor = true;
        colorDialog.FullOpen = true;
        // 
        // UserDefinedImagesFolderGroupBox
        // 
        UserDefinedImagesFolderGroupBox.Controls.Add(imagesFolderLabel);
        UserDefinedImagesFolderGroupBox.Controls.Add(btnOpen);
        UserDefinedImagesFolderGroupBox.Controls.Add(imagesFolderButton);
        resources.ApplyResources(UserDefinedImagesFolderGroupBox, "UserDefinedImagesFolderGroupBox");
        UserDefinedImagesFolderGroupBox.Name = "UserDefinedImagesFolderGroupBox";
        UserDefinedImagesFolderGroupBox.TabStop = false;
        // 
        // imagesFolderLabel
        // 
        imagesFolderLabel.AutoEllipsis = true;
        resources.ApplyResources(imagesFolderLabel, "imagesFolderLabel");
        imagesFolderLabel.Name = "imagesFolderLabel";
        // 
        // btnOpen
        // 
        resources.ApplyResources(btnOpen, "btnOpen");
        btnOpen.Name = "btnOpen";
        btnOpen.UseVisualStyleBackColor = true;
        btnOpen.Click += BtnOpen_Click;
        // 
        // imagesFolderButton
        // 
        resources.ApplyResources(imagesFolderButton, "imagesFolderButton");
        imagesFolderButton.Name = "imagesFolderButton";
        imagesFolderButton.UseVisualStyleBackColor = true;
        imagesFolderButton.Click += ImagesFolderButton_Click;
        // 
        // SaveSettingsForm
        // 
        AcceptButton = okButton;
        resources.ApplyResources(this, "$this");
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = cancelButton;
        Controls.Add(UserDefinedImagesFolderGroupBox);
        Controls.Add(defaultBackColorGroupBox);
        Controls.Add(saveIniGroupBox);
        Controls.Add(jpegQualityGroupBox);
        Controls.Add(defaultButton);
        Controls.Add(prefixGroupBox);
        Controls.Add(saveFolderGroupBox);
        Controls.Add(cancelButton);
        Controls.Add(okButton);
        FormBorderStyle = FormBorderStyle.FixedToolWindow;
        Name = "SaveSettingsForm";
        ShowIcon = false;
        Load += SaveSettingsForm_Load;
        saveFolderGroupBox.ResumeLayout(false);
        saveFolderGroupBox.PerformLayout();
        prefixGroupBox.ResumeLayout(false);
        prefixGroupBox.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)digitNumUpDown).EndInit();
        jpegQualityGroupBox.ResumeLayout(false);
        jpegQualityGroupBox.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)jpegQualityUpDown).EndInit();
        saveIniGroupBox.ResumeLayout(false);
        defaultBackColorGroupBox.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)defaultBackColorPictureBox).EndInit();
        UserDefinedImagesFolderGroupBox.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.GroupBox saveFolderGroupBox;
    private System.Windows.Forms.Label targetFolderLabel;
    private System.Windows.Forms.Label previousFolderLabel;
    private System.Windows.Forms.RadioButton targetFolderRadioButton;
    private System.Windows.Forms.RadioButton previousFolderRadioButton;
    private System.Windows.Forms.RadioButton sameFolderRadioButton;
    private System.Windows.Forms.Button targetFolderButton;
    private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    private System.Windows.Forms.GroupBox prefixGroupBox;
    private System.Windows.Forms.Label prefixLabel;
    private System.Windows.Forms.CheckBox isRenumberCheckBox;
    private System.Windows.Forms.Label exLabel;
    private System.Windows.Forms.TextBox prefixTextBox;
    private System.Windows.Forms.ComboBox extensionComboBox;
    private System.Windows.Forms.Label infoLabel;
    private System.Windows.Forms.Label extensionLabel;
    private System.Windows.Forms.Button defaultButton;
    private System.Windows.Forms.GroupBox jpegQualityGroupBox;
    private System.Windows.Forms.NumericUpDown jpegQualityUpDown;
    private System.Windows.Forms.Label jpegQualityLabel;
    private System.Windows.Forms.GroupBox saveIniGroupBox;
    private System.Windows.Forms.CheckBox saveIniCheckBox;
    private System.Windows.Forms.GroupBox defaultBackColorGroupBox;
    private System.Windows.Forms.PictureBox defaultBackColorPictureBox;
    private System.Windows.Forms.Button colorButton;
    private System.Windows.Forms.ColorDialog colorDialog;
    private System.Windows.Forms.Label lblDigitNum;
    private System.Windows.Forms.NumericUpDown digitNumUpDown;
    private System.Windows.Forms.GroupBox UserDefinedImagesFolderGroupBox;
    private System.Windows.Forms.Label imagesFolderLabel;
    private System.Windows.Forms.Button btnOpen;
    private System.Windows.Forms.Button imagesFolderButton;
}