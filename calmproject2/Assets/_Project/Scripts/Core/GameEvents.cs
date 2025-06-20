namespace SurvivalChaos
{
    public struct WaveSpawnEvent
    {
        public Lane lane;
        public WaveSpawnEvent(Lane lane)
        {
            this.lane = lane;
        }
    }

    public struct IncomeTickEvent { }

    public struct GlobalWeaponFiredEvent
    {
        public PlayerInfo player;
        public GlobalWeaponFiredEvent(PlayerInfo p)
        {
            player = p;
        }
    }

    public struct GameEndedEvent
    {
        public PlayerInfo winner;
        public GameEndedEvent(PlayerInfo w)
        {
            winner = w;
        }
    }

    public struct BuildingDestroyedEvent
    {
        public BuildingController building;
        public BuildingDestroyedEvent(BuildingController b)
        {
            building = b;
        }
    }
}
