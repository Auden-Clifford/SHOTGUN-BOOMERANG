﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShotgunBoomerang
{
    internal class Level
    {
        // Fields
        private List<Tile> _tileMap;
        private Vector2 _playerStart;

        // Properties
        /// <summary>
        /// Gets the level's tile map
        /// </summary>
        public List<Tile> TileMap { get { return _tileMap; } }

        /// <summary>
        /// Get's the level's starting player position.
        /// </summary>
        public Vector2 PlayerStart { get { return _playerStart; } }

        /// <summary>
        /// Creates a new level with a given tile map
        /// </summary>
        /// <param name="tileMap">The list of tiles the level will have</param>
        public Level(List<Tile> tileMap, Vector2 playerStart)
        {
            _tileMap = tileMap;
            _playerStart = playerStart;
        }

        /// <summary>
        /// Should tell the given spritebatch to display every tile in the level
        /// </summary>
        /// <param name="sb">The spritebatch in use</param>
        public void Draw(SpriteBatch sb)
        {
            foreach(Tile tile in _tileMap)
            {
                tile.Draw(sb);
            }
        }

        /// <summary>
        /// Should set the level back to it's original state
        /// </summary>
        public void Reset()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Should load the next level
        /// </summary>
        public void NextLevel()
        {
            throw new NotImplementedException();
        }
    }
}
