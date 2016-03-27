namespace HyperionDX
{
  partial class Form1
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
      this.btnTakeImage = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // btnTakeImage
      // 
      this.btnTakeImage.Location = new System.Drawing.Point(12, 113);
      this.btnTakeImage.Name = "btnTakeImage";
      this.btnTakeImage.Size = new System.Drawing.Size(260, 136);
      this.btnTakeImage.TabIndex = 0;
      this.btnTakeImage.Text = "Take image and send to Hyperion";
      this.btnTakeImage.UseVisualStyleBackColor = true;
      this.btnTakeImage.Click += new System.EventHandler(this.btnTakeImage_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(284, 261);
      this.Controls.Add(this.btnTakeImage);
      this.Name = "Form1";
      this.Text = "Form1";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btnTakeImage;
  }
}

