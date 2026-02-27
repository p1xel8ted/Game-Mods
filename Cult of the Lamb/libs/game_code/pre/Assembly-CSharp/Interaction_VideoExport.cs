// Decompiled with JetBrains decompiler
// Type: Interaction_VideoExport
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.VideoMenu;
using src.Extensions;
using UnityEngine;

#nullable disable
public class Interaction_VideoExport : Interaction
{
  private UIVideoExportMenuController videoMenu;

  public override void GetLabel() => this.Label = ScriptLocalization.Interactions.Look;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Interactable = false;
    GameManager.GetInstance().OnConversationNew();
    Time.timeScale = 0.0f;
    HUD_Manager.Instance.Hide(false, 0);
    this.videoMenu = MonoSingleton<UIManager>.Instance.VideoExportTemplate.Instantiate<UIVideoExportMenuController>();
    this.videoMenu.Show(TimeManager.CurrentDay);
    UIVideoExportMenuController videoMenu1 = this.videoMenu;
    videoMenu1.OnHidden = videoMenu1.OnHidden + (System.Action) (() =>
    {
      Time.timeScale = 1f;
      this.Interactable = true;
      GameManager.GetInstance().OnConversationEnd();
    });
    UIVideoExportMenuController videoMenu2 = this.videoMenu;
    videoMenu2.OnCancel = videoMenu2.OnCancel + (System.Action) (() =>
    {
      Time.timeScale = 1f;
      this.Interactable = true;
      GameManager.GetInstance().OnConversationEnd();
    });
  }
}
