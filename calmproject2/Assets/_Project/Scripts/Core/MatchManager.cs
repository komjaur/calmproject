using System.Collections.Generic;
using UnityEngine;

namespace SurvivalChaos
{
    /// <summary>
    /// Tracks matches that are in progress and finalises them when complete.
    /// </summary>
    public class MatchManager : MonoBehaviour
    {
        public static MatchManager Instance { get; private set; }

        private class ActiveMatch
        {
            public Match Match;
            public List<User> Profiles;
        }

        private readonly List<ActiveMatch> _active = new();
        public IReadOnlyList<Match> ActiveMatches
        {
            get
            {
                var list = new List<Match>(_active.Count);
                foreach (var am in _active) list.Add(am.Match);
                return list;
            }
        }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        /// <summary>Starts tracking a new match.</summary>
        public void StartMatch(Match match, List<User> profiles)
        {
            if (match == null || profiles == null) return;
            match.Start();
            _active.Add(new ActiveMatch { Match = match, Profiles = profiles });
        }

        /// <summary>
        /// Finishes an active match, updates user stats and logs it.
        /// </summary>
        public void FinishMatch(Match match, int[] placements, int[] goldEarned)
        {
            for (int i = 0; i < _active.Count; ++i)
            {
                if (_active[i].Match != match) continue;

                match.Finish();
                var am = _active[i];

                int count = am.Profiles.Count;
                int[] ratings = new int[count];
                for (int p = 0; p < count; ++p)
                    ratings[p] = am.Profiles[p].Elo;

                if (placements != null && placements.Length == count)
                {
                    int[] newRatings = EloUtility.UpdateAfterMatch(ratings, placements);
                    for (int p = 0; p < count; ++p)
                        am.Profiles[p].ApplyNewElo(newRatings[p]);
                }

                for (int p = 0; p < count; ++p)
                {
                    bool won = placements != null && placements[p] == 1;
                    int earned = goldEarned != null && goldEarned.Length > p ? goldEarned[p] : 0;
                    am.Profiles[p].RecordMatch(won, match.DurationSeconds, earned, match.Players[p].race.ToString());
                }

                GameDatabase.Instance.LogMatch(match);
                _active.RemoveAt(i);
                break;
            }
        }
    }
}
