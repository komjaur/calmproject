/*  LobbyDemo.cs
 *  Drag this onto an empty GameObject.
 *  Users configured in the Inspector will auto-connect to the lobby on Play,
 *  press ”Play” (i.e. request a match), and get forwarded to the matchmaker.
 */
using System.Collections.Generic;
using UnityEngine;
using SurvivalChaos;              // User, PlayerInfo, MatchmakingTicket, Race…

#region  Inspector-friendly user preset
[System.Serializable]            // visible in the Inspector
public class LobbyUserPreset
{
    public User  profile;        // persistent data object
    public Race  preferredRace = Race.Human;
    public Color color         = Color.white;
    public int   elo           = 1200;
    public bool  joinOnStart   = true;      // auto-join when scene starts?
}
#endregion

public class LobbyDemo : MonoBehaviour
{
    [Header("Users that will enter the lobby when the scene starts")]
    [SerializeField] private List<LobbyUserPreset> presets = new();

    // active users in the lobby, keyed by User.Id (or GetHashCode as a fallback)
    private readonly Dictionary<int, LobbyUser> active = new();

    #region Life-cycle --------------------------------------------------------

    private void Start()
    {
        foreach (LobbyUserPreset p in presets)
        {
            if (p.joinOnStart && p.profile != null)
            {
                AddUserToLobby(p.profile, p.elo, p.preferredRace, p.color);
                RequestPlay(p.profile);              // auto-press “Play”
            }
        }
    }

    private void Update()
    {
        // Poll the matchmaker every frame and print when a match pops out
        while (MatchmakingManager.Instance.TryDequeueReadyMatch(out Match match))
        {
            Debug.Log($"[Lobby] Match ready with {match.Players.Count} players");
            // TODO: Load gameplay scene and hand over match.Players
        }
    }
    #endregion

    #region Public API --------------------------------------------------------

    public void AddUserToLobby(User profile,
                               int   elo,
                               Race  race  = Race.Human,
                               Color color = default)
    {
        int key = profile.GetHashCode();              // or a stable User.Id
        if (active.ContainsKey(key)) return;

        active[key] = new LobbyUser(profile, elo, race, color);
        Debug.Log($"[Lobby] {profile.Name} joined (Elo {elo})");
    }

    public void RemoveUserFromLobby(User profile)
    {
        int key = profile.GetHashCode();
        if (!active.Remove(key)) return;

        Debug.Log($"[Lobby] {profile.Name} left the lobby");
    }

    /// Called when the UI “Play” button is pressed.
    public void RequestPlay(User profile)
    {
        int key = profile.GetHashCode();
        if (!active.TryGetValue(key, out LobbyUser u)) return;

        // Convert to the runtime‐only PlayerInfo used inside the game
        PlayerInfo player = new PlayerInfo(profile.GetHashCode(),
                                           u.SelectedRace,
                                           u.Color);

        // Bind the long-lived User ↔ runtime Player via the same id/hash
        MatchmakingTicket ticket = new MatchmakingTicket(player, u.Elo);
        MatchmakingManager.Instance.Enqueue(ticket);

        Debug.Log($"[Lobby] {profile.Name} queued for a game (Elo {u.Elo})");
    }
    #endregion
}

/// <summary>A lightweight wrapper that lives only while the user sits in the lobby.</summary>
public sealed class LobbyUser
{
    public User  Profile       { get; }
    public int   Elo           { get; }
    public Race  SelectedRace  { get; set; }
    public Color Color         { get; set; }

    public LobbyUser(User profile, int elo, Race race, Color color)
    {
        Profile      = profile;
        Elo          = elo;
        SelectedRace = race;
        Color        = color;
    }
}
