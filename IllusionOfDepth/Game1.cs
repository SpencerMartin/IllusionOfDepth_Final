using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace IllusionOfDepth
{
    public enum GameState
    {
        Finish,
        Title,
        Menu,
        Playing,
        Paused,
        Won
    };

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch activeBatch;
        SpriteBatch overlayBatch;

        cEffectManager mEffectManager;
        InputManager mInputManager;
        GameState mGameState;

        //timing
        double m_iElapsedMilliseconds = 0;
        int m_iFrameCount = 0;
        int m_iFPS = 0;
        string fps_string;

        //Main Textures
        Texture2D mDeanRunnerTexture,
                  mBackgroundTexture,
                  mTitleTexture,
                  mMenuTexture,
                  mTreeTexture,
                  mBulletTexture,
                  mHamsterTexture,
                  mDeanSmallHead;

        //Units
        Player mDeanRunner;
        Unit mCursor;
        const int NUM_TREES = 12;
        Vector2[] treeLocs = new Vector2[NUM_TREES];
        List<Enemies> treeUnitList = new List<Enemies>();
        const int NUM_HAM = 6;
        Vector2[] hamLocs = new Vector2[NUM_HAM];
        List<Enemies> hamUnitList = new List<Enemies>();

        //points
        int[] gHamsFound = new int[NUM_HAM];
        string found_ham_string;

        //Game Display data
        SpriteFont Timeless;
        Language mGameLanguage;
        int gHorizon;

        //camera
        Vector2 cameraPosition;

        //Particle
        double explosionTimer;

        //Shaders
        Microsoft.Xna.Framework.Graphics.Effect gSimple;
        Microsoft.Xna.Framework.Graphics.Effect gWaveEffect;
        EffectParameter gSeconds;
        RenderTarget2D tempTarget;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            mInputManager = new InputManager();
            mEffectManager = new cEffectManager();
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
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            for (int i = 0; i < NUM_HAM; i++)
                gHamsFound[i] = 0;

            mGameLanguage = new Language();
            mGameState = GameState.Title;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            activeBatch = new SpriteBatch(GraphicsDevice);
            overlayBatch = new SpriteBatch(GraphicsDevice);
            Timeless = Content.Load<SpriteFont>("Timeless");
            mEffectManager.LoadContent(Content);

            //shader stuff
            gSimple = Content.Load<Microsoft.Xna.Framework.Graphics.Effect>("redTint");
            gWaveEffect = Content.Load<Microsoft.Xna.Framework.Graphics.Effect>("waveEffect");
            gSeconds = gWaveEffect.Parameters["time"];

            gHorizon = graphics.PreferredBackBufferHeight / 3;  //horizon at 1/3 of screen

            //Texture Loading............................................................................
            loadTextures();

            //Unit Loading...............................................................................
            loadUnits();

            tempTarget = new RenderTarget2D(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            double timePassed = gameTime.ElapsedGameTime.TotalMilliseconds;
            m_iElapsedMilliseconds += timePassed;
            explosionTimer += timePassed;

            if (m_iElapsedMilliseconds > 1000)
            {
                m_iElapsedMilliseconds -= 1000;
                m_iFPS = m_iFrameCount;
                m_iFrameCount = 0;
            }

            if ( explosionTimer > 4000)
            {
                explosionTimer = 0;
                Random rand = new Random();
                Vector2 loc;
                loc.X = rand.Next(graphics.PreferredBackBufferWidth);
                loc.Y = rand.Next(graphics.PreferredBackBufferHeight);

                if(mGameState == GameState.Won)
                    mEffectManager.AddEffect(EffectType.explosion, loc);
            }


            mGameState = mInputManager.processInputs(mGameState, mDeanRunner, gameTime, mGameLanguage);
            foreach (Enemies ham in hamUnitList)
                ham.update(gameTime);

            if (mGameState == GameState.Title) { }
            if (mGameState == GameState.Menu) {
                mDeanRunner = new Player(mDeanRunnerTexture, 12, new Rectangle(0, 0, 128, 128), new Vector2(640, 480), new Vector2(64, 64), Color.White, 0.0f, 1.0f, SpriteEffects.None);
                gHamsFound = new int[NUM_HAM];
                loadUnits();

                if (mInputManager.menuOption == 1)      //play
                    mCursor = new Unit(mBulletTexture, new Rectangle(0, 0, 8, 8), new Vector2(350, 250), new Vector2(4, 4), Color.White, 0.0f, 1.0f, SpriteEffects.None, 1);
                else if (mInputManager.menuOption == 2) //Controls
                    mCursor = new Unit(mBulletTexture, new Rectangle(0, 0, 8, 8), new Vector2(350, 300), new Vector2(4, 4), Color.White, 0.0f, 1.0f, SpriteEffects.None, 1);
                else if (mInputManager.menuOption == 3) //Options
                    mCursor = new Unit(mBulletTexture, new Rectangle(0, 0, 8, 8), new Vector2(350, 350), new Vector2(4, 4), Color.White, 0.0f, 1.0f, SpriteEffects.None, 1);
                else if (mInputManager.menuOption == 4) //Quit
                    mCursor = new Unit(mBulletTexture, new Rectangle(0, 0, 8, 8), new Vector2(350, 400), new Vector2(4, 4), Color.White, 0.0f, 1.0f, SpriteEffects.None, 1);
            
            }
            if (mGameState == GameState.Playing) {
                //movements handled in InputManager
                int found = checkHamProximity();
                int score = 0;


                if(found == 7)
                {
                }
                else
                {
                    gHamsFound[found] = 1;
                }

                foreach(int i in gHamsFound)
                {
                    if (i == 1)
                        score+=100;
                }

                found_ham_string = "Score: "+score;

                if(score == 600)
                {
                    mGameState = GameState.Won;
                }

            }
            if (mGameState == GameState.Paused)
            {
                if (mInputManager.menuOption == 1)
                    mCursor = new Unit(mBulletTexture, new Rectangle(0, 0, 8, 8), new Vector2(350, 250), new Vector2(4, 4), Color.White, 0.0f, 1.0f, SpriteEffects.None, 1);
                else if (mInputManager.menuOption == 2)
                    mCursor = new Unit(mBulletTexture, new Rectangle(0, 0, 8, 8), new Vector2(350, 300), new Vector2(4, 4), Color.White, 0.0f, 1.0f, SpriteEffects.None, 1);
                else if (mInputManager.menuOption == 3)
                    mCursor = new Unit(mBulletTexture, new Rectangle(0, 0, 8, 8), new Vector2(350, 350), new Vector2(4, 4), Color.White, 0.0f, 1.0f, SpriteEffects.None, 1);
            }
            if (mGameState == GameState.Won)
            {
                mEffectManager.Update(gameTime, mDeanRunner.mAnimation.mSpritePosition, mDeanRunner);
            }
            if (mGameState == GameState.Finish)
                Exit();

            gSeconds.SetValue((float)gameTime.TotalGameTime.Milliseconds / 1000);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //FPS Handling
            m_iFrameCount++;
            System.Console.WriteLine("FPS is: " + m_iFPS);
            fps_string = "" + m_iFPS;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            

            if (mGameState == GameState.Title)
            {
                GraphicsDevice.Clear(Color.Black);
                drawTitle();
            }
            else if (mGameState == GameState.Menu)
            {
                GraphicsDevice.Clear(Color.Black);
                drawMenu();
            }
            else if (mGameState == GameState.Playing)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                drawPlaying();
            }
            else if (mGameState == GameState.Paused)
            {
                GraphicsDevice.Clear(Color.Gray);
                drawPaused();
            }
            else if (mGameState == GameState.Won)
            {
                GraphicsDevice.Clear(Color.Gray);
                drawWon();
            }

            
            base.Draw(gameTime);
        }


        public void loadTextures()
        {
            mDeanRunnerTexture = Content.Load<Texture2D>("dean_cycle");
            mBackgroundTexture = Content.Load<Texture2D>("background");
            mTitleTexture = Content.Load<Texture2D>("title_screen");
            mMenuTexture = Content.Load<Texture2D>("menu_screen");
            mTreeTexture = Content.Load<Texture2D>("tree");
            mBulletTexture = Content.Load<Texture2D>("bullet");
            mHamsterTexture = Content.Load<Texture2D>("hamsters");
            mDeanSmallHead = Content.Load<Texture2D>("dean_head_small");
        }


        public void loadUnits()
        {
            Vector2 origin = new Vector2(64,64);
            Color color = Color.White;
            SpriteEffects effect = SpriteEffects.None;

            treeLocs = new Vector2[NUM_TREES];
            treeUnitList = new List<Enemies>();
            hamLocs = new Vector2[NUM_HAM];
            hamUnitList = new List<Enemies>();
       

            Rectangle spriteLocation = new Rectangle(0, 0, 128, 128);
            mDeanRunner = new Player(mDeanRunnerTexture, 12, spriteLocation, new Vector2(640, 480), origin, color, 0.0f, 1.0f, effect);
            spriteLocation = new Rectangle(0, 0, 8, 8);
            mCursor = new Unit(mBulletTexture, spriteLocation, new Vector2(600, 300), new Vector2(4, 4), color, 0.0f, 1.0f, effect, 1);

            spriteLocation = new Rectangle(0, 0, 384, 384);
            Enemies tree = new Enemies(mTreeTexture, spriteLocation, new Vector2(300, 522), new Vector2(192, 370), Color.White, 0.0f, 1.0f, SpriteEffects.None, false, 1, 0);

            Random randomizer = new Random();
            for (int i = 0; i < NUM_TREES; i++)
            {
                treeLocs[i].X = randomizer.Next(graphics.PreferredBackBufferWidth);
                treeLocs[i].Y = randomizer.Next(gHorizon+64, graphics.PreferredBackBufferHeight - 192);
                treeUnitList.Add(tree);
            }

            spriteLocation = new Rectangle(0, 0, 48, 48);

            for (int i = 0; i < NUM_HAM; i++)
            {
                Enemies hamUnit = new Enemies(mHamsterTexture, spriteLocation, new Vector2(1000, 376), new Vector2(24, 48), Color.White, 0.0f, 1.0f, SpriteEffects.None, true, 4, 50);

                hamLocs[i].X = randomizer.Next(graphics.PreferredBackBufferWidth);
                hamLocs[i].Y = randomizer.Next(gHorizon + 26, graphics.PreferredBackBufferHeight - 26);
                hamUnitList.Add(hamUnit);
            }
        }

        public void drawTitle()
        {
            activeBatch.Begin();
            activeBatch.Draw(mTitleTexture, new Rectangle(0,0, 1280, 720), Color.White);

            activeBatch.End();
        }

        public void drawMenu()
        {
            activeBatch.Begin();

            if (mInputManager.displayControls)
            {
                activeBatch.DrawString(Timeless, mGameLanguage.ControlsString, new Vector2(325, 175), Color.Red, 0.0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0.1f);
                activeBatch.DrawString(Timeless, mGameLanguage.CreditString, new Vector2(500, 525), Color.Red, 0.0f, new Vector2(0, 0), 0.3f, SpriteEffects.None, 0.1f);
            }
            else if (mInputManager.displayOptions)
            {
                activeBatch.DrawString(Timeless, mGameLanguage.LanguageString, new Vector2(325, 175), Color.Red, 0.0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0.1f);
            }
            else
            {
                activeBatch.DrawString(Timeless, mGameLanguage.getLines()[3], new Vector2(375, 225), Color.Red, 0.0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0.1f);
                activeBatch.DrawString(Timeless, mGameLanguage.getLines()[4] + " + " + mGameLanguage.getLines()[11], new Vector2(375, 275), Color.Red, 0.0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0.1f);
                activeBatch.DrawString(Timeless, mGameLanguage.getLines()[14], new Vector2(375, 325), Color.Red, 0.0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0.1f);
                activeBatch.DrawString(Timeless, mGameLanguage.getLines()[9], new Vector2(375, 375), Color.Red, 0.0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0.1f);
                mCursor.cDraw(activeBatch);
            }
            activeBatch.End();

            overlayBatch.Begin();

            overlayBatch.DrawString(Timeless, fps_string, new Vector2(10, 10), Color.Red, 0.0f, new Vector2(0, 0), 0.2f, SpriteEffects.None, 0.1f);

            overlayBatch.End();
        }

        public void drawPlaying()
        {
            Vector2 cameraLocation = new Vector2(mDeanRunner.mAnimation.getPosition().X, 0.0f);
            //Vector2 cameraOffset = new Vector2(graphics.PreferredBackBufferWidth / 2, 0.0f);
            float cameraOffset = graphics.PreferredBackBufferWidth / 2;

            activeBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);

            activeBatch.Draw(mBackgroundTexture, new Rectangle(0, 0, 1280, 720), Color.White);
            
            mDeanRunner.draw(activeBatch, cameraLocation, cameraOffset, gHorizon );

            for (int i = 0; i < NUM_TREES; i++)
            {
                float depth = GlobalFunctions.calcDepth(treeLocs[i].Y, gHorizon, graphics.PreferredBackBufferHeight);
                float scale = GlobalFunctions.calcScale(depth);
                Color tint = GlobalFunctions.calcTint(depth);
                Vector2 drawPos = treeLocs[i];
                drawPos.X -= cameraLocation.X;
                drawPos.X *= scale; 
                drawPos.X += cameraOffset;
                treeUnitList[i].draw(activeBatch, drawPos, tint, scale, depth);
                //activeBatch.Draw(treeAnimations[i].mSpriteSourceTexture, drawPos, treeAnimations[i].mCurrentCelLocation, tint, 0.0f, new Vector2(192, 192), scale, SpriteEffects.None, depth);   

            }
            for (int i = 0; i < NUM_HAM; i++)
            {
                float depth = GlobalFunctions.calcDepth(hamLocs[i].Y, gHorizon, graphics.PreferredBackBufferHeight);
                float scale = GlobalFunctions.calcScale(depth);
                Color tint = GlobalFunctions.calcTint(depth);
                Vector2 drawPos = hamLocs[i];
                drawPos.X -= cameraLocation.X;
                drawPos.X *= scale; 
                drawPos.X += cameraOffset;
                hamUnitList[i].draw(activeBatch, drawPos, tint, scale, depth);
                //activeBatch.Draw(hamUnitList[i].mAnimation.mSpriteSourceTexture, drawPos, hamUnitList[i].mAnimation.mCurrentCelLocation, tint, 0.0f, new Vector2(192, 192), scale, SpriteEffects.None, depth);

            }

            activeBatch.End();

            overlayBatch.Begin();

            overlayBatch.DrawString(Timeless, fps_string, new Vector2(10, 10), Color.Red, 0.0f, new Vector2(0, 0), 0.2f, SpriteEffects.None, 0.1f);
            overlayBatch.DrawString(Timeless, found_ham_string, new Vector2(10, 30), Color.Red, 0.0f, new Vector2(0, 0), 0.2f, SpriteEffects.None, 0.1f);

            overlayBatch.End();
        }

        public void drawPaused()
        {
            activeBatch.Begin();

            if (mInputManager.displayControls)
            {
                activeBatch.DrawString(Timeless, mGameLanguage.ControlsString, new Vector2(325, 175), Color.Red, 0.0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0.1f);
            }
            else
            {
                activeBatch.DrawString(Timeless, mGameLanguage.getLines()[1], new Vector2(375, 225), Color.Red, 0.0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0.1f);
                activeBatch.DrawString(Timeless, mGameLanguage.getLines()[4], new Vector2(375, 275), Color.Red, 0.0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0.1f);
                activeBatch.DrawString(Timeless, mGameLanguage.getLines()[0] + " " + mGameLanguage.getLines()[2], new Vector2(375, 325), Color.Red, 0.0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0.1f);

                mCursor.cDraw(activeBatch);
            }

            activeBatch.End();

            overlayBatch.Begin();

            overlayBatch.DrawString(Timeless, fps_string, new Vector2(10, 10), Color.Red, 0.0f, new Vector2(0, 0), 0.2f, SpriteEffects.None, 0.1f);

            overlayBatch.End();

        }

        public void drawWon()
        {
            activeBatch.Begin();

            activeBatch.DrawString(Timeless, mGameLanguage.getLines()[19], new Vector2(375, 275), Color.Red, 0.0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0.1f);

            activeBatch.End();

            mEffectManager.Draw(activeBatch, gSimple);

        }


       

        public void updateCamera()
        {
            const float MULTIPLIER = 0.05f;

            if (cameraPosition.X < mDeanRunner.mAnimation.mSpritePosition.X)
            {
                cameraPosition.X -= ((cameraPosition.X - mDeanRunner.mAnimation.mSpritePosition.X) * MULTIPLIER);
            }
            else if (cameraPosition.X > mDeanRunner.mAnimation.mSpritePosition.X)
            {
                cameraPosition.X += ((cameraPosition.X - mDeanRunner.mAnimation.mSpritePosition.X) * -MULTIPLIER);
            }
            if (cameraPosition.Y < mDeanRunner.mAnimation.mSpritePosition.Y)
            {
                cameraPosition.Y -= ((cameraPosition.Y - mDeanRunner.mAnimation.mSpritePosition.Y) * MULTIPLIER);
            }
            else if (cameraPosition.Y > mDeanRunner.mAnimation.mSpritePosition.Y)
            {
                cameraPosition.Y += ((cameraPosition.Y - mDeanRunner.mAnimation.mSpritePosition.Y) * -MULTIPLIER);
            }

            if (cameraPosition.X > 1136)
                cameraPosition.X = 1136;
            if (cameraPosition.X < 400)
                cameraPosition.X = 400;
            if (cameraPosition.Y > 1236)
                cameraPosition.Y = 1236;
            if (cameraPosition.Y < 300)
                cameraPosition.Y = 300;


        }

        public int checkHamProximity()
        {
            Vector2 cameraLocation = new Vector2(mDeanRunner.mAnimation.getPosition().X, 0.0f);
            Vector2 cameraOffset = new Vector2(graphics.PreferredBackBufferWidth / 2, 0.0f);

            Vector2 hamLoc,
                    deanLoc;

            deanLoc = mDeanRunner.mAnimation.getPosition();

            Vector2 deanDraw = deanLoc;

            float depth = GlobalFunctions.calcDepth(mDeanRunner.mAnimation.mSpritePosition.Y, gHorizon, 656);
            float scale = GlobalFunctions.calcScale(depth);

            deanDraw.X -= cameraLocation.X;
            deanDraw.X *= scale;
            deanDraw.X += cameraOffset.X;
            deanDraw.Y += (32 * scale);

            depth = GlobalFunctions.calcDepth(mDeanRunner.mAnimation.mSpritePosition.Y, gHorizon, 720);
            scale = GlobalFunctions.calcScale(depth);

            for (int i = 0; i < hamUnitList.Count; i++ )
            {
                hamLoc = hamLocs[i];

                hamLoc.X -= cameraLocation.X;
                hamLoc.X *= scale;
                hamLoc.X += cameraOffset.X;

                if (Vector2.Distance(hamLoc, deanDraw) < 15)
                {
                    hamUnitList[i].stopMoving();
                    return i;
                }

            }

            return 7;
        }

    }
}
