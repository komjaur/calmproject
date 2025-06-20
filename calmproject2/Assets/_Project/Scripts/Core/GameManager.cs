using System.Collections.Generic;
using UnityEngine;

namespace SurvivalChaos
{
    public enum GameState { Pregame, Running, Victory, Loss }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public GameState State { get; private set; } = GameState.Pregame;
        public List<PlayerInfo> ActivePlayers { get; private set; } = new List<PlayerInfo>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void StartGame(List<PlayerInfo> players)
        {
            ActivePlayers = players;
            State = GameState.Running;
        }

        public void EliminatePlayer(PlayerInfo player)
        {
            if (!ActivePlayers.Contains(player)) return;
            ActivePlayers.Remove(player);
            if (ActivePlayers.Count <= 1)
            {
                State = GameState.Victory;
                var winner = ActivePlayers.Count == 1 ? ActivePlayers[0] : null;
                EventBus.Raise(new GameEndedEvent(winner));
            }
        }
    }
}
