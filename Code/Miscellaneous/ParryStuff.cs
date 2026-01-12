using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking.Match;

namespace CupaGoovno;

public class ParryStuff
{
    public void Init()
    {
        On.AbstractParryEffect.OnCollision += OnCollision;
        On.AbstractParryEffect.hit_cr += hit_cr;
        On.LevelPlayerMotor.OnGrounded += OnGrounded;
        On.LevelPlayerMotor.ForceParry += ForceParry;
        On.LevelPlayerMotor.OnParryComplete += OnParryComplete;
        On.ParrySwitch.parryCooldown_cr += parryCooldown_cr;
    }

    protected void OnCollision(On.AbstractParryEffect.orig_OnCollision orig, AbstractParryEffect self, object hit2, CollisionPhase phase)
    {
        GameObject hit = (GameObject)hit2;
        lastParriedName = hit.name;

        if (self.cancel)
        {
            return;
        }

        if (!self.player.IsDead && phase == CollisionPhase.Enter)
        {
            AbstractProjectile component = hit.GetComponent<AbstractProjectile>();
            if (component == null)
            {
                CollisionChild component2 = hit.GetComponent<CollisionChild>();
                AbstractCollidableObject abstractCollidableObject;
                if (component2 != null && component2.ForwardParry(out abstractCollidableObject))
                {
                    component = abstractCollidableObject.GetComponent<AbstractProjectile>();
                }
            }

            if (component != null && component.CanParry)
            {
                self.projectiles.Add(component);
                if (!self.player.stats.NextParryActivatesHealerCharm())
                {
                    self.sparks.Add(self.spark.Create(component.transform.position));
                }
                if (!self.didHitSomething)
                {
                    self.StartCoroutine(self.hit_cr(false));
                }
            }

            ParrySwitch component3 = hit.GetComponent<ParrySwitch>();
            if (component3 != null && component3.enabled && component3.IsParryable)
            {
                self.switches.Add(component3);
                if (!self.didHitSomething)
                {
                    self.StartCoroutine(self.hit_cr(false));
                }
            }

            AbstractLevelEntity component4 = hit.GetComponent<AbstractLevelEntity>();
            if (component4 != null && component4.enabled && component4.canParry)
            {
                self.entities.Add(component4);
                if (!self.didHitSomething)
                {
                    self.StartCoroutine(self.hit_cr(false));
                }
            }

            //if ((self.player.stats.Loadout.charm == Charm.charm_parry_attack || self.player.stats.CurseWhetsone) && !self.didHitSomething && !Level.IsChessBoss)
            bool whetstone = self.player.stats.Loadout.charm == Charm.charm_parry_attack;//new start
            bool relic = self.player.stats.CurseWhetsone;
            bool casualMode = self.player.stats.Loadout.charm == CustomCharms.casualMode;
            bool noChess = !Level.IsChessBoss;
            bool noDamage = !self.didHitSomething;
            if ((whetstone || relic || casualMode) && noChess && noDamage)//new end
            {
                IParryAttack component5 = self.player.GetComponent<IParryAttack>();
                if (component5 != null && !component5.AttackParryUsed)
                {
                    DamageReceiver damageReceiver = hit.GetComponent<DamageReceiver>();
                    if (damageReceiver == null)
                    {
                        DamageReceiverChild component6 = hit.GetComponent<DamageReceiverChild>();
                        if (component6 != null)
                        {
                            damageReceiver = component6.Receiver;
                        }
                    }

                    if (damageReceiver != null && damageReceiver.type == DamageReceiver.Type.Enemy)
                    {
                        component5.HasHitEnemy = true;
                        float damage = WeaponProperties.CharmParryAttack.damage * Other.GetDamageMultiplier(false, self.player.id);//new
                        DamageDealer damageDealer = new DamageDealer(damage, 0f, false, true, false);//new
                        //DamageDealer damageDealer = new DamageDealer(WeaponProperties.CharmParryAttack.damage, 0f, false, true, false);
                        damageDealer.DealDamage(hit);
                        self.ShowParryAttackEffect(hit);
                        self.StartCoroutine(self.hit_cr(true));
                        self.player.stats.SuperChangedFromParry(0.1f);//new start
                        if (SuperSandevistan.Active)
                        {
                            float maxSuper = self.player.stats.SuperMeterMax;
                            float maxDamage = new CustomSuperProperties.Sandevistan().maxDamage;
                            self.player.stats.SuperMeter -= maxSuper * damage / maxDamage;
                        }//new end
                    }
                }
            }

            if (SceneManager.GetActiveScene().name == Scenes.scene_level_flying_genie.ToString())//new start
            {
                GameObject[] objs = GameObject.FindObjectsOfType<GameObject>();
                foreach (GameObject obj in objs)
                {
                    if (obj.name == "FlyingGenie_Gem_Stone_Pink(Clone)")
                    {
                        GameObject.Destroy(obj);
                    }
                }
            }
        }
    }

