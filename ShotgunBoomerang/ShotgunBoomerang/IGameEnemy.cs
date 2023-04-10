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
    /// <summary>
    /// An interface that describes the methods 
    /// common to all Enemies in the game. 
    /// This should allow the program to treat 
    /// all of the different enemies as the same.
    /// </summary>
    internal interface IGameEnemy
    {
        /// <summary>
        /// Should tell the given spritebatch to draw/animate the enemy
        /// </summary>
        public void Draw(SpriteBatch sb, Vector2 screenOffset);

        /// <summary>
        /// method for use in the update loop, contains all logic the object needs to go through in a frame
        /// as well as any parameters from the game manager that might be needed for this logic. 
        /// Update will be the entry point for all data from Game manager to the other classes
        /// </summary>
        /// <param name="kb">The keyboard state this frame</param>
        /// <param name="prevKb"> The keyboard state last frame</param>
        /// <param name="ms">The mouse state this frame</param>
        /// <param name="prevMs">The mouse state last frame</param>
        /// <param name="tileMap">The current level's tiles</param>
        /// <param name="enemies">The current level's enemies</param>
        /// <param name="projectiles">The projectiles currently in play</param>
        public void Update(
            KeyboardState kb,
            KeyboardState prevKb,
            MouseState ms,
            MouseState prevMs,
            List<Tile> tileMap,
            List<IGameEnemy> enemies,
            List<IGameProjectile> projectiles,
            Player player);

        /// <summary>
        /// Should check if the enemy is colliding with any game objects
        /// </summary>
        /// <returns></returns>
        public bool CheckCollision();

        /// <summary>
        /// Should either launch a projectile or melee attack on the player
        /// </summary>
        public void Attack();

        /// <summary>
        /// should contain logic for movement
        /// </summary>
        public void Move();
    }
}
