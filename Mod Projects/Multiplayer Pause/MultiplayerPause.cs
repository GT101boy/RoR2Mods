using System;
using BepInEx;
using RoR2;
using UnityEngine;

namespace MultiplayerPause
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.grantimatter.multiplayerpause", "Multiplayer Pause", "0.1")]
    public class MultiplayerPause : BaseUnityPlugin
    {
        public void Awake()
        {
            R2API.SurvivorAPI.SurvivorCatalogReady += (s, e) =>
            {
                var survivor = new SurvivorDef
                {
                    bodyPrefab = BodyCatalog.FindBodyPrefab("BanditBody"),
                    descriptionToken = "BANDIT_DESCRIPTION",
                    displayPrefab = Resources.Load<GameObject>("Prefabs/Characters/BanditDisplay"),
                    primaryColor = new Color(0.8039216f, 0.482352942f, 0.843137264f),
                    unlockableName = ""
                };

                var newsurvivor = new SurvivorDef
                {
                    bodyPrefab = BodyCatalog.FindBodyPrefab("BanditBody"),
                    descriptionToken = "NEW_SURVIVOR_DESCRIPTION",
                    displayPrefab = Resources.Load<GameObject>("Prefabs/Characters/BanditDisplay"),
                    primaryColor = new Color(0.1f, 0.4f, 0.8f),
                    unlockableName = "",
                    
                };

                R2API.SurvivorAPI.SurvivorDefinitions.Insert(3, survivor);
                R2API.SurvivorAPI.SurvivorDefinitions.Insert(7, newsurvivor);
            };
        }

        private void Run_onStageStart(On.RoR2.Run.orig_BeginStage orig, RoR2.Run self)
        {
            orig(self);
            CreateCameras();
        }

        public void OnDisable()
        {
            On.RoR2.Run.BeginStage -= Run_onStageStart;
        }

        bool isPaused = false;
        public void Update()
        {
            // Only run if host
            if(Run.instance.localPlayerAuthority)
            {
                if(Input.GetKeyDown(KeyCode.M))
                {
                    if (isPaused)
                    {

                    }

                    isPaused = !isPaused;
                }
            }

            if(Input.GetKeyDown(KeyCode.T))
            {
                //Run.instance.AdvanceStage(Run.instance.nextStageScene.SceneName);
                TMPStuff.CreateTextMeshProUGUI();
            }

            if(Input.GetKeyDown(KeyCode.F5))
            {
                CreateCameras();
            }

            if(Input.GetKeyDown(KeyCode.F1))
            {
                SpawnItems.SpawnItem(1);
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                SpawnItems.SpawnItem(2);
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                SpawnItems.SpawnItem(3);
            }
            if(Input.GetKeyDown(KeyCode.F4))
            {
                SpawnItems.SpawnItem(4);
            }
            if(Input.GetKeyDown(KeyCode.F5))
            {
                SpawnItems.SpawnItem(5);
            }
        }
    }
}