using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;

namespace CupaGoovno;

public static class RiftHUD
{
    public static void Init()
    {
        GameObject canvasObject = new GameObject("RiftHUD");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        GameObject multiplier = new GameObject("Multiplier");
        multiplier.transform.SetParent(canvas.transform, false);

        multiplierText = multiplier.AddComponent<Text>();
        multiplierText.text = "x1";
        multiplierText.fontSize = 150;
        multiplierText.color = UnityEngine.Color.white;
        multiplierText.font = Resources.GetBuiltinResource<UnityEngine.Font>("Arial.ttf");
        multiplierText.fontStyle = UnityEngine.FontStyle.Bold;
        multiplierText.alignment = TextAnchor.UpperLeft;

        RectTransform multiplierRect = multiplierText.GetComponent<RectTransform>();
        multiplierRect.sizeDelta = new Vector2(400, 300);
        multiplierRect.anchorMin = new Vector2(1f, 0f);
        multiplierRect.anchorMax = new Vector2(1f, 0f);
        multiplierRect.anchoredPosition = new Vector2(-250, 30);
        multiplierRect.SetEulerAngles(0f, 0f, 5f);

        GameObject combo = new GameObject("Multiplier");
        combo.transform.SetParent(canvas.transform, false);

        comboText = combo.AddComponent<Text>();
        comboText.text = "COMBO: 0";
        comboText.fontSize = 40;
        comboText.color = UnityEngine.Color.white;
        comboText.font = Resources.GetBuiltinResource<UnityEngine.Font>("Arial.ttf");
        comboText.fontStyle = UnityEngine.FontStyle.Bold;
        comboText.alignment = TextAnchor.UpperLeft;

        RectTransform comboRect = comboText.GetComponent<RectTransform>();
        comboRect.sizeDelta = new Vector2(400, 200);
        comboRect.anchorMin = new Vector2(1f, 0f);
        comboRect.anchorMax = new Vector2(1f, 0f);
        comboRect.anchoredPosition = new Vector2(-70, 25);
        comboRect.SetEulerAngles(0f, 0f, -5f);

        GameObject points = new GameObject("Multiplier");
        points.transform.SetParent(canvas.transform, false);

        pointsText = points.AddComponent<Text>();
        pointsText.text = "0";
        pointsText.fontSize = 36;
        pointsText.color = UnityEngine.Color.white;
        pointsText.font = Resources.GetBuiltinResource<UnityEngine.Font>("Arial.ttf");
        pointsText.fontStyle = UnityEngine.FontStyle.Bold;
        pointsText.alignment = TextAnchor.UpperLeft;

        RectTransform pointsRect = pointsText.GetComponent<RectTransform>();
        pointsRect.sizeDelta = new Vector2(400, 200);
        pointsRect.anchorMin = new Vector2(1f, 0f);
        pointsRect.anchorMax = new Vector2(1f, 0f);
        pointsRect.anchoredPosition = new Vector2(-70, -35);
        pointsRect.SetEulerAngles(0f, 0f, -5f);
    }

    public static void UpdateMultiplier(int multiplier)
    {
        multiplierText.text = "x" + multiplier.ToString();
    }

    public static void UpdateCombo(int combo)
    {
        comboText.text = "COMBO: " + combo.ToString();
    }

    public static void UpdatePoints(int points)
    {
        pointsText.text = points.ToString();
    }

    private static Text multiplierText;
    private static Text comboText;
    private static Text pointsText;
}
