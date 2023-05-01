using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShotgunBoomerang
{
    public enum ScorpionState
    {
        Idle,
        Skitter,
        Shooting,
        Damaged
    }

    internal class ScorpionEnemy : MobileEntity, IGameEnemy
    {

        private Texture2D _bulletSprite;
        private Texture2D _vegemiteDropSprite;

        private Direction _currentDirection;
        //private Vector2 defaultSpeed;
        //private Texture2D leftTexture;
        //private Texture2D rightTexture;
        private int _playerDetectionDistance;


        private ScorpionState _currentState;
        //private bool skitterEnd;
        private Vector2 _startPosition;
        private double _shotTimer;
        private double _shotTime;
        //private float initialTimer;
        private float _shotCounter;
        private Vector2 _bulletSpeed;
        private Color _drawColor;
        private bool _horizontalCollision;

        private double _damageTimer;
        private double _damageTime;

        //private float timer;

        //private bool damaged;



        /// <summary>
        /// Creates a scorpion that shoots at the player.
        /// </summary>
        /// <param name="leftSprite">A left facing version of the scorpion</param>
        /// <param name="rightSprite">A right facing version</param>
        /// <param name="position">The position of the scorp</param>
        /// <param name="maxHealth">The max health of the scorp</param>
        /// <param name="damage">How much damage the scorp does</param>
        /// <param name="moveSpeed">How fast the scorp moves</param>
        /// <param name="skitterSpeed">How fast it dashes for cover upon seeing the player</param>
        /// <param name="bulletSprite">The sprite for it's projectile</param>
        public ScorpionEnemy(List<Texture2D> texturePack, Vector2 position)
        {
            // the main sprite should be the first one
            _sprite = texturePack[0];

            // the bullet sprite should be the second one
            _bulletSprite = texturePack[1];

            // the vegemite sprite should be the third one
            _vegemiteDropSprite = texturePack[2];

            _height = _sprite.Height;
            _width = _sprite.Width;

            _position = position;
            _startPosition = new Vector2(_position.X, _position.Y);

            _currentState = ScorpionState.Idle;
            _currentDirection = Direction.Left;

            _maxHealth = 50;
            _health = _maxHealth;
            _damage = 25;

            //this.bulletSprite = bulletSprite;
            _velocity = Vector2.Zero;
            _acceleration = new Vector2(1, 0);
            _bulletSpeed = new Vector2(4, 0);
            //counter = 0f;

            _drawColor = Color.White;
            _shotTimer = 0;
            _shotTime = .5;
            _shotCounter = 3;


            //damaged = false;
            //defaultSpeed = new Vector2(moveSpeed, 0);
            _playerDetectionDistance = 640;

            _horizontalCollision = false;

            _damageTimer = 0;
            _damageTime = 0.5;


            /*
            if (_currentDirection == Direction.Left)
            {
                _velocity.X *= -1;
            }
            */
        }

        /*
        /// <summary>
        /// Updates the object's position and velocity based on physics interactions
        /// </summary>
        protected override void ApplyPhysics()
        {
            // apply gravity to velocity
            _velocity.Y += GameManager.Gravity;

            // apply velocity to position
            _position.Y += _velocity.Y;
        }
        */

        /// <summary>
        /// Deals with the logic for drawing the scorp
        /// I.E. dealing with direction changes and damaged
        /// </summary>
        /// <param name="sb">The sprite batch</param>
        /// <param name="screenOffset">The screen offset</param>
        public override void Draw(SpriteBatch sb, Vector2 screenOffset)
        {
            //Depending on the direction enum, either face the sprite left or right
            switch (_currentDirection)
            {
                case Direction.Left:
                    sb.Draw(
                        _sprite,
                        _position - screenOffset,
                        null,
                        _drawColor,
                        0,
                        new Vector2(0, 0),
                        1,
                        SpriteEffects.FlipHorizontally,
                        0);
                    break;

                case Direction.Right:
                    sb.Draw(
                        _sprite,
                        _position - screenOffset,
                        null,
                        _drawColor,
                        0,
                        new Vector2(0, 0),
                        1,
                        SpriteEffects.None,
                        0);
                    break;
            }

            /*
            //Draw the sprite red if damaged
            if (!damaged)
            {
                sb.Draw(_sprite, _position - screenOffset, Color.White);
            }
            else
            {
                sb.Draw(_sprite, _position + screenOffset, Color.Red);
            }
            */


            //sb.Draw(_sprite, new Vector2(_position.X, _position.Y) - screenOffset, Color.Red);

            //Test to show the bounds of the detection
            /*
            Vector2 leftBottom = new Vector2(_position.X - (tileDetectionDistance * _sprite.Width), _position.Y + ((tileDetectionDistance / 4) * _sprite.Height));
            Vector2 rightBottom = new Vector2(_position.X + (tileDetectionDistance * _sprite.Width), _position.Y + ((tileDetectionDistance / 4) * _sprite.Height));
            Vector2 leftUpper = new Vector2(_position.X - (tileDetectionDistance * _sprite.Width), _position.Y - ((tileDetectionDistance) * _sprite.Height));
            Vector2 rightUpper = new Vector2(_position.X + (tileDetectionDistance * _sprite.Width), _position.Y - ((tileDetectionDistance) * _sprite.Height));
            sb.Draw(_sprite, leftBottom - screenOffset, Color.Black);
            sb.Draw(_sprite, rightBottom - screenOffset, Color.Black);
            sb.Draw(_sprite, leftUpper - screenOffset, Color.Black);
            sb.Draw(_sprite, rightUpper - screenOffset, Color.Black);
            */

            //Test to show the distance on the ledge checking


            //sb.Draw(rightTexture, rightLCPos - screenOffset, Color.Red);



        }


        /// <summary>
        /// Method for use in the game's update step; all logic calculated 
        /// for this object by frame should go into this function.
        /// </summary>
        /// <param name="currentLevel">The level currently being played</param>
        /// <param name="player">The player</param>
        /// <param name="gameTime">tracks in-game time intervals</param>
        public void Update(
            Level currentLevel,
            Player player,
            GameTime gameTime)
        {
            /*
            Move();
            Attack(player);
            if (CheckAttackRange(player))
            {
                skitterEnd = false;
                if (player.X < _position.X)
                {
                    _velocity.X = (Math.Abs(_velocity.X)) * 1.25f;
                    direction = directionState.Left;
                }
                else if (player.X >= _position.X)
                {
                    _velocity.X = (float)(Math.Abs(_velocity.X) * -1) * 1.25f;
                    direction = directionState.Right;
                }
                enemyState = EnemyState.Skitter;

            }
            CheckForLedge(direction, tileMap);
            ResolveTileCollisions(tileMap);
            */

            //****
            //BY THE WAY: if you write something outside the switch statement it
            //still happens, so you can write you physics and collision methods here
            //****

            float runFriction = 0.8f;

            ApplyPhysics();

            ResolveTileCollisions(currentLevel.CurrentTileMap);

            /*
            // check if the scorpion is dead and if it is, remove it from the enemies list
            if (!CheckHealth())
            {
                currentLevel.CurrentEnemies.Remove(this);
            }
            */

            // set draw color to white by default
            _drawColor = Color.White;


            switch (_currentState)
            {


                //Handles logic for before encountering the player.
                //The scorp will simply move back and forth between walls until the player comes within
                //range of it.
                case ScorpionState.Idle:
                    //Move();

                    /*
                    if(CheckForLedge(currentLevel.CurrentTileMap))
                    {
                        // reverse directions when a ledge is detected
                        _acceleration *= -1;
                    }
                    */
                    

                    _velocity.X += _acceleration.X;
                    _velocity *= runFriction;

                    // change directions based on velocity
                    if(_velocity.X > 0)
                    {
                        _currentDirection = Direction.Right;
                    }
                    else
                    {
                        _currentDirection = Direction.Left;
                    }

                    //ResolveTileCollisions(currentLevel.CurrentTileMap);

                    //If the player comes within range, change to the 
                    //skitter state, where the scorp quickly runs away from the player and than
                    //gets ready to shoot at them
                    if ((player.Position - _position).Length() < _playerDetectionDistance)
                    {
                        //skitterEnd = false;

                        /*
                        if (player.X < _position.X) //If the player is to the left of the scorp
                        {
                            _velocity.X = (Math.Abs(_velocity.X)) * 2;
                            _currentDirection = Direction.Right;
                        }
                        else if (player.X >= _position.X) //If the player is to the right/directly above
                        {
                            _velocity.X = (float)(Math.Abs(_velocity.X) * -1) * 2;
                            _currentDirection = Direction.Left;
                        }
                        */

                        _currentState = ScorpionState.Skitter;

                    }
                    break;




                //Handles the skitter logic. 
                //Scorp goes until it either hits a wall or a ledge.
                case ScorpionState.Skitter:

                    //Move();
                    //ResolveTileCollisions(currentLevel.CurrentTileMap);

                    // if the player is to the left, the skitter right
                    if (player.X < _position.X)
                    {
                        _currentDirection = Direction.Right;
                        
                        // acceleration becomes positive
                        _acceleration = new Vector2(MathF.Abs(_acceleration.X), MathF.Abs(_acceleration.Y));
                    }
                    else // if player is to the right, skitter left
                    {
                        _currentDirection = Direction.Left;

                        // acceleration becomes negative
                        _acceleration = new Vector2(-MathF.Abs(_acceleration.X), -MathF.Abs(_acceleration.Y));
                    }

                    // the scorpion runs 2x as fast in the skitter state
                    _velocity.X += _acceleration.X * 3;
                    _velocity *= runFriction;


                    //Upon hitting a wall, detected by _horizontalCollision, transition to the shooting state
                    if (/*CheckForLedge(currentLevel.CurrentTileMap) ||*/ _horizontalCollision)
                    {
                        _currentState = ScorpionState.Shooting;
                    }
                    break;



                //Handles shooting logic.
                //Once the scorp goes as far as it can, it stops, turns around, and
                //begins looking for the player.
                //If the player is in range, it will begin to shoot bullets at the player.
                case ScorpionState.Shooting:
                    // the scorp should not move while in thios state

                    //Attack(player, currentLevel.CurrentProjectiles, 1.0f, gameTime);

                    // if the player is within range, face towards them and shoot
                    if((player.Position - _position).Length() < _playerDetectionDistance)
                    {
                        // if the timer is still running do nothing
                        if (_shotTimer >= 0)
                        {
                            _shotTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                        }
                        // when the timer is up, shoot and reset the timer
                        else
                        {
                            if (player.X < _position.X)
                            {
                                _currentDirection = Direction.Left;

                                currentLevel.CurrentProjectiles.Add(
                                    new Bullet(
                                        _bulletSprite,
                                        _position,
                                        -_bulletSpeed));

                            }
                            else // if player x is greater than this.x
                            {
                                _currentDirection = Direction.Right;

                                currentLevel.CurrentProjectiles.Add(
                                    new Bullet(
                                        _bulletSprite,
                                        _position,
                                        _bulletSpeed));
                            }
                            _shotCounter++;
                            _shotTimer = _shotTime;
                        }
                        
                    }
                    
                    // once the scorp has shot 3 times, all ow it to return to idle
                    if(_shotCounter >= 3)
                    {
                        _shotCounter = 0;
                        _currentState = ScorpionState.Idle;
                    }
                    
                    break;

                case ScorpionState.Damaged:
                    _damageTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                    // if the enemy died, remove it from the enemies list
                    if (_health <= 0)
                    {
                        // has a 1 in 10 chance of dropping vegemite
                        Random dropRng = new Random();
                        if (dropRng.Next(10) == 0)
                        {
                            currentLevel.CurrentProjectiles.Add(
                                new Vegemite(_vegemiteDropSprite,
                                _position,
                                _velocity));
                        }

                        currentLevel.CurrentEnemies.Remove(this);
                        player.Kills++;
                    }

                    // when the time is up, transition idle state
                    if (_damageTimer <= 0)
                    {
                        _currentState = ScorpionState.Idle;
                    }

                    break;
            }

        }

        /*
        /// <summary>
        /// Checks if the player is in range and than fires a projectile in their direction
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="projectilesList">The current projectiles</param>
        /// <param name="timer">The timer</param>
        /// <param name="gameTime"></param>
        public void Attack(Player player, List<IGameProjectile> projectilesList, float timer, GameTime gameTime)
        {

            //If the player is within range of the scorp
            if (CheckAttackRange(player))
            {
                _shotTimer = timer;
                initialTimer = _shotTimer;

                //Shoots once every second
                counter += (float)gameTime.ElapsedGameTime.TotalSeconds;

                
                if (counter >= _shotTimer)
                {
                    _shotTimer = initialTimer;
                    counter = 0;

                    //If the player is to the left of the scorp, spawn a bullet going left.
                    //If the player is to the right, spawn a bullet going right
                    if (player.X < _position.X)
                    {
                        projectilesList.Add(new Bullet(bulletSprite, _position, new Vector2(-10, 0)));
                    }
                    else if (player.X > _position.X)
                    {
                        projectilesList.Add(new Bullet(bulletSprite, _position, new Vector2(10, 0)));
                    }
                }

            }
        }
        */

        /*
        /// <summary>
        /// Appeases the gods
        /// 
        /// The interface was being irritating so this was the fastest fix
        /// </summary>
        /// <param name="player"></param>
        public void Attack(Player player)
        {

        }
        */

        /*
        /// <summary>
        /// Checks whether or not the player is in range for the scorpion to begin attacking
        /// </summary>
        /// <param name="player">The player</param>
        /// <returns>Whether or not the player is in range</returns>
        public bool CheckAttackRange(Player player)
        {

            //Checks if the player is within a certain range of the scrop.
            if (
                player.X > _position.X - (tileDetectionDistance * _sprite.Width) //Checks if the player is on the left side
                &&
                player.X < _position.X + (tileDetectionDistance * _sprite.Width) //Checks if the player is on the right side
                &&
                player.Y <= _position.Y + (_sprite.Width) //Checks if the player isn't too low
                &&
                player.Y >= _position.Y - (tileDetectionDistance * _sprite.Height) //Checks if the player isn't too high
                )

            {
                return true;
            }
            else
            {
                return false;
            }

        }
        */

        /*
        /// <summary>
        /// Returns true if a ledge is detected
        /// </summary>
        /// <param name="tileMap">The tileMap</param>
        /// <returns>Whether or not there is a ledge</returns>
        public bool CheckForLedge(List<Tile> tileMap)
        {
            bool collisionDetected = false;

            // should represent a rectangle with width equal to the x velocity (i.e. it is the amount the scorpion will move next frame) and which is one tile in the ground
            Rectangle detectorRectangle = new Rectangle((int)_position.X + (int)_velocity.X, (int)_position.Y + _width, (int)_velocity.X, 64);

            // if the scorpion is moving right, also add the width of the sprite to the X pos
            if(_currentDirection == Direction.Right) { detectorRectangle.X += _width;}
            
            foreach(Tile tile in tileMap)
            {
                if(detectorRectangle.Intersects(tile.HitBox))
                {
                    return false;
                }
            }

            // if a ledge is detected, immeditatly stop 
            _velocity = Vector2.Zero;

            return true;
        }
        */

        /*
        /// <summary>
        /// Check for collision with another object
        /// </summary>
        /// <param name="other">The object to check against</param>
        /// <returns>Whether or not a collision was detected</returns>
        public bool CheckCollision(MobileEntity other)
        {
            return HitBox.Intersects(other.HitBox);
        }
        */

        /*
        /// <summary>
        /// Returns true if the scorpion still has health
        /// </summary>
        /// <returns></returns>
        public bool CheckHealth()
        {
            if (_health <= 0)
            {

                return false;
            }
            else
            {
                return true;
            }
        }
        */

        /*
        /// <summary>
        /// Adds the velocity to the position
        /// </summary>
        public void Move()
        {
            _position += _velocity;


        }
        */


        /// <summary>
        /// Resets the scorp
        /// </summary>
        public void Reset()
        {
            _health = _maxHealth;
            _position.X = _startPosition.X;
            _position.Y = _startPosition.Y;
            //isAlive = true;
            //onGround = false;
            _velocity = Vector2.Zero;
            _currentState = ScorpionState.Idle;
            _drawColor = Color.White;
            _damageTimer = 0;
            _shotCounter = 0;
            _shotTimer = 0;
        }

        /// <summary>
        /// Take damage
        /// </summary>
        /// <param name="damage">Amount of damage to take</param>
        public void TakeHit(GameObject attacker, float damage)
        {
            //If the scorp hasn't already been damaged, take damage
            if (_currentState != ScorpionState.Damaged)
            {
                _health -= damage;
                _damageTimer = _damageTime;
                

                _currentState = ScorpionState.Damaged;

                // get the normalized vector between the player's centerpoint and the enemy's centerpoint
                Vector2 attackerNormal = Vector2.Normalize(CenterPoint - attacker.CenterPoint);

                // throw the enemy away from it's attacker (throw force scales with damage)
                _velocity += attackerNormal * (damage / 2);

                /*
                if (player.X < _position.X)
                {
                    //bump.X = 2;
                }
                else if (player.X > _position.X)
                {
                    //bump.X = -2;
                }
                */

                //_velocity += bump * 3;
                //bump.X = 2;
            }
        }

        /// <summary>
        /// Definitely not just Auden's code I took cause it wasn't necessary to do the work again
        /// 
        /// </summary>
        /// <param name="tileMap"></param>
        protected override void ResolveTileCollisions(List<Tile> tileMap)
        {
            // reset wall sensor
            _horizontalCollision = false;

            // get the snake's hitbox
            Rectangle SnakeHitBox = this.HitBox;

            // temporary list for all intersections
            List<Rectangle> intersectionsList = new List<Rectangle>();

            // loop through the list of all tiles
            // to find which ones are interseting
            foreach (Tile tile in tileMap)
            {
                // if the snake is intersecting the tile
                if (this.CheckCollision(tile))
                {
                    // add it's hitbox to the list
                    intersectionsList.Add(tile.HitBox);
                }
            }

            while (intersectionsList.Count > 0)
            {
                // variable to store the largest intersection
                Rectangle largest = new Rectangle();

                // find the largest intersection
                foreach (Rectangle rectangle in intersectionsList)
                {
                    if (rectangle.Width * rectangle.Height >= largest.Width * largest.Height)
                    {
                        largest = rectangle;
                    }
                }

                // resolve the largest collision
                Rectangle intersectRect = Rectangle.Intersect(SnakeHitBox, largest);

                //check for a horizontal collision (intersection is taller than it is wide)
                if (intersectRect.Width <= intersectRect.Height)
                {
                    // if the snake X is less than (further left than) the intersection's X
                    // move the snake left
                    if (this._position.X < intersectRect.X)
                    {
                        SnakeHitBox.X -= intersectRect.Width;

                        //the player's X velocity cannot be positive when touching the right wall
                        this._velocity.X = Math.Clamp(_velocity.X, float.MinValue, 0);



                        //I implemented this conditional because I was concerned that the snake was getting caught
                        //on the lip of the next tile.
                        //It wasn't, but the limit doesn't affect the desired behaviour. Removing it makes no difference

                        /*
                        if (intersectRect.Height >= 16)
                        {
                            //goingLeft = !goingLeft;
                            this._velocity.X *= -1;
                            //_acceleration.X *= -1;

                        }
                        */


                    }
                    // otherwise move right
                    else
                    {
                        SnakeHitBox.X += intersectRect.Width;

                        //the player's X velocity cannot be negative when touching the left wall
                        //this._velocity.X = Math.Clamp(_velocity.X, 0, float.MaxValue);


                        if (intersectRect.Height >= 16)
                        {
                            //goingLeft = !goingLeft;
                            this._velocity.X *= -1;
                            //_acceleration.X *= -1;
                        }


                    }

                    // when a horizontal collision is made, the snake should
                    // head in the opposite direction and it should be reported
                    _acceleration *= -1;

                    _horizontalCollision = true;

                    //goingLeft = !goingLeft
                    this._position.X = SnakeHitBox.X;
                }
                // otherwise this must be a vertical collision
                else
                {
                    // if the snake Y is less than (further up than) the rectangle Y
                    // move the snake up
                    if (this._position.Y < intersectRect.Y)
                    {
                        SnakeHitBox.Y -= intersectRect.Height;

                        //the snake's Y velocity cannot be negative when touching the ground
                        this._velocity.Y = Math.Clamp(_velocity.Y, float.MinValue, 0);
                    }
                    // otherwise the snake has hit thier head, move down
                    else
                    {
                        SnakeHitBox.Y += intersectRect.Height;

                        //the player's Y velocity cannot be positive when touching the cieling
                        this._velocity.Y = Math.Clamp(_velocity.Y, 0, float.MaxValue);
                    }

                    this._position.Y = SnakeHitBox.Y;
                }

                // reset the intersections list and check again
                intersectionsList.Clear();

                // loop through the list of all tiles
                // to find which ones are interseting
                foreach (Tile tile in tileMap)
                {
                    // if the player is intersecting the tile
                    if (tile.HitBox.Intersects(SnakeHitBox))
                    {
                        // add it's hitbox to the list
                        intersectionsList.Add(tile.HitBox);
                    }
                }
            }
        }
    }
}
