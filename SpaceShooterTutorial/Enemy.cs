using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooterTutorial
{
    class Enemy : Entity
    {
        protected Texture2D texture;
        protected AnimatedSprite sprite;
        public float angle { get; set; }

        public Enemy(Texture2D texture, Vector2 position, Vector2 velocity) : base()
        {
            this.texture = texture;
            sprite = new AnimatedSprite(this.texture, 16, 16, 10);
            scale = new Vector2(Game1.randFloat(10, 20) * 0.1f);
            sourceOrigin = new Vector2(sprite.frameWidth * 0.5f, sprite.frameHeight * 0.5f);
            destOrigin = new Vector2((sprite.frameWidth * 0.5f) * scale.X, (sprite.frameHeight * 0.5f) * scale.Y);
            this.position = position;
            body.velocity = velocity;

            setupBoundingBox(sprite.frameWidth, sprite.frameHeight);
        }

        public new virtual void Update(GameTime gameTime)
        {
            destOrigin = new Vector2(
                (float)Math.Round((sprite.frameWidth * 0.5f) * scale.X),
                (float)Math.Round((sprite.frameHeight * 0.5f) * scale.Y)
            );

            sprite.Update(gameTime);

            base.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (isRotatable)
            {
                Rectangle destRect = new Rectangle((int)position.X, (int)position.Y, (int)(sprite.frameWidth * scale.X), (int)(sprite.frameHeight * scale.Y));
                spriteBatch.Draw(texture, destRect, sprite.sourceRect, Color.White, MathHelper.ToRadians(angle), sourceOrigin, SpriteEffects.None, 0);

            }
            else
            {
                Rectangle destRect = new Rectangle((int)position.X, (int)position.Y, (int)(sprite.frameWidth * scale.X), (int)(sprite.frameHeight * scale.Y));
                spriteBatch.Draw(texture, destRect, sprite.sourceRect, Color.White, MathHelper.ToRadians(angle), sourceOrigin, SpriteEffects.None, 0);
            }
        }
    }
}
