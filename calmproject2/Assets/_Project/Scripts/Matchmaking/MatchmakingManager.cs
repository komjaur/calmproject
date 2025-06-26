using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalChaos
{
    /// <summary>
    /// Central hub that turns queued MatchmakingTickets into 4-player Matches.
    /// Attach it to a GameObject (or make it DontDestroyOnLoad).
    /// </summary>
    public class MatchmakingManager : MonoBehaviour
    {
        // Singleton shortcut --------------------------------------------------
        public static MatchmakingManager Instance { get; private set; }

        [Tooltip("How player races are chosen when a match forms.")]
        public MatchType matchType = MatchType.Normal;

        // Tunables ------------------------------------------------------------
        private const int   PLAYERS_PER_MATCH   = 4;

        private const int   START_ELO_TOLERANCE = 50;   // initial ±Elo window
        private const int   TOLERANCE_STEP      = 25;   // widens after each scan
        private const float CHECK_INTERVAL      = 1.0f; // seconds between scans

        // Internal state ------------------------------------------------------
        private int  _currentTolerance  = START_ELO_TOLERANCE;
        private float _nextScanTime     = 0f;

        private readonly List<MatchmakingTicket> _queue       = new(); // waiting players
        private readonly Queue<Match>            _readyMatches = new(); // finished groups

        #region Unity lifecycle
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Update()
        {
            if (Time.time < _nextScanTime || _queue.Count < PLAYERS_PER_MATCH)
                return;

            _nextScanTime = Time.time + CHECK_INTERVAL;

            // 1) Sort once so Elo values are ascending – grouping gets cheaper
            _queue.Sort((a, b) => a.Elo.CompareTo(b.Elo));

            // 2) Slide a window and pull out any tight 4-player clusters
            for (int i = 0; i + PLAYERS_PER_MATCH - 1 < _queue.Count; )
            {
                int minElo = _queue[i].Elo;
                int maxElo = _queue[i + PLAYERS_PER_MATCH - 1].Elo;

                if (maxElo - minElo <= _currentTolerance)
                {
                    // Build a Match
                    List<PlayerInfo> players = new(PLAYERS_PER_MATCH);
                    Array races = Enum.GetValues(typeof(Race));
                    for (int p = 0; p < PLAYERS_PER_MATCH; ++p)
                    {
                        PlayerInfo original = _queue[i + p].Player;
                        Race race = original.race;
                        if (matchType == MatchType.Chaos)
                        {
                            int idx = UnityEngine.Random.Range(0, races.Length);
                            race = (Race)races.GetValue(idx);
                        }
                        players.Add(new PlayerInfo(original.id, race, original.color));
                    }

                    // Remove tickets from queue
                    _queue.RemoveRange(i, PLAYERS_PER_MATCH);

                    // Enqueue ready match
                    _readyMatches.Enqueue(new Match(players));

                    // stay at same index i – list shrank, new ticket now sits here
                }
                else
                {
                    ++i; // window too wide – shift right
                }
            }

            // 3) Make matching progressively easier the longer players wait
            _currentTolerance += TOLERANCE_STEP;
        }
        #endregion

        #region Public API -----------------------------------------------------
        /// <summary>Adds a player ticket to the queue.</summary>
        public void Enqueue(MatchmakingTicket ticket)
        {
            _queue.Add(ticket);
        }

        /// <summary>Called by the lobby/menu: is a 4-player match ready?</summary>
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
        #endregion
    }
}
