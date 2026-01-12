using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace CupaGoovno;

public class RiftPopup : AbstractPausableComponent
{
    public static void Create(RiftManager.ParryScore score, RiftManager.Column column)
    {
        GameObject popupObject = new("RiftPopup");
        popupObject.transform.SetParent(GameObject.Find("RiftHUD").transform, false);

        Text popupText = popupObject.AddComponent<Text>();
        popupText.text = score.ToString();
        popupText.fontSize = 50;
        popupText.color = p.colors[score];
        popupText.font = Resources.GetBuiltinResource<UnityEngine.Font>("Arial.ttf");
        popupText.fontStyle = FontStyle.Bold;
        popupText.alignment = TextAnchor.MiddleCenter;

        RectTransform rectTransform = popupText.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(400, 200);
        rectTransform.anchorMin = p.positions[column];
        rectTransform.anchorMax = p.positions[column];
        rectTransform.anchoredPosition = new Vector2(0, 0);

        popupObject.AddComponent<RiftPopup>();
        popups.Add(popupObject);
    }

    public static void DestroyAll()
    {
        while(popups.Count > 0)
        {
            if (popups[0] != null)
            {
                Destroy(popups[0]);
            }
            popups.RemoveAt(0);
        }
    }

    public override void OnLevelEnd()
    {
        base.OnLevelEnd();
        Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(fade_cr());
    }

    private void Update()
    {
        transform.AddPosition(0f, p.speed * CupheadTime.delta);
    }

    private IEnumerator fade_cr()
    {
        yield return CupheadTime.WaitForSeconds(this, p.durationIntro);

        float t = 0f;
        while(t < p.durationFade)
        {
            t += CupheadTime.Delta;
            float alpha = Mathf.Lerp(1f, 0f, t / p.durationFade);
            GetComponent<Text>().color = new Color(GetComponent<Text>().color.r, GetComponent<Text>().color.g, GetComponent<Text>().color.b, alpha);
            yield return null;
        }

        yield break;
    }

    private static RiftProperties.Popup p = new();
    private static List<GameObject> popups = [];
}
