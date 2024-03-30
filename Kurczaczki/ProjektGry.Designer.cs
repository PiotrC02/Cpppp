using System;

namespace Kurczaczki
{
    partial class ProjektGry
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjektGry));
            this.lblScore = new System.Windows.Forms.Label();
            this.pociskiTimer = new System.Windows.Forms.Timer(this.components);
            this.kurczakTimer = new System.Windows.Forms.Timer(this.components);
            this.jajkoTimer = new System.Windows.Forms.Timer(this.components);
            this.bossLivesLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.Location = new System.Drawing.Point(12, 9);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(138, 39);
            this.lblScore.TabIndex = 0;
            this.lblScore.Text = "Punkty: 0";
            // 
            // pociskiTimer
            // 
            this.pociskiTimer.Enabled = true;
            this.pociskiTimer.Interval = 20;
            this.pociskiTimer.Tick += new System.EventHandler(this.pociskiTimer_Tick);
            // 
            // kurczakTimer
            // 
            this.kurczakTimer.Enabled = true;
            this.kurczakTimer.Interval = 20;
            this.kurczakTimer.Tick += new System.EventHandler(this.kurczakTimer_Tick);
            // 
            // jajkoTimer
            // 
            this.jajkoTimer.Enabled = true;
            this.jajkoTimer.Interval = 20;
            this.jajkoTimer.Tick += new System.EventHandler(this.jajkoTimer_Tick);
            // 
            // bossLivesLabel
            // 
            this.bossLivesLabel.AutoSize = true;
            this.bossLivesLabel.BackColor = System.Drawing.Color.Red;
            this.bossLivesLabel.Font = new System.Drawing.Font("Comic Sans MS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.bossLivesLabel.Location = new System.Drawing.Point(498, 9);
            this.bossLivesLabel.Name = "bossLivesLabel";
            this.bossLivesLabel.Size = new System.Drawing.Size(132, 56);
            this.bossLivesLabel.TabIndex = 1;
            this.bossLivesLabel.Text = "Boss: ";
            // 
            // ProjektGry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 38F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1182, 653);
            this.Controls.Add(this.bossLivesLabel);
            this.Controls.Add(this.lblScore);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Comic Sans MS", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.MaximumSize = new System.Drawing.Size(1200, 700);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "ProjektGry";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kosmiczna Flota: Kontratak";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProjektGry_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ProjektGry_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Przerwa_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Timer pociskiTimer;
        private System.Windows.Forms.Timer kurczakTimer;
        private System.Windows.Forms.Timer jajkoTimer;
        private System.Windows.Forms.Label bossLivesLabel;
    }
}

