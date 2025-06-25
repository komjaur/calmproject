using UnityEngine;

namespace SurvivalChaos
{
    public class RaceSelectPanel : MonoBehaviour
    {
        /// <summary>
        /// Assigns the selected race and colour to the provided player.
        /// Intended to be hooked up to UI dropdowns or colour pickers.
        /// </summary>
        public void ApplySelection(PlayerInfo player, Race race, Color color)
        {
            if (player == null) return;

            player.race = race;
            player.color = color;
        }
    }
}
