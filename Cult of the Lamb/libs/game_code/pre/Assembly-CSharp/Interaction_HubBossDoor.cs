// Decompiled with JetBrains decompiler
// Type: Interaction_HubBossDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_HubBossDoor : Interaction
{
  private string sString;
  public FollowerLocation Location = FollowerLocation.Hub1;
  public string Scene;
  [TermsPopup("")]
  public string LocationName = "";

  private void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.OpenDoor;
  }

  public override void GetLabel() => this.Label = this.sString;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (false)
      this.StartCoroutine((IEnumerator) this.EnterTempleRoutine());
    else
      this.StartCoroutine((IEnumerator) this.InteractRoutine());
  }

  private IEnumerator EnterTempleRoutine()
  {
    Interaction_HubBossDoor interactionHubBossDoor = this;
    interactionHubBossDoor.state.CURRENT_STATE = StateMachine.State.InActive;
    yield return (object) new WaitForSecondsRealtime(3f);
    yield return (object) new WaitForSecondsRealtime(1f);
    interactionHubBossDoor.EnterTemple();
  }

  private void EnterTemple()
  {
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, this.Scene, 1f, this.LocationName, new System.Action(this.FadeSave));
  }

  private void FadeSave() => SaveAndLoad.Save();

  private IEnumerator InteractRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_HubBossDoor interactionHubBossDoor = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      interactionHubBossDoor.state.CURRENT_STATE = StateMachine.State.Idle;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    interactionHubBossDoor.state.CURRENT_STATE = StateMachine.State.InActive;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }
}
