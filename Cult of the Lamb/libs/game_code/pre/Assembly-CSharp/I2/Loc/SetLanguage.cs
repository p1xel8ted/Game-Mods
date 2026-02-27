// Decompiled with JetBrains decompiler
// Type: I2.Loc.SetLanguage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace I2.Loc;

[AddComponentMenu("I2/Localization/SetLanguage Button")]
public class SetLanguage : MonoBehaviour
{
  public string _Language;

  private void OnClick() => this.ApplyLanguage();

  public void ApplyLanguage()
  {
    if (!LocalizationManager.HasLanguage(this._Language))
      return;
    LocalizationManager.CurrentLanguage = this._Language;
  }
}
