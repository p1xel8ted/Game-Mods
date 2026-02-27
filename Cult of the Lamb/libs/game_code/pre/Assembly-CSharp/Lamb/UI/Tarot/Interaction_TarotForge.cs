// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Tarot.Interaction_TarotForge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using src.Extensions;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Tarot;

public class Interaction_TarotForge : Interaction
{
  public GameObject Menu;
  public GameObject PlayerLookTo;
  private string sString;
  private bool Activating;

  private void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = this.Activating ? "" : ScriptLocalization.Interactions.Look;
  }

  public override void GetLabel() => this.Label = this.sString;

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    base.OnInteract(state);
    PlayerFarming.Instance.GoToAndStop(this.gameObject, this.PlayerLookTo, GoToCallback: new System.Action(this.OpenMenu));
  }

  private void OpenMenu()
  {
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    GameManager.GetInstance().CameraSetOffset(new Vector3(-3f, 0.0f, 0.0f));
    HUD_Manager.Instance.Hide(false, 0);
    UITarotCardsMenuController cardsMenuController = MonoSingleton<UIManager>.Instance.TarotCardsMenuTemplate.Instantiate<UITarotCardsMenuController>();
    cardsMenuController.Show();
    cardsMenuController.OnHide = cardsMenuController.OnHide + (System.Action) (() => HUD_Manager.Instance.Show(0));
    cardsMenuController.OnHidden = cardsMenuController.OnHidden + (System.Action) (() =>
    {
      GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 0.0f));
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
      this.Activating = false;
    });
  }

  private void CallBack()
  {
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    this.Activating = false;
  }
}
