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
    internal abstract class MobileEntity : GameObject
    {

        // Fields
        protected Vector2 _velocity;
        protected float _health;
        protected float _maxHealth;
        protected float _damage;
        protected Vector2 _acceleration;
        protected bool onGround;


        // Properties
        /// <summary>
        /// Get's the entity's health clamped between the max and 0
        /// </summary>
        public float Health 
        { 
            get { return _health; } 
            set { _health = Math.Clamp(value, 0, _maxHealth); } 
        }

        /// <summary>
        /// Gets whether this entity is dead
        /// </summary>
        public bool IsDead
        {
            get 
            { 
                if (Health == 0)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Get's the entity's velocity
        /// </summary>
        public Vector2 Velocity { get { return _velocity; } set { _velocity = value; } }

        // Methods

        /// <summary>
        /// method for use in the update loop, contains all logic the object needs to go through in a frame
        /// as well as any parameters from the game manager that might be needed for this logic. 
        /// Update will be the entry point for all data from Game manager to the other classes
        /// -- The base update for all MobileEntity objects simply applies gravity and velocity
        /// </summary>
        /// <param name="kb">The keyboard state this frame</param>
        /// <param name="prevKb"> The keyboard state last frame</param>
        /// <param name="ms">The mouse state this frame</param>
        /// <param name="prevMs">The mouse state last frame</param>
        /// <param name="tileMap">The current level's tiles</param>
        /// <param name="enemies">The current level's enemies</param>
        /// <param name="projectiles">The projectiles currently in play</param>
        /// <param name="player">The player</param>
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
        }

        /// <summary>
        /// Updates the object's position and velocity based on physics interactions
        /// </summary>
        protected virtual void ApplyPhysics()
        {
            // apply gravity to velocity
            _velocity.Y += GameManager.Gravity;

            // apply velocity to position
            _position += _velocity;
        }

        /// <summary>
        /// Will check if the entity is colliding with any tiles then
        /// resolve those collisions by updating the player's position.
        /// </summary>
        protected abstract void ResolveTileCollisions(List<Tile> tileMap);
    }
}
