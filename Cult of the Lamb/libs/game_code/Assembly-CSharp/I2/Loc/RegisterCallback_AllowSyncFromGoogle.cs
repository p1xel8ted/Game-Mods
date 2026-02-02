// Decompiled with JetBrains decompiler
// Type: I2.Loc.RegisterCallback_AllowSyncFromGoogle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
