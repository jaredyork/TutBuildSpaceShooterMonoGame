using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooterTutorial
{
    class MenuButton
    {
        private Game1 game;
        private Vector2 position;
        private Texture2D texDefault;
        private Texture2D texOnDown;
        private Texture2D texOnHover;
        private Texture2D currentTexture;
        public Rectangle boundingBox;

        public bool isActive { get; set; }
        public bool lastIsDown = false;
        private bool _isDown = false;
        private bool _isHovered = false;

        public void SetDown(bool isDown)
        {
            if (!_isDown && isDown)
            {
                game.sndBtnDown.Play();
            }
            _isDown = isDown;

            ChangeTexture();
        }
        public void SetHovered(bool isHovered)
        {
            if (!_isHovered && !_isDown && isHovered)
            {
                game.sndBtnOver.Play();
            }
            _isHovered = isHovered;

            ChangeTexture();
        }

        private void ChangeTexture()
        {
            if (_isDown)
            {
                currentTexture = texOnDown;
            }
            else
            {
                if (_isHovered)
                {
                    currentTexture = texOnHover;
                }
                else
                {
                    currentTexture = texDefault;
                }
            }
        }

        public MenuButton(Game1 game, Vector2 position, Texture2D texDefault, Texture2D texOnDown, Texture2D texOnHover)
        {
            this.game = game;
            this.position = position;
            this.texDefault = texDefault;
            this.texOnDown = texOnDown;
            this.texOnHover = texOnHover;
            currentTexture = this.texDefault;
            boundingBox = new Rectangle((int)position.X, (int)position.Y, this.texDefault.Width, this.texDefault.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                spriteBatch.Draw(currentTexture, position, Color.White);
            }
        }
    }
}
