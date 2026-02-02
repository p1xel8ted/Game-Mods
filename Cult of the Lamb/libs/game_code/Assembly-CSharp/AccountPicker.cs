// Decompiled with JetBrains decompiler
// Type: AccountPicker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
