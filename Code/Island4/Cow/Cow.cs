using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class Cow
{
    public void Init()
    {
        On.FlyingCowboyLevelCowboy.SpawnUFOs += SpawnUFOs;
    }

    private void SpawnUFOs(On.FlyingCowboyLevelCowboy.orig_SpawnUFOs orig, FlyingCowboyLevelCowboy self)
    {
        LevelProperties.FlyingCowboy.UFOEnemy uFOEnemy = self.properties.CurrentState.uFOEnemy;
        Vector3 pos = new Vector3(0f, uFOEnemy.topUFOVerticalPosition); //new
        //		Vector3 pos = new Vector3(740f, uFOEnemy.topUFOVerticalPosition);
        self.ufo = self.ufoPrefab.Spawn<FlyingCowboyLevelUFO>();
        self.ufo.Init(pos, self.properties.CurrentState.uFOEnemy, uFOEnemy.UFOHealth);
    }
}
