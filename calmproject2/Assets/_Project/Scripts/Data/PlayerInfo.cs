using UnityEngine;

namespace SurvivalChaos
{
    [System.Serializable]
    public class PlayerInfo
    {
        public int id;
        public Race race;
        public int gold;

        public PlayerInfo(int id, Race race)
        {
            this.id = id;
            this.race = race;
            gold = 0;
        }
    }
}
