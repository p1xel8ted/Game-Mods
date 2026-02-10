// Decompiled with JetBrains decompiler
// Type: src.UI.Alerts.AchievementAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Alerts;
using src.Managers;

#nullable disable
namespace src.UI.Alerts;

public class AchievementAlert : AlertBadge<string>
{
  public override AlertCategory<string> _source
  {
    get => (AlertCategory<string>) PersistenceManager.PersistentData.AchievementAlerts;
  }
}
