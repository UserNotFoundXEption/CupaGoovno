using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class WeaponPeeShooterExProjectile : BasicProjectile
{
    public override float DestroyLifetime => CustomWeaponProperties.Peeshooter.Ex.destroyLifetime;
}
