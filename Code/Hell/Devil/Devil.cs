using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class Devil
{
    public void Init()
    {
        new DevilSitting().Init();
        new DevilSpinnerProjectile().Init();
        new DevilArm().Init();
        new DevilGiantHead().Init();
        new DevilSkull().Init();
        new DevilDragon().Init();
        new DevilBomb().Init();
        new DevilOribitngProjectile().Init();
        new DevilHandProjectile().Init();
        new DevilSwooper().Init();
        new DevilTear().Init();
        On.DevilLevel.phase_1_end_trans += phase_1_end_trans;
        On.DevilLevel.OnDestroy += OnDestroy;
    }

    private IEnumerator phase_1_end_trans(On.DevilLevel.orig_phase_1_end_trans orig, DevilLevel self)
    {
        foreach (DevilLevelEffectSpawner devilLevelEffectSpawner in self.smokeSpawners)
        {
            devilLevelEffectSpawner.KillSmoke();
        }
        while (!DevilLevelHole.PHASE_1_COMPLETE)
        {
            yield return null;
        }
        DevilSitting.DestroySpinner(self.sittingDevil);//new
        self.groundHandler.SetActive(false);
        bool startZoomout = false;
        float t = 0f;
        float cameraSlideUpTime = 1f;
        float time = 3.3f;
        float endCameraTime = 2f;
        Vector3 phase1Start = self.phase1Scroll.transform.position;
        Vector3 phase1End = Vector3.zero;
        Vector3 cameraStart = CupheadLevelCamera.Current.transform.position;
        Vector3 cameraEffectEnd = new Vector3(CupheadLevelCamera.Current.transform.position.x, 50f);
        Vector3 cameraOffsetEnd = new Vector3(CupheadLevelCamera.Current.transform.position.x, 600f);
        foreach (ParallaxLayer parallaxLayer in self.parallax)
        {
            //parallaxLayer.enabled = false;
            if(parallaxLayer != null)//new start
            {
                parallaxLayer.enabled = false;
            }//new end
        }
        self.sittingDevil.RemoveFire();
        yield return self.StartCoroutine(CupheadLevelCamera.Current.slide_camera_cr(cameraEffectEnd, cameraSlideUpTime));
        self.StartCoroutine(CupheadLevelCamera.Current.slide_camera_cr(cameraOffsetEnd, time));
        while (t < time)
        {
            if (t >= 2f && !startZoomout)
            {
                self.ZoomOut(cameraStart, endCameraTime);
                startZoomout = true;
            }
            t += CupheadTime.Delta;
            float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
            self.phase1Scroll.transform.position = Vector3.Lerp(phase1Start, phase1End, val);
            Color c = self.phase1Foreground.color;
            c.a = Mathf.Clamp(1f - t * 2f, 0f, 1f);
            self.phase1Foreground.color = c;
            yield return null;
        }
        self.phase1Scroll.transform.position = phase1End;
        self.giantHead.transform.parent = null;
        self.giantHead.StartIntroTransform();
        self.StartCoroutine(self.phase2BackgroundFade_cr(2f));
        AudioManager.FadeBGMVolume(0f, 0.5f, true);
        AudioManager.PlayBGMPlaylistManually(false);
        AudioManager.Play("transition_sting");
        yield return CupheadTime.WaitForSeconds(self, endCameraTime);
        self.phase1Scroll.gameObject.SetActive(false);
        UnityEngine.Object.Destroy(self.sittingDevil.gameObject);
        devil.giantHead.SpawnSpiral();//new
        yield return null;
    }

    protected void OnDestroy(On.DevilLevel.orig_OnDestroy orig, DevilLevel self)
    {
        Camera.UnFollow();
        orig(self);
    }

    public static DevilLevel devil;
    public static DevilLevelPitchforkOrbitingProjectile orbiter;
}
