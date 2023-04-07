using Microsoft.Xna.Framework.Graphics;
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
    public enum BoomerangState
    {
        Flying,
        Returning,
        Held
    }
    /// <summary>
    /// A projectile thrown by the player which bounces off walls and returns to the player's hand
    /// </summary>
    internal class Boomerang : MobileEntity, IGameProjectile
    {
        // fields
        private BoomerangState _currentState;

        // Constructor
        public Boomerang(Texture2D sprite, Vector2 position)
        {
            _sprite = sprite;
            _position = position;

            _currentState = BoomerangState.Held;

            _acceleration = new Vector2();
            _damage = 30;
            _health = 0;
            _maxHealth = 0;
            _velocity = new Vector2();
        }

        public override void ResolveTileCollisions(List<Tile> tileMap)
        {
            //gravity is applied beforehand
            ApplyPhysics();

            // get the boomerang's hitbox
            Rectangle boomerangHitBox = this.HitBox;

            // temporary list for all intersections
            List<Rectangle> intersectionsList = new List<Rectangle>();

            // loop through the list of all tiles
            // to find which ones are interseting
            foreach (Tile tile in tileMap)
            {
                // if the boomerang is intersecting the tile
                if (tile.HitBox.Intersects(boomerangHitBox))
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
                Rectangle intersectRect = Rectangle.Intersect(boomerangHitBox, largest);

                //check for a horizontal collision (intersection is taller than it is wide)
                if (intersectRect.Width <= intersectRect.Height)
                {
                    // if the boomerang X is less than (further left than) the intersection's x
                    // move the boomerang left
                    if (this._position.X < intersectRect.X)
                    {
                        boomerangHitBox.X -= intersectRect.Width;
                    }
                    // otherwise move right
                    else
                    {
                        boomerangHitBox.X += intersectRect.Width;
                    }

                    // negate the boomerang's X velocity
                    _velocity.X *= -1;

                    this._position.X = boomerangHitBox.X;
                }
                // otherwise this must be a vertical collision
                else
                {
                    // if the boomerang Y is less than (further up than) the intersection Y
                    // move the boomerang up
                    if (this._position.Y < intersectRect.Y)
                    {
                        boomerangHitBox.Y -= intersectRect.Height;
                    }
                    // otherwise the boomerang has hit the cieling, move down
                    else
                    {
                        boomerangHitBox.Y += intersectRect.Height;
                    }

                    // negate the Y velocity
                    _velocity.Y *= -1;

                    this._position.Y = boomerangHitBox.Y;
                }

                // reset the intersections list and check again
                intersectionsList.Clear();

                // loop through the list of all tiles
                // to find which ones are interseting
                foreach (Tile tile in tileMap)
                {
                    // if the boomerang is intersecting the tile
                    if (tile.HitBox.Intersects(boomerangHitBox))
                    {
                        // add it's hitbox to the list
                        intersectionsList.Add(tile.HitBox);
                    }
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            throw new NotImplementedException();
        }

        public void hit()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        protected override void ApplyPhysics()
        {
            _velocity += _acceleration;
            _position += _velocity;
        }
    }
}
