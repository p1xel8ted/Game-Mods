using System.ComponentModel;
using System.Windows.Forms;

namespace QModReloadedGUI
{
    partial class FrmChecklist
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmChecklist));
            this.ChkModPatched = new System.Windows.Forms.CheckBox();
            this.ChkModDirectoryExists = new System.Windows.Forms.CheckBox();
            this.ChkGameLocation = new System.Windows.Forms.CheckBox();
            this.ChkMonoCecilExists = new System.Windows.Forms.CheckBox();
            this.Chk0HarmonyExists = new System.Windows.Forms.CheckBox();
            this.ChkPatcherLocation = new System.Windows.Forms.CheckBox();
            this.ChkInjector = new System.Windows.Forms.CheckBox();
            this.ChkConfig = new System.Windows.Forms.CheckBox();
            this.ChkUnsafeExists = new System.Windows.Forms.CheckBox();
            this.ChkNumericsExists = new System.Windows.Forms.CheckBox();
            this.ChkMemoryExists = new System.Windows.Forms.CheckBox();
            this.ChkBuffersExists = new System.Windows.Forms.CheckBox();
            this.ChkBCLExists = new System.Windows.Forms.CheckBox();
            this.ChkThreadingExists = new System.Windows.Forms.CheckBox();
            this.ChkJsonExists = new System.Windows.Forms.CheckBox();
            this.ChkEncodingsExists = new System.Windows.Forms.CheckBox();
            this.ChkTupleExists = new System.Windows.Forms.CheckBox();
            this.ChkAssemblyExists = new System.Windows.Forms.CheckBox();
            this.LblHarmonyVersion = new System.Windows.Forms.Label();
            this.ChckSystemCodePages = new System.Windows.Forms.CheckBox();
            this.ChckMonoCecilInject = new System.Windows.Forms.CheckBox();
            this.ChkHelper = new System.Windows.Forms.CheckBox();
            this.ChkDotNetZip = new System.Windows.Forms.CheckBox();
            this.ChkInject = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // ChkModPatched
            // 
            this.ChkModPatched.AutoCheck = false;
            this.ChkModPatched.AutoSize = true;
            this.ChkModPatched.ForeColor = System.Drawing.SystemColors.Desktop;
            this.ChkModPatched.Location = new System.Drawing.Point(11, 11);
            this.ChkModPatched.Name = "ChkModPatched";
            this.ChkModPatched.Size = new System.Drawing.Size(277, 18);
            this.ChkModPatched.TabIndex = 0;
            this.ChkModPatched.Text = "Mod Patch Applied (Required for mods to function)";
            this.ChkModPatched.UseCompatibleTextRendering = true;
            this.ChkModPatched.UseVisualStyleBackColor = true;
            // 
            // ChkModDirectoryExists
            // 
            this.ChkModDirectoryExists.AutoCheck = false;
            this.ChkModDirectoryExists.AutoSize = true;
            this.ChkModDirectoryExists.ForeColor = System.Drawing.SystemColors.Desktop;
            this.ChkModDirectoryExists.Location = new System.Drawing.Point(11, 59);
            this.ChkModDirectoryExists.Name = "ChkModDirectoryExists";
            this.ChkModDirectoryExists.Size = new System.Drawing.Size(127, 18);
            this.ChkModDirectoryExists.TabIndex = 2;
            this.ChkModDirectoryExists.Text = "Mod Directory Exists";
            this.ChkModDirectoryExists.UseCompatibleTextRendering = true;
            this.ChkModDirectoryExists.UseVisualStyleBackColor = true;
            // 
            // ChkGameLocation
            // 
            this.ChkGameLocation.AutoCheck = false;
            this.ChkGameLocation.AutoSize = true;
            this.ChkGameLocation.ForeColor = System.Drawing.SystemColors.Desktop;
            this.ChkGameLocation.Location = new System.Drawing.Point(11, 35);
            this.ChkGameLocation.Name = "ChkGameLocation";
            this.ChkGameLocation.Size = new System.Drawing.Size(158, 18);
            this.ChkGameLocation.TabIndex = 3;
            this.ChkGameLocation.Text = "Game Location Configured";
            this.ChkGameLocation.UseCompatibleTextRendering = true;
            this.ChkGameLocation.UseVisualStyleBackColor = true;
            // 
            // ChkMonoCecilExists
            // 
            this.ChkMonoCecilExists.AutoCheck = false;
            this.ChkMonoCecilExists.AutoSize = true;
            this.ChkMonoCecilExists.ForeColor = System.Drawing.SystemColors.Desktop;
            this.ChkMonoCecilExists.Location = new System.Drawing.Point(11, 131);
            this.ChkMonoCecilExists.Name = "ChkMonoCecilExists";
            this.ChkMonoCecilExists.Size = new System.Drawing.Size(95, 18);
            this.ChkMonoCecilExists.TabIndex = 4;
            this.ChkMonoCecilExists.Text = "Mono.Cecil.dll";
            this.ChkMonoCecilExists.UseCompatibleTextRendering = true;
            this.ChkMonoCecilExists.UseVisualStyleBackColor = true;
            // 
            // Chk0HarmonyExists
            // 
            this.Chk0HarmonyExists.AutoCheck = false;
            this.Chk0HarmonyExists.AutoSize = true;
            this.Chk0HarmonyExists.ForeColor = System.Drawing.SystemColors.Desktop;
            this.Chk0HarmonyExists.Location = new System.Drawing.Point(11, 107);
            this.Chk0HarmonyExists.Name = "Chk0HarmonyExists";
            this.Chk0HarmonyExists.Size = new System.Drawing.Size(90, 18);
            this.Chk0HarmonyExists.TabIndex = 5;
            this.Chk0HarmonyExists.Text = "0Harmony.dll";
            this.Chk0HarmonyExists.UseCompatibleTextRendering = true;
            this.Chk0HarmonyExists.UseVisualStyleBackColor = true;
            // 
            // ChkPatcherLocation
            // 
            this.ChkPatcherLocation.AutoCheck = false;
            this.ChkPatcherLocation.AutoSize = true;
            this.ChkPatcherLocation.ForeColor = System.Drawing.SystemColors.Desktop;
            this.ChkPatcherLocation.Location = new System.Drawing.Point(11, 539);
            this.ChkPatcherLocation.Name = "ChkPatcherLocation";
            this.ChkPatcherLocation.Size = new System.Drawing.Size(260, 43);
            this.ChkPatcherLocation.TabIndex = 6;
            this.ChkPatcherLocation.Text = "Patcher and associated files located in \r\n\"Graveyard Keeper_Data\\Managed\" directo" +
    "ry \r\n(Required for everything to function)";
            this.ChkPatcherLocation.UseCompatibleTextRendering = true;
            this.ChkPatcherLocation.UseVisualStyleBackColor = true;
            // 
            // ChkInjector
            // 
            this.ChkInjector.AutoCheck = false;
            this.ChkInjector.AutoSize = true;
            this.ChkInjector.ForeColor = System.Drawing.SystemColors.Desktop;
            this.ChkInjector.Location = new System.Drawing.Point(11, 491);
            this.ChkInjector.Name = "ChkInjector";
            this.ChkInjector.Size = new System.Drawing.Size(117, 18);
            this.ChkInjector.TabIndex = 10;
            this.ChkInjector.Text = "QModReloaded.dll";
            this.ChkInjector.UseCompatibleTextRendering = true;
            this.ChkInjector.UseVisualStyleBackColor = true;
            // 
            // ChkConfig
            // 
            this.ChkConfig.AutoCheck = false;
            this.ChkConfig.AutoSize = true;
            this.ChkConfig.ForeColor = System.Drawing.SystemColors.Desktop;
            this.ChkConfig.Location = new System.Drawing.Point(11, 515);
            this.ChkConfig.Name = "ChkConfig";
            this.ChkConfig.Size = new System.Drawing.Size(178, 18);
            this.ChkConfig.TabIndex = 11;
            this.ChkConfig.Text = "QModReloadedGUI.exe.config";
            this.ChkConfig.UseCompatibleTextRendering = true;
            this.ChkConfig.UseVisualStyleBackColor = true;
            // 
            // ChkUnsafeExists
            // 
            this.ChkUnsafeExists.AutoCheck = false;
            this.ChkUnsafeExists.AutoSize = true;
            this.ChkUnsafeExists.ForeColor = System.Drawing.SystemColors.Desktop;
            this.ChkUnsafeExists.Location = new System.Drawing.Point(11, 251);
            this.ChkUnsafeExists.Name = "ChkUnsafeExists";
            this.ChkUnsafeExists.Size = new System.Drawing.Size(252, 18);
            this.ChkUnsafeExists.TabIndex = 12;
            this.ChkUnsafeExists.Text = "System.Runtime.CompilerServices.Unsafe.dll";
            this.ChkUnsafeExists.UseCompatibleTextRendering = true;
            this.ChkUnsafeExists.UseVisualStyleBackColor = true;
            // 
            // ChkNumericsExists
            // 
            this.ChkNumericsExists.AutoCheck = false;
            this.ChkNumericsExists.AutoSize = true;
            this.ChkNumericsExists.ForeColor = System.Drawing.SystemColors.Desktop;
            this.ChkNumericsExists.Location = new System.Drawing.Point(11, 227);
            this.ChkNumericsExists.Name = "ChkNumericsExists";
            this.ChkNumericsExists.Size = new System.Drawing.Size(168, 18);
            this.ChkNumericsExists.TabIndex = 13;
            this.ChkNumericsExists.Text = "System.Numerics.Vectors.dll";
            this.ChkNumericsExists.UseCompatibleTextRendering = true;
            this.ChkNumericsExists.UseVisualStyleBackColor = true;
            // 
            // ChkMemoryExists
            // 
            this.ChkMemoryExists.AutoCheck = false;
            this.ChkMemoryExists.AutoSize = true;
            this.ChkMemoryExists.ForeColor = System.Drawing.SystemColors.Desktop;
            this.ChkMemoryExists.Location = new System.Drawing.Point(11, 203);
            this.ChkMemoryExists.Name = "ChkMemoryExists";
            this.ChkMemoryExists.Size = new System.Drawing.Size(120, 18);
            this.ChkMemoryExists.TabIndex = 14;
            this.ChkMemoryExists.Text = "System.Memory.dll";
            this.ChkMemoryExists.UseCompatibleTextRendering = true;
            this.ChkMemoryExists.UseVisualStyleBackColor = true;
            // 
            // ChkBuffersExists
            // 
            this.ChkBuffersExists.AutoCheck = false;
            this.ChkBuffersExists.AutoSize = true;
            this.ChkBuffersExists.ForeColor = System.Drawing.SystemColors.Desktop;
            this.ChkBuffersExists.Location = new System.Drawing.Point(11, 179);
            this.ChkBuffersExists.Name = "ChkBuffersExists";
            this.ChkBuffersExists.Size = new System.Drawing.Size(115, 18);
            this.ChkBuffersExists.TabIndex = 15;
            this.ChkBuffersExists.Text = "System.Buffers.dll";
            this.ChkBuffersExists.UseCompatibleTextRendering = true;
            this.ChkBuffersExists.UseVisualStyleBackColor = true;
            // 
            // ChkBCLExists
            // 
            this.ChkBCLExists.AutoCheck = false;
            this.ChkBCLExists.AutoSize = true;
            this.ChkBCLExists.ForeColor = System.Drawing.SystemColors.Desktop;
            this.ChkBCLExists.Location = new System.Drawing.Point(11, 155);
            this.ChkBCLExists.Name = "ChkBCLExists";
            this.ChkBCLExists.Size = new System.Drawing.Size(187, 18);
            this.ChkBCLExists.TabIndex = 16;
            this.ChkBCLExists.Text = "Microsoft.Bcl.AsyncInterfaces.dll";
            this.ChkBCLExists.UseCompatibleTextRendering = true;
            this.ChkBCLExists.UseVisualStyleBackColor = true;
            // 
            // ChkThreadingExists
            // 
            this.ChkThreadingExists.AutoCheck = false;
            this.ChkThreadingExists.AutoSize = true;
            this.ChkThreadingExists.ForeColor = System.Drawing.SystemColors.Desktop;
            this.ChkThreadingExists.Location = new System.Drawing.Point(11, 323);
            this.ChkThreadingExists.Name = "ChkThreadingExists";
            this.ChkThreadingExists.Size = new System.Drawing.Size(222, 18);
            this.ChkThreadingExists.TabIndex = 17;
            this.ChkThreadingExists.Text = "System.Threading.Tasks.Extensions.dll";
            this.ChkThreadingExists.UseCompatibleTextRendering = true;
            this.ChkThreadingExists.UseVisualStyleBackColor = true;
            // 
            // ChkJsonExists
            // 
            this.ChkJsonExists.AutoCheck = false;
            this.ChkJsonExists.AutoSize = true;
            this.ChkJsonExists.ForeColor = System.Drawing.SystemColors.Desktop;
            this.ChkJsonExists.Location = new System.Drawing.Point(11, 299);
            this.ChkJsonExists.Name = "ChkJsonExists";
            this.ChkJsonExists.Size = new System.Drawing.Size(128, 18);
            this.ChkJsonExists.TabIndex = 18;
            this.ChkJsonExists.Text = "System.Text.Json.dll";
            this.ChkJsonExists.UseCompatibleTextRendering = true;
            this.ChkJsonExists.UseVisualStyleBackColor = true;
            // 
            // ChkEncodingsExists
            // 
            this.ChkEncodingsExists.AutoCheck = false;
            this.ChkEncodingsExists.AutoSize = true;
            this.ChkEncodingsExists.ForeColor = System.Drawing.SystemColors.Desktop;
            this.ChkEncodingsExists.Location = new System.Drawing.Point(11, 275);
            this.ChkEncodingsExists.Name = "ChkEncodingsExists";
            this.ChkEncodingsExists.Size = new System.Drawing.Size(183, 18);
            this.ChkEncodingsExists.TabIndex = 19;
            this.ChkEncodingsExists.Text = "System.Text.Encodings.Web.dll";
            this.ChkEncodingsExists.UseCompatibleTextRendering = true;
            this.ChkEncodingsExists.UseVisualStyleBackColor = true;
            // 
            // ChkTupleExists
            // 
            this.ChkTupleExists.AutoCheck = false;
            this.ChkTupleExists.AutoSize = true;
            this.ChkTupleExists.ForeColor = System.Drawing.SystemColors.Desktop;
            this.ChkTupleExists.Location = new System.Drawing.Point(11, 347);
            this.ChkTupleExists.Name = "ChkTupleExists";
            this.ChkTupleExists.Size = new System.Drawing.Size(136, 18);
            this.ChkTupleExists.TabIndex = 20;
            this.ChkTupleExists.Text = "System.ValueTuple.dll";
            this.ChkTupleExists.UseCompatibleTextRendering = true;
            this.ChkTupleExists.UseVisualStyleBackColor = true;
            // 
            // ChkAssemblyExists
            // 
            this.ChkAssemblyExists.AutoCheck = false;
            this.ChkAssemblyExists.AutoSize = true;
            this.ChkAssemblyExists.ForeColor = System.Drawing.SystemColors.Desktop;
            this.ChkAssemblyExists.Location = new System.Drawing.Point(11, 83);
            this.ChkAssemblyExists.Name = "ChkAssemblyExists";
            this.ChkAssemblyExists.Size = new System.Drawing.Size(129, 18);
            this.ChkAssemblyExists.TabIndex = 21;
            this.ChkAssemblyExists.Text = "Assembly-CSharp.dll";
            this.ChkAssemblyExists.UseCompatibleTextRendering = true;
            this.ChkAssemblyExists.UseVisualStyleBackColor = true;
            // 
            // LblHarmonyVersion
            // 
            this.LblHarmonyVersion.AutoSize = true;
            this.LblHarmonyVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblHarmonyVersion.Location = new System.Drawing.Point(100, 108);
            this.LblHarmonyVersion.Name = "LblHarmonyVersion";
            this.LblHarmonyVersion.Size = new System.Drawing.Size(41, 13);
            this.LblHarmonyVersion.TabIndex = 22;
            this.LblHarmonyVersion.Text = "label1";
            // 
            // ChckSystemCodePages
            // 
            this.ChckSystemCodePages.AutoCheck = false;
            this.ChckSystemCodePages.AutoSize = true;
            this.ChckSystemCodePages.ForeColor = System.Drawing.SystemColors.Desktop;
            this.ChckSystemCodePages.Location = new System.Drawing.Point(11, 395);
            this.ChckSystemCodePages.Name = "ChckSystemCodePages";
            this.ChckSystemCodePages.Size = new System.Drawing.Size(214, 18);
            this.ChckSystemCodePages.TabIndex = 23;
            this.ChckSystemCodePages.Text = "System.Text.Encoding.CodePages.dll";
            this.ChckSystemCodePages.UseCompatibleTextRendering = true;
            this.ChckSystemCodePages.UseVisualStyleBackColor = true;
            // 
            // ChckMonoCecilInject
            // 
            this.ChckMonoCecilInject.AutoCheck = false;
            this.ChckMonoCecilInject.AutoSize = true;
            this.ChckMonoCecilInject.ForeColor = System.Drawing.SystemColors.Desktop;
            this.ChckMonoCecilInject.Location = new System.Drawing.Point(11, 371);
            this.ChckMonoCecilInject.Name = "ChckMonoCecilInject";
            this.ChckMonoCecilInject.Size = new System.Drawing.Size(125, 18);
            this.ChckMonoCecilInject.TabIndex = 24;
            this.ChckMonoCecilInject.Text = "Mono.Cecil.Inject.dll";
            this.ChckMonoCecilInject.UseCompatibleTextRendering = true;
            this.ChckMonoCecilInject.UseVisualStyleBackColor = true;
            // 
            // ChkHelper
            // 
            this.ChkHelper.AutoCheck = false;
            this.ChkHelper.AutoSize = true;
            this.ChkHelper.ForeColor = System.Drawing.SystemColors.Desktop;
            this.ChkHelper.Location = new System.Drawing.Point(11, 467);
            this.ChkHelper.Name = "ChkHelper";
            this.ChkHelper.Size = new System.Drawing.Size(71, 18);
            this.ChkHelper.TabIndex = 25;
            this.ChkHelper.Text = "Helper.dll";
            this.ChkHelper.UseCompatibleTextRendering = true;
            this.ChkHelper.UseVisualStyleBackColor = true;
            // 
            // ChkDotNetZip
            // 
            this.ChkDotNetZip.AutoCheck = false;
            this.ChkDotNetZip.AutoSize = true;
            this.ChkDotNetZip.ForeColor = System.Drawing.SystemColors.Desktop;
            this.ChkDotNetZip.Location = new System.Drawing.Point(11, 443);
            this.ChkDotNetZip.Name = "ChkDotNetZip";
            this.ChkDotNetZip.Size = new System.Drawing.Size(89, 18);
            this.ChkDotNetZip.TabIndex = 26;
            this.ChkDotNetZip.Text = "DotNetZip.dll";
            this.ChkDotNetZip.UseCompatibleTextRendering = true;
            this.ChkDotNetZip.UseVisualStyleBackColor = true;
            // 
            // ChkInject
            // 
            this.ChkInject.AutoCheck = false;
            this.ChkInject.AutoSize = true;
            this.ChkInject.ForeColor = System.Drawing.SystemColors.Desktop;
            this.ChkInject.Location = new System.Drawing.Point(11, 419);
            this.ChkInject.Name = "ChkInject";
            this.ChkInject.Size = new System.Drawing.Size(125, 18);
            this.ChkInject.TabIndex = 27;
            this.ChkInject.Text = "Mono.Cecil.Inject.dll";
            this.ChkInject.UseCompatibleTextRendering = true;
            this.ChkInject.UseVisualStyleBackColor = true;
            // 
            // FrmChecklist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 588);
            this.Controls.Add(this.ChkInject);
            this.Controls.Add(this.ChkDotNetZip);
            this.Controls.Add(this.ChkHelper);
            this.Controls.Add(this.ChckMonoCecilInject);
            this.Controls.Add(this.ChckSystemCodePages);
            this.Controls.Add(this.LblHarmonyVersion);
            this.Controls.Add(this.ChkAssemblyExists);
            this.Controls.Add(this.ChkTupleExists);
            this.Controls.Add(this.ChkEncodingsExists);
            this.Controls.Add(this.ChkJsonExists);
            this.Controls.Add(this.ChkThreadingExists);
            this.Controls.Add(this.ChkBCLExists);
            this.Controls.Add(this.ChkBuffersExists);
            this.Controls.Add(this.ChkMemoryExists);
            this.Controls.Add(this.ChkNumericsExists);
            this.Controls.Add(this.ChkUnsafeExists);
            this.Controls.Add(this.ChkConfig);
            this.Controls.Add(this.ChkInjector);
            this.Controls.Add(this.ChkPatcherLocation);
            this.Controls.Add(this.Chk0HarmonyExists);
            this.Controls.Add(this.ChkMonoCecilExists);
            this.Controls.Add(this.ChkGameLocation);
            this.Controls.Add(this.ChkModDirectoryExists);
            this.Controls.Add(this.ChkModPatched);
            this.ForeColor = System.Drawing.Color.Red;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmChecklist";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Checklist";
            this.Load += new System.EventHandler(this.FrmChecklist_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CheckBox ChkModPatched;
        private CheckBox ChkModDirectoryExists;
        private CheckBox ChkGameLocation;
        private CheckBox ChkMonoCecilExists;
        private CheckBox Chk0HarmonyExists;
        private CheckBox ChkPatcherLocation;
        private CheckBox ChkInjector;
        private CheckBox ChkConfig;
        private CheckBox ChkUnsafeExists;
        private CheckBox ChkNumericsExists;
        private CheckBox ChkMemoryExists;
        private CheckBox ChkBuffersExists;
        private CheckBox ChkBCLExists;
        private CheckBox ChkThreadingExists;
        private CheckBox ChkJsonExists;
        private CheckBox ChkEncodingsExists;
        private CheckBox ChkTupleExists;
        private CheckBox ChkAssemblyExists;
        private Label LblHarmonyVersion;
        private CheckBox ChckSystemCodePages;
        private CheckBox ChckMonoCecilInject;
        private CheckBox ChkHelper;
        private CheckBox ChkDotNetZip;
        private CheckBox ChkInject;
    }
}