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
        private SpriteFont arial12;

        private Level testLevel;
        private Player player;

        

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

            // Load fonts
            arial12 = this.Content.Load<SpriteFont>("Arial12");

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
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            kb = Keyboard.GetState();
            ms = Mouse.GetState();

            // update the player
            player.ResolveCollisions(testLevel.TileMap);
            player.Update();

            prevKb = kb;
            prevMs = ms;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            MouseState ms = Mouse.GetState();

            _spriteBatch.Begin();

            testLevel.Draw(_spriteBatch);

            player.Draw(_spriteBatch);

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

            

            _spriteBatch.End();

            base.Draw(gameTime);
        }

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