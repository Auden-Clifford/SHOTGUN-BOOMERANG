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
    enum EnemyState
    {
        Idle,
        Skitter,
        Shooting
    }

    internal class ScorpionEnemy : MobileEntity, IGameEnemy
    {
        private directionState direction;
        private Vector2 defaultSpeed;
        private Texture2D leftTexture;
        private Texture2D rightTexture;
        private int tileDetectionDistance;


        private EnemyState enemyState;
        private bool skitterEnd;
        private Vector2 startPos;
        private float shootingTimer;
        private float initialTimer;
        private float counter;
        private Vector2 bulletSpeed;
        private Texture2D bulletSprite;
        private float timer;

        private bool damaged;



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
        public ScorpionEnemy(Texture2D leftSprite, Texture2D rightSprite, Vector2 position, float maxHealth, float damage, float moveSpeed, float skitterSpeed, Texture2D bulletSprite)
        {
            this.leftTexture = leftSprite;
            this.rightTexture = rightSprite;
            this._sprite = rightSprite;
            this._position = position;
            this._maxHealth = maxHealth;
            this._health = maxHealth;
            this._damage = damage;
            this.bulletSprite = bulletSprite;
            this._velocity = new Vector2(moveSpeed, 0);
            startPos = new Vector2(_position.X, _position.Y);
            bulletSpeed = new Vector2(4, 0);
            counter = 0f;
            _height = _sprite.Height;
            _width = _sprite.Width;

            damaged = false;
            defaultSpeed = new Vector2(moveSpeed, 0);
            tileDetectionDistance = 10;

            enemyState = EnemyState.Idle;
            direction = directionState.Left;

            if(direction == directionState.Left)
            {
                _velocity.X *= -1;
            }
        }

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

        /// <summary>
        /// Deals with the logic for drawing the scorp
        /// I.E. dealing with direction changes and damaged
        /// </summary>
        /// <param name="sb">The sprite batch</param>
        /// <param name="screenOffset">The screen offset</param>
        public override void Draw(SpriteBatch sb, Vector2 screenOffset)
        {
            //Depending on the direction enum, either face the sprite left or right
            switch (direction)
            {
                case directionState.Left:
                        _sprite = leftTexture;
                    break;

                case directionState.Right:
                        _sprite = rightTexture;
                    break;
            }

            //Draw the sprite red if damaged
            if (!damaged)
            {
                sb.Draw(_sprite, _position - screenOffset, Color.White);
            }
            else
            {
                sb.Draw(_sprite, _position + screenOffset, Color.Red);
            }


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
        /// Handles the update logic for the scorpion
        /// </summary>
        /// <param name="tileMap">The list of tiles</param>
        /// <param name="projectiles">The list of projectiles</param>
        /// <param name="player">The player</param>
        /// <param name="gameTime"></param>
        public void Update(List<Tile> tileMap, List<IGameProjectile> projectiles, Player player, GameTime gameTime)
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

            
            switch (enemyState)
            {


                //Handles logic for before encountering the player.
                //The scorp will simply move back and forth between walls until the player comes within
                //range of it.
                case EnemyState.Idle:
                    Move();
                    ResolveTileCollisions(tileMap);

                    //If the player comes within range, increase the scorps speed and change to the 
                    //skitter state, where the scorp quickly runs away from the player and than
                    //gets ready to shoot at them
                    if (CheckAttackRange(player))
                    {
                        skitterEnd = false;


                        if (player.X < _position.X) //If the player is to the left of the scorp
                        {
                            _velocity.X = (Math.Abs(_velocity.X)) * 2;
                            direction = directionState.Right;
                        }
                        else if (player.X >= _position.X) //If the player is to the right/directly above
                        {
                            _velocity.X = (float)(Math.Abs(_velocity.X) * -1) * 2;
                            direction = directionState.Left;
                        }

                        enemyState = EnemyState.Skitter;
                        
                    }
                    break;



                    
                    //Handles the skitter logic. 
                    //Scorp goes until it either hits a wall or a ledge.
                case EnemyState.Skitter:

                    Move();
                    ResolveTileCollisions(tileMap);

                    
                    //Upon hitting a wall, detected by skitterEnd which is only turned on true once
                    //a horizontal collision has been detected
                    if(!CheckForLedge(direction, tileMap) || skitterEnd)
                    {
                        enemyState = EnemyState.Shooting;
                    }
                    break;



                    //Handles shooting logic.
                    //Once the scorp goes as far as it can, it stops, turns around, and
                    //begins looking for the player.
                    //If the player is in range, it will begin to shoot bullets at the player.
                case EnemyState.Shooting:
                    _velocity.X = 0;
                    Attack(player, projectiles, 1.0f, gameTime);


                    if(player.X < _position.X)
                    {
                        direction = directionState.Left;
                    }
                    else // if player x is greater than this.x
                    {
                        direction = directionState.Right;
                    }

                    ResolveTileCollisions(tileMap);
                    break;
            }
            
        }

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
                shootingTimer = timer;
                initialTimer = shootingTimer;
                /*
                if(player.X < _position.X && shootingTimer == timer)
                {
                    projectilesList.Add(new Bullet(bulletSprite, _position, 10, -2));
                }else if(player.X > _position.X && shootingTimer == timer)
                {
                    projectilesList.Add(new Bullet(bulletSprite, _position, 10, 2));
                }
                */
                
                //Shoots once every second
                counter += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (counter >= shootingTimer)
                {
                    shootingTimer = initialTimer;
                    counter = 0;

                    if (player.X < _position.X)
                    {
                        projectilesList.Add(new Bullet(bulletSprite, _position, 10, -2));
                    }
                    else if (player.X > _position.X)
                    {
                        projectilesList.Add(new Bullet(bulletSprite, _position, 10, 2));
                    }
                }
                
            }
        }

        /// <summary>
        /// Appeases the gods
        /// 
        /// The interface was being irritating so this was the fastest fix
        /// </summary>
        /// <param name="player"></param>
        public void Attack(Player player)
        {

        }

        
        /// <summary>
        /// Checks whether or not the player is in range for the scorpion to begin attacking
        /// </summary>
        /// <param name="player">The player</param>
        /// <returns>Whether or not the player is in range</returns>
        public bool CheckAttackRange(Player player)
        {
            if(
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


        /// <summary>
        /// Returns false if no ledge is detected
        /// </summary>
        /// <param name="direction">The direction of the enemy</param>
        /// <param name="tileMap">The tileMap</param>
        /// <returns>Whether or not there is a ledge</returns>
        public bool CheckForLedge(directionState direction, List<Tile> tileMap)
        {
            bool collisionDetected = false;
            
            //Searches through the tileMap and if it detects that the land ends, returns false
            //Otherwise it returns true upon a collision, indicating the lack of a ledge
            if(direction == directionState.Right)
            {
                foreach (Tile tile in tileMap)
                {
                    if (tile.HitBox.Intersects(new Rectangle((int)_position.X + 70, (int)_position.Y + 40, 4, 4))){
                        return collisionDetected = true;
                    }
                }
            }

            if (direction == directionState.Left)
            {
                foreach (Tile tile in tileMap)
                {
                    if (tile.HitBox.Intersects(new Rectangle((int)_position.X + 10, (int)_position.Y + 40, 5, 5))){
                        return collisionDetected = true;

                    }
                }
            }

            _velocity = Vector2.Zero;
            return collisionDetected = false;
        }
        

        /// <summary>
        /// Check for collision with another object
        /// </summary>
        /// <param name="other">The object to check against</param>
        /// <returns>Whether or not a collision was detected</returns>
        public bool CheckCollision(MobileEntity other)
        {
            return HitBox.Intersects(other.HitBox);
        }

        /// <summary>
        /// Returns true if the scorpion still has health
        /// </summary>
        /// <returns></returns>
        public bool CheckHealth()
        {
            if(_health <= 0)
            {
                
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Adds the velocity to the position
        /// </summary>
        public void Move()
        {
            _position += _velocity;
            

        }


        /// <summary>
        /// Resets the scorp
        /// </summary>
        public void Reset()
        {
            _health = _maxHealth;
            _position.X = startPos.X;
            _position.Y = startPos.Y;
            //isAlive = true;
            onGround = false;
            _velocity = new Vector2(defaultSpeed.X, 0);
        }

        /// <summary>
        /// Take damage
        /// </summary>
        /// <param name="damage">Amount of damage to take</param>
        public void TakeDamage(float damage, Player player)
        {
            if (!damaged)
            {
                _health -= damage;
                CheckHealth();
                timer = 0.5f;
                damaged = true;
                if (player.X < _position.X)
                {
                    //bump.X = 2;
                }
                else if (player.X > _position.X)
                {
                    //bump.X = -2;
                }

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
            //gravity is applied beforehand

            ApplyPhysics();

            // get the player's hitbox
            Rectangle hitBox = this.HitBox;

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
                Rectangle intersectRect = Rectangle.Intersect(hitBox, largest);

                //check for a horizontal collision (intersection is taller than it is wide)
                if (intersectRect.Width <= intersectRect.Height)
                {

                    //Fixes problem where very small and brief horizontal collisions cause reversal
                    if (intersectRect.Height >= 16)
                    {
                        if (direction == directionState.Left)
                        {
                            direction = directionState.Right;
                        }
                        else if (direction == directionState.Right)
                        {
                            direction = directionState.Left;
                        }


                        if (enemyState == EnemyState.Skitter)
                        {
                            skitterEnd = true;
                        }
                    }
                    // if the player X is less than (further left than) the intersection's x
                    // move the player left
                    if (this._position.X < intersectRect.X)
                    {
                        hitBox.X -= intersectRect.Width;

                        //the player's X velocity cannot be positive when touching the right wall
                        // this._velocity.X = Math.Clamp(_velocity.X, float.MinValue, 0);
                        


                        


                        if (intersectRect.Height >= 16)
                        {
                            //goingLeft = !goingLeft;
                            this._velocity.X *= -1;

                            
                            //_acceleration.X *= -1;

                        }


                    }
                    // otherwise move right
                    else
                    {
                        hitBox.X += intersectRect.Width;

                        //the player's X velocity cannot be negative when touching the left wall
                        //this._velocity.X = Math.Clamp(_velocity.X, 0, float.MaxValue);


                        if (intersectRect.Height >= 16)
                        {
                            //goingLeft = !goingLeft;
                            this._velocity.X *= -1;

                            
                            //_acceleration.X *= -1;
                        }


                    }

                    

                    //goingLeft = !goingLeft;



                    this._position.X = hitBox.X;
                }
                // otherwise this must be a vertical collision
                else
                {
                    // if the player Y is less than (further up than) the rectangle Y
                    // move the player up
                    if (this._position.Y < intersectRect.Y)
                    {
                        hitBox.Y -= intersectRect.Height;

                        // this means the player is in contact with the ground
                        onGround = true;

                        //the player's Y velocity cannot be negative when touching the ground
                        this._velocity.Y = Math.Clamp(_velocity.Y, float.MinValue, 0);
                    }
                    // otherwise the player has hit thier head, move down
                    else
                    {
                        hitBox.Y += intersectRect.Height;

                        //the player's Y velocity cannot be positive when touching the cieling
                        this._velocity.Y = Math.Clamp(_velocity.Y, 0, float.MaxValue);
                    }

                    this._position.Y = hitBox.Y;
                }

                // reset the intersections list and check again
                intersectionsList.Clear();

                // loop through the list of all tiles
                // to find which ones are interseting
                foreach (Tile tile in tileMap)
                {
                    // if the player is intersecting the tile
                    if (tile.HitBox.Intersects(hitBox))
                    {
                        // add it's hitbox to the list
                        intersectionsList.Add(tile.HitBox);
                    }
                }
            }
        }
    }
}
