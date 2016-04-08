using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace Mobius
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont scoreFont;
        public static ContentManager content;
        static public Character player;
        int score;
        static private List<Bullet> activeBullets;
        static private Pool<Bullet> inactiveBullets;

        public static float difficulty;

		Level testLevel;
        public static Camera camera;
        public static Vector2 gravity = new Vector2(0.0f, 2.0f);
        CollisionDetector collisionDetector;

        Random random = new Random();

        // Effects used by this game.
        Effect colorEffect;
        Effect ribbonEffect;
        Effect bloomEffect;
        Effect raveEffect;
        Effect sunsetEffect;
        Effect rainEffect;
        Effect shaderEffect;

        // Effect specific variables
        float alpha = 0;
        float distance = 0;
        float radius = 0;
        bool bloomActive = true;
        float rainDistance = 0;

        Vector3[] raveColors = new Vector3[12] { 
            new Vector3(1f,     0f,     0f),
            new Vector3(1f,     0.5f,   0f),
            new Vector3(1f,     1f,     0f),
            new Vector3(0.5f,   1f,     0f),
            new Vector3(0f,     1f,     0f),
            new Vector3(0f,     1f,     0.5f),
            new Vector3(0f,     1f,     1f),
            new Vector3(0f,     0.5f,   1f),
            new Vector3(0f,     0f,     1f),
            new Vector3(0.5f,   0f,     1f),
            new Vector3(1f,     0f,     1f),
            new Vector3(1f,     0f,     0.5f),};

        Vector4[] ravePositions = new Vector4[10] {
            new Vector4(0f,     0f,     0f,     0f),
            new Vector4(0f,     0f,     10f,     0f),
            new Vector4(0f,     0f,     20f,     0f),
            new Vector4(0f,     0f,     30f,     0f),
            new Vector4(0f,     0f,     40f,     0f),
            new Vector4(0f,     0f,     50f,     0f),
            new Vector4(0f,     0f,     60f,     0f),
            new Vector4(0f,     0f,     70f,     0f),
            new Vector4(0f,     0f,     80f,     0f),
            new Vector4(0f,     0f,     90f,     0f)};


        RenderTarget2D renderTarget2D;
        RenderTarget2D renderTarget2D_2;

        Texture2D ControllerDetectScreenBackground;
        Texture2D TitleScreenBackground;
        Texture2D PausedScreen;
        Texture2D Ouroboros;
        Texture2D statScreen;
        Texture2D helpScreen;
        Texture2D creditScreen;
        float rotation;
        GamePadState currentState;
        GamePadState prevState;
        KeyboardState currentKeyState;
        KeyboardState prevKeyState;
        int resetTimer;
        List<Tuple<String, int>> highScores;

        Song backgroundMusic;
        bool songStart;

        public static bool levelCompleted;
        public static int upDiff;

        enum ScreenState { 
            Title,
            Game,
            Paused,
            Help,
            Credit
        }

        ScreenState CurrentScreen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            content = Content;
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.IsFullScreen = false;
            
            graphics.ApplyChanges();
            camera = new Camera(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, 8192, 768);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            testLevel = new Level(camera, graphics);
            // TODO: Add your initialization logic here
            score = 0;
            scoreFont = Content.Load<SpriteFont>("Courier New");
            player = new Character();
            player.age = 0;
            collisionDetector = new CollisionDetector();
            activeBullets = new List<Bullet>();
            inactiveBullets = new Pool<Bullet>(20);
            difficulty = 1;
            upDiff = 0;
            highScores = new List<Tuple<string, int>>();
            ReadHighScores();

            base.Initialize();
        }

        private void AdjustHighScores()
        {

        }

        private void ReadHighScores()
        {
            //using (StreamReader sr = File.OpenText(@"Content\highscores.txt"))
            //{
            //    string s = "";
            //    string[] splitLine;
            //    while ((s = sr.ReadLine()) != null)
            //    {
            //        splitLine = s.Split(',');
            //        highScores.Add(new Tuple<string,int>(splitLine[0], Convert.ToInt32(splitLine[1])));
            //        Console.WriteLine(s);
            //    }
            //}
        }

        private void WriteHighScores()
        {

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ControllerDetectScreenBackground = Content.Load<Texture2D>(@"enemy");
            TitleScreenBackground = Content.Load<Texture2D>(@"TitleScreen");
            PausedScreen = Content.Load<Texture2D>(@"PauseScreen");
            Ouroboros = Content.Load<Texture2D>(@"Ouroboros");
            statScreen = Content.Load<Texture2D>(@"StatScreen");
            helpScreen = Content.Load<Texture2D>(@"HelpScreen");
            creditScreen = Content.Load<Texture2D>(@"CreditsScreen");

            CurrentScreen = ScreenState.Title;

            backgroundMusic = Content.Load<Song>(@"Sounds/JazzStreetMusicians");              
            MediaPlayer.IsRepeating = true;

            colorEffect = Content.Load<Effect>("Shaders/color");
            ribbonEffect = Content.Load<Effect>("Shaders/ribbon");
            bloomEffect = Content.Load<Effect>("Shaders/bloom");
            raveEffect = Content.Load<Effect>("Shaders/rave");
            sunsetEffect = Content.Load<Effect>("Shaders/sunset");
            rainEffect = Content.Load<Effect>("Shaders/rain");
            shaderEffect = Content.Load<Effect>("Shaders/shaders");

            renderTarget2D = new RenderTarget2D(
                graphics.GraphicsDevice,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);

            renderTarget2D_2 = new RenderTarget2D(
                graphics.GraphicsDevice,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        #region Update
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Console.WriteLine(CurrentScreen.ToString());
            prevState = currentState;
            currentState = GamePad.GetState(PlayerIndex.One);
            prevKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();
            // Allows the game to exit            
            if (currentState.Buttons.Back == ButtonState.Pressed)
                this.Exit();            
            switch (CurrentScreen) {
                case ScreenState.Title:                    
                    UpdateTitleScreen();
                    break;
                case ScreenState.Game:
                    UpdateGameScreen(gameTime);                                        
                    break;
                case ScreenState.Paused:
                    UpdatePausedScreen();
                    break;
                case ScreenState.Help:
                    UpdateHelpScreen();
                    break;
                case ScreenState.Credit:
                    UpdateCreditScreen();
                    break;
            }

            if (!songStart) {
                MediaPlayer.Play(backgroundMusic);
                MediaPlayer.Volume = .8f;
                songStart = true;
            }

            alpha++;
            if (alpha > 510) alpha = 0;

            distance += 0.001f;
            if (distance > 1) distance = 0;

            rainDistance += 0.02f;
            if (rainDistance > 1) rainDistance = 0;

            if (bloomActive)
            {
                radius += 0.002f;
                if (radius > 0.3f) bloomActive = false;
            }
            else
            {
                radius = 0;
                bloomActive = true;
            }

            //Effect Variable Updates

            for (int i = 0; i < ravePositions.Length; i++)
            {
                if (ravePositions[i].Z < 0)
                {
                    ravePositions[i] = new Vector4((float)random.Next(3, 7) / 10, (float)random.Next(5, 9) / 10, 30f, random.Next(0, 11));
                }
                ravePositions[i].Z -= 1f;
            }

            base.Update(gameTime);            
        }

        private void UpdateHelpScreen() {
            if ((currentState.Buttons.A == ButtonState.Released && prevState.Buttons.A == ButtonState.Pressed)
                || (currentKeyState.IsKeyUp(Keys.A) && prevKeyState.IsKeyDown(Keys.A))) {
                    CurrentScreen = ScreenState.Game;
            }
        }

        private void UpdatePausedScreen() {
            if ((currentState.Buttons.Start == ButtonState.Released && prevState.Buttons.Start == ButtonState.Pressed) 
                || (currentKeyState.IsKeyUp(Keys.Escape) && prevKeyState.IsKeyDown(Keys.Escape)))
            {                
                CurrentScreen = ScreenState.Game;
                return;
            }
            rotation -= .01f;
        }

        private void UpdateTitleScreen() {
            if ((currentState.Buttons.A == ButtonState.Released && prevState.Buttons.A == ButtonState.Pressed)
                || (currentKeyState.IsKeyUp(Keys.A) == true && prevKeyState.IsKeyDown(Keys.A))) {
                CurrentScreen = ScreenState.Help;
                return;
            }
            if ((currentState.Buttons.B == ButtonState.Released && prevState.Buttons.B == ButtonState.Pressed)
            || (currentKeyState.IsKeyUp(Keys.B) == true && prevKeyState.IsKeyDown(Keys.B)))
            {
                CurrentScreen = ScreenState.Credit;
                return;
            }
        }

        private void UpdateCreditScreen(){
            if ((currentState.Buttons.B == ButtonState.Released && prevState.Buttons.B == ButtonState.Pressed)
                    || (currentKeyState.IsKeyUp(Keys.B) == true && prevKeyState.IsKeyDown(Keys.B)))
            {
                CurrentScreen = ScreenState.Title;
                return;
            }
        }

        private void UpdateGameScreen(GameTime gameTime) {

            if (currentState.Buttons.Start == ButtonState.Released && prevState.Buttons.Start == ButtonState.Pressed
                || currentKeyState.IsKeyUp(Keys.Escape) && prevKeyState.IsKeyDown(Keys.Escape))
            {
                CurrentScreen = ScreenState.Paused;
                return;
            }
            resetTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (resetTimer >= 2000) {
               moveGame(new Vector2(-camera.position.X,0));
               resetTimer = 0;
            }
            if (upDiff == 1)
            {
                if (!player.ouroFound)
                {
                    player.Suicide();
                    upDiff = 0;
                }
                else
                {
                    difficulty += .5f;
                    testLevel.ouroHasAppeared = false;
                    testLevel.itemHasAppeared = false;
                }
            }

            testLevel.Update(gameTime);
            camera.Update();
            player.Update(gameTime);           
            
            if (player.CheckDead())
            {
                camera = new Camera(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, 8192, 768);
                testLevel = new Level(camera, graphics);
                testLevel.background.resetMusic();
                player.ResetPlayer();
                difficulty = 1;
                score = 0;
                player.age = 0;
                CurrentScreen = ScreenState.Title;
                return;
            }


            List<Object> enemyTemp = testLevel.LEVELCOMPONENT(levelComponent.Enemies);
            for (int i = 0; i < enemyTemp.Count; i++)
            {
                Enemy temp2 = (Enemy)enemyTemp[i];
                temp2.Update(gameTime);
                collisionDetector.detect(enemyTemp[i], testLevel.LEVELCOMPONENT(levelComponent.Block));
                collisionDetector.detect(enemyTemp[i], testLevel.LEVELCOMPONENT(levelComponent.Platform));
                collisionDetector.detect(enemyTemp[i], testLevel.LEVELCOMPONENT(levelComponent.Floor));
                collisionDetector.detect(player, enemyTemp[i]);
                if (temp2.checkDeath())
                {
                    enemyTemp.RemoveAt(i);
                    i--;
                    score++;
                }
            }
            collisionDetector.detect(player, testLevel.LEVELCOMPONENT(levelComponent.Block));
            collisionDetector.detect(player, testLevel.LEVELCOMPONENT(levelComponent.Platform));
            collisionDetector.detect(player, testLevel.LEVELCOMPONENT(levelComponent.Floor));
            collisionDetector.detect(player, testLevel.collectibles);
            collisionDetector.detect(player, testLevel.ouros);

            foreach (Bullet bullet in activeBullets)
            {
                collisionDetector.detect(player, bullet);
            }

            for (int index = 0; index < activeBullets.Count(); index++)
            {
                collisionDetector.detect(player, activeBullets[index]);
                foreach (Enemy ez in enemyTemp)
                    collisionDetector.detect(activeBullets[index], ez);
                if (!activeBullets[index].onScreen(Game1.camera) || activeBullets[index].bulletType == bulletType.Dead)
                {
                    returnBullet(activeBullets[index]);
                    activeBullets.RemoveAt(index);
                    index--;
                }
                else { activeBullets[index].update(gameTime); }
            }

        }

        public void moveGame(Vector2 Movement) {
            return;
            foreach (Bullet bullet in activeBullets) {
                bullet.position += Movement;
                bullet.updateBoundingBox();
            }
            camera.moveCamera(Movement);
            testLevel.moveLevel(Movement);
            player.position += Movement;
            player.updateBoundingBox();
        }

        #endregion

        #region Draw

        private void DrawPausedScreen() {
            Vector2 cam = camera.topLeft();
            spriteBatch.Draw(PausedScreen, camera.topLeft(), Color.White);            
            spriteBatch.Draw(Ouroboros, new Vector2(camera.topLeft().X + 512, camera.topLeft().Y + 384), null, Color.White, rotation, new Vector2((float)Ouroboros.Width/2, (float)Ouroboros.Height/2),1, SpriteEffects.None, (float)1);
        }

        private void DrawTitleScreen() {
            spriteBatch.Draw(TitleScreenBackground, Vector2.Zero, Color.White); 
        }

        private void DrawCreditScreen() {
            spriteBatch.Draw(creditScreen, camera.topLeft(), Color.White);
        }

        private void DrawGameScreen() {

            spriteBatch.End();

            graphics.GraphicsDevice.SetRenderTarget(renderTarget2D);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, shaderEffect, camera.viewMatrix);
            shaderEffect.CurrentTechnique = shaderEffect.Techniques["NoTech"];

            testLevel.Draw(spriteBatch, graphics, shaderEffect);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, shaderEffect, camera.viewMatrix);
            shaderEffect.CurrentTechnique = shaderEffect.Techniques["NoTech"];

            foreach (Bullet e in activeBullets)
            {
                e.draw(spriteBatch);                
            }
            spriteBatch.Draw(statScreen, new Rectangle((int)camera.position.X - 512, 768, 1024, 128), Color.Black);
            spriteBatch.DrawString(scoreFont, player.GetHealth() + " \n" + score + "\n" + difficulty, camera.topLeft() + new Vector2(270, 794), Color.White);            

            spriteBatch.End();

            graphics.GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, shaderEffect);
            shaderEffect.CurrentTechnique = shaderEffect.Techniques["NoTech"];
            spriteBatch.Draw(renderTarget2D, Vector2.Zero, Color.White);
            
            
        }

        private void DrawHelpScreen() {
            spriteBatch.Draw(helpScreen, camera.topLeft(), Color.White);            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, camera.viewMatrix);
            
            switch (CurrentScreen) { 
                case ScreenState.Title:
                    DrawTitleScreen();
                    break;
                case ScreenState.Game:
                    DrawGameScreen();
                    break;
                case ScreenState.Paused:
                    DrawGameScreen();
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewMatrix);
                    DrawPausedScreen();
                    

                    break;
                case ScreenState.Help:
                    DrawHelpScreen();
                    break;
                case ScreenState.Credit:
                    DrawCreditScreen();
                    break;
            }
       
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public static void returnBullet(Bullet bullet){
            inactiveBullets.Return(bullet);
        }

        public static Bullet retrieveBullet()
        {
            return inactiveBullets.Fetch();
        }

        public static void addBullet(Bullet bullet)
        {
            activeBullets.Add(bullet);
        }
    }
        #endregion
}
