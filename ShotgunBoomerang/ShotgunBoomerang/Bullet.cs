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


        // Constructor

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

            aimedShot = false;
            parried = false;

            _damage = 10;

            // initialize unused variables as well
            _acceleration = Vector2.Zero;
            _health= 0;
            _maxHealth= 0;
        }


        // Methods

        /// <summary>
        /// method for use in the update loop, contains all logic the object needs to go through in a frame
        /// as well as any parameters from the game manager that might be needed for this logic. 
        /// Update will be the entry point for all data from Game manager to the other classes
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
            ApplyPhysics();

            //If it collides with the player, begin hit logic and remove the projectile
            if (CheckCollision(player))
            {
                player.TakeHit(this, _damage);
                projectiles.Remove(this);
            }

            if (parried)
            {
                //detetcts enemy collisions
                for (int i = 0; i < enemies.Count; i++)
                {
                    MobileEntity enemey = (MobileEntity)enemies[i];
                    if (this.CheckCollision(enemey))
                    {
                        enemies[i].TakeHit(this, _damage);
                        projectiles.Remove(this);
                    }
                }
            }

            // if it collides with a wall, the bullet is removed
            foreach (Tile tile in tileMap)
            {
                if (CheckCollision(tile))
                {
                    projectiles.Remove(this);
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
        public void ShotgunHit(Vector2 shotgunNormal)
        {
            // throw the bullet in the  direction of the shotgun blast
            _velocity = shotgunNormal * _velocity.Length() * 2;
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
