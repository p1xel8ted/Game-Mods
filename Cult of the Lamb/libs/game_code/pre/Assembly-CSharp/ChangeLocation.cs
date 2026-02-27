// Decompiled with JetBrains decompiler
// Type: ChangeLocation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using UnityEngine;

#nullable disable
public class ChangeLocation : BaseMonoBehaviour
{
  public string Scene = "Hub";
  [TermsPopup("")]
  public string LocationName = "";
  private bool Activated;

  private void OnTriggerEnter2D(Collider2D Collision)
  {
    if (this.Activated || !(Collision.gameObject.tag == "Player"))
      return;
    SimulationManager.Pause();
    Collision.gameObject.GetComponent<StateMachine>().CURRENT_STATE = StateMachine.State.InActive;
    this.Activated = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, this.Scene, 1f, this.LocationName, new System.Action(this.FadeSave));
  }

  private void FadeSave() => SaveAndLoad.Save();
}
