using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class MouseGhost
{
    public void Init()
    {
        On.MouseLevelGhostMouse.spawn_cr += spawn_cr;
    }

    private IEnumerator spawn_cr(On.MouseLevelGhostMouse.orig_spawn_cr orig, MouseLevelGhostMouse self)
    {
        float spawnOffset = 150f * self.transform.localScale.x;
        float yPos = self.basePos.y + UnityEngine.Random.Range(-35f, 35f);
        Vector2 start = new Vector2(self.basePos.x * 0.125f + spawnOffset, yPos);
        self.hp = self.properties.CurrentState.ghostMouse.hp;
        self.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
        self.animator.SetTrigger("Spawn");
        float t = 0f;
        while (t < 1.083f)
        {
            self.transform.SetLocalPosition(new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start.x, self.basePos.x, t / 1.083f)), new float?(yPos), null);
            t += CupheadTime.Delta;
            yield return null;
        }
        self.transform.SetLocalPosition(new float?(self.basePos.x), new float?(yPos), null);
        yield return self.animator.WaitForAnimationToStart(self, "Idle_A", false);
        self.state = MouseLevelGhostMouse.State.Idle;
        self.StartCoroutine(move_cr(self, self.transform.localScale.x));//new
        self.StartCoroutine(rotateAfterSpawn_cr(self));//new
        yield break;
    }

    private IEnumerator rotateAfterSpawn_cr(MouseLevelGhostMouse self)//new
    {
        float t = 0f;
        float startRotation = self.transform.rotation.z;
        float endRotation = 90f * -self.transform.localScale.x;
        while (t < 1f)
        {
            self.transform.SetEulerAngles(null, null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, startRotation, endRotation, t)));
            t += CupheadTime.Delta;
            yield return null;
        }
        yield break;
    }

    private IEnumerator move_cr(MouseLevelGhostMouse self, float direction)//new
    {
        if (Mathf.Abs(self.transform.position.x) > 700f)
        {
            self.transform.SetEulerAngles(null, null, -self.transform.eulerAngles.z);
        }
        float speed = UnityEngine.Random.Range(50f, 100f);
        float end = 750f * direction;
        if (direction < 0f)
        {
            while (self.transform.position.x > end)
            {
                self.transform.AddPosition(direction * speed * CupheadTime.Delta, 0f, 0f);
                yield return null;
            }
        }
        else
        {
            while (self.transform.position.x < end)
            {
                self.transform.AddPosition(direction * speed * CupheadTime.Delta, 0f, 0f);
                yield return null;
            }
        }
        self.transform.SetPosition(null, UnityEngine.Random.Range(-100f, 300f), null);
        self.StartCoroutine(move_cr(self, -direction));
        yield break;
    }
}
