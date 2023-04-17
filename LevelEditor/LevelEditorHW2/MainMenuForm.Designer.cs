namespace LevelEditor
{
    partial class MainMenuForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_LoadMap = new System.Windows.Forms.Button();
            this.groupBox_CreateNewMapMenu = new System.Windows.Forms.GroupBox();
            this.button_CreateMap = new System.Windows.Forms.Button();
            this.label_HeightInput = new System.Windows.Forms.Label();
            this.textBox_HeightInput = new System.Windows.Forms.TextBox();
            this.textBox_WidthInput = new System.Windows.Forms.TextBox();
            this.label_WidthInput = new System.Windows.Forms.Label();
            this.groupBox_CreateNewMapMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_LoadMap
            // 
            this.button_LoadMap.Location = new System.Drawing.Point(15, 15);
            this.button_LoadMap.Name = "button_LoadMap";
            this.button_LoadMap.Size = new System.Drawing.Size(235, 70);
            this.button_LoadMap.TabIndex = 0;
            this.button_LoadMap.Text = "Load Map";
            this.button_LoadMap.UseVisualStyleBackColor = true;
            this.button_LoadMap.Click += new System.EventHandler(this.button_LoadMap_Click);
            // 
            // groupBox_CreateNewMapMenu
            // 
            this.groupBox_CreateNewMapMenu.Controls.Add(this.button_CreateMap);
            this.groupBox_CreateNewMapMenu.Controls.Add(this.label_HeightInput);
            this.groupBox_CreateNewMapMenu.Controls.Add(this.textBox_HeightInput);
            this.groupBox_CreateNewMapMenu.Controls.Add(this.textBox_WidthInput);
            this.groupBox_CreateNewMapMenu.Controls.Add(this.label_WidthInput);
            this.groupBox_CreateNewMapMenu.Location = new System.Drawing.Point(15, 100);
            this.groupBox_CreateNewMapMenu.Name = "groupBox_CreateNewMapMenu";
            this.groupBox_CreateNewMapMenu.Size = new System.Drawing.Size(235, 190);
            this.groupBox_CreateNewMapMenu.TabIndex = 1;
            this.groupBox_CreateNewMapMenu.TabStop = false;
            this.groupBox_CreateNewMapMenu.Text = "Create New Map";
            // 
            // button_CreateMap
            // 
            this.button_CreateMap.Location = new System.Drawing.Point(15, 105);
            this.button_CreateMap.Name = "button_CreateMap";
            this.button_CreateMap.Size = new System.Drawing.Size(205, 65);
            this.button_CreateMap.TabIndex = 4;
            this.button_CreateMap.Text = "CreateMap";
            this.button_CreateMap.UseVisualStyleBackColor = true;
            this.button_CreateMap.Click += new System.EventHandler(this.button_CreateMap_Click);
            // 
            // label_HeightInput
            // 
            this.label_HeightInput.AutoSize = true;
            this.label_HeightInput.Location = new System.Drawing.Point(15, 68);
            this.label_HeightInput.Name = "label_HeightInput";
            this.label_HeightInput.Size = new System.Drawing.Size(75, 15);
            this.label_HeightInput.TabIndex = 3;
            this.label_HeightInput.Text = "Height (tiles)";
            // 
            // textBox_HeightInput
            // 
            this.textBox_HeightInput.Location = new System.Drawing.Point(95, 65);
            this.textBox_HeightInput.Name = "textBox_HeightInput";
            this.textBox_HeightInput.Size = new System.Drawing.Size(125, 23);
            this.textBox_HeightInput.TabIndex = 2;
            this.textBox_HeightInput.Text = "32";
            // 
            // textBox_WidthInput
            // 
            this.textBox_WidthInput.Location = new System.Drawing.Point(95, 27);
            this.textBox_WidthInput.Name = "textBox_WidthInput";
            this.textBox_WidthInput.Size = new System.Drawing.Size(125, 23);
            this.textBox_WidthInput.TabIndex = 1;
            this.textBox_WidthInput.Text = "48";
            // 
            // label_WidthInput
            // 
            this.label_WidthInput.AutoSize = true;
            this.label_WidthInput.Location = new System.Drawing.Point(15, 30);
            this.label_WidthInput.Name = "label_WidthInput";
            this.label_WidthInput.Size = new System.Drawing.Size(71, 15);
            this.label_WidthInput.TabIndex = 0;
            this.label_WidthInput.Text = "Width (tiles)";
            // 
            // MainMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 296);
            this.Controls.Add(this.groupBox_CreateNewMapMenu);
            this.Controls.Add(this.button_LoadMap);
            this.MaximizeBox = false;
            this.Name = "MainMenuForm";
            this.Text = "Level Editor";
            this.groupBox_CreateNewMapMenu.ResumeLayout(false);
            this.groupBox_CreateNewMapMenu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Button button_LoadMap;
        private GroupBox groupBox_CreateNewMapMenu;
        private Button button_CreateMap;
        private Label label_HeightInput;
        private TextBox textBox_HeightInput;
        private TextBox textBox_WidthInput;
        private Label label_WidthInput;
    }
}