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
    internal class Bullet : MobileEntity, IGameProjectile
    {
        // Fields
        bool aimedShot;
        bool parried;


        // Constructors

        /// <summary>
        /// Creates a new bullet moving horizontally at a specified speed
        /// </summary>
        /// <param name="sprite">The sprite to draw using</param>
        /// <param name="position">Where it's being spawned</param>
        /// <param name="damage">How much damage it does</param>
        /// <param name="moveSpeed">How fast to move. Value also determines direction</param>
        public Bullet(Texture2D sprite, Vector2 position, Vector2 velocity)
        {
            _sprite = sprite;
            _position = position;
            _velocity = velocity;

            _width = _sprite.Width;
            _height = _sprite.Height;

            aimedShot = true;
            parried = false;

            _damage = 10;

            // initialize unused variables as well
            _acceleration = Vector2.Zero;
            _health= 0;
            _maxHealth= 0;
        }

        public Bullet(Texture2D sprite, Vector2 position, float speed)
        {
            _sprite = sprite;
            _position = position;


            _velocity = new Vector2(speed, 0);

            _width = _sprite.Width;
            _height = _sprite.Height;

            aimedShot = false;
            parried = false;

            _damage = 10;

            // initialize unused variables as well
            _acceleration = Vector2.Zero;
            _health = 0;
            _maxHealth = 0;
        }



        // Methods

        /// <summary>
        /// Method for use in the game's update step; all logic 
        /// calculated by frame should go into this function.
        /// --- The bullet should fly in a straight line until it hits an entity or 
        /// a wall, it cannot damage enemies until it has been parried by the player
        /// </summary>
        /// <param name="currentLevel">The level currently being played</param>
        /// <param name="player">The player</param>
        /// <param name="gameTime">tracks in-game time intervals</param>
        public override void Update(
            Level currentLevel,
            Player player,
            GameTime gameTime)
        {
            ApplyPhysics();

            //If it collides with the player, begin hit logic and remove the projectile
            if (CheckCollision(player))
            {
                player.TakeHit(this, _damage);
                currentLevel.CurrentProjectiles.Remove(this);
            }

            // only detect enemy collisions if the bullet has been parried
            if (parried)
            {
                //detetcts enemy collisions
                for (int i = 0; i < currentLevel.CurrentEnemies.Count; i++)
                {
                    MobileEntity enemey = (MobileEntity)currentLevel.CurrentEnemies[i];
                    if (this.CheckCollision(enemey))
                    {
                        currentLevel.CurrentEnemies[i].TakeHit(this, _damage);
                        currentLevel.CurrentProjectiles.Remove(this);
                    }
                }
            }

            // if it collides with a wall, the bullet is removed
            foreach (Tile tile in currentLevel.CurrentTileMap)
            {
                if (CheckCollision(tile))
                {
                    currentLevel.CurrentProjectiles.Remove(this);
                }
            }
        }


        // Helper methods

        /// <summary>
        /// the bullet should not need to resolve collisions as it 
        /// is removed from the level when it hits a wall
        /// </summary>
        /// <param name="tileMap">The tiles</param>
        protected override void ResolveTileCollisions(List<Tile> tileMap)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Applies acceleration to velocity and velocity to 
        /// position; the bullet is not effected by gravity
        /// </summary>
        protected override void ApplyPhysics()
        {
            _velocity += _acceleration;
            _position += _velocity;
        }


        /// <summary>
        /// Activates when the player shoots the shotgun at the projectile
        /// The projectile flies in the direction of the shotgun blast
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="damage">How much damage to do</param>
        public void ShotgunHit(Vector2 velocity)
        {
            // throw the bullet in the direction given by the shotgun
            _velocity = velocity;
            parried = true;
        }

        /// <summary>
        /// Bullets should not need a reset function because they should not
        /// be spawned at the beginning of the level. this is a leftover from the interface
        /// </summary>
        /// <exception cref="NotImplementedException">unnecissary</exception>
        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
