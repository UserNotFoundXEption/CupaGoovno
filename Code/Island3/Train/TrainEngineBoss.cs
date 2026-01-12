using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class TrainEngineBoss
{
    public void Init()
    {
        On.TrainLevelEngineBoss.OnAttackAnimComplete += OnAttackAnimComplete;
    }

    private void OnAttackAnimComplete(On.TrainLevelEngineBoss.orig_OnAttackAnimComplete orig, TrainLevelEngineBoss self)
    {/*
        self.dropperPrefab.Create(self.dropperRoot.position, self.properties.CurrentState.engine.projectileUpSpeed, self.properties.CurrentState.engine.projectileXSpeed, self.properties.CurrentState.engine.projectileGravity);*/
    }
}
