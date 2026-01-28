// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Menus.PlayerMenu.UIPauseDetailsMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.UINavigator;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Menus.PlayerMenu;

public class UIPauseDetailsMenuController : UIMenuBase
{
  public void OnEnable() => MonoSingleton<UIManager>.Instance.SetCurrentCursor(0);

  public new void OnDisable()
  {
    MonoSingleton<UIManager>.Instance.SetPreviousCursor();
    MonoSingleton<UIManager>.Instance.ResetPreviousCursor();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    Time.timeScale = 1f;
  }

  public void Update()
  {
    if (this._canvasGroup.interactable && InputManager.Gameplay.GetMenuButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      this.OnCancelButtonInput();
    Time.timeScale = 0.0f;
  }

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    AudioManager.Instance.PauseActiveLoopsAndSFX();
    UIManager.PlayAudio("event:/ui/pause");
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/unpause");
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  public override void OnHideCompleted()
  {
    AudioManager.Instance.ResumePausedLoopsAndSFX();
    Object.Destroy((Object) this.gameObject);
  }
}
