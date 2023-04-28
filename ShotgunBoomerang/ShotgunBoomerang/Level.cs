using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        private LevelEnd _levelEnd;

        private Vector2 _levelSize;
        
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
        /// Get's the level's LevelEnd object
        /// </summary>
        public LevelEnd LevelEnd { get { return _levelEnd; } }

        /// <summary>
        /// Creates a new level with a given tile map
        /// </summary>
        /// <param name="tileMap">The list of tiles the level will have</param>
        public Level(List<Tile> tileMap, Vector2 levelSize, List<IGameEnemy> enemies, List <IGameProjectile> projectiles, Vector2 playerStart)
        {
            // current and original start off equal
            // but they will be copies of eachother (not references)
            _startTileMap = tileMap;
            _currentTileMap = new List<Tile>();

            _levelSize = levelSize;

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
        /// Creates a new level by reading in level data from a file
        /// (this process requires a list of textures in a specific order)
        /// </summary>
        /// <param name="texturePack">List of all the textures this level uses</param>
        /// <param name="filePath">path to open the level file</param>
        public Level(List<Texture2D> texturePack, string filePath)
        {
            // open the specified file for reading.
            StreamReader reader = new StreamReader(File.OpenRead(filePath));

            //read the first line to get the y and x dimentions of the grid
            string[] dimentions = reader.ReadLine().Split(',');
            int height = int.Parse(dimentions[0]);
            int width = int.Parse(dimentions[1]);

            // set the size of the level
            _levelSize = new Vector2(width, height);

            // create new empty object lists
            _startTileMap = new List<Tile>();
            _startEnemies = new List<IGameEnemy>();
            _startProjectiles = new List<IGameProjectile>();

            for(int y = 0; y < height; y++)
            {
                // get an array of strings that represent
                // the objects on this line
                string[] currentLine = reader.ReadLine().Split(',');

                for(int x = 0; x < width; x++)
                {
                    // load the misc tiles
                    if (currentLine[x] == "testTile")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[0],
                            new Vector2(x * 64, y * 64)));
                    }
                    else if (currentLine[x] == "bricks")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[1],
                            new Vector2(x * 64, y * 64)));
                    }
                    else if (currentLine[x] == "planksCenter")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[2],
                            new Vector2(x * 64, y * 64)));
                    }
                    else if (currentLine[x] == "planksLeft")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[3],
                            new Vector2(x * 64, y * 64)));
                    }
                    else if (currentLine[x] == "planksRight")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[4],
                            new Vector2(x * 64, y * 64)));
                    }
                    // load the snake enemy
                    else if (currentLine[x] == "snek")
                    {
                        _startEnemies.Add(
                            new SnakeEnemy(texturePack[5],
                            new Vector2(x * 64, y * 64)));
                    }
                    // load the playerStart
                    else if (currentLine[x] == "playerStart")
                    {
                        _playerStart = new Vector2(x * 64, y * 64);
                    }
                    // load the levelEnd
                    else if (currentLine[x] == "levelEnd")
                    {
                        _levelEnd = new LevelEnd(texturePack[6],
                            new Vector2(x * 64, y * 64));
                    }
                    // load vegemite "projectiles"
                    else if (currentLine[x] == "vegemite")
                    {
                        _startProjectiles.Add(
                            new Vegemite(texturePack[7],
                            new Vector2(x * 64, y * 64),
                            Vector2.Zero));
                    }
                    // load cave tiles
                    else if (currentLine[x] == "caveBottomCenter")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[8],
                            new Vector2(x * 64, y * 64)));
                    }
                    else if (currentLine[x] == "caveBottomLeft")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[9],
                            new Vector2(x * 64, y * 64)));
                    }
                    else if (currentLine[x] == "caveBottomRight")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[10],
                            new Vector2(x * 64, y * 64)));
                    }
                    else if (currentLine[x] == "caveCenterCenter")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[11],
                            new Vector2(x * 64, y * 64)));
                    }
                    else if (currentLine[x] == "caveCenterLeft")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[12],
                            new Vector2(x * 64, y * 64)));
                    }
                    else if (currentLine[x] == "caveCenterRight")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[13],
                            new Vector2(x * 64, y * 64)));
                    }
                    else if (currentLine[x] == "caveTopCenter")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[14],
                            new Vector2(x * 64, y * 64)));
                    }
                    else if (currentLine[x] == "caveTopLeft")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[15],
                            new Vector2(x * 64, y * 64)));
                    }
                    else if (currentLine[x] == "caveTopRight")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[16],
                            new Vector2(x * 64, y * 64)));
                    }
                    // load grass tiles
                    else if (currentLine[x] == "grassBottomCenter")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[17],
                            new Vector2(x * 64, y * 64)));
                    }
                    else if (currentLine[x] == "grassBottomLeft")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[18],
                            new Vector2(x * 64, y * 64)));
                    }
                    else if (currentLine[x] == "grassBottomRight")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[19],
                            new Vector2(x * 64, y * 64)));
                    }
                    else if (currentLine[x] == "grassCenterCenter")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[20],
                            new Vector2(x * 64, y * 64)));
                    }
                    else if (currentLine[x] == "grassCenterLeft")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[21],
                            new Vector2(x * 64, y * 64)));
                    }
                    else if (currentLine[x] == "grassCenterRight")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[22],
                            new Vector2(x * 64, y * 64)));
                    }
                    else if (currentLine[x] == "grassTopCenter")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[23],
                            new Vector2(x * 64, y * 64)));
                    }
                    else if (currentLine[x] == "grassTopLeft")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[24],
                            new Vector2(x * 64, y * 64)));
                    }
                    else if (currentLine[x] == "grassTopRight")
                    {
                        _startTileMap.Add(
                            new Tile(texturePack[25],
                            new Vector2(x * 64, y * 64)));
                    }
                }
            }

            // close the stream reader
            reader.Close();

            // set the current tiles to a copy of the start
            _currentTileMap = new List<Tile>();

            foreach (Tile tile in _startTileMap)
            {
                _currentTileMap.Add(tile);
            }

            // set the current enemies to a copy of the start
            _currentEnemies = new List<IGameEnemy>();

            foreach (IGameEnemy enemy in _startEnemies)
            {
                _currentEnemies.Add(enemy);

            }

            // set the current projectiles to a copy of the start
            _currentProjectiles = new List<IGameProjectile>();

            foreach (IGameProjectile projectile in _startProjectiles)
            {
                _currentProjectiles.Add(projectile);
            }
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

            _levelEnd.Draw(sb, screenOffset);
        }

        public void Update(KeyboardState kb,
            KeyboardState prevKb,
            MouseState ms,
            MouseState prevMs,
            Player player,
            GameTime gameTime)
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
                _currentEnemies[i].Update(kb, prevKb, ms, prevMs, _currentTileMap, _currentEnemies, _currentProjectiles, player, gameTime);
            }

            for(int i = _currentProjectiles.Count- 1; i >= 0; i--)
            {
                _currentProjectiles[i].Update(kb, prevKb, ms, prevMs, _currentTileMap, _currentEnemies, _currentProjectiles, player, gameTime);
            }

            if(player.Position.Y > _levelSize.Y * 64)
            {
                player.Health = 0;
            }

            _levelEnd.Update(kb,prevKb, ms, prevMs, _currentTileMap, _currentEnemies, _currentProjectiles, player, gameTime);
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
            player.Health = 100;
            player.IsHoldingBoomerang = true;
            player.Velocity = new Vector2(0, 0);
            
            player.Score = 0;
            player.Kills = 0;
            player.Timer = 0;
            player.Ammo = 2; // Necessary

            _levelEnd._inContactWithPlayer = false;

            //reset list of enemies and objects
            // by clearing and resetting the current lists to the start lists
            _currentEnemies.Clear();
            foreach (IGameEnemy enemy in _startEnemies)
            {
                enemy.Reset();
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
