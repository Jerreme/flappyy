
namespace Assets.Scripts.UIs
{
    class StaticVariables
    {
        public static float scoreRate = 1f;
        public static int incrementScoreBy = 1;

        public static float game_Speed { get; set; }
        public static float game_Smooth { get; set; }

        public static int game_PoolSize { get; set; }
        public static int game_Score { get; set; } 


        public static readonly string mode1 = "Calm";
        public static readonly string mode2 = "Normal";
        public static readonly string mode3 = "Crazy";

        public static void resetVariables()
        {
            scoreRate = 1f;
            incrementScoreBy = 1;
        }
    }
}
