using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class BeeSpitProjectile
{
    public void Init()
    {
        On.BeeLevelQueenSpitProjectile.move_cr += move_cr;
        On.BeeLevelQueenSpitProjectile.rotate_cr += rotate_cr;
    }

    private IEnumerator move_cr(On.BeeLevelQueenSpitProjectile.orig_move_cr orig, BeeLevelQueenSpitProjectile self)
    {
        float scale = self.transform.localScale.x;
        self.transform.AddPosition(250f * scale, -500f, 0f);//new
        self.transform.AddEulerAngles(0f, 0f, 90f * scale);//new
        self.transform.SetScale(scale * 8f, 8f, null);//new
        for (; ; )
        {
            //Vector2 move = self.transform.right * self.speed * CupheadTime.Delta * scale;
            Vector2 move = new Vector3(0f, self.speed * CupheadTime.Delta);//new
            self.transform.AddPosition(move.x, move.y, 0f);
            yield return null;
            if (self.transform.position.y > 720f)
            {
                self.End();
            }
        }
    }

    private IEnumerator rotate_cr(On.BeeLevelQueenSpitProjectile.orig_rotate_cr orig, BeeLevelQueenSpitProjectile self)
    {/*
		float rotTime = 0.15f;
		float scale = self.transform.localScale.x;
		yield return CupheadTime.WaitForSeconds(self, 0.05f);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(self, self.time.x);
			yield return self.StartCoroutine(self.tweenRotation_cr(0f, 90f * scale, rotTime));
			yield return CupheadTime.WaitForSeconds(self, self.time.y);
			yield return self.StartCoroutine(self.tweenRotation_cr(90f * scale, 180f * scale, rotTime));
			AudioManager.Play("bee_spit_bullet_turn");
			self.emitAudioFromObject.Add("bee_spit_bullet_turn");
			yield return CupheadTime.WaitForSeconds(self, self.time.x);
			yield return self.StartCoroutine(self.tweenRotation_cr(180f * scale, 90f * scale, rotTime));
			yield return CupheadTime.WaitForSeconds(self, self.time.y);
			yield return self.StartCoroutine(self.tweenRotation_cr(90f * scale, 0f, rotTime));
		}*/
        yield break;
    }
}
