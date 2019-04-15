using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IllusionOfDepth
{
    public class Enemies : Unit
    {

        public Enemies(Texture2D spritesheet, Rectangle spriteLocation, Vector2 position, Vector2 origin, Color tint, float rotation, float scale, SpriteEffects spriteEffects, bool moving, int numOfCells, int celTime)
        {
            mAnimation = new Animation(spritesheet, spriteLocation, position, numOfCells, celTime );
            mOrigin = origin;
            mTint = tint;
            mRotation = rotation;
            mScale = scale;
            mSpriteEffects = spriteEffects;
            mAnimation.isMoving = moving;
        }

        public override void update(GameTime gameTime)
        {
            mAnimation.update(gameTime);
        }
        
        
        public override void draw(SpriteBatch batch)
        {
            batch.Draw(mAnimation.mSpriteSourceTexture, mAnimation.mSpritePosition, mAnimation.mCurrentCelLocation, mTint, 0.0f, new Vector2(16, 16), 1.0f, SpriteEffects.None, 0.0f);   
        }

        public void draw(SpriteBatch batch, Vector2 drawPos, Color tint, float scale, float depth)
        {
            batch.Draw(mAnimation.mSpriteSourceTexture, drawPos, mAnimation.mCurrentCelLocation, tint, 0.0f, mOrigin, scale, SpriteEffects.None, depth);
        }

        public void stopMoving()
        {
            mAnimation.isMoving = false;
        }

    }





}
