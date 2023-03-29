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
        public static KeyboardState kb;
        public static KeyboardState prevKb;

        public static MouseState ms;
        public static MouseState prevMs;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D testTileSprite;
        private Texture2D playerSprite;
        private Texture2D blankRectangleSprite;
        private SpriteFont arial12;
        private SpriteFont arial36;

        private Level testLevel;
        private Player player;

        GameState gameState = GameState.MainMenu; // enum for managing gamestate (this is what starts the game on the menu screen)
        private bool debugOn = false; // boolean to toggle debug mode
        private bool godModeOn = false; // boolean to toggle god mode (doesn't do anything YET!)

        // Pause & Start menu objects
        private Rectangle pauseButtonDebug;
        private Rectangle pauseButtonGodMode;
        private Rectangle pauseButtonQuit;
        private Rectangle levelButtonPlay; // only one "level" right now so only one button

        public GameManager()
        {
            _graphics = new GraphicsDeviceManager(this);
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
            testTileSprite = this.Content.Load<Texture2D>("TestTile");
            playerSprite = this.Content.Load<Texture2D>("PlayerTestSprite");
            blankRectangleSprite = this.Content.Load<Texture2D>("blankRectangle");

            // Load fonts
            arial12 = this.Content.Load<SpriteFont>("Arial12");
            arial36 = this.Content.Load<SpriteFont>("Arial36");

            // set the game to full screen and the resolution to match the 16-bit artstyle
            // this resolution may not be the final one we choose
            _graphics.PreferredBackBufferWidth = testTileSprite.Width * 16;
            _graphics.PreferredBackBufferHeight = (int)(_graphics.PreferredBackBufferWidth * (0.5));
            //_graphics.IsFullScreen= true;
            _graphics.ApplyChanges();

            // create the test level
            testLevel = new Level(GenerateTestLevel(),
                new Vector2(testTileSprite.Width,
                _graphics.PreferredBackBufferHeight - testTileSprite.Height * 5));

            // set up the player
            player = new Player(playerSprite, testLevel.PlayerStart, 100);

            // A bunch of rectangles for the pause menu (163x100 draws these rectangles at a quarter size of the original file)
            pauseButtonDebug = new Rectangle(230, 300, 163, 100);
            pauseButtonGodMode = new Rectangle(430, 300, 163, 100);
            pauseButtonQuit = new Rectangle(630, 300, 163, 100);
            levelButtonPlay = new Rectangle(430, 225, 163, 100);

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

                    // "Press any button to continue"
                    if  (kb != prevKb && kb.IsKeyUp(Keys.Escape))
                    { gameState = GameState.LevelSelect; }

                    // Close the game by pressing escape
                    if (kb.IsKeyDown(Keys.Escape) && prevKb.IsKeyUp(Keys.Escape))
                    { Exit(); }

                    break;

                // We are on the LEVEL SELECT.
                // We can move back to the MAIN MENU, or move to GAMEPLAY.
                case GameState.LevelSelect:

                    // This isn't working yet
                    /*
                    // Return to main menu screen
                    if (kb.IsKeyDown(Keys.Escape) && prevKb.IsKeyUp(Keys.Escape))
                    { gameState = GameState.MainMenu; }
                    */

                    // Play the demo level
                    if (levelButtonPlay.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && prevMs.LeftButton != ButtonState.Pressed)
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

                    if (pauseButtonGodMode.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && prevMs.LeftButton != ButtonState.Pressed)
                    { godModeOn = !godModeOn; } // Enables god mode

                    if (pauseButtonQuit.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && prevMs.LeftButton != ButtonState.Pressed)
                    { gameState = GameState.MainMenu; } // Quitting to main menu

                    break;

                // We are in GAMEPLAY.
                // We can PAUSE.
                case GameState.Gameplay:

                    // Update the player
                    player.ResolveCollisions(testLevel.TileMap);
                    player.Update();

                    // Change to pause state if escape key pressed
                    if (kb.IsKeyDown(Keys.Escape) && prevKb.IsKeyUp(Keys.Escape))
                    { gameState = GameState.PauseMenu; }

                    break;
            }


            prevKb = kb;
            prevMs = ms;

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

            // Drawing differently depending on the gamestates
            switch (gameState)
            {
                // Drawing for main menu
                case GameState.MainMenu:

                    // (Placeholders) draws the logo and start text. Will probably use a custom logo and different font in the future
                    _spriteBatch.DrawString(arial36, "SHOTGUNBOOMERANG", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 280,
                        (_graphics.PreferredBackBufferHeight / 2) - 100), Color.White);

                    _spriteBatch.DrawString(arial12, "Press any button to start", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 100,
                        (_graphics.PreferredBackBufferHeight / 2) - 30), Color.White);

                    break;

                // Drawing for level select (will have more/different options in the future)
                case GameState.LevelSelect:

                    // Right now, we only have one "level."
                    _spriteBatch.Draw(blankRectangleSprite, levelButtonPlay, Color.White);
                    _spriteBatch.DrawString(arial12, "Play Demo", new Vector2(470, 265), Color.Black);

                    break;

                // Drawing for pause menu
                case GameState.PauseMenu:

                    // Pause & return text
                    _spriteBatch.DrawString(arial36, "PAUSED", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 100,
                        (_graphics.PreferredBackBufferHeight / 2) - 100), Color.White);

                    _spriteBatch.DrawString(arial12, "Press ESC to return to game", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 104,
                         (_graphics.PreferredBackBufferHeight / 2) - 40), Color.White);

                    // "Buttons"
                    _spriteBatch.Draw(blankRectangleSprite, pauseButtonDebug, Color.White);
                    _spriteBatch.Draw(blankRectangleSprite, pauseButtonGodMode, Color.White);
                    _spriteBatch.Draw(blankRectangleSprite, pauseButtonQuit, Color.White);

                    _spriteBatch.DrawString(arial12, "Debug: " + debugOn, new Vector2(265, 340), Color.Black);
                    _spriteBatch.DrawString(arial12, "God Mode: " + godModeOn, new Vector2(453, 340), Color.Black);
                    _spriteBatch.DrawString(arial12, "Quit to Menu", new Vector2(665, 340), Color.Black);

                    break;

                // Drawing for gameplay
                case GameState.Gameplay:

                    testLevel.Draw(_spriteBatch);
                    player.Draw(_spriteBatch);

                    // If debug enabled, print position & speed stats on screen
                    if (debugOn)
                    {
                        // print the window's X and Y
                        _spriteBatch.DrawString(arial12, $"Window width: {_graphics.PreferredBackBufferWidth}", new Vector2(10, 10), Color.White);
                        _spriteBatch.DrawString(arial12, $"Window height: {_graphics.PreferredBackBufferHeight}", new Vector2(10, 30), Color.White);

                        // print the mouse's X and Y
                        _spriteBatch.DrawString(arial12, $"Mouse Coordinates: {ms.X}, {ms.Y}", new Vector2(10, 50), Color.White);

                        // print the player's X and Y
                        _spriteBatch.DrawString(arial12, $"Player Coordinates: {player.Position.X}, {player.Position.Y}", new Vector2(10, 70), Color.White);

                        // print the player's Velocity
                        _spriteBatch.DrawString(arial12, $"Player Velocity: {player.Velocity.X}, {player.Velocity.Y}", new Vector2(10, 90), Color.White);

                        // print the player's state
                        _spriteBatch.DrawString(arial12, $"Player state: {player.CurrentState}", new Vector2(10, 110), Color.White);
                    }

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
                    _graphics.PreferredBackBufferHeight - testTileSprite.Height)));
            }

            return tileMap;
        }
    }
}