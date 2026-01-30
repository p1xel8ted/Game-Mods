// Decompiled with JetBrains decompiler
// Type: AccountPicker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Unify;
using Unify.Input;
using UnityEngine;

#nullable disable
public class AccountPicker : MonoBehaviour
{
  public void Start()
  {
    if (UnifyManager.platform != UnifyManager.Platform.GameCore)
      return;
    this.enabled = false;
  }

  public void Update()
  {
    if (!InputManager.UI.GetAccountPickerButtonDown())
      return;
    User sessionOwner = SessionManager.GetSessionOwner();
    if (sessionOwner == null || !(sessionOwner.gamePadId != GamePad.None))
      return;
    UserHelper.DisengageAllPlayers();
    UserHelper.Instance.ShowAccountPicker(sessionOwner.gamePadId);
  }
}
