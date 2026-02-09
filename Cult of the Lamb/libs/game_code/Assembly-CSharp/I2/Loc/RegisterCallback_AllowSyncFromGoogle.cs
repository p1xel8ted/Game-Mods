// Decompiled with JetBrains decompiler
// Type: I2.Loc.RegisterCallback_AllowSyncFromGoogle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace I2.Loc;

public class RegisterCallback_AllowSyncFromGoogle : MonoBehaviour
{
  public void Awake()
  {
    LocalizationManager.Callback_AllowSyncFromGoogle = new Func<LanguageSourceData, bool>(this.AllowSyncFromGoogle);
  }

  public void OnEnable()
  {
    LocalizationManager.Callback_AllowSyncFromGoogle = new Func<LanguageSourceData, bool>(this.AllowSyncFromGoogle);
  }

  public void OnDisable()
  {
    LocalizationManager.Callback_AllowSyncFromGoogle = (Func<LanguageSourceData, bool>) null;
  }

  public virtual bool AllowSyncFromGoogle(LanguageSourceData Source) => true;
}
