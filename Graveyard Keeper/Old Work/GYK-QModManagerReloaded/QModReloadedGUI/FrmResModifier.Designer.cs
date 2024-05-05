using System.ComponentModel;
using System.Windows.Forms;

namespace QModReloadedGUI
{
    partial class FrmResModifier
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmResModifier));
            this.LblCurrentMaxWidth = new System.Windows.Forms.Label();
            this.LblCurrentMaxHeight = new System.Windows.Forms.Label();
            this.TxtMaxWidth = new System.Windows.Forms.TextBox();
            this.TxtMaxHeight = new System.Windows.Forms.TextBox();
            this.TxtRequestedMaxHeight = new System.Windows.Forms.TextBox();
            this.TxtRequestedMaxWidth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BtnApply = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // LblCurrentMaxWidth
            // 
            this.LblCurrentMaxWidth.AutoSize = true;
            this.LblCurrentMaxWidth.Location = new System.Drawing.Point(12, 9);
            this.LblCurrentMaxWidth.Name = "LblCurrentMaxWidth";
            this.LblCurrentMaxWidth.Size = new System.Drawing.Size(98, 17);
            this.LblCurrentMaxWidth.TabIndex = 0;
            this.LblCurrentMaxWidth.Text = "Current Max Width";
            this.LblCurrentMaxWidth.UseCompatibleTextRendering = true;
            // 
            // LblCurrentMaxHeight
            // 
            this.LblCurrentMaxHeight.AutoSize = true;
            this.LblCurrentMaxHeight.Location = new System.Drawing.Point(12, 66);
            this.LblCurrentMaxHeight.Name = "LblCurrentMaxHeight";
            this.LblCurrentMaxHeight.Size = new System.Drawing.Size(102, 17);
            this.LblCurrentMaxHeight.TabIndex = 1;
            this.LblCurrentMaxHeight.Text = "Current Max Height";
            this.LblCurrentMaxHeight.UseCompatibleTextRendering = true;
            // 
            // TxtMaxWidth
            // 
            this.TxtMaxWidth.Location = new System.Drawing.Point(12, 29);
            this.TxtMaxWidth.Name = "TxtMaxWidth";
            this.TxtMaxWidth.ReadOnly = true;
            this.TxtMaxWidth.Size = new System.Drawing.Size(140, 20);
            this.TxtMaxWidth.TabIndex = 2;
            // 
            // TxtMaxHeight
            // 
            this.TxtMaxHeight.Location = new System.Drawing.Point(12, 86);
            this.TxtMaxHeight.Name = "TxtMaxHeight";
            this.TxtMaxHeight.ReadOnly = true;
            this.TxtMaxHeight.Size = new System.Drawing.Size(140, 20);
            this.TxtMaxHeight.TabIndex = 3;
            // 
            // TxtRequestedMaxHeight
            // 
            this.TxtRequestedMaxHeight.Location = new System.Drawing.Point(12, 228);
            this.TxtRequestedMaxHeight.Name = "TxtRequestedMaxHeight";
            this.TxtRequestedMaxHeight.Size = new System.Drawing.Size(140, 20);
            this.TxtRequestedMaxHeight.TabIndex = 9;
            // 
            // TxtRequestedMaxWidth
            // 
            this.TxtRequestedMaxWidth.Location = new System.Drawing.Point(12, 171);
            this.TxtRequestedMaxWidth.Name = "TxtRequestedMaxWidth";
            this.TxtRequestedMaxWidth.Size = new System.Drawing.Size(140, 20);
            this.TxtRequestedMaxWidth.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 208);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Requested Max Height";
            this.label1.UseCompatibleTextRendering = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Requested Max Width";
            this.label2.UseCompatibleTextRendering = true;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(-12, 274);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(200, 2);
            this.label3.TabIndex = 13;
            // 
            // BtnApply
            // 
            this.BtnApply.Image = global::QModReloadedGUI.Properties.Resources.login;
            this.BtnApply.Location = new System.Drawing.Point(12, 290);
            this.BtnApply.Name = "BtnApply";
            this.BtnApply.Size = new System.Drawing.Size(140, 23);
            this.BtnApply.TabIndex = 14;
            this.BtnApply.Text = "A&pply Change";
            this.BtnApply.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.BtnApply, "Enter desired resolution and click. If all went well, should refresh and current " +
        "should be  requested.\r\n");
            this.BtnApply.UseCompatibleTextRendering = true;
            this.BtnApply.UseVisualStyleBackColor = true;
            this.BtnApply.Click += new System.EventHandler(this.BtnApply_Click);
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(-12, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(200, 2);
            this.label4.TabIndex = 15;
            // 
            // FrmResModifier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(170, 326);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.BtnApply);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TxtRequestedMaxHeight);
            this.Controls.Add(this.TxtRequestedMaxWidth);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TxtMaxHeight);
            this.Controls.Add(this.TxtMaxWidth);
            this.Controls.Add(this.LblCurrentMaxHeight);
            this.Controls.Add(this.LblCurrentMaxWidth);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmResModifier";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Resolution Modifier";
            this.Load += new System.EventHandler(this.ResModifier_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label LblCurrentMaxWidth;
        private Label LblCurrentMaxHeight;
        private TextBox TxtMaxWidth;
        private TextBox TxtMaxHeight;
        private TextBox TxtRequestedMaxHeight;
        private TextBox TxtRequestedMaxWidth;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button BtnApply;
        private Label label4;
        private ToolTip toolTip1;
    }
}