﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
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
        private Image[,] _currentMapGrid;

        private PictureBox[,] _displayGrid;

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
            Image[,] grid = new Image[height, width];
            
            /*
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    //create a blue picturebox to put at each index of the array
                    PictureBox pb = new PictureBox();
                    pb.BackColor = Color.Gray;

                    // place that picture box in the correct spot in the grid
                    grid[y, x] = pb;
                }
            }
            */

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

            // set the tile to the selected one
            tileThatWasClicked.Image = pictureBox_CurrentTile.Image;

            // find the index of the tile
            for (int y = 0; y < _displayGrid.GetLength(0); y++)
            {
                for (int x = 0; x < _displayGrid.GetLength(1); x++)
                {
                    if (_displayGrid[y, x] == tileThatWasClicked)
                    {
                        // set the position in the image grid equal to the image of the picturebox
                        _currentMapGrid[y + ScrollBarY.Value, x + ScrollBarX.Value] = _displayGrid[y, x].Image;
                    }
                }
            }

            // report that there are unsaved changes in title bar
            // set _unsavedChanges to true
            if (_unsavedChanges == false)
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
                tileThatWasClicked.Image = pictureBox_CurrentTile.Image;

                // find the index of the tile
                for (int y = 0; y < _displayGrid.GetLength(0); y++)
                {
                    for (int x = 0; x < _displayGrid.GetLength(1); x++)
                    {
                        if (_displayGrid[y, x] == tileThatWasClicked)
                        {
                            // set the position in the image grid equal to the image of the picturebox
                            _currentMapGrid[y + ScrollBarY.Value, x + ScrollBarX.Value] = _displayGrid[y, x].Image;
                        }
                    }
                }

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
        public void tilePicker_Click(object? sender, EventArgs e)
        {
            // make sure the cast from object to Button works
            // otherwise don't do anything
            PictureBox tilePickerThatWasClicked = null;

            if (sender is PictureBox == false)
            {
                return;
            }
            else
            {
                tilePickerThatWasClicked = (PictureBox)sender;
            }

            // set the current tile's color to the color of the button that was clicked
            pictureBox_CurrentTile.Image = tilePickerThatWasClicked.Image;
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
                        Image i = _currentMapGrid[y, x];

                        // write the names of the images in the grid
                        // (each line of the file = a line of picture boxes in the grid)
                        if(i == tilePicker_TestTile.Image)
                        {
                            writer.Write("testTile,");
                        }
                        else if(i == tilePicker_Snek.Image)
                        {
                            writer.Write("snek,");
                        }
                        else if(i == tilePicker_PlayerStart.Image)
                        {
                            writer.Write("playerStart,");
                        }
                        else if(i == tilePicker_LevelEnd.Image)
                        {
                            writer.Write("levelEnd,");
                        }
                        // if it doesn't match either image, it must be empty
                        else
                        {
                            writer.Write("air,");
                        }
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
        private void BuildMap(Image[,] grid)
        {
            // create the display grid (y, x) coordinate format
            _displayGrid = new PictureBox[32, 48];

            // set the tile size so that the display grid's height fits within the height of the map view
            _tileSize = (groupBox_MapView.Height) / _displayGrid.GetLength(0);

            // set the map view box's width to fit the full display grid
            groupBox_MapView.Width = _tileSize * _displayGrid.GetLength(1);

            // set the X scrollbar's width to match the view box
            ScrollBarX.Width = groupBox_MapView.Width;

            // set the Y scrollbar's location to be 6 pixels after the edge of the view box
            ScrollBarY.Location = new Point(groupBox_MapView.Location.X + groupBox_MapView.Width + 6, ScrollBarY.Location.Y);

            // set the current grid equal to the grid being built
            _currentMapGrid = grid;

            // set unsaved changes to false (a new map is being built)
            _unsavedChanges = false;

            // turn the grid's dimentions into the width and height of the map
            int height = grid.GetLength(0);
            int width = grid.GetLength(1);

            // set the max values of the scrollbars to match the grid size
            // add 8 to account for the size of the scrollbar
            ScrollBarY.Maximum = height - _displayGrid.GetLength(0);
            ScrollBarX.Maximum = width - _displayGrid.GetLength(1);
            

            // the Y scrollbar starts at the bottom (the scrollbar cannot reach the maximum)
            ScrollBarY.Value = ScrollBarY.Maximum;

            // set the length of the scrollbar to value 1
            ScrollBarY.LargeChange = 1;
            ScrollBarX.LargeChange = 1;

            // set the window size to fit the map view window (with a margin of 30)
            this.Width = groupBox_MapView.Location.X + groupBox_MapView.Width + ScrollBarY.Width + 30;

            for(int y = 0; y < _displayGrid.GetLength(0); y++)
            {
                for(int x = 0; x < _displayGrid.GetLength(1); x++)
                {
                    // create a new picturebox at the correct place in the grid
                    _displayGrid[y, x] = new PictureBox();

                    // set the background to gray
                    _displayGrid[y, x].BackColor = Color.Gray;

                    // set the picturebox to the correct size
                    _displayGrid[y, x].Size = new Size(_tileSize, _tileSize);

                    // this should set the boxes' edge-to-edge with eachother within the map view
                    _displayGrid[y, x].Location = new Point(x * _tileSize, y * _tileSize);

                    _displayGrid[y, x].SizeMode = PictureBoxSizeMode.StretchImage;

                    // add tile_click to the click event
                    _displayGrid[y, x].MouseDown += tile_Click;
                    _displayGrid[y, x].MouseEnter += tile_MouseEnter;

                    // add the picturebox to the controls list
                    // add the picturebox to the controls list
                    groupBox_MapView.Controls.Add(_displayGrid[y, x]);
                }
            }

            // show only tiles within the first 48 x or the last 32 y (y is the first dimetion)
            // all shown tiles will be positioned edge-to edge in the window
            for (int y = 0; y < _displayGrid.GetLength(0); y++)
            {
                for(int x = 0; x < _displayGrid.GetLength(1); x++)
                {
                    // display the first 48 images in the x direction and the last 32 in the y
                    _displayGrid[y, x].Image = grid[y + grid.GetLength(0) - 32, x];
                }
            }
        }

        /// <summary>
        /// Takes a filepath and turns the specified 
        /// .level file into a grid of pictureboxes
        /// </summary>
        /// <param name="filePath">The path to the .level file</param>
        /// <returns>A grid of pictureboxes with colors loaded from a file</returns>
        private Image[,] LoadMapGrid(string filePath)
        {
            // open the specified file for reading.
            StreamReader reader = new StreamReader(File.OpenRead(filePath));

            //read the first line to get the y and x dimentions of the grid
            string[] dimentions = reader.ReadLine().Split(',');
            int height = int.Parse(dimentions[0]);
            int width = int.Parse(dimentions[1]);

            // create the empty grid
            Image[,] grid = new Image[height, width];

            for(int y = 0; y < height; y++)
            {
                // get an array of strings that represent
                // the images on each picturebox on this line
                string[] currentLine = reader.ReadLine().Split(',');

                for(int x = 0; x < width; x++)
                {
                    // set up an image to add to the grid, if there is nothing
                    // at the location in the saved file the image will be null
                    Image i = null;

                    if (currentLine[x] == "testTile")
                    {
                        i = tilePicker_TestTile.Image;
                    }
                    else if (currentLine[x] == "snek")
                    {
                        i = tilePicker_Snek.Image;
                    }
                    else if (currentLine[x] == "playerStart")
                    {
                        i = tilePicker_PlayerStart.Image;
                    }
                    else if (currentLine[x] == "levelEnd")
                    {
                        i = tilePicker_LevelEnd.Image;
                    }

                    // place the new image in the grid
                    grid[y, x] = i;
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

            // loop through all display tiles
            for(int y = 0; y < _displayGrid.GetLength(0); y++)
            {
                for(int x = 0; x < _displayGrid.GetLength(1); x++)
                {
                    _displayGrid[y, x].Image = _currentMapGrid[y + ScrollBarY.Value, x + ScrollBarX.Value];
                }
            }

            /*
            for(int y = 0; y < _currentMapGrid.GetLength(0); y++)
            {
                for(int x = 0; x < _currentMapGrid.GetLength(1); x++)
                {
                    // if theyre within the scrollbar value range, display them on the display grid
                    if((y >= ScrollBarY.Value && y < ScrollBarY.Value + 32) &&
                        (x >= ScrollBarX.Value && x < ScrollBarX.Value + 48))
                    {
                        _displayGrid[ _currentMapGrid[y, x].Location = new Point((x - ScrollBarX.Value) * _tileSize + 6, (y - ScrollBarY.Value) * _tileSize + 15);
                        _currentMapGrid[y, x].Show();
                    }
                    // otherwise hide them
                    else
                    {
                        _currentMapGrid[y, x].Hide();
                    }
                }
            }
            */

            groupBox_MapView.ResumeLayout();
        }
    }
}
