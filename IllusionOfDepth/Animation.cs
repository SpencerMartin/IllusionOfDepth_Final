using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IllusionOfDepth
{
    public class Animation
    {
        //source data
        public Texture2D mSpriteSourceTexture;
        public Rectangle mCurrentCelLocation;
        public List<Sprite> mSprites;


        //Destination data
        public Vector2 mSpritePosition;

        //Animation data
        public int mCurrentCel = 0;
        public int mNumberOfCels = 1;

        public int msPerCel = 125;
        public int msUntilNextCel;

        //slow in/out
        public bool isMoving;
        public SpriteEffects mSpriteEffects;

        public Vector2 mVelocity;
        public Vector2 mMaxVelocity;

        public Animation()
        {


        }

        //particle
        public Animation(Texture2D texture)
        {
            mSprites = new List<Sprite>();
            Sprite temp = new Sprite();
            temp.Initialize(texture);
            mSprites.Add(temp);
            mCurrentCel = 0;
            mNumberOfCels = 0;
            msPerCel = 16;
            msUntilNextCel = msPerCel;
            isMoving = false;
        }

        public Animation(Texture2D sourceSheet, Rectangle startingSource, Vector2 startingPosition)
        {
            mSpriteSourceTexture = sourceSheet;
            mCurrentCelLocation = startingSource;
            mSpritePosition = startingPosition;
            mSpriteEffects = SpriteEffects.None;
            mVelocity = new Vector2(0, 0);
            mMaxVelocity = new Vector2(5, 0);
            msUntilNextCel = msPerCel;
            
        }

        public Animation(Texture2D sourceSheet, Rectangle startingSource, Vector2 startingPosition, int numberOfCels)
        {
            mSpriteSourceTexture = sourceSheet;
            mCurrentCelLocation = startingSource;
            mSpritePosition = startingPosition;
            mSpriteEffects = SpriteEffects.None;
            mVelocity = new Vector2(0, 0);
            mMaxVelocity = new Vector2(5, 0);
            msUntilNextCel = msPerCel;
            mNumberOfCels = numberOfCels;
        }

        public Animation(Texture2D sourceSheet, Rectangle startingSource, Vector2 startingPosition, int numberOfCels, int celTime)
        {
            mSpriteSourceTexture = sourceSheet;
            mCurrentCelLocation = startingSource;
            mSpritePosition = startingPosition;
            mSpriteEffects = SpriteEffects.None;
            mVelocity = new Vector2(0, 0);
            mMaxVelocity = new Vector2(5, 0);
            msPerCel = celTime;
            msUntilNextCel = msPerCel;
            mNumberOfCels = numberOfCels;
        }

        public void update(GameTime gameTime)
        {

            //timing
            msUntilNextCel -= gameTime.ElapsedGameTime.Milliseconds;

            if(msUntilNextCel <= 0 && isMoving )
            {
                mCurrentCel++;
                msUntilNextCel = msPerCel;
            }

            if (mCurrentCel >= mNumberOfCels)
                mCurrentCel = 0;

            mCurrentCelLocation.X = mCurrentCelLocation.Width * mCurrentCel+1;

        }


        //accessors and mutators
        public Vector2 getPosition()
        {
            return mSpritePosition;
        }
        public void setXPosition(Vector2 position)
        {
            mSpritePosition = position;
        }

        public void setXPosition(float position)
        {
            mSpritePosition.X = position;
        }

        public void setYPosition(float position)
        {
            mSpritePosition.Y = position;
        }

        public Vector2 getVelocity()
        {
            return mVelocity;
        }

        public void setVelocity( Vector2 velocity )
        {
            mVelocity = velocity;
        }

        public void setXVelocity( int velocity )
        {
            mVelocity.X = velocity;
        }

        public void setYVelocity(int velocity)
        {
            mVelocity.Y = velocity;
        }

        public void draw(SpriteBatch batch)
        {
            Sprite temp = getCurrentSprite();
            batch.Draw(temp.m_tTexture, mSpritePosition, temp.m_rSrcRect, temp.m_cColor, temp.m_fRotation, temp.m_vOrigin, temp.m_fScale, temp.m_spEff, temp.m_fDepth);
        }

        public Sprite getCurrentSprite()
        {
            return mSprites[mCurrentCel];
        }

    }



    public class Sprite
    {
        public Texture2D m_tTexture;
        public Vector2 m_vOrigin;
        public Vector2 m_vPos;
        public Rectangle m_rSrcRect;
        public float m_fRotation;
        public float m_fScale;
        public float m_fDepth;
        public Color m_cColor;
        public SpriteEffects m_spEff;

        public Sprite()
        {
        }

        public void Initialize(Texture2D texture)
        {
            m_tTexture = texture;
            m_rSrcRect.X = 0;
            m_rSrcRect.Y = 0;
            m_rSrcRect.Width = m_tTexture.Width;
            m_rSrcRect.Height = m_tTexture.Height;

            m_vOrigin.X = m_rSrcRect.Width / 2;
            m_vOrigin.Y = m_rSrcRect.Height / 2;

            m_fScale = 1.0f;
            m_fRotation = 0.0f;
            m_fDepth = 1.0f;
            m_cColor = Color.White;
        }

        public void Initialize(Texture2D texture, Rectangle srcRect)
        {
            m_tTexture = texture;
            m_rSrcRect = srcRect;

            m_vOrigin.X = m_rSrcRect.Width / 2;
            m_vOrigin.Y = m_rSrcRect.Height / 2;

            m_fScale = 1.0f;
            m_fRotation = 0.0f;
            m_fDepth = 1.0f;
            m_cColor = Color.White;
            m_spEff = SpriteEffects.None;
        }

        public void Initialize(Texture2D texture, Rectangle srcRect, float scale, float depth, float rotation, Color color, SpriteEffects effect)
        {
            m_tTexture = texture;
            m_rSrcRect = srcRect;

            m_vOrigin.X = m_rSrcRect.Width / 2;
            m_vOrigin.Y = m_rSrcRect.Height / 2;

            m_fScale = scale;
            m_fRotation = rotation;
            m_fDepth = depth;
            m_cColor = color;
            m_spEff = effect;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(m_tTexture, m_vPos, m_rSrcRect, m_cColor, m_fRotation, m_vOrigin, m_fScale, SpriteEffects.None, m_fDepth);
        }

    }




}
