// Decompiled with JetBrains decompiler
// Type: Interaction_CorruptedMonolith
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI;
using Spine.Unity;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_CorruptedMonolith : Interaction
{
  public string sLabel;
  [SerializeField]
  public GameObject MonolithSprite;
  [SerializeField]
  public GameObject Rubblesprite;
  [SerializeField]
  public SimpleSetCamera SetCamera1;
  [SerializeField]
  public SimpleSetCamera SetCamera2;
  [SerializeField]
  public GameObject CorruptedLighting;
  [SerializeField]
  public GameObject ActivateLighting;
  [SerializeField]
  public GameObject PurpleStuff;
  [SerializeField]
  public GameObject RedStuff;
  [SerializeField]
  public SkeletonAnimation Tent1;
  [SerializeField]
  public SkeletonAnimation Tent2;
  [SerializeField]
  public SkeletonAnimation Tent3;
  [SerializeField]
  public SkeletonAnimation Tent4;
  [SerializeField]
  public SkeletonAnimation Tent5;
  [SerializeField]
  public SkeletonAnimation Tent6;
  [SerializeField]
  public SkeletonAnimation Tent7;
  [SerializeField]
  public SkeletonAnimation Tent8;
  [SerializeField]
  public SkeletonAnimation Tent9;
  [SerializeField]
  public SkeletonAnimation Tent10;
  [SerializeField]
  public SkeletonAnimation Tent11;
  [SerializeField]
  public SkeletonAnimation Tent12;
  [SerializeField]
  public SkeletonAnimation Tent13;
  [SerializeField]
  public SkeletonAnimation Tent14;
  [SerializeField]
  public SkeletonAnimation Tent15;
  public static System.Action OnCorruptedComplete;
  public FMODLoopSound[] loopingSounds;
  public int Num = -1;
  public float Timer;
  public float TargetTime = 2f;
  public bool IsCurrent;
  public bool FirstOnBecomeCurrent = true;

  public void Start()
  {
    this.loopingSounds = this.transform.parent.GetComponentsInChildren<FMODLoopSound>();
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.UpdateLocalisation();
    this.UpdateSprite();
    AudioManager.Instance.PlayMusic("event:/music/corrupted_room/corrupted_room");
    AudioManager.Instance.StopCurrentAtmos();
  }

  public void UpdateSprite()
  {
    this.MonolithSprite.SetActive(!DataManager.Instance.UnlockedCorruptedRelicsAndTarots);
    this.Rubblesprite.SetActive(DataManager.Instance.UnlockedCorruptedRelicsAndTarots);
  }

  public override void UpdateLocalisation()
  {
    this.Timer = 0.0f;
    this.TargetTime = 3.5f;
    this.Interactable = true;
    switch (this.Num)
    {
      case -1:
        this.sLabel = ScriptLocalization.Interactions.CorruptedShrine0;
        this.TargetTime = 4f;
        break;
      case 0:
        this.sLabel = ScriptLocalization.Interactions.CorruptedShrine1;
        break;
      case 1:
        this.sLabel = ScriptLocalization.Interactions.CorruptedShrine2;
        break;
      case 2:
        this.sLabel = ScriptLocalization.Interactions.CorruptedShrine3;
        this.TargetTime = 0.75f;
        this.Interactable = false;
        break;
      case 3:
        this.sLabel = ScriptLocalization.Interactions.CorruptedShrine4;
        this.TargetTime = 0.75f;
        this.Interactable = false;
        break;
      case 4:
        this.sLabel = ScriptLocalization.Interactions.CorruptedShrine5;
        this.TargetTime = 0.75f;
        this.Interactable = false;
        break;
      case 5:
        this.sLabel = ScriptLocalization.Interactions.CorruptedShrine6;
        break;
      case 6:
        this.sLabel = LocalizationManager.GetTranslation("Interactions/ALTCorruptedShrine7");
        break;
      case 7:
        this.HoldToInteract = true;
        this.sLabel = LocalizationManager.GetTranslation("Interactions/ALTCorruptedShrine8");
        break;
    }
    base.UpdateLocalisation();
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (!DataManager.Instance.UnlockedCorruptedRelicsAndTarots)
      this.Label = this.sLabel;
    else
      this.Label = "";
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    this.IsCurrent = true;
    if (this.FirstOnBecomeCurrent)
      playerFarming.indicator.PlayShake();
    this.FirstOnBecomeCurrent = false;
    AudioManager.Instance.PlayOneShot("event:/dialogue/corrupted_room/corrupted_vo");
  }

  public override void OnBecomeNotCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeNotCurrent(playerFarming);
    this.IsCurrent = false;
  }

  public override void Update()
  {
    base.Update();
    if (this.IsCurrent && (double) (this.Timer += Time.deltaTime) > (double) this.TargetTime && this.Num >= 2 && this.Num <= 4)
    {
      this.Timer = 0.0f;
      ++this.Num;
      this.UpdateLocalisation();
      switch (this.Num)
      {
        case 2:
        case 3:
        case 4:
        case 7:
          this.playerFarming.indicator.PlayShake();
          break;
      }
    }
    float num = Mathf.Clamp01(PlayerFarming.Instance.transform.position.y / 9f);
    foreach (FMODLoopSound loopingSound in this.loopingSounds)
      AudioManager.Instance.SetEventInstanceParameter(loopingSound.LoopedSound, "specific_filter", num);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.Num < 7)
    {
      if (this.Num <= -1)
      {
        this.SetCamera1.Play();
        state.CURRENT_STATE = StateMachine.State.CustomAnimation;
        Interactor.Overriding = true;
        PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/goat-ritual-start", 0, false);
        PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("rituals/goat-ritual-loop", 0, true, 0.0f);
        PlayerFarming.Instance.transform.DOMove(this.transform.position + new Vector3(0.0f, -0.5f), 0.5f);
      }
      ++this.Num;
      this.TargetTime = 0.0f;
      this.HasChanged = true;
      this.UpdateLocalisation();
      this.transform.DOShakePosition(0.2f, 0.2f, 100);
      AudioManager.Instance.PlayOneShot("event:/dialogue/corrupted_room/corrupted_vo");
    }
    else
      this.StartCoroutine((IEnumerator) this.GiveCorruptedTarotsAndRelics());
  }

  public IEnumerator GiveCorruptedTarotsAndRelics()
  {
    Interaction_CorruptedMonolith corruptedMonolith = this;
    corruptedMonolith.SetCamera2.Play();
    Interactor.Overriding = false;
    yield return (object) new WaitForSeconds(1f);
    CameraManager.instance.ShakeCameraForDuration(0.8f, 1.5f, 9999999f);
    corruptedMonolith.ActivateLighting.SetActive(true);
    corruptedMonolith.CorruptedLighting.SetActive(false);
    corruptedMonolith.RedStuff.SetActive(true);
    corruptedMonolith.PurpleStuff.SetActive(false);
    BiomeConstants.Instance.ImpactFrameForDuration();
    AudioManager.Instance.PlayOneShot("event:/player/death_hit", corruptedMonolith.playerFarming.gameObject);
    AudioManager.Instance.PlayOneShot("event:/comic sfx/impact", corruptedMonolith.playerFarming.gameObject);
    BiomeConstants.Instance.CoOpScreenEffect();
    AudioManager.Instance.PlayMusic("event:/music/corrupted_room/corrupted_room_red");
    AudioManager.Instance.PlayAtmos("event:/atmos/corrupted/corrupted_bubbles_smoke");
    corruptedMonolith.Tent1.AnimationState.SetAnimation(0, "wiggle", true);
    corruptedMonolith.Tent2.AnimationState.SetAnimation(0, "wiggle", true);
    corruptedMonolith.Tent3.AnimationState.SetAnimation(0, "wiggle", true);
    corruptedMonolith.Tent4.AnimationState.SetAnimation(0, "wiggle", true);
    corruptedMonolith.Tent5.AnimationState.SetAnimation(0, "wiggle", true);
    corruptedMonolith.Tent6.AnimationState.SetAnimation(0, "wiggle", true);
    corruptedMonolith.Tent7.AnimationState.SetAnimation(0, "wiggle", true);
    corruptedMonolith.Tent8.AnimationState.SetAnimation(0, "wiggle", true);
    corruptedMonolith.Tent9.AnimationState.SetAnimation(0, "wiggle", true);
    corruptedMonolith.Tent10.AnimationState.SetAnimation(0, "wiggle", true);
    corruptedMonolith.Tent11.AnimationState.SetAnimation(0, "wiggle", true);
    corruptedMonolith.Tent12.AnimationState.SetAnimation(0, "wiggle", true);
    corruptedMonolith.Tent13.AnimationState.SetAnimation(0, "wiggle", true);
    corruptedMonolith.Tent14.AnimationState.SetAnimation(0, "wiggle", true);
    corruptedMonolith.Tent15.AnimationState.SetAnimation(0, "wiggle", true);
    yield return (object) new WaitForSeconds(1.5f);
    DataManager.Instance.UnlockedCorruptedRelicsAndTarots = true;
    for (int index = 0; index < 5; ++index)
      SoulCustomTarget.Create(corruptedMonolith.playerFarming.gameObject, PlayerFarming.Instance.transform.position - Vector3.forward, Color.black, (System.Action) null, 0.2f);
    yield return (object) new WaitForSeconds(2f);
    UITarotCardsMenuController menu1 = MonoSingleton<UIManager>.Instance.TarotCardsMenuTemplate.Instantiate<UITarotCardsMenuController>();
    menu1.Show(TarotCards.CorruptedCards);
    yield return (object) menu1.YieldUntilHidden();
    UIRelicMenuController menu2 = MonoSingleton<UIManager>.Instance.RelicMenuTemplate.Instantiate<UIRelicMenuController>();
    List<RelicType> relicTypes = new List<RelicType>();
    foreach (RelicData relicData in EquipmentManager.RelicData)
    {
      if (relicData.RelicSubType == RelicSubType.Corrupted)
        relicTypes.Add(relicData.RelicType);
    }
    EquipmentManager.UnlockRelics(UpgradeSystem.Type.Relic_Pack_Corrupted);
    menu2.Show(relicTypes);
    yield return (object) menu2.YieldUntilHidden();
    System.Action corruptedComplete = Interaction_CorruptedMonolith.OnCorruptedComplete;
    if (corruptedComplete != null)
      corruptedComplete();
    CameraManager.instance.Stopshake();
    corruptedMonolith.SetCamera2.Reset();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/goat-ritual-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.333333343f);
    AudioManager.Instance.StopCurrentAtmos();
    RespawnRoomManager.Instance.PlayRespawn();
  }
}
