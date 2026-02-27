// Decompiled with JetBrains decompiler
// Type: Archer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Archer : FormationFighter
{
  public GameObject Arrow;
  public float MinimumRange = 4f;
  private Task_Archer ArcherTask;

  public override void SetTask()
  {
    this.AddNewTask(Task_Type.ARCHER, false);
    this.ArcherTask = this.CurrentTask as Task_Archer;
    this.ArcherTask.Init(this.DetectEnemyRange, this.AttackRange, this.LoseEnemyRange, this.PreAttackDuration, this.PostAttackDuration, this.MinimumRange, this.Arrow);
    this.BreakAttacksOnHit = true;
  }

  public override void OnDisable()
  {
    base.OnDisable();
    if (this.ArcherTask == null)
      return;
    this.ArcherTask.ClearTarget();
  }
}
