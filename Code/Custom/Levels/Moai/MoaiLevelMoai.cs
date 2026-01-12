using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class MoaiLevelMoai : AbstractLevelEntity
{
    public override void Awake()
    {
        base.Awake();
        GetComponent<DamageReceiver>().OnDamageTaken += OnDamageTaken;
        damageDealer = DamageDealer.NewEnemy();
        damageDealer.SetDirection(DamageDealer.Direction.Neutral, transform);
        hp = MoaiLevel.ultra ? p.hp * 10f : p.hp;

        mainAttacksCounter = Enum.GetValues(typeof(MoaiAttacks.Main))
            .Cast<MoaiAttacks.Main>()
            .ToDictionary(k => k, v => 0);
        supportAttacksCounter = Enum.GetValues(typeof(MoaiAttacks.Support))
            .Cast<MoaiAttacks.Support>()
            .ToDictionary(k => k, v => 0);
    }

    private void Update()
    {
        if (damageDealer != null)
        {
            damageDealer.Update();
        }
    }

    public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
    {
        base.OnCollisionPlayer(hit, phase);
        if (phase != CollisionPhase.Exit)
        {
            damageDealer.DealDamage(hit);
        }
    }

    private void OnDamageTaken(DamageDealer.DamageInfo info)
    {
        hp -= info.damage;
        Level.Current.timeline.DealDamage(info.damage);
        if (hp <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        StopAllCoroutines();
        GetComponent<PolygonCollider2D>().enabled = false;
        LogStatsOnWin();
        MoaiLevel.instance.zHack_OnWin();
    }

    private void LogStatsOnWin()
    {
        MoaiUtils.CsvLogMessage("Battle won. Stats for each attack:", false, false);
        foreach (var attack in mainAttacksCounter)
        {
            float alpha = 0f;
            float beta = 0f;
            if(MoaiContinousLearningManager.mainLearners[attack.Key] is MoaiPGBeta pgBeta)
            {
                alpha = pgBeta.alpha;
                beta = pgBeta.beta;
            }
            MoaiUtils.CsvLogMessage($"{attack.Key}: used {attack.Value}, alpha {alpha}, beta {beta}", false, false);
        }
        foreach (var attack in supportAttacksCounter)
        {
            float alpha = 0f;
            float beta = 0f;
            if (MoaiContinousLearningManager.supportLearners[attack.Key] is MoaiPGBeta pgBeta)
            {
                alpha = pgBeta.alpha;
                beta = pgBeta.beta;
            }
            MoaiUtils.CsvLogMessage($"{attack.Key}: used {attack.Value}, alpha {alpha}, beta {beta}", false, false);
        }
    }

    public void Attack(MoaiAttacks attacks, float mainParameter, float supportParameter)
    {
        Attack(attacks.main, mainParameter);
        Attack(attacks.support, supportParameter);
    }

    public void Attack(MoaiAttacks.Main main, float parameter)
    {
        mainAttacksCounter[main]++;
        state = States.Attacking;
        switch (main)
        {
            case MoaiAttacks.Main.Laser:
                StartCoroutine(MoaiAttackLaser.attack_cr(parameter));
                break;
            case MoaiAttacks.Main.Shitlings:
                StartCoroutine(MoaiAttackShitlings.attack_cr(parameter));
                break;
            case MoaiAttacks.Main.GiantStone:
                StartCoroutine(MoaiAttackGiantStone.attack_cr(parameter));
                break;
            case MoaiAttacks.Main.Rockets:
                StartCoroutine(MoaiAttackRockets.attack_cr(parameter));
                break;
            case MoaiAttacks.Main.Pusher:
                StartCoroutine(MoaiAttackPusher.attack_cr(parameter));
                break;
        }
    }

    public void Attack(MoaiAttacks.Support support, float parameter)
    {
        supportAttacksCounter[support]++;
        switch (support)
        {
            case MoaiAttacks.Support.Pollen:
                StartCoroutine(MoaiAttackPollen.attack_cr(parameter));
                break;
            case MoaiAttacks.Support.Spikes:
                StartCoroutine(MoaiAttackSpikes.attack_cr(parameter));
                break;
            case MoaiAttacks.Support.Crackhead:
                StartCoroutine(MoaiAttackCrackhead.attack_cr(parameter));
                break;
            case MoaiAttacks.Support.Bouncers:
                StartCoroutine(MoaiAttackBouncers.attack_cr(parameter));
                break;
            case MoaiAttacks.Support.Baseball:
                StartCoroutine(MoaiAttackBaseball.attack_cr(parameter));
                break;
        }
    }
    
    public float hp;
    public States state;
    
    private static MoaiProperties.Moai p = new();

    private DamageDealer damageDealer;
    private Dictionary<MoaiAttacks.Main, int> mainAttacksCounter;
    private Dictionary<MoaiAttacks.Support, int> supportAttacksCounter;

    public enum States
    {
        Idle,
        Attacking
    }
}
