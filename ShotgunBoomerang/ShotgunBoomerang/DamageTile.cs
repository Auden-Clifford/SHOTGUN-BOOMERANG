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
    internal class DamageTile : Tile
    {
        // Damage field
        private float _damage;

        // Field

        /// <summary>
        /// Read-Only damage value
        /// </summary>
        public float Damage
        {
            get { return _damage; }
        }
        
        /// <summary>
        /// Constructor for damaging tile. Damage value is currently fixed at 10.
        /// </summary>
        /// <param name="sprite">Sprite (uses base)</param>
        /// <param name="position">Position (uses base)</param>
        public DamageTile(Texture2D sprite, Vector2 position) : base(sprite, position)
        {
            _damage = 10;
        }

        /// <summary>
        /// Base method for use in the update loop, should contain all logic the object needs to go through 
        /// in a frame as well as any parameters from the game manager that might be needed for this logic. 
        /// Update will be the entry point for all data from Game manager to the other classes
        /// -- DamageTile update logic damages the player (wow)
        /// </summary>
        /// <param name="kb">The keyboard state this frame</param>
        /// <param name="prevKb"> The keyboard state last frame</param>
        /// <param name="ms">The mouse state this frame</param>
        /// <param name="prevMs">The mouse state last frame</param>
        /// <param name="tileMap">The current level's tiles</param>
        /// <param name="enemies">The current level's enemies</param>
        /// <param name="projectiles">The projectiles currently in play</param>
        /// <param name="player">The player</param>
        /// <exception cref="NotImplementedException">Tile currently has no update function</exception>
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
            // Checking for player collision
            if (CheckCollision(player))
            {
                player.TakeDamage(_damage);
            }
        }
    }
}
