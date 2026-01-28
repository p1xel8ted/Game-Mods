// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.UpgradeAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Lamb.UI.Alerts;

public class UpgradeAlert : AlertBadge<UpgradeSystem.Type>
{
  public override AlertCategory<UpgradeSystem.Type> _source
  {
    get => (AlertCategory<UpgradeSystem.Type>) DataManager.Instance.Alerts.Upgrades;
  }
}
