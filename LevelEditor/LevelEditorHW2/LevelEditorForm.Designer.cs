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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button_RedTile = new System.Windows.Forms.Button();
            this.button_DarkGreenTile = new System.Windows.Forms.Button();
            this.button_GreenTile = new System.Windows.Forms.Button();
            this.button_GrayTile = new System.Windows.Forms.Button();
            this.button_YellowTile = new System.Windows.Forms.Button();
            this.button_BlueTile = new System.Windows.Forms.Button();
            this.groupBox_CurrentTile = new System.Windows.Forms.GroupBox();
            this.pictureBox_CurrentTile = new System.Windows.Forms.PictureBox();
            this.button_Save = new System.Windows.Forms.Button();
            this.button_Load = new System.Windows.Forms.Button();
            this.groupBox_MapView = new System.Windows.Forms.GroupBox();
            this.ScrollBarX = new System.Windows.Forms.HScrollBar();
            this.ScrollBarY = new System.Windows.Forms.VScrollBar();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.groupBox_TileSelector.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox_CurrentTile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_CurrentTile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox_TileSelector
            // 
            this.groupBox_TileSelector.Controls.Add(this.pictureBox3);
            this.groupBox_TileSelector.Controls.Add(this.pictureBox2);
            this.groupBox_TileSelector.Controls.Add(this.pictureBox1);
            this.groupBox_TileSelector.Controls.Add(this.button_RedTile);
            this.groupBox_TileSelector.Controls.Add(this.button_DarkGreenTile);
            this.groupBox_TileSelector.Controls.Add(this.button_GreenTile);
            this.groupBox_TileSelector.Controls.Add(this.button_GrayTile);
            this.groupBox_TileSelector.Controls.Add(this.button_YellowTile);
            this.groupBox_TileSelector.Controls.Add(this.button_BlueTile);
            this.groupBox_TileSelector.Location = new System.Drawing.Point(15, 15);
            this.groupBox_TileSelector.Name = "groupBox_TileSelector";
            this.groupBox_TileSelector.Size = new System.Drawing.Size(158, 160);
            this.groupBox_TileSelector.TabIndex = 0;
            this.groupBox_TileSelector.TabStop = false;
            this.groupBox_TileSelector.Text = "Tiles";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(100, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // button_RedTile
            // 
            this.button_RedTile.BackColor = System.Drawing.Color.IndianRed;
            this.button_RedTile.Location = new System.Drawing.Point(53, 115);
            this.button_RedTile.Name = "button_RedTile";
            this.button_RedTile.Size = new System.Drawing.Size(41, 41);
            this.button_RedTile.TabIndex = 6;
            this.button_RedTile.UseVisualStyleBackColor = false;
            this.button_RedTile.Click += new System.EventHandler(this.colorPicker_Click);
            // 
            // button_DarkGreenTile
            // 
            this.button_DarkGreenTile.BackColor = System.Drawing.Color.DarkOliveGreen;
            this.button_DarkGreenTile.Location = new System.Drawing.Point(53, 22);
            this.button_DarkGreenTile.Name = "button_DarkGreenTile";
            this.button_DarkGreenTile.Size = new System.Drawing.Size(41, 41);
            this.button_DarkGreenTile.TabIndex = 5;
            this.button_DarkGreenTile.UseVisualStyleBackColor = false;
            this.button_DarkGreenTile.Click += new System.EventHandler(this.colorPicker_Click);
            // 
            // button_GreenTile
            // 
            this.button_GreenTile.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.button_GreenTile.Location = new System.Drawing.Point(6, 22);
            this.button_GreenTile.Name = "button_GreenTile";
            this.button_GreenTile.Size = new System.Drawing.Size(41, 41);
            this.button_GreenTile.TabIndex = 1;
            this.button_GreenTile.UseVisualStyleBackColor = false;
            this.button_GreenTile.Click += new System.EventHandler(this.colorPicker_Click);
            // 
            // button_GrayTile
            // 
            this.button_GrayTile.BackColor = System.Drawing.Color.LightSlateGray;
            this.button_GrayTile.Location = new System.Drawing.Point(6, 116);
            this.button_GrayTile.Name = "button_GrayTile";
            this.button_GrayTile.Size = new System.Drawing.Size(41, 41);
            this.button_GrayTile.TabIndex = 2;
            this.button_GrayTile.UseVisualStyleBackColor = false;
            this.button_GrayTile.Click += new System.EventHandler(this.colorPicker_Click);
            // 
            // button_YellowTile
            // 
            this.button_YellowTile.BackColor = System.Drawing.Color.Khaki;
            this.button_YellowTile.Location = new System.Drawing.Point(6, 69);
            this.button_YellowTile.Name = "button_YellowTile";
            this.button_YellowTile.Size = new System.Drawing.Size(41, 41);
            this.button_YellowTile.TabIndex = 3;
            this.button_YellowTile.UseVisualStyleBackColor = false;
            this.button_YellowTile.Click += new System.EventHandler(this.colorPicker_Click);
            // 
            // button_BlueTile
            // 
            this.button_BlueTile.BackColor = System.Drawing.Color.CadetBlue;
            this.button_BlueTile.Location = new System.Drawing.Point(53, 69);
            this.button_BlueTile.Name = "button_BlueTile";
            this.button_BlueTile.Size = new System.Drawing.Size(41, 41);
            this.button_BlueTile.TabIndex = 4;
            this.button_BlueTile.UseVisualStyleBackColor = false;
            this.button_BlueTile.Click += new System.EventHandler(this.colorPicker_Click);
            // 
            // groupBox_CurrentTile
            // 
            this.groupBox_CurrentTile.Controls.Add(this.pictureBox_CurrentTile);
            this.groupBox_CurrentTile.Location = new System.Drawing.Point(15, 200);
            this.groupBox_CurrentTile.Name = "groupBox_CurrentTile";
            this.groupBox_CurrentTile.Size = new System.Drawing.Size(158, 185);
            this.groupBox_CurrentTile.TabIndex = 1;
            this.groupBox_CurrentTile.TabStop = false;
            this.groupBox_CurrentTile.Text = "Current Tile";
            // 
            // pictureBox_CurrentTile
            // 
            this.pictureBox_CurrentTile.BackColor = System.Drawing.Color.CadetBlue;
            this.pictureBox_CurrentTile.Location = new System.Drawing.Point(6, 20);
            this.pictureBox_CurrentTile.Name = "pictureBox_CurrentTile";
            this.pictureBox_CurrentTile.Size = new System.Drawing.Size(146, 146);
            this.pictureBox_CurrentTile.TabIndex = 0;
            this.pictureBox_CurrentTile.TabStop = false;
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(39, 417);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(70, 70);
            this.button_Save.TabIndex = 2;
            this.button_Save.Text = "Save File";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // button_Load
            // 
            this.button_Load.Location = new System.Drawing.Point(30, 509);
            this.button_Load.Name = "button_Load";
            this.button_Load.Size = new System.Drawing.Size(70, 70);
            this.button_Load.TabIndex = 3;
            this.button_Load.Text = "Load File";
            this.button_Load.UseVisualStyleBackColor = true;
            this.button_Load.Click += new System.EventHandler(this.button_Load_Click);
            // 
            // groupBox_MapView
            // 
            this.groupBox_MapView.Location = new System.Drawing.Point(179, 15);
            this.groupBox_MapView.Name = "groupBox_MapView";
            this.groupBox_MapView.Size = new System.Drawing.Size(1295, 896);
            this.groupBox_MapView.TabIndex = 4;
            this.groupBox_MapView.TabStop = false;
            this.groupBox_MapView.Text = "Map";
            // 
            // ScrollBarX
            // 
            this.ScrollBarX.Location = new System.Drawing.Point(130, 914);
            this.ScrollBarX.Name = "ScrollBarX";
            this.ScrollBarX.Size = new System.Drawing.Size(1344, 17);
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
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Gray;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(44, 22);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(50, 50);
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(100, 87);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(50, 50);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 9;
            this.pictureBox3.TabStop = false;
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox_CurrentTile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_CurrentTile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox_TileSelector;
        private Button button_RedTile;
        private Button button_DarkGreenTile;
        private Button button_GreenTile;
        private Button button_GrayTile;
        private Button button_YellowTile;
        private Button button_BlueTile;
        private GroupBox groupBox_CurrentTile;
        private PictureBox pictureBox_CurrentTile;
        private Button button_Save;
        private Button button_Load;
        private GroupBox groupBox_MapView;
        private HScrollBar ScrollBarX;
        private VScrollBar ScrollBarY;
        private PictureBox pictureBox1;
        private PictureBox pictureBox3;
        private PictureBox pictureBox2;
    }
}