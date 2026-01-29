// Decompiled with JetBrains decompiler
// Type: Interaction_PurchasableFleece
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Spine.Unity;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_PurchasableFleece : Interaction
{
  [SerializeField]
  public GameObject normal;
  [SerializeField]
  public GameObject destroyed;
  [SerializeField]
  public GameObject eyes;
  [SerializeField]
  public SkeletonAnimation genericGhost;
  [SerializeField]
  public float zoom = 6f;
  [SerializeField]
  public GameObject Dirt;
  [SerializeField]
  public GameObject Hole;
  [SerializeField]
  public PlayerFleeceManager.FleeceType fleeceType;
  [SerializeField]
  public InventoryItem.ITEM_TYPE costItem = InventoryItem.ITEM_TYPE.WOOL;
  [SerializeField]
  public InventoryItem.ITEM_TYPE buryItem = InventoryItem.ITEM_TYPE.WOOL;
  [SerializeField]
  public int cost = 5;
  [SerializeField]
  public Transform playerMovPos;
  [SerializeField]
  public ParticleSystem buyParticles;
  [SerializeField]
  public VFXParticle buyParticlesRunes;
  [SerializeField]
  public VFXParticle ghostRevealRunes;
  [SerializeField]
  public ParticleSystem woolParticles;
  [SerializeField]
  public ParticleSystem woolCircleBurst;
  [SerializeField]
  public ParticleSystem ghostRevealParticles;
  [SerializeField]
  public List<SkeletonAnimation> snakes = new List<SkeletonAnimation>();
  [SerializeField]
  public GameObject NewBuildingAvailableObject;
  [SerializeField]
  public DataManager.Variables summonVariable;
  [SerializeField]
  public GameObject flowersContainer;
  [SerializeField]
  public Material redEyesMaterial;
  [SerializeField]
  public Material blueEyesMaterial;
  [SerializeField]
  public GameObject plantWool;
  [SerializeField]
  public bool fakeGrave;
  public EventInstance awaitingWoolLoopInstance;
  public string awaitingWoolLoopSFX = "event:/dlc/env/ghoststatue/awaitingwool_loop";
  public static bool loadedPlayerMenu;
  public Material matCache;
  public Interaction_PurchasableFleece.State CurrentState;
  public string GhostName;
  public float distanceToMove = 2.5f;

  public override bool AllowInteractionWithoutLabel => true;

  public GameObject GraveStone => this.normal;

  public PlayerFleeceManager.FleeceType FleeceType => this.fleeceType;

  public bool Ruined
  {
    get
    {
      return DataManager.Instance.RuinedGraveyards.Contains(this.fakeGrave ? this.transform.GetSiblingIndex() : (int) this.fleeceType);
    }
  }

  public void UpdateInteractions()
  {
    if (!DataManager.Instance.GetVariable(this.summonVariable))
      this.SetState(Interaction_PurchasableFleece.State.CanBury);
    else if (!DataManager.Instance.UnlockedFleeces.Contains((int) this.FleeceType))
      this.SetState(Interaction_PurchasableFleece.State.CanBuyFleece);
    else if (DataManager.Instance.UnlockedFleeces.Contains((int) this.FleeceType))
      this.SetState(Interaction_PurchasableFleece.State.Completed);
    else
      this.SetState(Interaction_PurchasableFleece.State.None);
  }

  public void SetState(Interaction_PurchasableFleece.State NewState)
  {
    this.Dirt.SetActive(false);
    this.Hole.SetActive(false);
    this.UpdateLocalisation();
    this.CurrentState = NewState;
    AudioManager.Instance.StopLoop(this.awaitingWoolLoopInstance);
    switch (NewState)
    {
      case Interaction_PurchasableFleece.State.None:
        this.Hole.SetActive(true);
        this.Interactable = false;
        break;
      case Interaction_PurchasableFleece.State.CanBury:
        this.Interactable = Inventory.GetItemQuantity(this.buryItem) > 0;
        this.Hole.SetActive(true);
        if (this.Interactable)
        {
          this.awaitingWoolLoopInstance = AudioManager.Instance.CreateLoop(this.awaitingWoolLoopSFX, this.gameObject, true);
          break;
        }
        break;
      case Interaction_PurchasableFleece.State.CanBuyFleece:
        this.Dirt.SetActive(true);
        this.Interactable = true;
        break;
      case Interaction_PurchasableFleece.State.Completed:
        this.Dirt.SetActive(true);
        this.Interactable = true;
        break;
    }
    this.NewBuildingAvailableObject.SetActive(Inventory.GetItemQuantity(this.buryItem) > 0 && DataManager.Instance.OnboardedLambGhostNPCs);
    if (Inventory.GetItemQuantity(this.buryItem) <= 0 || !DataManager.Instance.OnboardedLambGhostNPCs)
      return;
    this.SetEyes(true);
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    foreach (Component snake in this.snakes)
      snake.gameObject.SetActive(false);
    if (DataManager.Instance.UnlockedFleeces.Contains((int) this.fleeceType) || Inventory.GetItemQuantity(this.buryItem) > 0 && DataManager.Instance.OnboardedLambGhostNPCs)
      this.SetEyes(true);
    this.ActivateDistance = 2.2f;
    this.NewBuildingAvailableObject.SetActive(Inventory.GetItemQuantity(this.buryItem) > 0);
    this.UpdateInteractions();
    if (!((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null))
      return;
    GameManager.GetInstance().WaitForSeconds(1f, new System.Action(this.UpdateInteractions));
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (!Interaction_PurchasableFleece.loadedPlayerMenu)
      return;
    if ((bool) (UnityEngine.Object) MonoSingleton<UIManager>.Instance)
      MonoSingleton<UIManager>.Instance.UnloadPlayerUpgradesMenu();
    Interaction_PurchasableFleece.loadedPlayerMenu = false;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (!((UnityEngine.Object) this.flowersContainer != (UnityEngine.Object) null))
      return;
    this.flowersContainer.gameObject.SetActive(DataManager.Instance.UnlockedFleeces.Contains((int) this.FleeceType));
  }

  public override void OnDisable()
  {
    base.OnDisable();
    foreach (Component snake in this.snakes)
      snake.gameObject.SetActive(false);
    if (DataManager.Instance.UnlockedFleeces.Contains((int) this.fleeceType))
      this.SetEyes(true);
    AudioManager.Instance.StopLoop(this.awaitingWoolLoopInstance);
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    string translation = LocalizationManager.GetTranslation($"TarotCards/{this.fleeceType.ToString()}/Grave/Description");
    this.GhostName = $"<color=#FFD201>{LocalizationManager.GetTranslation($"TarotCards/{this.fleeceType.ToString()}/Grave/Name")}</color>";
    if (this.CurrentState == Interaction_PurchasableFleece.State.Completed)
      return;
    this.GhostName = $"{this.GhostName}\n<size=20>{translation}</size>";
  }

  public override void GetLabel()
  {
    base.GetLabel();
    switch (this.CurrentState)
    {
      case Interaction_PurchasableFleece.State.None:
      case Interaction_PurchasableFleece.State.Completed:
        string fleeceDisplayName = PlayerFleeceManager.GetFleeceDisplayName(this.fleeceType);
        if ((PlayerFleeceManager.FleeceType) DataManager.Instance.PlayerFleece == this.fleeceType)
          fleeceDisplayName = PlayerFleeceManager.GetFleeceDisplayName(PlayerFleeceManager.FleeceType.Default);
        this.Label = $"{"Interactions/Equip".Localized()} {fleeceDisplayName}";
        this.Interactable = true;
        break;
      case Interaction_PurchasableFleece.State.CanBury:
        this.Label = Inventory.GetItemQuantity(this.buryItem) > 0 ? string.Format(ScriptLocalization.Interactions.Bury, (object) CostFormatter.FormatCost(this.buryItem, 1, false)) : "";
        break;
      case Interaction_PurchasableFleece.State.CanBuyFleece:
        this.Label = string.Join(" ", ScriptLocalization.Interactions.LeaveOffering, CostFormatter.FormatCost(this.costItem, this.cost));
        break;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    switch (this.CurrentState)
    {
      case Interaction_PurchasableFleece.State.None:
      case Interaction_PurchasableFleece.State.Completed:
        this.EquipFleece();
        break;
      case Interaction_PurchasableFleece.State.CanBury:
        if (Inventory.GetItemQuantity(this.buryItem) > 0)
        {
          base.OnInteract(state);
          UIItemSelectorOverlayController itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(this.playerFarming, new List<InventoryItem.ITEM_TYPE>()
          {
            this.buryItem
          }, new ItemSelector.Params()
          {
            Key = "DLCGhost_Wool",
            Context = ItemSelector.Context.Bury,
            Offset = new Vector2(0.0f, 100f),
            HideOnSelection = true,
            RequiresDiscovery = true,
            ShowEmpty = true,
            DontCache = true
          });
          itemSelector.OnItemChosen += (System.Action<InventoryItem.ITEM_TYPE>) (chosenItem => this.StartCoroutine((IEnumerator) this.BuryWoolRoutine()));
          UIItemSelectorOverlayController overlayController = itemSelector;
          overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
          {
            itemSelector = (UIItemSelectorOverlayController) null;
            this.Interactable = true;
            this.HasChanged = true;
          });
          break;
        }
        this.playerFarming.indicator.PlayShake();
        break;
      case Interaction_PurchasableFleece.State.CanBuyFleece:
        if (Inventory.GetItemQuantity(this.costItem) >= this.cost || this.buryItem == InventoryItem.ITEM_TYPE.SPECIAL_WOOL_11)
        {
          base.OnInteract(state);
          this.StartCoroutine((IEnumerator) this.InteractRoutine());
          this.CurrentState = Interaction_PurchasableFleece.State.None;
          this.Interactable = false;
          break;
        }
        this.playerFarming.indicator.PlayShake();
        break;
    }
    this.HasChanged = true;
  }

  public void BuryWoolTest() => this.StartCoroutine((IEnumerator) this.BuryWoolRoutine());

  public IEnumerator BuryWoolRoutine()
  {
    Interaction_PurchasableFleece purchasableFleece = this;
    yield return (object) null;
    AudioManager.Instance.StopLoop(purchasableFleece.awaitingWoolLoopInstance);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, purchasableFleece.zoom);
    purchasableFleece.playerFarming.GoToAndStop(purchasableFleece.transform.position + new Vector3(1.5f, -0.5f), purchasableFleece.gameObject);
    yield return (object) new WaitForSeconds(0.5f);
    SummonGhostLambNPC s = purchasableFleece.GetComponentInParent<SummonGhostLambNPC>();
    if ((UnityEngine.Object) s != (UnityEngine.Object) null)
    {
      AudioManager.Instance.PlayOneShot("event:/dlc/env/ghoststatue/shroud_bury", purchasableFleece.transform.position);
      AudioManager.Instance.PlayOneShot("event:/dlc/music/ghoststatue/shroud_bury");
    }
    else
    {
      AudioManager.Instance.PlayOneShot("event:/dlc/env/ghoststatue/shroud_bury_knight", purchasableFleece.transform.position);
      AudioManager.Instance.PlayOneShot("event:/dlc/music/ghoststatue/shroud_bury_knight");
    }
    yield return (object) new WaitForSeconds(0.2f);
    purchasableFleece.playerFarming.CustomAnimation("woolhaven-ghost", false);
    yield return (object) new WaitForSeconds(0.2f);
    purchasableFleece.plantWool.gameObject.SetActive(true);
    purchasableFleece.woolParticles.Play();
    Vector3 position1 = purchasableFleece.Hole.transform.position;
    float num1 = 2.6f;
    int num2 = (double) purchasableFleece.plantWool.transform.position.z > (double) position1.z ? 1 : -1;
    Vector3 vector3 = position1 + Vector3.forward * num1 * (float) num2;
    DG.Tweening.Sequence sequence = DOTween.Sequence();
    sequence.Append((Tween) purchasableFleece.plantWool.transform.DOScale(0.5f, 0.0f));
    sequence.Append((Tween) purchasableFleece.plantWool.transform.DOScale(1.6f, 0.15f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack));
    sequence.Append((Tween) purchasableFleece.plantWool.transform.DOScale(0.8f, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad));
    sequence.Append((Tween) purchasableFleece.plantWool.transform.DOScale(1.2f, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad));
    sequence.Append((Tween) purchasableFleece.plantWool.transform.DOScale(1f, 0.15f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad));
    Vector3 originalPos = purchasableFleece.plantWool.transform.localPosition;
    float endValue = 0.1f;
    float duration = 1.66f;
    Tween shakeTween = (Tween) DOTween.To((DOGetter<float>) (() => 0.0f), (DOSetter<float>) (x => this.plantWool.transform.localPosition = new Vector3(originalPos.x + Mathf.Sin(Time.time * 50f) * x, originalPos.y, this.plantWool.transform.localPosition.z)), endValue, duration).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear);
    Tween t = (Tween) purchasableFleece.plantWool.transform.DOMoveZ(vector3.z, duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad);
    sequence.AppendCallback((TweenCallback) (() => shakeTween.Play<Tween>()));
    sequence.Append(t);
    sequence.AppendCallback((TweenCallback) (() =>
    {
      shakeTween.Kill();
      this.plantWool.transform.localPosition = new Vector3(originalPos.x, originalPos.y, this.plantWool.transform.localPosition.z);
    }));
    sequence.Append((Tween) purchasableFleece.plantWool.transform.DOMove(position1, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad));
    sequence.Play<DG.Tweening.Sequence>();
    yield return (object) sequence.WaitForCompletion();
    purchasableFleece.plantWool.gameObject.SetActive(false);
    purchasableFleece.ghostRevealRunes.PlayVFX(0.0f, (PlayerFarming) null, true);
    purchasableFleece.ghostRevealParticles.Play();
    purchasableFleece.woolCircleBurst.Play();
    BiomeConstants.Instance.EmitSmokeInteractionVFX(purchasableFleece.transform.position, Vector3.one);
    Vector3 position2 = purchasableFleece.transform.position;
    position2.y -= 0.5f;
    BiomeConstants.Instance.EmitSmokeInteractionVFX(position2, Vector3.one);
    purchasableFleece.Hole.SetActive(true);
    purchasableFleece.Dirt.SetActive(true);
    purchasableFleece.Dirt.transform.localScale = Vector3.one * 0.2f;
    purchasableFleece.Dirt.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    AudioManager.Instance.StopLoop(purchasableFleece.awaitingWoolLoopInstance);
    purchasableFleece.Hole.SetActive(false);
    purchasableFleece.SetEyes(false);
    Inventory.ChangeItemQuantity(purchasableFleece.buryItem, -1);
    BiomeConstants.Instance.ChromaticAbberationTween(2f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 1f);
    yield return (object) new WaitForSeconds(0.3f);
    if ((UnityEngine.Object) purchasableFleece.state == (UnityEngine.Object) null)
      purchasableFleece.state = purchasableFleece.playerFarming.state;
    if ((UnityEngine.Object) s != (UnityEngine.Object) null)
    {
      string[] reactAnimations = new string[3]
      {
        "woolhaven-ghost-react1",
        "woolhaven-ghost-react2",
        "woolhaven-ghost-react3"
      };
      purchasableFleece.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      purchasableFleece.playerFarming.state.LockStateChanges = true;
      purchasableFleece.playerFarming.Spine.AnimationState.SetAnimation(0, "idle", true);
      GameManager.GetInstance().WaitForSeconds(1.6f, (System.Action) (() =>
      {
        this.playerFarming.Spine.AnimationState.SetAnimation(0, reactAnimations[UnityEngine.Random.Range(0, reactAnimations.Length)], false);
        this.playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      }));
      s.RevealNPC((System.Action) (() =>
      {
        BiomeConstants.Instance.ChromaticAbberationTween(0.25f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
        this.playerFarming.state.LockStateChanges = false;
        DataManager.Instance.SetVariable(this.summonVariable, true);
        List<Objectives_Custom> objectivesOfType = ObjectiveManager.GetObjectivesOfType<Objectives_Custom>();
        ObjectivesData objective = (ObjectivesData) null;
        for (int index = 0; index < objectivesOfType.Count; ++index)
        {
          if (objectivesOfType[index].CustomQuestType == Objectives.CustomQuestTypes.ReturnLastLambGhosts)
            objective = (ObjectivesData) objectivesOfType[index];
        }
        if (objective != null)
          ObjectiveManager.UpdateObjective(objective);
        Interaction_DLCYngyaShrine.Instance.AddGhostJuiceFromLostWool();
      }));
      purchasableFleece.Interactable = Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.WOOL) > 0;
      purchasableFleece.HasChanged = true;
    }
    else
    {
      purchasableFleece.playerFarming.Spine.AnimationState.SetAnimation(0, "idle", true);
      purchasableFleece.StartCoroutine((IEnumerator) purchasableFleece.ShakeCameraWithRampUp(2f, 2f, 1f));
      GameManager.GetInstance().OnConversationNext(purchasableFleece.gameObject, 6f);
      yield return (object) new WaitForSeconds(2f);
      GameManager.GetInstance().OnConversationNext(purchasableFleece.gameObject);
      CameraManager.instance.ShakeCameraForDuration(1.5f, 1.5f, 1f, false);
      DataManager.Instance.SetVariable(purchasableFleece.summonVariable, true);
      for (int i = 0; i < 60; ++i)
      {
        SkeletonAnimation ghost = UnityEngine.Object.Instantiate<SkeletonAnimation>(purchasableFleece.genericGhost);
        ghost.gameObject.SetActive(true);
        ghost.transform.position = purchasableFleece.transform.position + Vector3.forward;
        ghost.transform.DOMove(purchasableFleece.transform.position + Vector3.back * 20f + (Vector3) (UnityEngine.Random.insideUnitCircle * 10f), 0.75f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) ghost.gameObject)));
        if ((double) UnityEngine.Random.value < 0.5)
          ghost.transform.localScale = new Vector3(-1f, 1f, 1f);
        yield return (object) new WaitForSeconds(0.016f);
      }
      yield return (object) new WaitForSeconds(2f);
      purchasableFleece.CurrentState = Interaction_PurchasableFleece.State.CanBuyFleece;
      purchasableFleece.OnInteract(purchasableFleece.state);
      BiomeConstants.Instance.ChromaticAbberationTween(0.25f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    }
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    playerFarming.indicator.ShowTopInfo(this.GhostName);
  }

  public override void OnBecomeNotCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeNotCurrent(playerFarming);
    playerFarming.indicator.HideTopInfo();
  }

  public IEnumerator InteractRoutine()
  {
    Interaction_PurchasableFleece purchasableFleece = this;
    purchasableFleece.Interactable = false;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 5f);
    PlayerFarming.Instance.GoToAndStop(purchasableFleece.playerMovPos.position, purchasableFleece.gameObject);
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    AudioManager.Instance.PlayOneShot("event:/dlc/env/ghoststatue/fleece_buy", purchasableFleece.transform.position);
    AudioManager.Instance.PlayOneShot("event:/dlc/music/ghoststatue/fleece_buy");
    yield return (object) PlayerFarming.Instance.Spine.YieldForAnimation("pray");
    PlayerFarming.Instance.Spine.state.SetAnimation(0, "idle", true);
    Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.WOOL, -purchasableFleece.cost);
    DataManager.Instance.UnlockedFleeces.Add((int) purchasableFleece.fleeceType);
    DLCShrineRoomLocationManager.CheckWoolhavenCompleteAchievement();
    DataManager.Instance.PlayerFleece = (int) purchasableFleece.fleeceType;
    DataManager.Instance.PlayerVisualFleece = (int) purchasableFleece.fleeceType;
    ObjectiveManager.CompleteShowFleeceObjective();
    GameManager.GetInstance().WaitForSeconds(2.36f, (System.Action) (() =>
    {
      PlayerFarming.Instance.simpleSpineAnimator?.SetSkin("Lamb_" + DataManager.Instance.PlayerFleece.ToString());
      BiomeConstants.Instance.EmitDisplacementEffectScale(PlayerFarming.Instance.transform.position, 0.3f);
    }));
    purchasableFleece.buyParticles.Play();
    purchasableFleece.buyParticlesRunes.PlayVFX(0.0f, (PlayerFarming) null, true);
    purchasableFleece.normal.transform.DOKill();
    purchasableFleece.normal.transform.DOShakePosition(1f, new Vector3(0.1f, 0.0f, 0.0f)).OnComplete<Tweener>((TweenCallback) (() => MMVibrate.StopRumble()));
    MMVibrate.RumbleContinuous(0.0f, 0.5f);
    purchasableFleece.SetEyes(true);
    yield return (object) new WaitForSeconds(1f);
    System.Threading.Tasks.Task loadTask = MonoSingleton<UIManager>.Instance.LoadPlayerUpgradesMenu();
    yield return (object) new WaitUntil((Func<bool>) (() => loadTask.IsCompleted));
    Interaction_PurchasableFleece.loadedPlayerMenu = true;
    PlayerFarming.Instance.CustomAnimationWithCallback("unlock-fleece", false, (System.Action) (() =>
    {
      MMVibrate.StopRumble();
      UIPlayerUpgradesMenuController upgradesMenuController1 = MonoSingleton<UIManager>.Instance.PlayerUpgradesMenuTemplate.Instantiate<UIPlayerUpgradesMenuController>();
      upgradesMenuController1.ShowNewFleecesUnlocked(new PlayerFleeceManager.FleeceType[1]
      {
        this.fleeceType
      }, true);
      AudioManager.Instance.PlayOneShot("event:/sermon/scroll_sermon_menu", this.gameObject);
      UIPlayerUpgradesMenuController upgradesMenuController2 = upgradesMenuController1;
      upgradesMenuController2.OnHidden = upgradesMenuController2.OnHidden + (System.Action) (() =>
      {
        PlayerFarming.Instance.IsGoat = false;
        PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
        GameManager.GetInstance().OnConversationEnd();
        foreach (Component snake in this.snakes)
          snake.gameObject.SetActive(false);
        PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
        PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
        if (!((UnityEngine.Object) this.flowersContainer != (UnityEngine.Object) null))
          return;
        this.StartCoroutine((IEnumerator) this.RevealFlowersIE());
      });
    }));
  }

  public void EquipFleece()
  {
    if ((PlayerFleeceManager.FleeceType) DataManager.Instance.PlayerFleece == this.fleeceType)
    {
      DataManager.Instance.PlayerFleece = 0;
      DataManager.Instance.PlayerVisualFleece = 0;
    }
    else
    {
      DataManager.Instance.PlayerFleece = (int) this.fleeceType;
      DataManager.Instance.PlayerVisualFleece = (int) this.fleeceType;
    }
    PlayerFarming.Instance.IsGoat = false;
    PlayerFarming.Instance.simpleSpineAnimator?.SetSkin("Lamb_" + DataManager.Instance.PlayerFleece.ToString());
    AudioManager.Instance.PlayOneShot("event:/player/layer_clothes", this.gameObject);
    AudioManager.Instance.PlayOneShot("event:/player/weapon_equip", this.gameObject);
    BiomeConstants.Instance.EmitSmokeInteractionVFX(PlayerFarming.Instance.transform.position, Vector3.one);
  }

  public IEnumerator RevealFlowersIE()
  {
    Interaction_PurchasableFleece purchasableFleece = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(purchasableFleece.flowersContainer, 6f);
    purchasableFleece.NewBuildingAvailableObject.SetActive(false);
    yield return (object) new WaitForSeconds(0.7f);
    purchasableFleece.flowersContainer.gameObject.SetActive(true);
    for (int index = 0; index < purchasableFleece.flowersContainer.transform.childCount; ++index)
      purchasableFleece.flowersContainer.transform.GetChild(index).gameObject.SetActive(false);
    EventInstance flowerLoop = AudioManager.Instance.CreateLoop("event:/dlc/env/woolhaven/flowerpot_bloom_loop", true);
    for (int i = 0; i < purchasableFleece.flowersContainer.transform.childCount; ++i)
    {
      AudioManager.Instance.PlayOneShot("event:/dlc/env/woolhaven/flowerpot_bloom", purchasableFleece.flowersContainer.transform.GetChild(i).transform.position);
      purchasableFleece.flowersContainer.transform.GetChild(i).gameObject.SetActive(true);
      purchasableFleece.flowersContainer.transform.GetChild(i).transform.DOPunchScale(Vector3.one * 0.2f, 0.5f);
      yield return (object) new WaitForSeconds(0.2f);
    }
    yield return (object) new WaitForSeconds(0.7f);
    AudioManager.Instance.StopLoop(flowerLoop);
    GameManager.GetInstance().OnConversationEnd();
    purchasableFleece.HasChanged = true;
    purchasableFleece.Interactable = true;
  }

  public void SetColdEyes(bool enable)
  {
    if ((UnityEngine.Object) this.eyes == (UnityEngine.Object) null)
      return;
    SpriteRenderer componentInChildren = this.eyes.GetComponentInChildren<SpriteRenderer>();
    if ((UnityEngine.Object) componentInChildren == (UnityEngine.Object) null)
      return;
    componentInChildren.material = enable ? this.blueEyesMaterial : this.redEyesMaterial;
  }

  public void SetEyes(bool enable) => this.eyes.gameObject.SetActive(enable);

  public void Destroy()
  {
    int num = (int) this.fleeceType;
    if (this.fakeGrave)
      num = this.transform.GetSiblingIndex();
    DataManager.Instance.RuinedGraveyards.Add(num);
    this.normal.gameObject.SetActive(!this.Ruined);
    this.destroyed.gameObject.SetActive(this.Ruined);
  }

  public IEnumerator ShakeCameraWithRampUp(float buildUp, float totalDuration, float maxShake)
  {
    float t = 0.0f;
    while ((double) (t += Time.deltaTime) < (double) buildUp)
    {
      float t1 = t / buildUp;
      CameraManager.instance.ShakeCameraForDuration(Mathf.Lerp(0.0f, maxShake / 2f, t1), Mathf.Lerp(0.0f, maxShake, t1), buildUp, false);
      yield return (object) null;
    }
    CameraManager.instance.ShakeCameraForDuration(maxShake / 2f, maxShake, totalDuration - buildUp, false);
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__49_0(InventoryItem.ITEM_TYPE chosenItem)
  {
    this.StartCoroutine((IEnumerator) this.BuryWoolRoutine());
  }

  public enum State
  {
    None,
    CanBury,
    CanBuyFleece,
    Completed,
  }
}
