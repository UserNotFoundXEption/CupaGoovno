using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class MoaiLevelWarning
{
    public static GameObject Create(Vector2 pos, float rotation, Vector2 scale)
    {
        GameObject warning = GameObject.Instantiate<GameObject>(prefab);
        Other.FixShader(warning);

        warning.transform.position = pos;
        warning.transform.SetEulerAngles(0f, 0f, rotation);
        warning.transform.SetScale(scale.x, scale.y);

        warning.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1f, 0f, 0f);

        warnings.Add(warning);
        return warning;
    }

    public static GameObject Create(Vector2 pos, float rotation, Vector2 scale, UnityEngine.Color color)
    {
        GameObject warning = Create(pos, rotation, scale);
        warning.GetComponent<SpriteRenderer>().color = color;
        return warning;
    }

    public static IEnumerator warning_cr(Vector2 pos, float rotation, Vector2 scale, float time, MonoBehaviour parent)
    {
        GameObject obj = Create(pos, rotation, scale);
        yield return CupheadTime.WaitForSeconds(parent, time);
        if(obj != null)
        {
            GameObject.Destroy(obj);
        }
    }

    public static IEnumerator warning_cr(Vector2 pos, float rotation, Vector2 scale, float time, MonoBehaviour parent, UnityEngine.Color color)
    {
        GameObject obj = Create(pos, rotation, scale, color);
        yield return CupheadTime.WaitForSeconds(parent, time);
        if (obj != null)
        {
            GameObject.Destroy(obj);
        }
    }

    public static void DestroyAll()
    {
        while(warnings.Count > 0)
        {
            if (warnings[0] != null)
            {
                GameObject.Destroy(warnings[0]);
            }
            warnings.RemoveAt(0);
        }
    }

    private static List<GameObject> warnings = new List<GameObject>();

    public static GameObject prefab;
}
