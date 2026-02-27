// Decompiled with JetBrains decompiler
// Type: Objectives_KillEnemies
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;

#nullable disable
[Serializable]
public class Objectives_KillEnemies : ObjectivesData
{
  public Enemy enemyType;
  public int enemiesKilledBeforeObjectiveBegan = -1;
  public int enemiesKilledRequired;

  private int enemiesKilled
  {
    get
    {
      return DataManager.Instance.GetEnemiesKilled(this.enemyType) - this.enemiesKilledBeforeObjectiveBegan;
    }
  }

  public override string Text
  {
    get
    {
      return string.Format(ScriptLocalization.Objectives.KillEnemies, (object) UnitObject.GetLocalisedEnemyName(this.enemyType), (object) this.enemiesKilled, (object) this.enemiesKilledRequired);
    }
  }

  public Objectives_KillEnemies()
  {
  }

  public Objectives_KillEnemies(
    string groupId,
    Enemy enemyType,
    int killsRequired,
    float questDuration)
    : base(groupId, questDuration)
  {
    this.Type = Objectives.TYPES.KILL_ENEMIES;
    this.enemyType = enemyType;
    this.enemiesKilledRequired = killsRequired;
  }

  public override void Init(bool initialAssigning)
  {
    base.Init(initialAssigning);
    if (this.enemiesKilledBeforeObjectiveBegan != -1)
      return;
    this.enemiesKilledBeforeObjectiveBegan = DataManager.Instance.GetEnemiesKilled(this.enemyType);
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_KillEnemies.FinalizedData_KillEnemies finalizedData = new Objectives_KillEnemies.FinalizedData_KillEnemies();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.EnemyType = this.enemyType;
    finalizedData.EnemiesKilledRequired = this.enemiesKilledRequired;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  protected override bool CheckComplete()
  {
    return base.CheckComplete() && this.enemiesKilled >= this.enemiesKilledRequired;
  }

  [Serializable]
  public class FinalizedData_KillEnemies : ObjectivesDataFinalized
  {
    public Enemy EnemyType;
    public int EnemiesKilledRequired;

    public override string GetText()
    {
      return string.Format(ScriptLocalization.Objectives.KillEnemies, (object) UnitObject.GetLocalisedEnemyName(this.EnemyType), (object) this.EnemiesKilledRequired, (object) this.EnemiesKilledRequired);
    }
  }
}
