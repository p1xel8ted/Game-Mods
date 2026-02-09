// Decompiled with JetBrains decompiler
// Type: ResetUIOnDisable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine;

#nullable disable
public class ResetUIOnDisable : BaseMonoBehaviour
{
  public void OnDisable() => this.ResetUI();

  public void ResetUI()
  {
    UIMenuBase currentMenu = MonoSingleton<UIManager>.Instance.CurrentMenu;
    if (!((Object) currentMenu != (Object) null))
      return;
    LetterBox.Hide();
    HUD_Manager.Instance.Show(0);
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().CameraResetTargetZoom();
    GameManager.GetInstance().AddPlayerToCamera();
    PlayerFarming.SetStateForAllPlayers();
    this.StopAllCoroutines();
    currentMenu?.Hide(true);
  }
}
