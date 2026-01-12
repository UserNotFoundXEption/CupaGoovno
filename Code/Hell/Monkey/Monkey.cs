using System.Collections;

namespace CupaGoovno;

public class Monkey
{
    public void Init()
    {
        new MonkeyGameManager().Init();
        new MonkeyMusicNote().Init();
        On.DicePalaceFlyingMemoryLevelStuffedToy.punishment_cr += punishment_cr;
        On.DicePalaceFlyingMemoryLevelStuffedToy.move_cr += move_cr;
    }

    private IEnumerator punishment_cr(On.DicePalaceFlyingMemoryLevelStuffedToy.orig_punishment_cr orig, DicePalaceFlyingMemoryLevelStuffedToy self)
    {
        YoMamaFat.mermaidLaser.transform.SetPosition(200f, 0f);//new start
        YoMamaFat.mermaidLaser.transform.SetScale(1.5f, 1.5f);
        YoMamaFat.mermaidLaser.SetStoneTime(2f);
        YoMamaFat.mermaidLaser.animator.SetTrigger("Start");
        YoMamaFat.mermaidLaser.StartLaser();
        yield return YoMamaFat.mermaidLaser.animator.WaitForAnimationToEnd(YoMamaFat.mermaidLaser, "Lightning_Start", false, true);
        yield return CupheadTime.WaitForSeconds(self, 1f);
        YoMamaFat.mermaidLaser.StopLaser();//new end
        /*self.timer = 0f;
        LevelProperties.DicePalaceFlyingMemory.StuffedToy p = base.properties.CurrentState.stuffedToy;
        bool speedUp = true;
        self.startedPunishment = true;
        base.animator.SetTrigger("OnNoMatch");
        AudioManager.PlayLoop("dice_palace_memory_monkey_shake");
        self.emitAudioFromObject.Add("dice_palace_memory_monkey_shake");
        while (speedUp)
        {
            if (self.speed >= p.punishSpeed)
            {
                speedUp = false;
                break;
            }
            self.speed += p.incrementSpeedBy;
            yield return null;
        }
        self.speed = p.punishSpeed;
        while (self.timer < p.punishTime && self.state == DicePalaceFlyingMemoryLevelStuffedToy.State.Closed)
        {
            self.timer += CupheadTime.Delta;
            yield return null;
        }
        base.animator.SetTrigger("Continue");
        while (self.speed > p.bounceSpeed)
        {
            self.speed -= p.incrementSpeedBy;
            yield return null;
        }
        AudioManager.Stop("dice_palace_memory_monkey_shake");
        self.speed = p.bounceSpeed;
        self.startedPunishment = false;
        self.SFXAllowAnticipation();
        yield return null;
        yield break;*/
    }

    protected IEnumerator move_cr(On.DicePalaceFlyingMemoryLevelStuffedToy.orig_move_cr orig, DicePalaceFlyingMemoryLevelStuffedToy self)
    {
        self.Open();
        yield return orig(self);
    }
}
