using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CupaGoovno;

public class PirateBoatBeam
{
    public static PirateLevelBoatBeam Create(PirateLevelBoatBeam self, Transform parent, float rotation)//new
    {
        PirateLevelBoatBeam pirateLevelBoatBeam = self.InstantiatePrefab<PirateLevelBoatBeam>();
        Init(pirateLevelBoatBeam, parent, rotation);
        return pirateLevelBoatBeam;
    }

    private static void Init(PirateLevelBoatBeam self, Transform parent, float rotation)//new
    {
        AudioManager.Play("level_pirate_ship_beam_fire");
        self.transform.SetParent(parent);
        self.transform.ResetLocalPosition();
        self.transform.ResetLocalRotation();
        self.transform.SetEulerAngles(null, null, rotation);
    }
}
