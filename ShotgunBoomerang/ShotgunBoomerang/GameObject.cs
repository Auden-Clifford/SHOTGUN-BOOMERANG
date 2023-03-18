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
    /// A base class for all physical objects in the game
    /// </summary>
    internal abstract class GameObject
    {
        // Fields
        protected Texture2D _sprite;
        protected Vector2 _position;


        // Properties

        /// <summary>
        /// Gets the player's hitbox based on texture and position
        /// </summary>
        public Rectangle HitBox 
        { 
            get 
            { 
                return new Rectangle(
                    (int)_position.X, 
                    (int)_position.Y, 
                    _sprite.Width, 
                    _sprite.Height); 
            } 
        }


        // Methods

        /// <summary>
        /// Tells the given spritebatch to draw 
        /// the object at the proper position
        /// </summary>
        /// <param name="sb">Spritebatch in use</param>
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(_sprite, _position, Color.White);
        }

        /// <summary>
        /// A base method for use in the update loop
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Checks if this object has collided with another
        /// </summary>
        /// <param name="other">Other object to check</param>
        /// <returns>True if the objects are intersecting; false otherwise</returns>
        public virtual bool CheckCollision(GameObject other)
        {
            return this.HitBox.Intersects(other.HitBox);
        }
    }
}
