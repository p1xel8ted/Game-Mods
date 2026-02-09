// Decompiled with JetBrains decompiler
// Type: CustomFontSettings
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CustomFontSettings : MonoBehaviour
{
  public List<CustomFontSettings.CustomParameterSetting> pars = new List<CustomFontSettings.CustomParameterSetting>();
  public List<CustomFontSettings.CustomParameterSetting> _original_pars;

  public void CheckCache()
  {
    if (this._original_pars != null)
      return;
    this._original_pars = new List<CustomFontSettings.CustomParameterSetting>();
    foreach (CustomFontSettings.CustomParameterSetting par in this.pars)
      this._original_pars.Add(this.ReadCustomParam(par.param));
  }

  public void Apply()
  {
    this.CheckCache();
    foreach (CustomFontSettings.CustomParameterSetting par in this.pars)
      this.SetCustomParam(par.param, par);
  }

  public void Restore()
  {
    this.CheckCache();
    foreach (CustomFontSettings.CustomParameterSetting originalPar in this._original_pars)
      this.SetCustomParam(originalPar.param, originalPar);
  }

  public void SetCustomParam(
    CustomFontSettings.CustomParameter par,
    CustomFontSettings.CustomParameterSetting data)
  {
    switch (par)
    {
      case CustomFontSettings.CustomParameter.Color:
        this.GetComponent<UILabel>().color = data.color;
        break;
      case CustomFontSettings.CustomParameter.EffectColor:
        this.GetComponent<UILabel>().effectColor = data.color;
        break;
      case CustomFontSettings.CustomParameter.TopAnchor:
        this.GetComponent<UIWidget>().topAnchor.absolute = (int) data.v;
        break;
      case CustomFontSettings.CustomParameter.BottomAnchor:
        this.GetComponent<UIWidget>().bottomAnchor.absolute = (int) data.v;
        break;
      case CustomFontSettings.CustomParameter.SimpleUITableOffset:
        this.GetComponent<SimpleUITable>().offset = (int) data.v;
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public CustomFontSettings.CustomParameterSetting ReadCustomParam(
    CustomFontSettings.CustomParameter par)
  {
    CustomFontSettings.CustomParameterSetting parameterSetting = new CustomFontSettings.CustomParameterSetting()
    {
      param = par
    };
    switch (par)
    {
      case CustomFontSettings.CustomParameter.Color:
        parameterSetting.color = this.GetComponent<UILabel>().color;
        break;
      case CustomFontSettings.CustomParameter.EffectColor:
        parameterSetting.color = this.GetComponent<UILabel>().effectColor;
        break;
      case CustomFontSettings.CustomParameter.TopAnchor:
        parameterSetting.v = (float) this.GetComponent<UIWidget>().topAnchor.absolute;
        break;
      case CustomFontSettings.CustomParameter.BottomAnchor:
        parameterSetting.v = (float) this.GetComponent<UIWidget>().bottomAnchor.absolute;
        break;
      case CustomFontSettings.CustomParameter.SimpleUITableOffset:
        parameterSetting.v = (float) this.GetComponent<SimpleUITable>().offset;
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    return parameterSetting;
  }

  [Serializable]
  public enum CustomParameter
  {
    Color,
    EffectColor,
    TopAnchor,
    BottomAnchor,
    SimpleUITableOffset,
  }

  [Serializable]
  public class CustomParameterSetting
  {
    public CustomFontSettings.CustomParameter param;
    public Color color;
    public float v;
  }
}
