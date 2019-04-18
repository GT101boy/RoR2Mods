using UnityEngine;
using TMPro;
using BepInEx;
using System;

namespace ExtraInfo
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.grantimatter.ExtraInfo", "Extra Info", "0.1")]
    public class TmpStuff : BaseUnityPlugin
    {
        public static TextMeshProUGUI StageText { get; private set; }
        private static RectTransform _stageTextRectT;
        //private static Canvas[] cans;

        public static void CreateTextMeshProObject()
        {
            if (StageText != null)
                return;
            var useCanvas = GameObject.Find("CursorIndicator(Clone)").GetComponent<Canvas>();

            if (useCanvas == null)
                return;

            var tmpObject = new GameObject("ExtraInfoGO");
            tmpObject.transform.SetParent(useCanvas.transform);

            _stageTextRectT = tmpObject.AddComponent<RectTransform>();
            StageText = tmpObject.AddComponent<TextMeshProUGUI>();

            _stageTextRectT.sizeDelta = new Vector2(400f, 700f);
            _stageTextRectT.anchorMin = new Vector2(0.17f, 0.5f);
            _stageTextRectT.anchorMax = new Vector2(0.17f, 0.5f);
            _stageTextRectT.pivot = new Vector2(0.5f, 0.5f);
            _stageTextRectT.anchoredPosition = Vector2.zero;

            StageText.fontSize = 24f;
            StageText.alignment = TextAlignmentOptions.TopLeft;
            UpdateText(0);
        }

        public static void UpdateText(int stage)
        {
            StageText.text = string.Concat("Stages Completed [ ", stage, " ]\n Loops Completed [ ", (int)Math.Floor(stage / 4.0d), " ]\n");
        }

        public static void UpdateText(string[] strings)
        {
            
        }
    }
}