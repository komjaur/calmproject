using System.Collections.Generic;
using UnityEngine;

namespace SurvivalChaos
{
    
    /// Turns MatchmakingTickets into ready 4-player Matches.
    public class MatchmakingManager : MonoBehaviour
    {
        public static MatchmakingManager Instance { get; private set; }
// Read-only access for debug overlays or UI
public IReadOnlyList<MatchmakingTicket> QueueReadOnly      => _queue;
public IReadOnlyCollection<Match>       ReadyMatchesReadOnly => _readyMatches;

        private const int PLAYERS_PER_MATCH = 4;
        private const int START_TOLERANCE = 50;  // ±Elo
        private const int TOLERANCE_STEP = 25;  // widens per scan
        private const float CHECK_INTERVAL = 1.0f;

        private readonly List<MatchmakingTicket> _queue = new();
        private readonly Queue<Match> _readyMatches = new();

        private int _tolerance = START_TOLERANCE;
        private float _nextScan = 0f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Update()
        {
            if (Time.time < _nextScan || _queue.Count < PLAYERS_PER_MATCH) return;
            _nextScan = Time.time + CHECK_INTERVAL;

            _queue.Sort((a, b) => a.Elo.CompareTo(b.Elo));

            for (int i = 0; i + PLAYERS_PER_MATCH - 1 < _queue.Count;)
            {
                int min = _queue[i].Elo;
                int max = _queue[i + PLAYERS_PER_MATCH - 1].Elo;

                if (max - min <= _tolerance)
                {
                    var players = new List<PlayerInfo>(PLAYERS_PER_MATCH);
                    for (int p = 0; p < PLAYERS_PER_MATCH; ++p)
                        players.Add(_queue[i + p].Player);

                    _queue.RemoveRange(i, PLAYERS_PER_MATCH);
                    _readyMatches.Enqueue(new Match(players));
                }
                else
                    ++i;
            }
            _tolerance += TOLERANCE_STEP;
        }

        // --------------------------- Public API -----------------------------
        public void Enqueue(MatchmakingTicket t) => _queue.Add(t);

        public bool TryDequeueReadyMatch(out Match match)
        {
            if (_readyMatches.Count > 0)
            {
                match = _readyMatches.Dequeue();
                return true;
            }
            match = null;
            return false;
        }

        /// <summary>Remove a player (by id/hash) from the waiting queue.</summary>
        public void RemoveFromQueue(int playerId)
        {
            _queue.RemoveAll(t => t.Player.id == playerId);
        }
    }
}
