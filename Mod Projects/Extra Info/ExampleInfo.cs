using System;
using BepInEx;
using RoR2;
using UnityEngine;

namespace ExtraInfo
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.grantimatter.ExtraInfo", "Extra Info", "0.1")]

    public class ExampleInfo : BaseUnityPlugin
    {
        public void OnGUI()
        {
            /*
            Rect stagesCompletedRect = new Rect(new Vector2(25f, Screen.height - 40f), new Vector2(400, 200));
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.normal.textColor = Color.white;
            guiStyle.fontSize = 20;
            GUI.contentColor = Color.white;
            //GUIContent cameraRig = new GUIContent("" + FindObjectOfType<RoR2.CameraRigController>().name + "Rig");

            GUIContent cameraFOV = new GUIContent(Camera.main.fieldOfView.ToString() + " Physical Camera? " + Camera.main.usePhysicalProperties);
            GUIContent guiContent = new GUIContent(string.Concat("Stages Completed [ ", Run.instance.stageClearCount.ToString(), " ]"));
            //GUIContent cameraComponents = new GUIContent(String.Concat("Camera Components: ", Camera.main.transform.GetComponents<Component>()));
            
            GUILayout.BeginArea(stagesCompletedRect, cameraFOV, guiStyle);
            //GUILayout.BeginScrollView(Vector2.zero);
            GUILayout.EndArea();
            */

        }
    }
}
