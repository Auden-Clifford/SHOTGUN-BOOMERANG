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
        public void Draw(SpriteBatch sb);

        /// <summary>
        /// Should contain logic for deciding when to attack and move
        /// </summary>
        public void Update();

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
