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
    public enum PlayerState
    {
        Facing_Left,
        Facing_Right,
        Running_Left,
        Running_Right,
        Airborn_Left,
        Airborn_Right,
        Sliding_Left,
        Sliding_Right
    }
    internal class Player : MobileEntity
    {
        // Fields

        private int _ammo;
        private bool _isHoldingBoomerang;
        private PlayerState _currentState;
        private float _shotgunRadius;
        private float _shotgunAngle;


        // Properties

        
        // Constructors
        /// <summary>
        /// Creates a new player with given texture, position, and health
        /// </summary>
        /// <param name="sprite">The player's texture/spritesheet</param>
        /// <param name="position">The player's starting position</param>
        /// <param name="health">The player's starting health</param>
        public Player(Texture2D sprite, Vector2 position, float health)
        {
            _sprite = sprite;
            _position = position;
            _health = health;

            _velocity = 0;
            _maxHealth = 100;
            _damage = 60;
            _ammo = 2;
            _isHoldingBoomerang = true;
            _currentState = PlayerState.Facing_Right;
            _shotgunRadius = 64;
            _shotgunAngle = 45;
        }


        // Methods
        /// <summary>
        /// Draws the player with animations based on FSM
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }

        /// <summary>
        /// Contains the logic for updating the player, including FSM and movement
        /// </summary>
        public override void Update()
        {
            base.Update();
        }
    }
}
