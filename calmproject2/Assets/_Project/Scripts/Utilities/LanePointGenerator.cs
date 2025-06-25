// LanePointGenerator.cs
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;   // Selection & Handles (editor-only)
#endif

namespace SurvivalChaos
{
    [ExecuteAlways]
    public sealed class LanePointGenerator : MonoBehaviour
    {
    #region Inspector
        [Min(0.1f)] public float Radius = 5f;
        public Color GizmoColor = Color.cyan;
        public bool HideChildrenInHierarchy = false;
    #endregion

    #region Public API
        public enum Team { Left, Top, Right, Bottom }
        public enum Lane { Top, Mid, Bottom }

        public Transform[] Points => _points;
        public IReadOnlyList<Transform> GetLanePath(Team t, Lane l) => _paths[(int)t, (int)l];
    #endregion

    #region Internal data
        private const float k_GizmoScale = 0.07f;

        private static readonly string[] PointNames =
        { "Top","TopLeft","Left","BottomLeft","Bottom","BottomRight","Right","TopRight","Center" };

        private static readonly Vector2[] k_Unit =
        { new(0,1),new(-1,1),new(-1,0),new(-1,-1),new(0,-1),new(1,-1),new(1,0),new(1,1),Vector2.zero };

        private static readonly int[][][] k_Lane =
        {
            new[]{ new[]{2,1,0,7,6}, new[]{2,8,6}, new[]{2,3,4,5,6} },           // LEFT
            new[]{ new[]{0,7,6,5,4}, new[]{0,8,4}, new[]{0,1,2,3,4} },           // TOP
            new[]{ new[]{6,5,4,3,2}, new[]{6,8,2}, new[]{6,7,0,1,2} },           // RIGHT
            new[]{ new[]{4,3,2,1,0}, new[]{4,8,0,7,6,8,2}, new[]{4,5,6,7,0} }    // BOTTOM
        };

        private static readonly int[] k_Spawn = { 2, 0, 6, 4 };   // Left, Top, Right, Bottom

#if UNITY_EDITOR
        private static readonly Color k_Red = Color.red;          // Mid-lane colour
#endif
    #endregion

    #region Private state
        [SerializeField] private Transform[] _points = new Transform[9];
        private readonly List<Transform>[,] _paths = new List<Transform>[4, 3];
        private float _prevRadius = -1f;
    #endregion

    #region Unity callbacks
#if UNITY_EDITOR
        private void OnValidate() => GenerateOrUpdate();
#endif
        [ContextMenu("Regenerate Lane Points")] private void Regenerate() => GenerateOrUpdate();

        private void Update()
        {
            if (!Application.isPlaying) return;
            if (!Mathf.Approximately(_prevRadius, Radius))
            { _prevRadius = Radius; GenerateOrUpdate(); }
        }

        private void OnDrawGizmos()
        {
            if (_points == null) return;

            /* point spheres */
            Gizmos.color = GizmoColor;
            float size = Radius * k_GizmoScale;
            foreach (var p in _points) if (p) Gizmos.DrawSphere(p.position, size);

#if UNITY_EDITOR
            /* show ONLY the Mid (red) lane of the selected team-spawn */
            Transform sel = Selection.activeTransform;
            if (sel == null) return;

            int team = -1;
            for (int t = 0; t < 4; ++t) if (sel == _points[k_Spawn[t]]) { team = t; break; }
            if (team < 0) return;

            var midPath = _paths[team, (int)Lane.Mid];
            if (midPath == null || midPath.Count < 2) return;

            Handles.color = k_Red;
            for (int i = 0; i < midPath.Count - 1; ++i)
                Handles.DrawAAPolyLine(4f, midPath[i].position, midPath[i + 1].position);
#endif
        }
    #endregion

    #region Core logic
        private void GenerateOrUpdate()
        {
            if (_points == null || _points.Length != 9) _points = new Transform[9];

            /* create / move points */
            for (int i = 0; i < 9; ++i)
            {
                if (_points[i] == null)
                {
                    var t = transform.Find(PointNames[i]) ??
                            new GameObject(PointNames[i]).transform;
                    t.SetParent(transform, false);
                    _points[i] = t;
                }
                Vector2 off = k_Unit[i] * Radius;
                _points[i].localPosition = new Vector3(off.x, off.y, 0f);

                // hide flags
                bool hidden = (_points[i].hideFlags & HideFlags.HideInHierarchy) != 0;
                if (HideChildrenInHierarchy && !hidden)
                    _points[i].hideFlags |= HideFlags.HideInHierarchy | HideFlags.DontSaveInBuild;
                else if (!HideChildrenInHierarchy && hidden)
                    _points[i].hideFlags = HideFlags.None;
            }

            /* rebuild cached paths */
            for (int t = 0; t < 4; ++t)
            for (int l = 0; l < 3; ++l)
            {
                var list = _paths[t, l] ?? (_paths[t, l] = new List<Transform>());
                list.Clear();
                foreach (int idx in k_Lane[t][l]) list.Add(_points[idx]);
            }
        }
    #endregion
    }
}
