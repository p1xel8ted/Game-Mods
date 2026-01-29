// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizeDropdown
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace I2.Loc;

[AddComponentMenu("I2/Localization/Localize Dropdown")]
public class LocalizeDropdown : MonoBehaviour
{
  public List<string> _Terms = new List<string>();

  public void Start()
  {
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.OnLocalize);
    this.OnLocalize();
  }

  public void OnDestroy()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.OnLocalize);
  }

  public void OnEnable()
  {
    if (this._Terms.Count == 0)
      this.FillValues();
    this.OnLocalize();
  }

  public void OnLocalize()
  {
    if (!this.enabled || (Object) this.gameObject == (Object) null || !this.gameObject.activeInHierarchy || string.IsNullOrEmpty(LocalizationManager.CurrentLanguage))
      return;
    this.UpdateLocalization();
  }

  public void FillValues()
  {
    Dropdown component = this.GetComponent<Dropdown>();
    if ((Object) component == (Object) null && I2Utils.IsPlaying())
    {
      this.FillValuesTMPro();
    }
    else
    {
      foreach (Dropdown.OptionData option in component.options)
        this._Terms.Add(option.text);
    }
  }

  public void UpdateLocalization()
  {
    Dropdown component = this.GetComponent<Dropdown>();
    if ((Object) component == (Object) null)
    {
      this.UpdateLocalizationTMPro();
    }
    else
    {
      component.options.Clear();
      foreach (string term in this._Terms)
      {
        string translation = LocalizationManager.GetTranslation(term);
        component.options.Add(new Dropdown.OptionData(translation));
      }
      component.RefreshShownValue();
    }
  }

  public void UpdateLocalizationTMPro()
  {
    TMP_Dropdown component = this.GetComponent<TMP_Dropdown>();
    if ((Object) component == (Object) null)
      return;
    component.options.Clear();
    foreach (string term in this._Terms)
    {
      string translation = LocalizationManager.GetTranslation(term);
      component.options.Add(new TMP_Dropdown.OptionData(translation));
    }
    component.RefreshShownValue();
  }

  public void FillValuesTMPro()
  {
    TMP_Dropdown component = this.GetComponent<TMP_Dropdown>();
    if ((Object) component == (Object) null)
      return;
    foreach (TMP_Dropdown.OptionData option in component.options)
      this._Terms.Add(option.text);
  }
}
