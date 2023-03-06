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
    internal abstract class MobileEntity : GameObject
    {
        // Fields
        private Vector2 _velocity;
        private float _health;
        private float _maxHealth;
        private float _damage;


        // Properties
        public float Health { get { return _health; } set { _health = Math.Clamp(value, 0, _maxHealth); } }
    }
}
