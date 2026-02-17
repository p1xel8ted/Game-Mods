// Decompiled with JetBrains decompiler
// Type: Interaction_DLCShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.BuildMenu;
using Lamb.UI.Menus.DoctrineChoicesMenu;
using MMTools;
using Spine.Unity;
using src.Extensions;
using src.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_DLCShrine : Interaction
{
  public static Interaction_DLCShrine Instance;
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public GameObject beam;
  [SerializeField]
  public GameObject rotBeam;
  [SerializeField]
  public GameObject cameraBone;
  [SerializeField]
  public SkeletonAnimation moleSpine;
  [SerializeField]
  public Interaction_SimpleConversation moleIntroConvo;
  [SerializeField]
  public GameObject redLighting;
  [SerializeField]
  public SpriteRenderer blueOutline;
  [SerializeField]
  public Interaction_SimpleConversation haroConvo;
  [SerializeField]
  public Interaction_SimpleConversation haroRitualGiftConvo;
  [SerializeField]
  public SkeletonAnimation spineProgressBar;
  [SerializeField]
  public GrenadeBullet grenadeBullet;
  [SerializeField]
  public GameObject woolhavenDLCName;
  [SerializeField]
  public GameObject teleporter;
  public bool IN_DUNGEON;
  public List<InventoryItem> Offerings = new List<InventoryItem>()
  {
    new InventoryItem(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, 5),
    new InventoryItem(InventoryItem.ITEM_TYPE.CRYSTAL, 5),
    new InventoryItem(InventoryItem.ITEM_TYPE.SPIDER_WEB, 5),
    new InventoryItem(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1)
  };
  public static List<StructureBrain.TYPES> Decorations = new List<StructureBrain.TYPES>()
  {
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_SMALLSTATUE1,
    StructureBrain.TYPES.DECORATION_DLC_WOOLHAVEN_STREETLAMP,
    StructureBrain.TYPES.DECORATION_DLC_YNGYA_CANDLE,
    StructureBrain.TYPES.DECORATION_DLC_YNGYA_FLOWERBUCKET
  };
  public List<TarotCards.Card> tarots = new List<TarotCards.Card>()
  {
    TarotCards.Card.MutatedResurrectFullHealth,
    TarotCards.Card.MutatedDropRotburn,
    TarotCards.Card.MutatedFreezeOnHit,
    TarotCards.Card.MutatedNegateHit,
    TarotCards.Card.MutatedInvincibility,
    TarotCards.Card.MutatedSpawnRotDemons
  };
  public List<int> requiredRottingFollowers = new List<int>()
  {
    1,
    3,
    4,
    5,
    6,
    7
  };
  public int[] playerAnimsSet = new int[2];
  public EventInstance rotBeamLoopInstance;
  public string rotBeamLoopSFX = "event:/dlc/env/yngya_shrine/rotten_amb_loop";

  public SkeletonAnimation Spine => this.spine;

  public void Awake()
  {
    this.woolhavenDLCName.transform.localScale = Vector3.zero;
    Interaction_DLCShrine.Instance = this;
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    Interaction_DLCShrine.Instance = (Interaction_DLCShrine) null;
    if (!(bool) (UnityEngine.Object) GameManager.GetInstance())
      return;
    GameManager.GetInstance().OnDLCsAuthenticated += new System.Action(this.OnDLCsAuthenticated);
  }

  public void Start()
  {
    GameManager.GetInstance().OnDLCsAuthenticated += new System.Action(this.OnDLCsAuthenticated);
    this.spineProgressBar.gameObject.SetActive(DataManager.Instance.YngyaOffering != -2);
    if (!DataManager.Instance.DisableYngyaShrine && DataManager.Instance.RevealedBaseYngyaShrine)
      return;
    this.gameObject.SetActive(false);
  }

  public void OnDLCsAuthenticated()
  {
    this.Interactable = DataManager.Instance.MAJOR_DLC;
    if (this.IN_DUNGEON)
    {
      this.spine.AnimationState.SetAnimation(0, SeasonsManager.WinterSeverity.ToString(), true);
      this.teleporter.gameObject.SetActive(false);
    }
    else if (DataManager.Instance.YngyaOffering == -1)
    {
      this.Interactable = false;
      this.spineProgressBar.state.SetAnimation(0, "3_activated", true);
    }
    else
    {
      int num = Mathf.Clamp(DataManager.Instance.YngyaOffering, 0, 3);
      if ((UnityEngine.Object) this.spineProgressBar != (UnityEngine.Object) null)
        this.spineProgressBar.state.SetAnimation(0, num.ToString(), false);
    }
    this.UpdateState();
  }

  public void UpdateState()
  {
    if ((DataManager.Instance.YngyaOffering == -1 || DataManager.Instance.YngyaOffering == -3) && !this.IN_DUNGEON)
      this.gameObject.SetActive(false);
    else if (DataManager.Instance.YngyaOffering == -2)
    {
      this.spine.Skeleton.SetSkin("rot");
      this.spineProgressBar.Skeleton.SetSkin("base_rot");
    }
    else
    {
      if (this.IN_DUNGEON)
        return;
      if (DataManager.Instance.YngyaOffering >= 3 && DataManager.Instance.BossesCompleted.Count < 4)
      {
        this.spine.state.SetAnimation(0, "idle-eyesoff", true);
        this.blueOutline.gameObject.SetActive(false);
      }
      else if (DataManager.Instance.YngyaOffering == -2)
      {
        int num = 0;
        int requiredRottingFollower = this.requiredRottingFollowers[DataManager.Instance.YngyaRotOfferingsReceived];
        for (int index = 0; index < DataManager.Instance.Followers.Count; ++index)
        {
          if (DataManager.Instance.Followers[index].Traits.Contains(FollowerTrait.TraitType.Mutated))
            ++num;
        }
        this.spine.state.SetAnimation(0, "idle-eyeson", num >= requiredRottingFollower);
        this.blueOutline.gameObject.SetActive(num >= requiredRottingFollower);
      }
      else if (DataManager.Instance.YngyaOffering != -1 && (DataManager.Instance.YngyaOffering >= 4 || Inventory.GetItemQuantity((InventoryItem.ITEM_TYPE) this.Offerings[DataManager.Instance.YngyaOffering].type) >= this.Offerings[DataManager.Instance.YngyaOffering].quantity))
      {
        this.spine.state.SetAnimation(0, "idle-eyeson", true);
        this.blueOutline.gameObject.SetActive(true);
      }
      else
      {
        this.spine.state.SetAnimation(0, "idle-eyesoff", true);
        this.blueOutline.gameObject.SetActive(false);
      }
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    FollowerManager.OnFollowerDie += new FollowerManager.FollowerGoneEvent(this.OnFollowerDied);
    FollowerRecruit.OnRecruitFinalised += new FollowerRecruit.FollowerEventDelegate(this.CheckRotBeam);
    this.UpdateState();
    this.CheckRotBeam();
  }

  public new void OnDisable()
  {
    FollowerManager.OnFollowerDie -= new FollowerManager.FollowerGoneEvent(this.OnFollowerDied);
    FollowerRecruit.OnRecruitFinalised -= new FollowerRecruit.FollowerEventDelegate(this.CheckRotBeam);
    AudioManager.Instance.StopLoop(this.rotBeamLoopInstance);
    for (int index = 0; index < PlayerFarming.players.Count; ++index)
    {
      if (this.playerAnimsSet[index] != 0)
      {
        this.playerAnimsSet[index] = 0;
        PlayerFarming.players[index].playerController.ResetSpecificMovementAnimations();
      }
    }
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (this.IN_DUNGEON)
      this.Label = this.Interactable ? LocalizationManager.GetTranslation("Interactions/YngyaShrine") : "";
    else if (DataManager.Instance.YngyaOffering == -2)
    {
      this.Interactable = true;
      this.Label = ScriptLocalization.FollowerInteractions.MakeDemand;
      int num = 0;
      int requiredRottingFollower = this.requiredRottingFollowers[DataManager.Instance.YngyaRotOfferingsReceived];
      for (int index = 0; index < DataManager.Instance.Followers.Count; ++index)
      {
        if (DataManager.Instance.Followers[index].Traits.Contains(FollowerTrait.TraitType.Mutated))
          ++num;
      }
      string requires = ScriptLocalization.Interactions.Requires;
      if (num >= requiredRottingFollower)
        this.Label = ScriptLocalization.FollowerInteractions.MakeDemand;
      else if (LocalizeIntegration.IsArabic())
        this.Label = $"{requires} ){requiredRottingFollower.ToString()} / <color=red> {num.ToString()}</color>(  <sprite name=\"icon_Trait_Mutated\">{FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.FOLLOWERS)}";
      else
        this.Label = $"{requires} (<color=red> {num.ToString()}</color> / {requiredRottingFollower.ToString()})  <sprite name=\"icon_Trait_Mutated\">{FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.FOLLOWERS)}";
    }
    else if (DataManager.Instance.YngyaOffering >= 3 && DataManager.Instance.BossesCompleted.Count < 4)
    {
      this.Interactable = false;
      this.Label = LocalizationManager.GetTranslation("Interactions/RequiresBishopsDefeated");
    }
    else if (DataManager.Instance.YngyaOffering >= 4)
      this.Label = this.Interactable ? LocalizationManager.GetTranslation("Interactions/FinalYngyaOffering") : "";
    else if (this.Interactable)
    {
      string str;
      if (DataManager.Instance.YngyaOffering >= 4)
        str = "";
      else
        str = $"{LocalizationManager.GetTranslation("Interactions/GiveOffering")}{InventoryItem.CapacityString((InventoryItem.ITEM_TYPE) this.Offerings[DataManager.Instance.YngyaOffering].type, this.Offerings[DataManager.Instance.YngyaOffering].quantity)}{StaticColors.GreyColorHex} | ({DataManager.Instance.YngyaOffering.ToString()}/4) ";
      this.Label = str;
    }
    else
      this.Label = $"{ScriptLocalization.Interactions.Requires} {ScriptLocalization.UI.WoolhavenDLC.Colour(StaticColors.YellowColorHex)}";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (DataManager.Instance.YngyaOffering == -2)
    {
      int num = 0;
      int requiredRottingFollower = this.requiredRottingFollowers[DataManager.Instance.YngyaRotOfferingsReceived];
      for (int index = 0; index < DataManager.Instance.Followers.Count; ++index)
      {
        if (DataManager.Instance.Followers[index].Traits.Contains(FollowerTrait.TraitType.Mutated))
          ++num;
      }
      if (num < requiredRottingFollower)
      {
        this.IndicateHighlighted(this.playerFarming);
        AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.transform.position);
        this.playerFarming.indicator.PlayShake();
      }
      else
        this.StartCoroutine((IEnumerator) this.MutatedFollowersRitualIE());
    }
    else if (DataManager.Instance.YngyaOffering >= 4 || Inventory.GetItemQuantity((InventoryItem.ITEM_TYPE) this.Offerings[DataManager.Instance.YngyaOffering].type) >= this.Offerings[DataManager.Instance.YngyaOffering].quantity)
    {
      this.StartCoroutine((IEnumerator) this.GiveOfferingIE());
    }
    else
    {
      this.IndicateHighlighted(this.playerFarming);
      AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.transform.position);
      this.playerFarming.indicator.PlayShake();
    }
  }

  public override void IndicateHighlighted(PlayerFarming playerFarming = null)
  {
    base.IndicateHighlighted(playerFarming);
    if (DataManager.Instance.YngyaOffering < 0)
      return;
    this.woolhavenDLCName.transform.DOKill();
    this.woolhavenDLCName.transform.DOScale(3f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
  }

  public override void EndIndicateHighlighted(PlayerFarming playerFarming = null)
  {
    base.EndIndicateHighlighted(playerFarming);
    this.woolhavenDLCName.transform.DOKill();
    this.woolhavenDLCName.transform.DOScale(0.0f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
  }

  public IEnumerator GiveOfferingIE()
  {
    Interaction_DLCShrine interactionDlcShrine = this;
    if (DataManager.Instance.YngyaOffering >= 3)
    {
      AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/base_building_giveoffering_final", interactionDlcShrine.transform.position);
      AudioManager.Instance.PlayOneShot("event:/dlc/music/yngya_shrine/base_building_giveoffering_final");
    }
    else
    {
      AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/base_building_giveoffering_basic", interactionDlcShrine.transform.position);
      AudioManager.Instance.PlayOneShot("event:/dlc/music/yngya_shrine/base_building_giveoffering_basic");
    }
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionDlcShrine.gameObject, 7f);
    if (DataManager.Instance.YngyaOffering < 4)
    {
      for (int i = 0; i < interactionDlcShrine.Offerings[DataManager.Instance.YngyaOffering].quantity; ++i)
      {
        ResourceCustomTarget.Create(interactionDlcShrine.gameObject, interactionDlcShrine.playerFarming.transform.position, (InventoryItem.ITEM_TYPE) interactionDlcShrine.Offerings[DataManager.Instance.YngyaOffering].type, (System.Action) null);
        yield return (object) new WaitForSeconds(0.1f);
      }
    }
    PlayerFarming.Instance.transform.DOMove(interactionDlcShrine.transform.position + new Vector3(0.0f, -1f), 1f);
    interactionDlcShrine.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) null;
    if (DataManager.Instance.YngyaOffering >= 3)
    {
      interactionDlcShrine.spine.state.SetAnimation(0, "ring-final", false);
      interactionDlcShrine.spine.state.AddAnimation(0, "ring-final-loop", true, 0.0f);
      PlayerFarming.Instance.simpleSpineAnimator.Animate("Yngya_Calls/yngya-calls", 0, false);
      PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("Yngya_Calls/yngya-calls-loop", 0, true, 0.0f);
      interactionDlcShrine.StartCoroutine((IEnumerator) interactionDlcShrine.ShakeCameraWithRampUp());
      yield return (object) new WaitForSeconds(2f);
      BiomeConstants.Instance.EmitDisplacementEffectScale(PlayerFarming.Instance.transform.position, 0.25f);
      yield return (object) new WaitForSeconds(1f);
    }
    else
    {
      PlayerFarming.Instance.simpleSpineAnimator.Animate("collect-ghosts", 0, false);
      PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("collect-ghosts-loop", 0, true, 0.0f);
      interactionDlcShrine.StartCoroutine((IEnumerator) interactionDlcShrine.ShakeCameraWithRampUp());
      yield return (object) new WaitForSeconds(2f);
      yield return (object) new WaitForSeconds(1f);
    }
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 2f);
    interactionDlcShrine.beam.gameObject.SetActive(true);
    if (!interactionDlcShrine.IN_DUNGEON)
    {
      int num = Mathf.Clamp(DataManager.Instance.YngyaOffering, 0, 3);
      if ((UnityEngine.Object) interactionDlcShrine.spineProgressBar != (UnityEngine.Object) null)
      {
        interactionDlcShrine.spineProgressBar.state.SetAnimation(0, num.ToString() + "_activate", false);
        interactionDlcShrine.spineProgressBar.state.AddAnimation(0, num.ToString() + "_activated", true, 0.0f);
      }
      interactionDlcShrine.spine.state.SetAnimation(0, "ring", false);
      interactionDlcShrine.spine.state.AddAnimation(0, "idle-eyeson", true, 0.0f);
    }
    float startValue = 0.0f;
    DOTween.To((DOGetter<float>) (() => startValue), (DOSetter<float>) (x => BiomeConstants.Instance.SetIceOverlayReveal(x)), 1f, 0.3f);
    GameManager.GetInstance().OnConversationNext(interactionDlcShrine.gameObject, 11f);
    yield return (object) new WaitForSeconds(3f);
    if (DataManager.Instance.YngyaOffering <= 2)
    {
      interactionDlcShrine.playerFarming.Spine.AnimationState.SetAnimation(0, "collect-ghosts-land", false);
      interactionDlcShrine.playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      interactionDlcShrine.playerFarming.state.facingAngle = 90f;
      interactionDlcShrine.beam.gameObject.SetActive(false);
    }
    if (!interactionDlcShrine.IN_DUNGEON)
    {
      interactionDlcShrine.spine.state.SetAnimation(0, "idle-eyesoff", true);
      interactionDlcShrine.blueOutline.gameObject.SetActive(false);
    }
    yield return (object) new WaitForSeconds(0.5f);
    startValue = 1f;
    DOTween.To((DOGetter<float>) (() => startValue), (DOSetter<float>) (x => BiomeConstants.Instance.SetIceOverlayReveal(x)), 0.0f, 0.5f);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNext(interactionDlcShrine.gameObject, 7f);
    yield return (object) new WaitForSeconds(0.5f);
    if (DataManager.Instance.YngyaOffering < 4)
    {
      Inventory.ChangeItemQuantity((InventoryItem.ITEM_TYPE) interactionDlcShrine.Offerings[DataManager.Instance.YngyaOffering].type, -interactionDlcShrine.Offerings[DataManager.Instance.YngyaOffering].quantity);
      ++DataManager.Instance.YngyaOffering;
    }
    if (DataManager.Instance.YngyaOffering == 1)
      yield return (object) interactionDlcShrine.StartCoroutine((IEnumerator) interactionDlcShrine.UnlockSnowIE());
    else if (DataManager.Instance.YngyaOffering == 2)
      yield return (object) interactionDlcShrine.StartCoroutine((IEnumerator) interactionDlcShrine.UnlockBaseExpansionIE());
    else if (DataManager.Instance.YngyaOffering == 3)
    {
      yield return (object) interactionDlcShrine.StartCoroutine((IEnumerator) interactionDlcShrine.UnlockRitualIE());
    }
    else
    {
      string Term = "UI/Popups/Error/ActivatingDLCWarning";
      if (SaveAndLoad.SAVE_SLOT >= 10 && SaveAndLoad.SaveExist(SaveAndLoad.SAVE_SLOT - 10))
        Term = "UI/Popups/Error/ActivatingDLCWarning_Alt";
      UIMenuConfirmationWindow errorWindow = MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate.Instantiate<UIMenuConfirmationWindow>();
      errorWindow.Configure(LocalizationManager.GetTranslation("UI/Popups/Warning/GenericHeader"), LocalizationManager.GetTranslation(Term));
      errorWindow.Show();
      errorWindow.Canvas.sortingOrder = 1000;
      errorWindow.OnConfirm += (System.Action) (() =>
      {
        SaveAndLoad.MakeBaseGameBackUpSave((System.Action) null);
        AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/woolhaven_dlc_activate", this.transform.position);
        this.playerFarming.Spine.AnimationState.SetAnimation(0, "Yngya_Calls/yngya-calls-rise", false);
        GameManager.GetInstance().OnConversationNext(this.gameObject, 6f);
        CameraManager.instance.ShakeCameraForDuration(1f, 2f, 5f);
        this.spine.transform.DOMoveZ(3.5f, 5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutCirc);
        this.transform.DOShakePosition(5f, new Vector3(0.25f, 0.0f, 0.0f)).SetUpdate<Tweener>(true);
        MMVibrate.Rumble(0.0f, 2f, 5f, (MonoBehaviour) GameManager.GetInstance());
        AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/base_building_descend", this.transform.position);
        MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Woolhaven Intro", 10f, "", (System.Action) (() =>
        {
          DataManager.Instance.YngyaOffering = -1;
          DataManager.Instance.MetaData.ActivatedMajorDLC = true;
        }));
        if ((bool) (UnityEngine.Object) CoopManager.Instance)
          CoopManager.Instance.LockAddRemovePlayer();
        errorWindow.Hide();
      });
      errorWindow.OnCancel += (System.Action) (() =>
      {
        this.spineProgressBar.state.SetAnimation(0, "2_activated", true);
        CoopManager.Instance.UnlockAddRemovePlayer();
        GameManager.GetInstance().OnConversationEnd();
        this.beam.gameObject.SetActive(false);
      });
    }
    interactionDlcShrine.UpdateState();
  }

  public void SetRotBeamActive(bool setActive)
  {
    bool activeSelf = this.rotBeam.gameObject.activeSelf;
    this.rotBeam.gameObject.SetActive(setActive);
    if (setActive)
    {
      AudioManager.Instance.StopLoop(this.rotBeamLoopInstance);
      this.rotBeamLoopInstance = AudioManager.Instance.CreateLoop(this.rotBeamLoopSFX, this.gameObject, true);
    }
    else
    {
      if (!activeSelf || setActive)
        return;
      AudioManager.Instance.StopLoop(this.rotBeamLoopInstance);
    }
  }

  public void CheckRotBeam(FollowerInfo info = null)
  {
    if (DataManager.Instance.YngyaOffering != -2)
      return;
    int num = 0;
    int requiredRottingFollower = this.requiredRottingFollowers[Mathf.Min(DataManager.Instance.YngyaRotOfferingsReceived, this.requiredRottingFollowers.Count - 1)];
    for (int index = 0; index < DataManager.Instance.Followers.Count; ++index)
    {
      if (DataManager.Instance.Followers[index].Traits.Contains(FollowerTrait.TraitType.Mutated))
        ++num;
    }
    this.SetRotBeamActive(num >= requiredRottingFollower);
    this.blueOutline.gameObject.SetActive(num >= requiredRottingFollower);
    if (num >= requiredRottingFollower)
      this.spine.state.SetAnimation(0, "idle-eyeson", true);
    else
      this.spine.state.SetAnimation(0, "idle-eyesoff", true);
  }

  public override void Update()
  {
    base.Update();
    if (!DataManager.Instance.MAJOR_DLC || SeasonsManager.Active || LetterBox.IsPlaying)
      return;
    for (int index = 0; index < PlayerFarming.players.Count; ++index)
    {
      if ((double) Vector3.Distance(PlayerFarming.players[index].transform.position, this.transform.position) < 7.0 && (double) PlayerFarming.players[index].transform.position.y < (double) this.transform.position.y && PlayerFarming.Location == FollowerLocation.Base)
      {
        if (!PlayerFarming.players[index].CarryingEgg && PlayerFarming.players[index].CarryingDeadFollowerID == -1)
        {
          if ((double) this.transform.position.x > (double) PlayerFarming.players[index].transform.position.x)
          {
            if (this.GetAngleDirectionFull(PlayerFarming.players[index].state.facingAngle) == Utils.DirectionFull.Left && this.playerAnimsSet[index] != 1)
            {
              this.playerAnimsSet[index] = 1;
              PlayerFarming.players[index].playerController.SetSpecificMovementAnimations("Yngya_Calls/idle-called-back", "Yngya_Calls/run-up-called-right", "Yngya_Calls/run-down-center-right", "Yngya_Calls/run-called-front", "Yngya_Calls/run-up-diagonal-called-back", "Yngya_Calls/run-horizontal-called-back");
            }
            else if (this.GetAngleDirectionFull(PlayerFarming.players[index].state.facingAngle) != Utils.DirectionFull.Left && this.playerAnimsSet[index] != -1)
            {
              this.playerAnimsSet[index] = -1;
              PlayerFarming.players[index].playerController.SetSpecificMovementAnimations("Yngya_Calls/idle-called-front", "Yngya_Calls/run-up-called-left", "Yngya_Calls/run-down-center-left", "Yngya_Calls/run-called-back", "Yngya_Calls/run-up-diagonal-called-front", "Yngya_Calls/run-horizontal-called-front");
            }
          }
          else if ((double) this.transform.position.x < (double) PlayerFarming.players[index].transform.position.x)
          {
            if (this.GetAngleDirectionFull(PlayerFarming.players[index].state.facingAngle) == Utils.DirectionFull.Left && this.playerAnimsSet[index] != -1)
            {
              this.playerAnimsSet[index] = -1;
              PlayerFarming.players[index].playerController.SetSpecificMovementAnimations("Yngya_Calls/idle-called-front", "Yngya_Calls/run-up-called-left", "Yngya_Calls/run-down-center-left", "Yngya_Calls/run-called-front", "Yngya_Calls/run-up-diagonal-called-back", "Yngya_Calls/run-horizontal-called-front");
            }
            else if (this.GetAngleDirectionFull(PlayerFarming.players[index].state.facingAngle) != Utils.DirectionFull.Left && this.playerAnimsSet[index] != 1)
            {
              this.playerAnimsSet[index] = 1;
              PlayerFarming.players[index].playerController.SetSpecificMovementAnimations("Yngya_Calls/idle-called-back", "Yngya_Calls/run-up-called-right", "Yngya_Calls/run-down-center-right", "Yngya_Calls/run-called-back", "Yngya_Calls/run-up-diagonal-called-front", "Yngya_Calls/run-horizontal-called-back");
            }
          }
        }
      }
      else if (this.playerAnimsSet[index] != 0)
      {
        this.playerAnimsSet[index] = 0;
        PlayerFarming.players[index].playerController.ResetSpecificMovementAnimations();
      }
    }
  }

  public Utils.DirectionFull GetAngleDirectionFull(float Angle)
  {
    Angle = Utils.Repeat(Angle, 360f);
    if ((double) Angle > 112.5 && (double) Angle < 157.5 || (double) Angle > 22.5 && (double) Angle < 67.5)
      return Utils.DirectionFull.Up;
    if ((double) Angle > 202.5 && (double) Angle < 247.5 || (double) Angle > 292.5 && (double) Angle < 337.5)
      return Utils.DirectionFull.Down;
    if ((double) Angle >= 67.5 && (double) Angle <= 112.5)
      return Utils.DirectionFull.Up;
    if ((double) Angle >= 247.5 && (double) Angle <= 292.5)
      return Utils.DirectionFull.Down;
    return (double) Angle >= 337.5 && (double) Angle <= 22.5 || (double) Angle < 157.5 || (double) Angle > 202.5 ? Utils.DirectionFull.Right : Utils.DirectionFull.Left;
  }

  public IEnumerator UnlockBaseExpansionIE()
  {
    Interaction_DLCShrine interactionDlcShrine = this;
    interactionDlcShrine.moleSpine.gameObject.SetActive(true);
    interactionDlcShrine.moleSpine.AnimationState.SetAnimation(0, "dig_up", false);
    interactionDlcShrine.moleSpine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/gofernon/burrow_in", interactionDlcShrine.moleSpine.transform.position);
    yield return (object) new WaitForSeconds(1f);
    interactionDlcShrine.moleIntroConvo.Play();
    yield return (object) new WaitForEndOfFrame();
    while (MMConversation.isPlaying)
      yield return (object) null;
    interactionDlcShrine.moleSpine.AnimationState.SetAnimation(0, "dig_down", false);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/gofernon/burrow_out", interactionDlcShrine.moleSpine.transform.position);
    yield return (object) new WaitForSeconds(1f);
    interactionDlcShrine.moleSpine.gameObject.SetActive(false);
    yield return (object) interactionDlcShrine.StartCoroutine((IEnumerator) Onboarding.Instance.OnboardBaseExpansionIE());
  }

  public IEnumerator UnlockSnowIE()
  {
    Interaction_DLCShrine interactionDlcShrine = this;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
    yield return (object) GameManager.GetInstance().StartCoroutine((IEnumerator) Interaction_DLCShrine.FadeIn());
    GameManager.SetGlobalOcclusionActive(false);
    Vector3 a = TownCentre.Instance.Centre.position + Vector3.down * 2f;
    SimulationManager.Pause();
    List<Follower> avaiableFollowers = new List<Follower>();
    foreach (Follower follower in Follower.Followers)
    {
      if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID, true, excludeFreezing: true))
      {
        avaiableFollowers.Add(follower);
        follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
        follower.HideAllFollowerIcons();
        Vector3 normalized = (a - follower.transform.position).normalized;
        if ((double) Vector3.Distance(a, follower.transform.position) < 3.0)
          follower.transform.position += (double) normalized.x < 0.0 ? Vector3.left * 3f : Vector3.right * 3f;
      }
    }
    WeatherSystemController.Instance.StopCurrentWeather(0.0f);
    WeatherSystemController.Instance.GetParticleSystem(WeatherSystemController.WeatherType.Snowing).transform.localPosition = new Vector3(0.0f, 2f, 2f);
    ParticleSystem.MainModule m = WeatherSystemController.Instance.GetParticleSystem(WeatherSystemController.WeatherType.Snowing).main;
    ParticleSystem.EmissionModule emission = WeatherSystemController.Instance.GetParticleSystem(WeatherSystemController.WeatherType.Snowing).emission with
    {
      enabled = true,
      rateOverTime = (ParticleSystem.MinMaxCurve) 100f
    };
    m.simulationSpace = ParticleSystemSimulationSpace.Local;
    m.useUnscaledTime = true;
    Follower target = (Follower) null;
    Vector3 fromPosition = Vector3.zero;
    if (avaiableFollowers.Count > 0)
    {
      target = avaiableFollowers[UnityEngine.Random.Range(0, avaiableFollowers.Count)];
      target.transform.position = a;
      GameManager.GetInstance().CamFollowTarget.ResetTargetCamera(0.0f);
      yield return (object) new WaitForEndOfFrame();
      fromPosition = GameManager.GetInstance().CamFollowTarget.transform.position;
      BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 0.0f, 0.0f);
      GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
      GameManager.GetInstance().CamFollowTarget.ForceSnapTo(target.transform.position + new Vector3(0.0f, -1.5f, -1.5f) - Vector3.forward);
      GameManager.GetInstance().CamFollowTarget.transform.localRotation = Quaternion.Euler(-45f, 0.0f, 0.0f);
    }
    yield return (object) new WaitForSecondsRealtime(0.5f);
    yield return (object) GameManager.GetInstance().StartCoroutine((IEnumerator) Interaction_DLCShrine.FadeOut(true));
    foreach (Follower follower in avaiableFollowers)
    {
      if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID))
      {
        string animationName = "Snow/look-up-happy";
        float num = UnityEngine.Random.value;
        if ((double) num < 0.20000000298023224)
          animationName = "Snow/look-up-wonder";
        else if ((double) num < 0.40000000596046448)
          animationName = "Snow/look-up-worried";
        follower.Spine.AnimationState.SetAnimation(1, animationName, false);
        if ((UnityEngine.Object) follower != (UnityEngine.Object) target)
        {
          follower.Spine.AnimationState.GetCurrent(1).TrackTime = UnityEngine.Random.Range(0.0f, 2f);
          AudioManager.Instance.PlayOneShot("event:/dialogue/followers/admire", target.transform.position);
        }
        follower.Spine.AnimationState.AddAnimation(1, "Snow/idle", true, 0.0f);
      }
    }
    WeatherSystemController.Instance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Dusting);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/base_building_giveoffering_first_frost", interactionDlcShrine.transform.position);
    yield return (object) new WaitForSeconds(3f);
    yield return (object) GameManager.GetInstance().StartCoroutine((IEnumerator) Interaction_DLCShrine.FadeIn());
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 0.0f, 0.0f);
    GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
    GameManager.GetInstance().CamFollowTarget.ForceSnapTo(fromPosition);
    GameManager.GetInstance().CamFollowTarget.transform.localRotation = Quaternion.Euler(-45f, 0.0f, 0.0f);
    m.simulationSpace = ParticleSystemSimulationSpace.World;
    m.useUnscaledTime = false;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    foreach (Follower follower in avaiableFollowers)
    {
      if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID))
        follower.Brain.CompleteCurrentTask();
    }
    yield return (object) GameManager.GetInstance().StartCoroutine((IEnumerator) Interaction_DLCShrine.FadeOut(true));
    GameManager.SetGlobalOcclusionActive(true);
    GameManager.GetInstance().OnConversationEnd(false);
    interactionDlcShrine.haroConvo.gameObject.SetActive(true);
    interactionDlcShrine.haroConvo.Play(interactionDlcShrine.playerFarming.gameObject);
    yield return (object) new WaitForEndOfFrame();
    while (LetterBox.IsPlaying)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionDlcShrine.playerFarming.gameObject);
    List<GameObject> decos = new List<GameObject>();
    for (int i = 0; i < Interaction_DLCShrine.Decorations.Count; ++i)
    {
      AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionDlcShrine.transform.position);
      FoundItemPickUp component = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION, 1, interactionDlcShrine.haroConvo.transform.position).GetComponent<FoundItemPickUp>();
      component.DecorationType = Interaction_DLCShrine.Decorations[i];
      decos.Add(component.gameObject);
      yield return (object) new WaitForSeconds(0.1f);
    }
    yield return (object) new WaitForSeconds(1f);
    foreach (GameObject gameObject in decos)
    {
      GameObject deco = gameObject;
      deco.transform.DOMove(interactionDlcShrine.playerFarming.transform.position, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) deco.gameObject)));
    }
    yield return (object) new WaitForSeconds(0.5f);
    for (int index = 0; index < Interaction_DLCShrine.Decorations.Count; ++index)
    {
      StructuresData.CompleteResearch(Interaction_DLCShrine.Decorations[index]);
      StructuresData.SetRevealed(Interaction_DLCShrine.Decorations[index]);
    }
    bool wait = true;
    UIBuildMenuController buildMenuController = MonoSingleton<UIManager>.Instance.BuildMenuTemplate.Instantiate<UIBuildMenuController>();
    buildMenuController.Show(Interaction_DLCShrine.Decorations);
    UIBuildMenuController buildMenuController1 = buildMenuController;
    buildMenuController1.OnHidden = buildMenuController1.OnHidden + (System.Action) (() =>
    {
      wait = false;
      buildMenuController = (UIBuildMenuController) null;
    });
    while (wait)
      yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
  }

  public IEnumerator UnlockRitualIE()
  {
    Interaction_DLCShrine interactionDlcShrine = this;
    MMConversation.CURRENT_CONVERSATION = new ConversationObject((List<ConversationEntry>) null, (List<MMTools.Response>) null, (System.Action) null, new List<DoctrineResponse>()
    {
      new DoctrineResponse(SermonCategory.Special, 6, true, (System.Action) null)
    });
    UIDoctrineChoicesMenuController doctrineChoicesInstance = MonoSingleton<UIManager>.Instance.DoctrineChoicesMenuTemplate.Instantiate<UIDoctrineChoicesMenuController>();
    doctrineChoicesInstance.Show();
    while (doctrineChoicesInstance.gameObject.activeInHierarchy)
      yield return (object) null;
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_FollowerWedding);
    GameManager.GetInstance().OnConversationEnd(false);
    interactionDlcShrine.haroRitualGiftConvo.gameObject.SetActive(true);
    interactionDlcShrine.haroRitualGiftConvo.Play(interactionDlcShrine.playerFarming.gameObject);
  }

  public IEnumerator ShakeCameraWithRampUp()
  {
    float t = 0.0f;
    while ((double) (t += Time.deltaTime) < 1.8999999761581421)
    {
      float t1 = t / 1.9f;
      CameraManager.instance.ShakeCameraForDuration(Mathf.Lerp(0.0f, 0.5f, t1), Mathf.Lerp(0.0f, 1.5f, t1), 3.9f, false);
      yield return (object) null;
    }
    CameraManager.instance.Stopshake();
  }

  public void DebugReveal() => this.StartCoroutine((IEnumerator) this.RevealIE());

  public IEnumerator RevealIE()
  {
    Interaction_DLCShrine interactionDlcShrine = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionDlcShrine.cameraBone, 6f);
    interactionDlcShrine.blueOutline.gameObject.SetActive(false);
    yield return (object) new WaitForSeconds(1f);
    interactionDlcShrine.spine.transform.position = new Vector3(interactionDlcShrine.spine.transform.position.x, interactionDlcShrine.spine.transform.position.y, 3.5f);
    interactionDlcShrine.gameObject.SetActive(true);
    interactionDlcShrine.spine.state.SetAnimation(0, "idle-eyesoff", true);
    CameraManager.instance.ShakeCameraForDuration(0.5f, 1f, 3f);
    interactionDlcShrine.spine.transform.DOMoveZ(0.0f, 3f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutCirc);
    interactionDlcShrine.transform.DOShakePosition(3f, new Vector3(0.25f, 0.0f, 0.0f)).SetUpdate<Tweener>(true);
    MMVibrate.Rumble(0.0f, 2f, 3f, (MonoBehaviour) GameManager.GetInstance());
    AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/base_building_ascend");
    yield return (object) new WaitForSeconds(2f);
    interactionDlcShrine.spine.state.SetAnimation(0, "ring", false);
    interactionDlcShrine.spine.state.AddAnimation(0, "idle-eyeson", true, 0.0f);
    BiomeConstants.Instance.EmitDisplacementEffect(interactionDlcShrine.transform.position);
    GameManager.GetInstance().OnConversationNext(interactionDlcShrine.cameraBone, 8f);
    yield return (object) new WaitForSeconds(2f);
    interactionDlcShrine.blueOutline.gameObject.SetActive(true);
    interactionDlcShrine.UpdateState();
    GameManager.GetInstance().OnConversationEnd();
    DataManager.Instance.RevealedBaseYngyaShrine = true;
  }

  public IEnumerator RotRevealIE()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_DLCShrine interactionDlcShrine = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    interactionDlcShrine.gameObject.SetActive(true);
    interactionDlcShrine.Spine.transform.localPosition = new Vector3(0.0f, -7f, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/rotten_ascend");
    CameraManager.instance.ShakeCameraForDuration(0.5f, 1f, 5f);
    interactionDlcShrine.Spine.transform.DOLocalMove(Vector3.zero, 3f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutCirc);
    interactionDlcShrine.transform.DOShakePosition(5f, new Vector3(0.25f, 0.0f, 0.0f)).SetUpdate<Tweener>(true);
    MMVibrate.Rumble(0.0f, 2f, 5f, (MonoBehaviour) GameManager.GetInstance());
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public static IEnumerator FadeIn()
  {
    bool waitingForFade = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 1f, "", (System.Action) (() => waitingForFade = false));
    while (waitingForFade)
      yield return (object) null;
  }

  public static IEnumerator FadeOut(bool resumeSimulation)
  {
    bool waitingForFade = true;
    MMTransition.ResumePlay((System.Action) (() => waitingForFade = false), resumeSimulation);
    while (waitingForFade)
      yield return (object) null;
  }

  public void SpawnSplatter(int amount, float duration)
  {
    this.StartCoroutine((IEnumerator) this.SpawnSplatterIE(amount, duration));
  }

  public IEnumerator SpawnSplatterIE(int amount, float duration)
  {
    Interaction_DLCShrine interactionDlcShrine = this;
    float increment = duration / (float) amount;
    for (int i = 0; i < amount; ++i)
    {
      GrenadeBullet component = ObjectPool.Spawn<GrenadeBullet>(interactionDlcShrine.grenadeBullet, interactionDlcShrine.transform.position, Quaternion.identity).GetComponent<GrenadeBullet>();
      component.SetOwner(interactionDlcShrine.gameObject);
      component.Play(UnityEngine.Random.Range(-3f, -1f), (float) UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(2f, 4f), -7f, hideIndicator: true);
      yield return (object) new WaitForSeconds(increment);
    }
  }

  public IEnumerator MutatedFollowersRitualIE()
  {
    Interaction_DLCShrine interactionDlcShrine = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionDlcShrine.gameObject, 7f);
    interactionDlcShrine.redLighting.gameObject.SetActive(true);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/rottenfollower_pray_interact", interactionDlcShrine.transform.position);
    yield return (object) interactionDlcShrine.StartCoroutine((IEnumerator) Interaction_DLCShrine.FadeIn());
    PlayerFarming.Instance.GoToAndStop(interactionDlcShrine.transform.position + Vector3.down * 2f + Vector3.right * 3f, interactionDlcShrine.gameObject);
    AnimationCurve animationCurve = new AnimationCurve(new Keyframe[3]
    {
      new Keyframe(0.0f, 0.0f),
      new Keyframe(0.5f, 0.5f),
      new Keyframe(1f, 0.0f)
    });
    List<Follower> mutatedFollowers = new List<Follower>();
    int requiredRottingFollower = interactionDlcShrine.requiredRottingFollowers[DataManager.Instance.YngyaRotOfferingsReceived];
    foreach (Follower follower in Follower.Followers)
    {
      if (follower.Brain.HasTrait(FollowerTrait.TraitType.Mutated) && !FollowerManager.FollowerLocked(follower.Brain.Info.ID, exludeChild: true))
      {
        mutatedFollowers.Add(follower);
        if (mutatedFollowers.Count >= requiredRottingFollower)
          break;
      }
    }
    for (int index = 0; index < mutatedFollowers.Count; ++index)
    {
      mutatedFollowers[index].Brain.CompleteCurrentTask();
      mutatedFollowers[index].Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
      float num = Mathf.Lerp(-2f, 2f, (float) index / (float) mutatedFollowers.Count);
      Vector3 vector3 = interactionDlcShrine.transform.position + Vector3.down;
      vector3.x += num;
      vector3.y -= animationCurve.Evaluate((float) index / (float) mutatedFollowers.Count) * 2f;
      vector3.y += (float) index * 0.05f;
      mutatedFollowers[index].transform.position = vector3;
    }
    SimulationManager.Pause();
    yield return (object) interactionDlcShrine.StartCoroutine((IEnumerator) Interaction_DLCShrine.FadeOut(false));
    AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/rottenfollower_pray_rise");
    foreach (Follower follower in mutatedFollowers)
    {
      Follower f = follower;
      if (f.Brain.CurrentTaskType == FollowerTaskType.ManualControl)
      {
        f.FacePosition(interactionDlcShrine.transform.position);
        GameManager.GetInstance().WaitForSeconds(UnityEngine.Random.Range(0.0f, 0.5f), (System.Action) (() =>
        {
          double num = (double) f.SetBodyAnimation("Rituals/mutated-start", false);
          f.AddBodyAnimation("Rituals/mutated-loop", true, 0.0f);
        }));
      }
      else
        f.Brain.CompleteCurrentTask();
    }
    yield return (object) new WaitForSeconds(2f);
    float increment = 0.3f;
    for (int i = 0; i < 15; ++i)
    {
      mutatedFollowers.Shuffle<Follower>();
      foreach (Follower follower in mutatedFollowers)
      {
        if (follower.Brain.CurrentTaskType == FollowerTaskType.ManualControl)
        {
          SoulCustomTarget.Create(interactionDlcShrine.transform.position + Vector3.back * 2f, follower.transform.position, Color.red, (System.Action) null);
          CameraManager.instance.ShakeCameraForDuration(0.5f, 0.75f, 0.1f);
          yield return (object) new WaitForSeconds(0.05f);
        }
      }
      yield return (object) new WaitForSeconds(increment);
      increment -= 0.05f;
    }
    AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/rottenfollower_pray_descend");
    foreach (Follower follower in mutatedFollowers)
    {
      Follower f = follower;
      if (f.Brain.CurrentTaskType == FollowerTaskType.ManualControl)
        GameManager.GetInstance().WaitForSeconds(UnityEngine.Random.Range(0.0f, 0.5f), (System.Action) (() =>
        {
          double num = (double) f.SetBodyAnimation("Rituals/mutated-end", false);
          f.AddBodyAnimation("idle", true, 0.0f);
        }));
    }
    yield return (object) new WaitForSeconds(2f);
    foreach (Follower follower in mutatedFollowers)
    {
      if (follower.Brain.CurrentTaskType == FollowerTaskType.ManualControl)
        follower.Brain.CompleteCurrentTask();
    }
    SimulationManager.Pause();
    AudioManager.Instance.PlayOneShot("event:/dlc/env/yngya_shrine/rottenfollower_pray_tarot_card_reveal", interactionDlcShrine.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(TarotCustomTarget.Create(interactionDlcShrine.transform.position + Vector3.back * 2f, interactionDlcShrine.playerFarming.transform.position, 2f, interactionDlcShrine.tarots[DataManager.Instance.YngyaRotOfferingsReceived], new System.Action(interactionDlcShrine.\u003CMutatedFollowersRitualIE\u003Eb__52_0)).gameObject);
    ++DataManager.Instance.YngyaRotOfferingsReceived;
    interactionDlcShrine.CheckRotBeam();
    interactionDlcShrine.HasChanged = true;
    interactionDlcShrine.Interactable = false;
  }

  public IEnumerator CompletedRotShrineIE()
  {
    Interaction_DLCShrine interactionDlcShrine = this;
    interactionDlcShrine.rotBeam.gameObject.SetActive(false);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionDlcShrine.gameObject, 6f);
    yield return (object) new WaitForSeconds(1f);
    CameraManager.instance.ShakeCameraForDuration(1f, 2f, 3f);
    interactionDlcShrine.spine.transform.DOMoveZ(3.5f, 3f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutCirc);
    interactionDlcShrine.transform.DOShakePosition(3f, new Vector3(0.25f, 0.0f, 0.0f)).SetUpdate<Tweener>(true);
    MMVibrate.Rumble(0.0f, 2f, 3f, (MonoBehaviour) GameManager.GetInstance());
    AudioManager.Instance.PlayOneShot("event:/locations/light_house/fireplace_shake", interactionDlcShrine.transform.position);
    DataManager.Instance.YngyaOffering = -3;
    yield return (object) new WaitForSeconds(2f);
    SimulationManager.UnPause();
    GameManager.GetInstance().OnConversationEnd();
    interactionDlcShrine.redLighting.gameObject.SetActive(false);
    interactionDlcShrine.gameObject.SetActive(false);
  }

  public void OnFollowerDied(
    int followerID,
    NotificationCentre.NotificationType notificationType)
  {
    this.CheckRotBeam();
  }

  [CompilerGenerated]
  public void \u003CMutatedFollowersRitualIE\u003Eb__52_0()
  {
    if (DataManager.Instance.YngyaRotOfferingsReceived >= this.tarots.Count)
    {
      this.StartCoroutine((IEnumerator) this.CompletedRotShrineIE());
    }
    else
    {
      SimulationManager.UnPause();
      GameManager.GetInstance().OnConversationEnd();
      this.redLighting.gameObject.SetActive(false);
    }
  }
}
