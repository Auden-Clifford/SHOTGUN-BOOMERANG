using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Reflection.Metadata;

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
        private float _drawAngle;


        // Properties

        /// <summary>
        /// Gets the current state of the boomerang
        /// </summary>
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

            _drawAngle = 0;
        }


        // Methods

        /// <summary>
        /// Draw override for the boomerang, allows the 
        /// boomerang to spin based on how fast its going
        /// </summary>
        /// <param name="sb">Spritebatch in use</param>
        /// <param name="screenOffset">offset of the screen coordinates from the world coordinates</param>
        public override void Draw(SpriteBatch sb, Vector2 screenOffset)
        {
            // add to the angle (spins faster when moving faster)
            _drawAngle += (_velocity.Length()) * (MathF.PI / 180);

            sb.Draw(
                _sprite,
                // ensure the sprite is drawn from the center of the hitbox
                _position - screenOffset + new Vector2(_width / 2, Height / 2), 
                null,
                Color.White,
                _drawAngle,
                // rotate around the texture's center
                new Vector2(_width / 2, _height / 2),
                1, // same scale
                SpriteEffects.None,
                0.0f);
        }

        /// <summary>
        /// Base method for use in the update loop, should contain all logic the object needs to go through 
        /// in a frame as well as any parameters from the game manager that might be needed for this logic. 
        /// Update will be the entry point for all data from Game manager to the other classes
        /// -- The boomerang's update should allow it to fly around the level 
        /// bouncing off walls, returning to the player once it slows down
        /// </summary>
        /// <param name="kb">The keyboard state this frame</param>
        /// <param name="prevKb"> The keyboard state last frame</param>
        /// <param name="ms">The mouse state this frame</param>
        /// <param name="prevMs">The mouse state last frame</param>
        /// <param name="tileMap">The current level's tiles</param>
        /// <param name="enemies">The current level's enemies</param>
        /// <param name="projectiles">The projectiles currently in play</param>
        /// <param name="player">The player</param>
        /// <param name="gameTime">tracks in-game time intervals</param>
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

                case BoomerangState.Flying:

                    _velocity *= airFriction;

                    ApplyPhysics();

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

                    // the boomerang should be removed if it touches the player or if
                    // it has been more then 10 seconds since the boomerang was thrown
                    if (this.CheckCollision(player))
                    {
                        // the tell the player that the boomerang has returned
                        player.IsHoldingBoomerang = true;
                        // the boomerang should remove itself from the game when it hits the player
                        projectiles.Remove(this);
                    }
                    break;
            }
        }


        // Helper methods

        /// <summary>
        /// Should allow the boomerang to bounce off walls
        /// </summary>
        /// <param name="tileMap">The current level's tiles</param>
        protected override void ResolveTileCollisions(List<Tile> tileMap)
        {
            // get the boomerang's hitbox
            Rectangle boomerangHitBox = this.HitBox;

            // temporary list for all intersections
            List<Rectangle> intersectionsList = new List<Rectangle>();

            // loop through the list of all tiles
            // to find which ones are interseting
            foreach (Tile tile in tileMap)
            {
                // if the boomerang is intersecting the tile
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
                    // otherwise the boomerang has hit the ceiling, move down
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

        /// <summary>
        /// Applies acceleration to velocity and velocity to 
        /// position; the boomerang is not effected by gravity
        /// </summary>
        protected override void ApplyPhysics()
        {
            _velocity += _acceleration;
            _position += _velocity;
        }


        /// <summary>
        /// parrys the boomerang forcing it away 
        /// from player and back into the flying state
        /// </summary>
        public void ShotgunHit(Vector2 shotgunNormal)
        {
            // throw the boomerang  in the  direction of the shotgun blast
            _velocity = shotgunNormal * _velocity.Length() * 2;

            // reset the acceleration
            _acceleration = Vector2.Zero;

            // go back into the flying state
            _currentState = BoomerangState.Flying;
        }

        /// <summary>
        /// The boomerang should never be in play at the beginning of the level 
        /// so this method is uneccisary and only a leftover from IGameProjectile
        /// </summary>
        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
