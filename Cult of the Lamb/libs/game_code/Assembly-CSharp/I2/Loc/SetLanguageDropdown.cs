// Decompiled with JetBrains decompiler
// Type: I2.Loc.SetLanguageDropdown
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace I2.Loc;

[AddComponentMenu("I2/Localization/SetLanguage Dropdown")]
public class SetLanguageDropdown : MonoBehaviour
{
  public void OnEnable()
  {
    Dropdown component = this.GetComponent<Dropdown>();
    if ((Object) component == (Object) null)
      return;
    string currentLanguage = LocalizationManager.CurrentLanguage;
    if (LocalizationManager.Sources.Count == 0)
      LocalizationManager.UpdateSources();
    List<string> allLanguages = LocalizationManager.GetAllLanguages();
    component.ClearOptions();
    component.AddOptions(allLanguages);
    component.value = allLanguages.IndexOf(currentLanguage);
    component.onValueChanged.RemoveListener(new UnityAction<int>(this.OnValueChanged));
    component.onValueChanged.AddListener(new UnityAction<int>(this.OnValueChanged));
  }

  public void OnValueChanged(int index)
  {
    Dropdown component = this.GetComponent<Dropdown>();
    if (index < 0)
    {
      index = 0;
      component.value = index;
    }
    LocalizationManager.CurrentLanguage = component.options[index].text;
  }
}
