// Decompiled with JetBrains decompiler
// Type: Objectives_ShootDummy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;

#nullable disable
[Serializable]
public class Objectives_ShootDummy : ObjectivesData
{
  private int Target = 5;

  public override bool AutoTrack => true;

  public override string Text
  {
    get
    {
      return string.Format(ScriptLocalization.Objectives_Custom.ShootCurse, (object) this.Target, (object) ((UnityEngine.Object) RatauGiveSpells.Instance != (UnityEngine.Object) null ? RatauGiveSpells.Instance.DummyCount : 0), (object) this.Target);
    }
  }

  public Objectives_ShootDummy()
  {
  }

  public Objectives_ShootDummy(string groupId)
    : base(groupId)
  {
    this.Type = Objectives.TYPES.SHOOT_DUMMIES;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_ShootDummy.FinalizedData_ShootDummy finalizedData = new Objectives_ShootDummy.FinalizedData_ShootDummy();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.Target = this.Target;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  protected override bool CheckComplete() => RatauGiveSpells.Instance.DummyCount >= this.Target;

  [Serializable]
  public class FinalizedData_ShootDummy : ObjectivesDataFinalized
  {
    public int Target;

    public override string GetText()
    {
      return string.Format(ScriptLocalization.Objectives_Custom.ShootCurse, (object) this.Target, (object) this.Target, (object) this.Target);
    }
  }
}
