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
        /// Tile currently has no update function
        /// </summary>
        /// <exception cref="NotImplementedException">Tile currently has no update function</exception>
        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
