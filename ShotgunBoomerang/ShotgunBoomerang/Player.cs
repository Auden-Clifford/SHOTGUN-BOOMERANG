using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Threading;

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
        Skid,
        Damaged
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

        private Texture2D _boomerangSprite;
        private int _ammo;
        private bool _isHoldingBoomerang;
        private PlayerState _currentState;
        private Direction _currentDirection;
        private float _shotgunRadius;
        private float _shotgunAngle;
        private bool _isCollidingWithGround;
        private float _jumpForce;

        private double score;
        private int kills;
        private double dmgTimer; //this might not be handled here. won't implement until it's clear

        private Color drawColor;

        // Properties

        /// <summary>
        /// Gets the player's current state
        /// </summary>
        public PlayerState CurrentState { get { return _currentState; } }

        /// <summary>
        /// Gets the direction the player is currently facing
        /// </summary>
        public Direction CurrentDirection { get { return _currentDirection; } }

        /// <summary>
        /// Gets or sets whether the player is holding the boomerang
        /// </summary>
        public bool IsHoldingBoomerang { get { return _isHoldingBoomerang; } set { _isHoldingBoomerang= value; } }

        /// <summary>
        /// Gets the X coord of the player
        /// </summary>
        public float X
        {
            get { return _position.X; }
        }

        /// <summary>
        /// Gets the Y coord of the player
        /// </summary>
        public float Y
        {
            get { return _position.Y; }
        }


        /// <summary>
        /// retrieves/sets the players current score
        /// </summary>
        public double Score { 
            get { return score; }
            set { score = value; }
        }

        /// <summary>
        /// retrieves/sets the players kill count
        /// </summary>
        public int Kills
        {
            get { return kills; }
            set { kills = value; }
        }

        /// <summary>
        /// retrieves/sets the players ammo
        /// </summary>
        public int Ammo
        {
            get { return _ammo; }
            set { _ammo = value; }
        }

        /// <summary>
        /// Gets the player's shotgun damage radius
        /// </summary>
        public float ShotgunRadius { get { return _shotgunRadius; } }
        
        // Constructors

        /// <summary>
        /// Creates a new player with given texture, position, and health
        /// </summary>
        /// <param name="sprite">The player's texture/spritesheet</param>
        /// /// <param name="sprite">The boomerang's texture/spritesheet</param>
        /// <param name="position">The player's starting position</param>
        /// <param name="health">The player's starting health</param>
        public Player(Texture2D sprite, Texture2D boomerangSprite, Vector2 position, float health)
        {
            _sprite = sprite;
            _boomerangSprite = boomerangSprite;
            _position = position;
            _health = health;

            _velocity = new Vector2(0, 0);
            _maxHealth = 100;
            _damage = 60;
            _acceleration = new Vector2(3, 0);
            _ammo = 2;
            _isHoldingBoomerang = true;
            _currentState = PlayerState.Idle;
            _shotgunRadius = 384;
            _shotgunAngle = MathF.PI / 4;
            _isCollidingWithGround = false;
            _jumpForce = 32;

            dmgTimer = .5;
            drawColor = Color.White;
        }


        // Methods
        /// <summary>
        /// Draws the player with animations based on FSM
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb, GraphicsDeviceManager graphics)
        {
            sb.Draw(
                _sprite,
                new Vector2(
                    graphics.PreferredBackBufferWidth / 2 
                    - _sprite.Width / 2,
                    graphics.PreferredBackBufferHeight / 2 
                    - _sprite.Height / 2), 
                drawColor);
        }

        /// <summary>
        /// Base method for use in the update loop, should contain all logic the object needs to go through 
        /// in a frame as well as any parameters from the game manager that might be needed for this logic. 
        /// Update will be the entry point for all data from Game manager to the other classes
        /// -- The player's update should allow the user to control the player using the keyboard and mouse. 
        /// The character has several different states that change depending on user input.
        /// </summary>
        /// <param name="kb">The keyboard state this frame</param>
        /// <param name="prevKb"> The keyboard state last frame</param>
        /// <param name="ms">The mouse state this frame</param>
        /// <param name="prevMs">The mouse state last frame</param>
        /// <param name="tileMap">The current level's tiles</param>
        /// <param name="enemies">The current level's enemies</param>
        /// <param name="projectiles">The projectiles currently in play</param>
        /// <param name="graphics">Contains info about the screen</param>
        public void Update(
            KeyboardState kb,
            KeyboardState prevKb,
            MouseState ms,
            MouseState prevMs,
            List<Tile> tileMap,
            List<IGameEnemy> enemies,
            List<IGameProjectile> projectiles,
            GraphicsDeviceManager graphics,
            GameTime gameTime)
        {
            // The player is slowed by different amounts depending
            // on whether they are running, skidding, or in the air
            float runFriction = 0.8f;
            float airFriction = 0.99f;
            float skidFriction = 0.7f;

            // resolve tile collisions before anything else
            ResolveTileCollisions(tileMap);

            // determine the direction the player is facing
            if(ms.Position.X < graphics.PreferredBackBufferWidth / 2)
            {
                _currentDirection = Direction.Right;
            }
            else
            {
                _currentDirection = Direction.Left;
            }

            switch (_currentState)
            {
                case PlayerState.Idle:
                    // if the player presses the spacebar or w (only once) jump
                    if(kb.IsKeyDown(Keys.Space) && 
                        prevKb.IsKeyUp(Keys.Space) || 
                        kb.IsKeyDown(Keys.W) && 
                        prevKb.IsKeyUp(Keys.W))
                    {
                        _velocity.Y -= _jumpForce;
                    }

                    // if the player left clicks (only once), perform a shotgun attack
                    if(ms.LeftButton == ButtonState.Pressed && 
                        prevMs.LeftButton == ButtonState.Released)
                    {
                        ShotgunAttack(ms, graphics, enemies, projectiles);  
                    }

                    // if the player right clicks (only once) and
                    // the boomerang is held, perform a boomerang attack
                    if (ms.RightButton == ButtonState.Pressed &&
                        prevMs.RightButton == ButtonState.Released &&
                        _isHoldingBoomerang)
                    {
                        BoomerangAttack(ms, graphics, projectiles);
                    }

                    // Transition to Airborne when no longer colliding with the ground
                    if (!_isCollidingWithGround)
                    {
                        _currentState = PlayerState.Airborne;
                    }

                    // Transition to Run when A or D is pressed or if X velocity is not 0
                    if(kb.IsKeyDown(Keys.A) || kb.IsKeyDown(Keys.D) || _velocity.X != 0)
                    {
                        _currentState = PlayerState.Run;
                    }

                    break;

                case PlayerState.Run:
                    // if the player presses the spacebar or w jump
                    if (kb.IsKeyDown(Keys.Space) &&
                        prevKb.IsKeyUp(Keys.Space) ||
                        kb.IsKeyDown(Keys.W) &&
                        prevKb.IsKeyUp(Keys.W))
                    {
                        _velocity.Y -= _jumpForce;
                    }

                    // if the player left clicks (only once), perform a shotgun attack
                    if (ms.LeftButton == ButtonState.Pressed &&
                        prevMs.LeftButton == ButtonState.Released)
                    {
                        ShotgunAttack(ms, graphics, enemies, projectiles);
                    }

                    // if the player right clicks (only once), perform a boomerang attack
                    if (ms.RightButton == ButtonState.Pressed &&
                        prevMs.RightButton == ButtonState.Released &&
                        _isHoldingBoomerang)
                    {
                        BoomerangAttack(ms, graphics, projectiles);
                    }

                    // while A or D are pressed, increase the player's velocity
                    if (kb.IsKeyDown(Keys.A))
                    {
                        _velocity.X -= _acceleration.X;
                    }

                    if (kb.IsKeyDown(Keys.D))
                    {
                        _velocity.X += _acceleration.X;
                    }

                    // apply ground friction
                    _velocity *= runFriction;

                    // Transition to Airborne when no longer colliding with the ground
                    if (!_isCollidingWithGround)
                    {
                        _currentState = PlayerState.Airborne;
                    }

                    // Transition to Slide when CTRL is pressed
                    if(kb.IsKeyDown(Keys.LeftControl))
                    {
                        
                        _currentState = PlayerState.Slide;
                    }

                    // Transition to Skid if the key does not match the direction of motion
                    if((kb.IsKeyUp(Keys.A) && kb.IsKeyUp(Keys.D)) // all keys are released
                        || (kb.IsKeyDown(Keys.A) && _velocity.X > 0) // A is pressed but the player is moving right
                        || (kb.IsKeyDown(Keys.D) && _velocity.X < 0)) // D is pressed but the player is moving left
                    {
                        _currentState = PlayerState.Skid;
                    }
                        break;

                case PlayerState.Airborne:
                    // if the player left clicks (only once), perform a shotgun attack
                    if (ms.LeftButton == ButtonState.Pressed &&
                        prevMs.LeftButton == ButtonState.Released && _ammo > 0)
                    {
                        ShotgunAttack(ms, graphics, enemies, projectiles);
                        _ammo--;
                    }

                    // if the player right clicks (only once), perform a boomerang attack
                    if (ms.RightButton == ButtonState.Pressed &&
                        prevMs.RightButton == ButtonState.Released &&
                        _isHoldingBoomerang)
                    {
                        BoomerangAttack(ms, graphics, projectiles);
                    }

                    // appky air friction to velocity
                    _velocity *= airFriction;

                    // this state ends once the player hits the ground
                    if(_isCollidingWithGround)
                    {
                        _ammo = 2;
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
                    // if the player presses the spacebar or W (only once), jump
                    if (kb.IsKeyDown(Keys.Space) &&
                        prevKb.IsKeyUp(Keys.Space) ||
                        kb.IsKeyDown(Keys.W) &&
                        prevKb.IsKeyUp(Keys.W))
                    {
                        _velocity.Y -= _jumpForce;
                    }

                    // if the player left clicks (only once), perform a shotgun attack
                    if (ms.LeftButton == ButtonState.Pressed &&
                        prevMs.LeftButton == ButtonState.Released)
                    {
                        ShotgunAttack(ms, graphics, enemies, projectiles);
                    }

                    // if the player right clicks (only once), perform a boomerang attack
                    if (ms.RightButton == ButtonState.Pressed &&
                        prevMs.RightButton == ButtonState.Released &&
                        _isHoldingBoomerang)
                    {
                        BoomerangAttack(ms, graphics, projectiles);
                    }

                    // Transition to Run when CTRL is released
                    if (kb.IsKeyUp(Keys.LeftControl))
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
                    // if the player presses the spacebar or W (only once), jump
                    if (kb.IsKeyDown(Keys.Space) &&
                        prevKb.IsKeyUp(Keys.Space) ||
                        kb.IsKeyDown(Keys.W) &&
                        prevKb.IsKeyUp(Keys.W))
                    {
                        _velocity.Y -= _jumpForce;
                    }

                    // if the player left clicks (only once), perform a shotgun attack
                    if (ms.LeftButton == ButtonState.Pressed &&
                        prevMs.LeftButton == ButtonState.Released)
                    {
                        ShotgunAttack(ms, graphics, enemies, projectiles);
                    }

                    // if the player right clicks (only once), perform a boomerang attack
                    if (ms.RightButton == ButtonState.Pressed &&
                        prevMs.RightButton == ButtonState.Released &&
                        _isHoldingBoomerang)
                    {
                        BoomerangAttack(ms, graphics, projectiles);
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
                    // Transition to Airborne when no longer colliding with ground
                    if (!_isCollidingWithGround)
                    {
                        _currentState = PlayerState.Airborne;
                    }
                    // Transition to Run if A or D is pressed
                    if (kb.IsKeyDown(Keys.A) || (kb.IsKeyDown(Keys.D)))
                    {
                        _currentState = PlayerState.Run;
                    }
                    break;

                case PlayerState.Damaged:
                    dmgTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                    drawColor = Color.Red;

                    _velocity *= 0.99f;

                    if(dmgTimer <= 0)
                    {
                        drawColor = Color.White;
                        dmgTimer = .5;
                        _currentState = PlayerState.Idle;
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
        /// **IMPORTANT: this method must always come at the beginning of the player's update method**
        /// </summary>
        /// <param name="tileMap">The list of tiles in the currently loaded level</param>
        protected override void ResolveTileCollisions(List<Tile> tileMap)
        {
            
            //gravity is applied beforehand
            ApplyPhysics();

            // get the player's hitbox
            Rectangle playerHitBox = this.HitBox;

            // temporary list for all intersections
            List<Rectangle> intersectionsList = new List<Rectangle>();

            // loop through the list of all tiles
            // to find which ones are interseting
            foreach (Tile tile in tileMap)
            {
                // if the player is intersecting the tile
                if (tile.CheckCollision(this))
                {
                    // add it's hitbox to the list
                    intersectionsList.Add(tile.HitBox);
                }
            }

            while(intersectionsList.Count > 0)
            {
                // variable to store the largest intersection
                Rectangle largest = new Rectangle();

                // find the largest intersection
                foreach(Rectangle rectangle in intersectionsList)
                {
                    if (rectangle.Width * rectangle.Height >= largest.Width * largest.Height)
                    {
                        largest = rectangle;
                    }
                }

                // resolve the largest collision
                Rectangle intersectRect = Rectangle.Intersect(playerHitBox, largest);

                //check for a horizontal collision (intersection is taller than it is wide)
                if (intersectRect.Width <= intersectRect.Height)
                {
                    // if the player X is less than (further left than) the intersection's x
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

                    this._position.X = playerHitBox.X;
                }
                // otherwise this must be a vertical collision
                else
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

                    this._position.Y = playerHitBox.Y;
                }

                // reset the intersections list and check again
                intersectionsList.Clear();

                // loop through the list of all tiles
                // to find which ones are interseting
                foreach (Tile tile in tileMap)
                {
                    // if the player is intersecting the tile
                    if (tile.HitBox.Intersects(playerHitBox))
                    {
                        // add it's hitbox to the list
                        intersectionsList.Add(tile.HitBox);
                    }
                }
            }
        }

        private void ShotgunAttack(MouseState ms, GraphicsDeviceManager graphics, List<IGameEnemy> enemies, List<IGameProjectile> projectiles)
        {
            // need the mouse's position to be a Vector2 for math
            Vector2 mousePos = new Vector2(ms.Position.X, ms.Position.Y);

            //caluclates screen center
            Vector2 screenCenter = new Vector2(graphics.PreferredBackBufferWidth / 2,
                graphics.PreferredBackBufferHeight / 2);

            // velocity normal between the mouse and the player's centerpoint
            Vector2 mouseCenterNormal = Vector2.Normalize(mousePos - screenCenter);

            // throw the player back in the opposite direction of the blast
            _velocity -= mouseCenterNormal * (_damage / 2);

            //calculates the lines which bound the blast
            float angle = MathF.Atan2(-mouseCenterNormal.Y, mouseCenterNormal.X);

            //Caluclates shotgun hits on enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                MobileEntity currentEnemy = (MobileEntity)enemies[i];
                
                float distance = Vector2.Distance(CenterPoint, currentEnemy.CenterPoint);
                float enemyAngle = MathF.Atan2(-(currentEnemy.CenterPoint.Y - CenterPoint.Y),
                    (currentEnemy.CenterPoint.X - screenCenter.X));

                if (distance <= _shotgunRadius && // only if the enemy is within the radius
                    enemyAngle <= angle + _shotgunAngle / 2 && // and the angle between the enemy and the player is less than the max spread angle
                    enemyAngle >= angle - _shotgunAngle / 2)  // and the angle between the enemy and the player is greater than the min spread angle
                {
                    enemies[i].TakeDamage(_damage, this);
                }
                
            }

            for(int i = 0; i < projectiles.Count; i++)
            {
                Boomerang currentProjectile = (Boomerang)projectiles[i];

                float distance = Vector2.Distance(CenterPoint, currentProjectile.CenterPoint);
                float projectileAngle = MathF.Atan2(-(currentProjectile.CenterPoint.Y - CenterPoint.Y),
                    (currentProjectile.CenterPoint.X - screenCenter.X));

                //caluclates if enemy is in range
                if (distance <= _shotgunRadius & // only if the projectile is within the radius
                    projectileAngle <= angle + _shotgunAngle / 2 && // and the angle between the projectile and the player is less than the max spread angle
                    projectileAngle >= angle - _shotgunAngle / 2)  // and the angle between the projectile and the player is greater than the min spread angle
                {
                    currentProjectile.ShotgunHit(mouseCenterNormal, graphics, _damage);
                }
            }
            
        }

        private void BoomerangAttack(MouseState ms, GraphicsDeviceManager graphics, List<IGameProjectile> projectiles)
        {
            Vector2 mousePos = new Vector2(ms.X, ms.Y);

            // velocity normal between the mouse and the center of the screen
            Vector2 velocityNormal = Vector2.Normalize(mousePos -
                new Vector2(graphics.PreferredBackBufferWidth / 2,
                           graphics.PreferredBackBufferHeight / 2));

            // add the boomerang to the game at the player's position witht the player's velocity +some
            projectiles.Add(new Boomerang(_boomerangSprite, _position, _velocity + velocityNormal * 30));

            // player is no longer holding the boomerang
            _isHoldingBoomerang = false;


        }

        /// <summary>
        /// Player takes damage, gains i frames
        /// </summary>
        public void TakeDamage(float incDmg)
        {
            if (_currentState != PlayerState.Damaged)
            {
                _health -= incDmg;
                _currentState = PlayerState.Damaged;
            }
        }
    }
}