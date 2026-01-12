using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CupaGoovno;

public interface IMoaiLearnerDiscrete
{
    public void SaveHealthInfo();
    public float GetReward(bool heart);
    public void Update(MoaiAttacks attacks, bool heart);

    public float[][] table {  get; set; }
    public float bossHp { get; set; }
    public float playerHp { get; set; }
}
