using System;
using System.Collections.Generic;
using BepInEx;
using EntityStates.BeetleQueenMonster;
using On.RoR2.UI;
using RoR2;
using TMPro;
using UnityEngine;
using ObjectivePanelController = IL.RoR2.UI.ObjectivePanelController;

namespace ExtraInfo
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.grantimatter.ExtraInfo", "Extra Info", "0.1")]
    public class ExtraInfo : BaseUnityPlugin
    {
        private const string PlayerStatsTextAreaName = "PlayerStatsTextArea";
        private const string StageStatsTextAreaName = "StageStatsTextArea";
        
        //public static AssetBundle ExtraInfoAssetBundle { get; private set; }
        private static int chestsOpened;
        private static int terminalsUsed;
        private static int barrelsOpened;
        private static int chanceShrinesExpired;
        private static int bloodShrinesExpired;

        //private static int _currentStage;
        public void Awake()
        {
            On.RoR2.BarrelInteraction.OnInteractionBegin += Barrel_Opened;
            On.RoR2.ShopTerminalBehavior.DropPickup += Terminal_DropPickup;
            On.RoR2.PlayerCharacterMasterController.OnBodyStart += Body_Start;
            On.RoR2.CharacterBody.Start += CharacterBody_Started;
            On.RoR2.Run.BeginStage += Run_BeginStage;
            On.RoR2.ChestBehavior.Open += Chest_Opened;
            On.RoR2.UI.MainMenu.MainMenuController.Start += Main_MenuStart;
            On.RoR2.UI.SettingsPanelController.Start += SettingsPanel_Start;
            
            //ExtraInfoAssetBundle = AssetBundle.LoadFromFile("");
        }

        public void Chest_Opened(On.RoR2.ChestBehavior.orig_Open o, RoR2.ChestBehavior s)
        {
            o(s);
            chestsOpened++;
            UpdateTextArea(StageStatsTextAreaName);
        }

        private void Barrel_Opened(On.RoR2.BarrelInteraction.orig_OnInteractionBegin orig, BarrelInteraction self, Interactor activator)
        {
            orig(self, activator);
            barrelsOpened++;
            UpdateTextArea(StageStatsTextAreaName);
        }

        private void Terminal_DropPickup(On.RoR2.ShopTerminalBehavior.orig_DropPickup o, ShopTerminalBehavior s)
        {
            o(s);
            terminalsUsed++;
            //if(s.)
            UpdateTextArea(StageStatsTextAreaName);
        }

        public void Body_Start(On.RoR2.PlayerCharacterMasterController.orig_OnBodyStart o, RoR2.PlayerCharacterMasterController s)
        {
            o(s);
            //GetPlayerStats(s);
        }

        public void CharacterBody_Started(On.RoR2.CharacterBody.orig_Start o, RoR2.CharacterBody s)
        {
            o(s);
            //GetPlayerStats(s);
        }
        
        // Gets called when a new stage is loaded
        private void Run_BeginStage(On.RoR2.Run.orig_BeginStage o, Run s)
        {
            o(s);
            ResetInteractions();
            UpdateTextArea(StageStatsTextAreaName);
        }

        private static void ResetInteractions()
        {
            chestsOpened = 0;
            terminalsUsed = 0;
            barrelsOpened = 0;
            chanceShrinesExpired = 0;
            bloodShrinesExpired = 0;
        }

        // Gets called at the main menu;
        public void Main_MenuStart(On.RoR2.UI.MainMenu.MainMenuController.orig_Start o, RoR2.UI.MainMenu.MainMenuController m)
        {
            o(m);
            TmpStuff.HideTextArea(StageStatsTextAreaName);
        }

        public void SettingsPanel_Start(On.RoR2.UI.SettingsPanelController.orig_Start o, RoR2.UI.SettingsPanelController s)
        {
            o(s);
            TmpStuff.HideTextArea(StageStatsTextAreaName);
        }
        
        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.F9))
            {
                Run.instance.AdvanceStage(Run.instance.nextStageScene);
            }
            if(Input.GetKeyDown(KeyCode.F1))
            {
                TmpStuff.UpdateText(StageStatsTextAreaName, GetStageStats());
                TmpStuff.UpdateText(PlayerStatsTextAreaName, GetPlayerStats());
            }
        }

        public static string[] GetStageStats()
        {
            var header = "              Loot\n";

            var statCounts = new List<int>();
            var statStrings = new List<string>();

            if(statCounts != null)
                statCounts.Clear();
            if(statStrings != null)
                statStrings.Clear();

            // Get loot counts

            var chests = FindObjectsOfType<ChestBehavior>();

            statCounts.Add(FindObjectsOfType<ChestBehavior>().Length - chestsOpened);
            statCounts.Add(FindObjectsOfType<ShopTerminalBehavior>().Length - terminalsUsed);
            statCounts.Add(FindObjectsOfType<BarrelInteraction>().Length - barrelsOpened);

            statStrings.Add(" Chests\n");
            statStrings.Add(" Terminals\n");
            statStrings.Add(" Barrels\n");

            // Get shrine counts
            statCounts.Add(FindObjectsOfType<ShrineChanceBehavior>().Length - chanceShrinesExpired);
            statCounts.Add(FindObjectsOfType<ShrineBloodBehavior>().Length - bloodShrinesExpired);
            statCounts.Add(FindObjectsOfType<ShrineCombatBehavior>().Length);
            statCounts.Add(FindObjectsOfType<ShrineRestackBehavior>().Length);
            statCounts.Add(FindObjectsOfType<ShrineBossBehavior>().Length);

            statStrings.Add(" Chance Shrines\n");
            statStrings.Add(" Blood Shrines\n");
            statStrings.Add(" Combat Shrines\n");
            statStrings.Add(" Reorder Shrines\n");
            statStrings.Add(" Mountain Shrines\n");

            var finalStrings = new List<string>();

            for (int i = 0; i < statCounts.Count; i++)
            {
                if (statCounts[i] > 0)
                {
                    if (statCounts[i] < 2)
                    {
                        statStrings[i] = statStrings[i].Substring(0, statStrings[i].Length - 2) + "\n";
                    }

                    finalStrings.Add(statCounts[i] + statStrings[i]);
                }
            }

            var finalStats = finalStrings.ToArray();

            return finalStats;
        }

        
        public string[] GetPlayerStats()
        {
            //var playerSpeed = cb.moveSpeed;
            //var fireRate = cb.damage;
            //var 
            var charBod = new CharacterBody[PlayerCharacterMasterController.instances.Count];
            for (int i = 0; i < charBod.Length; i++)
            {
                charBod[i] = PlayerCharacterMasterController.instances[i].master.GetBody();
            }
                //cb.
                var playerStats = new string[] { charBod[0].GetUserName() + "\n", "Move Speed: " + charBod[0].moveSpeed + "\n", "Attack Speed: " + charBod[0].attackSpeed + "\n", "Crit Chance: " + charBod[0].crit + "%\n"};
                //var playStats = string.
                return playerStats;
                //return charBod[0].GetUserName();
        }
        

        private void UpdateTextArea(string textAreaName)
        {
            if(!TmpStuff.textAreas.ContainsKey(StageStatsTextAreaName))
            {
                CreateMainInfoArea();
            }
            else
            {
                TmpStuff.UpdateText(StageStatsTextAreaName, GetStageStats());
            }

            if (!TmpStuff.textAreas.ContainsKey(PlayerStatsTextAreaName))
                CreatePlayerStatsArea();
            else
                TmpStuff.UpdateText(PlayerStatsTextAreaName, GetPlayerStats());
        }

        private void AddObjective(string str)
        {
            //IL.RoR2.UI.ObjectivePanelController.ObjectiveTracker.GenerateString;
        }

        private void CreateMainInfoArea()
        {
            TmpStuff.CreateTextArea(new Rect(0.81f, 0.65f, 1000f, 6000f), 24f, Color.white, TmpStuff.MainCanvasName, StageStatsTextAreaName, true, GetStageStats());
        }

        private void CreatePlayerStatsArea()
        {
            TmpStuff.CreateTextArea(new Rect(0.05f, 0.75f, 1000f, 6000f), 24f, Color.green, TmpStuff.MainCanvasName, PlayerStatsTextAreaName, true, GetPlayerStats());

        }
    }
}