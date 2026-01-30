// Decompiled with JetBrains decompiler
// Type: I2.Loc.SetLanguage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace I2.Loc;

[AddComponentMenu("I2/Localization/SetLanguage Button")]
public class SetLanguage : MonoBehaviour
{
  public string _Language;

  public void OnClick() => this.ApplyLanguage();

  public void ApplyLanguage()
  {
    if (!LocalizationManager.HasLanguage(this._Language))
      return;
    LocalizationManager.CurrentLanguage = this._Language;
  }
}
