using UnityEngine;

namespace SurvivalChaos
{
    [System.Serializable]
    public class PlayerInfo
    {
        public int id;
        public Race race;
        public Color color = Color.white;
        public int gold;

        public PlayerInfo(int id, Race race) : this(id, race, Color.white) { }

        public PlayerInfo(int id, Race race, Color color)
        {
            this.id = id;
            this.race = race;
            this.color = color;
            gold = 0;
        }
    }
}
