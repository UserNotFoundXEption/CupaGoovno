using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class Baroness
{
    public void Init()
    {
        new BaronessJellyBeans().Init();
        new BaronessCastle().Init();
        new BaronessPlatform().Init();
        new BaronessWaffle().Init();
        new BaronessPeppermint().Init();
        new BaronessCupcake().Init();
        new BaronessGumball().Init();
        new BaronessJawbreaker().Init();
    }
}
