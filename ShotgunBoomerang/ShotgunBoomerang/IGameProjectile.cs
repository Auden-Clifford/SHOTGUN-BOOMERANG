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
    /// common to all projectiles in the game. 
    /// This should allow the program to treat 
    /// all of the different projectiles as the same.
    /// </summary>
    internal interface IGameProjectile
    {
        /// <summary>
        /// Should get the projectile's centerpoint
        /// </summary>
        public Vector2 CenterPoint { get; }

        /// <summary>
        /// Should tell the given spritebatch to draw/animate the projectile at the correct part of the screen
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
            Player player,
            GameTime gameTime);

        /*
        /// <summary>
        /// Should contain logic for what to do when the projectile hits a player or enemy
        /// </summary>
        public void HitEntity();
        */

        /// <summary>
        /// Method that allows all projectiles to take hits from the shotgun
        /// </summary>
        /// <param name="shotgunNormal">The normalized vector between the shotgun and the projectile</param>
        public void ShotgunHit(Vector2 shotgunNormal);
    }
}
