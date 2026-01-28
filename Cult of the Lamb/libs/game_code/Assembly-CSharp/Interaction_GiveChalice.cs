// Decompiled with JetBrains decompiler
// Type: Interaction_GiveChalice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_GiveChalice : Interaction
{
  public Worshipper w;
  public PlayerFarming playerFarming;
  public string sGiveChalice;

  public override void GetLabel() => this.Label = this.sGiveChalice;

  public void Start()
  {
    this.w = this.GetComponent<Worshipper>();
    this.ActivateDistance = 2f;
    this.UpdateLocalisation();
  }

  public override void UpdateLocalisation() => base.UpdateLocalisation();

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.playerFarming = state.GetComponent<PlayerFarming>();
    this.StartCoroutine((IEnumerator) this.GoToAndGiveChalice());
  }

  public IEnumerator GoToAndGiveChalice()
  {
    Interaction_GiveChalice interactionGiveChalice = this;
    GameObject TargetPosition = new GameObject();
    float angle = Utils.GetAngle(interactionGiveChalice.transform.position, Altar.Instance.CentrePoint.transform.position);
    TargetPosition.transform.position = interactionGiveChalice.transform.position + new Vector3(1.2f * Mathf.Cos(angle * ((float) Math.PI / 180f)), 1.2f * Mathf.Sin(angle * ((float) Math.PI / 180f)));
    interactionGiveChalice.playerFarming.GoToAndStop(TargetPosition, interactionGiveChalice.w.gameObject);
    while (interactionGiveChalice.playerFarming.GoToAndStopping)
      yield return (object) null;
    interactionGiveChalice.playerFarming.TimedAction(1.8f, new System.Action(interactionGiveChalice.PlayerInactive), "present-chalice");
    interactionGiveChalice.w.TimedAnimation("accept-chalice", 4.5f, new System.Action(interactionGiveChalice.ChaliceComplete));
    GameManager.GetInstance().OnConversationNew(false, false);
    GameManager.GetInstance().OnConversationNext(interactionGiveChalice.w.gameObject, 6f);
  }

  public void PlayerInactive() => this.state.CURRENT_STATE = StateMachine.State.InActive;

  public void ChaliceComplete() => this.StartCoroutine((IEnumerator) this.ChaliceCompleteWait());

  public IEnumerator ChaliceCompleteWait()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_GiveChalice interactionGiveChalice = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      interactionGiveChalice.w.Pray();
      UnityEngine.Object.Destroy((UnityEngine.Object) interactionGiveChalice);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    interactionGiveChalice.state.CURRENT_STATE = StateMachine.State.Idle;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }
}
