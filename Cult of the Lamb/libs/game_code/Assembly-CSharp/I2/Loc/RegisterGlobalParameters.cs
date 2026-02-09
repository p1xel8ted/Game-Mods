// Decompiled with JetBrains decompiler
// Type: I2.Loc.RegisterGlobalParameters
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace I2.Loc;

public class RegisterGlobalParameters : MonoBehaviour, ILocalizationParamsManager
{
  public virtual void OnEnable()
  {
    if (LocalizationManager.ParamManagers.Contains((ILocalizationParamsManager) this))
      return;
    LocalizationManager.ParamManagers.Add((ILocalizationParamsManager) this);
    LocalizationManager.LocalizeAll(true);
  }

  public virtual void OnDisable()
  {
    LocalizationManager.ParamManagers.Remove((ILocalizationParamsManager) this);
  }

  public virtual string GetParameterValue(string ParamName) => (string) null;
}