    private IEnumerator hit_cr(On.AbstractParryEffect.orig_hit_cr orig, AbstractParryEffect self, bool hitEnemy = false)
    {
        bool cooldown = false;//new start
        bool sugarEquipped = self.player.stats.Loadout.charm == Charm.charm_parry_plus;
        bool regularLevel = self is LevelPlayerParryEffect;
        bool dancersRift = SceneManager.GetActiveScene().name == "scene_level_dancers_rift";

        if ((Level.IsChessBoss || !sugarEquipped) && !dancersRift)
        {
            yield return orig(self, hitEnemy);
            yield break;
        }//new end

        if (self.player.IsDead || !self.player.gameObject.activeInHierarchy || !self.gameObject.activeInHierarchy)
        {
            yield break;
        }
        bool hit = false;
        self.didHitSomething = true;
        IParryAttack parryController = self.player.GetComponent<IParryAttack>();
        if (parryController != null)
        {
            parryController.AttackParryUsed = true;
        }
        self.animator.enabled = true;
        //self.sprites.SetActive(true);
        if (!hitEnemy)
        {
            foreach (ParrySwitch parrySwitch in self.switches)
            {
                //parrySwitch.OnParryPrePause(self.player);
                if (sugarEquipped && regularLevel)//new start
                {
                    if (!cooldownParryList.Contains(parrySwitch.gameObject))
                    {
                        cooldownParryList.Add(parrySwitch.gameObject);
                        Other.Player().StartCoroutine(parryCooldown_cr(parrySwitch.gameObject));
                        parrySwitch.OnParryPrePause(self.player);
                    }
                    else
                    {
                        cooldown = true;
                    }
                }
                else
                {
                    parrySwitch.OnParryPrePause(self.player);
                }//new end
            }
            foreach (AbstractLevelEntity abstractLevelEntity in self.entities)
            {
                abstractLevelEntity.OnParry(self.player);
            }
            foreach (AbstractProjectile abstractProjectile in self.projectiles)
            {
                abstractProjectile.OnParry(self.player);
                //self.player.stats.OnParry(abstractProjectile.ParryMeterMultiplier, abstractProjectile.CountParryTowardsScore);
                if (sugarEquipped)//new start
                {
                    self.player.stats.OnParry(abstractProjectile.ParryMeterMultiplier * 1.4f, abstractProjectile.CountParryTowardsScore);
                }
                else
                {
                    self.player.stats.OnParry(abstractProjectile.ParryMeterMultiplier, abstractProjectile.CountParryTowardsScore);
                }//new end
            }
        }
        if (self.player.IsDead || !self.player.gameObject.activeInHierarchy || !self.gameObject.activeInHierarchy)
        {
            yield break;
        }

        if (!cooldown)//new
        {//new
            self.sprites.SetActive(true);//new
            if (Level.Current == null || !Level.IsChessBoss || !Level.Current.Ending)
            {
                PauseManager.Pause();
            }
            AudioManager.Play("player_parry");
            self.OnPaused();
        }//new

        float pauseTime = (!hitEnemy) ? 0.185f : 0.13875f;
        if (cooldown || dancersRift)//new start
        {
            pauseTime = 0f;
        }//new end
        float t = 0f;
        while (t < pauseTime)
        {
            hit = self.IsHit;
            if (hit)
            {
                t = pauseTime;
            }
            t += Time.fixedDeltaTime;
            for (int i = 0; i < 2; i++)
            {
                PlayerId playerId = (i != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne;
                if (self.player != null && self.player.id == playerId)
                {
                    if (pauseTime - t < 0.134f)
                    {
                        self.player.BufferInputs();
                    }
                }
                else
                {
                    AbstractPlayerController abstractPlayerController = PlayerManager.GetPlayer(playerId);
                    if (abstractPlayerController != null)
                    {
                        abstractPlayerController.BufferInputs();
                    }
                }
            }
            yield return new WaitForFixedUpdate();
        }

        while (LevelNewPlayerGUI.Current != null && LevelNewPlayerGUI.Current.gameObject.activeInHierarchy)
        {
            yield return null;
        }
        if (!hit)
        {
            self.OnSuccess();
            if (Level.Current == null || !Level.IsChessBoss || !Level.Current.Ending)
            {
                PauseManager.Unpause();
            }
            self.OnUnpaused();
            self.OnEnd();
            self.transform.parent = null;
            self.GetComponent<Collider2D>().enabled = false;
            if (!hitEnemy)
            {
                foreach (ParrySwitch parrySwitch2 in self.switches)
                {
                    parrySwitch2.OnParryPostPause(self.player);
                }
            }
        }
    }

    private IEnumerator parryCooldown_cr(GameObject gameObject)
    {
        yield return CupheadTime.WaitForSeconds(Other.Player(), 0.1f);
        if (gameObject != null && cooldownParryList.Contains(gameObject))
        {
            cooldownParryList.Remove(gameObject);
        }
    }

    private void OnGrounded(On.LevelPlayerMotor.orig_OnGrounded orig, LevelPlayerMotor self)
    {
        bool isNoBetterParryLevel = false;
        Levels[] noBetterParryLevels = [Levels.Devil, Levels.Veggies, Levels.Clown, Levels.Train, Levels.Dragon, Levels.Mouse, Levels.DicePalaceCigar];
        foreach (Levels level in noBetterParryLevels)
        {
            if (Level.Current.CurrentLevel == level)
            {
                isNoBetterParryLevel = true;
            }
        }

        bool parrying = self.Parrying && self.parryManager.state == LevelPlayerMotor.ParryManager.State.NotReady;
        bool onPlatform = self.directionManager.down.gameObject.GetComponent<LevelPlatform>() != null;
        bool sugarEquiped = PlayerManager.GetFirst().stats.Loadout.charm == Charm.charm_parry_plus;

        bool ignorePlatform = !isNoBetterParryLevel && parrying && onPlatform && !sugarEquiped;
        if (!ignorePlatform || !Settings.betterPlatforms)
        {
            orig(self);
        }
    }

    private void ForceParry(On.LevelPlayerMotor.orig_ForceParry orig, LevelPlayerMotor self)
    {
        if(SceneManager.GetActiveScene().name == "scene_level_dancers_rift")
        {
            orig(self);
        }
        else
        {
            bool tmp = self.hardExitParry;
            self.hardExitParry = false;
            orig(self);
            self.hardExitParry = tmp;
        }

        /*if (self.hitManager.state != LevelPlayerMotor.HitManager.State.Hit)//new
        //if (self.hitManager.state != LevelPlayerMotor.HitManager.State.Hit && !self.hardExitParry)
        {
            self.hitManager.state = LevelPlayerMotor.HitManager.State.Inactive;
            self.parryManager.state = LevelPlayerMotor.ParryManager.State.NotReady;
            self.Parrying = true;
            if (self.OnParryEvent != null)
            {
                self.OnParryEvent();
            }
        }*/
    }

    public void OnParryComplete(On.LevelPlayerMotor.orig_OnParryComplete orig, LevelPlayerMotor self)
    {
        orig(self);
        bool dancersRift = SceneManager.GetActiveScene().name == "scene_level_dancers_rift";
        if (dancersRift)
        {
            self.velocityManager.y *= 0.75f;
        }
    }

    public IEnumerator parryCooldown_cr(On.ParrySwitch.orig_parryCooldown_cr orig, ParrySwitch self)
    {
        float t = 0f;
        while (t < self.coolDown)
        {
            t += CupheadTime.Delta / SuperSandevistan.multiplier;//new
            //t += CupheadTime.Delta;
            yield return null;
        }
        Collider2D collider = self.GetComponent<Collider2D>();
        collider.enabled = true;
        yield return null;
        yield break;
    }

    public static string lastParriedName = "";

    private List<GameObject> cooldownParryList = new List<GameObject>();
}
