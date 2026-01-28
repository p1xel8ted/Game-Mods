// Decompiled with JetBrains decompiler
// Type: Interaction_RelicFish
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_RelicFish : Interaction
{
  [SerializeField]
  public SkeletonAnimation fishSpine;
  public bool waitingForEventListener;

  public override void GetLabel()
  {
    base.GetLabel();
    this.Label = LocalizationManager.GetTranslation("Interactions/Tickle");
  }

  public void Awake() => this.CheckSkin();

  public void CheckSkin()
  {
    if (DataManager.Instance.FoundRelicInFish)
      this.fishSpine.skeleton.SetSkin("happyEye");
    else
      this.fishSpine.skeleton.SetSkin("defaultEye");
  }

  public override void OnEnableInteraction() => base.OnEnableInteraction();

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Interactable = false;
    this.StartCoroutine((IEnumerator) this.InteractionIE());
  }

  public IEnumerator InteractionIE()
  {
    Interaction_RelicFish interactionRelicFish = this;
    interactionRelicFish.fishSpine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(interactionRelicFish.HandleAnimationStateEvent);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionRelicFish.gameObject);
    interactionRelicFish.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionRelicFish.playerFarming.Spine.AnimationState.SetAnimation(0, "pet-dog", false);
    interactionRelicFish.playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    yield return (object) new WaitForSeconds(1.5f);
    CameraManager.instance.ShakeCameraForDuration(0.75f, 1f, 2f);
    GameManager.GetInstance().CameraSetZoom(6f);
    AudioManager.Instance.PlayOneShot("event:/doctrine_stone/doctrine_shake", interactionRelicFish.fishSpine.gameObject);
    interactionRelicFish.fishSpine.AnimationState.SetAnimation(0, "toothpulled", false);
    interactionRelicFish.fishSpine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    while (!interactionRelicFish.waitingForEventListener)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/player/harvest_meat_done", interactionRelicFish.gameObject);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact, interactionRelicFish.playerFarming);
    CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
    DataManager.Instance.FoundRelicInFish = true;
    interactionRelicFish.CheckSkin();
    bool waiting = true;
    GameObject gameObject = RelicCustomTarget.Create(interactionRelicFish.transform.position, interactionRelicFish.playerFarming.transform.position, 1f, RelicType.RerollWeapon, (System.Action) (() => waiting = false));
    AudioManager.Instance.PlayOneShot("event:/relics/heart_convert_blessed", gameObject);
    GameManager.GetInstance().OnConversationNext(gameObject);
    while (waiting)
      yield return (object) null;
    interactionRelicFish.fishSpine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(interactionRelicFish.HandleAnimationStateEvent);
    GameManager.GetInstance().OnConversationEnd();
    interactionRelicFish.gameObject.SetActive(false);
    PlayerFarming.SetStateForAllPlayers();
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.FindKudaiiRelic);
  }

  public void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.ToString() == "relic")
      this.waitingForEventListener = true;
    else
      AudioManager.Instance.PlayOneShot("event:/fishing/splash", this.fishSpine.gameObject);
  }
}
