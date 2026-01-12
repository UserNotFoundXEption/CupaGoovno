using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class Roulette
{
    public void Init()
    {
        On.DicePalaceRouletteLevelRoulette.marble_drop_cr += marble_drop_cr;
        On.DicePalaceRouletteLevelRoulette.SpawnMarble += SpawnMarble;
    }

    private IEnumerator marble_drop_cr(On.DicePalaceRouletteLevelRoulette.orig_marble_drop_cr orig, DicePalaceRouletteLevelRoulette self)
    {
        LevelProperties.DicePalaceRoulette.MarbleDrop p = self.properties.CurrentState.marbleDrop;
        string[] spawnPattern = p.marblePositionStrings.GetRandom<string>().Split(new char[]{','});
        float waitTime = 0f;
        self.state = DicePalaceRouletteLevelRoulette.State.Marble;
        self.firstLaunch = true;
        self.stopMarbles = false;
        self.animator.Play("Roulette_Attack_Start");
        AudioManager.Play("dice_palace_roulette_attack_start");
        self.emitAudioFromObject.Add("dice_palace_roulette_attack_start");
        yield return self.animator.WaitForAnimationToStart(self, "Roulette_Attack_Loop", false);
        AudioManager.PlayLoop("dice_palace_roulette_attack_loop");
        self.emitAudioFromObject.Add("dice_palace_roulette_attack_loop");
        self.StartCoroutine(self.marble_sound_cr());
        yield return CupheadTime.WaitForSeconds(self, p.marbleInitalDelay);
        SwitchPlatformsColliders(false);//new
        for (int i = 0; i < spawnPattern.Length; i++)
        {
            if (i == 3)//new start
            {
                SwitchCollider(self, false);
            }//new end
            if (spawnPattern[i][0] == 'D')
            {
                Parser.FloatTryParse(spawnPattern[i].Substring(1), out waitTime);
                yield return CupheadTime.WaitForSeconds(self, waitTime);
            }
            else
            {
                string[] array = spawnPattern[i].Split(new char[]
                {
                    '-'
                });
                foreach (string s in array)
                {
                    float xOffset = 0f;
                    Parser.FloatTryParse(s, out xOffset);
                    self.SpawnMarble(xOffset);
                    if(array.Length == 1 && xOffset != 550f)//new start
                    {
                        SpawnMarbleParryable(self, 1100f - xOffset);
                    }//new end
                }
            }
            i %= spawnPattern.Length;
            yield return CupheadTime.WaitForSeconds(self, p.marbleDelay);
        }
        self.stopMarbles = true;
        SwitchCollider(self, true);//new
        SwitchPlatformsColliders(true);//new
        yield return CupheadTime.WaitForSeconds(self, p.hesitate);
        self.animator.SetTrigger("Continue");
        AudioManager.Stop("dice_palace_roulette_attack_loop");
        AudioManager.Play("dice_palace_roulette_attack_end");
        self.emitAudioFromObject.Add("dice_palace_roulette_attack_end");
        yield return CupheadTime.WaitForSeconds(self, 0.5f);
        self.state = DicePalaceRouletteLevelRoulette.State.Idle;
        yield break;
    }

    private void SpawnMarble(On.DicePalaceRouletteLevelRoulette.orig_SpawnMarble orig, DicePalaceRouletteLevelRoulette self, float xOffset)//new
    {
        LevelProperties.DicePalaceRoulette.MarbleDrop marbleDrop = self.properties.CurrentState.marbleDrop;
        float rotation = Mathf.Atan2((float)Level.Current.Ground, 0f) * 57.29578f;
        Vector2 position = self.transform.position;
        //position.y = 360f;
        position.y = 450f;//new
        position.x = ((!self.onRight) ? (640f - xOffset) : (-640f + xOffset));
        BasicProjectile proj = self.marble.Create(position, rotation, marbleDrop.marbleSpeed);
        proj.transform.SetScale(2f, 2f, null);
    }

    private void SpawnMarbleParryable(DicePalaceRouletteLevelRoulette self, float xOffset)//new
    {
        LevelProperties.DicePalaceRoulette.MarbleDrop marbleDrop = self.properties.CurrentState.marbleDrop;
        float rotation = Mathf.Atan2((float)Level.Current.Ground, 0f) * 57.29578f;
        Vector2 position = self.transform.position;
        //position.y = 360f;
        position.y = 450f;//new
        position.x = ((!self.onRight) ? (640f - xOffset) : (-640f + xOffset));
        BasicProjectile proj = self.marble.Create(position, rotation, marbleDrop.marbleSpeed);
        proj.transform.SetScale(2f, 2f, null);
        proj.SetParryable(true);
        GameObject sprite = proj.transform.GetChild(0).gameObject;
        SpriteRenderer spriteRenderer = sprite.GetComponentInChildren<SpriteRenderer>();
        if(spriteRenderer != null)
        {
            spriteRenderer.color = Color.magenta;
        }
    }

    private void SwitchPlatformsColliders(bool enabled)//new
    {
        Color color = enabled ? Color.white : Color.grey;
        Vector2 offset = enabled ? Vector2.zero : new Vector2(2137f, 2137f);
        foreach(DicePalaceRouletteLevelPlatform platform in platforms)
        {
            CircleCollider2D collider = platform.gameObject.GetComponent<CircleCollider2D>();
            if(collider != null)
            {
                collider.offset = offset;
            }
            SpriteRenderer renderer = platform.gameObject.GetComponent<SpriteRenderer>();
            if(renderer != null)
            {
                renderer.color = color;
            }
        }
    }

    private void SwitchCollider(DicePalaceRouletteLevelRoulette self, bool enabled)
    {
        BoxCollider2D collider = self.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.enabled = enabled;
        }

        float alpha = enabled ? 1f : 0.5f;
        SpriteRenderer renderer = self.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = new Color(0f, 0f, 0f, alpha);
        }

        SpriteRenderer childRenderer = self.transform.GetChild(1).GetComponent<SpriteRenderer>();
        if (childRenderer != null)
        {
            childRenderer.color = new Color(1f, 1f, 1f, alpha);
        }
    }

    public static DicePalaceRouletteLevelPlatform[] platforms;
}
