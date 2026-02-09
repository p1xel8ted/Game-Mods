// Decompiled with JetBrains decompiler
// Type: Interaction_Tailor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using src.Extensions;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_Tailor : Interaction
{
  public Transform SewingPosition;
  public Transform InteractionBookPosition;
  public Collider2D Collider;
  public UITailorMinigameOverlayController _uiCookingMinigameOverlayController;
  public static Interaction_Tailor Instance;
  public Structures_Tailor _StructureInfo;
  public Structure Structure;
  public string sLabel;
  public bool activating;
  [CompilerGenerated]
  public bool \u003CForceShowAssignMenu\u003Ek__BackingField;
  public bool isLoadingAssets;
  public UITailorMenuController menuController;
  public bool givenOutfit;
  public Coroutine assignRoutine;
  public Vector3 followerPreviousPosition = Vector3.zero;

  public Structures_Tailor Brain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_Tailor;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public bool ForceShowAssignMenu
  {
    get => this.\u003CForceShowAssignMenu\u003Ek__BackingField;
    set => this.\u003CForceShowAssignMenu\u003Ek__BackingField = value;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.ForceShowAssignMenu = false;
    this.UpdateLocalisation();
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    Interaction_Tailor.Instance = this;
    DataManager.Instance.RevealedTailor = true;
  }

  public virtual void OnBrainAssigned()
  {
    DataManager.Instance.SetVariable(DataManager.Variables.UnlockedTailor, true);
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.UpdatePathfinding();
  }

  public void UpdatePathfinding()
  {
    if ((UnityEngine.Object) this.Collider != (UnityEngine.Object) null)
      AstarPath.active.UpdateGraphs(this.Collider.bounds);
    else
      Debug.Log((object) "DIDNT WORK");
  }

  public override void OnDisableInteraction()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (!((UnityEngine.Object) Interaction_Tailor.Instance == (UnityEngine.Object) this))
      return;
    Interaction_Tailor.Instance = (Interaction_Tailor) null;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = LocalizationManager.GetTranslation("Interactions/Craft");
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Label = this.sLabel;
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.activating)
      return;
    SimulationManager.Pause();
    this.activating = true;
    base.OnInteract(state);
    this.menuController = MonoSingleton<UIManager>.Instance.ShowTailorMenu(this.Brain);
    UITailorMenuController menuController1 = this.menuController;
    menuController1.OnHidden = menuController1.OnHidden + (System.Action) (() =>
    {
      this.menuController = (UITailorMenuController) null;
      this.activating = false;
      this.CheckGivenOutfit();
    });
    this.menuController.OnConfirm += new System.Action(this.CookAll);
    if (this.ForceShowAssignMenu)
    {
      Debug.Log((object) "Force Show Assign Menu!");
      this.menuController.ForceAssignTab();
    }
    UITailorMenuController menuController2 = this.menuController;
    menuController2.OnCancel = menuController2.OnCancel + (System.Action) (() => SimulationManager.UnPause());
    this.ForceShowAssignMenu = false;
  }

  public void CheckGivenOutfit()
  {
    if (DataManager.Instance.outfitsCreated < 24 || this.givenOutfit || DataManager.Instance.UnlockedClothing.Contains(FollowerClothingType.Special_7))
      return;
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_OUTFIT, 1, this.playerFarming.transform.position).GetComponent<FoundItemPickUp>().clothingType = FollowerClothingType.Special_7;
    this.givenOutfit = true;
  }

  public void CookAll()
  {
    if (this.isLoadingAssets)
      return;
    Debug.Log((object) "CookAll!!!");
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.playerFarming.CameraBone, 5f);
    this.isLoadingAssets = true;
    this.StartCoroutine((IEnumerator) UIManager.LoadAssets(MonoSingleton<UIManager>.Instance.LoadTailorMinigameAssets(), (System.Action) (() =>
    {
      this.isLoadingAssets = false;
      this._uiCookingMinigameOverlayController = MonoSingleton<UIManager>.Instance.TailorMinigameOverlayControllerTemplate.Instantiate<UITailorMinigameOverlayController>();
      this._uiCookingMinigameOverlayController.Initialise(this.Brain.Data, this, this.playerFarming);
      this._uiCookingMinigameOverlayController.OnCook += new System.Action(this.OnCook);
      this._uiCookingMinigameOverlayController.OnUnderCook += new System.Action(this.OnUnderCook);
      this._uiCookingMinigameOverlayController.OnBurn += new System.Action(this.OnBurn);
      this._uiCookingMinigameOverlayController.OnFinish += new System.Action(this.OnFinishCooking);
      this.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    })));
  }

  public void OnCook() => this.MealFinishedCooking(true);

  public void OnUnderCook() => this.OnBurn();

  public void OnBurn()
  {
    AudioManager.Instance.PlayOneShot("event:/cooking/fire_start", this.transform.position);
    foreach (ClothingData.CostItem costItem in TailorManager.GetClothingData(this.Brain.Data.QueuedClothings[0].ClothingType).Cost)
      InventoryItem.Spawn(costItem.ItemType, Mathf.FloorToInt((float) costItem.Cost * 0.5f), this.SewingPosition.position);
    this.MealFinishedCooking(false);
  }

  public void OnFinishCooking()
  {
    if (this.Brain.Data.QueuedClothings.Count > 0)
      return;
    this.EndCooking();
  }

  public void EndCooking()
  {
    this._uiCookingMinigameOverlayController.OnCook -= new System.Action(this.OnCook);
    this._uiCookingMinigameOverlayController.OnUnderCook -= new System.Action(this.OnUnderCook);
    this._uiCookingMinigameOverlayController.OnBurn -= new System.Action(this.OnBurn);
    this._uiCookingMinigameOverlayController.OnFinish -= new System.Action(this.OnFinishCooking);
    this._uiCookingMinigameOverlayController = (UITailorMinigameOverlayController) null;
    GameManager.GetInstance().CameraResetTargetZoom();
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().AddPlayersToCamera();
  }

  public virtual void MealFinishedCooking(bool AddToInventory)
  {
    StructuresData.ClothingStruct clothingStruct = this.Brain.Data.QueuedClothings.Count > 0 ? this.Brain.Data.QueuedClothings[0] : this.Brain.Data.CurrentTailoringClothes;
    ObjectiveManager.CompleteCraftClothingObjective(clothingStruct.ClothingType);
    if (AddToInventory)
    {
      this._StructureInfo.Data.AllClothing.Add(clothingStruct);
      ClothingData clothingData = TailorManager.GetClothingData(clothingStruct.ClothingType);
      if (clothingData.SpecialClothing)
        TailorManager.RemoveClothingFromDeadFollower(clothingData.ClothingType);
      ResourceCustomTarget.Create(this.InteractionBookPosition.gameObject, this.SewingPosition.position, InventoryItem.ITEM_TYPE.GIFT_MEDIUM, (System.Action) (() =>
      {
        this.transform.localScale = new Vector3(0.75f, 1.25f, 1f);
        this.transform.DOScale(Vector3.one, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
        Debug.Log((object) "=========================".Colour(Color.yellow));
        foreach (StructuresData.ClothingStruct message in this._StructureInfo.Data.AllClothing)
          Debug.Log((object) message);
        Debug.Log((object) "=========================".Colour(Color.yellow));
      }), this.transform.parent, false);
    }
    if (this.Brain.Data.QueuedClothings.Count <= 0)
      return;
    this.Brain.Data.QueuedClothings.RemoveAt(0);
  }

  public void AssignFollowerClothing(
    Follower follower,
    FollowerClothingType clothingType,
    string variant)
  {
    this.assignRoutine = this.StartCoroutine((IEnumerator) this.AssignFollowerClothingIE(follower, clothingType, variant));
  }

  public IEnumerator AssignFollowerClothingIE(
    Follower follower,
    FollowerClothingType clothingType,
    string variant)
  {
    Interaction_Tailor interactionTailor = this;
    LetterBox.Instance.ShowSkipPrompt();
    interactionTailor.StartCoroutine((IEnumerator) interactionTailor.WaitForSkip(follower, clothingType, variant));
    FollowerBrain followerWearingOutfit = TailorManager.GetClothingData(clothingType).SpecialClothing ? TailorManager.GetFollowerWearingOutfit(clothingType) : (FollowerBrain) null;
    Follower followerById = followerWearingOutfit != null ? FollowerManager.FindFollowerByID(followerWearingOutfit.Info.ID) : (Follower) null;
    if (!((UnityEngine.Object) followerById != (UnityEngine.Object) null))
    {
      Vector3 zero = Vector3.zero;
    }
    else
    {
      Vector3 position = followerById.transform.position;
    }
    interactionTailor.followerPreviousPosition = follower.transform.position;
    follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    follower.transform.position = interactionTailor.transform.position;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(follower.gameObject);
    double num1 = (double) follower.SetBodyAnimation("idle", true);
    follower.HideAllFollowerIcons();
    yield return (object) new WaitForSeconds(0.5f);
    follower.Brain.AssignClothing(clothingType, variant);
    BiomeConstants.Instance.EmitSmokeInteractionVFX(follower.transform.position, Vector3.one);
    AudioManager.Instance.PlayOneShot("event:/relics/puff_of_smoke", follower.gameObject);
    FollowerBrain.SetFollowerCostume(follower.Spine.skeleton, follower.Brain.Info.XPLevel, follower.Brain.Info.SkinName, follower.Brain.Info.SkinColour, FollowerOutfitType.None, follower.Brain.Info.Hat, clothingType, follower.Brain.Info.Customisation, follower.Brain.Info.Special, follower.Brain.Info.Necklace, variant, follower.Brain._directInfoAccess);
    double num2 = (double) follower.SetBodyAnimation("Reactions/react-admire" + UnityEngine.Random.Range(1, 4).ToString(), false);
    follower.AddBodyAnimation("idle", true, 0.0f);
    ObjectiveManager.CompleteAssignClothingObjective(clothingType, follower.Brain.Info.ID);
    if (!DataManager.Instance.ClothingAssigned.Contains(clothingType) && clothingType != FollowerClothingType.None)
    {
      NotificationCentre.Instance.PlayFaithNotification("Notifications/AssignedNewOutfit/Notification/On", 5f, NotificationBase.Flair.Positive, follower.Brain._directInfoAccess.ID);
      DataManager.Instance.ClothingAssigned.Add(clothingType);
    }
    AudioManager.Instance.PlayOneShot("event:/building/finished_fabric", follower.gameObject);
    yield return (object) new WaitForSeconds(0.75f);
    yield return (object) new WaitForSeconds(0.5f);
    double num3 = (double) follower.SetBodyAnimation("spawn-out", false);
    yield return (object) new WaitForSeconds(0.75f);
    GameManager.GetInstance().OnConversationEnd();
    interactionTailor.ForceShowAssignMenu = true;
    interactionTailor.OnInteract(interactionTailor.playerFarming.state);
    follower.transform.position = interactionTailor.followerPreviousPosition;
    follower.Brain.CompleteCurrentTask();
    follower.ShowAllFollowerIcons();
    follower.AddThought((Thought) UnityEngine.Random.Range(335, 340));
    follower.Outfit.SetOutfit(follower.Spine, false);
    DataManager.Instance.AssignedFollowersOutfits = true;
    interactionTailor.StopAllCoroutines();
  }

  public IEnumerator WaitForSkip(
    Follower follower,
    FollowerClothingType clothingType,
    string variant)
  {
    Interaction_Tailor interactionTailor = this;
    yield return (object) new WaitForSecondsRealtime(1f);
    while (!InputManager.Gameplay.GetAttackButtonDown())
      yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
    interactionTailor.StopCoroutine(interactionTailor.assignRoutine);
    follower.Brain.AssignClothing(clothingType, variant);
    FollowerBrain.SetFollowerCostume(follower.Spine.Skeleton, follower.Brain._directInfoAccess, forceUpdate: true);
    DataManager.Instance.AssignedFollowersOutfits = true;
    interactionTailor.ForceShowAssignMenu = true;
    interactionTailor.OnInteract(interactionTailor.playerFarming.state);
    follower.AddThought((Thought) UnityEngine.Random.Range(335, 340));
    follower.transform.position = interactionTailor.followerPreviousPosition;
    follower.Brain.CompleteCurrentTask();
    follower.ShowAllFollowerIcons();
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__25_0()
  {
    this.menuController = (UITailorMenuController) null;
    this.activating = false;
    this.CheckGivenOutfit();
  }

  [CompilerGenerated]
  public void \u003CCookAll\u003Eb__28_0()
  {
    this.isLoadingAssets = false;
    this._uiCookingMinigameOverlayController = MonoSingleton<UIManager>.Instance.TailorMinigameOverlayControllerTemplate.Instantiate<UITailorMinigameOverlayController>();
    this._uiCookingMinigameOverlayController.Initialise(this.Brain.Data, this, this.playerFarming);
    this._uiCookingMinigameOverlayController.OnCook += new System.Action(this.OnCook);
    this._uiCookingMinigameOverlayController.OnUnderCook += new System.Action(this.OnUnderCook);
    this._uiCookingMinigameOverlayController.OnBurn += new System.Action(this.OnBurn);
    this._uiCookingMinigameOverlayController.OnFinish += new System.Action(this.OnFinishCooking);
    this.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAction0;
  }

  [CompilerGenerated]
  public void \u003CMealFinishedCooking\u003Eb__34_0()
  {
    this.transform.localScale = new Vector3(0.75f, 1.25f, 1f);
    this.transform.DOScale(Vector3.one, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    Debug.Log((object) "=========================".Colour(Color.yellow));
    foreach (StructuresData.ClothingStruct message in this._StructureInfo.Data.AllClothing)
      Debug.Log((object) message);
    Debug.Log((object) "=========================".Colour(Color.yellow));
  }
}
