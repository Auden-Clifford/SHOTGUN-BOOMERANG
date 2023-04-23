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

        protected int _width;
        protected int _height;

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
                    _width, 
                    _height); 
            } 
        }

        /// <summary>
        /// returns the center point of a game object
        /// </summary>
        public Vector2 CenterPoint
        {
            get
            {
                return new Vector2(
                    HitBox.Center.X,
                    HitBox.Center.Y);
            }
        }

        /// <summary>
        /// Get's or sets the object's position
        /// </summary>
        public virtual Vector2 Position { get { return _position; } set { _position = value; } }

        public Texture2D Sprite { get { return _sprite; } }

        /// <summary>
        /// Gets the object's width
        /// </summary>
        public int Width { get { return _width; } }

        /// <summary>
        /// Get's the object's height
        /// </summary>
        public int Height { get { return _height; } }


        // Methods

        /// <summary>
        /// Tells the given spritebatch to draw 
        /// the object at the proper position (using the screen offset)
        /// </summary>
        /// <param name="sb">Spritebatch in use</param>
        public virtual void Draw(SpriteBatch sb, Vector2 screenOffset)
        {
            sb.Draw(_sprite, _position - screenOffset, Color.White);
        }

        /// <summary>
        /// Base method for use in the update loop, should contain all logic the object needs to go through 
        /// in a frame as well as any parameters from the game manager that might be needed for this logic. 
        /// Update will be the entry point for all data from Game manager to the other classes
        /// </summary>
        /// <param name="kb">The keyboard state this frame</param>
        /// <param name="prevKb"> The keyboard state last frame</param>
        /// <param name="ms">The mouse state this frame</param>
        /// <param name="prevMs">The mouse state last frame</param>
        /// <param name="tileMap">The current level's tiles</param>
        /// <param name="enemies">The current level's enemies</param>
        /// <param name="projectiles">The projectiles currently in play</param>
        /// <param name="player">The player</param>
        public abstract void Update(
            KeyboardState kb,
            KeyboardState prevKb,
            MouseState ms,
            MouseState prevMs,
            List<Tile> tileMap,
            List<IGameEnemy> enemies,
            List<IGameProjectile> projectiles,
            Player player,
            GameTime gameTime);

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
