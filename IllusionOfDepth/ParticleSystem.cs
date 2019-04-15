using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IllusionOfDepth
{
    public class ParticleSystem
    {
        List<Particle> mParticleList;


        ParticleSystem()
        {
            mParticleList = new List<Particle>(GlobalFunctions.MAX_NUM_PARTICLES);
        }

        void addParticle(Particle part)
        {
            mParticleList.Add(part);
        }

        void updateAll(GameTime gameTime, int screenHeight)
        {
            foreach(Particle part in mParticleList)
            {
                part.Update(gameTime);
            }
        }






    }
}
