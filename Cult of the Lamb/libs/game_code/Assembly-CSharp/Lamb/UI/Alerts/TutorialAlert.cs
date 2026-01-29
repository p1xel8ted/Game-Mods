// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.TutorialAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Lamb.UI.Alerts;

public class TutorialAlert : AlertBadge<TutorialTopic>
{
  public override AlertCategory<TutorialTopic> _source
  {
    get => (AlertCategory<TutorialTopic>) DataManager.Instance.Alerts.Tutorial;
  }
}
