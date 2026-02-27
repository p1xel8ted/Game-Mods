// Decompiled with JetBrains decompiler
// Type: Interaction_CatchSpider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_CatchSpider : Interaction
{
  public CritterSpider CritterSpider;
  public bool isCrab;
  private const int spidersRequiredForEasterEgg = 20;
  private const int dayRequiredForEasterEgg = 15;
  private string sString;
  private bool Activating;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.ActivateDistance = 1f;
    if (!this.isCrab || !DataManager.GetFollowerSkinUnlocked("Crab"))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  private void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.CatchCritter;
  }

  public override void GetLabel() => this.Label = this.Activating ? "" : this.sString;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.CatchCritterRoutine());
    if (this.isCrab)
      return;
    ++DataManager.Instance.SpidersCaught;
  }

  private IEnumerator CatchCritterRoutine()
  {
    Interaction_CatchSpider interactionCatchSpider = this;
    AudioManager.Instance.PlayOneShot("event:/player/weed_pick", interactionCatchSpider.transform.position);
    interactionCatchSpider.Activating = true;
    interactionCatchSpider.CritterSpider.enabled = false;
    interactionCatchSpider.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "catch-critter", false);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    interactionCatchSpider.state.facingAngle = Utils.GetAngle(interactionCatchSpider.state.transform.position, interactionCatchSpider.transform.position);
    interactionCatchSpider.transform.DOMove(PlayerFarming.Instance.transform.position, 1f);
    yield return (object) new WaitForSeconds(0.2f);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/spider_small/gethit", interactionCatchSpider.transform.position);
    if (interactionCatchSpider.isCrab)
    {
      if (!DataManager.GetFollowerSkinUnlocked("Crab"))
        FollowerSkinCustomTarget.Create(interactionCatchSpider.transform.position, PlayerFarming.Instance.transform.position, 0.4f, "Crab", (System.Action) null);
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FISH_CRAB, 1, interactionCatchSpider.transform.position);
    }
    else
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MEAT_MORSEL, 1, interactionCatchSpider.transform.position);
    interactionCatchSpider.CritterSpider.Inventory.DropItem();
    interactionCatchSpider.gameObject.SetActive(false);
    yield return (object) new WaitForSeconds(0.4f);
    interactionCatchSpider.state.CURRENT_STATE = StateMachine.State.Idle;
    interactionCatchSpider.gameObject.Recycle();
  }

  private IEnumerator Delay(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }
}
