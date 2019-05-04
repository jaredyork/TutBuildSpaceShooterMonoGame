using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SpaceShooterTutorial
{
    class Entity
    {
        protected bool isRotatable = false;
        public Vector2 scale = new Vector2(1.5f, 1.5f);
        public Vector2 position = new Vector2(0, 0);
        protected Vector2 sourceOrigin = new Vector2(0, 0);
        public Vector2 destOrigin = new Vector2(0, 0);
        public PhysicsBody body { get; set; }

        public Entity()
        {
            body = new PhysicsBody();
        }

        public void setupBoundingBox(int width, int height)
        {
            body.boundingBox = new Rectangle((int)(position.X - destOrigin.X), (int)(position.Y - destOrigin.Y), (int)(width * scale.X), (int)(height * scale.Y));
        }

        public void Update(GameTime gameTime)
        {
            if (body != null)
            {
                position.X += body.velocity.X;
                position.Y += body.velocity.Y;

                body.boundingBox = new Rectangle((int)position.X - (int)destOrigin.X, (int)position.Y - (int)destOrigin.Y, body.boundingBox.Width, body.boundingBox.Height);

            }
            else
            {
                Console.WriteLine("[BaseEntity] body not found, skipping position updates.");
            }
        }
    }
}
