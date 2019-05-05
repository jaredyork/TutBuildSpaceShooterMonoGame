using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooterTutorial
{
    class Explosion : Entity
    {
        private Texture2D texture;
        public AnimatedSprite sprite;
        public Vector2 origin;

        public Explosion(Texture2D texture, Vector2 position) : base()
        {
            this.texture = texture;
            sprite = new AnimatedSprite(this.texture, 32, 32, 5);
            sprite.setCanRepeat(false);
            this.position = position;
            origin = new Vector2((sprite.frameWidth * 0.5f) * scale.X, (sprite.frameHeight * 0.5f) * scale.Y);
            setupBoundingBox(sprite.frameWidth, sprite.frameHeight);
        }

        public new void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);

            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destRect = new Rectangle((int)position.X - (int)origin.X, (int)position.Y - (int)origin.Y, (int)(sprite.frameWidth * scale.X), (int)(sprite.frameHeight * scale.Y));
            spriteBatch.Draw(texture, destRect, sprite.sourceRect, Color.White);
        }
    }
}
