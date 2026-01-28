// Decompiled with JetBrains decompiler
// Type: Interaction_Schedule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_Schedule : Interaction
{
  public GameObject Menu;
  [CompilerGenerated]
  public bool \u003CActivated\u003Ek__BackingField;

  public bool Activated
  {
    get => this.\u003CActivated\u003Ek__BackingField;
    set => this.\u003CActivated\u003Ek__BackingField = value;
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activated)
      return;
    base.OnInteract(state);
    UnityEngine.Object.Instantiate<GameObject>(this.Menu, GameObject.FindWithTag("Canvas").transform).GetComponent<UISchedule>().CallbackClose = new System.Action(this.CallbackCancel);
    state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    this.playerFarming.simpleSpineAnimator.Animate("build", 0, true);
    this.Activated = true;
  }

  public override void GetLabel() => this.Label = this.Activated ? "" : "Schedule";

  public void CallbackCancel()
  {
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.Activated = false;
  }
}
