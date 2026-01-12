using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class HitFlash2
{
    public void Init()
    {
        On.HitFlash.flash_cr += flash_cr;
        On.HitFlash.SetColor += SetColor;
    }

    private IEnumerator flash_cr(On.HitFlash.orig_flash_cr orig, HitFlash self)
    {
        self.flashing = true;
        while (self.time > 0f)
        {
            SpriteRenderer spriteRenderer = self.GetComponent<SpriteRenderer>();//new start
            if (spriteRenderer != null)
            {
                originalColorDic[self] = spriteRenderer.color;
            }//new end
            self.SetColor(1f);
            yield return CupheadTime.WaitForSeconds(self, 0.0416f);
            self.SetColor(0f);
            yield return CupheadTime.WaitForSeconds(self, 0.0832f);
        }
        self.flashing = false;
        yield break;
    }

    public void SetColor(On.HitFlash.orig_SetColor orig, HitFlash self, float t)
    {
        if (self.self != null)
        {
            //Color color = Color.Lerp(self.self.normalColor, self.damageColor, t);
            Color color;//new start
            if (originalColorDic.ContainsKey(self))
            {
                color = Color.Lerp(originalColorDic[self], self.damageColor, t);
            }
            else
            {
                color = Color.Lerp(self.self.normalColor, self.damageColor, t);
            }//new end
            self.self.renderer.color = color;
        }
        foreach (HitFlash.RendererProperties rendererProperties in self.renderers)
        {
            Color color2 = Color.Lerp(rendererProperties.normalColor, self.damageColor, t);
            rendererProperties.renderer.color = color2;
        }
    }

    Dictionary<HitFlash, Color> originalColorDic = new Dictionary<HitFlash, Color>();
}
