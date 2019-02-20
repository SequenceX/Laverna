namespace Laverna
{
    partial class LavernaMainFrame
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.versionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.validationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.functionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sequenceGrabberToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sESequenceModifierToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.strandPrimerDesignerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sequenceGrabberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sESequenceModifierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.strandPrimerDesignerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.xxAbgeneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xxStdLongPrimerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xxGGCDesignerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xxLibsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xxVariantsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xxSeqprimersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.aboutToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(640, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.xxAbgeneToolStripMenuItem,
            this.xxStdLongPrimerToolStripMenuItem,
            this.xxGGCDesignerToolStripMenuItem,
            this.xxLibsToolStripMenuItem,
            this.xxVariantsToolStripMenuItem,
            this.xxSeqprimersToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(71, 20);
            this.toolStripMenuItem1.Text = "Functions";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem3.Text = "xxSubcloning";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem4.Text = "xxHIFI";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem5.Text = "xxAnnealing";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.versionsToolStripMenuItem,
            this.validationsToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // versionsToolStripMenuItem
            // 
            this.versionsToolStripMenuItem.Name = "versionsToolStripMenuItem";
            this.versionsToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.versionsToolStripMenuItem.Text = "Versions";
            this.versionsToolStripMenuItem.Click += new System.EventHandler(this.versionsToolStripMenuItem_Click);
            // 
            // validationsToolStripMenuItem
            // 
            this.validationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.functionsToolStripMenuItem,
            this.toolsToolStripMenuItem1});
            this.validationsToolStripMenuItem.Name = "validationsToolStripMenuItem";
            this.validationsToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.validationsToolStripMenuItem.Text = "Validations";
            // 
            // functionsToolStripMenuItem
            // 
            this.functionsToolStripMenuItem.Name = "functionsToolStripMenuItem";
            this.functionsToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.functionsToolStripMenuItem.Text = "Functions";
            // 
            // toolsToolStripMenuItem1
            // 
            this.toolsToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sequenceGrabberToolStripMenuItem1,
            this.sESequenceModifierToolStripMenuItem1,
            this.strandPrimerDesignerToolStripMenuItem1});
            this.toolsToolStripMenuItem1.Name = "toolsToolStripMenuItem1";
            this.toolsToolStripMenuItem1.Size = new System.Drawing.Size(126, 22);
            this.toolsToolStripMenuItem1.Text = "Tools";
            // 
            // sequenceGrabberToolStripMenuItem1
            // 
            this.sequenceGrabberToolStripMenuItem1.Name = "sequenceGrabberToolStripMenuItem1";
            this.sequenceGrabberToolStripMenuItem1.Size = new System.Drawing.Size(195, 22);
            this.sequenceGrabberToolStripMenuItem1.Text = "Sequence Grabber";
            // 
            // sESequenceModifierToolStripMenuItem1
            // 
            this.sESequenceModifierToolStripMenuItem1.Name = "sESequenceModifierToolStripMenuItem1";
            this.sESequenceModifierToolStripMenuItem1.Size = new System.Drawing.Size(195, 22);
            this.sESequenceModifierToolStripMenuItem1.Text = "SE Sequence Modifier";
            // 
            // strandPrimerDesignerToolStripMenuItem1
            // 
            this.strandPrimerDesignerToolStripMenuItem1.Name = "strandPrimerDesignerToolStripMenuItem1";
            this.strandPrimerDesignerToolStripMenuItem1.Size = new System.Drawing.Size(195, 22);
            this.strandPrimerDesignerToolStripMenuItem1.Text = "Strand Primer Designer";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sequenceGrabberToolStripMenuItem,
            this.sESequenceModifierToolStripMenuItem,
            this.strandPrimerDesignerToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // sequenceGrabberToolStripMenuItem
            // 
            this.sequenceGrabberToolStripMenuItem.Name = "sequenceGrabberToolStripMenuItem";
            this.sequenceGrabberToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.sequenceGrabberToolStripMenuItem.Text = "Sequence Grabber";
            this.sequenceGrabberToolStripMenuItem.Click += new System.EventHandler(this.sequenceGrabberToolStripMenuItem_Click);
            // 
            // sESequenceModifierToolStripMenuItem
            // 
            this.sESequenceModifierToolStripMenuItem.Name = "sESequenceModifierToolStripMenuItem";
            this.sESequenceModifierToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.sESequenceModifierToolStripMenuItem.Text = "SE Sequence Modifier";
            this.sESequenceModifierToolStripMenuItem.Click += new System.EventHandler(this.sESequenceModifierToolStripMenuItem_Click);
            // 
            // strandPrimerDesignerToolStripMenuItem
            // 
            this.strandPrimerDesignerToolStripMenuItem.Name = "strandPrimerDesignerToolStripMenuItem";
            this.strandPrimerDesignerToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.strandPrimerDesignerToolStripMenuItem.Text = "Strand Primer Designer";
            this.strandPrimerDesignerToolStripMenuItem.Click += new System.EventHandler(this.strandPrimerDesignerToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetSettingsToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // resetSettingsToolStripMenuItem
            // 
            this.resetSettingsToolStripMenuItem.Name = "resetSettingsToolStripMenuItem";
            this.resetSettingsToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.resetSettingsToolStripMenuItem.Text = "Reset Settings";
            this.resetSettingsToolStripMenuItem.Click += new System.EventHandler(this.resetSettingsToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Location = new System.Drawing.Point(0, 94);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(121, 148);
            this.panel1.TabIndex = 2;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(3, 85);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(116, 35);
            this.button3.TabIndex = 5;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(3, 44);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(116, 35);
            this.button2.TabIndex = 4;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(5, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 35);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // xxAbgeneToolStripMenuItem
            // 
            this.xxAbgeneToolStripMenuItem.Name = "xxAbgeneToolStripMenuItem";
            this.xxAbgeneToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.xxAbgeneToolStripMenuItem.Text = "xxAbgene";
            // 
            // xxStdLongPrimerToolStripMenuItem
            // 
            this.xxStdLongPrimerToolStripMenuItem.Name = "xxStdLongPrimerToolStripMenuItem";
            this.xxStdLongPrimerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.xxStdLongPrimerToolStripMenuItem.Text = "xxStd Long Primer";
            // 
            // xxGGCDesignerToolStripMenuItem
            // 
            this.xxGGCDesignerToolStripMenuItem.Name = "xxGGCDesignerToolStripMenuItem";
            this.xxGGCDesignerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.xxGGCDesignerToolStripMenuItem.Text = "xx GGC Designer";
            // 
            // xxLibsToolStripMenuItem
            // 
            this.xxLibsToolStripMenuItem.Name = "xxLibsToolStripMenuItem";
            this.xxLibsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.xxLibsToolStripMenuItem.Text = "xxLibs?";
            // 
            // xxVariantsToolStripMenuItem
            // 
            this.xxVariantsToolStripMenuItem.Name = "xxVariantsToolStripMenuItem";
            this.xxVariantsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.xxVariantsToolStripMenuItem.Text = "xxVariants?";
            // 
            // xxSeqprimersToolStripMenuItem
            // 
            this.xxSeqprimersToolStripMenuItem.Name = "xxSeqprimersToolStripMenuItem";
            this.xxSeqprimersToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.xxSeqprimersToolStripMenuItem.Text = "xxSeqprimers";
            // 
            // LavernaMainFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 486);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "LavernaMainFrame";
            this.Text = "ProGSY";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripMenuItem sequenceGrabberToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem versionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem validationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem functionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sESequenceModifierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem strandPrimerDesignerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sequenceGrabberToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sESequenceModifierToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem strandPrimerDesignerToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem resetSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xxAbgeneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xxStdLongPrimerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xxGGCDesignerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xxLibsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xxVariantsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xxSeqprimersToolStripMenuItem;
    }
}

