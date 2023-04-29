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
    
    public enum EnemyState
    {
        Damaged,
        Run
    }

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
        public void Draw(SpriteBatch sb, Vector2 offset);

        /// <summary>
        /// Method for use in the game's update step; all logic calculated 
        /// for this object by frame should go into this function.
        /// </summary>
        /// <param name="currentLevel">The level currently being played</param>
        /// <param name="player">The player</param>
        /// <param name="gameTime">tracks in-game time intervals</param>
        public void Update(
            Level currentLevel,
            Player player,
            GameTime gameTime);

        /// <summary>
        /// Contains logic for an enemy taking a hit from a given source
        /// </summary>
        /// <param name="attacker">Source of the damage</param>
        /// <param name="damage">Amount of damage to take</param>
        public void TakeHit(GameObject attacker, float damage);

        
        /// <summary>
        /// Resets the enemy to how it 
        /// was at the start of the level
        /// </summary>
        public void Reset();
        
    }
}
