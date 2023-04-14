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
    internal class Tile : GameObject
    {
        /// <summary>
        /// Creates a new tile with a given texture and position
        /// </summary>
        /// <param name="sprite">The tile's texture</param>
        /// <param name="position">The tile's position</param>
        public Tile(Texture2D sprite, Vector2 position)
        {
            _sprite = sprite;
            _position = position;
        }

        /// <summary>
        /// Creates a new tile with a given texture and coordinates
        /// </summary>
        /// <param name="sprite">The tile's texture</param>
        /// <param name="x">The tile's X coordinate</param>
        /// <param name="y">The tile's Y coordinate</param>
        public Tile(Texture2D sprite, int x, int y)
            : this(sprite, new Vector2(x, y)) { }

        /// <summary>
        /// Base method for use in the update loop, should contain all logic the object needs to go through 
        /// in a frame as well as any parameters from the game manager that might be needed for this logic. 
        /// Update will be the entry point for all data from Game manager to the other classes
        /// -- There is currently no update logic for Tiles
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
            throw new NotImplementedException();
        }
    }
}
