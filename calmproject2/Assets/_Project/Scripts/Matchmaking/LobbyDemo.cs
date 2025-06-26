/*  LobbyDemo.cs
 *  Drop this on an empty GameObject.
 *  Tick / untick “want To Play” in the Inspector while the game is running
 *  to add / remove users from the matchmaking queue.
 */
using System.Collections.Generic;
using UnityEngine;
using SurvivalChaos;   // User, PlayerInfo, MatchmakingTicket, Race …

#region  Inspector-friendly preset -------------------------------------------
[System.Serializable]
public class LobbyUserPreset
{
    public User  profile;                      // persistent data (stats etc.)
    public Race  preferredRace = Race.Human;   // race pre-selected in lobby
    public Color color         = Color.white;  // UI / minimap colour

    [Tooltip("If checked the user is queued for a match; "
           + "untick to pull them out again.")]
    public bool  wantToPlay    = true;
}
#endregion

public class LobbyDemo : MonoBehaviour
{
    // ───────────────────────────────────────────── singleton for debug overlay
    private static LobbyDemo _instance;
    public  static IReadOnlyDictionary<int, LobbyUser> ActiveReadOnly
        => _instance?.active;

    // ───────────────────────────────────────────── inspector list of presets
    [Header("Users visible in the lobby")]
    [SerializeField] private List<LobbyUserPreset> presets = new();

    // runtime state – one entry per user profile
    private readonly Dictionary<int, LobbyUser> active = new();

    // ──────────────────────────────────────────────── life-cycle
    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        foreach (LobbyUserPreset p in presets)
        {
            if (p.profile == null) continue;

            AddUserToLobby(p);                       // always in the lobby
            if (p.wantToPlay) SetPlayState(p.profile, true);  // maybe queued
        }
    }

    private void Update()
    {
        // 1) Watch the Inspector toggles and reflect them in the queue
        foreach (LobbyUserPreset p in presets)
        {
            if (p.profile == null) continue;

            int key = p.profile.GetHashCode();
            if (!active.TryGetValue(key, out LobbyUser lu)) continue;

            if (p.wantToPlay && !lu.IsQueued)
                SetPlayState(p.profile, true);
            else if (!p.wantToPlay && lu.IsQueued)
                SetPlayState(p.profile, false);
        }

        while (MatchmakingManager.Instance.TryDequeueReadyMatch(out Match match))
        {
            Debug.Log($"[Lobby] Match ready with {match.Players.Count} players");

            // Build profile list for MatchManager
            var profiles = new List<User>(match.Players.Count);
            foreach (var pInfo in match.Players)
            {
                if (active.TryGetValue(pInfo.id, out LobbyUser lu))
                    profiles.Add(lu.Profile);
            }

            // Register with MatchManager and overlay
            var manager = FindObjectOfType<MatchManager>();
            if (manager) manager.StartMatch(match, profiles);
            var overlay = FindObjectOfType<MatchmakingDebugOverlay>();
            if (overlay) overlay.MarkMatchAsPlaying(match);

            // TODO: load gameplay scene & hand over match.Players
        }
    }

    // ───────────────────────────────────────────── internal helpers
    private void AddUserToLobby(LobbyUserPreset preset)
    {
        int key = preset.profile.GetHashCode();
        if (active.ContainsKey(key)) return;

        active[key] = new LobbyUser(preset);
        Debug.Log($"[Lobby] {preset.profile.Name} joined (Elo {preset.profile.Elo})");
    }

    /// Toggle whether the given user is in the matchmaking queue.
    private void SetPlayState(User profile, bool wantsToPlay)
    {
        int key = profile.GetHashCode();
        if (!active.TryGetValue(key, out LobbyUser lu)) return;

        if (wantsToPlay)
        {
            // build PlayerInfo → ticket → enqueue
            PlayerInfo player = new PlayerInfo(key, lu.SelectedRace, lu.Color);
            var ticket        = new MatchmakingTicket(player, lu.Profile.Elo);
            MatchmakingManager.Instance.Enqueue(ticket);
            lu.IsQueued = true;
            Debug.Log($"[Lobby] {profile.Name} queued (Elo {lu.Profile.Elo})");
        }
        else
        {
            MatchmakingManager.Instance.RemoveFromQueue(key);
            lu.IsQueued = false;
            Debug.Log($"[Lobby] {profile.Name} un-queued");
        }
    }
}

/// <summary>Light-weight runtime wrapper for a user sitting in the lobby.</summary>
public sealed class LobbyUser
{
    public User  Profile      { get; }

    public Race  SelectedRace { get; set; }
    public Color Color        { get; set; }
    public bool  IsQueued     { get; set; }

    public LobbyUser(LobbyUserPreset p)
    {
        Profile      = p.profile;
        SelectedRace = p.preferredRace;
        Color        = p.color;
    }
}

