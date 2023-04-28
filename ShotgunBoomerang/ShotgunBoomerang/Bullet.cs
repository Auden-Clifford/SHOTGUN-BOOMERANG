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
    internal class Bullet : MobileEntity, IGameProjectile
    {
        private bool active;


        /// <summary>
        /// Creates a new bullet moving horizontally at a specified speed
        /// </summary>
        /// <param name="sprite">The sprite to draw using</param>
        /// <param name="position">Where it's being spawned</param>
        /// <param name="damage">How much damage it does</param>
        /// <param name="moveSpeed">How fast to move. Value also determines direction</param>
        public Bullet(Texture2D sprite, Vector2 position, float damage, float moveSpeed)
        {
            this._sprite = sprite;
            this._position = position;
            this._damage = damage;
            this._velocity = new Vector2(moveSpeed, 0);
            active = true;
        }

        /// <summary>
        /// Should tell the given spritebatch to draw/animate the projectile at the correct part of the screen
        /// </summary>
        public void Draw(SpriteBatch sb, Vector2 screenOffset)
        {
            if (active)
            {
                sb.Draw(_sprite, _position - screenOffset, Color.White);
            }
        }

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
        public void Update(
            List<Tile> tileMap,
            List<IGameEnemy> enemies,
            List<IGameProjectile> projectiles,
            Player player,
            GameTime gameTime)
        {
            //Only updates if active
            if (active)
            {
                //Is not affected by gravity currently
                _velocity.Y = 0;
                Math.Clamp(_velocity.Y, 0, 0);
                _position += _velocity;

                //If it collides with the player, begin hit logic and remove the projectile
                if (CheckCollision(player))
                {
                    Hit(player, _damage);
                    projectiles.Remove(this);
                }
            }
        }

        /// <summary>
        /// I swear this isn't recursion. This is just fixing a problem arising from a conflict between
        /// the interface and the inheritance
        /// </summary>
        /// <param name="kb"></param>
        /// <param name="prevKb"></param>
        /// <param name="ms"></param>
        /// <param name="prevMs"></param>
        /// <param name="tileMap"></param>
        /// <param name="enemies"></param>
        /// <param name="projectiles"></param>
        /// <param name="player"></param>
        /// <param name="gameTime"></param>
        public void Update(
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
            Update(tileMap, enemies, projectiles, player, gameTime);
        }
            

        /// <summary>
        /// Disables the projectile and damages the player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="damage">How much damage to do</param>
        public void Hit(Player player, float damage)
        {
            player.TakeDamage(damage);
            active = false;
        }

        /// <summary>
        /// If the projectile hits a wall, deactivate it.
        /// </summary>
        /// <param name="tileMap">The tiles</param>
        protected override void ResolveTileCollisions(List<Tile> tileMap)
        {
            foreach (Tile tile in tileMap)
            {
                if (CheckCollision(tile))
                {
                    active = false;
                }
            }
        }

        /// <summary>
        /// Checks if there is a collision with another object
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool CheckCollision(MobileEntity other)
        {
            return this.HitBox.Intersects(other.HitBox);
        }
    }
}
