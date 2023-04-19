using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ShapeUtils;

namespace ShotgunBoomerang
{
    // Enumerator for major program states
    enum GameState
    {
        MainMenu,
        LevelSelect,
        PauseMenu,
        Gameplay,
        Dead,
        Victory
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

        private int skullSize = 1;
        private bool skullGrow = true;
        private Texture2D awesomeFlamingSkull;
        private Song deathSound;

        private Texture2D menuBackground;
        private Texture2D blankRectangleSprite;
        private Texture2D darkFilter;
        private Texture2D healthBar;
        private Texture2D ammoBar;
        private Texture2D demoDisplay;

        //private Texture2D testTileSprite;
        private Texture2D playerSprite;
        //private Texture2D snakeSprite;
        private Texture2D boomerangSprite;

        private List<Texture2D> demoLevelTexturepack;

        private SpriteFont arial12;
        private SpriteFont arial36;

        private Level currentLevel;

        private Level demoLevel;
        private Level levelOne;
        private Level levelTwo;
        private Level levelThree;
        private Player player;

        GameState gameState = GameState.MainMenu; // enum for managing gamestate (this is what starts the game on the menu screen)
        private bool debugOn = false; // boolean to toggle debug mode
        private bool infiniteHP = false;
        private bool infiniteAmmo = false;

        // Pause & Start & Death menu objects
        private string pauseText;
        private Rectangle pauseButtonDebug;
        private Rectangle pauseButtonQuit;
        private Rectangle pauseButtonHP;
        private Rectangle pauseButtonAmmo;
        private Rectangle pauseButtonReset;

        private string levelText;
        private Texture2D levelSprite;
        private Rectangle buttonPlayDemo;
        private Rectangle buttonPlayOne;
        private Rectangle buttonPlayTwo;
        private Rectangle buttonPlayThree;

        private Rectangle deadRespawnButton;
        private Rectangle deadQuitButton;

        private SnakeEnemy snek;

