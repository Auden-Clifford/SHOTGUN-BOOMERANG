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
    internal class Level
    {
        // Fields

        // original and current tiles, enemies, and projectiles
        // are needed for resetting and comparison

        // the originals will be what the level comes pre-loaded with and cannot be changed
        // the current ones will change as gameplay goes on (enemy is killed, projectile is created or disappears)
        private List<Tile> _startTileMap;
        private List<Tile> _currentTileMap;

        private List<IGameEnemy> _startEnemies;
        private List<IGameEnemy> _currentEnemies;

        private List<IGameProjectile> _startProjectiles;
        private List<IGameProjectile> _currentProjectiles;

        private Vector2 _playerStart;

        // Properties
        /// <summary>
        /// Gets the level's original tile map
        /// </summary>
        public List<Tile> StartTileMap { get { return _startTileMap; } }

        /// <summary>
        /// Gets or sets the level's current tile map
        /// </summary>
        public List<Tile> CurrentTileMap { get { return _currentTileMap; } set { _currentTileMap = value; } }

        /// <summary>
        /// Gets the level's original set of enemies
        /// </summary>
        public List<IGameEnemy> StartEnemies { get { return _startEnemies; } }

        /// <summary>
        /// Gets or sets the level's current enemies
        /// </summary>
        public List<IGameEnemy> CurrentEnemies { get { return _currentEnemies; } set { _currentEnemies = value; } }

        /// <summary>
        /// Gets the levels original set of projectiles
        /// </summary>
        public List<IGameProjectile> StartProjectiles { get { return _startProjectiles; } }

        /// <summary>
        /// Gets or sets the level's current projectiles
        /// </summary>
        public List<IGameProjectile> CurrentProjectiles { get { return _currentProjectiles; } set { _currentProjectiles = value; } }

        /// <summary>
        /// Get's the level's starting player position.
        /// </summary>
        public Vector2 PlayerStart { get { return _playerStart; } }

        /// <summary>
        /// Creates a new level with a given tile map
        /// </summary>
        /// <param name="tileMap">The list of tiles the level will have</param>
        public Level(List<Tile> tileMap, List<IGameEnemy> enemies, List <IGameProjectile> projectiles, Vector2 playerStart)
        {
            // current and original start off equal
            // but they will be copies of eachother (not references)
            _startTileMap = tileMap;
            _currentTileMap = new List<Tile>();

            foreach(Tile tile in _startTileMap)
            {
                _currentTileMap.Add(tile);
            }

            _startEnemies = enemies;
            _currentEnemies = new List<IGameEnemy>();

            foreach (IGameEnemy enemy in _startEnemies)
            {
                _currentEnemies.Add(enemy);
                
            }

            _startProjectiles = projectiles;
            _currentProjectiles = new List<IGameProjectile>();

            
            foreach(IGameProjectile projectile in _startProjectiles)
            {
                _currentProjectiles.Add(projectile);
            }
            
            _playerStart = playerStart;
        }

        /// <summary>
        /// Should tell the given spritebatch to display every tile, projectile, and enemy in the level
        /// </summary>
        /// <param name="sb">The spritebatch in use</param>
        /// <param name="screenOffset">The offset of the screen from the world coordinates</param>
        public void Draw(SpriteBatch sb, Vector2 screenOffset)
        {
            foreach(Tile tile in _currentTileMap)
            {
                tile.Draw(sb, screenOffset);
            }

            foreach(IGameEnemy enemy in _currentEnemies)
            {
                enemy.Draw(sb, screenOffset);
            }

            foreach(IGameProjectile projectile in _currentProjectiles)
            {
                projectile.Draw(sb, screenOffset);
            }
        }

        public void Update(KeyboardState kb,
            KeyboardState prevKb,
            MouseState ms,
            MouseState prevMs,
            Player player)
        {
            // tiles dont have an update yet so disregard this
            /*
            foreach (Tile tile in _currentTileMap)
            {
                tile.Update(kb, prevKb, ms, prevMs, _currentTileMap, _currentEnemies, _currentProjectiles, player);
            }
            */

            for (int i = _currentEnemies.Count - 1; i >= 0; i--)
            {
                _currentEnemies[i].Update(_currentTileMap, _currentProjectiles, player);
            }

            for(int i = _currentProjectiles.Count- 1; i >= 0; i--)
            {
                _currentProjectiles[i].Update(kb, prevKb, ms, prevMs, _currentTileMap, _currentEnemies, _currentProjectiles, player);
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

        /// <summary>
        /// Resets the player's position to the starting position
        /// And sets player velocity to 0
        /// </summary>
        /// <param name="player">player to reset</param>
        public void ResetLevel(Player player)
        {
            player.Position = _playerStart;
            player.IsHoldingBoomerang = true;
            player.Velocity = new Vector2(0, 0);
            
            player.Score = 0;
            player.Kills = 0;

            //health reset

            //reset list of enemies and objects
            // by clearing and resetting the current lists to the start lists
            _currentEnemies.Clear();
            foreach (IGameEnemy enemy in _startEnemies)
            {
                
                _currentEnemies.Add(enemy);
            }

            _currentProjectiles.Clear();
            foreach (IGameProjectile projectile in _startProjectiles)
            {
                _currentProjectiles.Add(projectile);
            }
        }
    }
}
