using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooterTutorial
{
    class ChaserShip : Enemy
    {
        public enum States
        {
            MOVE_DOWN,
            CHASE
        }
        private States state = States.MOVE_DOWN;

        public void SetState(States state)
        {
            this.state = state;
            isRotatable = true;
        }

        public States GetState()
        {
            return state;
        }

        public ChaserShip(Texture2D texture, Vector2 position, Vector2 velocity) : base(texture, position, velocity)
        {
            angle = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
