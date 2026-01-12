using System.Collections;
using UnityEngine;

namespace CupaGoovno;

public class PirateBoat
{
    public void Init()
    {
        On.PirateLevelBoat.transform_cr += transform_cr;
    }

    private IEnumerator transform_cr(On.PirateLevelBoat.orig_transform_cr orig, PirateLevelBoat self)
    {
        self.boatProperties = self.properties.CurrentState.boat;
        self.animator.Play("Idle");
        yield return CupheadTime.WaitForSeconds(self, 0.3f);
        self.animator.SetTrigger("OnTransform");
        AudioManager.Play("level_pirate_boat_transform");
        self.emitAudioFromObject.Add("level_pirate_boat_transform");
        yield return CupheadTime.WaitForSeconds(self, self.boatProperties.winceDuration);
        self.animator.SetTrigger("OnTransformContinue");
        yield return CupheadTime.WaitForSeconds(self, 1f);
        self.ully.gameObject.SetActive(true);
        self.StartCoroutine(warningBeam_cr(self));//new
        self.StartCoroutine(fallingObjects_cr(self));//new
        for (; ; )
        {
            for (int count = 0; count < self.boatProperties.bulletCount; count++)
            {
                yield return CupheadTime.WaitForSeconds(self, self.boatProperties.attackDelay);
                self.animator.SetTrigger("OnShoot");
            }
            yield return CupheadTime.WaitForSeconds(self, self.boatProperties.bulletPostWait);
            self.animator.SetTrigger("OnBeamStart");
            yield return CupheadTime.WaitForSeconds(self, self.boatProperties.beamDelay + 1f);
            self.animator.SetTrigger("OnBeamContinue");
            CupheadLevelCamera.Current.StartShake(2f);
            //self.beam = self.beamPrefab.Create(self.beamRoot);
            self.beam = PirateBoatBeam.Create(self.beamPrefab, self.beamRoot, beamRotation);//new
            yield return CupheadTime.WaitForSeconds(self, self.boatProperties.beamDuration);
            self.animator.SetTrigger("OnBeamEnd");
            CupheadLevelCamera.Current.EndShake(0.4f);
            self.beam.EndBeam();
            self.beam = null;
            yield return CupheadTime.WaitForSeconds(self, self.boatProperties.beamPostWait);
            self.animator.Play("Transform_Idle");
        }
    }

    private IEnumerator warningBeam_cr(PirateLevelBoat self)//new
    {
        GameObject rectangle = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rectangle.transform.localScale = new Vector3(1500f, 40f, 1f);
        rectangle.transform.position = new Vector3(-250f, -70, 0f);
        Other.SetTransparentMaterial(rectangle, new Color(1f, 0.5f, 1f, 0.6f));

        GameObject pivot = new GameObject("Pivot");
        pivot.transform.position = rectangle.transform.position + new Vector3(750, 0, 0);
        rectangle.transform.parent = pivot.transform;

        float rotationSpeed = 1f;
        MinMax angleRange = new MinMax(-20f, 10f);

        float t = 0;
        for (; ; )
        {
            rotationSpeed = 2f - self.properties.CurrentHealth / self.properties.TotalHealth * 3;
            t += CupheadTime.Delta * rotationSpeed;
            beamRotation = angleRange.GetFloatAt((Mathf.Sin(t) + 1) / 2);
            pivot.transform.SetEulerAngles(null, null, beamRotation);
            if (self.beam != null)
            {
                self.beam.transform.SetEulerAngles(null, null, beamRotation);
            }
            yield return null;
        }
    }

    private IEnumerator fallingObjects_cr(PirateLevelBoat self)//new
    {
        MouseLevelFallingObject obj = YoMamaFat.fallingObject;
        LevelProperties.Mouse.Claw p = YoMamaFat.fallingObjectProperties;

        for (; ; )
        {
            float diff = 50f;
            for (int i = 0; i < 4; i++)
            {
                obj.Create(diff + 800f - 200f * i, p);
                yield return CupheadTime.WaitForSeconds(self, p.objectSpawnDelay * 1.5f);
            }
            for (int i = 0; i < 4; i++)
            {
                obj.Create(diff + 200f * i, p);
                yield return CupheadTime.WaitForSeconds(self, p.objectSpawnDelay * 1.5f);
            }
            diff = -diff;
        }
    }

    private float beamRotation;
}
