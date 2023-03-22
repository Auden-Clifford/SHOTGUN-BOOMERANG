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
    /// Determines the different states the player can be in
    /// </summary>
    public enum PlayerState
    {
        Idle,
        Run,
        Airborne,
        Slide,
        Skid
    }

    /// <summary>
    /// Determines the directions the player can face
    /// </summary>
    public enum Direction
    {
        Left,
        Right
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

            _velocity = new Vector2(0, 0);
            _maxHealth = 100;
            _damage = 60;
            _acceleration = 3;
            _ammo = 2;
            _isHoldingBoomerang = true;
            _currentState = PlayerState.Idle;
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
            // The player is slowed by different amounts depending
            // on whether they are running, skidding, or in the air
            float runFriction = 0.8f;
            float airFriction = 0.7f;
            float skidFriction = 0.9f;

            switch(_currentState)
            {
                case PlayerState.Idle:
                    // Transition to Airborne when no longer colliding with the ground
                    // Transition to Run when A or D is pressed
                    break;

                case PlayerState.Run: 
                    // Transition to Airborne when no longer colliding with the ground
                    // Transition to Slide when CTRL is pressed
                    // Transition to Skid when A or D is no longer pressed
                    break;

                case PlayerState.Airborne: 
                    // Transition to Idle when there is no horizontal velocity and player collides with ground
                    // Transition to Run when there is horizontal velocity and player collides with ground
                    break;

                case PlayerState.Slide: 
                    // Transition to Run when CTRL is released
                    // Transition to Airborne when no longer colliding with ground
                    break;

                case PlayerState.Skid:
                    // Transition to Idle when there is no horizontal velocity
                    // Transition to Run if A or D is pressed
                    break;
            }
        }
    }
}
