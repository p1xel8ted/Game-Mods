// Decompiled with JetBrains decompiler
// Type: Objectives_ShootDummy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_ShootDummy : ObjectivesData
{
  [IgnoreMember]
  public int Target = 5;

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

  public override bool CheckComplete() => RatauGiveSpells.Instance.DummyCount >= this.Target;

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_ShootDummy : ObjectivesDataFinalized
  {
    [Key(3)]
    public int Target;

    public override string GetText()
    {
      return string.Format(ScriptLocalization.Objectives_Custom.ShootCurse, (object) this.Target, (object) this.Target, (object) this.Target);
    }
  }
}
