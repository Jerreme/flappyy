using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.UIs
{
    class StaticVariables
    {
        public static float scoreRate = 0.2f;
        public static int incrementScoreBy = 1;

        public static float game_Speed { get; set; }
        public static float game_Smooth { get; set; }


        public static int game_PoolSize { get; set; }
        public static int game_Score { get; set; } 


        public static readonly string mode1 = "Calm";
        public static readonly string mode2 = "Normal";
        public static readonly string mode3 = "Crazy";
    }
}
