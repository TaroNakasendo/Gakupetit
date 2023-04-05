namespace Com.Nakasendo.Gakupetit;

partial class SizeForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SizeForm));
        oirginalLabel = new Label();
        label4 = new Label();
        label3 = new Label();
        autoHeightLabel = new Label();
        x1Label = new Label();
        x2Label = new Label();
        originalRadioButton = new RadioButton();
        groupBox1 = new GroupBox();
        cubicComboBox = new ComboBox();
        listComboBox = new ComboBox();
        autoHeightRadioButton = new RadioButton();
        cubicRadioButton = new RadioButton();
        freeSizeRadioButton = new RadioButton();
        listRadioButton = new RadioButton();
        okButton = new Button();
        cancelButton = new Button();
        autoHeightUpDown = new NumericUpDown();
        widthUpDown = new NumericUpDown();
        heightUpDown = new NumericUpDown();
        ((System.ComponentModel.ISupportInitialize)autoHeightUpDown).BeginInit();
        ((System.ComponentModel.ISupportInitialize)widthUpDown).BeginInit();
        ((System.ComponentModel.ISupportInitialize)heightUpDown).BeginInit();
        SuspendLayout();
        // 
        // oirginalLabel
        // 
        resources.ApplyResources(oirginalLabel, "oirginalLabel");
        oirginalLabel.Name = "oirginalLabel";
        // 
        // label4
        // 
        resources.ApplyResources(label4, "label4");
        label4.Name = "label4";
        // 
        // label3
        // 
        resources.ApplyResources(label3, "label3");
        label3.Name = "label3";
        // 
        // autoHeightLabel
        // 
        resources.ApplyResources(autoHeightLabel, "autoHeightLabel");
        autoHeightLabel.Name = "autoHeightLabel";
        // 
        // x1Label
        // 
        resources.ApplyResources(x1Label, "x1Label");
        x1Label.Name = "x1Label";
        // 
        // x2Label
        // 
        resources.ApplyResources(x2Label, "x2Label");
        x2Label.Name = "x2Label";
        // 
        // originalRadioButton
        // 
        resources.ApplyResources(originalRadioButton, "originalRadioButton");
        originalRadioButton.Name = "originalRadioButton";
        // 
        // groupBox1
        // 
        resources.ApplyResources(groupBox1, "groupBox1");
        groupBox1.Name = "groupBox1";
        groupBox1.TabStop = false;
        // 
        // cubicComboBox
        // 
        cubicComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        resources.ApplyResources(cubicComboBox, "cubicComboBox");
        cubicComboBox.Name = "cubicComboBox";
        cubicComboBox.Enter += CubicComboBox_Enter;
        // 
        // listComboBox
        // 
        listComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        resources.ApplyResources(listComboBox, "listComboBox");
        listComboBox.Name = "listComboBox";
        listComboBox.Enter += ListComboBox_Enter;
        // 
        // autoHeightRadioButton
        // 
        resources.ApplyResources(autoHeightRadioButton, "autoHeightRadioButton");
        autoHeightRadioButton.Name = "autoHeightRadioButton";
        // 
        // cubicRadioButton
        // 
        resources.ApplyResources(cubicRadioButton, "cubicRadioButton");
        cubicRadioButton.Name = "cubicRadioButton";
        // 
        // freeSizeRadioButton
        // 
        resources.ApplyResources(freeSizeRadioButton, "freeSizeRadioButton");
        freeSizeRadioButton.Name = "freeSizeRadioButton";
        // 
        // listRadioButton
        // 
        listRadioButton.Checked = true;
        resources.ApplyResources(listRadioButton, "listRadioButton");
        listRadioButton.Name = "listRadioButton";
        listRadioButton.TabStop = true;
        // 
        // okButton
        // 
        resources.ApplyResources(okButton, "okButton");
        okButton.Name = "okButton";
        okButton.Click += OKButton_Click;
        // 
        // cancelButton
        // 
        cancelButton.DialogResult = DialogResult.Cancel;
        resources.ApplyResources(cancelButton, "cancelButton");
        cancelButton.Name = "cancelButton";
        // 
        // autoHeightUpDown
        // 
        resources.ApplyResources(autoHeightUpDown, "autoHeightUpDown");
        autoHeightUpDown.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
        autoHeightUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        autoHeightUpDown.Name = "autoHeightUpDown";
        autoHeightUpDown.Value = new decimal(new int[] { 640, 0, 0, 0 });
        autoHeightUpDown.ValueChanged += AutoHeightUpDown_ValueChanged;
        autoHeightUpDown.Enter += AutoHeightUpDown_Enter;
        autoHeightUpDown.KeyUp += AutoHeightUpDown_KeyUp;
        // 
        // widthUpDown
        // 
        resources.ApplyResources(widthUpDown, "widthUpDown");
        widthUpDown.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
        widthUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        widthUpDown.Name = "widthUpDown";
        widthUpDown.Value = new decimal(new int[] { 640, 0, 0, 0 });
        widthUpDown.Enter += WidthHeightUpDown_Enter;
        // 
        // heightUpDown
        // 
        resources.ApplyResources(heightUpDown, "heightUpDown");
        heightUpDown.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
        heightUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        heightUpDown.Name = "heightUpDown";
        heightUpDown.Value = new decimal(new int[] { 480, 0, 0, 0 });
        heightUpDown.Enter += WidthHeightUpDown_Enter;
        // 
        // SizeForm
        // 
        AcceptButton = okButton;
        resources.ApplyResources(this, "$this");
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = cancelButton;
        Controls.Add(heightUpDown);
        Controls.Add(widthUpDown);
        Controls.Add(autoHeightUpDown);
        Controls.Add(oirginalLabel);
        Controls.Add(label4);
        Controls.Add(label3);
        Controls.Add(autoHeightLabel);
        Controls.Add(x1Label);
        Controls.Add(x2Label);
        Controls.Add(originalRadioButton);
        Controls.Add(groupBox1);
        Controls.Add(cubicComboBox);
        Controls.Add(listComboBox);
        Controls.Add(autoHeightRadioButton);
        Controls.Add(cubicRadioButton);
        Controls.Add(freeSizeRadioButton);
        Controls.Add(listRadioButton);
        Controls.Add(okButton);
        Controls.Add(cancelButton);
        FormBorderStyle = FormBorderStyle.FixedToolWindow;
        Name = "SizeForm";
        ShowIcon = false;
        Load += SizeForm_Load;
        ((System.ComponentModel.ISupportInitialize)autoHeightUpDown).EndInit();
        ((System.ComponentModel.ISupportInitialize)widthUpDown).EndInit();
        ((System.ComponentModel.ISupportInitialize)heightUpDown).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label oirginalLabel;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label autoHeightLabel;
    private System.Windows.Forms.Label x1Label;
    private System.Windows.Forms.Label x2Label;
    private System.Windows.Forms.RadioButton originalRadioButton;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.ComboBox cubicComboBox;
    private System.Windows.Forms.ComboBox listComboBox;
    private System.Windows.Forms.RadioButton autoHeightRadioButton;
    private System.Windows.Forms.RadioButton cubicRadioButton;
    private System.Windows.Forms.RadioButton freeSizeRadioButton;
    private System.Windows.Forms.RadioButton listRadioButton;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.NumericUpDown autoHeightUpDown;
    private System.Windows.Forms.NumericUpDown widthUpDown;
    private System.Windows.Forms.NumericUpDown heightUpDown;
}