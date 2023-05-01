using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShotgunBoomerang
{

    /// <summary>
    /// Idle is when a player is not detected, the Koala just moves back and forth
    /// 
    /// Following is when a player has been detected, but isn't in range, and the koala moves after the player
    /// 
    /// Charging is when the Koala has been damaged, and charges after the player for a collision attack
    /// 
    /// Shooting is when a player is in range, so the Koala, slowly moves towards them while shooting
    /// </summary>
    enum KoalaState
    {
        Idle,
        Following,
        Charging,
        Shooting
    }


    internal class Koala : MobileEntity, IGameEnemy
    {

        private Texture2D leftSprite;
        private Texture2D rightSprite;
        private Texture2D bulletSprite;

        private float shotTimer;
        private float initialTimer;
        private float damagedTimer;
        private float defaultSpeed;
        private float contactDamage;

        private int horizontalDetectionDistance;
        private int verticalDetectionDistance;

        private int shootingRange;

        private bool damaged;
        private bool hasCharged;

        private KoalaState koalaState;

        private Vector2 startPos;

        private Direction direction;


        public Koala(Texture2D leftSprite, Texture2D rightSprite, Vector2 position, float maxHealth, float damage, float contactDamage, float moveSpeed, float shotTimer, Texture2D bulletSprite)
        {
            this.leftSprite = leftSprite;
            this.rightSprite = rightSprite;
            this._position = position;
            this._maxHealth = maxHealth;
            this._health = maxHealth;
            this._damage = damage;
            this.contactDamage = contactDamage;
            this._sprite = rightSprite;
            this.shotTimer = shotTimer;
            this.initialTimer = shotTimer;
            this.bulletSprite = bulletSprite;
            this._velocity = new Vector2(moveSpeed, 0);
            this.defaultSpeed = moveSpeed;
            this._width = 128;
            this._height = 64;
            horizontalDetectionDistance = 10;
            verticalDetectionDistance = 5;
            shootingRange = 6;

            startPos = new Vector2(_position.X, _position.Y);
            direction = Direction.Right;
            koalaState = KoalaState.Idle;

            damaged = false;
            hasCharged = false;
        }

        /// <summary>
        /// Koala has 4 states. These are idle, following, shooting, and charging.
        /// While idle the koala patrols back and forth.
        /// While following the koala moves after the player and moves slightly faster
        /// While shooting the koala moves slightly slower, but periodically shoots
        /// projectiles
        /// When the koala is damaged for the first time, it gains a burst of speed during it's I-frames
        /// This only happens once.
        /// </summary>
        /// <param name="currentLevel"></param>
        /// <param name="player"></param>
        /// <param name="gameTime"></param>
        public void Update(
            Level currentLevel,
            Player player,
            GameTime gameTime
            )
        {
            /*
            if(player.X < _position.X)
            {
                direction = Direction.Left;
            }
            else if(player.X >= _position.X)
            {
                direction = Direction.Right;
            }
            */



            
            switch (koalaState)
            {
                //While the koala is idle, it moves back and forth between walls until the player
                //Comes into range
                case KoalaState.Idle:

                    ApplyPhysics();
                    Move();

                    //State change logic:
                    //If the player is within the detection distance, begin following them
                    if(
                        player.X >= _position.X - (64 * horizontalDetectionDistance)
                        && player.X <= _position.X + (64 * horizontalDetectionDistance)
                        && player.Y <= _position.Y + (64 * verticalDetectionDistance)
                        && player.Y >= _position.Y - (64 * verticalDetectionDistance))
                    {
                        koalaState = KoalaState.Following;
                    }
                    break;


                //Once the player has come into range, begin moving after them
                case KoalaState.Following:

                    ChasePlayer(player);
                    ApplyPhysics();
                    Move();
                    if (
                        player.X >= _position.X - (64 * shootingRange)
                        && player.X <= _position.X + (64 * shootingRange)
                        && player.Y <= _position.Y + 64
                        && player.Y >= _position.Y - (64 * verticalDetectionDistance))
                    {
                        koalaState = KoalaState.Shooting;
                    }

                    if (damaged && !hasCharged)
                    {
                        hasCharged = true;
                        koalaState = KoalaState.Charging;
                    }

                    if(Vector2.Distance(player.Position, _position) > (horizontalDetectionDistance * 64))
                    {
                        koalaState = KoalaState.Idle;
                    }
                    break;


                //If the player damages the koala, charge at the player.
                //If the player is hit by the charge, stop immediately and begin shooting
                //If the charge misses, continue for a short distance
                case KoalaState.Charging:

                    _velocity.X = defaultSpeed * 3;
                    ApplyPhysics();
                    Move();

                    if (CheckCollision(player))
                    {
                        player.TakeHit(this, contactDamage);
                        koalaState = KoalaState.Shooting;

                    }
                    break;


                //While the player is within range, continously shoot at them.
                case KoalaState.Shooting:

                    Attack(player, currentLevel.CurrentProjectiles, 5, 20, 1.0f, gameTime);

                    _velocity.X = defaultSpeed / 2;
                    ApplyPhysics();
                    Move();

                    if (
                        !(player.X >= _position.X - (64 * shootingRange)
                        && player.X <= _position.X + (64 * shootingRange)
                        && player.Y >= _position.Y + 64
                        && player.Y <= _position.Y - (64 * verticalDetectionDistance)))
                    {
                        koalaState = KoalaState.Following;
                    }

                    if (damaged && !hasCharged)
                    {
                        hasCharged = true;
                        koalaState = KoalaState.Charging;
                    }
                    break;
            }

            
            _velocity.X = defaultSpeed;
            //Move();

            if (!CheckHealth())
            {
                currentLevel.CurrentEnemies.Remove(this);
                player.Kills++;
            }


            if (damagedTimer > 0 && damaged)
            {
                damagedTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                damaged = false;
                damagedTimer = 0.5f;
                if(koalaState == KoalaState.Charging)
                {
                    koalaState = KoalaState.Following;
                }
            }
            CheckPlayerContact(player);
            //ApplyPhysics();
            ResolveTileCollisions(currentLevel.CurrentTileMap);
            


        }

        /// <summary>
        /// Draws the koala
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="screenOffset"></param>
        public override void Draw(SpriteBatch sb, Vector2 screenOffset)
        {

            switch (direction)
            {
                case Direction.Left:
                    _sprite = leftSprite;
                    break;

                case Direction.Right:
                    _sprite = rightSprite;
                    break;
            }


            if (damaged)
            {
                sb.Draw(_sprite, _position - screenOffset, Color.Red);
            }
            else
            {
                sb.Draw(_sprite, _position - screenOffset, Color.White);
            }
            
        }



        /// <summary>
        /// Moves the koala back and forth between walls
        /// </summary>
        public void Move()
        {
            if (direction == Direction.Right)
            {
                _position.X += _velocity.X;
            }
            else if (direction == Direction.Left)
            {
                _position.X -= _velocity.X;
            }
            
        }

        /// <summary>
        /// Chase after the player
        /// </summary>
        /// <param name="player"></param>
        public void ChasePlayer(Player player)
        {
            if(player.X > _position.X)
            {
                direction = Direction.Right;
            }else if(player.X < _position.X)
            {
                direction = Direction.Left;
            }
            _velocity.X = defaultSpeed * 1.5f;
        }


        


        /// <summary>
        /// Resets the koala
        /// </summary>
        public void Reset()
        {
            _position.X = startPos.X;
            _position.Y = startPos.Y;
            _health = _maxHealth;
        }

        /// <summary>
        /// Take damage
        /// </summary>
        /// <param name="damage">Amount of damage to take</param>
        public void TakeHit(GameObject attacker, float damage)
        {
            if (!damaged)
            {
                _health -= damage;
                damagedTimer = 0.5f;
                damaged = true;

                
            }
        }

        /// <summary>
        /// Checks for a collision with the player and deals contact damage if one is detected
        /// </summary>
        /// <param name="player">The player</param>
        public void CheckPlayerContact(Player player)
        {
            if (CheckCollision(player))
            {
                player.TakeHit(this, contactDamage);
            }
        }

        /// <summary>
        /// Returns true if koala still has health
        /// </summary>
        /// <returns></returns>
        public bool CheckHealth()
        {
            if (_health > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Fires aimed aimed shots at the player based on a timer
        /// </summary>
        /// <param name="player">The player to shoot at</param>
        /// <param name="projectilesList">The list of projectiles</param>
        /// <param name="damage">How much damage the shot should do</param>
        /// <param name="timer">The time between shots</param>
        /// <param name="gameTime">Game time</param>
        public void Attack(Player player, List<IGameProjectile> projectilesList, float damage, float timer, GameTime gameTime)
        {
            // if the timer is still running do nothing
            if (shotTimer >= 0)
            {
                shotTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            // when the timer is up, shoot and reset the timer
            else
            {
                projectilesList.Add(
                    new Bullet(
                        bulletSprite,
                        CenterPoint,
                        Vector2.Normalize(player.Position - _position) * 10));
                shotTimer = timer;
            }
        }


        /// <summary>
        /// Fires an unaimed shot
        /// </summary>
        /// <param name="player"></param>
        /// <param name="projectilesList"></param>
        /// <param name="speed"></param>
        /// <param name="damage"></param>
        /// <param name="timer"></param>
        /// <param name="gameTime"></param>
        public void Attack(Player player, List<IGameProjectile> projectilesList, float speed, float damage, float timer, GameTime gameTime)
        {
            // if the timer is still running do nothing
            if (shotTimer >= 0)
            {
                shotTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            // when the timer is up, shoot and reset the timer
            else
            {
                if(player.X > _position.X)
                {
                    projectilesList.Add(
                    new Bullet(
                        bulletSprite,
                        CenterPoint,
                        speed
                        ));
                }
                else
                {
                    projectilesList.Add(
                    new Bullet(
                        bulletSprite,
                        CenterPoint,
                        -(speed)
                        ));
                }
                
                shotTimer = initialTimer;
            }
        }


        /// <summary>
        /// Override of Mobile Entity method
        /// </summary>
        protected override void ApplyPhysics()
        {
            // apply gravity to velocity
            _velocity.Y += GameManager.Gravity;

            // apply velocity to position
            _position.Y += _velocity.Y;
        }

        /// <summary>
        /// Allows the koala to properly collide with surfaces
        /// </summary>
        /// <param name="tileMap">list of tiles in the level</param>
        protected override void ResolveTileCollisions(List<Tile> tileMap)
        {

            //ApplyPhysics();

            // get a copy of the enemy's hitbox
            Rectangle enemyHitBox = this.HitBox;

            // temporary list for all intersections
            List<Rectangle> intersectionsList = new List<Rectangle>();

            // loop through the list of all tiles
            // to find which ones are interseting
            foreach (Tile tile in tileMap)
            {
                // if the enemy is intersecting the tile
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
                Rectangle intersectRect = Rectangle.Intersect(HitBox, largest);

                //check for a horizontal collision (intersection is taller than it is wide)
                //Also checks that the collision is greater than 1/4 the size of a tile,
                //in case it's just a brief intersection due to colliding before a vertical
                //collision is resolved
                if (intersectRect.Width <= intersectRect.Height && intersectRect.Height >= 16)
                {
                    // if the enemy X is less than (further left than) the intersection's x
                    // move the enemy left
                    if (this._position.X < intersectRect.X)
                    {
                        enemyHitBox.X -= intersectRect.Width;

                        //the enemy's X velocity cannot be positive when touching the right wall
                        switch (direction)
                        {
                            case Direction.Left:
                                direction = Direction.Right;
                                break;

                            case Direction.Right:
                                direction = Direction.Left;
                                break;
                        }
                        this._velocity.X = Math.Clamp(_velocity.X, float.MinValue, 0);
                    }
                    // otherwise move right
                    else
                    {
                        enemyHitBox.X += intersectRect.Width;

                        //the enemy's X velocity cannot be negative when touching the left wall
                        switch (direction)
                        {
                            case Direction.Left:
                                direction = Direction.Right;
                                break;

                            case Direction.Right:
                                direction = Direction.Left;
                                break;
                        }
                        this._velocity.X = Math.Clamp(_velocity.X, 0, float.MaxValue);
                    }

                    this._position.X = enemyHitBox.X;
                }
                // otherwise this must be a vertical collision
                else
                {
                    // if the enemy Y is less than (further up than) the rectangle Y
                    // move the enemy up
                    if (this._position.Y < intersectRect.Y)
                    {
                        enemyHitBox.Y -= intersectRect.Height;

                        //the enemy's Y velocity cannot be negative when touching the ground
                        this._velocity.Y = Math.Clamp(_velocity.Y, float.MinValue, 0);
                    }
                    // otherwise the player has hit thier head, move down
                    else
                    {
                        enemyHitBox.Y += intersectRect.Height;

                        //the enemy's Y velocity cannot be positive when touching the cieling
                        this._velocity.Y = Math.Clamp(_velocity.Y, 0, float.MaxValue);
                    }

                    this._position.Y = enemyHitBox.Y;
                }

                // reset the intersections list and check again
                intersectionsList.Clear();

                // loop through the list of all tiles
                // to find which ones are interseting
                foreach (Tile tile in tileMap)
                {
                    // if the enemy is intersecting the tile
                    if (tile.HitBox.Intersects(enemyHitBox))
                    {
                        // add it's hitbox to the list
                        intersectionsList.Add(tile.HitBox);
                    }
                }
            }

            if (direction == Direction.Left)
            {
                Console.WriteLine();
            }
        }
    }
}
