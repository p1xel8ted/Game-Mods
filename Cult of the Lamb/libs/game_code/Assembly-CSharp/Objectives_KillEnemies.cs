// Decompiled with JetBrains decompiler
// Type: Objectives_KillEnemies
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_KillEnemies : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public Enemy enemyType;
  [Key(17)]
  public int enemiesKilledBeforeObjectiveBegan = -1;
  [Key(18)]
  public int enemiesKilledRequired;

  [IgnoreMember]
  public int enemiesKilled
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

  public override bool CheckComplete()
  {
    return base.CheckComplete() && this.enemiesKilled >= this.enemiesKilledRequired;
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_KillEnemies : ObjectivesDataFinalized
  {
    [Key(3)]
    public Enemy EnemyType;
    [Key(4)]
    public int EnemiesKilledRequired;

    public override string GetText()
    {
      string str = LocalizeIntegration.ReverseText(this.EnemiesKilledRequired.ToString());
      return string.Format(ScriptLocalization.Objectives.KillEnemies, (object) UnitObject.GetLocalisedEnemyName(this.EnemyType), (object) str, (object) str);
    }
  }
}
