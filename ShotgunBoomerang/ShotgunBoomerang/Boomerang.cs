﻿using Microsoft.Xna.Framework.Graphics;
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

        // Properties
        public BoomerangState CurrentState { get { return _currentState; } }

        // Constructor
        /// <summary>
        /// Creates a new boomerang with a given texture and position
        /// </summary>
        /// <param name="sprite">The boomerang's texture</param>
        /// <param name="position">The boomerang's position</param>
        public Boomerang(Texture2D sprite, Vector2 position)
        {
            _sprite = sprite;
            _position = position;

            _currentState = BoomerangState.Held;

            _acceleration = Vector2.Zero;
            _damage = 30;
            _health = 0;
            _maxHealth = 0;
            _velocity = Vector2.Zero;
        }

        /// <summary>
        /// Should allow the boomerang to bounce off walls
        /// </summary>
        /// <param name="tileMap">The current level's tiles</param>
        public void ResolveTileCollisions()
        {
            // take the tilemap currently in use
            List<Tile> tileMap = GameManager.currentTileMap;

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

        public void Draw(SpriteBatch sb, Player player)
        {
            // draw the boomerang on the screen offset by the screen's world offset
            sb.Draw(_sprite, _position - player.Position +
                new Vector2(GameManager.graphics.PreferredBackBufferWidth / 2 - player.Sprite.Width / 2,
                GameManager.graphics.PreferredBackBufferHeight / 2 - player.Sprite.Height / 2), Color.White);
        }

        public void hit()
        {
            throw new NotImplementedException();
        }

        public void Update(Player player)
        {
            switch(_currentState)
            {
                case BoomerangState.Held:
                    // while the boomerang is being held, it's position and velocity are the same as the player
                    _velocity = player.Velocity;
                    _position = player.Position;

                    // a transition to the Flying state will happen if the player right clicks once
                    if(GameManager.ms.RightButton == ButtonState.Pressed && 
                        GameManager.prevMs.RightButton == ButtonState.Released)
                    {
                        Vector2 mousePos = new Vector2(GameManager.ms.X, GameManager.ms.Y);

                        // velocity normal between the mouse and the player's centerpoint (center of the screen)
                        Vector2 velocityNormal = Vector2.Normalize(mousePos -
                            new Vector2(GameManager.graphics.PreferredBackBufferWidth / 2,
                           GameManager.graphics.PreferredBackBufferHeight / 2));

                        // add some velocity when the boomerang is leaving the player's hand
                        _velocity += velocityNormal * _damage;

                        _currentState = BoomerangState.Flying;
                    }
                    break;

                case BoomerangState.Flying:
                    // the boomerang will experience a slowing due to friction while in the air
                    float airFriction = 0.99f;

                    _velocity *= airFriction;

                    // The boomerang can bounce off walls while flying
                    ResolveTileCollisions();

                    // The boomerang will return to the player once it has reached a low enough speed
                    if(Math.Abs(_velocity.Length()) <= 0.1f)
                    {
                        _currentState = BoomerangState.Returning;
                    }

                    break;
                case BoomerangState.Returning:
                    // while the boomerang is returning, it does not experience friction or collide with walls
                    // it experiences constant acceleration towards the player

                    // velocity normal between the boomerang and the player's centerpoint (center of the screen)
                    Vector2 playerBoomerangNormal = Vector2.Normalize(
                            new Vector2(GameManager.graphics.PreferredBackBufferWidth / 2,
                           GameManager.graphics.PreferredBackBufferHeight / 2) - _position);

                    _acceleration = playerBoomerangNormal;

                    // apply physics to the boomerang
                    ApplyPhysics();

                    // if the boomerang intersects with the player, transition to held state
                    if(this.CheckCollision(player))
                    {
                        // the boomerang should no longer have acceleration once it is being held
                        _acceleration = Vector2.Zero;

                        _currentState = BoomerangState.Held;
                    }
                    break;
            }
        }

        protected override void ApplyPhysics()
        {
            _velocity += _acceleration;
            _position += _velocity;
        }
    }
}
