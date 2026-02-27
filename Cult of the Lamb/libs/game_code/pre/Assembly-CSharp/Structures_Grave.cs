// Decompiled with JetBrains decompiler
// Type: Structures_Grave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Structures_Grave : StructureBrain
{
  private const float CHANCE_OF_SPAWNING_ZOMBIE = 0.05f;
  public bool UpgradedGrave;
  public Action<int> OnSoulsGained;

  public int SoulMax => this.UpgradedGrave ? 15 : 10;

  public float TimeBetweenSouls => this.UpgradedGrave ? 600f : 360f;

  public int SoulCount
  {
    get => this.Data.SoulCount;
    set
    {
      int soulCount = this.SoulCount;
      this.Data.SoulCount = Mathf.Clamp(value, 0, this.SoulMax);
      if (this.SoulCount <= soulCount)
        return;
      Action<int> onSoulsGained = this.OnSoulsGained;
      if (onSoulsGained == null)
        return;
      onSoulsGained(this.SoulCount - soulCount);
    }
  }

  public FollowerTask GetOverrideTask(FollowerBrain brain) => (FollowerTask) null;

  public bool CheckOverrideComplete() => true;

  public override void OnAdded() => TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhase);

  public override void OnRemoved() => TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhase);

  private void OnNewPhase()
  {
  }

  private void SpawnZombie(FollowerInfo deadBody)
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

  private Grave GetGrave(int ID)
  {
    foreach (Grave grave in Grave.Graves)
    {
      if (grave.StructureInfo.ID == ID)
        return grave;
    }
    return (Grave) null;
  }
}
