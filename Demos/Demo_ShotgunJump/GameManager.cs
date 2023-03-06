using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace GravityAndCollisionsPE
{
	public class GameManager : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		private Texture2D playerTexture;
		private Texture2D obstacleTexture;

		private float playerSpeedX;
		private Vector2 playerVelocity;
		private Vector2 jumpVelocity;
		private Vector2 playerPosition;
		private Vector2 gravity;

		private List<Rectangle> obstacleRects;
		private KeyboardState prevKB;

		public GameManager()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}


		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// </summary>
		protected override void Initialize()
		{
			playerPosition = new Vector2(400, 100);
			playerVelocity = Vector2.Zero;
			jumpVelocity = new Vector2(0, -15.0f);
			gravity = new Vector2(0, 0.5f);

			playerSpeedX = 5.0f;

			obstacleRects = new List<Rectangle>();

			base.Initialize();
		}


		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			playerTexture = Content.Load<Texture2D>("mario");
			obstacleTexture = Content.Load<Texture2D>("pixel");

			// Manually load the obstacles
			LoadObstaclesFromFile("Content/obstacles.txt");
		}

		/// <summary>
		/// Loads obstacle rectangles from the given file
		/// </summary>
		/// <param name="file">Path to the file containing obstacles</param>
		private void LoadObstaclesFromFile(string file)
		{
			// Provide an error message in Visual Studio's output window
			// as a reminder to students
			if (!File.Exists(file))
			{
				System.Diagnostics.Debug.WriteLine($"Error: Cannot find file '{file}' in the output directory!");
				System.Diagnostics.Debug.WriteLine($"       Did you remember to add the file to the MGCB Editor AND set its Build Action to 'Copy'?");
				return;
			}

			// open the stream reader
			StreamReader reader = new StreamReader(File.OpenRead(file));

			string line = reader.ReadLine();

			while (line != null)
			{
                // if the line starts with "//" ignore it
                if (line.StartsWith("//")) { }
                // if the line is empty ignore it (trim to make sure it isn't full of spaces)
                else if (line.Trim() == "") { }
                // otherwise it should contain useful data
                else
                {
                    // create a new rectagle with the data on the line
                    string[] rectComponents = line.Split(',');
                    obstacleRects.Add(new Rectangle(
                        int.Parse(rectComponents[0]),
                        int.Parse(rectComponents[1]),
                        int.Parse(rectComponents[2]),
                        int.Parse(rectComponents[3])));
                }

				line = reader.ReadLine();
            }
			

			reader.Close();
		}


		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, etc.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			// Handle input, apply gravity and then deal with collisions
			ProcessInput();
			ApplyGravity();
			ResolveCollisions();

			// Save the old state at the end of the frame
			prevKB = Keyboard.GetState();
			base.Update(gameTime);
		}

		/// <summary>
		/// Handles movement for sidescrolling game with gravity
		/// </summary>
		private void ProcessInput()
		{
			KeyboardState kb = Keyboard.GetState();

			// check for left and right movement
			if(kb.IsKeyDown(Keys.A))
			{
				playerPosition.X -= playerSpeedX;
			}
			if(kb.IsKeyDown(Keys.D))
			{
				playerPosition.X += playerSpeedX;
			}

			// check for jumping
			if(kb.IsKeyDown(Keys.W) && prevKB.IsKeyUp(Keys.W))
			{
				playerVelocity.Y = jumpVelocity.Y;
			}

			prevKB = kb;
		}

		/// <summary>
		/// Applies gravity to the player
		/// </summary>
		private void ApplyGravity()
		{
			// Apply gravity to player's velocity
			playerVelocity += gravity;

			// Apply player's velocity to player's position
			playerPosition += playerVelocity;
        }

        /// <summary>
        /// Handles player collisions with obstacles
        /// </summary>
        private void ResolveCollisions()
		{
			// get the player's hitbox
			Rectangle playerHitBox = GetPlayerRect();

			// temporary list for all intersections
			List<Rectangle> intersectionsList = new List<Rectangle>();

			// loop through the list of all rectangles
			foreach(Rectangle rectangle in obstacleRects)
			{
				// if the player is intersecting
				if (rectangle.Intersects(playerHitBox))
				{
					intersectionsList.Add(rectangle);
				}
			}

			// loop through and resolve all horizontal intersections
			foreach(Rectangle rectangle in intersectionsList)
			{
				// create a new rectangle from the collision between the player and the rectangle
				Rectangle intersectRect = Rectangle.Intersect(playerHitBox, rectangle);

                //check for a horizontal collision (intersection is taller than it is wide)
                if (intersectRect.Width <= intersectRect.Height)
				{
					// if the player X is less than (further left than) the rectangle x
					// move the player left
                    if(playerPosition.X < intersectRect.X)
					{
						playerPosition.X -= intersectRect.Width;
					}
                    // otherwise move right
                    else
                    {
						playerPosition.X += intersectRect.Width;
					}

                }
            }

            // loop through and resolve all vertical intersections
            foreach (Rectangle rectangle in intersectionsList)
			{

                // create a new rectangle from the collision between the player and the rectangle
                Rectangle intersectRect = Rectangle.Intersect(playerHitBox, rectangle);

                // check for vertical collision (intersection is wider than it is tall)
                if (intersectRect.Width >= intersectRect.Height)
                {
                    // if the player Y is less than (further up than) the rectangle x
                    // move the player up
                    if (playerPosition.Y < intersectRect.Y)
                    {
                        playerPosition.Y -= intersectRect.Height;
                    }
                    // otherwise move down
                    else
                    {
                        playerPosition.Y += intersectRect.Height;
                    }

                    //the player's Y velocity is set to 0
                    playerVelocity.Y = 0;
                }
            }
		}

		/// <summary>
		/// Determines if a key was initially pressed this frame
		/// </summary>
		/// <param name="key">The key to check</param>
		/// <returns>True if this is the first frame the key is pressed, false otherwise</returns>
		private bool SingleKeyPress(Keys key)
		{
			return Keyboard.GetState().IsKeyDown(key) && prevKB.IsKeyUp(key);
		}


		/// <summary>
		/// Gets a rectangle for the player based on the player's
		/// current position (a vector of float values)
		/// </summary>
		/// <returns>A rectangle representing the bounds of the player</returns>
		private Rectangle GetPlayerRect()
		{
			return new Rectangle(
				(int)playerPosition.X,
				(int)playerPosition.Y,
				playerTexture.Width,
				playerTexture.Height);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			_spriteBatch.Begin();

			// Draw the player using a rectangle make from their position
			_spriteBatch.Draw(playerTexture, GetPlayerRect(), Color.White);

			// Draw each obstactle
			foreach (Rectangle rect in obstacleRects)
			{
				_spriteBatch.Draw(obstacleTexture, rect, Color.SeaGreen);
			}

			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}