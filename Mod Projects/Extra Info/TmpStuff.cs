using UnityEngine;
using TMPro;
using BepInEx;
using System.Collections.Generic;

namespace ExtraInfo
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.grantimatter.ExtraInfo", "Extra Info", "0.1")]
    public class TmpStuff : BaseUnityPlugin
    {
        #region Canvas Names
        
        public const string MainCanvasName = "MainCanvas";
        public const string CursorCanvasName = "CursorIndicator(Clone)";
        public const string ObjectivePanelCanvasName = "ObjectivePanel";
        public const string ItemInventoryCanvasName = "ItemInventoryDisplay";
        public const string ScoreboardPanelCanvasName = "ScoreboardPanel";
        public const string ChatBoxCanvas = "ChatBoxRoot";
        public const string HealthBarCanvas = "HealthBarRoot";
        public const string TimerPanelCanvas = "TimerPanel";
        public const string ScreenTintCanvas = "ScreenTintCanvas(Clone)";
        public const string ItemsBackgroundCanvas = "ItemsBackground";
        public const string CrosshairCanvas = "CrosshairCanvas";
        #endregion
            
        //public static List<TextArea> textAreas = new List<TextArea>();
        public static Dictionary<string, TextArea> textAreas = new Dictionary<string, TextArea>();
        private static RectTransform _stageTextRectT;
        //private static Canvas[] cans;
        public static TextMeshProUGUI StageText { get; private set; }

        public struct TextArea
        {
            public GameObject containerObject;
            public TextMeshProUGUI textObject;
        }
        
        // Update text area info

        #region UpdateText
        public static void UpdateText(string txtAreaName, string str)
        {
            if (textAreas.ContainsKey(txtAreaName))
            {
                textAreas[txtAreaName].textObject.text = str;
            }
        }

        public static void UpdateText(string txtAreaName, string[] strings)
        {
            if(textAreas.ContainsKey(txtAreaName))
            {
                textAreas[txtAreaName].textObject.text = string.Concat(strings);
            }
        }
        #endregion
        
        // Create a text area with or without info already populated
        public static void CreateTextArea(Rect textRect, float fontSize, Color fontColor, string targetCanvas, string textAreaName, bool alterBright = true, string[] strings = null)
        {
            var newArea = new TextArea();
            newArea.containerObject = new GameObject(string.Concat(textAreaName, " "));
            newArea.containerObject.transform.SetParent(GameObject.Find(targetCanvas).transform);
            newArea.containerObject.transform.localPosition = Vector3.zero;
            
            var rt = newArea.containerObject.AddComponent<RectTransform>();
            newArea.textObject = newArea.containerObject.AddComponent<TextMeshProUGUI>();
            
            rt.sizeDelta = new Vector2(textRect.width, textRect.height);
            rt.anchorMin = new Vector2(textRect.x, textRect.y);
            rt.anchorMax = new Vector2(textRect.x, textRect.y);
            rt.pivot = new Vector2(0f, 1f);
            rt.anchoredPosition = Vector2.zero;
            
            
            newArea.textObject.alignment = TextAlignmentOptions.TopLeft;
            newArea.textObject.fontSize = fontSize;
            
            newArea.textObject.color = fontColor;
            newArea.textObject.raycastTarget = false;

            newArea.textObject.text = string.Concat(strings);
            //newArea.textObject.text = "IS IT JUST NOT GETTING ANY DATA????";
            
            textAreas.Add(textAreaName, newArea);
        }
        
        public static void HideTextArea(string textAreaName)
        {
            if(textAreas.ContainsKey(textAreaName))
            {
                textAreas[textAreaName].textObject.canvasRenderer.SetAlpha(0f);
            }
        }

        public static void ShowTextArea(string textAreaName)
        {
            if(textAreas.ContainsKey(textAreaName))
            {
                textAreas[textAreaName].textObject.canvasRenderer.SetAlpha(1f);
            }
        }
        
        public static void CreateTextArea(float xPos, float yPos, float width, float height, float fontSize, Color fontColor, string targetCanvas, string textAreaName, string defaultText = "", bool alterBright = true)
        {
            CreateTextArea(new Rect(xPos, yPos, width, height), fontSize, fontColor, targetCanvas, textAreaName, alterBright);
        }
        
        public static void CreateTextArea(float xPos, float yPos, float width, float height, float fontSize, Color fontColor, string targetCanvas, string textAreaName, string[] defaultText = null, bool alterBright = true)
        {
            CreateTextArea(new Rect(xPos, yPos, width, height), fontSize, fontColor, targetCanvas, textAreaName, alterBright, defaultText);
        }
    }
}