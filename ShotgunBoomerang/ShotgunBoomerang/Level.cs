using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShotgunBoomerang
{
    internal class Level
    {
        // Fields
        private List<Tile> _tileMap;

        // Properties
        /// <summary>
        /// Gets the level's tile map
        /// </summary>
        public List<Tile> TileMap { get { return _tileMap; } }

        /// <summary>
        /// Creates a new level with a given tile map
        /// </summary>
        /// <param name="tileMap">The list of tiles the level will have</param>
        public Level(List<Tile> tileMap)
        {
            _tileMap = tileMap;
        }
    }
}
