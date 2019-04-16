using System;
using System.Collections.Generic;
using UnityEngine;
using BepInEx;
using RoR2;
using On.RoR2;

namespace MultiplayerPause
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.grantimatter.multiMonitor", "Multiple Monitor Support", "0.1")]
    public class MultiMonitorMain : BaseUnityPlugin
    {
        // Public Variables
        public int maxDisplays = 4;
        public int leftMonitorIndex = 1;
        public int rightMonitorIndex = 3;
        public int centerMonitorIndex = 0;
        public float fov = 50f;

        // Private Variables
        private List<MonitorDisplay> activatedDisplays = new List<MonitorDisplay>();

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
            public float renderScale;
            public int displayIndex;
            public bool useDisplay;
            public MonitorPosition monitorPosition;
        }

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
            int numDisplays = Display.displays.Length;

            for (int i = 0; i < numDisplays; i++)
            {
                if(i == leftMonitorIndex || i == rightMonitorIndex)
                {
                    MonitorDisplay md = new MonitorDisplay();
                    md.displayIndex = i;
                    md.display = Display.displays[i];
                    md.monitorPosition = i == rightMonitorIndex ? MonitorPosition.right : MonitorPosition.left;
                    md.useDisplay = true;
                    md.renderScale = 0.5f;
                    md.assCamera = CreateCam(md);
                    Display.displays[i].Activate();
                    activatedDisplays.Add(md);
                }
            }
        }

        private Camera CreateCam(MonitorDisplay md)
        {
            Camera monCam = Instantiate(Camera.main);
            monCam.transform.SetParent(Camera.main.transform.parent);
            monCam.transform.position = Camera.main.transform.position;
            monCam.transform.localRotation = OrientCamera(md.monitorPosition, monCam.fieldOfView);
            monCam.targetDisplay = md.displayIndex;
            return monCam;
        }

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.LeftBracket))
                fov--;
            if (Input.GetKeyDown(KeyCode.RightBracket))
                fov++;

            Camera.main.fieldOfView = fov;
            UpdateCameras();
        }

        private void UpdateCameras()
        {
            for (int i = 0; i < activatedDisplays.Count; i++)
            {
                if(activatedDisplays[i].assCamera.fieldOfView != fov)
                {
                    activatedDisplays[i].assCamera.fieldOfView = fov;
                    activatedDisplays[i].assCamera.transform.localRotation = OrientCamera(activatedDisplays[i].monitorPosition, activatedDisplays[i].assCamera.fieldOfView);
                }
            }
        }

        public Quaternion OrientCamera(MonitorPosition mp, float fov)
        {
            Quaternion newCamRot;

            switch(mp)
            {
                case MonitorPosition.center:
                    newCamRot = Quaternion.identity;
                    break;

                case MonitorPosition.left:
                    newCamRot = Quaternion.Euler(Vector3.up * (- fov - 30f));
                    break;

                case MonitorPosition.right:
                    newCamRot = Quaternion.Euler(Vector3.up * (fov + 30f));
                    break;

                default:
                    newCamRot = Quaternion.identity;
                    break;
            }

            return newCamRot;
        }
    }
}