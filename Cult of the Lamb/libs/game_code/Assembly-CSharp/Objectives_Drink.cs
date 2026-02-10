// Decompiled with JetBrains decompiler
// Type: Objectives_Drink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_Drink : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public InventoryItem.ITEM_TYPE DrinkType;
  [Key(17)]
  public int TargetFollower;
  [IgnoreMember]
  public bool complete;

  public override string Text
  {
    get
    {
      string name = FollowerInfo.GetInfoByID(this.TargetFollower, true)?.Name;
      string localizedName = CookingData.GetLocalizedName(this.DrinkType);
      return string.Format(LocalizationManager.GetTranslation("Objectives/Drink"), (object) name, (object) localizedName);
    }
  }

  public Objectives_Drink()
  {
  }

  public Objectives_Drink(
    string groupId,
    InventoryItem.ITEM_TYPE drinkType,
    float questExpireDuration = -1f)
    : base(groupId, questExpireDuration)
  {
    this.Type = Objectives.TYPES.DRINK;
    this.DrinkType = drinkType;
  }

  public override void Init(bool initialAssigning)
  {
    base.Init(initialAssigning);
    this.complete = false;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_Drink.FinalizedData_Drink finalizedData = new Objectives_Drink.FinalizedData_Drink();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.DrinkType = this.DrinkType;
    finalizedData.TargetFollowerName = FollowerInfo.GetInfoByID(this.TargetFollower, true)?.Name;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public override bool CheckComplete()
  {
    base.CheckComplete();
    return this.complete;
  }

  public override void Update()
  {
    base.Update();
    if (this.IsFailed || this.TargetFollower == -1 || FollowerInfo.GetInfoByID(this.TargetFollower) != null)
      return;
    this.Failed();
  }

  public void CheckComplete(InventoryItem.ITEM_TYPE drinkType, int targetFollowerID_1)
  {
    if (drinkType == this.DrinkType && this.TargetFollower == targetFollowerID_1)
      this.complete = true;
    else if (FollowerInfo.GetInfoByID(this.TargetFollower) == null)
      this.Failed();
    this.CheckComplete();
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_Drink : ObjectivesDataFinalized
  {
    [Key(3)]
    public InventoryItem.ITEM_TYPE DrinkType;
    [Key(4)]
    public string TargetFollowerName;

    public override string GetText()
    {
      string localizedName = CookingData.GetLocalizedName(this.DrinkType);
      return string.Format(LocalizationManager.GetTranslation("Objectives/Drink"), (object) this.TargetFollowerName, (object) localizedName);
    }
  }
}
