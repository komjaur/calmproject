/*  MatchmakingDemo.cs
 *  Drop this script on an empty GameObject.
 *  Fill the “Players” list in the Inspector and press Play:
 *  every entry is turned into a MatchmakingTicket and queued.
 */

using System.Collections.Generic;
using UnityEngine;
using SurvivalChaos;   // <-- gives us PlayerInfo, MatchmakingTicket, Race, etc.

#region  Inspector-friendly player preset
// A tiny, serialisable struct that shows up in the Inspector.
[System.Serializable]
public class PlayerConfig
{
    public int   id    = 0;
    public Race  race  = Race.Human;   // replace with your own default
    public Color color = Color.white;
    public int   elo   = 1200;
}
#endregion

public class MatchmakingDemo : MonoBehaviour
{
    [Header("Players queued automatically on Play")]
    [SerializeField] private List<PlayerConfig> players = new();

    private void Start()
    {
        foreach (PlayerConfig cfg in players)
        {
            // Build the in-game PlayerInfo object.
            PlayerInfo player = new PlayerInfo(cfg.id, cfg.race, cfg.color);

            // Build a matchmaking ticket with the chosen Elo.
            MatchmakingTicket ticket = new MatchmakingTicket(player, cfg.elo);

            // Enqueue it.
            MatchmakingManager.Instance.Enqueue(ticket);

            Debug.Log($"Enqueued #{player.id} | Elo {cfg.elo} | Race {cfg.race}");
        }
    }

#if UNITY_EDITOR
    // Optional convenience: add four sample players when the component
    // is first added to a GameObject.
    private void Reset()
    {
        players = new List<PlayerConfig>
        {
            new PlayerConfig { id = 0, race = Race.Human,  color = new Color32(231,  76, 60, 255), elo = 1200 },
            new PlayerConfig { id = 1, race = Race.Orc,    color = new Color32( 41, 128,185, 255), elo = 1180 },
            new PlayerConfig { id = 2, race = Race.Orc,    color = new Color32( 46, 204,113, 255), elo = 1230 },
            new PlayerConfig { id = 3, race = Race.Undead, color = new Color32(241, 196, 15, 255), elo = 1215 }
        };
    }
#endif
}
