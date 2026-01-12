using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class MonkeyMusicNote
{
    public void Init()
    {
        On.DicePalaceFlyingMemoryMusicNote.death_timer_cr += death_timer_cr;
    }

    private IEnumerator death_timer_cr(On.DicePalaceFlyingMemoryMusicNote.orig_death_timer_cr orig, DicePalaceFlyingMemoryMusicNote self)
    {
        for(; ; )
        {
            self.lifetime = 5f;
            yield return CupheadTime.WaitForSeconds(self, 3f);
        }
    }
}
