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
    internal class Vegemite : MobileEntity , IGameProjectile
    {
        // fields
        private Vector2 _startPosition;

        // constructor
        /// <summary>
        /// Creates a new Vegemite healing item (can be given an initial velocity)
        /// technically counts as a projectile to fit into level framework.
        /// </summary>
        /// <param name="sprite">The vegemite's sprite</param>
        /// <param name="position">The vegemite's position</param>
        /// <param name="velocity">The vegemite's initial velocity</param>
        public Vegemite(Texture2D sprite, Vector2 position, Vector2 velocity)
        {
            _sprite = sprite;
            _position = position;
            _startPosition = position;
            _velocity = velocity;

            _width = _sprite.Width;
            _height = _sprite.Height;

            _acceleration = Vector2.Zero;

            // in this case, damage will be used to heal
            _damage = 40;

            _health = 0;
            _maxHealth = 0;
        }

        /// <summary>
        /// A leftover from the vegemite counting as a projectile,
        /// the player can toss the health items by shooting them
        /// </summary>
        /// <param name="shotgunNormal"></param>
        public void ShotgunHit(Vector2 shotgunNormal)
        {
            // throw the vegemite  in the  direction of the shotgun blast
            _velocity = shotgunNormal * _velocity.Length() * 2;
        }

        public override void Update(
            Level currentLevel,
            Player player,
            GameTime gameTime)
        {
            float groundFriction = 0.99f;

            ResolveTileCollisions(currentLevel.CurrentTileMap);

            // slow down over time
            _velocity *= groundFriction;

            // if the player intersects the healing item, add to thier health and remove the vegemite
            if(CheckCollision(player))
            {
                player.Health += _damage;
                currentLevel.CurrentProjectiles.Remove(this);
            }
        }

        /// <summary>
        /// Resolves collisions with tiles so that the vegemite 
        /// collides with them properly
        /// </summary>
        /// <param name="tileMap">The list of tiles in the currently loaded level</param>
        protected override void ResolveTileCollisions(List<Tile> tileMap) 
        {
            // gravity is applied beforehand
            ApplyPhysics();

            // get the vegemite's hitbox
            Rectangle vegemiteHitBox = this.HitBox;

            // temporary list for all intersections
            List<Rectangle> intersectionsList = new List<Rectangle>();

            // loop through the list of all tiles
            // to find which ones are interseting
            foreach (Tile tile in tileMap)
            {
                // if the vegemite is intersecting the tile
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
                Rectangle intersectRect = Rectangle.Intersect(vegemiteHitBox, largest);

                //check for a horizontal collision (intersection is taller than it is wide)
                if (intersectRect.Width <= intersectRect.Height)
                {
                    // if the vegemite X is less than (further left than) the intersection's x
                    // move the vegemite left
                    if (this._position.X < intersectRect.X)
                    {
                        vegemiteHitBox.X -= intersectRect.Width;

                        //the vegemite's X velocity cannot be positive when touching the right wall
                        this._velocity.X = Math.Clamp(_velocity.X, float.MinValue, 0);
                    }
                    // otherwise move right
                    else
                    {
                        vegemiteHitBox.X += intersectRect.Width;

                        //the vegemite's X velocity cannot be negative when touching the left wall
                        this._velocity.X = Math.Clamp(_velocity.X, 0, float.MaxValue);
                    }

                    this._position.X = vegemiteHitBox.X;
                }
                // otherwise this must be a vertical collision
                else
                {
                    // if the vegemite Y is less than (further up than) the rectangle Y
                    // move the vegemite up
                    if (this._position.Y < intersectRect.Y)
                    {
                        vegemiteHitBox.Y -= intersectRect.Height;

                        //the vegemite's Y velocity cannot be negative when touching the ground
                        this._velocity.Y = Math.Clamp(_velocity.Y, float.MinValue, 0);
                    }
                    // otherwise the vegemite has hit the cieling, move down
                    else
                    {
                        vegemiteHitBox.Y += intersectRect.Height;

                        //the vegemite's Y velocity cannot be positive when touching the cieling
                        this._velocity.Y = Math.Clamp(_velocity.Y, 0, float.MaxValue);
                    }

                    this._position.Y = vegemiteHitBox.Y;
                }

                // reset the intersections list and check again
                intersectionsList.Clear();

                // loop through the list of all tiles
                // to find which ones are interseting
                foreach (Tile tile in tileMap)
                {
                    // if the vegemite is intersecting the tile
                    if (tile.HitBox.Intersects(vegemiteHitBox))
                    {
                        // add it's hitbox to the list
                        intersectionsList.Add(tile.HitBox);
                    }
                }
            }
        }

        /// <summary>
        /// Resets the item to it's initial values for level restarting
        /// </summary>
        public void Reset()
        {
            _position = _startPosition;
            _velocity = Vector2.Zero;
        }
    }
}
