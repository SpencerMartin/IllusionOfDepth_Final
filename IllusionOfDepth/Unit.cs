using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IllusionOfDepth
{
    public class Unit
    {
        public Animation mAnimation;
        public Vector2 mOrigin;
        public Color mTint;
        public float mRotation;
        public float mScale;
        public SpriteEffects mSpriteEffects;

        public Unit() { }

        public Unit(Texture2D spritesheet, Rectangle spriteLocation, Vector2 position, Vector2 origin, Color tint, float rotation, float scale, SpriteEffects spriteEffects, int numberOfCells)
        {
            mAnimation = new Animation(spritesheet, spriteLocation, position, numberOfCells);
            mOrigin = origin;
            mTint = tint;
            mRotation = rotation;
            mScale = scale;
            mSpriteEffects = spriteEffects;
        }

        public virtual void update(Player player)
        {
            Vector2 mousePos;

            mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            mRotation = Convert.ToSingle((-(180 / 3.14159)) * (Math.Atan2(mousePos.Y - mAnimation.mSpritePosition.Y, mousePos.X - mAnimation.mSpritePosition.X)));

            mAnimation.setXPosition(player.mAnimation.getPosition().X);
            mAnimation.setYPosition(player.mAnimation.getPosition().Y);
        }

        public virtual void update(GameTime gameTime)
        {

        }

        public virtual void update(Player player, Enemies enemy)
        {
            Vector2 playerLoc;

            playerLoc = player.mAnimation.mSpritePosition;

            mRotation = Convert.ToSingle((-(180 / 3.14159)) * (Math.Atan2(playerLoc.Y - mAnimation.mSpritePosition.Y, playerLoc.X - mAnimation.mSpritePosition.X)));

            mAnimation.setXPosition(enemy.mAnimation.getPosition().X);
            mAnimation.setYPosition(enemy.mAnimation.getPosition().Y);

        }

        public virtual void update(KeyboardState keyState, GameTime gameTime)
        {
            mAnimation.update(gameTime);
        }

        public virtual void draw(SpriteBatch batch) 
        {
            batch.Draw(mAnimation.mSpriteSourceTexture, mAnimation.mSpritePosition, mAnimation.mCurrentCelLocation, mTint, mRotation, mOrigin, mScale, mAnimation.mSpriteEffects, 0.1f);

        }

        public virtual void draw(SpriteBatch batch, Vector2 cameraLocation, float cameraOffset, int horizon)
        {

        }

        public void cDraw(SpriteBatch batch)
        {
            batch.Draw(mAnimation.mSpriteSourceTexture, mAnimation.mSpritePosition, mAnimation.mCurrentCelLocation, mTint, mRotation, mOrigin, mScale, mAnimation.mSpriteEffects, 0.1f);
        }




    }

    public class Player : Unit
    {
        
        public Player(Texture2D spritesheet, Rectangle spriteLocation, Vector2 position, Vector2 origin, Color tint, float rotation, float scale, SpriteEffects spriteEffects)
        {
            mAnimation = new Animation(spritesheet, spriteLocation, position);
            mOrigin = origin;
            mTint = tint;
            mRotation = rotation;
            mScale = scale;
            mSpriteEffects = spriteEffects;
        }

        public Player(Texture2D spritesheet, int numCells, Rectangle spriteLocation, Vector2 position, Vector2 origin, Color tint, float rotation, float scale, SpriteEffects spriteEffects)
        {
            mAnimation = new Animation(spritesheet, spriteLocation, position, numCells);
            mOrigin = origin;
            mTint = tint;
            mRotation = rotation;
            mScale = scale;
            mSpriteEffects = spriteEffects;
        }
        
        public override void update(KeyboardState keyState, GameTime gameTime)
        {
            mAnimation.isMoving = false;


            if (keyState.IsKeyDown(Keys.D))
            {
                mAnimation.isMoving = true;
                if(mAnimation.mSpritePosition.X < 1280)
                    mAnimation.mSpritePosition.X++;
                mAnimation.mSpriteEffects = SpriteEffects.None;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                mAnimation.isMoving = true;
                if(mAnimation.mSpritePosition.X > 0)
                    mAnimation.mSpritePosition.X--;
                mAnimation.mSpriteEffects = SpriteEffects.FlipHorizontally;
            }
            if (keyState.IsKeyDown(Keys.W))
            {
                mAnimation.isMoving = true;
                if(mAnimation.mSpritePosition.Y > 304)
                    mAnimation.mSpritePosition.Y--;
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                mAnimation.isMoving = true;
                if (mAnimation.mSpritePosition.Y < 656)                  
                    mAnimation.mSpritePosition.Y++;
            }
            


            mAnimation.update(gameTime);
        }

        public override void draw(SpriteBatch batch, Vector2 cameraPos, float cameraOffset, int horizonY )
        {
            float depth = GlobalFunctions.calcDepth(mAnimation.mSpritePosition.Y, horizonY, 656);
            float scale = GlobalFunctions.calcScale(depth);

            Vector2 drawPos = mAnimation.mSpritePosition;
            drawPos.X -= cameraPos.X;
            drawPos.X *= scale; 
            drawPos.X += cameraOffset;

            batch.Draw(mAnimation.mSpriteSourceTexture, drawPos, mAnimation.mCurrentCelLocation, GlobalFunctions.calcTint(depth), mRotation, mOrigin, scale, mAnimation.mSpriteEffects, depth);
        }



    }



}
