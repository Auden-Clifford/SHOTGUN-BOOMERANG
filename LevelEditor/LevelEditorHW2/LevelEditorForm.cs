using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * Auden Clifford
 * 2/7/2023
 * Homework 2: the level editor
 */

namespace LevelEditor
{
    /// <summary>
    /// Opens a window that allows the 
    /// user to edit a map by changing tile colors
    /// </summary>
    public partial class LevelEditorForm : Form
    {
        //Fields
        private int _viewHeight;

        // stores all the picture boxes in the current map
        private PictureBox[,] _currentMapGrid;

        // tracks whether there are unsaved changes
        private bool _unsavedChanges;

        // store the tile size
        private int _tileSize;


        // constructors

        /// <summary>
        /// Creates a new level editor window with a 
        /// new map of the spacified width and height
        /// </summary>
        /// <param name="width">Width of the map (in tiles)</param>
        /// <param name="height">Height of the map (in height)</param>
        public LevelEditorForm(int width, int height)
        {
            InitializeComponent();

            //create a new grid of the specified dimentions
            PictureBox[,] grid = new PictureBox[height, width];

            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    //create a blue picturebox to put at each index of the array
                    PictureBox pb = new PictureBox();
                    pb.BackColor = Color.CadetBlue;

                    // place that picture box in the correct spot in the grid
                    grid[y, x] = pb;
                }
            }

