using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SurvivalChaos
{
    /// <summary>
    /// Very simple Elo-based matchmaking for 4-player matches.
    /// </summary>
    public class MatchmakingManager : MonoBehaviour
    {
        public static MatchmakingManager Instance { get; private set; }

        private readonly List<MatchmakingTicket> queue = new();

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

        private void OnEnable()
        {
            StartCoroutine(ProcessLoop());
        }

        /// <summary>
        /// Adds a ticket to the matchmaking queue.
        /// </summary>
        public void Enqueue(MatchmakingTicket ticket)
        {
            if (ticket != null)
                queue.Add(ticket);
        }

        private IEnumerator ProcessLoop()
        {
            var wait = new WaitForSeconds(1f);
            while (true)
            {
                yield return wait;
                ProcessQueue();
            }
        }

        private void ProcessQueue()
        {
            if (queue.Count < 4)
                return;

            queue.Sort((a, b) => a.Elo.CompareTo(b.Elo));

            for (int i = 0; i <= queue.Count - 4;)
            {
                var block = queue.GetRange(i, 4);
                int maxElo = block.Max(t => t.Elo);
                int minElo = block.Min(t => t.Elo);
                int diff = maxElo - minElo;

                bool allReady = block.All(t => t.CurrentTolerance >= diff);
                if (allReady)
                {
                    queue.RemoveRange(i, 4);
                    StartMatch(block);
                }
                else
                {
                    i++;
                }
            }
        }

        private void StartMatch(List<MatchmakingTicket> tickets)
        {
            List<PlayerInfo> players = tickets.Select(t => t.Player).ToList();
            GameManager.Instance.StartGame(players);
        }
    }
}
