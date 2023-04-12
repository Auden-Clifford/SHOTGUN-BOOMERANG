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
    internal class SnakeEnemy : MobileEntity, IGameEnemy
    {
        private bool goingLeft;
        private bool _isCollidingWithGround;
        private List<Rectangle> intersectionHitboxes;
        private Vector2 bump;
        private bool isAlive;

        public SnakeEnemy(Texture2D sprite, Vector2 position, float maxHealth, float damage)

        {
            this._sprite = sprite;
            this._position = position;
            this._maxHealth = maxHealth;
            this._health = maxHealth;
            this._damage = damage;

            _velocity = new Vector2(2, 0);
            goingLeft = false;
            _isCollidingWithGround = false;
            intersectionHitboxes = new List<Rectangle>();
            bump = new Vector2(2, -3);
            isAlive = true;
        }

        
        /// <summary>
        /// Draws the snake
        /// </summary>
        /// <param name="sb">The sprite batch</param>
        /// <param name="offset">The screenoffset</param>
        public override void Draw(SpriteBatch sb, Vector2 offset)
        {

            sb.Draw(Sprite, _position - offset, Color.White);

        }

        /// <summary>
        /// Handles the update logic for the snake
        /// </summary>
        /// <param name="tileMap">The level map</param>
        /// <param name="projectiles">The list of projectiles</param>
        /// <param name="player">The player</param>
        public void Update(
            List<Tile> tileMap,
            List<IGameProjectile> projectiles,
            Player player)
        {
            if(_velocity.X != 2)
            {
                _velocity.X = 2;
            }
            Attack(player);
            Move();
            ResolveTileCollisions(tileMap);


        }


        /// <summary>
        /// Checks for a collision with another object
        /// </summary>
        /// <param name="other">The object to check against</param>
        /// <returns>Whether or not a collision was detected</returns>
        public bool CheckCollision(MobileEntity other)
        {
            if (this.HitBox.Intersects(other.HitBox))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// If a collision is detected against the player, damage and bump them
        /// </summary>
        /// <param name="player">The player</param>
        public void Attack(Player player)
        {
            if (CheckCollision(player))
            {
                player.Health -= _damage;
                if(player.Position.X >= this.Position.X)
                {
                    bump.X = 2;
                }
                else //If player is to the left of the enemy
                {
                    bump.X = -2;
                }

                player.Velocity += bump;
            }
        }

        /// <summary>
        /// Take damage
        /// </summary>
        /// <param name="damage">Amount of damage to take</param>
        public void TakeDamage(float damage)
        {
            _health -= damage;
        }

        /// <summary>
        /// Checks health to see if the enemy has died
        /// </summary>
        public void CheckHealth()
        {
            if(_health <= 0)
            {
                isAlive = false;
                
            }
        }

        /// <summary>
        /// Gets or sets the X of the enemy
        /// </summary>
        public float X
        {
            get { return this._position.X; }
            set { this._position.X = value; }
        }

        /// <summary>
        /// Moves the enemy automatically, switching directions if the enemy collides with a wall
        /// </summary>
        public void Move()
        {
            
            if (!goingLeft) //If going right
            {
                _position += _velocity;
            }
            else //If going left
            {
                _position -= _velocity;
            }

            

            
        }

        
        /// <summary>
        /// Definitely not just Auden's code I took cause it wasn't necessary to do the work again
        /// That would be insane (:
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
                    intersectionHitboxes.Add(tile.HitBox);
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
                    // if the player X is less than (further left than) the intersection's x
                    // move the player left
                    if (this._position.X < intersectRect.X)
                    {
                        hitBox.X -= intersectRect.Width;

                        //the player's X velocity cannot be positive when touching the right wall
                        this._velocity.X = Math.Clamp(_velocity.X, float.MinValue, 0);
                        if (intersectRect.Height >= 16)
                        {
                            goingLeft = !goingLeft;
                        }
                    }
                    // otherwise move right
                    else
                    {
                        hitBox.X += intersectRect.Width;

                        //the player's X velocity cannot be negative when touching the left wall
                        this._velocity.X = Math.Clamp(_velocity.X, 0, float.MaxValue);
                        if (intersectRect.Height >= 16)
                        {
                            goingLeft = !goingLeft;
                        }
                    }

                    

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
                        _isCollidingWithGround = true;

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
