// Decompiled with JetBrains decompiler
// Type: src.Interaction_RelicBook
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using src.Extensions;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace src;

public class Interaction_RelicBook : Interaction
{
  public bool _activating;

  public override void OnEnable()
  {
    base.OnEnable();
    this.HasSecondaryInteraction = true;
  }

  public override void GetLabel()
  {
    this.Label = this.Interactable ? ScriptLocalization.Interactions.Look : "";
  }

  public override void GetSecondaryLabel()
  {
    this.SecondaryLabel = this.SecondaryInteractable ? ScriptLocalization.Interactions.ViewTarotCards : "";
  }

  public override void OnInteract(StateMachine state)
  {
    if (this._activating)
      return;
    base.OnInteract(state);
    this.playerFarming.GoToAndStop(this.gameObject, this.gameObject, GoToCallback: new System.Action(this.OpenRelicMenu));
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    if (this._activating)
      return;
    base.OnSecondaryInteract(state);
    this.playerFarming.GoToAndStop(this.gameObject, this.gameObject, GoToCallback: new System.Action(this.OpenTarotMenu));
  }

  public void OpenRelicMenu()
  {
    Time.timeScale = 0.0f;
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    GameManager.GetInstance().CameraSetOffset(new Vector3(-3f, 0.0f, 0.0f));
    HUD_Manager.Instance.Hide(false, 0);
    UIRelicMenuController relicMenuController = MonoSingleton<UIManager>.Instance.RelicMenuTemplate.Instantiate<UIRelicMenuController>();
    relicMenuController.Show();
    relicMenuController.OnHide = relicMenuController.OnHide + (System.Action) (() =>
    {
      Time.timeScale = 1f;
      HUD_Manager.Instance.Show(0);
    });
    relicMenuController.OnHidden = relicMenuController.OnHidden + (System.Action) (() =>
    {
      GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 0.0f));
      PlayerFarming.SetStateForAllPlayers();
      this._activating = false;
      this.HasChanged = true;
    });
  }

  public void OpenTarotMenu()
  {
    Time.timeScale = 0.0f;
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    GameManager.GetInstance().CameraSetOffset(new Vector3(-3f, 0.0f, 0.0f));
    HUD_Manager.Instance.Hide(false, 0);
    UITarotCardsMenuController cardsMenuController = MonoSingleton<UIManager>.Instance.TarotCardsMenuTemplate.Instantiate<UITarotCardsMenuController>();
    cardsMenuController.Show();
    cardsMenuController.OnHide = cardsMenuController.OnHide + (System.Action) (() =>
    {
      Time.timeScale = 1f;
      HUD_Manager.Instance.Show(0);
    });
    cardsMenuController.OnHidden = cardsMenuController.OnHidden + (System.Action) (() =>
    {
      GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 0.0f));
      PlayerFarming.SetStateForAllPlayers();
      this._activating = false;
      this.HasChanged = true;
    });
  }

  [CompilerGenerated]
  public void \u003COpenRelicMenu\u003Eb__6_1()
  {
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 0.0f));
    PlayerFarming.SetStateForAllPlayers();
    this._activating = false;
    this.HasChanged = true;
  }

  [CompilerGenerated]
  public void \u003COpenTarotMenu\u003Eb__7_1()
  {
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 0.0f));
    PlayerFarming.SetStateForAllPlayers();
    this._activating = false;
    this.HasChanged = true;
  }
}
