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

//Implemented by Dean Lawson in ParallaxMG

namespace IllusionOfDepth
{
    class GlobalFunctions
    {
        public static int MAX_NUM_PARTICLES = 2000;

        public static float calcDepth(float yPos, int horizonY, int screenHeight)
        {
            return (yPos - horizonY) / (screenHeight - horizonY);
        }

        public static float calcScale(float depth)
        {
            return 0.25f + (depth * 0.75f);
        }

        public static Color calcTint(float depth)
        {
            float greyValue = 0.75f + (depth * 0.25f);
            return new Color(greyValue, greyValue, greyValue);
        }
    }
}
