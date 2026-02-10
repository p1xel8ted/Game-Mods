// Decompiled with JetBrains decompiler
// Type: Interaction_CatchSpider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_CatchSpider : Interaction
{
  public CritterSpider CritterSpider;
  public bool isCrab;
  public const int spidersRequiredForEasterEgg = 10;
  public const int dayRequiredForEasterEgg = 5;
  public string sString;
  public bool Activating;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.ActivateDistance = 1f;
    if (!this.isCrab || !DataManager.GetFollowerSkinUnlocked("Crab"))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void Start() => this.UpdateLocalisation();

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

  public IEnumerator CatchCritterRoutine()
  {
    Interaction_CatchSpider interactionCatchSpider = this;
    string webberSkin = "Webber";
    AudioManager.Instance.PlayOneShot("event:/player/weed_pick", interactionCatchSpider.transform.position);
    interactionCatchSpider.Activating = true;
    interactionCatchSpider.CritterSpider.enabled = false;
    interactionCatchSpider.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionCatchSpider.playerFarming.Spine.AnimationState.SetAnimation(0, "catch-critter", false);
    interactionCatchSpider.playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    interactionCatchSpider.state.facingAngle = Utils.GetAngle(interactionCatchSpider.state.transform.position, interactionCatchSpider.transform.position);
    interactionCatchSpider.transform.DOMove(interactionCatchSpider.playerFarming.transform.position, 1f);
    yield return (object) new WaitForSeconds(0.2f);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/spider_small/gethit", interactionCatchSpider.transform.position);
    if (interactionCatchSpider.isCrab)
    {
      if (!DataManager.GetFollowerSkinUnlocked("Crab"))
      {
        GameManager.GetInstance().OnConversationNew();
        GameManager.GetInstance().OnConversationNext(interactionCatchSpider.gameObject, 6f);
        FollowerSkinCustomTarget.Create(interactionCatchSpider.transform.position, interactionCatchSpider.playerFarming.transform.position, 0.4f, "Crab", (System.Action) (() =>
        {
          GameManager.GetInstance().OnConversationEnd();
          GameManager.GetInstance().AddPlayerToCamera();
        }));
      }
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FISH_CRAB, 1, interactionCatchSpider.transform.position);
    }
    else
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MEAT_MORSEL, 1, interactionCatchSpider.transform.position);
    interactionCatchSpider.CritterSpider.Inventory.DropItem();
    interactionCatchSpider.gameObject.SetActive(false);
    List<StructureBrain.TYPES> decorationsToUnlock = Interaction_WebberSkull.GetDecorationsToUnlock();
    bool flag = false;
    foreach (StructureBrain.TYPES Types in decorationsToUnlock)
    {
      if (!StructuresData.GetUnlocked(Types))
      {
        flag = true;
        break;
      }
    }
    if (DataManager.Instance.SpidersCaught >= 10 && TimeManager.CurrentDay >= 5 && !DataManager.Instance.FollowerSkinsUnlocked.Contains(webberSkin) | flag && (UnityEngine.Object) Interaction_WebberSkull.WebberSkull == (UnityEngine.Object) null && !interactionCatchSpider.isCrab)
    {
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(interactionCatchSpider.gameObject, 6f);
      AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_reveal", interactionCatchSpider.gameObject);
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.WEBBER_SKULL, 1, interactionCatchSpider.transform.position, (float) UnityEngine.Random.Range(9, 11), new System.Action<PickUp>(interactionCatchSpider.\u003CCatchCritterRoutine\u003Eb__11_1));
    }
    else
    {
      yield return (object) new WaitForSeconds(0.4f);
      interactionCatchSpider.state.CURRENT_STATE = StateMachine.State.Idle;
      interactionCatchSpider.gameObject.Recycle();
      interactionCatchSpider.CritterSpider.enabled = true;
      interactionCatchSpider.Activating = false;
    }
  }

  public IEnumerator Delay(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  [CompilerGenerated]
  public void \u003CCatchCritterRoutine\u003Eb__11_1(PickUp pickUp)
  {
    pickUp.SetInitialSpeedAndDiraction((float) UnityEngine.Random.Range(9, 11), Utils.GetAngle(this.transform.position, Vector3.zero));
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.Delay(1f, (System.Action) (() =>
    {
      Interaction_WebberSkull component = pickUp.GetComponent<Interaction_WebberSkull>();
      component.Structure.CreateStructure(FollowerLocation.Base, component.transform.position);
      component.Structure.Brain.AddToGrid();
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.Delay(1f, (System.Action) (() =>
      {
        GameManager.GetInstance().OnConversationEnd();
        this.state.CURRENT_STATE = StateMachine.State.Idle;
        this.gameObject.Recycle();
        this.CritterSpider.enabled = true;
        this.Activating = false;
      })));
    })));
    GameManager.GetInstance().OnConversationNext(pickUp.gameObject, 6f);
  }

  [CompilerGenerated]
  public void \u003CCatchCritterRoutine\u003Eb__11_3()
  {
    GameManager.GetInstance().OnConversationEnd();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.gameObject.Recycle();
    this.CritterSpider.enabled = true;
    this.Activating = false;
  }
}
