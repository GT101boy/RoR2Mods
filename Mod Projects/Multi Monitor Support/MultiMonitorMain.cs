using System;
using System.Collections.Generic;
using UnityEngine;
using BepInEx;
using RoR2;
using On.RoR2;

namespace MultiplayerPause
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.grantimatter.multiplayerpause", "Multiplayer Pause", "0.1")]
    public class MultiMonitorMain : BaseUnityPlugin
    {
        // Public Variables

        // Private Variables
        private List<MonitorDisplay> displayList = new List<MonitorDisplay>();


        // Store enum that defines where the monitor is in space
        public enum MonitorPosition
        {
            left, right, center, top, bottom, topLeft, topRight, bottomLeft, bottomRight,
        }

        // Create struct to store information about each monitor
        struct MonitorDisplay
        {
            public Display display;
            public Camera assCamera;
            public Vector2 screenSize;
            public int displayIndex;
            public bool useDisplay;
            public MonitorPosition monitorPosition;
        }

        private Camera[] monitorCams;
        private int numDisplays;

        public void Awake()
        {
            SetupListeners();
        }

        private void SetupListeners()
        {
            On.RoR2.Run.BeginStage += Run_onStageStart;
        }

        private void Run_onStageStart(On.RoR2.Run.orig_BeginStage orig, RoR2.Run self)
        {
            orig(self);
            CreateCameras();
        }

        private void CreateCameras()
        {
            numDisplays = Display.displays.Length;

            if(numDisplays < 2)
                return;

            MonitorDisplay[] dispArr = new MonitorDisplay[numDisplays];
            int usingMonitors = 0;

            for (int i = 0; i < numDisplays; i++)
            {
                dispArr[i].displayIndex = i;
                dispArr[i].monitorPosition = MonitorPosition.center;
                dispArr[i].useDisplay = true;
                dispArr[i].display = Display.displays[dispArr[i].displayIndex];
                usingMonitors++;
            }

            Camera[] monCams = new Camera[usingMonitors];
            for (int i = 0; i < monCams.Length; i++)
            {
                if (monCams[i] != null)
                    Destroy(monCams[i].gameObject);

                monCams[i] = Instantiate(Camera.main);
                monCams[i].transform.SetParent(Camera.main.transform.parent);
                monCams[i].transform.position = Camera.main.transform.position;
                monCams[i].targetDisplay = dispArr[i].displayIndex;

                DontDestroyOnLoad(monCams[i]);
            }

            displayList.AddRange(dispArr);

            leftMonitorCam.transform.localEulerAngles = new Vector3(0f, -93.333f, 0f);
            rightMonitorCam.transform.localEulerAngles = new Vector3(0f, 93.333f, 0f);

            leftMonitorCam.targetDisplay = 1;
            rightMonitorCam.targetDisplay = 3;

            Display.displays[1].Activate();
            Display.displays[3].Activate();
        }
    }
}
