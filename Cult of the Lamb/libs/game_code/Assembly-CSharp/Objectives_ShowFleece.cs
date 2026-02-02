// Decompiled with JetBrains decompiler
// Type: Objectives_ShowFleece
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_ShowFleece : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public PlayerFleeceManager.FleeceType FleeceType;
  [IgnoreMember]
  public bool complete;

  public override string Text
  {
    get
    {
      string str = $"TarotCards/{this.FleeceType}/Name".Localized();
      return string.Format("Objectives/ShowFleece".Localized(), (object) str);
    }
  }

  public Objectives_ShowFleece()
  {
  }

  public Objectives_ShowFleece(
    string groupId,
    PlayerFleeceManager.FleeceType fleeceType,
    float questExpireDuration = -1f)
    : base(groupId, questExpireDuration)
  {
    this.Type = Objectives.TYPES.SHOW_FLEECE;
    this.FleeceType = fleeceType;
  }

  public override void Init(bool initialAssigning)
  {
    base.Init(initialAssigning);
    this.complete = false;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_ShowFleece.FinalizedData_ShowFleece finalizedData = new Objectives_ShowFleece.FinalizedData_ShowFleece();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.FleeceType = this.FleeceType;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public override bool CheckComplete()
  {
    if (this.IsComplete)
      return true;
    if (DataManager.Instance == null)
      return false;
    return (PlayerFleeceManager.FleeceType) DataManager.Instance.PlayerFleece == this.FleeceType || (PlayerFleeceManager.FleeceType) DataManager.Instance.PlayerVisualFleece == this.FleeceType;
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_ShowFleece : ObjectivesDataFinalized
  {
    [Key(3)]
    public PlayerFleeceManager.FleeceType FleeceType;

    public override string GetText()
    {
      string str = $"TarotCards/{this.FleeceType}/Name".Localized();
      return string.Format("Objectives/ShowFleece".Localized(), (object) str);
    }
  }
}
