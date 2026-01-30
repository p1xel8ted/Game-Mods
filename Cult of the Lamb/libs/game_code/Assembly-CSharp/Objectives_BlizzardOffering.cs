// Decompiled with JetBrains decompiler
// Type: Objectives_BlizzardOffering
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_BlizzardOffering : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public int Target;
  [Key(17)]
  public InventoryItem.ITEM_TYPE TargetType;

  public override bool AutoTrack => true;

  public override Objectives.TIMER_TYPE TimerType => Objectives.TIMER_TYPE.Large;

  public override string Text
  {
    get
    {
      int num = 0;
      for (int index = 0; index < DataManager.Instance.BlizzardOfferingsGiven.Count; ++index)
      {
        if (DataManager.Instance.BlizzardOfferingsGiven[index].Type == this.TargetType)
          num = DataManager.Instance.BlizzardOfferingsGiven[index].Quantity;
      }
      return string.Format(LocalizationManager.GetTranslation("Objectives/Custom/BlizzardOffering"), (object) num, (object) this.Target, (object) FontImageNames.GetIconByType(this.TargetType));
    }
  }

  public Objectives_BlizzardOffering()
  {
  }

  public Objectives_BlizzardOffering(
    string groupId,
    int target,
    InventoryItem.ITEM_TYPE targetType,
    float questExpireDuration = -1f)
    : base(groupId, questExpireDuration)
  {
    this.Type = Objectives.TYPES.BLIZZARD_OFFERING;
    this.Target = target;
    this.TargetType = targetType;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    int num = 0;
    for (int index = 0; index < DataManager.Instance.BlizzardOfferingsGiven.Count; ++index)
    {
      if (DataManager.Instance.BlizzardOfferingsGiven[index].Type == this.TargetType)
        num = DataManager.Instance.BlizzardOfferingsGiven[index].Quantity;
    }
    Objectives_BlizzardOffering.FinalizedData_BlizzardOffering finalizedData = new Objectives_BlizzardOffering.FinalizedData_BlizzardOffering();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.Count = num;
    finalizedData.Target = this.Target;
    finalizedData.TargetType = this.TargetType;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public override bool CheckComplete()
  {
    for (int index = 0; index < DataManager.Instance.BlizzardOfferingsGiven.Count; ++index)
    {
      if (DataManager.Instance.BlizzardOfferingsGiven[index].Type == this.TargetType && DataManager.Instance.BlizzardOfferingsGiven[index].Quantity < this.Target)
        return false;
    }
    return true;
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_BlizzardOffering : ObjectivesDataFinalized
  {
    [Key(3)]
    public int Count;
    [Key(4)]
    public int Target;
    [Key(5)]
    public InventoryItem.ITEM_TYPE TargetType;

    public override string GetText()
    {
      string str1 = LocalizeIntegration.ReverseText(this.Count.ToString());
      string str2 = LocalizeIntegration.ReverseText(this.Target.ToString());
      return string.Format(LocalizationManager.GetTranslation("Objectives/Custom/BlizzardOffering"), (object) str1, (object) str2, (object) FontImageNames.GetIconByType(this.TargetType));
    }
  }
}
