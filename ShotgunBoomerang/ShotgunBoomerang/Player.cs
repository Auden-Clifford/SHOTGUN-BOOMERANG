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
    /// <summary>
    /// Determines the different states the player can be in
    /// </summary>
    public enum PlayerState
    {
        Idle,
        Run,
        Airborne,
        Slide,
        Skid
    }

    /// <summary>
    /// Determines the directions the player can face
    /// </summary>
    public enum Direction
    {
        Left,
        Right
    }
    internal class Player : MobileEntity
    {
        // Fields

        private int _ammo;
        private bool _isHoldingBoomerang;
        private PlayerState _currentState;
        private float _shotgunRadius;
        private float _shotgunAngle;
        private bool _isCollidingWithGround;
        private float _jumpForce;


        // Properties
        /// <summary>
        /// Gets the player's current state
        /// </summary>
        public PlayerState CurrentState { get { return _currentState; } }
        
        // Constructors

        /// <summary>
        /// Creates a new player with given texture, position, and health
        /// </summary>
        /// <param name="sprite">The player's texture/spritesheet</param>
        /// <param name="position">The player's starting position</param>
        /// <param name="health">The player's starting health</param>
        public Player(Texture2D sprite, Vector2 position, float health)
        {
            _sprite = sprite;
            _position = position;
            _health = health;

            _velocity = new Vector2(0, 0);
            _maxHealth = 100;
            _damage = 60;
            _acceleration = 3;
            _ammo = 2;
            _isHoldingBoomerang = true;
            _currentState = PlayerState.Idle;
            _shotgunRadius = 64;
            _shotgunAngle = 45;
            _isCollidingWithGround = false;
            _jumpForce = 16;
        }


        // Methods
        /// <summary>
        /// Draws the player with animations based on FSM
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }

        /// <summary>
        /// Contains the logic for updating the player, including FSM and movement
        /// </summary>
        public override void Update()
        {
            // The player is slowed by different amounts depending
            // on whether they are running, skidding, or in the air
            float runFriction = 0.8f;
            float airFriction = 0.99f;
            float skidFriction = 0.7f;

            switch (_currentState)
            {
                case PlayerState.Idle:
                    // if the player presses the spacebar or w jump
                    if(GameManager.kb.IsKeyDown(Keys.Space) || GameManager.kb.IsKeyDown(Keys.W))
                    {
                        _velocity.Y -= _jumpForce;
                    }

                    // Transition to Airborne when no longer colliding with the ground
                    if(!_isCollidingWithGround)
                    {
                        _currentState = PlayerState.Airborne;
                    }

                    // Transition to Run when A or D is pressed
                    if(GameManager.kb.IsKeyDown(Keys.A) || GameManager.kb.IsKeyDown(Keys.D))
                    {
                        _currentState = PlayerState.Run;
                    }

                    break;

                case PlayerState.Run:
                    // if the player presses the spacebar or w jump
                    if (GameManager.kb.IsKeyDown(Keys.Space) || GameManager.kb.IsKeyDown(Keys.W))
                    {
                        _velocity.Y -= _jumpForce;
                    }

                    // while A or D are pressed, increase the player's velocity
                    if (GameManager.kb.IsKeyDown(Keys.A))
                    {
                        _velocity.X -= _acceleration;
                    }

                    if (GameManager.kb.IsKeyDown(Keys.D))
                    {
                        _velocity.X += _acceleration;
                    }

                    // apply ground friction
                    _velocity *= runFriction;

                    // Transition to Airborne when no longer colliding with the ground
                    if (!_isCollidingWithGround)
                    {
                        _currentState = PlayerState.Airborne;
                    }

                    // Transition to Slide when CTRL is pressed
                    if(GameManager.kb.IsKeyDown(Keys.LeftControl))
                    {
                        _currentState = PlayerState.Slide;
                    }

                    // Transition to Skid if the key does not match the direction of motion
                    if((GameManager.kb.IsKeyUp(Keys.A) && GameManager.kb.IsKeyUp(Keys.D)) // all keys are released
                        || (GameManager.kb.IsKeyDown(Keys.A) && _velocity.X > 0) // A is pressed but the player is moving right
                        || (GameManager.kb.IsKeyDown(Keys.D) && _velocity.X < 0)) // D is pressed but the player is moving left
                    {
                        _currentState = PlayerState.Skid;
                    }
                        break;

                case PlayerState.Airborne:
                    // appky air friction to velocity
                    _velocity *= airFriction;

                    // this state ends once the player hits the ground
                    if(_isCollidingWithGround)
                    {
                        // if the horizontal velocity is 0, transition to idle
                        if(_velocity.X == 0)
                        {
                            _currentState = PlayerState.Idle;
                        }
                        // otherwise transition to run
                        else
                        {
                            _currentState = PlayerState.Run;
                        }
                    }
                    break;

                case PlayerState.Slide:
                    // if the player presses the spacebar or W, jump
                    if (GameManager.kb.IsKeyDown(Keys.Space) || GameManager.kb.IsKeyDown(Keys.W))
                    {
                        _velocity.Y -= _jumpForce;
                    }

                    // Transition to Run when CTRL is released
                    if (GameManager.kb.IsKeyUp(Keys.LeftControl))
                    {
                        _currentState = PlayerState.Run;
                    }

                    // Transition to Airborne when no longer colliding with ground
                    if(!_isCollidingWithGround)
                    {
                        _currentState = PlayerState.Airborne;
                    }
                    break;

                case PlayerState.Skid:
                    // if the player presses the spacebar or W, jump
                    if (GameManager.kb.IsKeyDown(Keys.Space) || GameManager.kb.IsKeyDown(Keys.W))
                    {
                        _velocity.Y -= _jumpForce;
                    }

                    // apply friction to the player's velocity
                    _velocity *= skidFriction;

                    // once the player reaches 0.01 velocity it should become 0
                    if(MathF.Abs(_velocity.X) < 0.1) 
                    {
                        _velocity.X = 0;
                    }
                    // Transition to Idle when there is no horizontal velocity
                    if(_velocity.X == 0)
                    {
                        _currentState = PlayerState.Idle;
                    }
                    // Transition to Run if A or D is pressed
                    if(GameManager.kb.IsKeyDown(Keys.A) || (GameManager.kb.IsKeyDown(Keys.D)))
                    {
                        _currentState = PlayerState.Run;
                    }
                    break;
            }
            

            // the player's isCollidingWithGround variable must always
            // be set to false at the end of Update, it will be detected again in ResolveCollisions
            _isCollidingWithGround = false;
        }

        /// <summary>
        /// Resolves collisions with tiles to that the player 
        /// collides with them properly and reports whether 
        /// the player is in contact with the ground
        /// **IMPORTANT: this method must always come before the player's update method**
        /// </summary>
        /// <param name="tileMap">The list of tiles in the currently loaded level</param>
        public void ResolveCollisions(List<Tile> tileMap)
        {
            //gravity is applied beforehand
            ApplyPhysics();

            // get the player's hitbox
            Rectangle playerHitBox = this.HitBox;

            // temporary list for all intersections
            List<Rectangle> intersectionsList = new List<Rectangle>();

            // loop through the list of all rectangles
            foreach (Tile tile in tileMap)
            {
                // if the player is intersecting the tile
                if (tile.HitBox.Intersects(playerHitBox))
                {
                    // add it's hitbox to the list
                    intersectionsList.Add(tile.HitBox);
                }
            }

            // loop through and resolve all horizontal intersections
            foreach (Rectangle rectangle in intersectionsList)
            {
                // create a new rectangle from the collision between the player and the rectangle
                Rectangle intersectRect = Rectangle.Intersect(playerHitBox, rectangle);

                //check for a horizontal collision (intersection is taller than it is wide)
                if (intersectRect.Width <= intersectRect.Height)
                {
                    // if the player X is less than (further left than) the rectangle x
                    // move the player left
                    if (this._position.X < intersectRect.X)
                    {
                        playerHitBox.X -= intersectRect.Width;

                        //the player's X velocity cannot be positive when touching the right wall
                        this._velocity.X = Math.Clamp(_velocity.X, float.MinValue, 0);
                    }
                    // otherwise move right
                    else
                    {
                        playerHitBox.X += intersectRect.Width;

                        //the player's X velocity cannot be negative when touching the left wall
                        this._velocity.X = Math.Clamp(_velocity.X, 0, float.MaxValue);
                    }

                }


            }

            this._position.X = playerHitBox.X;

            // loop through and resolve all vertical intersections
            foreach (Rectangle rectangle in intersectionsList)
            {

                // create a new rectangle from the collision between the player and the rectangle
                Rectangle intersectRect = Rectangle.Intersect(playerHitBox, rectangle);

                // check for vertical collision (intersection is wider than it is tall)
                if (intersectRect.Width > intersectRect.Height)
                {
                    // if the player Y is less than (further up than) the rectangle Y
                    // move the player up
                    if (this._position.Y < intersectRect.Y)
                    {
                        playerHitBox.Y -= intersectRect.Height;

                        // this means the player is in contact with the ground
                        _isCollidingWithGround = true;

                        //the player's Y velocity cannot be negative when touching the ground
                        this._velocity.Y = Math.Clamp(_velocity.Y, float.MinValue, 0);
                    }
                    // otherwise the player has hit thier head, move down
                    else
                    {
                        playerHitBox.Y += intersectRect.Height;

                        //the player's Y velocity cannot be positive when touching the cieling
                        this._velocity.Y = Math.Clamp(_velocity.Y, 0, float.MaxValue);
                    }

                    
                }


            }

            this._position.Y = playerHitBox.Y;
        }

        private void ShotgunAttack()
        {

        }
    }
}