            // build the map from the new grid
            BuildMap(grid);
        }

        /// <summary>
        /// Creates a new level editor window 
        /// with a map loaded from a file
        /// </summary>
        /// <param name="filepath">File to load from</param>
        public LevelEditorForm (string filepath)
        {
            InitializeComponent();

            BuildMap(LoadMapGrid(filepath));
        }


        // Event Methods

        /// <summary>
        /// Sets the back color of the tile that 
        /// was clicked equal to the selected tile color
        /// </summary>
        public void tile_Click(object? sender, EventArgs e)
        {
            // make sure the cast from object to PictureBox works
            // otherwise don't do anything
            PictureBox tileThatWasClicked = null;

            if (sender is PictureBox == false)
            {
                return;
            }
            else
            {
                tileThatWasClicked = (PictureBox)sender;
            }

            // do not capture the mouse input, allows other tiles to pick up on input
            tileThatWasClicked.Capture = false;

            // set the color of the tile to the selected color
            tileThatWasClicked.BackColor = pictureBox_CurrentTile.BackColor;

            // report that there are unsaved changes in title bar
            // set _unsavedChanges to true
            if(_unsavedChanges == false)
            {
                this.Text += " *";
                _unsavedChanges = true;
            }
        }

        /// <summary>
        /// Allows the user to draw by clicking and dragging; 
        /// checks if the left mouse button is down when the 
        /// mouse enters the tile, changes the back color if true
        /// </summary>
        private void tile_MouseEnter(object? sender, EventArgs e)
        {
            // make sure the cast from object to PictureBox works
            // otherwise don't do anything
            PictureBox tileThatWasClicked = null;

            if (sender is PictureBox == false)
            {
                return;
            }
            else
            {
                tileThatWasClicked = (PictureBox)sender;
            }

            // check if the left mouse button is down when it enters
            if(Control.MouseButtons == MouseButtons.Left)
            {
                // set the color of the tile to the selected color
                tileThatWasClicked.BackColor = pictureBox_CurrentTile.BackColor;

                // report that there are unsaved changes in title bar
                // set _unsavedChanges to true
                if (_unsavedChanges == false)
                {
                    this.Text += " *";
                    _unsavedChanges = true;
                }
            }
        }
        
        /// <summary>
        /// Sets the current tile's color equal to
        /// the color button the player clicked on
        /// </summary>
        public void colorPicker_Click(object? sender, EventArgs e)
        {
            // make sure the cast from object to Button works
            // otherwise don't do anything
            Button colorButtonThatWasClicked = null;

            if (sender is Button == false)
            {
                return;
            }
            else
            {
                colorButtonThatWasClicked = (Button)sender;
            }

            // set the current tile's color to the color of the button that was clicked
            pictureBox_CurrentTile.BackColor = colorButtonThatWasClicked.BackColor;
        }

        /// <summary>
        /// Takes the current map grid and saves it as a 
        /// .level file at a location specified by the user
        /// </summary>
        private void button_Save_Click(object sender, EventArgs e)
        {
            // set unsaved changes to false
            _unsavedChanges = false;

            // create a new save file dialog so the user can choose a path
            SaveFileDialog saveMapFile = new SaveFileDialog();

            //set up the saveFileDialog's properties for map saving
            saveMapFile.Title = "Save a level file";
            saveMapFile.Filter = "Level Files|*.level";

            // show the file dialog and allow the player to choose a save path
            // if the player chooses a path the map will be saved to that path.
            if(saveMapFile.ShowDialog() == DialogResult.OK)
            {
                // open a new file stream at the spacified path
                StreamWriter writer = new StreamWriter(File.OpenWrite(saveMapFile.FileName));

                // write the size of the grid on the first line in y,x format
                writer.WriteLine(_currentMapGrid.GetLength(0) + "," + _currentMapGrid.GetLength(1));

                for(int y = 0; y < _currentMapGrid.GetLength(0); y++)
                {
                    for(int x = 0; x < _currentMapGrid.GetLength(1); x++)
                    {
                        PictureBox pb = _currentMapGrid[y, x];

                        // write the picture boxes' colors separated by a comma
                        // (each line of the file = a line of picture boxes in the grid)
                        writer.Write(pb.BackColor.ToArgb() + ",");
                    }

                    // start a new line for the next set of picture boxes
                    writer.WriteLine();
                }

                //close the stream after
                writer.Close();

                // put up a message box informing the user that the file saved sucessfully
                MessageBox.Show("File saved successfully", "File Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // add the level's name to the title bar of the window
                // the level's name should be the last item in the file path
                this.Text = "Level Editor - " + saveMapFile.FileName.Split('\\')[^1];
            }
        }

        /// <summary>
        /// Allows the user to choose a map file to load
        /// </summary>
        private void button_Load_Click(object sender, EventArgs e)
        {
            //create the file dialog window
            OpenFileDialog openMapFile = new OpenFileDialog();

            //customize the file dialog for map opening
            openMapFile.Title = "Open a level file";
            openMapFile.Filter = "Level Files|*.level";

            //show the window and allow the player to choose a file
            //if the player chooses a file, the map will be loaded into the editor window
            if (openMapFile.ShowDialog() == DialogResult.OK)
            {
                // remove the current map from view
                groupBox_MapView.Controls.Clear();

                // build the map stored in the specified file
                BuildMap(LoadMapGrid(openMapFile.FileName));
            }
        }

        /// <summary>
        /// If there are unsaved changes when the user attempts to close the program 
        /// a message box will appear to ask them if they are sure they wish to quit. 
        /// if the player clicks "no" the program will not close
        /// </summary>
        private void LevelEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(_unsavedChanges == true)
            {
                // if the user decides not to quit, cancel the closing
                if (MessageBox.Show("There are unsaved changes. Are you sure you want to quit?",
                    "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Takes a grid of Pictureboxes as 
        /// input and displays them on screen
        /// </summary>
        /// <param name="grid">the 2D array of PictureBoxes which will be placed on screen</param>
        private void BuildMap(PictureBox[,] grid)
        {
            // set the current grid equal to the grid being built
            _currentMapGrid = grid;

            // set unsaved changes to false (a new map is being built)
            _unsavedChanges = false;

            // turn the grid's dimentions into the width and height of the map
            int height = grid.GetLength(0);
            int width = grid.GetLength(1);

            // set the tile size so 32 tiles fit within the height of the map view (with a margin of 12; 6 on each side)
            _tileSize = (groupBox_MapView.Height - 12) / 32;

            // set the map view box's width to fit the tiles (with a margin of 12; 6 on each side)
            groupBox_MapView.Width = _tileSize * 48 + 12;

            // set the X scrollbar's width to match the view box
            ScrollBarX.Width = groupBox_MapView.Width;

            // set the Y scrollbar's cocation to be just after the edge of the view box
            ScrollBarY.Location = new Point(groupBox_MapView.Location.X + groupBox_MapView.Width + 6, ScrollBarY.Location.Y);

            // set the max values of the scrollbars to match the grid size
            // add 8 to account for the size of the scrollbar
            ScrollBarX.Maximum = width - 48;
            ScrollBarY.Maximum = height - 32;

            // the Y scrollbar starts at the bottom (the scrollbar cannot reach the maximum)
            ScrollBarY.Value = ScrollBarY.Maximum;

            ScrollBarY.LargeChange = 1;
            ScrollBarX.LargeChange = 1;

            // set the window size to fit the map view window (with a margin of 30)
            this.Width = groupBox_MapView.Location.X + groupBox_MapView.Width + ScrollBarY.Width + 30;

            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    // use the picturebox already in the grid
                    PictureBox pb = grid[y, x];

                    //set it's size
                    pb.Size = new Size(_tileSize, _tileSize);

                    //this should set the boxes edge-to-edge with each other
                    // y is given a margin of 15 with the map border
                    // x is given a margin of 6 with the map border
                    pb.Location = new Point(x * _tileSize + 6, y * _tileSize + 15);

                    // add tile_click to the click event
                    pb.MouseDown += tile_Click;
                    pb.MouseEnter += tile_MouseEnter;

                    // add the picturebox to the controls list
                    groupBox_MapView.Controls.Add(pb);

                    // hide all tiles t first
                    pb.Hide();
                }
            }

            // show only tiles within the first 48 x or the last 32 y (y is the first dimetion)
            // all shown tiles will be positioned edge-to edge in the window
            for (int y = 0; y < 32; y++)
            {
                for(int x = 0; x < 48; x++)
                {
                    grid[y + grid.GetLength(0) - 32, x].Location = new Point(x * _tileSize + 6, y * _tileSize + 15);
                    grid[y + grid.GetLength(0) - 32, x].Show();
                }
            }
        }

        /// <summary>
        /// Takes a filepath and turns the specified 
        /// .level file into a grid of pictureboxes
        /// </summary>
        /// <param name="filePath">The path to the .level file</param>
        /// <returns>A grid of pictureboxes with colors loaded from a file</returns>
        private PictureBox[,] LoadMapGrid(string filePath)
        {
            // open the specified file for reading.
            StreamReader reader = new StreamReader(File.OpenRead(filePath));

            //read the first line to get the y and x dimentions of the grid
            string[] dimentions = reader.ReadLine().Split(',');
            int height = int.Parse(dimentions[0]);
            int width = int.Parse(dimentions[1]);

            // create the empty grid
            PictureBox[,] grid = new PictureBox[height, width];

            for(int y = 0; y < height; y++)
            {
                // get a string that represents the colors
                // of every picturebox in the current line
                string[] currentLine = reader.ReadLine().Split(',');

                for(int x = 0; x < width; x++)
                {
                    // create a new picturebox and give
                    // it a color from the saved file
                    PictureBox pb = new PictureBox();
                    pb.BackColor = Color.FromArgb(int.Parse(currentLine[x]));

                    // place the new picturebox in the grid
                    grid[y, x] = pb;
                }
            }

            // close the stream reader
            reader.Close();

            // put up a message box informing the user that the file loaded sucessfully
            MessageBox.Show("File loaded successfully", "File Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // add the level's name to the title bar of the window
            // the level's name should be the last item in the file path
            this.Text = "Level Editor - " + filePath.Split('\\')[^1];

            return grid;
        }

        /// <summary>
        /// Allows the user to scroll throughout a 
        /// level that is larger than the view screen
        /// </summary>
        private void ScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            groupBox_MapView.SuspendLayout();

            // loop through all tiles
            for(int y = 0; y < _currentMapGrid.GetLength(0); y++)
            {
                for(int x = 0; x < _currentMapGrid.GetLength(1); x++)
                {
                    // if theyre within the scrollbar value range, show them and position them
                    // so that they properly fill the screen and sit edge-to-edge
                    if((y >= ScrollBarY.Value && y < ScrollBarY.Value + 32) &&
                        (x >= ScrollBarX.Value && x < ScrollBarX.Value + 48))
                    {
                        _currentMapGrid[y, x].Location = new Point((x - ScrollBarX.Value) * _tileSize + 6, (y - ScrollBarY.Value) * _tileSize + 15);
                        _currentMapGrid[y, x].Show();
                    }
                    // otherwise hide them
                    else
                    {
                        _currentMapGrid[y, x].Hide();
                    }
                }
            }

            groupBox_MapView.ResumeLayout();
        }
    }
}
