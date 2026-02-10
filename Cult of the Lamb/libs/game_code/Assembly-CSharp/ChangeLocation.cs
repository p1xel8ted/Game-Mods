// Decompiled with JetBrains decompiler
// Type: ChangeLocation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using UnityEngine;

#nullable disable
public class ChangeLocation : BaseMonoBehaviour
{
  public string Scene = "Hub";
  [TermsPopup("")]
  public string LocationName = "";
  public bool Activated;

  public void OnTriggerEnter2D(Collider2D Collision)
  {
    if (this.Activated || !Collision.gameObject.CompareTag("Player"))
      return;
    SimulationManager.Pause();
    Collision.gameObject.GetComponent<StateMachine>().CURRENT_STATE = StateMachine.State.InActive;
    this.Activated = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, this.Scene, 1f, this.LocationName, new System.Action(this.FadeSave));
  }

  public void FadeSave() => SaveAndLoad.Save();
}
