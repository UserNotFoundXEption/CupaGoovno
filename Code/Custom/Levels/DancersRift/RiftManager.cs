using Blender.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace CupaGoovno;

public class RiftManager : AbstractPausableComponent
{
    public void Init()
    {
        instance = this;
        CopySpawnDelayList(p.spawnDelays);
        enemies = new()
        {
            { Column.Left, [] },
            { Column.Middle, [] },
            { Column.Right, [] }
        };
        points = 0;
        combo = 0;
        maxCombo = 0;
        pointsMultiplier = 1;
        scoreCounters = new int[5];
        StartCoroutine(main_cr());
        StartCoroutine(startMusic_cr());
        RiftHUD.Init();
    }

    public IEnumerator main_cr()
    {
        bool movedEnemiesThisBeat = false;
        bool spawnedLastEnemy = false;
        t = 0f;
        timeTotal = 0f;
        for(; ; )
        {
            t += CupheadTime.Delta;
            timeTotal += CupheadTime.Delta;

            if (t >= p.timePerBeat - p.enemyMoveTime && !movedEnemiesThisBeat)
            {
                UpdateEnemies();
                movedEnemiesThisBeat = true;
            }

            if (t >= p.timePerBeat)
            {
                t -= p.timePerBeat;
                if (!spawnedLastEnemy && CheckForEnemySpawn())
                {
                    spawnedLastEnemy = true;
                }
                EnemyForceStop();
                movedEnemiesThisBeat = false;
            }

            yield return null;
        }
    }

    private bool CheckForEnemySpawn()
    {
        currentBeat++;
        foreach (var delay in spawnDelays)
        {
            if (delay.beatsToSpawn == currentBeat)
            {
                RiftEnemyAbstract enemy;
                GameObject obj;
                switch (delay.type)
                {
                    case Enemies.Slime:
                        obj = Instantiate(enemyPrefabSlime);
                        enemy = obj.AddComponent<RiftEnemySlime>();
                        break;
                    case Enemies.Bird:
                        obj = Instantiate(enemyPrefabBird);
                        enemy = obj.AddComponent<RiftEnemyBird>();
                        break;
                    case Enemies.Cupcake:
                        obj = Instantiate(enemyPrefabCupcake);
                        enemy = obj.AddComponent<RiftEnemyCupcake>();
                        break;
                    default:
                        obj = Instantiate(enemyPrefabElterKettle);
                        enemy = obj.AddComponent<RiftEnemyElderKettle>();
                        break;
                }
                enemy.Init(delay);
                enemies[delay.column].Add(enemy);
            }
        }

        spawnDelays.RemoveAll(delay => delay.beatsToSpawn == currentBeat);
        if(spawnDelays.Count == 0)
        {
            StartCoroutine(win_cr());
        }
        return spawnDelays.Count == 0;
    }

    private void EnemyForceStop()
    {
        foreach (var column in enemies.Keys)
        {
            for (int i = 0; i < enemies[column].Count; i++)
            {
                RiftEnemyAbstract enemy = enemies[column][i];
                enemy.ForceStop();
            }
        }
    }

    private void UpdateEnemies()
    {
        foreach (var column in enemies.Keys)
        {
            for (int i = 0; i < enemies[column].Count; i++)
            {
                int countBefore = enemies[column].Count;
                RiftEnemyAbstract enemy = enemies[column][i];
                enemy.Move();
                int countAfter = enemies[column].Count;
                i += countAfter - countBefore;
            }
        }
    }

    public void OnStringParry(int id)
    {
        Column column = (Column)Enum.GetValues(typeof(Column)).GetValue(id);
        ParryScore score = GetParryScore(column);
        scoreCounters[(int)score]++;
        if (score == ParryScore.Miss)
        {
            OnMiss();
        }
        else
        {
            OnScore(score);
        }
        RiftPopup.Create(score, column);
    }

    private ParryScore GetParryScore(Column parriedColumn)
    {
        int c = 0;
        List<EnemyParryInfo> validEnemies = [];
        foreach(var column in enemies.Keys)
        {
            for (int i = 0; i < enemies[column].Count; i++)
            {
                RiftEnemyAbstract enemy = enemies[column][i];
                ParryScore score = enemy.GetParryScore(parriedColumn);
                c++;
                if (score != ParryScore.Miss)
                {
                    validEnemies.Add(new(enemy, score));
                }
            }
        }

        if(validEnemies.Count == 0)
        {
            return ParryScore.Miss;
        }
        else
        {
            float maxDifference = int.MinValue;
            EnemyParryInfo closestEnemy = validEnemies[0];
            foreach(EnemyParryInfo enemyParry in validEnemies)
            {
                float difference = enemyParry.enemy.GetDifference();
                if (difference > maxDifference)
                {
                    maxDifference = difference;
                    closestEnemy = enemyParry;
                }
            }

            closestEnemy.enemy.Damage();
            return closestEnemy.score;
        }
    }

    public void OnMiss()
    {
        combo = 0;
        pointsMultiplier = 1;
        RiftHUD.UpdateMultiplier(1);
        RiftHUD.UpdateCombo(0);
    }

