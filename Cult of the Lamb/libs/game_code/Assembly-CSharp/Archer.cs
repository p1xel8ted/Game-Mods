// Decompiled with JetBrains decompiler
// Type: Archer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Archer : FormationFighter
{
  public GameObject Arrow;
  public float MinimumRange = 4f;
  public Task_Archer ArcherTask;

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
