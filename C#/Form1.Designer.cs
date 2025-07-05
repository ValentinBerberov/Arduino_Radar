namespace ArduinoInterfacing;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    public PictureBox pictureBox;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1200, 700);
        Text = "Radar";

        pictureBox = new();
        pictureBox.Location = new Point(0, 0);
        pictureBox.Width = 1200;
        pictureBox.Height = 650;
        pictureBox.Visible = true;

        Controls.Add(pictureBox);
        
    }

    #endregion
}
