// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.TutorialAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
