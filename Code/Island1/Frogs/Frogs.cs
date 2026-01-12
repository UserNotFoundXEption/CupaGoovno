using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class Frogs
{
    public void Init()
    {
        new FrogTall().Init();
        new FrogShort().Init();
        new FrogMorphed().Init();
        new FrogClapBullet().Init();
        new FrogTigerBullet().Init();
    }
}
