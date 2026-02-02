// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.DoctrineAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Lamb.UI.Alerts;

public class DoctrineAlert : AlertBadge<DoctrineUpgradeSystem.DoctrineType>
{
  public override AlertCategory<DoctrineUpgradeSystem.DoctrineType> _source
  {
    get => (AlertCategory<DoctrineUpgradeSystem.DoctrineType>) DataManager.Instance.Alerts.Doctrine;
  }
}
