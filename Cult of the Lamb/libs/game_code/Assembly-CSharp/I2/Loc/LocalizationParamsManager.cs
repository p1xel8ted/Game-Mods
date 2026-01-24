// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizationParamsManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace I2.Loc;

public class LocalizationParamsManager : MonoBehaviour, ILocalizationParamsManager
{
  [SerializeField]
  public List<LocalizationParamsManager.ParamValue> _Params = new List<LocalizationParamsManager.ParamValue>();
  public bool _IsGlobalManager;

  public string GetParameterValue(string ParamName)
  {
    if (this._Params != null)
    {
      int index = 0;
      for (int count = this._Params.Count; index < count; ++index)
      {
        if (this._Params[index].Name == ParamName)
          return this._Params[index].Value;
      }
    }
    return (string) null;
  }

  public void SetParameterValue(string ParamName, string ParamValue, bool localize = true)
  {
    bool flag = false;
    int index = 0;
    for (int count = this._Params.Count; index < count; ++index)
    {
      if (this._Params[index].Name == ParamName)
      {
        LocalizationParamsManager.ParamValue paramValue = this._Params[index] with
        {
          Value = ParamValue
        };
        this._Params[index] = paramValue;
        flag = true;
        break;
      }
    }
    if (!flag)
      this._Params.Add(new LocalizationParamsManager.ParamValue()
      {
        Name = ParamName,
        Value = ParamValue
      });
    if (!localize)
      return;
    this.OnLocalize();
  }

  public void OnLocalize()
  {
    Localize component = this.GetComponent<Localize>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.OnLocalize(true);
  }

  public virtual void OnEnable()
  {
    if (!this._IsGlobalManager)
      return;
    this.DoAutoRegister();
  }

  public void DoAutoRegister()
  {
    if (LocalizationManager.ParamManagers.Contains((ILocalizationParamsManager) this))
      return;
    LocalizationManager.ParamManagers.Add((ILocalizationParamsManager) this);
    LocalizationManager.LocalizeAll(true);
  }

  public void OnDisable()
  {
    LocalizationManager.ParamManagers.Remove((ILocalizationParamsManager) this);
  }

  [Serializable]
  public struct ParamValue
  {
    public string Name;
    public string Value;
  }
}
