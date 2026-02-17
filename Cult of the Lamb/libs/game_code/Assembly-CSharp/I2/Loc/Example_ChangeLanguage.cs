// Decompiled with JetBrains decompiler
// Type: I2.Loc.Example_ChangeLanguage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
