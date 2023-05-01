using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShotgunBoomerang
{
    enum DirectionState
    {
        Left,
        Right
    }

    internal class KoalaTree : MobileEntity, IGameEnemy
    {
        private Texture2D _bulletSprite;
        private Texture2D _vegemiteSprite;

        private DirectionState direction = DirectionState.Right;
        private Texture2D leftTexture;
        private Texture2D rightTexture;
        private bool damaged;
        private int tileDetectionDistance;
        private Texture2D bulletSprite;
        private float shootingTimer;
        private float counter;
        private float initialTimer;
        private float damagedTimer;
        private Vector2 _startPosition;

        private Direction _currentDirection;
        private bool _damaged;
        private bool spawnKoala;

        //private Texture2D leftTexture;
        //private Texture2D rightTexture;

        private Texture2D deathKoalaLeft;
        private Texture2D deathKoalaRight;

        private int _playerDetectionDistance;
        private double _shotTimer;
        private double _shotTime;
        //private float _counter;
        //private float _initialTimer;
        private double _damagedTimer;
        private double _damagedTime;

        private Color _drawColor;

        /// <summary>
        /// Creates a new KoalaTree enemy at a given position
        /// </summary>
        /// <param name="TexturePack">List of textures used by this enemy</param>
        /// <param name="position">The enemy's position</param>
        /// <param name="spawnKoala">Whether or not to spawn a Koala on death</param>
        public KoalaTree(List<Texture2D> texturePack, Vector2 position, bool spawnKoala)
        {
            // The main sprite should be the first one
            _sprite = texturePack[0];


            // the bullet sprite should be the second one
            _bulletSprite = texturePack[1];
            

            // the vegemite sprite should be the third one
            _vegemiteSprite = texturePack[2];

            //Death koala should be 4th and 5th textures
            deathKoalaLeft = texturePack[3];
            deathKoalaRight = texturePack[4];

            _width = _sprite.Width;
            _height = _sprite.Height;

            _position = position;
            _startPosition = new Vector2(position.X, position.Y);

            _currentDirection = Direction.Right;
            _damage = 20;
            _damaged = false;

            this.spawnKoala = spawnKoala;
            
            _maxHealth = 180;
            _health = _maxHealth;
            _playerDetectionDistance = 512;
            _velocity = Vector2.Zero;
            _drawColor = Color.White;

            _shotTimer = 0; // timer variable
            _shotTime = 1; // amount of time on said timer variable

            _damagedTimer = 0; // timer variable
            _damagedTime = .25; // amount of time on said timer variable

            _acceleration = Vector2.Zero;
        }

        /// <summary>
        /// Draws the koala facing the proper direction. 
        /// the koala will always face the player
        /// </summary>
        /// <param name="sb">Spritebatch in use</param>
        /// <param name="screenOffset">offset of the screen coordinates from the world coordinates</param>
        public override void Draw(SpriteBatch sb, Vector2 screenOffset)
        {
            //Depending on the direction enum, either face the sprite left or right
            switch (_currentDirection)
            {
                // draw facing the correct direction
                case Direction.Left:
                    sb.Draw(
                        _sprite,
                        _position - screenOffset,
                        null,
                        _drawColor,
                        0,
                        new Vector2(0, 0),
                        1,
                        SpriteEffects.None,
                        0);
                    break;

                case Direction.Right:
                    sb.Draw(
                        _sprite,
                        _position - screenOffset,
                        null,
                        _drawColor,
                        0,
                        new Vector2(0, 0),
                        1,
                        SpriteEffects.FlipHorizontally,
                        0);
                    break;
            }
        }

        public void Update(
            Level currentLevel,
            Player player,
            GameTime gameTime
            )
        {
            ApplyPhysics();
            ResolveTileCollisions(currentLevel.CurrentTileMap);

            // turn to face the player
            if (player.X < _position.X)
            {
                _currentDirection = Direction.Left;

            }else if(player.Y >= _position.Y)
            {
                _currentDirection = Direction.Right;
            }


            // detect if the player is within range
            if((player.Position - _position).Length() < _playerDetectionDistance)
            {
                // if the timer is still running do nothing
                if(_shotTimer >= 0)
                {
                    _shotTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                }
                // when the timer is up, shoot and reset the timer
                else
                {
                    currentLevel.CurrentProjectiles.Add(
                        new Bullet(
                            _bulletSprite,
                            CenterPoint,
                            Vector2.Normalize(player.Position - _position) * 10));
                    _shotTimer = _shotTime;
                }
            }

            // if the enemy took damage, change the color
            if(_damaged)
            {
                _damagedTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                _drawColor = Color.Red; // the enemy will draw red when damaged

                // if the enemy died, remove it from the list
                // the enemy has a 1 in 10 chance of dropping vegemite upon death
                if (Health <= 0)
                {
                    Random dropRng = new Random();
                    if (dropRng.Next(10) == 0)
                    {
                        currentLevel.CurrentProjectiles.Add(
                            new Vegemite(
                                _vegemiteSprite,
                                _position,
                                _velocity));
                    }

                    if (spawnKoala)
                    {
                        currentLevel.CurrentEnemies.Add(new Koala(
                            deathKoalaLeft,
                            deathKoalaRight,
                            _position,
                            180,
                            20,
                            20,
                            3,
                            0.33f,
                            7,
                            _bulletSprite));
                    }
                    currentLevel.CurrentEnemies.Remove(this);
                    player.Kills++;
                }

                // transition out of damaged state when time is up
                if(_damagedTimer <= 0)
                {
                    _damaged = false;
                    _drawColor = Color.White;
                }
            }
        }



        /// <summary>
        /// Allows the koala tree to properly collide with surfaces
        /// </summary>
        /// <param name="tileMap">list of tiles in the level</param>
        protected override void ResolveTileCollisions(List<Tile> tileMap)
        {
            // get a copy of the enemy's hitbox
            Rectangle enemyHitBox = this.HitBox;

            // temporary list for all intersections
            List<Rectangle> intersectionsList = new List<Rectangle>();

            // loop through the list of all tiles
            // to find which ones are interseting
            foreach (Tile tile in tileMap)
            {
                // if the enemy is intersecting the tile
                if (this.CheckCollision(tile))
                {
                    // add it's hitbox to the list
                    intersectionsList.Add(tile.HitBox);
                }
            }

            while (intersectionsList.Count > 0)
            {
                // variable to store the largest intersection
                Rectangle largest = new Rectangle();

                // find the largest intersection
                foreach (Rectangle rectangle in intersectionsList)
                {
                    if (rectangle.Width * rectangle.Height >= largest.Width * largest.Height)
                    {
                        largest = rectangle;
                    }
                }

                // resolve the largest collision
                Rectangle intersectRect = Rectangle.Intersect(HitBox, largest);

                //check for a horizontal collision (intersection is taller than it is wide)
                if (intersectRect.Width <= intersectRect.Height)
                {
                    // if the enemy X is less than (further left than) the intersection's x
                    // move the enemy left
                    if (this._position.X < intersectRect.X)
                    {
                        enemyHitBox.X -= intersectRect.Width;

                        //the enemy's X velocity cannot be positive when touching the right wall
                        this._velocity.X = Math.Clamp(_velocity.X, float.MinValue, 0);
                    }
                    // otherwise move right
                    else
                    {
                        enemyHitBox.X += intersectRect.Width;

                        //the enemy's X velocity cannot be negative when touching the left wall
                        this._velocity.X = Math.Clamp(_velocity.X, 0, float.MaxValue);
                    }

                    this._position.X = enemyHitBox.X;
                }
                // otherwise this must be a vertical collision
                else
                {
                    // if the enemy Y is less than (further up than) the rectangle Y
                    // move the enemy up
                    if (this._position.Y < intersectRect.Y)
                    {
                        enemyHitBox.Y -= intersectRect.Height;

                        //the enemy's Y velocity cannot be negative when touching the ground
                        this._velocity.Y = Math.Clamp(_velocity.Y, float.MinValue, 0);
                    }
                    // otherwise the player has hit thier head, move down
                    else
                    {
                        enemyHitBox.Y += intersectRect.Height;

                        //the enemy's Y velocity cannot be positive when touching the cieling
                        this._velocity.Y = Math.Clamp(_velocity.Y, 0, float.MaxValue);
                    }

                    this._position.Y = enemyHitBox.Y;
                }

                // reset the intersections list and check again
                intersectionsList.Clear();

                // loop through the list of all tiles
                // to find which ones are interseting
                foreach (Tile tile in tileMap)
                {
                    // if the enemy is intersecting the tile
                    if (tile.HitBox.Intersects(enemyHitBox))
                    {
                        // add it's hitbox to the list
                        intersectionsList.Add(tile.HitBox);
                    }
                }
            }
        }

        /// <summary>
        /// Contains logic for an enemy taking a hit from a given source
        /// </summary>
        /// <param name="attacker">Source of the damage</param>
        /// <param name="damage">Amount of damage to take</param>
        public void TakeHit(GameObject attacker, float damage)
        {
            if (!_damaged)
            {
                _health -= damage;
                _damagedTimer = _damagedTime;
                _damaged = true;
            }
        }

        /// <summary>
        /// Resets the enemy to how it 
        /// was at the start of the level
        /// </summary>
        public void Reset()
        {
            _health = _maxHealth;
            _damaged = false;
            _damagedTimer = 0;
            _shotTimer = 0;
            _drawColor = Color.White;

            // the koala shouldn't move, but just in case
            _position.X = _startPosition.X;
            _position.Y = _startPosition.Y;
            _velocity = Vector2.Zero;
        }
    }
}
