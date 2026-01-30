// Decompiled with JetBrains decompiler
// Type: Interaction_VideoExport
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.VideoMenu;
using src.Extensions;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_VideoExport : Interaction
{
  public UIVideoExportMenuController videoMenu;

  public override void GetLabel() => this.Label = ScriptLocalization.Interactions.Look;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Interactable = false;
    GameManager.GetInstance().OnConversationNew();
    Time.timeScale = 0.0f;
    HUD_Manager.Instance.Hide(false, 0);
    this.videoMenu = MonoSingleton<UIManager>.Instance.VideoExportTemplate.Instantiate<UIVideoExportMenuController>();
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

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__2_0()
  {
    Time.timeScale = 1f;
    this.Interactable = true;
    GameManager.GetInstance().OnConversationEnd();
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__2_1()
  {
    Time.timeScale = 1f;
    this.Interactable = true;
    GameManager.GetInstance().OnConversationEnd();
  }
}
