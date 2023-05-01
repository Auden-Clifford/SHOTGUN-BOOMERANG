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
        // Fields
        private float _damage;

        
        // properties

        /// <summary>
        /// Read-Only damage value
        /// </summary>
        public float Damage
        {
            get { return _damage; }
        }


        // Constructor
        
        /// <summary>
        /// Constructor for damaging tile. Damage value is currently fixed at 10.
        /// </summary>
        /// <param name="sprite">Sprite (uses base)</param>
        /// <param name="position">Position (uses base)</param>
        public DamageTile(Texture2D sprite, Vector2 position) : base(sprite, position)
        {
            _damage = 10;
        }


        // Methods

        /// <summary>
        /// Method for use in the game's update step; all logic 
        /// calculated by frame should go into this function.
        /// --- The DamageTile should sense when an entity is on top of it and damage them
        /// </summary>
        /// <param name="currentLevel">The level currently being played</param>
        /// <param name="player">The player</param>
        /// <param name="gameTime">tracks in-game time intervals</param>
        public override void Update(
            Level currentLevel,
            Player player,
            GameTime gameTime)
        {
            // Checking for player collision
            if (CheckCollision(player))
            {
                player.TakeHit(this, _damage);
            }

            // checking for enemy collision
            foreach(IGameEnemy enemy in currentLevel.CurrentEnemies)
            {
                MobileEntity currentEnemy = enemy as MobileEntity;

                if(CheckCollision(currentEnemy))
                {
                    enemy.TakeHit(this, _damage);
                }
            }
        }


        // Helper methods

        /// <summary>
        /// Returns a rectangle that is 1 pixel higher than the hitbox, allows 
        /// the player to take damage from this tile without being inside it
        /// </summary>
        /// <returns></returns>
        private Rectangle DamageBox()
        {
            return new Rectangle(HitBox.X, HitBox.Y - 1, Width, Height + 1);
        }

        /// <summary>
        /// Checks if another entity has collided with this damage tile's damage area
        /// </summary>
        /// <param name="other">Other object to check</param>
        /// <returns>True if the objects are intersecting; false otherwise</returns>
        protected override bool CheckCollision(GameObject other)
        {
            return DamageBox().Intersects(other.HitBox);
        }
    }
}
