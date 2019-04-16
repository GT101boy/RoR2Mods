using UnityEngine;
using TMPro;
using BepInEx;
using System;

namespace ExtraInfo
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.grantimatter.ExtraInfo", "Extra Info", "0.1")]
    public class TMPStuff : BaseUnityPlugin
    {
        private static TextMeshProUGUI stageText;
        private static RectTransform stageTextRectT;
        private static int scene = 0;
        private static float timePassed = 0;
        private static Canvas[] cans;

        public static void CreateTextMeshProUGUI()
        {
            if (stageText != null)
                return;
            Canvas useCanvas = GameObject.Find("CursorIndicator(Clone)").GetComponent<Canvas>();

            if (useCanvas == null)
                return;

            GameObject TMPObject = new GameObject("ExtraInfoGO");
            TMPObject.transform.SetParent(useCanvas.transform);

            stageTextRectT = TMPObject.AddComponent<RectTransform>();
            stageText = TMPObject.AddComponent<TextMeshProUGUI>();

            stageTextRectT.sizeDelta = new Vector2(400f, 700f);
            stageTextRectT.anchorMin = new Vector2(0.17f, 0.5f);
            stageTextRectT.anchorMax = new Vector2(0.17f, 0.5f);
            stageTextRectT.pivot = new Vector2(0.5f, 0.5f);
            stageTextRectT.anchoredPosition = Vector2.zero;

            stageText.fontSize = 24f;
            stageText.alignment = TextAlignmentOptions.TopLeft;
            UpdateText(scene);
        }

        public static void UpdateText(int stage)
        {
            stageText.text = string.Concat("Stages Completed [ ", stage, " ]\n Loops Completed [ ", (int)Math.Floor(stage / 4.0d), " ]\n");
        }
    }
}