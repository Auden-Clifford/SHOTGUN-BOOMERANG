﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShotgunBoomerang
{
    public enum SnakeState
    {
        Patrol,
        Frightened,
        Airborne,
        Damaged
    }
    internal class SnakeEnemy : MobileEntity, IGameEnemy
    {
        // Fields

        private Vector2 _startPos;
        private double _damageTimer;
        private double _frightenTimer;

        private SnakeState _currentState;

        private Texture2D _vegemiteDropSprite;
        private bool _inContactWithGround;

        
        // Constructor

        public SnakeEnemy(List<Texture2D> texturePack, Vector2 position)

        {
            // the snake's texture should be the first one
            _sprite = texturePack[0];

            // the vegemite sprite should be second
            _vegemiteDropSprite = texturePack[1];

            _position = position;
            _startPos = new Vector2(position.X, position.Y);
            _maxHealth = 100;
            _health = _maxHealth;
            _damage = 20;

            _currentState = SnakeState.Patrol;

            _acceleration = new Vector2(1, 0);

            

            _velocity = new Vector2(0, 0);
            _width = _sprite.Width;
            _height = _sprite.Height;

            _damageTimer = 0;
            _frightenTimer = 0;

            _inContactWithGround = true;
        }


        // Methods        
        
        /// <summary>
        /// Draws the snake
        /// </summary>
        /// <param name="sb">The sprite batch</param>
        /// <param name="offset">The screenoffset</param>
        public override void Draw(SpriteBatch sb, Vector2 offset)
        {
            switch(_currentState)
            {
                // draw normally while patrolling
                case SnakeState.Patrol:
                    sb.Draw(_sprite, _position - offset, Color.White);
                    break;

                // draw normally while frightened
                case SnakeState.Frightened:
                    sb.Draw(_sprite, _position - offset, Color.White);
                    break;

                // draw normally while airborne
                case SnakeState.Airborne:
                    sb.Draw(_sprite, _position - offset, Color.White);
                    break;

                // draw in red while damaged
                case SnakeState.Damaged:
                    sb.Draw(_sprite, _position - offset, Color.Red);
                    break;
            }
        }

        /// <summary>
        /// Handles the update logic for the snake
        /// </summary>
        /// <param name="tileMap">The level map</param>
        /// <param name="projectiles">The list of projectiles</param>
        /// <param name="player">The player</param>
        public override void Update(
            Level currentLevel,
            Player player,
            GameTime gameTime)
        {
            // set friction constant
            float runFriction = 0.8f;
            float airFriction = 0.99f;

            // calculate physics
            ResolveTileCollisions(currentLevel.CurrentTileMap);

            switch(_currentState)
            {
                case SnakeState.Patrol:
                    // add the velocity to the accelertation
                    _velocity.X += _acceleration.X;
                    _velocity *= runFriction;

                    // detect whether the enemy has intersected the player and damage them
                    if(CheckCollision(player))
                    {
                        player.TakeHit(this, _damage);
                    }

                    // if the snake is no longer touching the ground, transition to airborne
                    if (!_inContactWithGround)
                    {
                        _currentState = SnakeState.Airborne;
                    }

                    // the enemy will transition to damage state when the TakeDamage function is called
                    break;

                case SnakeState.Frightened:
                    _frightenTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                    // add the velocity to the accelertation (acceleration is 3x faster when frightened)
                    _velocity.X += _acceleration.X * 3;
                    _velocity *= runFriction;

                    // detect whether the enemy has intersected the player and damage them
                    if (CheckCollision(player))
                    {
                        player.TakeHit(this, _damage);
                    }

                    // if the snake is no longer touching the ground, transition to airborne
                    if(!_inContactWithGround)
                    {
                        _currentState = SnakeState.Airborne;
                    }

                    // when the time is up, transition back to the patrol state
                    if(_frightenTimer <= 0)
                    {
                        _currentState = SnakeState.Patrol;
                    }

                    break;

                case SnakeState.Airborne:
                    // add the velocity to the accelertation
                    //_velocity.X += _acceleration.X * 3;
                    _velocity *= airFriction;

                    // transition to frighten if the snake is in contact with ground and 
                    // the frighten timer is not up
                    if(_inContactWithGround && _frightenTimer > 0)
                    {
                        _currentState = SnakeState.Frightened;
                    }
                    // transition to patrol state if the snake touches the ground
                    // and there is no remaining fighten time
                    else if(_inContactWithGround)
                    {
                        _currentState = SnakeState.Patrol;
                    }
                    break;

                case SnakeState.Damaged:
                    _damageTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                    // if the enemy died, remove it from the enemies list
                    if(_health <= 0)
                    {
                        // has a 1 in 10 chance of dropping vegemite
                        Random dropRng = new Random();
                        if(dropRng.Next(10) == 0)
                        {
                            currentLevel.CurrentProjectiles.Add(
                                new Vegemite(_vegemiteDropSprite,
                                _position,
                                _velocity));
                        }

                        currentLevel.CurrentEnemies.Remove(this);
                        player.Kills++;
                    }

                    // when the time is up, transition frightened state
                    if(_damageTimer <= 0)
                    {
                        _currentState = SnakeState.Frightened;
                        _frightenTimer = 5;
                    }
                    break;
            }
        }

        /// <summary>
        /// Definitely not just Auden's code I took cause it wasn't necessary to do the work again
        /// 
        /// </summary>
        /// <param name="tileMap"></param>
        protected override void ResolveTileCollisions(List<Tile> tileMap)
        {
            //gravity is applied beforehand
            ApplyPhysics();

            // reset ground sensor
            _inContactWithGround = false;

            // get the snake's hitbox
            Rectangle SnakeHitBox = this.HitBox;

            // temporary list for all intersections
            List<Rectangle> intersectionsList = new List<Rectangle>();

            // loop through the list of all tiles
            // to find which ones are interseting
            foreach (Tile tile in tileMap)
            {
                // if the snake is intersecting the tile
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
                Rectangle intersectRect = Rectangle.Intersect(SnakeHitBox, largest);

                //check for a horizontal collision (intersection is taller than it is wide)
                if (intersectRect.Width <= intersectRect.Height)
                {
                    // if the snake X is less than (further left than) the intersection's X
                    // move the snake left
                    if (this._position.X < intersectRect.X)
                    {
                        SnakeHitBox.X -= intersectRect.Width;

                        //the player's X velocity cannot be positive when touching the right wall
                        this._velocity.X = Math.Clamp(_velocity.X, float.MinValue, 0);
                        
                    }
                    // otherwise move right
                    else
                    {
                        SnakeHitBox.X += intersectRect.Width;

                        //the snake's X velocity cannot be negative when touching the left wall
                        this._velocity.X = Math.Clamp(_velocity.X, 0, float.MaxValue);
                    }

                    // when a horizontal collision is made, the snake should head in the opposite direction
                    _acceleration *= -1;

                    this._position.X = SnakeHitBox.X;
                }
                // otherwise this must be a vertical collision
                else
                {
                    // if the snake Y is less than (further up than) the rectangle Y
                    // move the snake up
                    if (this._position.Y < intersectRect.Y)
                    {
                        SnakeHitBox.Y -= intersectRect.Height;

                        // report that the snake is touching the ground
                        _inContactWithGround = true;

                        //the snake's Y velocity cannot be negative when touching the ground
                        this._velocity.Y = Math.Clamp(_velocity.Y, float.MinValue, 0);
                    }
                    // otherwise the snake has hit thier head, move down
                    else
                    {
                        SnakeHitBox.Y += intersectRect.Height;

                        //the player's Y velocity cannot be positive when touching the cieling
                        this._velocity.Y = Math.Clamp(_velocity.Y, 0, float.MaxValue);
                    }

                    this._position.Y = SnakeHitBox.Y;
                }

                // reset the intersections list and check again
                intersectionsList.Clear();

                // loop through the list of all tiles
                // to find which ones are interseting
                foreach (Tile tile in tileMap)
                {
                    // if the player is intersecting the tile
                    if (tile.HitBox.Intersects(SnakeHitBox))
                    {
                        // add it's hitbox to the list
                        intersectionsList.Add(tile.HitBox);
                    }
                }
            }
        }

        /// <summary>
        /// Allows the enemy to take damage from a given source
        /// </summary>
        /// <param name="attacker">Source of the damage</param>
        /// <param name="damage">Amount of damage</param>
        public void TakeHit(GameObject attacker, float damage)
        {
            _health -= damage;

            // get the normalized vector between the player's centerpoint and the enemy's centerpoint
            Vector2 attackerNormal = Vector2.Normalize(CenterPoint - attacker.CenterPoint);

            // throw the enemy away from it's attacker (throw force scales with damage)
            _velocity = attackerNormal * (damage / 2);

            _currentState = SnakeState.Damaged;

            _damageTimer = 0.5f;
        }

        /// <summary>
        /// Resets the snake to the way it 
        /// was at the beginning of the level
        /// </summary>
        public void Reset()
        {
            _health = _maxHealth;
            _position.X = _startPos.X;
            _position.Y = _startPos.Y;
            _velocity = new Vector2(0, 0);
            _currentState = SnakeState.Patrol;
            _damageTimer = 0;
            _frightenTimer = 0;
        }
    }
}
