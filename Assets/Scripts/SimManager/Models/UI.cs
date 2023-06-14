using System.Collections.Generic;

namespace Anthology.Models
{
    public static class UI
    {
        /** the size of the UI grid (n x n) */
        public static int GridSize { get; set; } = 6;

        /** ms between movement actions */
        public static int SleepMove { get; set; } = 1000;

        /** ms between action ticks when still */
        public static int SleepStill { get; set; } = 10;

        /** whether or not the UI is paused */
        public static bool Paused { get; set; } = true;

        /** each slot in the array is the characters contained on one board space */
        public static List<List<string>> Board { get; set; } = new();

        /** name of the agent selected in the agent dropdown */
        public static string ActiveAgent { get; set; } = "-None-";

        /** Clears the board GUI and fills all the cells with empty strings */
        public static void Init()
        {
            Board.Clear();
            for (int i = 0; i < GridSize; i++)
            {
                Board[i] = new();
                for (int k = 0; k < GridSize; k++)
                {
                    Board[i].Add(string.Empty);
                }
            }
        }

        public static void Update() { }
    }
}
