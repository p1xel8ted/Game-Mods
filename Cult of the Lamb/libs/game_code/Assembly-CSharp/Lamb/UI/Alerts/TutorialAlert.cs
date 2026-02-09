// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.TutorialAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
