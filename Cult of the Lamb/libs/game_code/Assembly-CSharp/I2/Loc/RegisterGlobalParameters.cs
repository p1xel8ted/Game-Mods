// Decompiled with JetBrains decompiler
// Type: I2.Loc.RegisterGlobalParameters
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
