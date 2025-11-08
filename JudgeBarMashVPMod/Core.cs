using System.Collections.Generic;
using MelonLoader;
using UnityEngine;
using UnityEngine.InputSystem;

[assembly: MelonInfo(typeof(JudgeBarMashVPMod.Core), "JudgeBarMashVPMod", "1.0.0", "MDuh", null)]
[assembly: MelonGame("AXTORM", "MASH VP! ReVISION")]

namespace JudgeBarMashVPMod
{
    public class Core : MelonMod
    {
        internal const int MaxLogEntries = 50;
        private static readonly List<string> _judgementLog = new List<string>(MaxLogEntries);
        private static readonly object _logLock = new object();
        private static GUIStyle _labelStyle;

        public static void PushJudgementLine(string line)
        {
            lock (_logLock)
            {
                _judgementLog.Add(line);
                if (_judgementLog.Count > MaxLogEntries)
                    _judgementLog.RemoveAt(0); // remove oldest
            }
        }

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
        }

        public override void OnGUI()
        {
            List<string> snapshot;
            lock (_logLock)
            {
                if (_judgementLog.Count == 0)
                    return;
                snapshot = new List<string>(_judgementLog);
            }

            if (_labelStyle == null)
            {
                _labelStyle = new GUIStyle()
                {
                    fontSize = 14,
                    normal = { textColor = Color.white }
                };
            }

            const float startX = 50f;
            const float startY = 100f;
            const float padding = 8f;
            const float lineHeight = 18f;
            const float width = 600f;

            int lines = snapshot.Count;
            float height = padding * 2f + (lines * lineHeight);

            var bgRect = new Rect(startX, startY, width, height);

            // Draw 50% black background for readability
            var prevColor = GUI.color;
            GUI.color = new Color(0f, 0f, 0f, 0.5f);
            GUI.Box(bgRect, GUIContent.none);
            GUI.color = prevColor;

            // Draw newest at the top, oldest at the bottom
            for (int i = 0; i < lines; i++)
            {
                int idx = snapshot.Count - 1 - i; // newest first
                float y = startY + padding + (i * lineHeight);
                var rect = new Rect(startX + padding, y, width - (padding * 2f), lineHeight);
                GUI.Label(rect, snapshot[idx], _labelStyle);
            }
        }

        public override void OnUpdate() // Runs once per frame.
        {
            if (Keyboard.current != null && Keyboard.current[Key.Backquote].wasPressedThisFrame)
            {
                lock (_logLock)
                {
                    _judgementLog.Clear();
                }
            }
        }
    }
}