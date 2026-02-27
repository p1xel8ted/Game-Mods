// Decompiled with JetBrains decompiler
// Type: I2.Loc.ToggleLanguage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace I2.Loc;

public class ToggleLanguage : MonoBehaviour
{
  private void Start() => this.Invoke("test", 3f);

  private void test()
  {
    List<string> allLanguages = LocalizationManager.GetAllLanguages();
    int num1 = allLanguages.IndexOf(LocalizationManager.CurrentLanguage);
    int num2 = num1 >= 0 ? (num1 + 1) % allLanguages.Count : 0;
    this.Invoke(nameof (test), 3f);
  }
}
