namespace Mini_project_WebBrowser
{
    partial class victorform
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
            this.labevictor = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.addvform = new System.Windows.Forms.Button();
            this.cancelv = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // labevictor
            //
            this.labevictor.AutoSize = true;
            this.labevictor.Location = new System.Drawing.Point(40, 30);
            this.labevictor.Name = "labevictor";
            this.labevictor.Size = new System.Drawing.Size(261, 13);
            this.labevictor.TabIndex = 0;
            this.labevictor.Text = "Enter a word to block (pages containing it won\'t open):";
            //
            // textBox1
            //
            this.textBox1.Location = new System.Drawing.Point(43, 55);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(290, 20);
            this.textBox1.TabIndex = 1;
            //
            // addvform
            //
            this.addvform.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.addvform.Location = new System.Drawing.Point(150, 100);
            this.addvform.Name = "addvform";
            this.addvform.Size = new System.Drawing.Size(85, 27);
            this.addvform.TabIndex = 2;
            this.addvform.Text = "OK";
            this.addvform.UseVisualStyleBackColor = true;
            //
            // cancelv
            //
            this.cancelv.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelv.Location = new System.Drawing.Point(248, 100);
            this.cancelv.Name = "cancelv";
            this.cancelv.Size = new System.Drawing.Size(85, 27);
            this.cancelv.TabIndex = 3;
            this.cancelv.Text = "Cancel";
            this.cancelv.UseVisualStyleBackColor = true;
            //
            // victorform
            //
            this.AcceptButton = this.addvform;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelv;
            this.ClientSize = new System.Drawing.Size(380, 150);
            this.Controls.Add(this.cancelv);
            this.Controls.Add(this.addvform);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.labevictor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "victorform";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add blocked word";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labevictor;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button addvform;
        private System.Windows.Forms.Button cancelv;
    }
}
