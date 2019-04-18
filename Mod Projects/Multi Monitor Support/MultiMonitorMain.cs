using System;
using System.Collections.Generic;
using BepInEx;
using UnityEngine;

namespace MultiMonitor
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
        public static float bezelCorrection = 1f;
        //public float fov = 50f;

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
            public Camera assCam;
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
                    md.assCam = CreateCam(md);
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
            monCam.transform.localRotation = OrientCamera(md, monCam);
            monCam.targetDisplay = md.displayIndex;
            return monCam;
        }

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.LeftBracket))
                bezelCorrection -= 0.1f;
            if (Input.GetKeyDown(KeyCode.RightBracket))
                bezelCorrection += 0.1f;

            UpdateCameras();
        }

        private void UpdateCameras()
        {
            for (int i = 0; i < activatedDisplays.Count; i++)
            {
                if(activatedDisplays[i].assCam.fieldOfView != Camera.main.fieldOfView)
                {
                    activatedDisplays[i].assCam.fieldOfView = Camera.main.fieldOfView;
                    activatedDisplays[i].assCam.transform.localRotation = OrientCamera(activatedDisplays[i], activatedDisplays[i].assCam);
                }
            }
        }

        private static Quaternion OrientCamera(MonitorDisplay md, Camera cam)
        {
            Quaternion newCamRot;
            var radAngle = (cam.fieldOfView * bezelCorrection) * Mathf.Deg2Rad;
            var radHFOV = (float)(2f * Math.Atan(Mathf.Tan(radAngle / 2) * cam.aspect));
            var hFOV = (Mathf.Rad2Deg * radHFOV);
            //float hFOV = md.assCam.fieldOfView;

            switch(md.monitorPosition)
            {
                case MonitorPosition.center:
                    newCamRot = Quaternion.identity;
                    break;

                case MonitorPosition.left:
                    newCamRot = Quaternion.Euler(Vector3.up * (- hFOV));
                    break;

                case MonitorPosition.right:
                    newCamRot = Quaternion.Euler(Vector3.up * (hFOV));
                    break;

                default:
                    newCamRot = Quaternion.identity;
                    break;
            }

            return newCamRot;
        }
    }
}