using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ShotgunBoomerang
{
    internal class LevelEnd : GameObject
    {
        // Fields
        private bool _inContactWithPlayer;

        // Properties
        public bool IncidentWithPlayer { get { return _inContactWithPlayer; } }

        // Constructor
        /// <summary>
        /// Creates a new LevelEnd object at a given location
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="position"></param>
        public LevelEnd(Texture2D sprite, Vector2 position)
        {
            _sprite = sprite;
            _position = position;
            _inContactWithPlayer = false;
        }

        /// <summary>
        /// Method for use in the update loop, should contain all logic the object needs to go through 
        /// in a frame as well as any parameters from the game manager that might be needed for this logic. 
        /// Update will be the entry point for all data from Game manager to the other classes.
        /// -- The Level End object will detect whether it has collided with the player
        /// </summary>
        ///<param name = "kb" > The keyboard state this frame</param>
        /// <param name="prevKb"> The keyboard state last frame</param>
        /// <param name="ms">The mouse state this frame</param>
        /// <param name="prevMs">The mouse state last frame</param>
        /// <param name="tileMap">The current level's tiles</param>
        /// <param name="enemies">The current level's enemies</param>
        /// <param name="projectiles">The projectiles currently in play</param>
        /// <param name="player">The player</param>
        /// <param name="gameTime">The game time</param>
        public override void Update(
            KeyboardState kb, 
            KeyboardState prevKb, 
            MouseState ms,
            MouseState prevMs, 
            List<Tile> tileMap, 
            List<IGameEnemy> enemies,
            List<IGameProjectile> projectiles, 
            Player player, 
            Microsoft.Xna.Framework.GameTime gameTime)
        {
            if(this.HitBox.Intersects(player.HitBox))
            {
                _inContactWithPlayer = true;
            }
        }
    }
}
