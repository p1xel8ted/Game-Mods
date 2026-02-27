// Decompiled with JetBrains decompiler
// Type: AccountPicker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Unify;
using Unify.Input;
using UnityEngine;

#nullable disable
public class AccountPicker : MonoBehaviour
{
  private void Start()
  {
    if (UnifyManager.platform != UnifyManager.Platform.GameCore)
      return;
    this.enabled = false;
  }

  private void Update()
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
