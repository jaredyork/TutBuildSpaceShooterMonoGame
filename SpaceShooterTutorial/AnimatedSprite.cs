using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooterTutorial
{
    class AnimatedSprite
    {
        private Texture2D texture;
        public int frameWidth { get; set; }
        public int frameHeight { get; set; }
        public int duration { get; set; }
        public Rectangle sourceRect { get; set; }
        public int amountFrames { get; set; }
        public int currentFrame { get; set; }
        private int updateTick = 0;
        private bool _repeats = true;
        public void setCanRepeat(bool canRepeat)
        {
            _repeats = canRepeat;
        }
        private bool _finished = false;
        public bool isFinished()
        {
            return _finished;
        }

        public AnimatedSprite(Texture2D texture, int frameWidth, int frameHeight, int duration)
        {
            this.texture = texture;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.duration = duration;
            amountFrames = this.texture.Width / this.frameWidth;
            sourceRect = new Rectangle(currentFrame * this.frameWidth, 0, this.frameWidth, this.frameHeight);
        }

        public void Update(GameTime gameTime)
        {
            if (updateTick < duration)
            {
                updateTick++;
            }
            else
            {
                if (currentFrame < amountFrames - 1)
                {
                    currentFrame++;
                }
                else
                {
                    if (_repeats)
                    {
                        currentFrame = 0;
                    }
                    else
                    {
                        _finished = true;
                    }
                }

                sourceRect = new Rectangle(currentFrame * this.frameWidth, 0, this.frameWidth, this.frameHeight);
                currentFrame++;
                updateTick = 0;
            }
        }
    }
}
