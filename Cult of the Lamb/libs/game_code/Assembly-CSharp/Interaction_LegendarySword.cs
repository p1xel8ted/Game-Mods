// Decompiled with JetBrains decompiler
// Type: Interaction_LegendarySword
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_LegendarySword : Interaction
{
  public System.Action OnSwordPulled;

  public override void GetLabel()
  {
    base.GetLabel();
    this.Label = "Pull";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.StartCoroutine((IEnumerator) this.PullSwordSequence());
  }

  public IEnumerator PullSwordSequence()
  {
    Interaction_LegendarySword interactionLegendarySword = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionLegendarySword.gameObject, 5f);
    interactionLegendarySword.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForSeconds(interactionLegendarySword.playerFarming.simpleSpineAnimator.Animate("attack-charge3", 0, false).Animation.Duration);
    yield return (object) new WaitForSeconds(interactionLegendarySword.playerFarming.simpleSpineAnimator.Animate("cards/cards-stop-sword3", 0, false).Animation.Duration);
    GameManager.GetInstance().OnConversationEnd();
    System.Action onSwordPulled = interactionLegendarySword.OnSwordPulled;
    if (onSwordPulled != null)
      onSwordPulled();
    BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionLegendarySword.transform.position);
    interactionLegendarySword.gameObject.SetActive(false);
    RoomLockController.RoomCompleted();
  }
}
