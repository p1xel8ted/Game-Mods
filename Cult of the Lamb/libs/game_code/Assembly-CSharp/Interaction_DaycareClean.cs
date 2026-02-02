// Decompiled with JetBrains decompiler
// Type: Interaction_DaycareClean
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_DaycareClean : Interaction
{
  [SerializeField]
  public Interaction_Daycare daycare;
  [SerializeField]
  public Structure structure;
  public Coroutine cleaningRoutine;

  public override void OnDisable()
  {
    base.OnDisable();
    this.cleaningRoutine = (Coroutine) null;
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Label = this.structure.Brain.Data.Inventory.Count <= 0 || this.structure.Brain.Data.Inventory[0].quantity < 4 || this.cleaningRoutine != null ? "" : ScriptLocalization.Interactions.Clean;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.cleaningRoutine = this.StartCoroutine((IEnumerator) this.CleanPoopsIE());
  }

  public IEnumerator CleanPoopsIE()
  {
    Interaction_DaycareClean interactionDaycareClean = this;
    interactionDaycareClean.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionDaycareClean.state.facingAngle = Utils.GetAngle(interactionDaycareClean.state.transform.position, interactionDaycareClean.transform.position);
    yield return (object) new WaitForEndOfFrame();
    interactionDaycareClean.playerFarming.simpleSpineAnimator.Animate("cleaning", 0, true);
    AudioManager.Instance.PlayOneShot("event:/player/sweep", interactionDaycareClean.transform.position);
    Debug.Log((object) ("Chore duration: " + DataManager.GetChoreDuration(interactionDaycareClean.playerFarming).ToString()));
    float timeBetween = 1f / (float) interactionDaycareClean.structure.Brain.Data.Inventory[0].quantity;
    for (int i = 0; i < interactionDaycareClean.structure.Brain.Data.Inventory[0].quantity; ++i)
    {
      yield return (object) new WaitForSeconds(timeBetween);
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.POOP, 1, interactionDaycareClean.transform.position);
      AudioManager.Instance.PlayOneShot("event:/followers/poop_pop", interactionDaycareClean.transform.position);
    }
    interactionDaycareClean._playerFarming.playerChoreXPBarController.AddChoreXP(interactionDaycareClean.playerFarming, (float) interactionDaycareClean.structure.Brain.Data.Inventory[0].quantity);
    interactionDaycareClean.structure.Brain.Data.Inventory.Clear();
    interactionDaycareClean.daycare.UpdatePoopStates();
    AudioManager.Instance.PlayOneShot("event:/followers/poop_pop", interactionDaycareClean.transform.position);
    ++DataManager.Instance.itemsCleaned;
    System.Action onCrownReturn = interactionDaycareClean.playerFarming.OnCrownReturn;
    if (onCrownReturn != null)
      onCrownReturn();
    interactionDaycareClean.state.CURRENT_STATE = StateMachine.State.Idle;
    interactionDaycareClean.cleaningRoutine = (Coroutine) null;
  }
}
