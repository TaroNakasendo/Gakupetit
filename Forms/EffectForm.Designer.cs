namespace Com.Nakasendo.Gakupetit;

partial class EffectForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EffectForm));
        effectListView = new ListView();
        SuspendLayout();
        // 
        // effectListView
        // 
        effectListView.Activation = ItemActivation.OneClick;
        effectListView.BackColor = SystemColors.Window;
        resources.ApplyResources(effectListView, "effectListView");
        effectListView.FullRowSelect = true;
        effectListView.GridLines = true;
        effectListView.HoverSelection = true;
        effectListView.MultiSelect = false;
        effectListView.Name = "effectListView";
        effectListView.ShowGroups = false;
        effectListView.ShowItemToolTips = true;
        effectListView.UseCompatibleStateImageBehavior = false;
        effectListView.SelectedIndexChanged += EffectListView_SelectedIndexChanged;
        effectListView.Click += EffectListView_Click;
        // 
        // EffectForm
        // 
        resources.ApplyResources(this, "$this");
        AutoScaleMode = AutoScaleMode.Dpi;
        Controls.Add(effectListView);
        FormBorderStyle = FormBorderStyle.SizableToolWindow;
        Name = "EffectForm";
        ShowIcon = false;
        ShowInTaskbar = false;
        Load += EffectForm_Load;
        ResumeLayout(false);
    }

    #endregion
    private System.Windows.Forms.ListView effectListView;
}