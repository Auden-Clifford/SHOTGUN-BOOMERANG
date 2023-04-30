using System;
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

        // formatting
        private int _largeMargin;
        private int _smallMargin;

        // tile catagories
        private List<PictureBox> _miscTiles;
        private List<PictureBox> _entities;
        private List<PictureBox> _grass;
        private List<PictureBox> _cave;
        private List<PictureBox> _mesa;


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

            // build the map from the new grid
            BuildMapEditor(grid);
        }

        /// <summary>
        /// Creates a new level editor window 
        /// with a map loaded from a file
        /// </summary>
        /// <param name="filepath">File to load from</param>
        public LevelEditorForm (string filepath)
        {
            InitializeComponent();

            BuildMapEditor(LoadMapGrid(filepath));
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

            // set the current tile's image to the image of the button that was clicked
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

                        // save test tiles
                        if(i == tilePicker_TestTile.Image)
                        {
                            writer.Write("testTile,");
                        }
                        // save entities
                        else if(i == tilePicker_Snek.Image)
                        {
                            writer.Write("snek,");
                        }
                        else if(i == tilePicker_Scorpion.Image)
                        {
                            writer.Write("scorpion,");
                        }
                        else if(i == tilePicker_PlayerStart.Image)
                        {
                            writer.Write("playerStart,");
                        }
                        else if(i == tilePicker_LevelEnd.Image)
                        {
                            writer.Write("levelEnd,");
                        }
                        else if(i == tilePicker_Vegemite.Image)
                        {
                            writer.Write("vegemite,");
                        }
                        else if(i == tilePicker_WoodSpike.Image)
                        {
                            writer.Write("woodSpike,");
                        }
                        // save plank tiles
                        else if(i == tilePicker_PlanksLeft.Image)
                        {
                            writer.Write("planksLeft,");
                        }
                        else if (i == tilePicker_PlanksCenter.Image)
                        {
                            writer.Write("planksCenter,");
                        }
                        else if (i == tilePicker_PlanksRight.Image)
                        {
                            writer.Write("planksRight,");
                        }
                        // save brick tiles
                        else if (i == tilePicker_Bricks.Image)
                        {
                            writer.Write("bricks,");
                        }
                        // save grass tiles
                        else if(i == tilePicker_GrassBottomCenter.Image)
                        {
                            writer.Write("grassBottomCenter,");
                        }
                        else if (i == tilePicker_GrassBottomLeft.Image)
                        {
                            writer.Write("grassBottomLeft,");
                        }
                        else if (i == tilePicker_GrassBottomRight.Image)
                        {
                            writer.Write("grassBottomRight,");
                        }
                        else if (i == tilePicker_GrassCenterCenter.Image)
                        {
                            writer.Write("grassCenterCenter,");
                        }
                        else if (i == tilePicker_GrassCenterLeft.Image)
                        {
                            writer.Write("grassCenterLeft,");
                        }
                        else if (i == tilePicker_GrassCenterRight.Image)
                        {
                            writer.Write("grassCenterRight,");
                        }
                        else if (i == tilePicker_GrassTopCenter.Image)
                        {
                            writer.Write("grassTopCenter,");
                        }
                        else if (i == tilePicker_GrassTopLeft.Image)
                        {
                            writer.Write("grassTopLeft,");
                        }
                        else if (i == tilePicker_GrassTopRight.Image)
                        {
                            writer.Write("grassTopRight,");
                        }
                        // save cave tiles
                        else if (i == tilePicker_CaveBottomCenter.Image)
                        {
                            writer.Write("caveBottomCenter,");
                        }
                        else if (i == tilePicker_CaveBottomLeft.Image)
                        {
                            writer.Write("caveBottomLeft,");
                        }
                        else if (i == tilePicker_CaveBottomRight.Image)
                        {
                            writer.Write("caveBottomRight,");
                        }
                        else if (i == tilePicker_CaveCenterCenter.Image)
                        {
                            writer.Write("caveCenterCenter,");
                        }
                        else if (i == tilePicker_CaveCenterLeft.Image)
                        {
                            writer.Write("caveCenterLeft,");
                        }
                        else if (i == tilePicker_CaveCenterRight.Image)
                        {
                            writer.Write("caveCenterRight,");
                        }
                        else if (i == tilePicker_CaveTopCenter.Image)
                        {
                            writer.Write("caveTopCenter,");
                        }
                        else if (i == tilePicker_CaveTopLeft.Image)
                        {
                            writer.Write("caveTopLeft,");
                        }
                        else if (i == tilePicker_CaveTopRight.Image)
                        {
                            writer.Write("caveTopRight,");
                        }
                        // save mesa tiles
                        else if (i == tilePicker_MesaBottomCenter.Image)
                        {
                            writer.Write("mesaBottomCenter,");
                        }
                        else if (i == tilePicker_MesaBottomLeft.Image)
                        {
                            writer.Write("mesaBottomLeft,");
                        }
                        else if (i == tilePicker_MesaBottomRight.Image)
                        {
                            writer.Write("mesaBottomRight,");
                        }
                        else if (i == tilePicker_MesaCenterCenter.Image)
                        {
                            writer.Write("mesaCenterCenter,");
                        }
                        else if (i == tilePicker_MesaCenterLeft.Image)
                        {
                            writer.Write("mesaCenterLeft,");
                        }
                        else if (i == tilePicker_MesaCenterRight.Image)
                        {
                            writer.Write("mesaCenterRight,");
                        }
                        else if (i == tilePicker_MesaTopCenter.Image)
                        {
                            writer.Write("mesaTopCenter,");
                        }
                        else if (i == tilePicker_MesaTopLeft.Image)
                        {
                            writer.Write("mesaTopLeft,");
                        }
                        else if (i == tilePicker_MesaTopRight.Image)
                        {
                            writer.Write("mesaTopRight,");
                        }
                        // if it doesn't match any image, it must be empty
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
                BuildMapEditor(LoadMapGrid(openMapFile.FileName));
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
        /// sets up and initializes most of the pieces required for the level editor
        /// takes an array of images as input to build the level
        /// </summary>
        /// <param name="grid">the 2D array of images which will be placed on screen</param>
        private void BuildMapEditor(Image[,] grid)
        {
            this.Focus();

            KeyPreview = true;

            // initialize the catagory lists and add the proper tile pickers
            _miscTiles = new List<PictureBox>()
            {
                tilePicker_TestTile,
                tilePicker_PlanksLeft,
                tilePicker_PlanksCenter,
                tilePicker_PlanksRight,
                tilePicker_Bricks
            };

            _entities = new List<PictureBox>()
            {
                tilePicker_LevelEnd,
                tilePicker_PlayerStart,
                tilePicker_Snek,
                tilePicker_Scorpion,
                tilePicker_Vegemite,
                tilePicker_WoodSpike
            };

            _grass = new List<PictureBox>()
            {
                tilePicker_GrassTopLeft, tilePicker_GrassTopCenter, tilePicker_GrassTopRight,
                tilePicker_GrassCenterLeft, tilePicker_GrassCenterCenter, tilePicker_GrassCenterRight,
                tilePicker_GrassBottomLeft, tilePicker_GrassBottomCenter, tilePicker_GrassBottomRight,
            };

            _cave = new List<PictureBox>()
            {
                tilePicker_CaveTopLeft, tilePicker_CaveTopCenter, tilePicker_CaveTopRight,
                tilePicker_CaveCenterLeft, tilePicker_CaveCenterCenter, tilePicker_CaveCenterRight,
                tilePicker_CaveBottomLeft, tilePicker_CaveBottomCenter, tilePicker_CaveBottomRight,
            };

            _mesa = new List<PictureBox>()
            {
                tilePicker_MesaTopLeft, tilePicker_MesaTopCenter, tilePicker_MesaTopRight,
                tilePicker_MesaCenterLeft, tilePicker_MesaCenterCenter, tilePicker_MesaCenterRight,
                tilePicker_MesaBottomLeft, tilePicker_MesaBottomCenter, tilePicker_MesaBottomRight,
            };

            // set the selected catagory to the first index
            comboBox_TilePickerCatagories.SelectedIndex = 0;

            // set up formatting things
            _largeMargin = 12;
            _smallMargin = 6;

            // create the display grid (y, x) coordinate format
            _displayGrid = new PictureBox[32, 48];

            // set the tile size so that the display grid's height fits within the height of the map view
            _tileSize = (groupBox_MapView.Height - _largeMargin * 2) / _displayGrid.GetLength(0);

            // set the map view box's width to fit the full display grid
            groupBox_MapView.Width = _tileSize * _displayGrid.GetLength(1) + _largeMargin * 2;

            // set the X scrollbar's width to match the view box
            ScrollBarX.Width = groupBox_MapView.Width;

            // set the Y scrollbar's location to be 6 pixels after the edge of the view box
            ScrollBarY.Location = new Point(groupBox_MapView.Location.X + groupBox_MapView.Width + _smallMargin, ScrollBarY.Location.Y);

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
            this.Width = groupBox_MapView.Location.X + groupBox_MapView.Width + ScrollBarY.Width + _largeMargin * 2;

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
                    _displayGrid[y, x].Location = new Point(x * _tileSize + _largeMargin, y * _tileSize + _largeMargin * 2);

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

                    // load test tiles
                    if (currentLine[x] == "testTile")
                    {
                        i = tilePicker_TestTile.Image;
                    }
                    // load entities
                    else if (currentLine[x] == "snek")
                    {
                        i = tilePicker_Snek.Image;
                    }
                    else if (currentLine[x] == "scorpion")
                    {
                        i = tilePicker_Scorpion.Image;
                    }
                    else if (currentLine[x] == "playerStart")
                    {
                        i = tilePicker_PlayerStart.Image;
                    }
                    else if (currentLine[x] == "levelEnd")
                    {
                        i = tilePicker_LevelEnd.Image;
                    }
                    else if (currentLine[x] == "vegemite")
                    {
                        i = tilePicker_Vegemite.Image;
                    }
                    else if (currentLine[x] == "woodSpike,")
                    {
                        i = tilePicker_WoodSpike.Image;
                    }
                    // load plank tiles
                    else if (currentLine[x] == "planksLeft")
                    {
                        i = tilePicker_PlanksLeft.Image;
                    }
                    else if (currentLine[x] == "planksCenter")
                    {
                        i = tilePicker_PlanksCenter.Image;
                    }
                    else if (currentLine[x] == "planksRight")
                    {
                        i = tilePicker_PlanksRight.Image;
                    }
                    else if (currentLine[x] == "bricks")
                    {
                        i = tilePicker_Bricks.Image;
                    }
                    // load grass tiles
                    else if (currentLine[x] == "grassBottomCenter")
                    {
                        i = tilePicker_GrassBottomCenter.Image;
                    }
                    else if (currentLine[x] == "grassBottomLeft")
                    {
                        i = tilePicker_GrassBottomLeft.Image;
                    }
                    else if (currentLine[x] == "grassBottomRight")
                    {
                        i = tilePicker_GrassBottomRight.Image;
                    }
                    else if (currentLine[x] == "grassCenterCenter")
                    {
                        i = tilePicker_GrassCenterCenter.Image;
                    }
                    else if (currentLine[x] == "grassCenterLeft")
                    {
                        i = tilePicker_GrassCenterLeft.Image;
                    }
                    else if (currentLine[x] == "grassCenterRight")
                    {
                        i = tilePicker_GrassCenterRight.Image;
                    }
                    else if (currentLine[x] == "grassTopCenter")
                    {
                        i = tilePicker_GrassTopCenter.Image;
                    }
                    else if (currentLine[x] == "grassTopLeft")
                    {
                        i = tilePicker_GrassTopLeft.Image;
                    }
                    else if (currentLine[x] == "grassTopRight")
                    {
                        i = tilePicker_GrassTopRight.Image;
                    }
                    // load cave tiles
                    else if (currentLine[x] == "caveBottomCenter")
                    {
                        i = tilePicker_CaveBottomCenter.Image;
                    }
                    else if (currentLine[x] == "caveBottomLeft")
                    {
                        i = tilePicker_CaveBottomLeft.Image;
                    }
                    else if (currentLine[x] == "caveBottomRight")
                    {
                        i = tilePicker_CaveBottomRight.Image;
                    }
                    else if (currentLine[x] == "caveCenterCenter")
                    {
                        i = tilePicker_CaveCenterCenter.Image;
                    }
                    else if (currentLine[x] == "caveCenterLeft")
                    {
                        i = tilePicker_CaveCenterLeft.Image;
                    }
                    else if (currentLine[x] == "caveCenterRight")
                    {
                        i = tilePicker_CaveCenterRight.Image;
                    }
                    else if (currentLine[x] == "caveTopCenter")
                    {
                        i = tilePicker_CaveTopCenter.Image;
                    }
                    else if (currentLine[x] == "caveTopLeft")
                    {
                        i = tilePicker_CaveTopLeft.Image;
                    }
                    else if (currentLine[x] == "caveTopRight")
                    {
                        i = tilePicker_CaveTopRight.Image;
                    }
                    // load mesa tiles
                    else if (currentLine[x] == "mesaBottomCenter")
                    {
                        i = tilePicker_MesaBottomCenter.Image;
                    }
                    else if (currentLine[x] == "mesaBottomLeft")
                    {
                        i = tilePicker_MesaBottomLeft.Image;
                    }
                    else if (currentLine[x] == "mesaBottomRight")
                    {
                        i = tilePicker_MesaBottomRight.Image;
                    }
                    else if (currentLine[x] == "mesaCenterCenter")
                    {
                        i = tilePicker_MesaCenterCenter.Image;
                    }
                    else if (currentLine[x] == "mesaCenterLeft")
                    {
                        i = tilePicker_MesaCenterLeft.Image;
                    }
                    else if (currentLine[x] == "mesaCenterRight")
                    {
                        i = tilePicker_MesaCenterRight.Image;
                    }
                    else if (currentLine[x] == "mesaTopCenter")
                    {
                        i = tilePicker_MesaTopCenter.Image;
                    }
                    else if (currentLine[x] == "mesaTopLeft")
                    {
                        i = tilePicker_MesaTopLeft.Image;
                    }
                    else if (currentLine[x] == "mesaTopRight")
                    {
                        i = tilePicker_MesaTopRight.Image;
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

            groupBox_MapView.ResumeLayout();
        }

        private void comboBox_TilePickerCatagories_SelectedIndexChanged(object sender, EventArgs e)
        {
            // index 0 should be the misc tiles
            if (comboBox_TilePickerCatagories.SelectedIndex == 0)
            {
                // show all the tile pickers in the list 
                foreach (PictureBox tilePicker in _miscTiles)
                {
                    tilePicker.Show();
                }

                // hide all other tile pickers
                foreach (PictureBox tilePicker in _entities)
                {
                    tilePicker.Hide();
                }
                foreach (PictureBox tilePicker in _grass)
                {
                    tilePicker.Hide();
                }
                foreach (PictureBox tilePicker in _cave)
                {
                    tilePicker.Hide();
                }
                foreach (PictureBox tilePicker in _mesa)
                {
                    tilePicker.Hide();
                }
            }

            // index 1 should be entities
            if (comboBox_TilePickerCatagories.SelectedIndex == 1)
            {
                // show all the tile pickers in the list 
                foreach (PictureBox tilePicker in _entities)
                {
                    tilePicker.Show();
                }

                // hide all other tile pickers
                foreach (PictureBox tilePicker in _miscTiles)
                {
                    tilePicker.Hide();
                }
                foreach (PictureBox tilePicker in _grass)
                {
                    tilePicker.Hide();
                }
                foreach (PictureBox tilePicker in _cave)
                {
                    tilePicker.Hide();
                }
                foreach (PictureBox tilePicker in _mesa)
                {
                    tilePicker.Hide();
                }
            }

            // index 2 should be grass
            if (comboBox_TilePickerCatagories.SelectedIndex == 2)
            {
                // show all the tile pickers in the list 
                foreach (PictureBox tilePicker in _grass)
                {
                    tilePicker.Show();
                }

                // hide all other tile pickers
                foreach (PictureBox tilePicker in _miscTiles)
                {
                    tilePicker.Hide();
                }
                foreach (PictureBox tilePicker in _entities)
                {
                    tilePicker.Hide();
                }
                foreach (PictureBox tilePicker in _cave)
                {
                    tilePicker.Hide();
                }
                foreach (PictureBox tilePicker in _mesa)
                {
                    tilePicker.Hide();
                }
            }

            // index 3 should be cave
            if (comboBox_TilePickerCatagories.SelectedIndex == 3)
            {
                // show all the tile pickers in the list 
                foreach (PictureBox tilePicker in _cave)
                {
                    tilePicker.Show();
                }

                // hide all other tile pickers
                foreach (PictureBox tilePicker in _miscTiles)
                {
                    tilePicker.Hide();
                }
                foreach (PictureBox tilePicker in _entities)
                {
                    tilePicker.Hide();
                }
                foreach (PictureBox tilePicker in _grass)
                {
                    tilePicker.Hide();
                }
                foreach (PictureBox tilePicker in _mesa)
                {
                    tilePicker.Hide();
                }
            }

            // index 4 should be mesa
            if (comboBox_TilePickerCatagories.SelectedIndex == 4)
            {
                // show all the tile pickers in the list 
                foreach (PictureBox tilePicker in _mesa)
                {
                    tilePicker.Show();
                }

                // hide all other tile pickers
                foreach (PictureBox tilePicker in _miscTiles)
                {
                    tilePicker.Hide();
                }
                foreach (PictureBox tilePicker in _entities)
                {
                    tilePicker.Hide();
                }
                foreach (PictureBox tilePicker in _grass)
                {
                    tilePicker.Hide();
                }
                foreach (PictureBox tilePicker in _cave)
                {
                    tilePicker.Hide();
                }
            }
        }

        private void LevelEditorForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            // sense when the user presses the r key
            if(e.KeyChar == 'r')
            {
                // if grass tiles are selected, allow the user to scroll through them forward
                if(comboBox_TilePickerCatagories.SelectedIndex == 2)
                {
                    for(int i = 0; i < _grass.Count; i++)
                    {
                        // find the tile currently selected
                        if (_grass[i].Image == pictureBox_CurrentTile.Image)
                        {
                            // if the tile was one of the grass tiles, go to the next one
                            pictureBox_CurrentTile.Image = _grass[(i + 1) % 9].Image;
                            break;
                        }
                    }
                }

                // if cave tiles are selected, allow the user to scroll through them forward
                if (comboBox_TilePickerCatagories.SelectedIndex == 3)
                {
                    for (int i = 0; i < _cave.Count; i++)
                    {
                        // find the tile currently selected
                        if (_cave[i].Image == pictureBox_CurrentTile.Image)
                        {
                            // if the tile was one of the grass tiles, go to the next one
                            pictureBox_CurrentTile.Image = _cave[(i + 1) % 9].Image;
                            break;
                        }
                    }
                }

                // if mesa tiles are selected, allow the user to scroll through them forward
                if (comboBox_TilePickerCatagories.SelectedIndex == 4)
                {
                    for (int i = 0; i < _mesa.Count; i++)
                    {
                        // find the tile currently selected
                        if (_mesa[i].Image == pictureBox_CurrentTile.Image)
                        {
                            // if the tile was one of the grass tiles, go to the next one
                            pictureBox_CurrentTile.Image = _mesa[(i + 1) % 9].Image;
                            break;
                        }
                    }
                }
            }
            // sense when the user presses the r key
            if (e.KeyChar == 'q')
            {
                // if grass tiles are selected, allow the user to scroll through them forward
                if (comboBox_TilePickerCatagories.SelectedIndex == 2)
                {
                    for (int i = 0; i < _grass.Count; i++)
                    {
                        // find the tile currently selected
                        if (_grass[i].Image == pictureBox_CurrentTile.Image)
                        {
                            // if the tile was one of the grass tiles, go to the next one
                            pictureBox_CurrentTile.Image = _grass[(i - 1) % 9].Image;
                            break;
                        }
                    }
                }

                // if cave tiles are selected, allow the user to scroll through them forward
                if (comboBox_TilePickerCatagories.SelectedIndex == 3)
                {
                    for (int i = 0; i < _cave.Count; i++)
                    {
                        // find the tile currently selected
                        if (_cave[i].Image == pictureBox_CurrentTile.Image)
                        {
                            // if the tile was one of the grass tiles, go to the next one
                            pictureBox_CurrentTile.Image = _cave[(i - 1) % 9].Image;
                            break;
                        }
                    }
                }

                // if mesa tiles are selected, allow the user to scroll through them forward
                if (comboBox_TilePickerCatagories.SelectedIndex == 4)
                {
                    for (int i = 0; i < _mesa.Count; i++)
                    {
                        // find the tile currently selected
                        if (_mesa[i].Image == pictureBox_CurrentTile.Image)
                        {
                            // if the tile was one of the grass tiles, go to the next one
                            pictureBox_CurrentTile.Image = _mesa[(i - 1) % 9].Image;
                            break;
                        }
                    }
                }
            }
        }
    }
}
