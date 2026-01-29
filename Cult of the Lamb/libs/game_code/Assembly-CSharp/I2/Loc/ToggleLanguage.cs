// Decompiled with JetBrains decompiler
// Type: I2.Loc.ToggleLanguage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace I2.Loc;

public class ToggleLanguage : MonoBehaviour
{
  public void Start() => this.Invoke("test", 3f);

  public void test()
  {
    List<string> allLanguages = LocalizationManager.GetAllLanguages();
    int num1 = allLanguages.IndexOf(LocalizationManager.CurrentLanguage);
    int num2 = num1 >= 0 ? (num1 + 1) % allLanguages.Count : 0;
    this.Invoke(nameof (test), 3f);
  }
}
