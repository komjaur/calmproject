/*
 *  MatchmakingDebugOverlay.cs
 *  Put this on any always-alive object (e.g. your “_Managers” GameObject).
 *  At runtime it draws counts + player lists in the top-left corner.
 *
 *  You can style it or move it anywhere; it’s pure IMGUI.
 */
using System.Collections.Generic;
using UnityEngine;
using SurvivalChaos;

public class MatchmakingDebugOverlay : MonoBehaviour
{
    // matches currently counting down to game start
    private readonly Dictionary<Match, int> _starting = new();
    // matches that have left the queue and entered actual gameplay
    private readonly List<Match> _playing = new();

    // ------------------------------------------------------------------- API
    /// <summary>Sets the remaining countdown time for a soon-to-start match.</summary>
    public void SetCountdown(Match match, int seconds)
    {
        if (match != null)
            _starting[match] = Mathf.Max(0, seconds);
    }

    /// <summary>Moves a match from the starting list to actively playing.</summary>
    public void MarkMatchAsPlaying(Match match)
    {
        if (match != null)
        {
            _starting.Remove(match);
            _playing.Add(match);
        }
    }

    // ------------------------------------------------------------- IMGUI draw
    private const float PAD   = 6f;
    private const float LINEH = 18f;

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(PAD, PAD, 480, Screen.height),
                            GUI.skin.box);

        DrawSection("LOBBY USERS",
                    LobbyDemo.ActiveReadOnly.Values,
                    u => $"{u.Profile.Name} ({u.Profile.Elo})");

        DrawSection("QUEUE",
                    MatchmakingManager.Instance.QueueReadOnly,
                    t => $"{t.Player.id}  Elo {t.Elo}");

        DrawSection("MATCHES – CREATING",
                    MatchmakingManager.Instance.ReadyMatchesReadOnly,
                    m => ListNames(m.Players));

        DrawSection("MATCHES – STARTING",
                    _starting,
                    kv => $"{ListNames(kv.Key.Players)} (in {kv.Value})");

        DrawSection("MATCHES – PLAYING",
                    _playing,
                    m => ListNames(m.Players));

        GUILayout.EndArea();
    }

    // ---------------------------------------------------------- helpers -----
    private static void DrawSection<T>(string caption,
                                       IEnumerable<T> rows,
                                       System.Func<T, string> toLine)
    {
        GUILayout.Label($"<b>{caption}</b>", RichLabel);
        int count = 0;
        foreach (T item in rows)
        {
            ++count;
            GUILayout.Label(toLine(item), Label);
        }
        if (count == 0) GUILayout.Label("– none –", ItalicLabel);
        GUILayout.Space(PAD); // extra gap after every section
    }

    private static string ListNames(IReadOnlyList<PlayerInfo> ps)
    {
        var parts = new List<string>(ps.Count);
        foreach (var p in ps) parts.Add(p.id.ToString());
        return string.Join(", ", parts);
    }

    // quick cached styles ----------------------------------------------------
    private static GUIStyle _label, _rich, _italic;
    private static GUIStyle Label      => _label  ??= new GUIStyle(GUI.skin.label){fontSize=12};
    private static GUIStyle RichLabel  => _rich   ??= new GUIStyle(Label){richText=true};
    private static GUIStyle ItalicLabel=> _italic ??= new GUIStyle(Label){fontStyle=FontStyle.Italic};
}
