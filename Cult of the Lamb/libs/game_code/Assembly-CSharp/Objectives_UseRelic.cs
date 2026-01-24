// Decompiled with JetBrains decompiler
// Type: Objectives_UseRelic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_UseRelic : ObjectivesData
{
  public override bool AutoTrack => true;

  public override string Text => ScriptLocalization.Objectives_Custom.UseRelic;

  public Objectives_UseRelic()
  {
  }

  public Objectives_UseRelic(string groupId)
    : base(groupId)
  {
    this.Type = Objectives.TYPES.USE_RELIC;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_UseRelic.FinalizedData_UseRelic finalizedData = new Objectives_UseRelic.FinalizedData_UseRelic();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.Target = (UnityEngine.Object) RelicRoomManager.Instance != (UnityEngine.Object) null ? RelicRoomManager.Instance.RelicTargetCount : 1;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public override bool CheckComplete()
  {
    return !((UnityEngine.Object) RelicRoomManager.Instance != (UnityEngine.Object) null) || RelicRoomManager.Instance.RelicUsedCount >= RelicRoomManager.Instance.RelicTargetCount;
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_UseRelic : ObjectivesDataFinalized
  {
    [Key(3)]
    public int Target;

    public override string GetText() => ScriptLocalization.Objectives_Custom.UseRelic;
  }
}
