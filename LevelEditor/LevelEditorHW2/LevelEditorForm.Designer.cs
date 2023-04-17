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
            this.tilePicker_PlayerStart = new System.Windows.Forms.PictureBox();
            this.tilePicker_LevelEnd = new System.Windows.Forms.PictureBox();
            this.tilePicker_Snek = new System.Windows.Forms.PictureBox();
            this.tilePicker_BlankGrey = new System.Windows.Forms.PictureBox();
            this.tilePicker_TestTile = new System.Windows.Forms.PictureBox();
            this.groupBox_CurrentTile = new System.Windows.Forms.GroupBox();
            this.pictureBox_CurrentTile = new System.Windows.Forms.PictureBox();
            this.button_Save = new System.Windows.Forms.Button();
            this.button_Load = new System.Windows.Forms.Button();
            this.groupBox_MapView = new System.Windows.Forms.GroupBox();
            this.ScrollBarX = new System.Windows.Forms.HScrollBar();
            this.ScrollBarY = new System.Windows.Forms.VScrollBar();
            this.groupBox_TileSelector.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_PlayerStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_LevelEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_Snek)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_BlankGrey)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_TestTile)).BeginInit();
            this.groupBox_CurrentTile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_CurrentTile)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox_TileSelector
            // 
            this.groupBox_TileSelector.Controls.Add(this.tilePicker_PlayerStart);
            this.groupBox_TileSelector.Controls.Add(this.tilePicker_LevelEnd);
            this.groupBox_TileSelector.Controls.Add(this.tilePicker_Snek);
            this.groupBox_TileSelector.Controls.Add(this.tilePicker_BlankGrey);
            this.groupBox_TileSelector.Controls.Add(this.tilePicker_TestTile);
            this.groupBox_TileSelector.Location = new System.Drawing.Point(15, 15);
            this.groupBox_TileSelector.Name = "groupBox_TileSelector";
            this.groupBox_TileSelector.Size = new System.Drawing.Size(176, 139);
            this.groupBox_TileSelector.TabIndex = 0;
            this.groupBox_TileSelector.TabStop = false;
            this.groupBox_TileSelector.Text = "Tiles";
            // 
            // tilePicker_PlayerStart
            // 
            this.tilePicker_PlayerStart.BackColor = System.Drawing.SystemColors.Control;
            this.tilePicker_PlayerStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tilePicker_PlayerStart.Image = ((System.Drawing.Image)(resources.GetObject("tilePicker_PlayerStart.Image")));
            this.tilePicker_PlayerStart.Location = new System.Drawing.Point(118, 22);
            this.tilePicker_PlayerStart.Name = "tilePicker_PlayerStart";
            this.tilePicker_PlayerStart.Size = new System.Drawing.Size(50, 50);
            this.tilePicker_PlayerStart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.tilePicker_PlayerStart.TabIndex = 11;
            this.tilePicker_PlayerStart.TabStop = false;
            // 
            // tilePicker_LevelEnd
            // 
            this.tilePicker_LevelEnd.BackColor = System.Drawing.SystemColors.Control;
            this.tilePicker_LevelEnd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tilePicker_LevelEnd.Image = ((System.Drawing.Image)(resources.GetObject("tilePicker_LevelEnd.Image")));
            this.tilePicker_LevelEnd.Location = new System.Drawing.Point(63, 78);
            this.tilePicker_LevelEnd.Name = "tilePicker_LevelEnd";
            this.tilePicker_LevelEnd.Size = new System.Drawing.Size(50, 50);
            this.tilePicker_LevelEnd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.tilePicker_LevelEnd.TabIndex = 10;
            this.tilePicker_LevelEnd.TabStop = false;
            // 
            // tilePicker_Snek
            // 
            this.tilePicker_Snek.BackColor = System.Drawing.Color.Transparent;
            this.tilePicker_Snek.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tilePicker_Snek.Image = ((System.Drawing.Image)(resources.GetObject("tilePicker_Snek.Image")));
            this.tilePicker_Snek.Location = new System.Drawing.Point(6, 78);
            this.tilePicker_Snek.Name = "tilePicker_Snek";
            this.tilePicker_Snek.Size = new System.Drawing.Size(50, 50);
            this.tilePicker_Snek.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.tilePicker_Snek.TabIndex = 9;
            this.tilePicker_Snek.TabStop = false;
            this.tilePicker_Snek.Click += new System.EventHandler(this.colorPicker_Click);
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
            // tilePicker_TestTile
            // 
            this.tilePicker_TestTile.BackColor = System.Drawing.SystemColors.Control;
            this.tilePicker_TestTile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tilePicker_TestTile.Image = ((System.Drawing.Image)(resources.GetObject("tilePicker_TestTile.Image")));
            this.tilePicker_TestTile.Location = new System.Drawing.Point(62, 22);
            this.tilePicker_TestTile.Name = "tilePicker_TestTile";
            this.tilePicker_TestTile.Size = new System.Drawing.Size(50, 50);
            this.tilePicker_TestTile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.tilePicker_TestTile.TabIndex = 7;
            this.tilePicker_TestTile.TabStop = false;
            this.tilePicker_TestTile.Click += new System.EventHandler(this.colorPicker_Click);
            // 
            // groupBox_CurrentTile
            // 
            this.groupBox_CurrentTile.Controls.Add(this.pictureBox_CurrentTile);
            this.groupBox_CurrentTile.Location = new System.Drawing.Point(15, 186);
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
            this.button_Save.Location = new System.Drawing.Point(15, 386);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(176, 64);
            this.button_Save.TabIndex = 2;
            this.button_Save.Text = "Save File";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // button_Load
            // 
            this.button_Load.Location = new System.Drawing.Point(15, 456);
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
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_PlayerStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_LevelEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_Snek)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_BlankGrey)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_TestTile)).EndInit();
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
        private PictureBox tilePicker_TestTile;
        private PictureBox tilePicker_Snek;
        private PictureBox tilePicker_BlankGrey;
        private PictureBox tilePicker_LevelEnd;
        private PictureBox tilePicker_PlayerStart;
    }
}