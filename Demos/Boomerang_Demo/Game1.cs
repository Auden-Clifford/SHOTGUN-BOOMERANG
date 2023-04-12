using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Boomerang_Demo
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState keyboard;
        private KeyboardState prvKb;
        private Rectangle player;
        private Texture2D playerTexture;
        private Vector2 playerPosition;
        private Rectangle boomerangRect;
        private Texture2D boomerangTexture;
        private Vector2 boomerangPosition;
        private Vector2 boomerangVelocity;
        private bool isThrown;
        private bool returning = false;
        private float boomerangRotation = 0;



        private MouseState mouse;
        private MouseState prvMouse;

        private SpriteFont arial12;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.ApplyChanges();
            // TODO: Add your initialization logic here
            playerPosition = new Vector2(500, 500);
            boomerangPosition = new Vector2(playerPosition.X, playerPosition.Y);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            playerTexture = Content.Load<Texture2D>("PlayerTestSprite");
            player = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, playerTexture.Width, playerTexture.Height);
            boomerangTexture = Content.Load<Texture2D>("boomerang");
            boomerangRect = new Rectangle((int)boomerangPosition.X, (int)boomerangPosition.Y, boomerangTexture.Width * 3, boomerangTexture.Height * 3);
            arial12 = Content.Load<SpriteFont>("File");
            
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            keyboard = Keyboard.GetState();
            mouse = Mouse.GetState();
            Movement();
            player.X = (int)playerPosition.X;
            player.Y = (int)playerPosition.Y;

            //Boomerang();

            if (!isThrown)
            {
                boomerangPosition.X = playerPosition.X;
                boomerangPosition.Y = playerPosition.Y;
            }

            ApplyMovement(CalculateDirection());
            ApplyDecel();
            Return();

            




            // TODO: Add your update logic here
            prvKb = Keyboard.GetState();
            prvMouse = Mouse.GetState();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(playerTexture, player, Color.White);
            
            //Only draw the boomerang if it's been thrown
            if (isThrown)
            {
                //_spriteBatch.Draw(boomerangTexture, boomerangRect, Color.White);
                _spriteBatch.Draw(
                    boomerangTexture,
                    boomerangRect,
                    null,
                    Color.White,
                    boomerangRotation,
                    new Vector2(boomerangTexture.Width / 2, boomerangTexture.Height / 2),
                    SpriteEffects.None,
                    0
                    );
            }

            //_spriteBatch.DrawString(arial12, "playerPos = " + player.X + ", " + player.Y, new Vector2(50, 50), Color.White);


            _spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Checks if a given key was pressed only once
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool SingleKeyPress(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key) && prvKb.IsKeyUp(key);
        }

        /// <summary>
        /// Checks if the mouse was clicked only once
        /// </summary>
        /// <param name="previous">The previous mouse state</param>
        /// <returns></returns>
        private bool SingleMouseClick(MouseState previous)
        {
            return (Mouse.GetState().LeftButton == ButtonState.Pressed) && (previous.LeftButton == ButtonState.Released);
        }

        /// <summary>
        /// Handles player movement
        /// </summary>
        /// <param name="position"></param>
        /// <param name="player"></param>
        private void Movement()
        {
            keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.W))
            {
                playerPosition.Y -= 5;
            }

            if(keyboard.IsKeyDown(Keys.A))
            {
                playerPosition.X -= 5;
            }

            if(keyboard.IsKeyDown(Keys.S))
            {
                playerPosition.Y += 5;
            }
            
            if(keyboard.IsKeyDown(Keys.D))
            {
                playerPosition.X += 5;
            }

        }

        /*
        private void Boomerang()
        {
            mouse = Mouse.GetState();
            Vector2 movement = Vector2.Zero;


            if (mouse.LeftButton == ButtonState.Pressed && prvMouse.LeftButton == ButtonState.Released)
            {
                Vector2 mousePos = new Vector2((int)mouse.X, (int)mouse.Y);

                Vector2 direction = mousePos - boomerangPosition;
                direction.Normalize();

                movement = Vector2.Multiply(direction, 3);
            }

            while(movement != Vector2.Zero)
            {
                boomerangPosition += movement;
                boomerangRect.X = (int)boomerangPosition.X;
                boomerangRect.Y = (int)boomerangPosition.Y;

                if(movement.X > 0)
                {
                    movement.X -= 0.1f;
                }else if(movement.X < 0)
                {
                    movement.X += 0.1f;
                }

                if(movement.X < 0.1f && movement.X > -0.1f)
                {
                    movement.X = 0;
                }

                if(movement.Y > 0)
                {
                    movement.Y -= 0.1f;
                }else if(movement.Y < 0)
                {
                    movement.Y += 0.1f;
                }

                if(movement.Y < 0.1f && movement.Y > -0.1f)
                {
                    movement.Y = 0;
                }
            }

            prvMouse = Mouse.GetState();
        }
        */


        /// <summary>
        /// Decellerates the boomerang once thrown.
        /// Does not decel the boomerang on it's return.
        /// </summary>
        public void ApplyDecel()
        {
            //If the boomerang isn't returning and it isn't stopped, lower the velocity it the 
            //value is positive and increases it if it's negative(the velocity begins to grow closer
            //to zero).
            if (!returning && boomerangVelocity.X != 0 || boomerangVelocity.Y != 0)
            {
                if (boomerangVelocity.X > 0)
                {
                    boomerangVelocity.X -= 0.1f;
                }
                else if (boomerangVelocity.X < 0)
                {
                    boomerangVelocity.X += 0.1f;
                }

                //If the boomerang's velocity is closer to zero than each increment's value
                //set the velocity to zero
                if (boomerangVelocity.X < 0.5f && boomerangVelocity.X > -0.5)
                {
                    boomerangVelocity.X = 0;
                }

                if (boomerangVelocity.Y > 0 )
                {
                    boomerangVelocity.Y -= 0.1f;
                }
                else if (boomerangVelocity.Y < 0)
                {
                    boomerangVelocity.Y += 0.1f;
                }

                //If the boomerang's velocity is closer to zero than each increment's value
                //set the velocity to zero
                if (boomerangVelocity.Y < 0.5f && boomerangVelocity.Y > -0.5f)
                {
                    boomerangVelocity.Y = 0;
                }
            }
        }

        /// <summary>
        /// Takes a vector and applies it to the boomerang as velocity
        /// </summary>
        /// <param name="directionalVector">The vector to apply as movement</param>
        public void ApplyMovement(Vector2 directionalVector)
        {
            boomerangRotation += 0.1f;
            boomerangVelocity += directionalVector;
            boomerangPosition += boomerangVelocity;
            boomerangRect.X = (int)boomerangPosition.X;
            boomerangRect.Y = (int)boomerangPosition.Y;
        }

        /// <summary>
        /// Calculates the directional vector for throwing the boomerang
        /// </summary>
        /// <returns>The vector to be applied to the boomerangs velocity</returns>
        public Vector2 CalculateDirection()
        {
            //If the mouse is clicked once and the boomerang hasn't already been thrown,
            //Calculate and return the movement vector for the throw.
            //Otherwise, this simply returns the zero vector
            if(SingleMouseClick(prvMouse) && !isThrown)
            {
                isThrown = true;
                Vector2 mousePos = new Vector2(prvMouse.X, prvMouse.Y);
                Vector2 finalVector = Vector2.Multiply(Vector2.Normalize(mousePos - playerPosition), 10);
                return finalVector;
            }
            return Vector2.Zero;
        }

        /// <summary>
        /// Returns the boomerang to the player once it has reached the end of the throw
        /// </summary>
        public void Return()
        {
            //If the boomerang was thrown and it's velocity now equals 0, begin returning it
            if(isThrown && boomerangVelocity.X == 0 && boomerangVelocity.Y == 0)
            {
                returning = true;

            }

            //If the boomerang has begun to return, set boomerangVelocity to the normalized directional
            //Vector between itself and the player, multiply by a number to increase power.
            if (returning)
            {

                //ApplyMovement(Vector2.Multiply(Vector2.Normalize(playerPosition - boomerangPosition), 10));
                boomerangVelocity = Vector2.Multiply(Vector2.Normalize(playerPosition - boomerangPosition), 10);
            }

            //If the boomerang is returning and intersects with the player, reset it
            if (boomerangRect.Intersects(player) && returning)
            {
                isThrown = false;
                returning = false;
                boomerangVelocity.X = 0;
                boomerangVelocity.Y = 0;

            }
        }
    }
}