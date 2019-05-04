using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooterTutorial
{
    class ScrollingBackground
    {
        private List<Texture2D> textures = new List<Texture2D>();
        private List<ScrollingBackgroundLayer> layers = new List<ScrollingBackgroundLayer>();

        public ScrollingBackground(List<Texture2D> textures)
        {
            this.textures = textures;

            for (int i = -1; i < 3; i++)
            {
                for (int j = 0; j < 3; j++) { // 3 layers
                    Texture2D texture = textures[Game1.randInt(0, textures.Count - 1)];
                    Vector2 position = new Vector2(0, texture.Height * i);
                    Vector2 velocity = new Vector2(0, (j + 1) * 0.2f);
                    ScrollingBackgroundLayer layer = new ScrollingBackgroundLayer(this, texture, j, i, position, velocity);
                    layers.Add(layer);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            List<ScrollingBackgroundLayer> layersDepth0 = new List<ScrollingBackgroundLayer>();
            List<ScrollingBackgroundLayer> layersDepth1 = new List<ScrollingBackgroundLayer>();
            List<ScrollingBackgroundLayer> layersDepth2 = new List<ScrollingBackgroundLayer>();
            List<ScrollingBackgroundLayer> layersToReset = new List<ScrollingBackgroundLayer>();
            
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].Update(gameTime);

                switch (layers[i].depth)
                {
                    case 0:
                        {
                            layersDepth0.Add(layers[i]);
                            break;
                        }

                    case 1:
                        {
                            layersDepth1.Add(layers[i]);
                            break;
                        }

                    case 2:
                        {
                            layersDepth2.Add(layers[i]);
                            break;
                        }
                }
            }

            bool resetLayersDepth0 = false;
            bool resetLayersDepth1 = false;
            bool resetLayersDepth2 = false;

            // Loop through layers in depth 0
            for (int i = 0; i < layersDepth0.Count; i++)
            {
                if (layersDepth0[i].positionIndex == -1)
                {
                    if (layersDepth0[i].position.Y > 0)
                    {
                        resetLayersDepth0 = true;
                    }
                }
            }

            // Loop through layers in depth 1
            for (int i = 0; i < layersDepth1.Count; i++)
            {
                if (layersDepth1[i].positionIndex == -1)
                {
                    if (layersDepth1[i].position.Y > 0)
                    {
                        resetLayersDepth1 = true;
                    }
                }
            }

            // Loop through layers in depth 2
            for (int i = 0; i < layersDepth2.Count; i++)
            {
                if (layersDepth2[i].positionIndex == -1)
                {
                    if (layersDepth2[i].position.Y > 0)
                    {
                        resetLayersDepth2 = true;
                    }
                }
            }

            if (resetLayersDepth0)
            {
                for (int i = 0; i < layersDepth0.Count; i++)
                {
                    layersDepth0[i].position = layersDepth0[i].initialPosition;
                }
            }

            if (resetLayersDepth1)
            {
                for (int i = 0; i < layersDepth1.Count; i++)
                {
                    layersDepth1[i].position = layersDepth1[i].initialPosition;
                }
            }

            if (resetLayersDepth2)
            {
                for (int i = 0; i < layersDepth2.Count; i++)
                {
                    layersDepth2[i].position = layersDepth2[i].initialPosition;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].Draw(spriteBatch);
            }
        }
    }
}
