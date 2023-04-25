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
            this.tilePicker_PlanksRight = new System.Windows.Forms.PictureBox();
            this.tilePicker_PlanksCenter = new System.Windows.Forms.PictureBox();
            this.tilePicker_PlanksLeft = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tilePicker_BlankGrey = new System.Windows.Forms.PictureBox();
            this.comboBox_TilePickerCatagories = new System.Windows.Forms.ComboBox();
            this.tilePicker_PlayerStart = new System.Windows.Forms.PictureBox();
            this.tilePicker_LevelEnd = new System.Windows.Forms.PictureBox();
            this.tilePicker_Snek = new System.Windows.Forms.PictureBox();
            this.tilePicker_TestTile = new System.Windows.Forms.PictureBox();
            this.groupBox_CurrentTile = new System.Windows.Forms.GroupBox();
            this.pictureBox_CurrentTile = new System.Windows.Forms.PictureBox();
            this.button_Save = new System.Windows.Forms.Button();
            this.button_Load = new System.Windows.Forms.Button();
            this.groupBox_MapView = new System.Windows.Forms.GroupBox();
            this.ScrollBarX = new System.Windows.Forms.HScrollBar();
            this.ScrollBarY = new System.Windows.Forms.VScrollBar();
            this.tilePicker_Bricks = new System.Windows.Forms.PictureBox();
            this.groupBox_TileSelector.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_PlanksRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_PlanksCenter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_PlanksLeft)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_BlankGrey)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_PlayerStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_LevelEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_Snek)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_TestTile)).BeginInit();
            this.groupBox_CurrentTile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_CurrentTile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_Bricks)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox_TileSelector
            // 
            this.groupBox_TileSelector.Controls.Add(this.tilePicker_Bricks);
            this.groupBox_TileSelector.Controls.Add(this.tilePicker_PlanksRight);
            this.groupBox_TileSelector.Controls.Add(this.tilePicker_PlanksCenter);
            this.groupBox_TileSelector.Controls.Add(this.tilePicker_PlanksLeft);
            this.groupBox_TileSelector.Controls.Add(this.groupBox1);
            this.groupBox_TileSelector.Controls.Add(this.comboBox_TilePickerCatagories);
            this.groupBox_TileSelector.Controls.Add(this.tilePicker_PlayerStart);
            this.groupBox_TileSelector.Controls.Add(this.tilePicker_LevelEnd);
            this.groupBox_TileSelector.Controls.Add(this.tilePicker_Snek);
            this.groupBox_TileSelector.Controls.Add(this.tilePicker_TestTile);
            this.groupBox_TileSelector.Location = new System.Drawing.Point(15, 15);
            this.groupBox_TileSelector.Name = "groupBox_TileSelector";
            this.groupBox_TileSelector.Size = new System.Drawing.Size(176, 309);
            this.groupBox_TileSelector.TabIndex = 0;
            this.groupBox_TileSelector.TabStop = false;
            this.groupBox_TileSelector.Text = "Tiles";
            // 
            // tilePicker_PlanksRight
            // 
            this.tilePicker_PlanksRight.BackColor = System.Drawing.Color.Transparent;
            this.tilePicker_PlanksRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tilePicker_PlanksRight.Image = ((System.Drawing.Image)(resources.GetObject("tilePicker_PlanksRight.Image")));
            this.tilePicker_PlanksRight.Location = new System.Drawing.Point(118, 109);
            this.tilePicker_PlanksRight.Name = "tilePicker_PlanksRight";
            this.tilePicker_PlanksRight.Size = new System.Drawing.Size(50, 50);
            this.tilePicker_PlanksRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.tilePicker_PlanksRight.TabIndex = 16;
            this.tilePicker_PlanksRight.TabStop = false;
            this.tilePicker_PlanksRight.Click += new System.EventHandler(this.tilePicker_Click);
            // 
            // tilePicker_PlanksCenter
            // 
            this.tilePicker_PlanksCenter.BackColor = System.Drawing.Color.Transparent;
            this.tilePicker_PlanksCenter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tilePicker_PlanksCenter.Image = ((System.Drawing.Image)(resources.GetObject("tilePicker_PlanksCenter.Image")));
            this.tilePicker_PlanksCenter.Location = new System.Drawing.Point(62, 109);
            this.tilePicker_PlanksCenter.Name = "tilePicker_PlanksCenter";
            this.tilePicker_PlanksCenter.Size = new System.Drawing.Size(50, 50);
            this.tilePicker_PlanksCenter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.tilePicker_PlanksCenter.TabIndex = 15;
            this.tilePicker_PlanksCenter.TabStop = false;
            this.tilePicker_PlanksCenter.Click += new System.EventHandler(this.tilePicker_Click);
            // 
            // tilePicker_PlanksLeft
            // 
            this.tilePicker_PlanksLeft.BackColor = System.Drawing.Color.Transparent;
            this.tilePicker_PlanksLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tilePicker_PlanksLeft.Image = ((System.Drawing.Image)(resources.GetObject("tilePicker_PlanksLeft.Image")));
            this.tilePicker_PlanksLeft.Location = new System.Drawing.Point(6, 109);
            this.tilePicker_PlanksLeft.Name = "tilePicker_PlanksLeft";
            this.tilePicker_PlanksLeft.Size = new System.Drawing.Size(50, 50);
            this.tilePicker_PlanksLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.tilePicker_PlanksLeft.TabIndex = 14;
            this.tilePicker_PlanksLeft.TabStop = false;
            this.tilePicker_PlanksLeft.Click += new System.EventHandler(this.tilePicker_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tilePicker_BlankGrey);
            this.groupBox1.Location = new System.Drawing.Point(62, 224);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(62, 79);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Eraser";
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
            this.tilePicker_BlankGrey.Click += new System.EventHandler(this.tilePicker_Click);
            // 
            // comboBox_TilePickerCatagories
            // 
            this.comboBox_TilePickerCatagories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_TilePickerCatagories.FormattingEnabled = true;
            this.comboBox_TilePickerCatagories.Items.AddRange(new object[] {
            "Misc",
            "Entities"});
            this.comboBox_TilePickerCatagories.Location = new System.Drawing.Point(6, 22);
            this.comboBox_TilePickerCatagories.Name = "comboBox_TilePickerCatagories";
            this.comboBox_TilePickerCatagories.Size = new System.Drawing.Size(121, 23);
            this.comboBox_TilePickerCatagories.TabIndex = 12;
            this.comboBox_TilePickerCatagories.SelectedIndexChanged += new System.EventHandler(this.comboBox_TilePickerCatagories_SelectedIndexChanged);
            // 
            // tilePicker_PlayerStart
            // 
            this.tilePicker_PlayerStart.BackColor = System.Drawing.SystemColors.Control;
            this.tilePicker_PlayerStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tilePicker_PlayerStart.Image = ((System.Drawing.Image)(resources.GetObject("tilePicker_PlayerStart.Image")));
            this.tilePicker_PlayerStart.Location = new System.Drawing.Point(118, 53);
            this.tilePicker_PlayerStart.Name = "tilePicker_PlayerStart";
            this.tilePicker_PlayerStart.Size = new System.Drawing.Size(50, 50);
            this.tilePicker_PlayerStart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.tilePicker_PlayerStart.TabIndex = 11;
            this.tilePicker_PlayerStart.TabStop = false;
            this.tilePicker_PlayerStart.Click += new System.EventHandler(this.tilePicker_Click);
            // 
            // tilePicker_LevelEnd
            // 
            this.tilePicker_LevelEnd.BackColor = System.Drawing.SystemColors.Control;
            this.tilePicker_LevelEnd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tilePicker_LevelEnd.Image = ((System.Drawing.Image)(resources.GetObject("tilePicker_LevelEnd.Image")));
            this.tilePicker_LevelEnd.Location = new System.Drawing.Point(62, 53);
            this.tilePicker_LevelEnd.Name = "tilePicker_LevelEnd";
            this.tilePicker_LevelEnd.Size = new System.Drawing.Size(50, 50);
            this.tilePicker_LevelEnd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.tilePicker_LevelEnd.TabIndex = 10;
            this.tilePicker_LevelEnd.TabStop = false;
            this.tilePicker_LevelEnd.Click += new System.EventHandler(this.tilePicker_Click);
            // 
            // tilePicker_Snek
            // 
            this.tilePicker_Snek.BackColor = System.Drawing.Color.Transparent;
            this.tilePicker_Snek.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tilePicker_Snek.Image = ((System.Drawing.Image)(resources.GetObject("tilePicker_Snek.Image")));
            this.tilePicker_Snek.Location = new System.Drawing.Point(6, 53);
            this.tilePicker_Snek.Name = "tilePicker_Snek";
            this.tilePicker_Snek.Size = new System.Drawing.Size(50, 50);
            this.tilePicker_Snek.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.tilePicker_Snek.TabIndex = 9;
            this.tilePicker_Snek.TabStop = false;
            this.tilePicker_Snek.Click += new System.EventHandler(this.tilePicker_Click);
            // 
            // tilePicker_TestTile
            // 
            this.tilePicker_TestTile.BackColor = System.Drawing.SystemColors.Control;
            this.tilePicker_TestTile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tilePicker_TestTile.Image = ((System.Drawing.Image)(resources.GetObject("tilePicker_TestTile.Image")));
            this.tilePicker_TestTile.Location = new System.Drawing.Point(6, 53);
            this.tilePicker_TestTile.Name = "tilePicker_TestTile";
            this.tilePicker_TestTile.Size = new System.Drawing.Size(50, 50);
            this.tilePicker_TestTile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.tilePicker_TestTile.TabIndex = 7;
            this.tilePicker_TestTile.TabStop = false;
            this.tilePicker_TestTile.Click += new System.EventHandler(this.tilePicker_Click);
            // 
            // groupBox_CurrentTile
            // 
            this.groupBox_CurrentTile.Controls.Add(this.pictureBox_CurrentTile);
            this.groupBox_CurrentTile.Location = new System.Drawing.Point(15, 330);
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
            this.button_Save.Location = new System.Drawing.Point(15, 530);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(176, 64);
            this.button_Save.TabIndex = 2;
            this.button_Save.Text = "Save File";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // button_Load
            // 
            this.button_Load.Location = new System.Drawing.Point(15, 600);
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
            this.groupBox_MapView.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox_MapView.Size = new System.Drawing.Size(1277, 908);
            this.groupBox_MapView.TabIndex = 4;
            this.groupBox_MapView.TabStop = false;
            this.groupBox_MapView.Text = "Map";
            // 
            // ScrollBarX
            // 
            this.ScrollBarX.Location = new System.Drawing.Point(197, 926);
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
            // tilePicker_Bricks
            // 
            this.tilePicker_Bricks.BackColor = System.Drawing.SystemColors.Control;
            this.tilePicker_Bricks.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tilePicker_Bricks.Image = ((System.Drawing.Image)(resources.GetObject("tilePicker_Bricks.Image")));
            this.tilePicker_Bricks.Location = new System.Drawing.Point(62, 53);
            this.tilePicker_Bricks.Name = "tilePicker_Bricks";
            this.tilePicker_Bricks.Size = new System.Drawing.Size(50, 50);
            this.tilePicker_Bricks.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.tilePicker_Bricks.TabIndex = 17;
            this.tilePicker_Bricks.TabStop = false;
            this.tilePicker_Bricks.Click += new System.EventHandler(this.tilePicker_Click);
            // 
            // LevelEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1518, 949);
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
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_PlanksRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_PlanksCenter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_PlanksLeft)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_BlankGrey)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_PlayerStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_LevelEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_Snek)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_TestTile)).EndInit();
            this.groupBox_CurrentTile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_CurrentTile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tilePicker_Bricks)).EndInit();
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
        private ComboBox comboBox_TilePickerCatagories;
        private GroupBox groupBox1;
        private PictureBox tilePicker_PlanksRight;
        private PictureBox tilePicker_PlanksCenter;
        private PictureBox tilePicker_PlanksLeft;
        private PictureBox tilePicker_Bricks;
    }
}