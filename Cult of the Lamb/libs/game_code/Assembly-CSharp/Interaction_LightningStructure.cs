// Decompiled with JetBrains decompiler
// Type: Interaction_LightningStructure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_LightningStructure : Interaction
{
  public Coroutine lightningStrikeRoutine;
  public EventInstance lightningLoopSfx;
  public Structure structure;
  public Interaction otherInteraction;
  public GameObject lightningContainer;
  public SpriteRenderer lightningIndicator;
  public SpriteRenderer lightningIndicator2;
  public bool struck;
  public bool wasProtected;

  public void Configure(Structure structure, Interaction otherInteraction)
  {
    this.structure = structure;
    this.otherInteraction = otherInteraction;
    if ((Object) otherInteraction != (Object) null)
      otherInteraction.enabled = false;
    this.lightningContainer = BiomeConstants.Instance.LightningIndicator;
    this.lightningContainer.gameObject.SetActive(true);
    this.lightningContainer.transform.position = this.transform.position;
    SpriteRenderer[] componentsInChildren = this.lightningContainer.transform.GetComponentsInChildren<SpriteRenderer>(true);
    this.lightningIndicator = componentsInChildren[0];
    this.lightningIndicator2 = componentsInChildren[1];
    this.LightningStrikeIncoming();
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Label = this.Interactable ? LocalizationManager.GetTranslation("FollowerInteractions/Protect") : "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.StartCoroutine((IEnumerator) this.ProtectFromLightningIE());
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.Strike();
    AudioManager.Instance.StopLoop(this.lightningLoopSfx);
    if (!((Object) this.lightningContainer != (Object) null))
      return;
    this.lightningContainer.gameObject.SetActive(false);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.Strike();
    AudioManager.Instance.StopLoop(this.lightningLoopSfx);
    if (!((Object) this.lightningContainer != (Object) null))
      return;
    this.lightningContainer.gameObject.SetActive(false);
  }

  public void LightningStrikeIncoming()
  {
    NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/StructureLightningIncoming", this.structure.Brain.Data.GetLocalizedName());
    AudioManager.Instance.PlayOneShot("event:/weapon/crit_hit", PlayerFarming.Instance.gameObject);
    this.lightningStrikeRoutine = GameManager.GetInstance().StartCoroutine((IEnumerator) this.LightningStrikeIncomingIE());
  }

  public IEnumerator LightningStrikeIncomingIE()
  {
    Interaction_LightningStructure lightningStructure = this;
    float timeUntilStrike = 15f;
    float time = 0.0f;
    Color indicatorColor = Color.white;
    float flashTickTimer = 0.0f;
    lightningStructure.lightningLoopSfx = AudioManager.Instance.CreateLoop("event:/dlc/follower/lightning_warning_loop", lightningStructure.gameObject, true);
    lightningStructure.lightningContainer.gameObject.SetActive(true);
    while ((double) (time += Time.deltaTime) < (double) timeUntilStrike && PlayerFarming.Location == FollowerLocation.Base)
    {
      flashTickTimer += Time.deltaTime;
      if ((double) flashTickTimer >= 0.11999999731779099 && BiomeConstants.Instance.IsFlashLightsActive)
      {
        indicatorColor = indicatorColor == Color.white ? Color.red : Color.white;
        lightningStructure.lightningIndicator.material.SetColor("_Color", indicatorColor);
        lightningStructure.lightningIndicator2.material.SetColor("_Color", indicatorColor);
        flashTickTimer = 0.0f;
      }
      yield return (object) null;
    }
    AudioManager.Instance.StopLoop(lightningStructure.lightningLoopSfx);
    lightningStructure.lightningStrikeRoutine = (Coroutine) null;
    if ((Object) lightningStructure.lightningContainer != (Object) null)
      lightningStructure.lightningContainer.gameObject.SetActive(false);
    lightningStructure.Strike();
    lightningStructure.Interactable = false;
    lightningStructure.HasChanged = true;
  }

  public IEnumerator ProtectFromLightningIE()
  {
    Interaction_LightningStructure lightningStructure = this;
    lightningStructure.wasProtected = true;
    AudioManager.Instance.PlayOneShot("event:/dlc/follower/lightning_protect", lightningStructure.transform.position);
    AudioManager.Instance.StopLoop(lightningStructure.lightningLoopSfx);
    if ((Object) HUD_Manager.Instance != (Object) null)
      HUD_Manager.Instance.ClearLightningTarget();
    if ((Object) lightningStructure.lightningContainer != (Object) null)
      lightningStructure.lightningContainer.gameObject.SetActive(false);
    if (lightningStructure.lightningStrikeRoutine != null)
      lightningStructure.StopCoroutine(lightningStructure.lightningStrikeRoutine);
    lightningStructure.lightningStrikeRoutine = (Coroutine) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(lightningStructure.playerFarming.gameObject, 7f);
    lightningStructure.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    lightningStructure.playerFarming.simpleSpineAnimator.Animate("sermons/sermon-start-nobook", 0, false);
    lightningStructure.playerFarming.transform.DOMove(lightningStructure.transform.position, 0.5f);
    lightningStructure.playerFarming.Spine.transform.DOLocalMove(Vector3.back / 2f, 0.5f);
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForSeconds(1.26666665f);
    WeatherSystemController.Instance.TriggerLightningStrike(lightningStructure.transform.position + Vector3.back * 1.5f);
    yield return (object) new WaitForSeconds(0.2f);
    lightningStructure.LambHitSequence();
    lightningStructure.playerFarming.Spine.transform.localPosition = Vector3.zero;
    GameManager.GetInstance().OnConversationNext(lightningStructure.playerFarming.gameObject);
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, Random.Range(3, 6), lightningStructure.transform.position);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    if ((Object) lightningStructure.otherInteraction != (Object) null)
      lightningStructure.otherInteraction.enabled = true;
    Object.Destroy((Object) lightningStructure);
  }

  public void LambHitSequence()
  {
    PlayerFarming instance = PlayerFarming.Instance;
    AudioManager.Instance.PlayOneShot("event:/player/gethit", instance.transform.position);
    BiomeConstants.Instance.EmitHitVFX(instance.transform.position, Quaternion.identity.z, "HitFX_Blocked");
    instance.simpleSpineAnimator.FlashWhite(false);
    CameraManager.instance.ShakeCameraForDuration(1.5f, 1.7f, 0.2f);
    instance.state.facingAngle = Utils.GetAngle(PlayerFarming.Instance.transform.position, this.transform.position);
    instance.state.CURRENT_STATE = StateMachine.State.HitThrown;
    instance.playerController.MakeUntouchable(1f, false);
    instance.transform.DOMoveX(instance.transform.position.x + ((double) instance.transform.position.x > (double) this.transform.position.x ? 0.5f : -0.5f), 0.75f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive));
  }

  public void Strike()
  {
    if (this.struck || this.wasProtected)
      return;
    this.struck = true;
    WeatherSystemController.Instance.TriggerLightningStrike(this.structure.Brain);
    if ((Object) this.otherInteraction != (Object) null)
      this.otherInteraction.enabled = true;
    Object.Destroy((Object) this);
  }
}
