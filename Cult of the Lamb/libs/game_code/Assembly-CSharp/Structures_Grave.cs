// Decompiled with JetBrains decompiler
// Type: Structures_Grave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_Grave : StructureBrain
{
  public const float CHANCE_OF_SPAWNING_ZOMBIE = 0.05f;
  public bool UpgradedGrave;

  public override int SoulMax => this.UpgradedGrave ? 15 : 10;

  public float TimeBetweenSouls => this.UpgradedGrave ? 600f : 360f;

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public override void OnNewPhaseStarted()
  {
  }

  public void SpawnZombie(FollowerInfo deadBody)
  {
    FollowerBrain brain = FollowerBrain.GetOrCreateBrain(deadBody);
    brain.ResetStats();
    brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    brain.Location = FollowerLocation.Base;
    brain.DesiredLocation = FollowerLocation.Base;
    brain.CurrentTask.Arrive();
    brain.ApplyCurseState(Thought.Zombie);
    brain.LastPosition = this.Data.Position;
    brain.HardSwapToTask((FollowerTask) new FollowerTask_Zombie());
    FollowerManager.CreateNewFollower(brain._directInfoAccess, this.Data.Position);
  }

  public Grave GetGrave(int ID)
  {
    foreach (Grave grave in Grave.Graves)
    {
      if (grave.StructureInfo.ID == ID)
        return grave;
    }
    return (Grave) null;
  }
}
