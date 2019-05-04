using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SpaceShooterTutorial
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        private Texture2D texSinglePixel;
        private List<Texture2D> texBgs = new List<Texture2D>();
        private Texture2D texBtnPlay;
        private Texture2D texBtnPlayDown;
        private Texture2D texBtnPlayHover;
        private Texture2D texBtnRestart;
        private Texture2D texBtnRestartDown;
        private Texture2D texBtnRestartHover;
        private Texture2D texPlayer;
        private Texture2D texPlayerLaser;
        private Texture2D texEnemyLaser;
        private List<Texture2D> texEnemies = new List<Texture2D>();
        private Texture2D texExplosion;

        public SoundEffect sndBtnDown;
        public SoundEffect sndBtnOver;
        public List<SoundEffect> sndExplode = new List<SoundEffect>();
        public SoundEffect sndLaser;

        private SpriteFont fontArial;
        

        enum GameState
        {
            MainMenu,
            Gameplay,
            GameOver
        }
        GameState _gameState;

        KeyboardState lastKeyState = Keyboard.GetState();
        KeyboardState keyState = Keyboard.GetState();

        private MenuButton playButton;
        private MenuButton restartButton;

        private List<Explosion> explosions = new List<Explosion>();
        private List<Enemy> enemies = new List<Enemy>();
        private List<EnemyLaser> enemyLasers = new List<EnemyLaser>();
        private List<PlayerLaser> playerLasers = new List<PlayerLaser>();
        private Player player = null;
        private ScrollingBackground scrollingBackground;

        private int restartDelay = 60 * 2;
        private int restartTick = 0;

        private int spawnEnemyDelay = 60;
        private int spawnEnemyTick = 0;

        private int playerShootDelay = 15;
        private int playerShootTick = 0;

        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            this.IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 640;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // Load textures

            texSinglePixel = Content.Load<Texture2D>("singlepixel");

            for (int i = 0; i < 2; i++)
            {
                texBgs.Add(Content.Load<Texture2D>("sprBg" + i));
            }

            texBtnPlay = Content.Load<Texture2D>("sprBtnPlay");
            texBtnPlayDown = Content.Load<Texture2D>("sprBtnPlayDown");
            texBtnPlayHover = Content.Load<Texture2D>("sprBtnPlayHover");

            texBtnRestart = Content.Load<Texture2D>("sprBtnRestart");
            texBtnRestartDown = Content.Load<Texture2D>("sprBtnRestartDown");
            texBtnRestartHover = Content.Load<Texture2D>("sprBtnRestartHover");

            texPlayer = Content.Load<Texture2D>("sprPlayer");
            texPlayerLaser = Content.Load<Texture2D>("sprLaserPlayer");
            texEnemyLaser = Content.Load<Texture2D>("sprLaserEnemy0");

            for (int i = 0; i < 3; i++)
            {
                texEnemies.Add(Content.Load<Texture2D>("sprEnemy" + i));
            }

            texExplosion = Content.Load<Texture2D>("sprExplosion");

            fontArial = Content.Load<SpriteFont>("ArialHeading");

            scrollingBackground = new ScrollingBackground(texBgs);

            playButton = new MenuButton(this, new Vector2(graphics.PreferredBackBufferWidth * 0.5f - (int)(texBtnPlay.Width * 0.5), graphics.PreferredBackBufferHeight * 0.5f), texBtnPlay, texBtnPlayDown, texBtnPlayHover);
            restartButton = new MenuButton(this, new Vector2(graphics.PreferredBackBufferWidth * 0.5f - (int)(texBtnPlay.Width * 0.5), graphics.PreferredBackBufferHeight * 0.5f), texBtnRestart, texBtnRestartDown, texBtnRestartHover);


            // Load sounds
            sndBtnDown = Content.Load<SoundEffect>("sndBtnDown");
            sndBtnOver = Content.Load<SoundEffect>("sndBtnOver");
            for (int i = 0; i < 2; i++)
            {
                sndExplode.Add(Content.Load<SoundEffect>("sndExplode" + i));
            }
            sndLaser = Content.Load<SoundEffect>("sndLaser");


            changeGameState(GameState.MainMenu);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            lastKeyState = keyState;
            keyState = Keyboard.GetState();

            scrollingBackground.Update(gameTime);

            switch (_gameState) {
                case GameState.MainMenu:
                    {
                        UpdateMainMenu(gameTime);
                        break;
                    }

                case GameState.Gameplay:
                    {
                        UpdateGameplay(gameTime);
                        break;
                    }

                case GameState.GameOver:
                    {
                        UpdateGameOver(gameTime);
                        break;
                    }
            }

            base.Update(gameTime);
        }

        private void UpdateMainMenu(GameTime gameTime)
        {
            if (playButton.isActive)
            {
                MouseState mouseState = Mouse.GetState();

                if (playButton.boundingBox.Contains(mouseState.Position))
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        playButton.SetDown(true);
                        playButton.SetHovered(false);
                    }
                    else
                    {
                        playButton.SetDown(false);
                        playButton.SetHovered(true);
                    }

                    if (mouseState.LeftButton == ButtonState.Released && playButton.lastIsDown)
                    {
                        changeGameState(GameState.Gameplay);
                    }
                }
                else
                {
                    playButton.SetDown(false);
                    playButton.SetHovered(false);
                }

                playButton.lastIsDown = mouseState.LeftButton == ButtonState.Pressed ? true : false;
            }
            else
            {
                playButton.isActive = true;
            }
        }

        private void UpdateGameplay(GameTime gameTime)
        {
            if (player == null)
            {
                player = new Player(texPlayer, new Vector2(graphics.PreferredBackBufferWidth * 0.5f, graphics.PreferredBackBufferHeight * 0.5f));
            }
            else
            {
                player.body.velocity = new Vector2(0, 0);

                if (player.isDead())
                {
                    if (restartTick < restartDelay)
                    {
                        restartTick++;
                    }
                    else
                    {
                        changeGameState(GameState.GameOver);
                        restartTick = 0;
                    }
                }
                else
                {
                    if (keyState.IsKeyDown(Keys.W))
                    {
                        player.MoveUp();
                    }
                    if (keyState.IsKeyDown(Keys.S))
                    {
                        player.MoveDown();
                    }
                    if (keyState.IsKeyDown(Keys.A))
                    {
                        player.MoveLeft();
                    }
                    if (keyState.IsKeyDown(Keys.D))
                    {
                        player.MoveRight();
                    }
                    if (keyState.IsKeyDown(Keys.Space))
                    {
                        if (playerShootTick < playerShootDelay)
                        {
                            playerShootTick++;
                        }
                        else
                        {
                            sndLaser.Play();
                            PlayerLaser laser = new PlayerLaser(texPlayerLaser, new Vector2(player.position.X + player.destOrigin.X, player.position.Y), new Vector2(0, -10));
                            playerLasers.Add(laser);
                            playerShootTick = 0;
                        }
                    }
                }

                player.Update(gameTime);

                player.position.X = MathHelper.Clamp(player.position.X, 0, graphics.PreferredBackBufferWidth - player.body.boundingBox.Width);
                player.position.Y = MathHelper.Clamp(player.position.Y, 0, graphics.PreferredBackBufferHeight - player.body.boundingBox.Height);
            }


            /**
             * UPDATE ENTITY POSITIONS
             **/
            for (int i = 0; i < playerLasers.Count; i++)
            {
                playerLasers[i].Update(gameTime);

                if (playerLasers[i].position.Y < 0)
                {
                    playerLasers.Remove(playerLasers[i]);
                    continue;
                }
            }

            for (int i = 0; i < enemyLasers.Count; i++)
            {
                enemyLasers[i].Update(gameTime);

                if (player != null)
                {
                    if (!player.isDead())
                    {
                        if (player.body.boundingBox.Intersects(enemyLasers[i].body.boundingBox))
                        {
                            sndExplode[randInt(0, sndExplode.Count - 1)].Play();
                            Explosion explosion = new Explosion(texExplosion, new Vector2(player.position.X + player.destOrigin.X, player.position.Y + player.destOrigin.Y));
                            explosions.Add(explosion);

                            player.setDead(true);
                        }
                    }
                }

                if (enemyLasers[i].position.Y > GraphicsDevice.Viewport.Height)
                {
                    enemyLasers.Remove(enemyLasers[i]);
                }
                
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update(gameTime);

                if (player != null)
                {
                    if (!player.isDead())
                    {
                        if (player.body.boundingBox.Intersects(enemies[i].body.boundingBox))
                        {
                            sndExplode[randInt(0, sndExplode.Count - 1)].Play();
                            Explosion explosion = new Explosion(texExplosion, new Vector2(player.position.X + player.destOrigin.X, player.position.Y + player.destOrigin.Y));
                            explosions.Add(explosion);

                            player.setDead(true);
                        }

                        if (enemies[i].GetType() == typeof(GunShip))
                        {
                            GunShip enemy = (GunShip)enemies[i];

                            if (enemy.canShoot)
                            {
                                EnemyLaser laser = new EnemyLaser(texEnemyLaser, new Vector2(enemy.position.X, enemy.position.Y), new Vector2(0, 5));
                                enemyLasers.Add(laser);

                                enemy.resetCanShoot();
                            }
                        }
                        if (enemies[i].GetType() == typeof(ChaserShip))
                        {
                            ChaserShip enemy = (ChaserShip)enemies[i];

                            if (Vector2.Distance(enemies[i].position, player.position + player.destOrigin) < 320)
                            {
                                enemy.SetState(ChaserShip.States.CHASE);
                            }

                            if (enemy.GetState() == ChaserShip.States.CHASE)
                            {
                                Vector2 direction = (player.position + player.destOrigin) - enemy.position;
                                direction.Normalize();

                                float speed = 3;
                                enemy.body.velocity = direction * speed;

                                if (enemy.position.X + (enemy.destOrigin.X) < player.position.X + (player.destOrigin.X))
                                {
                                    enemy.angle = enemy.angle - 5;
                                }
                                else
                                {
                                    enemy.angle = enemy.angle + 5;
                                }
                            }
                        }
                    }
                }

                if (enemies[i].position.Y > GraphicsDevice.Viewport.Height)
                {
                    enemies.Remove(enemies[i]);
                }
            }

            for (int i = 0; i < explosions.Count; i++)
            {
                explosions[i].Update(gameTime);

                if (explosions[i].sprite.isFinished())
                {
                    explosions.Remove(explosions[i]);
                }
            }

            /**
             * COLLISIONS
             **/

            for (int i = 0; i < playerLasers.Count; i++)
            {
                bool shouldDestroyLaser = false;
                for (int j = 0; j < enemies.Count; j++)
                {
                    if (playerLasers[i].body.boundingBox.Intersects(enemies[j].body.boundingBox))
                    {
                        sndExplode[randInt(0, sndExplode.Count - 1)].Play();

                        Explosion explosion = new Explosion(texExplosion, new Vector2(enemies[j].position.X, enemies[j].position.Y));
                        explosion.scale = enemies[j].scale;

                        Console.WriteLine("Shot enemy.  Origin: " + enemies[j].destOrigin + ", pos: " + enemies[j].position);

                        explosion.position.Y += enemies[j].body.boundingBox.Height * 0.5f;
                        explosions.Add(explosion);

                        enemies.Remove(enemies[j]);

                        shouldDestroyLaser = true;
                    }
                }

                if (shouldDestroyLaser)
                {
                    playerLasers.Remove(playerLasers[i]);
                }
            }


            // Enemy spawning
            if (spawnEnemyTick < spawnEnemyDelay)
            {
                spawnEnemyTick++;
            }
            else
            {
                Enemy enemy = null;
                
                if (randInt(0, 10) <= 3)
                {
                    Vector2 spawnPos = new Vector2(randFloat(0, graphics.PreferredBackBufferWidth), -128);
                    enemy = new GunShip(texEnemies[0], spawnPos, new Vector2(0, randFloat(1, 3)));
                }
                else if (randInt(0, 10) >= 5)
                {
                    Vector2 spawnPos = new Vector2(randFloat(0, graphics.PreferredBackBufferWidth), -128);
                    enemy = new ChaserShip(texEnemies[1], spawnPos, new Vector2(0, randFloat(1, 3)));
                }
                else
                {
                    Vector2 spawnPos = new Vector2(randFloat(0, graphics.PreferredBackBufferWidth), -128);
                    enemy = new CarrierShip(texEnemies[2], spawnPos, new Vector2(0, randFloat(1, 3)));
                }

                enemies.Add(enemy);

                spawnEnemyTick = 0;
            }
        }

        private void UpdateGameOver(GameTime gameTime)
        {
            if (restartButton.isActive)
            {
                MouseState mouseState = Mouse.GetState();

                if (restartButton.boundingBox.Contains(mouseState.Position))
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        restartButton.SetDown(true);
                        restartButton.SetHovered(false);
                    }
                    else
                    {
                        restartButton.SetDown(false);
                        restartButton.SetHovered(true);
                    }

                    if (mouseState.LeftButton == ButtonState.Released && restartButton.lastIsDown)
                    {
                        changeGameState(GameState.Gameplay);
                    }
                }
                else
                {
                    restartButton.SetDown(false);
                    restartButton.SetHovered(false);
                }

                restartButton.lastIsDown = mouseState.LeftButton == ButtonState.Pressed ? true : false;
            }
            else
            {
                restartButton.isActive = true;
            }
        }

        private void resetGameplay()
        {
            if (player != null)
            {
                player.setDead(false);
                player.position = new Vector2((int)(graphics.PreferredBackBufferWidth * 0.5), (int)(graphics.PreferredBackBufferHeight * 0.5));
            }
        }

        private void changeGameState(GameState gameState)
        {
            playButton.isActive = false;
            restartButton.isActive = false;
            explosions.Clear();
            enemies.Clear();
            playerLasers.Clear();
            enemyLasers.Clear();
            resetGameplay();

            _gameState = gameState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap);

            scrollingBackground.Draw(spriteBatch);

            switch (_gameState)
            {
                case GameState.MainMenu:
                    {
                        DrawMainMenu(spriteBatch);
                        break;
                    }

                case GameState.Gameplay:
                    {
                        DrawGameplay(spriteBatch);
                        break;
                    }

                case GameState.GameOver:
                    {
                        DrawGameOver(spriteBatch);
                        break;
                    }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void DrawMainMenu(SpriteBatch spriteBatch)
        {
            string title = "SPACE SHOOTER";
            spriteBatch.DrawString(fontArial, title, new Vector2(graphics.PreferredBackBufferWidth * 0.5f - (fontArial.MeasureString(title).X * 0.5f), graphics.PreferredBackBufferHeight * 0.2f), Color.White);

            playButton.Draw(spriteBatch);
        }

        public void DrawGameplay(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(spriteBatch);
            }

            for (int i = 0; i < playerLasers.Count; i++)
            {
                playerLasers[i].Draw(spriteBatch);
            }

            for (int i = 0; i < enemyLasers.Count; i++)
            {
                enemyLasers[i].Draw(spriteBatch);
            }

            for (int i = 0; i < explosions.Count; i++)
            {
                explosions[i].Draw(spriteBatch);
            }

            if (player != null)
            {
                player.Draw(spriteBatch);
            }
        }

        public void DrawGameOver(SpriteBatch spriteBatch)
        {
            string title = "GAME OVER";
            spriteBatch.DrawString(fontArial, title, new Vector2(graphics.PreferredBackBufferWidth * 0.5f - (fontArial.MeasureString(title).X * 0.5f), graphics.PreferredBackBufferHeight * 0.2f), Color.White);

            restartButton.Draw(spriteBatch);
        }

        public static int randInt(int minNumber, int maxNumber)
        {
            return new Random().Next(minNumber, maxNumber);
        }

        public static float randFloat(float minNumber, float maxNumber)
        {
            return (float)new Random().NextDouble() * (maxNumber - minNumber) + minNumber;
        }
    }
}
