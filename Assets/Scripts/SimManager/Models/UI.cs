using System.Collections.Generic;

namespace Anthology.Models
{
    /// <summary>
    /// Manages UI for web interface.
    /// </summary>
    public static class UI
    {
        /// <summary>
        /// The size of the UI grid (n x n).
        /// </summary>
        public static int GridSize { get; set; } = 6;

        /// <summary>
        /// Time between movement actions in milliseconds.
        /// </summary>
        public static int SleepMove { get; set; } = 1000;

        /// <summary>
        /// Milliseconds between action ticks when still.
        /// </summary>
        public static int SleepStill { get; set; } = 10;

        /// <summary>
        /// Whether or not the UI is paused.
        /// </summary>
        public static bool Paused { get; set; } = false;

        /// <summary>
        /// Each slot in the array is the characters contained on one board space.
        /// </summary>
        public static List<List<string>> Board { get; set; } = new();

        /// <summary>
        /// Name of the agent selected in the agent dropdown.
        /// </summary>
        public static string ActiveAgent { get; set; } = "-None-";

        /// <summary>
        /// Clears the board GUI and fills all the cells with empty strings.
        /// </summary>
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

        /// <summary>
        /// UI state to be updated per tick.
        /// </summary>
        public static void Update() { }
    }
}
