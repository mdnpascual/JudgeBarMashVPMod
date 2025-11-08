using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Il2Cpp;
using Il2CppAxtorm.Atom.MusicPlay;
using Il2CppAxtorm.GameEngine.MusicGame.Events;
using Il2CppAxtorm.GameEngine.MusicGame.Judgement;
using MelonLoader;

namespace JudgeBarMashVPMod.Patches
{
    public static class GamePatches
    {
        [HarmonyPatch(typeof(FastSlowViewer), nameof(FastSlowViewer.OnJudgement))]
        public static class Patch_OnJudgement
        {
            private static void Postfix(JudgementEvent e)
            {
                string line = $"{e.DeltaMs:F2} ms - {e.Judgement}";
                JudgeBarMashVPMod.Core.PushJudgementLine(line);
                Melon<Core>.Logger.Warning(line);
            }
        }
    }
}

