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
    public class cEffectManager
    {
        public List<Effect> m_lAllEffects;

        public cEffectManager()
        {
            m_lAllEffects = new List<Effect>();
        }

        public void LoadContent(ContentManager Content)
        {
            Effect.LoadContent(Content);
        }

        public void AddEffect(EffectType type, Vector2 matchPos)
        {
            Effect tempEffect = new Effect(this);
            tempEffect.Initialize(type, matchPos);
            m_lAllEffects.Add(tempEffect);
        }

        public void Update(GameTime gameTime, Vector2 matchPos, Player player)
        {
            for (int i = m_lAllEffects.Count() - 1; i >= 0; i--)
            {
                m_lAllEffects[i].Update(gameTime, matchPos, player);

                if (!m_lAllEffects[i].isAlive())
                {
                    m_lAllEffects.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch batch, Microsoft.Xna.Framework.Graphics.Effect effect)
        {
            foreach (Effect e in m_lAllEffects)
            {
                e.Draw(batch, effect);
            }
        }

        public int GetNumActiveParticles()
        {
            int total = 0;

            foreach(Effect e in m_lAllEffects)
            {
                total += e.m_allParticles.Count;
            }

            return total;
        }

    }
}
