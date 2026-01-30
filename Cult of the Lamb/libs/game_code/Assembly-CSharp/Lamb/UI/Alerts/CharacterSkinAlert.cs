// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Alerts.CharacterSkinAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Lamb.UI.Alerts;

public class CharacterSkinAlert : AlertBadge<string>
{
  public override AlertCategory<string> _source
  {
    get => (AlertCategory<string>) DataManager.Instance.Alerts.CharacterSkinAlerts;
  }
}
