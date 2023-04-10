using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShotgunBoomerang
{
    // Enumerator for major program states
    enum GameState
    {
        MainMenu,
        LevelSelect,
        PauseMenu,
        Gameplay
    }

    public class GameManager : Game
    {
        // constant gravity (needs to be accessable to all classes)
        public static float Gravity = 1f;

        // many classes will need access to the mouse and keyboard
        private KeyboardState kb;
        private KeyboardState prevKb;

        private MouseState ms;
        private MouseState prevMs;

        // many classes will have to keep track of the currently loaded tiles
        //private List<Tile> currentTileMap;
        //private List<IGameEnemy> currentEnemies;
        //private List<IGameProjectile> currentProjectiles;

        private Vector2 screenOffset;

        private static GraphicsDeviceManager graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D menuBackground;
        private Texture2D testTileSprite;
        private Texture2D playerSprite;
        private Texture2D blankRectangleSprite;
        private Texture2D boomerangSprite;
        private SpriteFont arial12;
        private SpriteFont arial36;

        private Level testLevel;
        private Player player;

        GameState gameState = GameState.MainMenu; // enum for managing gamestate (this is what starts the game on the menu screen)
        private bool debugOn = false; // boolean to toggle debug mode

        // Pause & Start menu objects
        private string pauseText;
        private Rectangle pauseButtonDebug;
        private Rectangle pauseButtonQuit;

        private string levelText;
        private Rectangle buttonPlayDemo;
        private Rectangle buttonPlayOne;
        private Rectangle buttonPlayTwo;
        private Rectangle buttonPlayThree;

        public GameManager()
        {
            // Sets width and height to match hardware fullscreen
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.ApplyChanges();
            //Window.AllowUserResizing = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // give the keyboard and mouse states initial values
            kb = Keyboard.GetState();
            prevKb = Keyboard.GetState();

            ms = Mouse.GetState();
            prevMs = Mouse.GetState();

            base.Initialize();
        }

        protected override void LoadContent()
        {

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load textures
            menuBackground = this.Content.Load<Texture2D>("pixeldesertback");
            testTileSprite = this.Content.Load<Texture2D>("TestTile");
            playerSprite = this.Content.Load<Texture2D>("PlayerTestSprite");
            blankRectangleSprite = this.Content.Load<Texture2D>("blankRectangle");
            boomerangSprite = this.Content.Load<Texture2D>("Boomerang");

            // Load fonts
            arial12 = this.Content.Load<SpriteFont>("Arial12");
            arial36 = this.Content.Load<SpriteFont>("Arial36");

            // create the test level
            testLevel = new Level(
                GenerateTestLevel(),
                new List<IGameEnemy>(),
                new List<IGameProjectile>(),
                new Vector2(testTileSprite.Width, 
                -testTileSprite.Width * 3));

            // set up the player
            player = new Player(playerSprite, boomerangSprite, testLevel.PlayerStart, 100);

            // A bunch of rectangles for the pause menu (163x100 draws these rectangles at a quarter size of the original file)
            pauseButtonDebug = new Rectangle(230, 300, 163, 100);
            pauseButtonQuit = new Rectangle(630, 300, 163, 100);

            buttonPlayDemo = new Rectangle(430, 300, 163, 50);
            buttonPlayOne = new Rectangle(430, 400, 163, 100);
            buttonPlayTwo = new Rectangle(430, 550, 163, 100);
            buttonPlayThree = new Rectangle(430, 700, 163, 100);
        }

        /// <summary>
        /// Update!! This is where the game states are managed!!
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            // Getting mouse and keyboard states
            kb = Keyboard.GetState();
            ms = Mouse.GetState();

            // Game State Manager!!!! Previous mouse/keyboard states accounted for!
            switch (gameState)
            {
                // We are on the MAIN MENU.
                // We can move to the LEVEL SELECT, or close the game.
                case GameState.MainMenu:

                    // "Press any button to continue" (or user can click)
                    if  ((kb != prevKb && kb.IsKeyUp(Keys.Escape) && prevKb.IsKeyUp(Keys.Escape))
                        || (ms.LeftButton == ButtonState.Pressed && prevMs.LeftButton != ButtonState.Pressed))
                    { gameState = GameState.LevelSelect; }

                    // Close the game by pressing escape
                    if (kb.IsKeyDown(Keys.Escape) && prevKb.IsKeyUp(Keys.Escape))
                    { Exit(); }

                    break;

                // We are on the LEVEL SELECT.
                // We can move back to the MAIN MENU, or move to GAMEPLAY.
                case GameState.LevelSelect:

                    // Return to main menu screen
                    if (kb.IsKeyDown(Keys.Escape) && prevKb.IsKeyUp(Keys.Escape))
                    { gameState = GameState.MainMenu; }

                    // Play the demo level
                    if (buttonPlayDemo.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && prevMs.LeftButton != ButtonState.Pressed)
                    { gameState = GameState.Gameplay; }

                    break;

                // We are on the PAUSE MENU.
                // We can return to GAMEPLAY, or quit to the MAIN MENU.
                case GameState.PauseMenu:

                    // Return to gameplay if escape key pressed
                    if (kb.IsKeyDown(Keys.Escape) && prevKb.IsKeyUp(Keys.Escape))
                    { gameState = GameState.Gameplay; }

                    // "Buttons" on the pause menu are clicked
                    if (pauseButtonDebug.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && prevMs.LeftButton != ButtonState.Pressed)
                    { debugOn = !debugOn; } // Enables debug

                    if (pauseButtonQuit.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && prevMs.LeftButton != ButtonState.Pressed)
                    {
                        testLevel.ResetLevel(player);
                        gameState = GameState.MainMenu; } // Quitting to main menu

                    break;

                // We are in GAMEPLAY.
                // We can PAUSE.
                case GameState.Gameplay:

                    // Update the player
                    player.Update(kb, prevKb, ms, prevMs, testLevel.CurrentTileMap, testLevel.CurrentEnemies, testLevel.CurrentProjectiles, graphics);

                    // Update elements of the level
                    testLevel.Update(kb, prevKb, ms, prevMs, player);

                    // Change to pause state if escape key pressed
                    if (kb.IsKeyDown(Keys.Escape) && prevKb.IsKeyUp(Keys.Escape))
                    { gameState = GameState.PauseMenu; }

                    //Resets the level to starting state
                    if (kb.IsKeyDown(Keys.R) && prevKb.IsKeyUp(Keys.R))
                    {
                        testLevel.ResetLevel(player);
                    }

                    break;
            }

            // update logic items
            prevKb = kb;
            prevMs = ms;

            screenOffset = player.Position -
                new Vector2(graphics.PreferredBackBufferWidth / 2 
                - player.Sprite.Width / 2,
                graphics.PreferredBackBufferHeight / 2 
                - player.Sprite.Height / 2);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw, where everything is drawn (wow)
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            MouseState ms = Mouse.GetState();

            _spriteBatch.Begin();

            // If debug enabled, print position & speed stats on screen
            if (debugOn)
            { DrawDebug(); }

            // Drawing differently depending on the gamestates
            switch (gameState)
            {
                // Drawing for main menu
                case GameState.MainMenu:

                    // Background
                    _spriteBatch.Draw(menuBackground, new Rectangle(0, 0,
                        graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

                    // (Placeholders) draws the logo and start text. Will probably use a custom logo and different font in the future
                    _spriteBatch.DrawString(arial36, "SHOTGUNBOOMERANG", new Vector2((graphics.PreferredBackBufferWidth / 2) - 280,
                        (graphics.PreferredBackBufferHeight / 2) - 100), Color.Black);

                    _spriteBatch.DrawString(arial12, "Press any button to start", new Vector2((graphics.PreferredBackBufferWidth / 2) - 100,
                        (graphics.PreferredBackBufferHeight / 2) - 30), Color.Black);

                    break;

                // Drawing for level select
                case GameState.LevelSelect:

                    // Background
                    _spriteBatch.Draw(menuBackground, new Rectangle(0, 0,
                        graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

                    // "Level Select" text
                    _spriteBatch.DrawString(arial36, "- SELECT LEVEL -", new Vector2((graphics.PreferredBackBufferWidth / 2) - 190,
                        (graphics.PreferredBackBufferHeight / 2) - 450), Color.Black);

                    // Level Buttons
                    DrawButton(_spriteBatch, ms, buttonPlayDemo, "Demo");
                    DrawButton(_spriteBatch, ms, buttonPlayOne, "Stage One");
                    DrawButton(_spriteBatch, ms, buttonPlayTwo, "Stage Two");
                    DrawButton(_spriteBatch, ms, buttonPlayThree, "Stage Three");

                    break;

                // Drawing for pause menu
                case GameState.PauseMenu:

                    testLevel.Draw(_spriteBatch, screenOffset);
                    player.Draw(_spriteBatch, graphics);

                    // Pause & return text
                    _spriteBatch.DrawString(arial36, "- PAUSED -", new Vector2((graphics.PreferredBackBufferWidth / 2) - 120,
                        (graphics.PreferredBackBufferHeight / 2) - 450), Color.Black);

                    // Buttons
                    DrawButton(_spriteBatch, ms, pauseButtonDebug, "Debug: " + debugOn);
                    DrawButton(_spriteBatch, ms, pauseButtonQuit, "Quit to Menu");

                    // Text at the bottom that changes depending on hover
                    if (pauseButtonDebug.Contains(ms.Position))
                    { pauseText = "Enable debug text (position, velocity, etc.)"; }
                    else if (pauseButtonQuit.Contains(ms.Position))
                    { pauseText = "Quit to the main menu";  }
                    else
                    { pauseText = "Press ESC to return to game"; }

                    _spriteBatch.DrawString(arial12, pauseText, new Vector2(graphics.PreferredBackBufferWidth / 2 -
                        arial12.MeasureString(pauseText).X /2, graphics.PreferredBackBufferHeight - 150), Color.Black);

                    break;

                // Drawing for gameplay
                case GameState.Gameplay:

                    testLevel.Draw(_spriteBatch, screenOffset);
                    player.Draw(_spriteBatch, graphics);

                    break;
            }

            // Important code! Leave at the end of the method
            _spriteBatch.End(); 
            base.Draw(gameTime);
        }

        /// <summary>
        /// Generates the demo level
        /// </summary>
        /// <returns></returns>
        private List<Tile> GenerateTestLevel()
        {
            List<Tile> tileMap = new List<Tile>();

            // create a 32-tile long floor
            for(int i = 0; i < 32; i++)
            {
                tileMap.Add(new Tile(testTileSprite, 
                    new Vector2(i * testTileSprite.Width, 
                    0)));
            }

            return tileMap;
        }

        /// <summary>
        /// Draws the debug text. Helper Method!
        /// </summary>
        public void DrawDebug()
        {
            // print the window's X and Y
            _spriteBatch.DrawString(arial12, $"Window width: {graphics.PreferredBackBufferWidth}", new Vector2(10, 10), Color.White);
            _spriteBatch.DrawString(arial12, $"Window height: {graphics.PreferredBackBufferHeight}", new Vector2(10, 30), Color.White);

            // print the mouse's X and Y
            _spriteBatch.DrawString(arial12, $"Mouse Coordinates: {ms.X}, {ms.Y}", new Vector2(10, 50), Color.White);

            // print the player's X and Y
            _spriteBatch.DrawString(arial12, $"Player Coordinates: {player.Position.X}, {player.Position.Y}", new Vector2(10, 70), Color.White);

            // print the player's Velocity
            _spriteBatch.DrawString(arial12, $"Player Velocity: {player.Velocity.X}, {player.Velocity.Y}", new Vector2(10, 90), Color.White);

            // print the player's state
            _spriteBatch.DrawString(arial12, $"Player state: {player.CurrentState}", new Vector2(10, 110), Color.White);

            /*
            // print the boomerang's X and Y
            _spriteBatch.DrawString(arial12, $"Boomerang Coordinates: {boomerang.Position.X}, {boomerang.Position.Y}", new Vector2(10, 130), Color.White);

            // print the boomerang's velocity
            _spriteBatch.DrawString(arial12, $"Boomerang Velocity: {boomerang.Velocity.X}, {boomerang.Velocity.Y}", new Vector2(10, 150), Color.White);

            // print the boomerang's state
            _spriteBatch.DrawString(arial12, $"Boomerang state: {boomerang.CurrentState}", new Vector2(10, 170), Color.White);
            */
        }

        /// <summary>
        /// Draws a button and its text. Helper Method!
        /// </summary>
        /// <param name="sb">Spritebatch</param>
        /// <param name="ms">Mouse state</param>
        /// <param name="rect">Rectangle</param>
        /// <param name="text">Text in the button</param>
        public void DrawButton(SpriteBatch sb, MouseState ms, Rectangle rect, string text)
        {
            // Button color changes with hover & on click!
            Color buttonColor = Color.White;
            if (ms.LeftButton == ButtonState.Pressed && rect.Contains(ms.Position))
            {
                buttonColor = Color.Gray;
            }
            else if (rect.Contains(ms.Position))
            {
                buttonColor = Color.DarkGray;
            }
            else
            {
                buttonColor = Color.White;
            }

            // Positions button & text within button (scales dependent on text size)
            _spriteBatch.Draw(blankRectangleSprite, rect, buttonColor);
            _spriteBatch.DrawString(arial12, text, new Vector2(rect.X + rect.Width / 2 - arial12.MeasureString(text).X / 2,
                rect.Y + rect.Height / 2 - arial12.MeasureString(text).Y / 2), Color.Black);
        }
    }
}