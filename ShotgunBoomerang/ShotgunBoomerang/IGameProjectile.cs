using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Should tell the given spritebatch to draw/animate the projectile
        /// </summary>
        public void Draw(SpriteBatch sb);

        /// <summary>
        /// Should contain logic for the movement and physics interactions of the projectile
        /// </summary>
        public void Update();

        /// <summary>
        /// Should Resolve Collisions with Tiles
        /// </summary>
        //public void ResolveTileCollisions(List<Tile> tilemap);

        /// <summary>
        /// Should contain logic for what to do when the projectile hits a player or enemy
        /// </summary>
        public void hit();
    }
}
