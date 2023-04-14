namespace LevelEditor
{
    partial class LevelEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LevelEditorForm));
            this.groupBox_TileSelector = new System.Windows.Forms.GroupBox();
            this.tilePicker_snek = new System.Windows.Forms.PictureBox();
            this.tilePicker_BlankGrey = new System.Windows.Forms.PictureBox();
            this.tilePicker_testTile = new System.Windows.Forms.PictureBox();
            this.groupBox_CurrentTile = new System.Windows.Forms.GroupBox();
            this.pictureBox_CurrentTile = new System.Windows.Forms.PictureBox();
            this.button_Save = new System.Windows.Forms.Button();
            this.button_Load = new System.Windows.Forms.Button();
            this.groupBox_MapView = new System.Windows.Forms.GroupBox();
            this.ScrollBarX = new System.Windows.Forms.HScrollBar();
            this.ScrollBarY = new System.Windows.Forms.VScrollBar();
            this.groupBox_TileSelector.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_snek)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_BlankGrey)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_testTile)).BeginInit();
            this.groupBox_CurrentTile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_CurrentTile)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox_TileSelector
            // 
            this.groupBox_TileSelector.Controls.Add(this.tilePicker_snek);
            this.groupBox_TileSelector.Controls.Add(this.tilePicker_BlankGrey);
            this.groupBox_TileSelector.Controls.Add(this.tilePicker_testTile);
            this.groupBox_TileSelector.Location = new System.Drawing.Point(15, 15);
            this.groupBox_TileSelector.Name = "groupBox_TileSelector";
            this.groupBox_TileSelector.Size = new System.Drawing.Size(176, 82);
            this.groupBox_TileSelector.TabIndex = 0;
            this.groupBox_TileSelector.TabStop = false;
            this.groupBox_TileSelector.Text = "Tiles";
            // 
            // tilePicker_snek
            // 
            this.tilePicker_snek.BackColor = System.Drawing.Color.Transparent;
            this.tilePicker_snek.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tilePicker_snek.Image = ((System.Drawing.Image)(resources.GetObject("tilePicker_snek.Image")));
            this.tilePicker_snek.Location = new System.Drawing.Point(118, 22);
            this.tilePicker_snek.Name = "tilePicker_snek";
            this.tilePicker_snek.Size = new System.Drawing.Size(50, 50);
            this.tilePicker_snek.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.tilePicker_snek.TabIndex = 9;
            this.tilePicker_snek.TabStop = false;
            this.tilePicker_snek.Click += new System.EventHandler(this.colorPicker_Click);
            // 
            // tilePicker_BlankGrey
            // 
            this.tilePicker_BlankGrey.BackColor = System.Drawing.Color.Gray;
            this.tilePicker_BlankGrey.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tilePicker_BlankGrey.Location = new System.Drawing.Point(6, 22);
            this.tilePicker_BlankGrey.Name = "tilePicker_BlankGrey";
            this.tilePicker_BlankGrey.Size = new System.Drawing.Size(50, 50);
            this.tilePicker_BlankGrey.TabIndex = 8;
            this.tilePicker_BlankGrey.TabStop = false;
            this.tilePicker_BlankGrey.Click += new System.EventHandler(this.colorPicker_Click);
            // 
            // tilePicker_testTile
            // 
            this.tilePicker_testTile.BackColor = System.Drawing.SystemColors.Control;
            this.tilePicker_testTile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tilePicker_testTile.Image = ((System.Drawing.Image)(resources.GetObject("tilePicker_testTile.Image")));
            this.tilePicker_testTile.Location = new System.Drawing.Point(62, 22);
            this.tilePicker_testTile.Name = "tilePicker_testTile";
            this.tilePicker_testTile.Size = new System.Drawing.Size(50, 50);
            this.tilePicker_testTile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.tilePicker_testTile.TabIndex = 7;
            this.tilePicker_testTile.TabStop = false;
            this.tilePicker_testTile.Click += new System.EventHandler(this.colorPicker_Click);
            // 
            // groupBox_CurrentTile
            // 
            this.groupBox_CurrentTile.Controls.Add(this.pictureBox_CurrentTile);
            this.groupBox_CurrentTile.Location = new System.Drawing.Point(15, 103);
            this.groupBox_CurrentTile.Name = "groupBox_CurrentTile";
            this.groupBox_CurrentTile.Size = new System.Drawing.Size(176, 194);
            this.groupBox_CurrentTile.TabIndex = 1;
            this.groupBox_CurrentTile.TabStop = false;
            this.groupBox_CurrentTile.Text = "Current Tile";
            // 
            // pictureBox_CurrentTile
            // 
            this.pictureBox_CurrentTile.BackColor = System.Drawing.Color.Gray;
            this.pictureBox_CurrentTile.Location = new System.Drawing.Point(6, 20);
            this.pictureBox_CurrentTile.Name = "pictureBox_CurrentTile";
            this.pictureBox_CurrentTile.Size = new System.Drawing.Size(164, 164);
            this.pictureBox_CurrentTile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_CurrentTile.TabIndex = 0;
            this.pictureBox_CurrentTile.TabStop = false;
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(15, 303);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(176, 64);
            this.button_Save.TabIndex = 2;
            this.button_Save.Text = "Save File";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // button_Load
            // 
            this.button_Load.Location = new System.Drawing.Point(15, 373);
            this.button_Load.Name = "button_Load";
            this.button_Load.Size = new System.Drawing.Size(176, 64);
            this.button_Load.TabIndex = 3;
            this.button_Load.Text = "Load File";
            this.button_Load.UseVisualStyleBackColor = true;
            this.button_Load.Click += new System.EventHandler(this.button_Load_Click);
            // 
            // groupBox_MapView
            // 
            this.groupBox_MapView.Location = new System.Drawing.Point(197, 15);
            this.groupBox_MapView.Name = "groupBox_MapView";
            this.groupBox_MapView.Size = new System.Drawing.Size(1277, 896);
            this.groupBox_MapView.TabIndex = 4;
            this.groupBox_MapView.TabStop = false;
            this.groupBox_MapView.Text = "Map";
            // 
            // ScrollBarX
            // 
            this.ScrollBarX.Location = new System.Drawing.Point(197, 914);
            this.ScrollBarX.Name = "ScrollBarX";
            this.ScrollBarX.Size = new System.Drawing.Size(1277, 17);
            this.ScrollBarX.TabIndex = 5;
            this.ScrollBarX.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ScrollBar_Scroll);
            // 
            // ScrollBarY
            // 
            this.ScrollBarY.Location = new System.Drawing.Point(1492, 15);
            this.ScrollBarY.Name = "ScrollBarY";
            this.ScrollBarY.Size = new System.Drawing.Size(17, 896);
            this.ScrollBarY.TabIndex = 0;
            this.ScrollBarY.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ScrollBar_Scroll);
            // 
            // LevelEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1518, 943);
            this.Controls.Add(this.ScrollBarY);
            this.Controls.Add(this.ScrollBarX);
            this.Controls.Add(this.groupBox_MapView);
            this.Controls.Add(this.button_Load);
            this.Controls.Add(this.button_Save);
            this.Controls.Add(this.groupBox_CurrentTile);
            this.Controls.Add(this.groupBox_TileSelector);
            this.MaximizeBox = false;
            this.Name = "LevelEditorForm";
            this.Text = "Level Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LevelEditorForm_FormClosing);
            this.groupBox_TileSelector.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_snek)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_BlankGrey)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_testTile)).EndInit();
            this.groupBox_CurrentTile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_CurrentTile)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox_TileSelector;
        private GroupBox groupBox_CurrentTile;
        private PictureBox pictureBox_CurrentTile;
        private Button button_Save;
        private Button button_Load;
        private GroupBox groupBox_MapView;
        private HScrollBar ScrollBarX;
        private VScrollBar ScrollBarY;
        private PictureBox tilePicker_testTile;
        private PictureBox tilePicker_snek;
        private PictureBox tilePicker_BlankGrey;
    }
}