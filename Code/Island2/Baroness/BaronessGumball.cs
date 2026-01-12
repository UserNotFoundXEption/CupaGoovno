using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class BaronessGumball
{
    public void Init()
    {
        On.BaronessLevelGumball.leaving_castle_cr += leaving_castle_cr;
        On.BaronessLevelGumball.death_cr += death_cr;
    }

    protected virtual IEnumerator leaving_castle_cr(On.BaronessLevelGumball.orig_leaving_castle_cr orig, BaronessLevelGumball self)
    {
        yield return orig(self);
        gumball = self;
        BaronessPlatform.controlledByGumball = true;
    }

    private IEnumerator death_cr(On.BaronessLevelGumball.orig_death_cr orig, BaronessLevelGumball self)
    {
        BaronessPlatform.controlledByGumball = false;
        yield return orig(self);
    }

    public static BaronessLevelGumball gumball;
}
