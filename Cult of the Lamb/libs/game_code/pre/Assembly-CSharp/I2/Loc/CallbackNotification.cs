// Decompiled with JetBrains decompiler
// Type: I2.Loc.CallbackNotification
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
