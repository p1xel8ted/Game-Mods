// Decompiled with JetBrains decompiler
// Type: I2.Loc.Example_ChangeLanguage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace I2.Loc;

public class Example_ChangeLanguage : MonoBehaviour
{
  public void SetLanguage_English() => this.SetLanguage("English");

  public void SetLanguage_French() => this.SetLanguage("French");

  public void SetLanguage_Spanish() => this.SetLanguage("Spanish");

  public void SetLanguage(string LangName)
  {
    if (!LocalizationManager.HasLanguage(LangName))
      return;
    LocalizationManager.CurrentLanguage = LangName;
  }
}
