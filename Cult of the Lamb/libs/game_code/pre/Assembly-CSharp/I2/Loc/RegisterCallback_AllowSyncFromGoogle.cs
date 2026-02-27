// Decompiled with JetBrains decompiler
// Type: I2.Loc.RegisterCallback_AllowSyncFromGoogle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
