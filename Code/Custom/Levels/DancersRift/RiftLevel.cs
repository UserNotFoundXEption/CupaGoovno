using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class RiftLevel : Level
{
    public override Levels CurrentLevel => (Levels)Enum.Parse(typeof(Levels), "RiftLevel");
    public override Scenes CurrentScene => (Scenes)Enum.Parse(typeof(Scenes), "scene_level_dancers_rift");
    public override Sprite BossPortrait => RiftManager.enemyPrefabElterKettle.GetComponent<SpriteRenderer>().sprite;
    public override string BossQuote { get { return "Even Steven Hawking used to move better than you"; } }

    public override void OnLevelStart()
    {
        base.OnLevelStart();

        DestroyNonGroundColliders();
        GameObject.Find("Level_Ground").transform.AddPosition(0f, -250f, 0f);

        instance = this;
        manager.Init();
    }

    public override void PartialInit()
    {
        timeline = new();
        timeline.health = 1;
        goalTimes = new GoalTimes(300f, 300f, 300f);
        base.PartialInit();
    }

    public override void Awake()
    {
        base.Awake();

        FixSpriteShaders();
        GetPrefabs();

        for (int i = 0; i < 3; i++)
        {
            RiftString riftString = RiftString.Create(i);
        }

        manager = gameObject.AddComponent<RiftManager>();
    }

    public override void CreatePlayers()
    {
        blockChalice = true;
        base.CreatePlayers();
    }

    private void FixSpriteShaders()
    {
        GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            Shader errorShader = Shader.Find("Hidden/InternalErrorShader");
            Shader defaultShader = Shader.Find("Sprites/Default");
            if (spriteRenderer != null && spriteRenderer.material.shader == errorShader)
            {
                spriteRenderer.material = new Material(defaultShader);
            }
        }
    }

    private void DestroyNonGroundColliders()
    {
        GameObject.Destroy(GameObject.Find("Level_Wall_Left").gameObject);
        GameObject.Destroy(GameObject.Find("Level_Wall_Right").gameObject);
        GameObject.Destroy(GameObject.Find("Level_Ceiling").gameObject);
    }

    private void GetPrefabs()
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            switch (obj.name)
            {
                case "String":
                    RiftString.prefab = obj;
                    break;
                case "ElderKettle":
                    RiftManager.enemyPrefabElterKettle = obj;
                    break;
                case "Slime":
                    RiftManager.enemyPrefabSlime = obj;
                    break;
                case "Bird":
                    RiftManager.enemyPrefabBird = obj;
                    RiftEnemyBird.sprite3hp = obj.GetComponent<SpriteRenderer>().sprite;
                    break;
                case "BirdSprite2Hp":
                    RiftEnemyBird.sprite2hp = obj.GetComponent<SpriteRenderer>().sprite;
                    break;
                case "BirdSprite1Hp":
                    RiftEnemyBird.sprite1hp = obj.GetComponent<SpriteRenderer>().sprite;
                    break;
                case "Cupcake":
                    RiftManager.enemyPrefabCupcake = obj;
                    RiftEnemyCupcake.spriteMoving = obj.GetComponent<SpriteRenderer>().sprite;
                    break;
                case "CupcakeSpriteLanding":
                    RiftEnemyCupcake.spriteLanding = obj.GetComponent<SpriteRenderer>().sprite;
                    break;
                case "Theme":
                    obj.transform.parent = transform;
                    RiftManager.theme = obj.GetComponent<AudioSource>();
                    break;
            }
        }
    }

    public static RiftManager manager;
    public static RiftLevel instance;

    private static RiftProperties p = new();
}
