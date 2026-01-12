using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public class Mouse
{
    public void Init()
    {
        new MouseCan().Init();
        new MouseCherryBomb().Init();
        new MouseGhost().Init();
        new MouseCat().Init();
        new MouseSawBladeManager().Init();
    }
}