    private void OnScore(ParryScore score)
    {
        points += p.points[(int)score] * pointsMultiplier;
        RiftHUD.UpdatePoints(points);

        combo++;
        if (combo > maxCombo)
        {
            maxCombo = combo;
        }
        RiftHUD.UpdateCombo(combo);

        int newMultiplier = pointsMultiplier;
        for(int i = 0; i < p.pointsMultiplierCombos.Length; i++)
        {
            if(combo >= p.pointsMultiplierCombos[i])
            {
                newMultiplier = i + 2;
            }
        }
        if (newMultiplier > pointsMultiplier)
        {
            pointsMultiplier = newMultiplier;
            RiftHUD.UpdateMultiplier(pointsMultiplier);
        }
    }

    private void CopySpawnDelayList(List<EnemySpawnDelay> list)
    {
        spawnDelays = new();
        foreach (var delay in list)
        {
            spawnDelays.Add(new EnemySpawnDelay(delay.type, delay.beatsToSpawn, delay.column, delay.arguments));
        }
    }

    private IEnumerator startMusic_cr()
    {
        yield return CupheadTime.WaitForSeconds(this, 0.18f);
        theme.volume = Other.GetMusicVolumeMultiplier();
        theme.Play();
    }

    public override void OnLevelEnd()
    {
        if (this != null)
        {
            StopAllCoroutines();
            StartCoroutine(CustomLevelUtils.endMusic_cr(theme));
        }
    }

    private IEnumerator win_cr()
    {
        yield return CupheadTime.WaitForSeconds(this, 5f);
        string message = "";
        message += $"Perfect: {scoreCounters[0]}\n";
        message += $"Great: {scoreCounters[1]}\n";
        message += $"Good: {scoreCounters[2]}\n";
        message += $"Ok: {scoreCounters[3]}\n";
        message += $"Miss: {scoreCounters[4]}\n";
        message += $"Max combo: {maxCombo}\n\n";

        RankInfo rankInfo = GetRankInfo();
        message += $"Rank: {rankInfo.name}\n";
        message += $"Points: {points}\n";
        message += $"Points to next rank: {rankInfo.pointsToNextRank}\n\n";

        message += $"The level will end in 20 seconds \n";

        char[] ranksValidForAchievement = ['B', 'A', 'S'];
        if (ranksValidForAchievement.Contains(rankInfo.name))
        {
            CustomAchievements.Unlock(CustomAchievements.Achievements.DancersRift);
        }

        yield return Other.notification_cr(
            this,
            "Congratulations!",
            message,
            20f,
            400f,
            true
            );

        RiftLevel.instance.zHack_OnWin();
    }

    private RankInfo GetRankInfo()
    {
        int count = p.rankNames.Count();
        for (int i = 0; i < count; i++)
        {
            if(points > p.rankPoints[i])
            {
                if (i == 0)
                {
                    return new(p.rankNames[i], 0);
                }
                else
                {
                    return new(p.rankNames[i], p.rankPoints[i - 1] - points);
                }
            }
        }
        return new(p.rankNames[count - 1], p.rankPoints[count - 2] - points);
    }

    public override void OnPause()
    {
        base.OnPause();
        theme.Pause();
    }

    public override void OnUnpause()
    {
        base.OnUnpause();
        theme.UnPause();
    }

    public static void KillAllEnemies()
    {
        foreach (var column in enemies.Keys)
        {
            while (enemies[column].Count > 0)
            {
                enemies[column][0].Die();
            }
        }
    }

    public static float t;
    public static float timeTotal;
    public static Dictionary<Column, List<RiftEnemyAbstract>> enemies;
    public static GameObject enemyPrefabElterKettle;
    public static GameObject enemyPrefabSlime;
    public static GameObject enemyPrefabBird;
    public static GameObject enemyPrefabCupcake;
    public static AudioSource theme;
    public static RiftManager instance;

    private int points;
    private int combo;
    private int maxCombo;
    private int pointsMultiplier;
    private int[] scoreCounters;
    private List<EnemySpawnDelay> spawnDelays;
    private int currentBeat;

    private static RiftProperties p = new();

    public struct EnemySpawnDelay
    {
        public EnemySpawnDelay(Enemies type, int beatsToSpawn, Column column, string arguments)
        {
            this.type = type;
            this.beatsToSpawn = beatsToSpawn;
            this.column = column;
            this.arguments = arguments;
        }

        public EnemySpawnDelay(Enemies type, int beatsToSpawn, Column column)
        {
            this.type = type;
            this.beatsToSpawn = beatsToSpawn;
            this.column = column;
            this.arguments = "";
        }

        public Enemies type;
        public int beatsToSpawn;
        public Column column;
        public string arguments;
    }

    private struct RankInfo
    {
        public RankInfo(char name, int pointsToNextRank)
        {
            this.name = name;
            this.pointsToNextRank = pointsToNextRank;
        }

        public char name;
        public int pointsToNextRank;
    }

    private struct EnemyParryInfo
    {
        public EnemyParryInfo(RiftEnemyAbstract enemy, ParryScore score)
        {
            this.enemy = enemy;
            this.score = score;
        }

        public RiftEnemyAbstract enemy;
        public ParryScore score;
    }

    public enum Enemies
    {
        ElderKettle,
        Slime,
        Bird,
        Cupcake
    }

    public enum Column
    {
        Left,
        Middle,
        Right
    }

    public enum ParryScore
    {
        Perfect,
        Great,
        Good,
        Ok,
        Miss
    }
}
