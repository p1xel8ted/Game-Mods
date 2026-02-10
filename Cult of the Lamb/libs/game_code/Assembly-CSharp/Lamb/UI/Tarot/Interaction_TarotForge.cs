// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Tarot.Interaction_TarotForge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using src.Extensions;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Tarot;

public class Interaction_TarotForge : Interaction
{
  public GameObject Menu;
  public GameObject PlayerLookTo;
  public string sString;
  public string relicString;
  public bool Activating;
  public UIMenuBase menu;

  public override void OnDisable()
  {
    if (!this.Activating)
      return;
    this.menu?.Hide(true);
    Time.timeScale = 1f;
    HUD_Manager.Instance.Show(0);
    this.HasChanged = true;
    this.Activating = false;
    this.menu = (UIMenuBase) null;
  }

  public void Start()
  {
    this.UpdateLocalisation();
    this.HasSecondaryInteraction = DataManager.Instance.OnboardedRelics;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = this.Activating ? "" : ScriptLocalization.Interactions.Look;
  }

  public override void GetLabel() => this.Label = this.sString;

  public override void GetSecondaryLabel()
  {
    if (!this.HasSecondaryInteraction)
      return;
    this.SecondaryLabel = ScriptLocalization.Interactions.ViewRelics;
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    this.Activating = true;
    base.OnInteract(state);
    this.playerFarming.GoToAndStop(this.gameObject, this.PlayerLookTo, GoToCallback: (System.Action) (() => this.OpenMenu((UIMenuBase) MonoSingleton<UIManager>.Instance.TarotCardsMenuTemplate.Instantiate<UITarotCardsMenuController>())));
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    this.Activating = true;
    base.OnSecondaryInteract(state);
    this.playerFarming.GoToAndStop(this.gameObject, this.PlayerLookTo, GoToCallback: (System.Action) (() => this.OpenMenu((UIMenuBase) MonoSingleton<UIManager>.Instance.RelicMenuTemplate.Instantiate<UIRelicMenuController>())));
  }

  public void OpenMenu(UIMenuBase menu)
  {
    this.menu = menu;
    Time.timeScale = 0.0f;
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    GameManager.GetInstance().CameraSetOffset(new Vector3(-3f, 0.0f, 0.0f));
    HUD_Manager.Instance.Hide(false, 0);
    menu.Show();
    menu.OnHide += (System.Action) (() =>
    {
      Time.timeScale = 1f;
      HUD_Manager.Instance.Show(0);
      this.HasChanged = true;
    });
    menu.OnHidden += (System.Action) (() =>
    {
      GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 0.0f));
      PlayerFarming.SetStateForAllPlayers();
      this.Activating = false;
      this.menu = (UIMenuBase) null;
    });
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__11_0()
  {
    this.OpenMenu((UIMenuBase) MonoSingleton<UIManager>.Instance.TarotCardsMenuTemplate.Instantiate<UITarotCardsMenuController>());
  }

  [CompilerGenerated]
  public void \u003COnSecondaryInteract\u003Eb__12_0()
  {
    this.OpenMenu((UIMenuBase) MonoSingleton<UIManager>.Instance.RelicMenuTemplate.Instantiate<UIRelicMenuController>());
  }

  [CompilerGenerated]
  public void \u003COpenMenu\u003Eb__13_0()
  {
    Time.timeScale = 1f;
    HUD_Manager.Instance.Show(0);
    this.HasChanged = true;
  }

  [CompilerGenerated]
  public void \u003COpenMenu\u003Eb__13_1()
  {
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 0.0f));
    PlayerFarming.SetStateForAllPlayers();
    this.Activating = false;
    this.menu = (UIMenuBase) null;
  }
}
