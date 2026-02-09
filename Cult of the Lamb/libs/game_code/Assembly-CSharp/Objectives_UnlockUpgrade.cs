// Decompiled with JetBrains decompiler
// Type: Objectives_UnlockUpgrade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_UnlockUpgrade : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public UpgradeSystem.Type UnlockType;

  public override string Text
  {
    get => LocalizationManager.GetTranslation($"Objectives/Unlock/{this.UnlockType.ToString()}");
  }

  public Objectives_UnlockUpgrade()
  {
  }

  public Objectives_UnlockUpgrade(
    string groupId,
    UpgradeSystem.Type unlockType,
    float expireTimestamp = -1f)
    : base(groupId, expireTimestamp)
  {
    this.Type = Objectives.TYPES.UNLOCK;
    this.UnlockType = unlockType;
    UpgradeSystem.OnUpgradeUnlocked += new UpgradeSystem.UnlockEvent(this.UpgradeSystem_OnUpgradeUnlocked);
  }

  public override void Init(bool initialAssigning)
  {
    UpgradeSystem.OnUpgradeUnlocked -= new UpgradeSystem.UnlockEvent(this.UpgradeSystem_OnUpgradeUnlocked);
    UpgradeSystem.OnUpgradeUnlocked += new UpgradeSystem.UnlockEvent(this.UpgradeSystem_OnUpgradeUnlocked);
    base.Init(initialAssigning);
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_UnlockUpgrade.FinalizedData_UnlockUpgrade finalizedData = new Objectives_UnlockUpgrade.FinalizedData_UnlockUpgrade();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.UnlockType = this.UnlockType;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public void UpgradeSystem_OnUpgradeUnlocked(UpgradeSystem.Type upgradeType)
  {
    if (upgradeType != this.UnlockType)
      return;
    ObjectiveManager.UpdateObjective((ObjectivesData) this);
    UpgradeSystem.OnUpgradeUnlocked -= new UpgradeSystem.UnlockEvent(this.UpgradeSystem_OnUpgradeUnlocked);
  }

  public override bool CheckComplete() => UpgradeSystem.GetUnlocked(this.UnlockType);

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_UnlockUpgrade : ObjectivesDataFinalized
  {
    [Key(3)]
    public UpgradeSystem.Type UnlockType;

    public override string GetText()
    {
      return LocalizationManager.GetTranslation($"Objectives/Unlock/{this.UnlockType.ToString()}");
    }
  }
}
