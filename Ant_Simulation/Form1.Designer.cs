namespace Ant_Simulation
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
            this.Step = new System.Windows.Forms.Button();
            this.Step5 = new System.Windows.Forms.Button();
            this.Step10 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.GameBoardBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.GameBoardBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Step
            // 
            this.Step.Location = new System.Drawing.Point(12, 12);
            this.Step.Name = "Step";
            this.Step.Size = new System.Drawing.Size(82, 39);
            this.Step.TabIndex = 0;
            this.Step.Text = "Step";
            this.Step.UseVisualStyleBackColor = true;
            this.Step.Click += new System.EventHandler(this.Step_Click_1);
            // 
            // Step5
            // 
            this.Step5.Location = new System.Drawing.Point(100, 12);
            this.Step5.Name = "Step5";
            this.Step5.Size = new System.Drawing.Size(82, 39);
            this.Step5.TabIndex = 1;
            this.Step5.Text = "Step 5";
            this.Step5.UseVisualStyleBackColor = true;
            this.Step5.Click += new System.EventHandler(this.Step5_Click);
            // 
            // Step10
            // 
            this.Step10.Location = new System.Drawing.Point(188, 12);
            this.Step10.Name = "Step10";
            this.Step10.Size = new System.Drawing.Size(82, 39);
            this.Step10.TabIndex = 2;
            this.Step10.Text = "Step 10";
            this.Step10.UseVisualStyleBackColor = true;
            this.Step10.Click += new System.EventHandler(this.Step10_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(403, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 39);
            this.button1.TabIndex = 3;
            this.button1.Text = "Generate New Board";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // GameBoardBox
            // 
            this.GameBoardBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GameBoardBox.Location = new System.Drawing.Point(12, 57);
            this.GameBoardBox.Name = "GameBoardBox";
            this.GameBoardBox.Size = new System.Drawing.Size(517, 457);
            this.GameBoardBox.TabIndex = 4;
            this.GameBoardBox.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 526);
            this.Controls.Add(this.GameBoardBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Step10);
            this.Controls.Add(this.Step5);
            this.Controls.Add(this.Step);
            this.MinimumSize = new System.Drawing.Size(425, 300);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.GameBoardBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Step;
        private System.Windows.Forms.Button Step5;
        private System.Windows.Forms.Button Step10;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox GameBoardBox;
    }
}

