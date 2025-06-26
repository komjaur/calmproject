using System.Collections.Generic;
using UnityEngine;

namespace SurvivalChaos
{
    public enum GameState { Pregame, Running, Victory, Loss }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public GameState State { get; private set; } = GameState.Pregame;
        public List<PlayerInfo> ActivePlayers { get; private set; } = new();

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

        /// <summary>
        /// Starts a new game with the provided players.
        /// </summary>
        public void StartGame(List<PlayerInfo> players)
        {
            ActivePlayers = players ?? new List<PlayerInfo>();
            State = GameState.Running;
            Debug.Log("Game Started");
            Debug.Log(ActivePlayers);
        }

   
    }
}
