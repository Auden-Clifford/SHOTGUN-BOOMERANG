using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShotgunBoomerang
{
    internal class KoalaTree : MobileEntity, IGameEnemy
    {

        private Direction direction = Direction.Right;
        private Texture2D leftTexture;
        private Texture2D rightTexture;
        private bool damaged;
        private int tileDetectionDistance;
        private Texture2D bulletSprite;
        private float shootingTimer;
        private float counter;
        private float initialTimer;
        private float damagedTimer;


        public KoalaTree(Texture2D leftSprite, Texture2D rightSprite, Vector2 position, float maxHealth, float damage, Texture2D bulletSprite)
        {
            this._sprite = leftSprite;
            this._position = position;
            this._maxHealth = maxHealth;
            this._health = maxHealth;
            this._damage = damage;
            this.bulletSprite = bulletSprite;
            this._height = 128;
            this._width = 128;

            leftTexture = leftSprite;
            rightTexture = rightSprite;
            tileDetectionDistance = 8;
            counter = 0;

        }

        public override void Update(
            Level currentLevel,
            Player player,
            GameTime gameTime
            )
        {
            if(player.X < _position.X)
            {
                direction = Direction.Left;

            }else if(player.Y >= _position.Y)
            {
                direction = Direction.Right;
            }

            Attack(player, currentLevel.CurrentProjectiles, 10, 1.0f, gameTime);
            if (!CheckHealth())
            {
                currentLevel.CurrentEnemies.Remove(this);
            }
            ResolveTileCollisions(currentLevel.CurrentTileMap);
        }


        public override void Draw(SpriteBatch sb, Vector2 screenOffset)
        {
            //Depending on the direction enum, either face the sprite left or right
            
            switch (direction)
            {
                case Direction.Left:
                    _sprite = leftTexture;
                    break;

                case Direction.Right:
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
                sb.Draw(_sprite, _position - screenOffset, Color.Red);
                damagedTimer -= 0.1f;
                if(damagedTimer < 0)
                {
                    damaged = false;
                }
            }
        }


        /// <summary>
        /// Enemy is stationary.
        /// This function exists only to satisfy the interface
        /// </summary>
        public void Move()
        {
            //Enemy does not move
        }

        public bool CheckCollision(MobileEntity other)
        {
            return true;
        }

        /// <summary>
        /// Resets the scorp
        /// </summary>
        public void Reset()
        {
            _health = _maxHealth;
            //isAlive = true;
            //onGround = false;
        }

        /// <summary>
        /// Apply damage to the enemy
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="player"></param>
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
        /// Checks current health and returns true if the koala has health left and false if it doesn't
        /// </summary>
        /// <returns></returns>
        public bool CheckHealth()
        {
            if(_health > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Appeases the gods
        /// 
        /// Appeases conflict between interface and inheritance
        /// </summary>
        /// <param name="player">The player</param>
        public void Attack(Player player)
        {
            
        }

        public void Attack(Player player, List<IGameProjectile> projectilesList, float damage, float timer, GameTime gameTime)
        {
            //If the player is within range, begin the timer
            //Once it goes off, reset the timer and shoot a projectile
            if (CheckRange(player))
            {
                shootingTimer = timer;
                initialTimer = shootingTimer;

                //Shoots once every second
                counter += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (counter >= shootingTimer)
                {
                    shootingTimer = initialTimer;
                    counter = 0;

                    // normal vector between the player and the koala
                    Vector2 velocityNormal = Vector2.Normalize(player.Position - _position);

                    projectilesList.Add(new Bullet(bulletSprite, CenterPoint, velocityNormal * 10));
                }


            }
        }

        //Checks if the player is within the desired bounds around the enemy.
        //Can be tweaked by changing the tileDetectionDistance assigned in the constructor
        public bool CheckRange(Player player)
        {
            if (
                player.X > _position.X - (tileDetectionDistance * 64) //Checks if the player is on the left side
                &&
                player.X < _position.X + (tileDetectionDistance * 64) //Checks if the player is on the right side
                &&
                player.Y <= _position.Y + (64) //Checks if the player isn't too low
                &&
                player.Y >= _position.Y - (tileDetectionDistance * 64) //Checks if the player isn't too high
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
                Rectangle intersectRect = Rectangle.Intersect(hitBox, largest);

                //check for a horizontal collision (intersection is taller than it is wide)
                if (intersectRect.Width <= intersectRect.Height)
                {

                    //Fixes problem where very small and brief horizontal collisions cause reversal
                    if (intersectRect.Height >= 16)
                    {
                        if (direction == Direction.Left)
                        {
                            direction = Direction.Right;
                        }
                        else if (direction == Direction.Right)
                        {
                            direction = Direction.Left;
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
                        //onGround = true;

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
