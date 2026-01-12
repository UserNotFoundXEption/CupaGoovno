using UnityEngine;

namespace CupaGoovno;

public class Tutorial
{
    public static bool GetOrb()//new
    {
        GameObject parryOrb = GameObject.Find("tutorial_sphere_1");
        GameObject parryOrbCollider = GameObject.Find("tutorial_sphere_1_Collider");
        if (parryOrb != null && parryOrbCollider != null)
        {
            Transform arrow = parryOrb.transform.Find("Arrow");
            if (arrow != null)
            {
                UnityEngine.GameObject.Destroy(arrow.gameObject);
            }
            parryOrb.transform.parent = null;
            parryOrbCollider.transform.parent = parryOrb.transform;
            parryOrbCollider.transform.ResetLocalPosition();
            YoMamaFat.parryOrbTransform = parryOrb.transform;
            YoMamaFat.parryOrbTransform.position = new Vector3(2137f, 2137f);
            YoMamaFat.parryOrb = parryOrbCollider.GetComponent<TutorialLevelParryNext>();
            UnityEngine.GameObject.DontDestroyOnLoad(parryOrb);
            return true;
        }
        return false;
    }
}
