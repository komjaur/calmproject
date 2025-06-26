using System;

namespace SurvivalChaos
{
    /// <summary>
    /// Simple Elo rating utilities for free-for-all matches.
    /// </summary>
    public static class EloUtility
    {
        /// <summary>
        /// Calculates new ratings from current ratings and final placements.
        /// Placement of 1 means first place.
        /// </summary>
        /// <param name="ratings">Current Elo ratings.</param>
        /// <param name="placements">Final placements matching the ratings array.</param>
        /// <param name="kFactor">K-factor used in the calculation.</param>
        public static int[] UpdateAfterMatch(int[] ratings, int[] placements, int kFactor = 32)
        {
            int count = ratings.Length;
            int[] newRatings = new int[count];

            for (int i = 0; i < count; i++)
            {
                double delta = 0;
                for (int j = 0; j < count; j++)
                {
                    if (i == j) continue;

                    double expected = 1.0 / (1.0 + Math.Pow(10.0, (ratings[j] - ratings[i]) / 400.0));
                    double actual = Score(placements[i], placements[j]);
                    delta += kFactor * (actual - expected);
                }
                newRatings[i] = ratings[i] + (int)Math.Round(delta);
            }

            return newRatings;
        }

        private static double Score(int placeA, int placeB)
        {
            if (placeA == placeB) return 0.5;
            return placeA < placeB ? 1.0 : 0.0;
        }
    }
}
