using Assets.Scripts.UIs;
using System;

namespace Assets.Scripts.ObstacleScripts
{
    class ObstacleGenerator_Computer
    {
        const float height_defLow = 2.5f;
        const float height_defHigh = 4.2f;

        

        public float getScore()
        {
            return StaticVariables.game_Score;
            //return (float)Math.Round(float.Parse(InGameScoreText.text.Replace("Score: ", "")), 2);
        }
        public float map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            if (value < fromLow) 
                return toLow;
            else if (value > fromHigh)
                return toHigh;
            else 
                return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }

        
    }
}
