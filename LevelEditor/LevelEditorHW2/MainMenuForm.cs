/*
 * Auden Clifford
 * 2/7/2023
 * Homework 2: the level editor
 */

namespace LevelEditor
{
    /// <summary>
    /// Opens a window that allows the user to either create a new map 
    /// with a custom width and height or load a map from a .level file
    /// </summary>
    public partial class MainMenuForm : Form
    {
        // Fields
        private int minHeight;
        private int maxHeight;

        private int minWidth;
        private int maxWidth;
        /// <summary>
        /// opens the window at the beginning of the program
        /// </summary>
        public MainMenuForm()
        {
            InitializeComponent();

            minHeight = 32;
            maxHeight = 256;

            minWidth = 48;
            maxWidth = 256;
        }

        /// <summary>
        /// Checks whether the given width and height values 
        /// are winin range, then creates a new map of the 
        /// spacified width and height
        /// </summary>
        private void button_CreateMap_Click(object sender, EventArgs e)
        {
            int widthInput = int.Parse(textBox_WidthInput.Text);
            int heightInput = int.Parse(textBox_HeightInput.Text);

            string errorMessage = "Errors:";

            //check if width is below minimum
            if(widthInput < minWidth)
            {
                errorMessage += "\n - Width too small. Minimum is 10";
            }
            
            //check if width is above maximum
            if(widthInput > maxWidth)
            {
                errorMessage += "\n - Width too large. Maximum is 30";
            }

            //check if height is below minimum
            if(heightInput < minHeight)
            {
                errorMessage += "\n - Height too small. Minimum is 10";
            }

            //check if height is above maximum
            if(heightInput > maxHeight)
            {
                errorMessage += "\n - height too large. Maximum is 30";
            }

            //if anything was added to the error
            //message, print it in a message box
            if(errorMessage.Length > 7)
            {
                MessageBox.Show(
                    errorMessage, 
                    "Error creating map", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
            else //otherwise create a new map
            {
                // feed it the width and height
                LevelEditorForm levelEditor = new LevelEditorForm(widthInput, heightInput);

                // show the window (without allowing the user to click on the old window)
                levelEditor.ShowDialog();
            }

        }

        /// <summary>
        /// Propmts the user to choose a file through file diolog, 
        /// then sends that file path to the Level Editor to be opened
        /// </summary>
        private void button_LoadMap_Click(object sender, EventArgs e)
        {
            //create the file dialog window
            OpenFileDialog openMapFile = new OpenFileDialog();

            //customize the file dialog for map opening
            openMapFile.Title = "Open a level file";
            openMapFile.Filter = "Level Files|*.level";

            //show the window and allow the player to choose a file
            //if the player chooses a file, the level editor will be opened with the file.
            if(openMapFile.ShowDialog() == DialogResult.OK)
            {
                // give the level editor constructor the retrieved file path
                LevelEditorForm levelEditor = new LevelEditorForm(openMapFile.FileName);

                // show the window (without allowing the user to click on the old window)
                levelEditor.ShowDialog();
            }
        }
    }
}