using System;
using BepInEx;
using RoR2;
using UnityEngine;

namespace ExtraInfo
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.grantimatter.ExtraInfo", "Extra Info", "0.1")]
    public class ExtraInfo : BaseUnityPlugin
    {
        public static AssetBundle ExtraInfoAssetBundle { get; private set; }

        private static int _currentStage;
        public void Awake()
        {
            On.RoR2.Run.BeginStage += Run_BeginStage;
            On.RoR2.UI.MainMenu.MainMenuController.Start += Main_MenuStart;
            On.RoR2.UI.SettingsPanelController.Start += SettingsPanel_Start;
            
            ExtraInfoAssetBundle = AssetBundle.LoadFromFile("");
        }

        public void Main_MenuStart(On.RoR2.UI.MainMenu.MainMenuController.orig_Start o, RoR2.UI.MainMenu.MainMenuController m)
        {
            o(m);
            DisableTmpText();
        }

        public void SettingsPanel_Start(On.RoR2.UI.SettingsPanelController.orig_Start o, RoR2.UI.SettingsPanelController s)
        {
            o(s);
            DisableTmpText();
        }

        public static void DisableTmpText()
        {
            if (TmpStuff.StageText != null)
            {
                TmpStuff.StageText.canvasRenderer.SetAlpha(0f);
            }
        }

        private static void EnableTmpText()
        {
            if (TmpStuff.StageText != null)
            {
                TmpStuff.StageText.canvasRenderer.SetAlpha(1f);
            }
        }

        private float time;
        private Canvas[] cans;
        public void Update()
        {
            if (Time.time - time > 1f)
            {
                Canvas[] newCans = FindObjectsOfType<Canvas>();
                if (cans != newCans)
                {
                    cans = newCans;
                    TmpStuff.UpdateText();
                }
                
                time = Time.time;
            }
            
            if(Input.GetKeyDown(KeyCode.F9))
            {
                //Run.instance.AdvanceStage(Run.instance.nextStageScene);
            }
            if(Input.GetKeyDown(KeyCode.F1))
            {
    
            }
        }

        private static void Run_BeginStage(On.RoR2.Run.orig_BeginStage o, Run s)
        {
            _currentStage = Run.instance.stageClearCount;
            if (TmpStuff.StageText == null)
            {
                TmpStuff.CreateTextMeshProObject();
            
            }
            else
            {
                EnableTmpText();
            }
            
            TmpStuff.UpdateText(_currentStage);
            o(s);
        }
    }
}