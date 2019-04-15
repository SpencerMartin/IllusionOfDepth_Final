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
    public enum EffectType
    {
        smoke,
        fire,
        explosion,
        snow,
        spiral,
        spew
    }

    public class Effect
    {
        public cEffectManager m_emManager;
        public EffectType m_eType;

        public Texture2D particleTexture;
        static public Texture2D snowflakeTexture;
        static public Texture2D circleTexture;
        static public Texture2D starTexture;
        static public Texture2D mediumDeanTexture;
        static public Texture2D smallDeanTexture;


        public Vector2 m_vOrigin;
        public int m_iRadius;

        public BlendState m_eBlendType;

        public int m_iEffectDuration;
        public int m_iNewParticleAmmount;
        public int m_iBurstFrequencyMS;
        public int m_iBurstCountdownMS;
        public Vector2 m_vMatchPos;

        public Random myRandom;

        public List<Particle> m_allParticles;


        public Effect(cEffectManager manager)
        {
            m_emManager = manager;
            m_allParticles = new List<Particle>();
            myRandom = new Random();
        }

        static public void LoadContent(ContentManager content)
        {
            snowflakeTexture = content.Load<Texture2D>("snowFlake");
            circleTexture = content.Load<Texture2D>("smallWhiteCircle");
            starTexture = content.Load<Texture2D>("whiteStar");
            mediumDeanTexture= content.Load<Texture2D>("dean_head_small");
        }

        public bool isAlive()
        {
            if (m_iEffectDuration > 0)
                return true;
            if (m_allParticles.Count() > 0)
                return true;
            return false;
        }

        public void Initialize(EffectType pType, Vector2 matchPos)
        {
            m_eType = pType;

            switch (m_eType)
            {
                case EffectType.fire:
                    m_vMatchPos = matchPos;
                    FireInitialize(matchPos);
                    break;
                case EffectType.smoke:
                    SmokeInitialize();
                    break;
                case EffectType.explosion:
                    ExplosionInitialize(matchPos);
                    break;
                case EffectType.snow:
                    SnowInitialize();
                    break;
                case EffectType.spiral:
                    SpiralInitialize();
                    break;
                case EffectType.spew:
                    SpewInitialize(matchPos);
                    break;

            }
        }

        public void SpiralInitialize()
        {
            //Explosion
            particleTexture = starTexture;
            m_iEffectDuration = 60000;
            m_iNewParticleAmmount = 1;
            m_iBurstFrequencyMS = 64;
            m_iBurstCountdownMS = m_iBurstFrequencyMS;

            m_vOrigin = new Vector2(640, 100);
            m_iRadius = 50;
            m_eBlendType = BlendState.NonPremultiplied;

        }
        public void SnowInitialize()
        {
            //Explosion
            particleTexture = snowflakeTexture;
            m_iEffectDuration = 60000;
            m_iNewParticleAmmount = 1;
            m_iBurstFrequencyMS = 64;
            m_iBurstCountdownMS = m_iBurstFrequencyMS;

            m_vOrigin = new Vector2(640, -50);
            m_iRadius = 50;
            m_eBlendType = BlendState.NonPremultiplied;

        }

        public void FireInitialize(Vector2 matchPos)
        {
            //Fire
            particleTexture = circleTexture;
            m_iEffectDuration = 200;
            m_iNewParticleAmmount = 15;
            m_iBurstFrequencyMS = 16;
            m_iBurstCountdownMS = m_iBurstFrequencyMS;
            m_vMatchPos = matchPos;

            m_vOrigin = new Vector2(640, 400);
            m_iRadius = 15;
            m_eBlendType = BlendState.Additive;

        }
        public void SmokeInitialize()
        {
            //Smoke
            particleTexture = circleTexture;
            m_iEffectDuration = 60000;
            m_iNewParticleAmmount = 4;
            m_iBurstFrequencyMS = 16;
            m_iBurstCountdownMS = m_iBurstFrequencyMS;

            m_vOrigin = new Vector2(640, 640);
            m_iRadius = 50;
            m_eBlendType = BlendState.Additive;


        }
        public void ExplosionInitialize(Vector2 matchPos)
        {
            //Explosion
            particleTexture = circleTexture;
            m_iEffectDuration = 16;
            m_iNewParticleAmmount = 400;
            m_iBurstFrequencyMS = 16;
            m_iBurstCountdownMS = m_iBurstFrequencyMS;
            m_vMatchPos = matchPos;

            m_vOrigin = new Vector2(200, 720);
            m_iRadius = 20;
            m_eBlendType = BlendState.NonPremultiplied;

        }

        //Mine
        public void SpewInitialize(Vector2 matchPos)
        {
            //Spew particles
            particleTexture = mediumDeanTexture;
            m_iEffectDuration = 16;
            m_iNewParticleAmmount = 16;
            m_iBurstFrequencyMS = 16;
            m_iBurstCountdownMS = m_iBurstFrequencyMS;

            m_vOrigin = new Vector2(particleTexture.Width / 2, particleTexture.Height / 2);
            m_iRadius = 20;
            m_eBlendType = BlendState.NonPremultiplied;

        }

        public void createParticle(Vector2 matchPos)
        {
            switch (m_eType)
            {
                case EffectType.fire:
                    createFireParticle();
                    break;
                case EffectType.smoke:
                    createSmokeParticle();
                    break;
                case EffectType.explosion:
                    createExplosionParticle();
                    break;
                case EffectType.snow:
                    createSnowParticle();
                    break;
                case EffectType.spiral:
                    createSpiralParticle();
                    break;
                case EffectType.spew:
                    createSpewParticle(matchPos);
                    break;
            }
        }


        public void createSpiralParticle()
        {
            int initAge = 3000; //3 seconds

            Vector2 initPos = m_vOrigin;


            Vector2 initVel = new Vector2(((float)(100.0f * Math.Cos(m_iEffectDuration))),
                                          ((float)(100.0f * Math.Sin(m_iEffectDuration))));

            Vector2 initAcc = new Vector2(0, 75);
            float initDamp = 1.0f;

            float initRot = 0.0f;
            float initRotVel = 2.0f;
            float initRotDamp = 0.99f;

            float initScale = 0.2f;
            float initScaleVel = 0.2f;
            float initScaleAcc = -0.1f;
            float maxScale = 1.0f;

            Color initColor = Color.DarkRed;
            Color finalColor = Color.DarkRed;
            finalColor *= 0;
            //finalColor.A = 0;
            int fadeAge = initAge;

            Particle tempParticle = new Particle();
            tempParticle.Create(particleTexture, initAge, initPos, initVel, initAcc, initDamp, initRot, initRotVel, initRotDamp, initScale, initScaleVel, initScaleAcc, maxScale, initColor, finalColor, fadeAge);
            m_allParticles.Add(tempParticle);
        }

        public void createFireParticle()
        {
            int initAge = 500 + (int)myRandom.Next(500); //3 seconds
            int fadeAge = initAge - (int)myRandom.Next(100);

            Vector2 initPos = m_vMatchPos;
            
            Vector2 initVel = Vector2.Zero;
            initVel.X = -20;
            initVel.Y = 0;

            Vector2 initAcc = new Vector2(0, -myRandom.Next(300));

            float initDamp = 0.96f;

            float initRot = 0.0f;
            float initRotVel = 2.0f;
            float initRotDamp = 0.99f;

            float initScale = 0.5f;
            float initScaleVel = -0.1f;
            float initScaleAcc = 0.0f;
            float maxScale = 1.0f;

            Color initColor = Color.DarkBlue;
            Color finalColor = Color.DarkOrange;
            finalColor.A = 0;


            Particle tempParticle = new Particle();
            tempParticle.Create(particleTexture, initAge, initPos, initVel, initAcc, initDamp, initRot, initRotVel, initRotDamp, initScale, initScaleVel, initScaleAcc, maxScale, initColor, finalColor, fadeAge);
            m_allParticles.Add(tempParticle);
        }
        public void createSmokeParticle()
        {
            int initAge = 5000 + (int)myRandom.Next(5000);
            int fadeAge = initAge - (int)myRandom.Next(5000);

            Vector2 initPos = m_vOrigin;
            Vector2 offset;
            offset.X = ((float)(myRandom.Next(m_iRadius) * Math.Cos(myRandom.Next(360))));
            offset.Y = ((float)(myRandom.Next(m_iRadius) * Math.Sin(myRandom.Next(360))));
            initPos += offset;

            Vector2 offset2 = Vector2.Zero;
            offset2.X += (float)(400 * Math.Cos(m_iEffectDuration / 500.0f));
            initPos += offset2;

            Vector2 initVel = Vector2.Zero;
            initVel.X = 0;//
            initVel.Y = -30 - myRandom.Next(30);

            Vector2 initAcc = new Vector2(10 + myRandom.Next(10), 0);

            float initDamp = 1.0f;

            float initRot = 0.0f;
            float initRotVel = 0.0f;
            float initRotDamp = 1.0f;

            float initScale = 0.6f;
            float initScaleVel = ((float)myRandom.Next(10)) / 50.0f;
            float initScaleAcc = 0.0f;
            float maxScale = 3.0f;

            Color initColor = Color.Black;
            initColor.A = 128;
            Color finalColor = new Color(32, 32, 32);
            finalColor.A = 0;


            Particle tempParticle = new Particle();
            tempParticle.Create(particleTexture, initAge, initPos, initVel, initAcc, initDamp, initRot, initRotVel, initRotDamp, initScale, initScaleVel, initScaleAcc, maxScale, initColor, finalColor, fadeAge);
            m_allParticles.Add(tempParticle);
        }

        public void createExplosionParticle()
        {
            int initAge = 1500 + (int)myRandom.Next(2000);
            int fadeAge = initAge / 2;

            Vector2 initPos = m_vMatchPos;
            Vector2 offset;
            offset.X = ((float)(myRandom.Next(m_iRadius) * Math.Cos(myRandom.Next(360))));
            offset.Y = ((float)(myRandom.Next(m_iRadius) * Math.Sin(myRandom.Next(360))));
            initPos += offset;

            Vector2 initVel = Vector2.Zero;
            initVel.X = myRandom.Next(500) + (offset.X * 30);
            initVel.Y = -60 * Math.Abs(offset.Y);

            Vector2 initAcc = new Vector2(0, 400);

            float initDamp = 1.0f;

            float initRot = 0.0f;
            float initRotVel = initVel.X / 50.0f;
            float initRotDamp = 0.97f;

            float initScale = 0.1f + ((float)myRandom.Next(10)) / 50.0f;
            float initScaleVel = ((float)myRandom.Next(10) - 5) / 50.0f;
            float initScaleAcc = 0.0f;
            float maxScale = 1.0f;

            byte randomGray = (byte)(myRandom.Next(128) + 128);
            Color initColor = new Color(randomGray, 0, 0);

            Color finalColor = new Color(32, 32, 32);
            finalColor = Color.Black;

            Particle tempParticle = new Particle();
            tempParticle.Create(particleTexture, initAge, initPos, initVel, initAcc, initDamp, initRot, initRotVel, initRotDamp, initScale, initScaleVel, initScaleAcc, maxScale, initColor, finalColor, fadeAge);
            m_allParticles.Add(tempParticle);
        }

        public void createSnowParticle()
        {
            float initScale = 0.1f + ((float)myRandom.Next(10)) / 20.0f;
            float initScaleVel = 0.0f;
            float initScaleAcc = 0.0f;
            float maxScale = 1.0f;

            int initAge = (int)(10000 / initScale);
            int fadeAge = initAge;

            Vector2 initPos = m_vOrigin;
            Vector2 offset;
            offset.X = ((float)(myRandom.Next(m_iRadius) * Math.Cos(myRandom.Next(360))));
            offset.Y = ((float)(myRandom.Next(m_iRadius) * Math.Sin(myRandom.Next(360))));
            initPos += offset;

            Vector2 offset2 = Vector2.Zero;
            offset2.X += (float)(600 * Math.Cos(m_iEffectDuration / 500.0));
            initPos += offset2;


            Vector2 initVel = Vector2.Zero;
            initVel.X = myRandom.Next(10) - 5;
            initVel.Y = 100 * initScale;

            Vector2 initAcc = new Vector2(0, 0);

            float initDamp = 1.0f;

            float initRot = 0.0f;
            float initRotVel = initVel.X / 5.0f; ;
            float initRotDamp = 1.0f;

            Color initColor = Color.White;
            Color finalColor = Color.White;
            finalColor.A = 0;

            Particle tempParticle = new Particle();
            tempParticle.Create(particleTexture, initAge, initPos, initVel, initAcc, initDamp, initRot, initRotVel, initRotDamp, initScale, initScaleVel, initScaleAcc, maxScale, initColor, finalColor, fadeAge);
            m_allParticles.Add(tempParticle);
        }

        public void createSpewParticle(Vector2 matchPos)
        {
            int duration = 3000;

            Vector2 pos = matchPos;
            Vector2 offset;
            offset.X = ((float)(myRandom.Next(m_iRadius) * Math.Cos(myRandom.Next(360))));
            offset.Y = ((float)(myRandom.Next(m_iRadius) * Math.Sin(myRandom.Next(360))));

            if (pos.Y >= 720)//don't create because it's off the screen
                return;

            Vector2 vel = new Vector2(myRandom.Next(-500, 500), -400);
            vel.X = myRandom.Next(500) + (offset.X * 30);
            vel.Y = -60 * Math.Abs(offset.Y);
            Vector2 acc = new Vector2(0.0f, 400.0f);
            float velDamp = 0.99f;

            float rot = 0.0f;
            float rotVel = 1.0f;
            float rotDamp = 1.0f;

            float scale = 0.1f;
            float scaleVel = 0.1f;
            float scaleAcc = -0.1f;
            float maxScale = 0.5f;

            Color initColor = Color.White;
            Color finalColor = Color.Red;
            int fadeAge = duration / 2;

            Particle temp = new Particle();
            temp.Create(mediumDeanTexture, duration, pos, vel, acc, velDamp, rot, rotVel, rotDamp, scale, scaleVel, scaleAcc, maxScale, initColor, finalColor, fadeAge);

            m_allParticles.Add(temp);

        }

        public void Update(GameTime gameTime, Vector2 matchPos, Player player)
        {
            m_iEffectDuration -= gameTime.ElapsedGameTime.Milliseconds;
            m_iBurstCountdownMS -= gameTime.ElapsedGameTime.Milliseconds;

            if ((m_iBurstCountdownMS <= 0) && (m_iEffectDuration >= 0))
            {
                for (int i = 0; i < m_iNewParticleAmmount; i++)
                    createParticle(matchPos);
                m_iBurstCountdownMS = m_iBurstFrequencyMS;
            }

            for (int i = m_allParticles.Count() - 1; i >= 0; i--)
            {
               m_allParticles[i].Update(gameTime);

                if (m_allParticles[i].m_iAge <= 0)
                {
                    if(this.m_eType == EffectType.spew) 
                        m_emManager.AddEffect(EffectType.explosion, m_allParticles[i].m_vPos);
                    m_allParticles.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch batch, Microsoft.Xna.Framework.Graphics.Effect effect)
        {
            batch.Begin(SpriteSortMode.BackToFront, m_eBlendType, null, null, null, effect);
            foreach (Particle p in m_allParticles)
            {
                p.Draw(batch);
            }
            batch.End();

        }
    }
}
