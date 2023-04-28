using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ShotgunBoomerang
{
    public enum BoomerangState
    {
        Flying,
        Returning
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
        public Boomerang(Texture2D sprite, Vector2 position, Vector2 velocity)
        {
            _sprite = sprite;
            _position = position;
            _width = sprite.Width;
            _height = sprite.Height;

            _velocity = velocity;

            _currentState = BoomerangState.Flying;

            _acceleration = Vector2.Zero;
            _damage = 1;
            _health = 0;
            _maxHealth = 0;
        }

        /// <summary>
        /// Should allow the boomerang to bounce off walls
        /// </summary>
        /// <param name="tileMap">The current level's tiles</param>
        protected override void ResolveTileCollisions(List<Tile> tileMap)
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

        
        public override void Draw(SpriteBatch sb, Vector2 screenOffset)
        {
            base.Draw(sb, screenOffset);
        }
        

        public void HitEntity()
        {
            throw new NotImplementedException();
        }

        public override void Update(
            KeyboardState kb,
            KeyboardState prevKb,
            MouseState ms,
            MouseState prevMs,
            List<Tile> tileMap,
            List<IGameEnemy> enemies,
            List<IGameProjectile> projectiles,
            Player player,
            GameTime gameTime)
        {
            //detetcts enemy collisions
            for (int i = 0; i < enemies.Count; i++)
            {
                MobileEntity enemey = (MobileEntity) enemies[i];
                if (this.CheckCollision(enemey))
                {
                    enemies[i].TakeHit(this, _damage * _velocity.Length());
                }
            }

            // the boomerang will experience a slowing due to friction while in the air
            float airFriction = 0.99f;

            switch (_currentState)
            {
                /*
                case BoomerangState.Held:
                    // while the boomerang is being held, it's position and velocity are the same as the player
                    _velocity = player.Velocity;
                    _position = player.Position;

                    // a transition to the Flying state will happen if the player right clicks once
                    if(ms.RightButton == ButtonState.Pressed && 
                        prevMs.RightButton == ButtonState.Released)
                    {
                        Vector2 mousePos = new Vector2(ms.X, ms.Y);

                        // velocity normal between the mouse and the player's centerpoint
                        Vector2 velocityNormal = Vector2.Normalize(mousePos -
                            new Vector2(player.Position.X + player.Sprite.Width / 2,
                           player.Position.Y + player.Sprite.Height / 2));

                        // add some velocity when the boomerang is leaving the player's hand
                        _velocity += velocityNormal * _damage;

                        _currentState = BoomerangState.Flying;
                    }
                    break;
                */

                case BoomerangState.Flying:

                    _velocity *= airFriction;

                    // The boomerang can bounce off walls while flying
                    ResolveTileCollisions(tileMap);

                    // The boomerang will return to the player once it has reached a low enough speed
                    if(Math.Abs(_velocity.Length()) <= 20)
                    {
                        _currentState = BoomerangState.Returning;
                    }

                    break;
                case BoomerangState.Returning:
                    // while the boomerang is returning, it does not experience friction or collide with walls
                    // it experiences constant acceleration towards the player

                    // velocity normal between the boomerang and the player's centerpoint
                    Vector2 playerBoomerangNormal = Vector2.Normalize(
                            new Vector2(player.Position.X + player.Sprite.Width / 2,
                           player.Position.Y + player.Sprite.Height / 2) - _position);

                    _acceleration = playerBoomerangNormal * 2;

                    _velocity *= airFriction;

                    // apply physics to the boomerang
                    ApplyPhysics();

                    // if the boomerang intersects with the player, go back to the player's hand
                    if(this.CheckCollision(player))
                    {
                        // the tell the player that the boomerang has returned
                        player.IsHoldingBoomerang = true;
                        // the boomerang should remove itself from the game when it hits the player
                        projectiles.Remove(this);
                    }
                    //if()
                    break;
            }
        }

        protected override void ApplyPhysics()
        {
            _velocity += _acceleration;
            _position += _velocity;
        }


        /// <summary>
        /// parrys boomerang
        /// forcing it away from player
        /// </summary>
        public void ShotgunHit(Vector2 shotgunNormal)
        {
            // throw the boomerang  in the  direction of the shotgun blast
            _velocity = shotgunNormal * _velocity.Length() * 2;

            // go back into the flying state
            _currentState = BoomerangState.Flying;
        }

        public void Reset()
        {
            // the boomerang is not reset when the level is
            // reset as it is not in the original level list
        }
    }
}
