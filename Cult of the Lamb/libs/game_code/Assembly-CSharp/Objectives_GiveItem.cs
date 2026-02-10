// Decompiled with JetBrains decompiler
// Type: Objectives_GiveItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_GiveItem : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public string NPCTerm;
  [Key(17)]
  public int Quantity;
  [Key(18)]
  public int Target;
  [Key(19)]
  public InventoryItem.ITEM_TYPE TargetType;
  [Key(20)]
  public FollowerLocation Location = FollowerLocation.None;

  public override string Text
  {
    get
    {
      return this.Location != FollowerLocation.None ? string.Format(LocalizationManager.GetTranslation("Objectives/Custom/GiveItemWithLocation"), (object) LocalizationManager.GetTranslation(this.NPCTerm), (object) FontImageNames.GetIconByType(this.TargetType), (object) LocalizationManager.GetTranslation($"NAMES/Places/{this.Location}"), (object) this.Quantity, (object) this.Target) : string.Format(LocalizationManager.GetTranslation("Objectives/Custom/GiveItem"), (object) LocalizationManager.GetTranslation(this.NPCTerm), (object) FontImageNames.GetIconByType(this.TargetType), (object) this.Quantity, (object) this.Target);
    }
  }

  public Objectives_GiveItem()
  {
  }

  public Objectives_GiveItem(
    string groupId,
    string npcTerm,
    int target,
    InventoryItem.ITEM_TYPE targetType,
    float questExpireDuration = -1f,
    FollowerLocation location = FollowerLocation.None)
    : base(groupId, questExpireDuration)
  {
    this.Type = Objectives.TYPES.GIVE_ITEM;
    this.Target = target;
    this.TargetType = targetType;
    this.NPCTerm = npcTerm;
    this.Location = location;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_GiveItem.FinalizedData_GiveItem finalizedData = new Objectives_GiveItem.FinalizedData_GiveItem();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.NPCTerm = this.NPCTerm;
    finalizedData.Count = this.Quantity;
    finalizedData.Target = this.Target;
    finalizedData.TargetType = this.TargetType;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    finalizedData.Location = this.Location;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public override bool CheckComplete() => this.Quantity >= this.Target;

  public void AddItem(InventoryItem.ITEM_TYPE itemType)
  {
    if (this.TargetType != itemType)
      return;
    ++this.Quantity;
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_GiveItem : ObjectivesDataFinalized
  {
    [Key(3)]
    public string NPCTerm;
    [Key(4)]
    public int Count;
    [Key(5)]
    public int Target;
    [Key(6)]
    public InventoryItem.ITEM_TYPE TargetType;
    [Key(7)]
    public FollowerLocation Location = FollowerLocation.None;

    public override string GetText()
    {
      string str1 = LocalizeIntegration.ReverseText(this.Count.ToString());
      string str2 = LocalizeIntegration.ReverseText(this.Target.ToString());
      return this.Location != FollowerLocation.None ? string.Format(LocalizationManager.GetTranslation("Objectives/Custom/GiveItemWithLocation"), (object) LocalizationManager.GetTranslation(this.NPCTerm), (object) FontImageNames.GetIconByType(this.TargetType), (object) LocalizationManager.GetTranslation($"NAMES/Places/{this.Location}"), (object) str1, (object) str2) : string.Format(LocalizationManager.GetTranslation("Objectives/Custom/GiveItem"), (object) LocalizationManager.GetTranslation(this.NPCTerm), (object) FontImageNames.GetIconByType(this.TargetType), (object) str1, (object) str2);
    }
  }
}
