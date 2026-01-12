using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class FlowerEffect
{
    public void Init()
    {
        On.ChromaticAberrationFilmGrain.psychedelic_effect += psychedelic_effect;
        On.ChromaticAberrationFilmGrain.PsychedelicEffect += PsychedelicEffect;
        On.CupheadRenderer.TouchFuzzy += TouchFuzzy;
    }
    public void PsychedelicEffect(On.ChromaticAberrationFilmGrain.orig_PsychedelicEffect orig, ChromaticAberrationFilmGrain self, float amount, float speed, float time)
    {
        if (psychedelicCoroutine == null)
        {
            psychedelicCoroutine = self.StartCoroutine(self.psychedelic_effect(amount, speed, time));
        }
    }
    private IEnumerator psychedelic_effect(On.ChromaticAberrationFilmGrain.orig_psychedelic_effect orig, ChromaticAberrationFilmGrain self, float amount, float speed, float time)
    {
        float t = 0f;
        float slowdownTime = 0.5f;
        while (amount > 0f)
        {
            t += Time.deltaTime;
            float angle = speed * t;
            float phase = Mathf.Sin(angle) * amount;
            /*self.r = Vector2.up * phase;
            self.g = Vector2.up * phase / 2f;
            self.b = Vector2.down * phase;*/
            self.r = Vector2.up * phase + Vector2.left * phase / 2;//new start
            self.g = Vector2.up * phase / 2f + Vector2.right * phase;
            self.b = Vector2.down * phase;//new end
            if (t >= time)
            {
                amount -= slowdownTime;
            }
            yield return null;
        }
        self.r = self.rStart;
        self.g = self.gStart;
        self.b = self.bStart;
        yield return null;
        yield break;
    }

    public static void StopPsychedelicEffect(ChromaticAberrationFilmGrain self)//new
    {
        if (psychedelicCoroutine != null)
        {
            self.StopCoroutine(psychedelicCoroutine);
            psychedelicCoroutine = null;
        }
        self.r = self.rStart;
        self.g = self.gStart;
        self.b = self.bStart;
    }

    public void TouchFuzzy(On.CupheadRenderer.orig_TouchFuzzy orig, CupheadRenderer self, float amount, float speed, float time)
    {
        self.rendererCamera.GetComponent<ChromaticAberrationFilmGrain>().PsychedelicEffect(amount, speed, time);
        //self.StartCoroutine(self.change_blur_cr(time));
    }

    public static void StopFuzzy(CupheadRenderer self)//new
    {
        StopPsychedelicEffect(self.rendererCamera.GetComponent<ChromaticAberrationFilmGrain>());
    }

    private static Coroutine psychedelicCoroutine;
}
