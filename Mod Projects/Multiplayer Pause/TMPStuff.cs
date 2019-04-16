﻿using UnityEngine;
using TMPro;
using RoR2;
using BepInEx;

namespace MultiplayerPause
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.grantimatter.multiplayerpause", "Multiplayer Pause", "0.1")]
    public class TMPStuff : BaseUnityPlugin
    {
        public static void CreateTextMeshProUGUI()
        {
            TextMeshProUGUI stageText = FindObjectOfType<Canvas>().gameObject.AddComponent<TextMeshProUGUI>();
            RectTransform stageTextRectT = stageText.GetComponent<RectTransform>();

            stageTextRectT.sizeDelta = new Vector2(500f, 100f);
            stageTextRectT.anchorMin = new Vector2(0.17f, 0.045f);
            stageTextRectT.anchorMax = new Vector2(0.17f, 0.045f);
            stageTextRectT.pivot = new Vector2(0.5f, 0.5f);
            stageTextRectT.anchoredPosition = new Vector2(0, 0);

            stageText.fontSize = 32f;
            stageText.alignment = TextAlignmentOptions.Center;
            stageText.text = string.Concat("Stages Completed [ ", Run.instance.stageClearCount.ToString(), " ]");
        }
    }
}