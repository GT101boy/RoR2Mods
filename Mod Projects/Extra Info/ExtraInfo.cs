using BepInEx;
using RoR2;
using UnityEngine;

namespace ExtraInfo
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.grantimatter.ExtraInfo", "Extra Info", "0.1")]
    public class ExtraInfo : BaseUnityPlugin
    {
        private static int currentStage = 0;
        public void Awake()
        {
            On.RoR2.Run.BeginStage += Run_BeginStage;
        }

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.F9))
            {
                Run.instance.AdvanceStage(Run.instance.nextStageScene);
            }
            if(Input.GetKeyDown(KeyCode.F1))
            {

            }
        }

        private void Run_BeginStage(On.RoR2.Run.orig_BeginStage o, Run s)
        {
            currentStage = Run.instance.stageClearCount;
            TMPStuff.CreateTextMeshProUGUI();
            TMPStuff.UpdateText(currentStage);
            o(s);
        }
    }
}