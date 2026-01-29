// Decompiled with JetBrains decompiler
// Type: Interaction_AltarEvents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_AltarEvents : Interaction
{
  public GameObject PlayerPosition;
  public int FollowersNeeded = 3;

  public void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation() => base.UpdateLocalisation();

  public override void GetLabel() => this.Label = ScriptLocalization.Interactions.SacrificeFollower;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.StartCoroutine((IEnumerator) this.WaitForPlayerPosition());
  }

  public IEnumerator WaitForPlayerPosition()
  {
    Interaction_AltarEvents interactionAltarEvents = this;
    interactionAltarEvents.state.GetComponent<PlayerFarming>().GoToAndStop(interactionAltarEvents.PlayerPosition);
    GameManager.GetInstance().OnConversationNew(false, false);
    GameManager.GetInstance().OnConversationNext(interactionAltarEvents.state.gameObject, 10f);
    while ((double) Vector3.Distance(interactionAltarEvents.PlayerPosition.transform.position, interactionAltarEvents.state.transform.position) > 0.30000001192092896)
      yield return (object) null;
    interactionAltarEvents.state.facingAngle = -90f;
    yield return (object) new WaitForSeconds(0.2f);
    GameManager.GetInstance().OnConversationEnd();
    List<StructuresData> structuresDataList = new List<StructuresData>();
  }
}
