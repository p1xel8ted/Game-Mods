// Decompiled with JetBrains decompiler
// Type: I2.Loc.CallbackNotification
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace I2.Loc;

public class CallbackNotification : MonoBehaviour
{
  public void OnModifyLocalization()
  {
    if (string.IsNullOrEmpty(Localize.MainTranslation))
      return;
    string translation = LocalizationManager.GetTranslation("Color/Red");
    Localize.MainTranslation = Localize.MainTranslation.Replace("{PLAYER_COLOR}", translation);
  }
}
