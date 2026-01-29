// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UINewItemOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI.Assets;
using Lamb.UI.BuildMenu;
using Spine;
using Spine.Unity;
using src.Extensions;
using src.UINavigator;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UINewItemOverlayController : UIMenuBase
{
  public Transform LerpChild;
  public Image imageOfItemUnlocked;
  public InventoryIconMapping _inventoryIconMapping;
  public CanvasGroup _infoCanvas;
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  public TextMeshProUGUI Title;
  public TextMeshProUGUI Subtitle;
  public TextMeshProUGUI Description;
  public UINewItemOverlayController.TypeOfCard MyTypeOfCard;
  public SkeletonGraphic Spine;
  [SpineEvent("", "Spine", true, true, false)]
  public string eventName;
  public string bookOpen = "book-open";
  public string bookClose = "book-close";
  public EventData eventData;
  public EventData bookOpenData;
  public EventData bookCloseData;
  public RectTransform SpineParent;
  public Vector3 SpineStartPosition;
  public SkeletonGraphic FollowerSpine;
  public SkeletonGraphic ClothingSpine;
  public StructureBrain.TYPES pickedBuilding;
  public EventInstance _loopingSoundInstance;
  public bool _createdLoop;

  public override void Awake()
  {
    base.Awake();
    this._canvasGroup.alpha = 0.0f;
    this.ClothingSpine.enabled = false;
    this.FollowerSpine.enabled = false;
    this.imageOfItemUnlocked.enabled = false;
    this.Title.text = " ";
    this.Subtitle.text = " ";
    this._controlPrompts.HideAcceptButton();
  }

  public void Start()
  {
    this.eventData = this.Spine.Skeleton.Data.FindEvent(this.eventName);
    this.bookOpenData = PlayerFarming.Instance.Spine.Skeleton.Data.FindEvent(this.bookOpen);
    this.bookCloseData = PlayerFarming.Instance.Spine.Skeleton.Data.FindEvent(this.bookClose);
    this.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    PlayerFarming.Instance.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.PlayerHandleAnimationStateEvent);
  }

  public void Show(
    UINewItemOverlayController.TypeOfCard myTypeOfCard,
    Vector3 startPosition,
    string skinName,
    bool instant = false)
  {
    this.MyTypeOfCard = myTypeOfCard;
    Debug.Log((object) "Show item overlay ");
    if (myTypeOfCard == UINewItemOverlayController.TypeOfCard.FollowerSkin)
    {
      this.FollowerSpine.Skeleton.SetSkin(skinName);
      WorshipperData.SkinAndData characters = WorshipperData.Instance.GetCharacters(skinName);
      foreach (WorshipperData.SlotAndColor slotAndColour in characters.SlotAndColours[Mathf.Min(0, characters.SlotAndColours.Count - 1)].SlotAndColours)
      {
        Slot slot = this.FollowerSpine.Skeleton.FindSlot(slotAndColour.Slot);
        if (slot != null)
          slot.SetColor(slotAndColour.color);
      }
      this.Title.text = $"{ScriptLocalization.UI.KnowledgeAcquired} {ScriptLocalization.UI.SkinTitle}";
      this.Subtitle.text = WorshipperData.Instance.GetSkinsLocationString(characters);
      this.Description.text = ScriptLocalization.UI.SkinDescription;
      this.Description.isRightToLeftText = this.Subtitle.isRightToLeftText = this.Title.isRightToLeftText = LocalizeIntegration.IsArabic();
      this.imageOfItemUnlocked.sprite = (Sprite) null;
      this.Spine.Skeleton.SetSkin("Trinket");
      DataManager.SetFollowerSkinUnlocked(skinName);
    }
    else
      Debug.Log((object) "UH OH something went wrong :( ");
    this.Show(instant);
    this.SpineStartPosition = Camera.main.WorldToScreenPoint(startPosition);
    this.StartCoroutine((IEnumerator) this.MoveSpine());
  }

  public void Show(
    UINewItemOverlayController.TypeOfCard myTypeOfCard,
    Vector3 startPosition,
    FollowerClothingType itemType,
    bool instant = false)
  {
    List<WorshipperData.SlotsAndColours> slotAndColours = TailorManager.GetClothingData(itemType).SlotAndColours;
    this.MyTypeOfCard = myTypeOfCard;
    this.Spine.Skeleton.SetSkin("Outfit");
    this.Title.text = $"{ScriptLocalization.UI.KnowledgeAcquired} {TailorManager.LocalizedName(itemType)}";
    this.Subtitle.text = TailorManager.LocalizedDescription(itemType);
    this.ClothingSpine.Skeleton.SetSkin(TailorManager.GetSkin(itemType));
    this.Description.text = LocalizationManager.GetTranslation("Clothing/Lore");
    this.Description.isRightToLeftText = this.Subtitle.isRightToLeftText = this.Title.isRightToLeftText = LocalizeIntegration.IsArabic();
    foreach (WorshipperData.SlotAndColor slotAndColour in slotAndColours[Mathf.Min(0, slotAndColours.Count - 1)].SlotAndColours)
    {
      Slot slot = this.ClothingSpine.Skeleton.FindSlot(slotAndColour.Slot);
      if (slot != null)
        slot.SetColor(slotAndColour.color);
    }
    Debug.Log((object) ("Skin =: " + TailorManager.GetSkin(itemType)));
    this.Show(instant);
    this.SpineStartPosition = Camera.main.WorldToScreenPoint(startPosition);
    this.StartCoroutine((IEnumerator) this.MoveSpine());
  }

  public void Show(
    UINewItemOverlayController.TypeOfCard myTypeOfCard,
    Vector3 startPosition,
    InventoryItem.ITEM_TYPE itemType,
    bool instant = false)
  {
    this.MyTypeOfCard = myTypeOfCard;
    switch (myTypeOfCard)
    {
      case UINewItemOverlayController.TypeOfCard.Gift:
        this.Title.text = $"{ScriptLocalization.UI.KnowledgeAcquired} {InventoryItem.LocalizedName(itemType)}";
        this.Description.text = InventoryItem.LocalizedLore(itemType);
        this.Subtitle.text = InventoryItem.LocalizedDescription(itemType);
        this.Description.isRightToLeftText = this.Subtitle.isRightToLeftText = this.Title.isRightToLeftText = LocalizeIntegration.IsArabic();
        this.imageOfItemUnlocked.sprite = this._inventoryIconMapping.GetImage(itemType);
        this.Spine.Skeleton.SetSkin("Trinket");
        break;
      case UINewItemOverlayController.TypeOfCard.Necklace:
        this.Title.text = $"{ScriptLocalization.UI.KnowledgeAcquired} {InventoryItem.LocalizedName(itemType)}";
        this.Description.text = InventoryItem.LocalizedLore(itemType);
        this.Subtitle.text = InventoryItem.LocalizedDescription(itemType);
        this.Description.isRightToLeftText = this.Subtitle.isRightToLeftText = this.Title.isRightToLeftText = LocalizeIntegration.IsArabic();
        this.imageOfItemUnlocked.sprite = this._inventoryIconMapping.GetImage(itemType);
        this.Spine.Skeleton.SetSkin("Trinket");
        break;
      default:
        Debug.Log((object) "UH OH something went wrong :( ");
        break;
    }
    this.Show(instant);
    this.SpineStartPosition = Camera.main.WorldToScreenPoint(startPosition);
    this.StartCoroutine((IEnumerator) this.MoveSpine());
  }

  public void Show(
    UINewItemOverlayController.TypeOfCard myTypeOfCard,
    Vector3 startPosition,
    bool pickFromAvailable,
    bool instant = false)
  {
    this.MyTypeOfCard = myTypeOfCard;
    this.Title.text = ScriptLocalization.UI.KnowledgeAcquired;
    switch (myTypeOfCard)
    {
      case UINewItemOverlayController.TypeOfCard.Trinket:
        Debug.Log((object) "WARNING: Removed trinket card type".Colour(Color.red));
        break;
      case UINewItemOverlayController.TypeOfCard.Decoration:
        if (pickFromAvailable)
        {
          List<StructureBrain.TYPES> listFromLocation = DataManager.Instance.GetDecorationListFromLocation(PlayerFarming.Location);
          foreach (StructureBrain.TYPES types in DataManager.Instance.GetDecorationListFromCategory(DataManager.DecorationType.All))
          {
            if (!listFromLocation.Contains(types))
              listFromLocation.Add(types);
          }
          this.pickedBuilding = listFromLocation[UnityEngine.Random.Range(0, listFromLocation.Count - 1)];
        }
        Debug.Log((object) ("Picked building: " + this.pickedBuilding.ToString()));
        this.imageOfItemUnlocked.sprite = TypeAndPlacementObjects.GetByType(this.pickedBuilding).IconImage;
        if ((UnityEngine.Object) this.imageOfItemUnlocked.sprite == (UnityEngine.Object) null)
          Debug.Log((object) ("Missing icon image from TypeAndPlacementObjects for: " + this.pickedBuilding.ToString()));
        this.Title.text = $"{ScriptLocalization.UI.KnowledgeAcquired} {StructuresData.LocalizedName(this.pickedBuilding)}";
        this.Subtitle.text = DataManager.GetDecorationLocation(this.pickedBuilding);
        this.Description.text = StructuresData.LocalizedDescription(this.pickedBuilding);
        this.Description.isRightToLeftText = this.Subtitle.isRightToLeftText = this.Title.isRightToLeftText = LocalizeIntegration.IsArabic();
        StructuresData.CompleteResearch(this.pickedBuilding);
        StructuresData.SetRevealed(this.pickedBuilding);
        this.Spine.Skeleton.SetSkin("Decoration");
        break;
      default:
        Debug.Log((object) "UH OH something went wrong :( ");
        break;
    }
    this.Show(instant);
    this.SpineStartPosition = Camera.main.WorldToScreenPoint(startPosition);
    this.StartCoroutine((IEnumerator) this.MoveSpine());
  }

  public void TurnOnImage()
  {
    switch (this.MyTypeOfCard)
    {
      case UINewItemOverlayController.TypeOfCard.FollowerSkin:
        this.FollowerSpine.enabled = true;
        this.imageOfItemUnlocked.enabled = false;
        this.ClothingSpine.enabled = false;
        break;
      case UINewItemOverlayController.TypeOfCard.Outfit:
        this.FollowerSpine.enabled = false;
        this.ClothingSpine.enabled = true;
        this.imageOfItemUnlocked.enabled = false;
        break;
      default:
        this.FollowerSpine.enabled = false;
        this.ClothingSpine.enabled = false;
        this.imageOfItemUnlocked.enabled = true;
        break;
    }
  }

  public void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (this.eventData != e.Data)
      return;
    AudioManager.Instance.PlayOneShot("event:/player/new_item_reveal", this.gameObject);
    this.TurnOnImage();
  }

  public void PlayerHandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (this.bookOpenData == e.Data)
    {
      if (this._createdLoop)
        return;
      Debug.Log((object) "Play Page Loop");
      this._loopingSoundInstance = AudioManager.Instance.CreateLoop("event:/player/new_item_pages_loop", PlayerFarming.Instance.gameObject, true);
      this._createdLoop = true;
    }
    else
    {
      if (this.bookCloseData != e.Data)
        return;
      this._createdLoop = false;
      AudioManager.Instance.StopLoop(this._loopingSoundInstance);
      AudioManager.Instance.PlayOneShot("event:/player/new_item_book_close", PlayerFarming.Instance.gameObject);
      PlayerFarming.Instance.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.PlayerHandleAnimationStateEvent);
    }
  }

  public override void OnHideCompleted()
  {
    AudioManager.Instance.StopLoop(this._loopingSoundInstance);
    this.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    PlayerFarming.Instance.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.PlayerHandleAnimationStateEvent);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public new void OnDisable()
  {
    AudioManager.Instance.StopLoop(this._loopingSoundInstance);
    this.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    PlayerFarming.Instance.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.PlayerHandleAnimationStateEvent);
  }

  public IEnumerator MoveSpine()
  {
    this.Spine.rectTransform.position = this.SpineStartPosition;
    this.Spine.AnimationState.SetAnimation(0, "reveal", false);
    this.Spine.AnimationState.AddAnimation(0, "static", true, 0.0f);
    float Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.Spine.rectTransform.position = Vector3.Lerp(this.SpineStartPosition, this.SpineParent.position, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
  }

  public IEnumerator EndSpine()
  {
    this.Spine.AnimationState.SetAnimation(0, "end", false);
    float Progress = 0.0f;
    float Duration = 0.3f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.Spine.rectTransform.position = Vector3.Lerp(this.SpineParent.position, this.SpineStartPosition + new Vector3(0.5f, -0.5f), Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    this.Spine.gameObject.SetActive(false);
  }

  public override IEnumerator DoShowAnimation()
  {
    UINewItemOverlayController overlayController = this;
    overlayController._canvasGroup.alpha = 1f;
    PlayerFarming.Instance.SpineUseDeltaTime(false);
    HUD_Manager.Instance.Hide(false, 0);
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive, PlayerNotToInclude: PlayerFarming.Instance);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    AudioManager.Instance.PlayOneShot("event:/player/new_item_pickup", PlayerFarming.Instance.gameObject);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("find-item/find-item-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("find-item/find-item-loop", 0, true, 0.0f);
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, -2f));
    overlayController.LerpChild.localPosition = new Vector3(-300f, overlayController.LerpChild.localPosition.y);
    Vector3 vector3_1 = new Vector3(0.0f, overlayController.LerpChild.localPosition.y);
    overlayController._infoCanvas.alpha = 0.0f;
    overlayController._infoCanvas.DOFade(1f, 0.66f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuart);
    overlayController.LerpChild.DOLocalMoveX(vector3_1.x, 0.66f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    yield return (object) new WaitForSecondsRealtime(1f);
    overlayController._controlPrompts.ShowAcceptButton();
    while (!InputManager.UI.GetCancelButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) && !InputManager.UI.GetAcceptButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      yield return (object) null;
    overlayController._controlPrompts.HideAcceptButton();
    if (overlayController.MyTypeOfCard == UINewItemOverlayController.TypeOfCard.Decoration)
    {
      int num1 = (int) overlayController._loopingSoundInstance.setVolume(0.33f);
      UIBuildMenuController buildMenuController = MonoSingleton<UIManager>.Instance.BuildMenuTemplate.Instantiate<UIBuildMenuController>();
      buildMenuController.Show(overlayController.pickedBuilding);
      UIBuildMenuController buildMenuController1 = buildMenuController;
      buildMenuController1.OnHidden = buildMenuController1.OnHidden + (System.Action) (() =>
      {
        int num2 = (int) this._loopingSoundInstance.setVolume(1f);
        buildMenuController = (UIBuildMenuController) null;
      });
      overlayController.PushInstance<UIBuildMenuController>(buildMenuController);
      while ((UnityEngine.Object) buildMenuController != (UnityEngine.Object) null)
        yield return (object) null;
    }
    AudioManager.Instance.PlayOneShot("event:/player/new_item_sequence_close", overlayController.gameObject);
    overlayController.LerpChild.localPosition = new Vector3(0.0f, overlayController.LerpChild.localPosition.y);
    Vector3 vector3_2 = new Vector3(300f, overlayController.LerpChild.localPosition.y);
    overlayController._infoCanvas.DOFade(0.0f, 0.66f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuart);
    overlayController.LerpChild.DOLocalMoveX(vector3_2.x, 0.66f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    yield return (object) new WaitForSecondsRealtime(0.5f);
    overlayController.StartCoroutine((IEnumerator) overlayController.EndSpine());
    yield return (object) new WaitForSecondsRealtime(0.15f);
    if (overlayController.MyTypeOfCard == UINewItemOverlayController.TypeOfCard.Decoration)
      PlayerFarming.Instance.simpleSpineAnimator.Animate("find-item/find-decoration-stop", 0, false);
    else
      PlayerFarming.Instance.simpleSpineAnimator.Animate("find-item/find-item-stop", 0, false);
    yield return (object) new WaitForSecondsRealtime(2f);
    Time.timeScale = 1f;
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 0.0f));
    PlayerFarming.Instance.SpineUseDeltaTime(true);
    PlayerFarming.SetStateForAllPlayers();
    HUD_Manager.Instance.Show(0);
    overlayController.Hide(true);
  }

  public enum TypeOfCard
  {
    Weapon,
    Curse,
    Trinket,
    Decoration,
    Gift,
    Necklace,
    FollowerSkin,
    MapLocation,
    Relic,
    Outfit,
  }
}