        public GameManager()
        {
            // Sets width and height to match hardware fullscreen
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.ApplyChanges();
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
            //testTileSprite = this.Content.Load<Texture2D>("TestTile");
            playerSprite = this.Content.Load<Texture2D>("PlayerTestSprite");
            blankRectangleSprite = this.Content.Load<Texture2D>("blankRectangle");
            boomerangSprite = this.Content.Load<Texture2D>("Boomerang");
            //snakeSprite = this.Content.Load<Texture2D>("Snek");
            darkFilter = this.Content.Load<Texture2D>("darkfilter");
            healthBar = this.Content.Load<Texture2D>("redsquare");
            ammoBar = this.Content.Load<Texture2D>("ammoui");
            demoDisplay = this.Content.Load<Texture2D>("demoDisplay");
            awesomeFlamingSkull = this.Content.Load<Texture2D>("awesomeflamingskull");

            // These are the textures the test level will need to display prperly
            demoLevelTexturepack = new List<Texture2D>()
            {
                this.Content.Load<Texture2D>("TestTile"),
                this.Content.Load<Texture2D>("Snek"),
                this.Content.Load<Texture2D>("endFlag")
            };

            deathSound = this.Content.Load<Song>("BadToTheBones");

            levelSprite = demoDisplay;

            // Load fonts
            arial12 = this.Content.Load<SpriteFont>("Arial12");
            arial36 = this.Content.Load<SpriteFont>("Arial36");

            // create the test level
            /*
            testLevel = new Level(
                GenerateTestLevel(),
                new List<IGameEnemy>(),
                new List<IGameProjectile>(),
                new Vector2(testTileSprite.Width, 
                -testTileSprite.Width * 3));
            */

            demoLevel = new Level(demoLevelTexturepack, "../../../../Levels/testLevel3.level");
            // set up the player
            player = new Player(playerSprite, boomerangSprite, demoLevel.PlayerStart, 100);

            //Test enemy
            /*
            snek = new SnakeEnemy(
                snakeSprite,
                new Vector2(
                    testLevel.PlayerStart.X + (testTileSprite.Width * 3),
                    testLevel.PlayerStart.Y + (testTileSprite.Height)),
                    100,
                    20,
                    2);
            testLevel.StartEnemies.Add(snek);
            testLevel.CurrentEnemies.Add(snek);
            testLevel.StartEnemies.Add(snek);
            */

            // A bunch of rectangles for the pause menu (163x100 draws these rectangles at a quarter size of the original file)
            pauseButtonDebug = new Rectangle(graphics.PreferredBackBufferWidth /2 - 264, graphics.PreferredBackBufferHeight / 2 - 110, 163, 100);
            pauseButtonHP = new Rectangle(graphics.PreferredBackBufferWidth / 2 - 81, graphics.PreferredBackBufferHeight / 2 - 110, 163, 100);
            pauseButtonAmmo = new Rectangle(graphics.PreferredBackBufferWidth / 2 + 102, graphics.PreferredBackBufferHeight / 2 - 110, 163, 100);

            pauseButtonReset = new Rectangle(graphics.PreferredBackBufferWidth / 2 - 173, graphics.PreferredBackBufferHeight / 2 + 10, 163, 100);
            pauseButtonQuit = new Rectangle(graphics.PreferredBackBufferWidth / 2 + 10, graphics.PreferredBackBufferHeight / 2 + 10, 163, 100);

            // Rectangles for level select screen
            buttonPlayDemo = new Rectangle(510, 300, 163, 50);
            buttonPlayOne = new Rectangle(510, 400, 163, 100);
            buttonPlayTwo = new Rectangle(510, 550, 163, 100);
            buttonPlayThree = new Rectangle(510, 700, 163, 100);

            // Rectangles for dead screen
            deadRespawnButton = new Rectangle(graphics.PreferredBackBufferWidth / 2 - 173, graphics.PreferredBackBufferHeight / 2 - 50, 163, 100);
            deadQuitButton = new Rectangle(graphics.PreferredBackBufferWidth / 2 + 10, graphics.PreferredBackBufferHeight / 2 - 50, 163, 100);

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

                    // Turning off debug outside of the levels
                    debugOn = false;

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

                    // Turning off debug outside of the level
                    debugOn = false;

                    // Return to main menu screen
                    if (kb.IsKeyDown(Keys.Escape) && prevKb.IsKeyUp(Keys.Escape))
                    { gameState = GameState.MainMenu; }

                    // Play the demo level
                    if (buttonPlayDemo.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && prevMs.LeftButton != ButtonState.Pressed)
                    {
                        currentLevel = demoLevel;
                        gameState = GameState.Gameplay;
                    }

                    // Play level one
                    if (buttonPlayOne.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && prevMs.LeftButton != ButtonState.Pressed)
                    {
                        currentLevel = levelOne;
                        gameState = GameState.Gameplay;
                    }

                    // Play level two
                    if (buttonPlayTwo.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && prevMs.LeftButton != ButtonState.Pressed)
                    {
                        currentLevel = levelTwo;
                        gameState = GameState.Gameplay;
                    }

                    // Play level three
                    if (buttonPlayThree.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && prevMs.LeftButton != ButtonState.Pressed)
                    {
                        currentLevel = levelThree;
                        gameState = GameState.Gameplay;
                    }

                    break;

                // We are on the PAUSE MENU.
                // We can return to GAMEPLAY, or quit to the MAIN MENU.
                case GameState.PauseMenu:

                    // Return to gameplay if escape key pressed
                    if (kb.IsKeyDown(Keys.Escape) && prevKb.IsKeyUp(Keys.Escape))
                    { gameState = GameState.Gameplay; }

                    // Debug button clicked
                    if (pauseButtonDebug.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && prevMs.LeftButton != ButtonState.Pressed)
                    { debugOn = !debugOn; } // Enables debug

                    // Inf HP button clicked
                    if (pauseButtonHP.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && prevMs.LeftButton != ButtonState.Pressed)
                    { infiniteHP = !infiniteHP; } // Enables infinite health

                    // Inf Ammo button clicked
                    if (pauseButtonAmmo.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && prevMs.LeftButton != ButtonState.Pressed)
                    { infiniteAmmo = !infiniteAmmo; } // Enables infinite ammo

                    // Reset button clicked
                    if (pauseButtonReset.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && prevMs.LeftButton != ButtonState.Pressed)
                    {
                        currentLevel.ResetLevel(player);
                        gameState = GameState.Gameplay;
                    }

                    // Quit button clicked
                    if (pauseButtonQuit.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && prevMs.LeftButton != ButtonState.Pressed)
                    {
                        currentLevel.ResetLevel(player);
                        gameState = GameState.MainMenu;
                    }

                    break;

                // We are in GAMEPLAY.
                // We can PAUSE, DIE when health reaches zero, or WIN when touching the flagpole.
                case GameState.Gameplay:

                    // Update the player
                    player.Update(kb, prevKb, ms, prevMs, currentLevel.CurrentTileMap, currentLevel.CurrentEnemies, currentLevel.CurrentProjectiles, graphics, gameTime);

                    // Updating player health and ammo if godmode options are enabled
                    if (infiniteHP && player.Health != 100)
                    { player.Health = 100; }
                    if (infiniteAmmo && player.Ammo != 2)
                    { player.Ammo = 2; }

                    // Dying when health reaches zero
                    if (player.Health <= 0)
                    { gameState = GameState.Dead; }
                    
                    // Winning if flagpole is touched
                    if (currentLevel.LevelEnd.IncidentWithPlayer)
                    { gameState = GameState.Victory; }

                    //Update the test snake
                    
                    //snek.Update(kb, prevKb, testLevel.CurrentTileMap, testLevel.CurrentProjectiles, player);

                    // Update elements of the level
                    currentLevel.Update(kb, prevKb, ms, prevMs, player, gameTime);

                    // Change to pause state if escape key pressed
                    if (kb.IsKeyDown(Keys.Escape) && prevKb.IsKeyUp(Keys.Escape))
                    { gameState = GameState.PauseMenu; }

                    // Pressing R is a quick restart
                    if (kb.IsKeyDown(Keys.R) && prevKb.IsKeyUp(Keys.R))
                    {
                        currentLevel.ResetLevel(player);
                    }

                    //this is likely test code. waiting for confirmation before delete
                    /*
                    if(ms.LeftButton == ButtonState.Pressed && prevMs.LeftButton != ButtonState.Pressed)
                    {
                        snek.TakeDamage(25, player);
                    }
                    */
                    
                    break;

                // You are DEAD. This state occurs from GAMEPLAY.
                // You can RESPAWN, or QUIT. BOTH reset the level.
                case GameState.Dead:

                    if(MediaPlayer.State != MediaState.Playing)
                    {
                        MediaPlayer.Play(deathSound);
                    }
                    // Quit button clicked
                    if (deadRespawnButton.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && prevMs.LeftButton != ButtonState.Pressed)
                    {
                        currentLevel.ResetLevel(player);
                        gameState = GameState.Gameplay;
                        MediaPlayer.Stop();
                    }

                    // Quit button clicked
                    if (deadQuitButton.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && prevMs.LeftButton != ButtonState.Pressed)
                    {
                        currentLevel.ResetLevel(player);
                        gameState = GameState.MainMenu;
                        MediaPlayer.Stop();
                    }

                    // Skeleton logic
                    if (skullSize <= 1)
                    { skullGrow = true; }
                    if (skullSize >= 360)
                    { skullGrow = false; }
                    if (skullGrow)
                    { skullSize += 5; }
                    else
                    { skullSize -= 5;}

                    break;

                // The level has ENDED.
                // We can RETURN TO LEVEL SELECT ONLY.
                case GameState.Victory:

                    // Pressing Enter returns to menu
                    if (kb.IsKeyDown(Keys.Enter) && prevKb.IsKeyUp(Keys.Enter))
                    {
                        currentLevel.ResetLevel(player);
                        gameState = GameState.LevelSelect;
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

            // Drawing differently depending on the gamestates
            switch (gameState)
            {
                // Drawing for main menu
                case GameState.MainMenu:

                    // Background
                    _spriteBatch.Draw(menuBackground, new Rectangle(0, 0,
                        graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

                    // (Placeholders) draws the logo and start text. Will probably use a custom logo and different font in the future
                    _spriteBatch.DrawString(arial36, "SHOTGUNBOOMERANG", new Vector2((graphics.PreferredBackBufferWidth / 2) -
                        arial36.MeasureString("SHOTGUNBOOMERANG").X / 2, (graphics.PreferredBackBufferHeight / 2) - 100), Color.Black);

                    _spriteBatch.DrawString(arial12, "Press any button to start", new Vector2((graphics.PreferredBackBufferWidth / 2)
                        - arial12.MeasureString("Press any button to start").X / 2, (graphics.PreferredBackBufferHeight / 2) - 30), Color.Black);

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
                    DrawButton(ms, buttonPlayDemo, "Demo");
                    DrawButton(ms, buttonPlayOne, "Stage One");
                    DrawButton(ms, buttonPlayTwo, "Stage Two");
                    DrawButton(ms, buttonPlayThree, "Stage Three");

                    // Changes text and sprite depending on hover
                    if (buttonPlayDemo.Contains(ms.Position))
                    {
                        levelText = "Enter the testing stage.";
                        levelSprite = demoDisplay;
                    }
                    else if (buttonPlayOne.Contains(ms.Position))
                    {
                        levelText = "level one desc goes here, image will change upon hover";
                        //levelSprite = oneDisplay;
                    }
                    else if (buttonPlayTwo.Contains(ms.Position))
                    {
                        levelText = "level two desc goes here, image will change upon hover";
                        //levelSprite = twoDisplay;
                    }
                    else if (buttonPlayThree.Contains(ms.Position))
                    {
                        levelText = "level three desc goes here, image will change upon hover";
                        //levelSprite = threeDisplay;
                    }
                    else // Note that level sprite will remain the same as whatever was last hovered over
                    {
                        levelText = "Press ESC to quit to title screen";
                    }

                    // Drawing preview box then writing text
                    _spriteBatch.Draw(blankRectangleSprite, new Rectangle(780, 300, 615, 500), Color.White);
                    _spriteBatch.Draw(levelSprite, new Rectangle(783, 303, 609, 494), Color.White);
                    _spriteBatch.DrawString(arial12, levelText, new Vector2(1087 - arial12.MeasureString(levelText).X / 2,
                        790 - arial12.MeasureString(levelText).Y), Color.Black);

                    break;

                // Drawing for pause menu
                case GameState.PauseMenu:

                    currentLevel.Draw(_spriteBatch, screenOffset);
                    player.Draw(_spriteBatch, graphics);
                    DrawHPAmmo();
                    _spriteBatch.Draw(darkFilter, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

                    // Pause & return text
                    _spriteBatch.DrawString(arial36, "- PAUSED -", new Vector2((graphics.PreferredBackBufferWidth / 2)
                        - arial36.MeasureString("- PAUSED -").X / 2, (graphics.PreferredBackBufferHeight / 2) - 450), Color.Black);

                    // Buttons
                    DrawButton(ms, pauseButtonDebug, "Debug: " + debugOn);
                    DrawButton(ms, pauseButtonHP, "Inf. Health : " + infiniteHP);
                    DrawButton(ms, pauseButtonAmmo, "Inf. Ammo : " + infiniteAmmo);
                    DrawButton(ms, pauseButtonReset, "Reset Stage");
                    DrawButton(ms, pauseButtonQuit, "Quit to Menu");

                    // Text at the bottom that changes depending on hover
                    if (pauseButtonDebug.Contains(ms.Position))
                    { pauseText = "Enable debug mode (displays position, velocity, hitboxes, etc.)"; }
                    else if (pauseButtonAmmo.Contains(ms.Position))
                    { pauseText = "Enable infinite health"; }
                    else if (pauseButtonHP.Contains(ms.Position))
                    { pauseText = "Enable infinite ammo"; }
                    else if (pauseButtonReset.Contains(ms.Position))
                    { pauseText = "Reset and restart the current stage (you can also do this by pressing R during gameplay)"; }
                    else if (pauseButtonQuit.Contains(ms.Position))
                    { pauseText = "Quit to the main menu"; }
                    else
                    { pauseText = "Press ESC to return to game"; }

                    _spriteBatch.DrawString(arial12, pauseText, new Vector2(graphics.PreferredBackBufferWidth / 2 -
                        arial12.MeasureString(pauseText).X / 2, graphics.PreferredBackBufferHeight - 150), Color.Black);

                    break;

                // Drawing for gameplay
                case GameState.Gameplay:

                    currentLevel.Draw(_spriteBatch, screenOffset);
                    player.Draw(_spriteBatch, graphics);
                    DrawHPAmmo();

                    break;

                // Drawing for death screen
                case GameState.Dead:

                    currentLevel.Draw(_spriteBatch, screenOffset);
                    player.Draw(_spriteBatch, graphics);
                    DrawHPAmmo();
                    _spriteBatch.Draw(darkFilter, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

                    _spriteBatch.DrawString(arial36, "YOU DIED!", new Vector2(graphics.PreferredBackBufferWidth / 2 - arial36.MeasureString("YOU DIED!").X / 2,
                        (graphics.PreferredBackBufferHeight / 2) - 450), Color.Black);

                    DrawButton(ms, deadRespawnButton, "Respawn");
                    DrawButton(ms, deadQuitButton, "Quit to Menu");

                    _spriteBatch.Draw(awesomeFlamingSkull, new Rectangle(1400, 300, skullSize, skullSize), Color.White); // THE SKELETON APPEARS

                    break;

                // Drawing for victory screen
                case GameState.Victory:

                    currentLevel.Draw(_spriteBatch, screenOffset);
                    player.Draw(_spriteBatch, graphics);
                    DrawHPAmmo();
                    _spriteBatch.Draw(darkFilter, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

                    _spriteBatch.DrawString(arial36, "STAGE CLEAR", new Vector2(graphics.PreferredBackBufferWidth / 2 - arial36.MeasureString("STAGE CLEAR").X / 2,
                        (graphics.PreferredBackBufferHeight / 2) - 250), Color.Black);

                    _spriteBatch.DrawString(arial12, "Press Enter to return to menu", new Vector2(graphics.PreferredBackBufferWidth / 2 - arial12.MeasureString("Press Enter to return to menu").X / 2,
                         (graphics.PreferredBackBufferHeight / 2) - 200), Color.Black);

                    _spriteBatch.DrawString(arial12, "Score: " + (int)player.Score, new Vector2(graphics.PreferredBackBufferWidth / 2 - arial12.MeasureString("Score: " + (int)player.Score).X / 2,
                         (graphics.PreferredBackBufferHeight / 2) - 150), Color.Black);

                    _spriteBatch.DrawString(arial12, "Kills: " + player.Kills, new Vector2(graphics.PreferredBackBufferWidth / 2 - arial12.MeasureString("Kills: " + player.Kills).X / 2,
                         (graphics.PreferredBackBufferHeight / 2) - 130), Color.Black);

                    _spriteBatch.DrawString(arial12, "Time: " + (int)player.Timer + " seconds", new Vector2(graphics.PreferredBackBufferWidth / 2 - arial12.MeasureString("Time: " + (int)player.Timer + " seconds").X / 2,
                        (graphics.PreferredBackBufferHeight / 2) - 110), Color.Black);

                    break;
            }

            // If debug enabled, print position & speed stats on screen
            ShapeBatch.Begin(GraphicsDevice);
            if (debugOn)
            { DrawDebug(); }
            ShapeBatch.End();

            // Important code! Leave at the end of the method
            _spriteBatch.End(); 
            base.Draw(gameTime);
        }

        /*
        /// <summary>
        /// Generates the demo level
        /// </summary>
        /// <returns></returns>
        private List<Tile> GenerateTestLevel()
        {
            List<Tile> tileMap = new List<Tile>();


            //Adds a wall at the beginning
            tileMap.Add(new Tile(testTileSprite, new Vector2(0, -testTileSprite.Height)));


            // create a 32-tile long floor
            int i = 0;

            for(i = 0; i < 32; i++)
            {
                tileMap.Add(new Tile(testTileSprite, 
                    new Vector2(i * testTileSprite.Width, 
                    0)));
            }

            //Adds a wall at the end
            tileMap.Add(new Tile(testTileSprite, new Vector2((i - 1) * testTileSprite.Width, -testTileSprite.Height)));

            return tileMap;
        }
        */

        /// <summary>
        /// Draws the debug text and hitboxes. Helper Method!
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

            // print the player's state and direction
            _spriteBatch.DrawString(arial12, $"Player state: {player.CurrentState}", new Vector2(10, 110), Color.White);
            _spriteBatch.DrawString(arial12, $"Player direction: {player.CurrentDirection}", new Vector2(10, 130), Color.White);

            // print the player's health, ammo, and other stats
            _spriteBatch.DrawString(arial12, $"Player health: {player.Health}", new Vector2(10, 170), Color.White);
            _spriteBatch.DrawString(arial12, $"Player ammo: {player.Ammo}", new Vector2(10, 190), Color.White);
            _spriteBatch.DrawString(arial12, $"Kills: {player.Kills}", new Vector2(10, 210), Color.White);
            _spriteBatch.DrawString(arial12, $"Time: {(int)player.Timer}", new Vector2(10, 230), Color.White);
            _spriteBatch.DrawString(arial12, $"Score: {(int)player.Score}", new Vector2(10, 250), Color.White);

            // draw hitboxes

            // player hitbox
            ShapeBatch.BoxOutline(
                graphics.PreferredBackBufferWidth / 2 - player.Sprite.Width / 2,
                graphics.PreferredBackBufferHeight / 2 - player.Sprite.Height / 2,
                player.Sprite.Width, player.Sprite.Height, Color.White);

            // player shotgun radius
            ShapeBatch.CircleOutline(
                new Vector2(graphics.PreferredBackBufferWidth / 2, 
                graphics.PreferredBackBufferHeight / 2), 
                player.ShotgunRadius, Color.White);

            
            // player shotgun angles
            Vector2 screenCenter = new Vector2(
                graphics.PreferredBackBufferWidth / 2,
                    graphics.PreferredBackBufferHeight / 2);

            // normal of the vector between mouse and screen center
            Vector2 mouseCenterNormal = Vector2.Normalize(new Vector2(ms.Position.X, ms.Position.Y) - screenCenter);

            float angle = MathF.Atan2(-mouseCenterNormal.Y, mouseCenterNormal.X);
           
            

            //angle = -MathF.Atan(mouseCenterNormal.Y / mouseCenterNormal.X);

            try
            {
                // middle line
                ShapeBatch.Line(
                    screenCenter,
                    player.ShotgunRadius,
                    angle,
                    Color.White);
                
                // top line
                ShapeBatch.Line(
                    screenCenter,
                    player.ShotgunRadius,
                    angle + MathF.PI / 8,
                    Color.Red);

                // bottom line
                ShapeBatch.Line(
                    screenCenter,
                    player.ShotgunRadius,
                    angle - MathF.PI / 8,
                    Color.Red);

            }
            catch { }
            
            foreach(IGameEnemy enemy in currentLevel.CurrentEnemies)
            {
                MobileEntity currentEnemy = enemy as MobileEntity;

                ShapeBatch.BoxOutline(
                    currentEnemy.Position.X - screenOffset.X,
                    currentEnemy.Position.Y - screenOffset.Y,
                    currentEnemy.Sprite.Width,
                    currentEnemy.Sprite.Height,
                    Color.White);
            }


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
        public void DrawButton(MouseState ms, Rectangle rect, string text)
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

        /// <summary>
        /// Draws the player's HP and Ammo UI. Helper Method!
        /// </summary>
        public void DrawHPAmmo()
        {
            _spriteBatch.Draw(healthBar, new Rectangle(80, graphics.PreferredBackBufferHeight - 120, (int)(4 * player.Health), 30), Color.Red);
            for (int i = 1; i <= player.Ammo; i++)
            {
                _spriteBatch.Draw(ammoBar, new Rectangle(80 + 48 * (i - 1), graphics.PreferredBackBufferHeight - 200, 32, 64), Color.White);
            }
        }
    }
}