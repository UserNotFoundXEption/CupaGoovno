using System.Collections;

namespace CupaGoovno;

public class MermaidMerdusa
{
    public void Init()
    {
        On.FlyingMermaidLevelMerdusa.zap_cr += zap_cr;
    }

    private IEnumerator zap_cr(On.FlyingMermaidLevelMerdusa.orig_zap_cr orig, FlyingMermaidLevelMerdusa self)
    {
        for(; ; )//new
        {//new
            AudioManager.Play("level_mermaid_merdusa_zap_loop_start");
            //self.animator.SetTrigger("Zap");
            self.animator.Play("Zap_Start");//new
            yield return self.animator.WaitForAnimationToEnd(self, "Zap_Start", false, true);
            self.laser.SetStoneTime(self.properties.CurrentState.zap.stoneTime);
            self.laser.animator.SetTrigger("Start");
            self.laser.transform.SetParent(null);
            AudioManager.PlayLoop("level_mermaid_merdusa_zap_loop");
            self.laser.StartLaser();
            yield return self.laser.animator.WaitForAnimationToEnd(self, "Lightning_Start", false, true);
            self.laser.animator.SetTrigger("End");
            AudioManager.Stop("level_mermaid_merdusa_zap_loop");
            AudioManager.Play("level_mermaid_merdusa_zap_loop_end");
            self.laser.StopLaser();
            self.animator.Play("Zap_End");
            self.animator.SetTrigger("Continue");
            yield return self.animator.WaitForAnimationToEnd(self, "Zap_End", false, true);
            yield return CupheadTime.WaitForSeconds(self, self.properties.CurrentState.zap.hesitateAfterAttack.RandomFloat());
        }//new
        //self.state = FlyingMermaidLevelMerdusa.State.Idle;
    }
}