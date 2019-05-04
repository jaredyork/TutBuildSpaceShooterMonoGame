using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooterTutorial
{
    class CarrierShip : Enemy
    {
        public CarrierShip(Texture2D texture, Vector2 position, Vector2 velocity) : base(texture, position, velocity)
        {
            
        }
    }
}
