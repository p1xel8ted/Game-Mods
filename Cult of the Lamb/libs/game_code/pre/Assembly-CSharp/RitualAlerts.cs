// Decompiled with JetBrains decompiler
// Type: RitualAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class RitualAlerts : AlertCategory<UpgradeSystem.Type>
{
  private UpgradeSystem.Type[] _allRituals;

  public RitualAlerts()
  {
    this._allRituals = UpgradeSystem.AllRituals();
    UpgradeSystem.OnAbilityUnlocked += new Action<UpgradeSystem.Type>(this.OnRitualAdded);
    UpgradeSystem.OnAbilityLocked += new Action<UpgradeSystem.Type>(this.OnRitualRemoved);
  }

  ~RitualAlerts()
  {
    this._allRituals = (UpgradeSystem.Type[]) null;
    UpgradeSystem.OnAbilityUnlocked -= new Action<UpgradeSystem.Type>(this.OnRitualAdded);
    UpgradeSystem.OnAbilityLocked -= new Action<UpgradeSystem.Type>(this.OnRitualRemoved);
  }

  private void OnRitualAdded(UpgradeSystem.Type upgradeType)
  {
    if (!this._allRituals.Contains<UpgradeSystem.Type>(upgradeType))
      return;
    this.AddOnce(upgradeType);
  }

  private void OnRitualRemoved(UpgradeSystem.Type upgradeType) => this.Remove(upgradeType);
}
