// Decompiled with JetBrains decompiler
// Type: Interaction_Schedule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Interaction_Schedule : Interaction
{
  public GameObject Menu;

  public bool Activated { get; set; }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activated)
      return;
    base.OnInteract(state);
    UnityEngine.Object.Instantiate<GameObject>(this.Menu, GameObject.FindWithTag("Canvas").transform).GetComponent<UISchedule>().CallbackClose = new System.Action(this.CallbackCancel);
    state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
    this.Activated = true;
  }

  public override void GetLabel() => this.Label = this.Activated ? "" : "Schedule";

  private void CallbackCancel()
  {
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.Activated = false;
  }
}
