// Decompiled with JetBrains decompiler
// Type: Interaction_AltarEvents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_AltarEvents : Interaction
{
  public GameObject PlayerPosition;
  public int FollowersNeeded = 3;

  private void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation() => base.UpdateLocalisation();

  public override void GetLabel() => this.Label = ScriptLocalization.Interactions.SacrificeFollower;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.StartCoroutine((IEnumerator) this.WaitForPlayerPosition());
  }

  private IEnumerator WaitForPlayerPosition()
  {
    Interaction_AltarEvents interactionAltarEvents = this;
    interactionAltarEvents.state.GetComponent<PlayerFarming>().GoToAndStop(interactionAltarEvents.PlayerPosition);
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(interactionAltarEvents.state.gameObject, 10f);
    while ((double) Vector3.Distance(interactionAltarEvents.PlayerPosition.transform.position, interactionAltarEvents.state.transform.position) > 0.30000001192092896)
      yield return (object) null;
    interactionAltarEvents.state.facingAngle = -90f;
    yield return (object) new WaitForSeconds(0.2f);
    GameManager.GetInstance().OnConversationEnd();
    List<StructuresData> structuresDataList = new List<StructuresData>();
  }
}
