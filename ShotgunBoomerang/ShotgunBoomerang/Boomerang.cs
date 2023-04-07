using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShotgunBoomerang
{
    /// <summary>
    /// A projectile thrown by the player which bounces off walls and returns to the player's hand
    /// </summary>
    internal class Boomerang : MobileEntity, IGameProjectile
    {
        public bool CheckCollision()
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch sb)
        {
            throw new NotImplementedException();
        }

        public void hit()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
