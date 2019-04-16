using System;
using BepInEx;
using RoR2;
using UnityEngine;

namespace ExtraInfo
{
    [BepInDependency("com.bepis.r2api")]
    public class ExtraInfo : BaseUnityPlugin
    {
        private static int currentStage = 0;
        public static int CurrentStage { get { return currentStage; } }
        public void Awake()
        {
            On.RoR2.Run.BeginStage += Run_BeginStage;
        }

        private void Run_BeginStage(On.RoR2.Run.orig_BeginStage orig, Run self)
        {
            orig(self);
            TMPStuff.CreateTextMeshProUGUI();
        }
    }
}