// Decompiled with JetBrains decompiler
// Type: FoundItemPickUp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class FoundItemPickUp : Interaction
{
  public UnityEvent CallbackEnd;
  public static List<FoundItemPickUp> FoundItemPickUps = new List<FoundItemPickUp>();
  public GameObject uINewCard;
  public UINewItemOverlayController.TypeOfCard typeOfCard;
  [SpineSkin("", "", true, false, false)]
  public string SkinToForce;
  public bool GiveRecruit;
  public SkeletonAnimation Spine;
  public bool FollowerSkinForceSelection;
  public int WeaponLevel = -1;
  public int DurabilityLevel = -1;
  public TarotCards.Card WeaponModifier = TarotCards.Card.Count;
  public SpriteRenderer itemSprite;
  public StructureBrain.TYPES DecorationType;
  public FollowerLocation Location;
  public int LocationLayer = 1;
  public EquipmentType TypeOfWeapon;
  public EquipmentType StartingWeapon;
  public EquipmentType TypeOfCurse;
  public EquipmentType StartingCurse;
  public bool SpawnedOldCard;
  public List<WeaponIcons> WeaponSprites = new List<WeaponIcons>();
  private float WeaponOnGroundDamage;
  private float CurrentWeaponDamage;
  private float DamageDifference;
  private string DamageDifferenceString;
  private List<WeaponAttachmentData> weaponAttachments = new List<WeaponAttachmentData>();
  private WeaponPickUpUI weaponPickupUI;
  private bool weaponAssigned;
  private float rand;
  private PickUp newCard;
  private GameObject g;
  private InventoryItem.ITEM_TYPE itemType;
  private List<string> FollowerSkinsAvailable = new List<string>();
  private string PickedSkin;
  private Vector3 BookTargetPosition;
  private float Timer;
  private bool spawningMenu = true;

  public float CurrentWeaponDurability { get; set; }

  private void Start()
  {
    if (this.typeOfCard != UINewItemOverlayController.TypeOfCard.Weapon)
      return;
    this.GetWeapon();
  }

  public override void GetLabel()
  {
    this.Label = ScriptLocalization.Interactions.PickUp;
    switch (this.typeOfCard)
    {
      case UINewItemOverlayController.TypeOfCard.Weapon:
        break;
      case UINewItemOverlayController.TypeOfCard.Curse:
        this.DamageDifference = this.CurrentWeaponDamage - this.WeaponOnGroundDamage;
        if ((double) this.DamageDifference > 0.0)
          this.DamageDifferenceString = $" | DMG: {(object) this.WeaponOnGroundDamage}<color=red> -{(object) this.DamageDifference}</color>";
        else
          this.DamageDifferenceString = $" | DMG: {(object) this.WeaponOnGroundDamage}<color=green>+{(object) (float) ((double) this.DamageDifference * -1.0)}</color>";
        this.Label = $"{ScriptLocalization.Interactions.PickUp} {PlayerDetails_Player.GetWeaponCondition(this.DurabilityLevel)}{PlayerDetails_Player.GetWeaponMod(this.WeaponModifier)}{EquipmentManager.GetEquipmentData(this.TypeOfCurse).GetLocalisedTitle()} {PlayerDetails_Player.GetWeaponLevel(this.WeaponLevel)}{this.DamageDifferenceString}";
        break;
      default:
        this.Label = ScriptLocalization.Interactions.PickUp;
        break;
    }
  }

  public void MagnetToPlayer()
  {
    PickUp component = this.GetComponent<PickUp>();
    component.MagnetToPlayer = true;
    component.MagnetDistance = 100f;
    component.AddToInventory = false;
    this.AutomaticallyInteract = true;
    component.Callback.AddListener((UnityAction) (() =>
    {
      CameraManager.instance.ShakeCameraForDuration(0.7f, 0.9f, 0.3f);
      this.SpawnMenu();
    }));
  }

  private void GetWeapon()
  {
    if (this.weaponAssigned)
      return;
    if (!this.SpawnedOldCard)
    {
      this.TypeOfWeapon = DataManager.Instance.WeaponPool[UnityEngine.Random.Range(0, DataManager.Instance.WeaponPool.Count)];
      this.rand = (float) UnityEngine.Random.Range(0, 100);
      if ((double) this.rand >= 0.0 && (double) this.rand <= (double) DataManager.WeaponDurabilityChance[0])
        this.DurabilityLevel = 0;
      else if ((double) this.rand > (double) DataManager.WeaponDurabilityChance[0] && (double) this.rand < (double) DataManager.WeaponDurabilityChance[1])
        this.DurabilityLevel = 1;
      else if ((double) this.rand > (double) DataManager.WeaponDurabilityChance[1] && (double) this.rand < (double) DataManager.WeaponDurabilityChance[2])
        this.DurabilityLevel = 2;
      else if ((double) this.rand > (double) DataManager.WeaponDurabilityChance[2])
        this.DurabilityLevel = 3;
      this.CurrentWeaponDurability = DataManager.WeaponDurabilityLevels[this.DurabilityLevel];
    }
    this.itemSprite.sprite = EquipmentManager.GetEquipmentData(this.TypeOfWeapon).WorldSprite;
    this.itemSprite.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    this.weaponAssigned = true;
  }

  private void GetCurse()
  {
    if (!this.SpawnedOldCard)
    {
      this.TypeOfCurse = DataManager.Instance.CursePool[UnityEngine.Random.Range(0, DataManager.Instance.CursePool.Count)];
      while (this.TypeOfCurse == DataManager.Instance.CurrentCurse)
        this.TypeOfCurse = DataManager.Instance.CursePool[UnityEngine.Random.Range(0, DataManager.Instance.CursePool.Count)];
      this.rand = (float) UnityEngine.Random.Range(0, 100);
      if ((double) this.rand >= 0.0 && (double) this.rand <= (double) DataManager.WeaponDurabilityChance[0])
        this.DurabilityLevel = 0;
      else if ((double) this.rand > (double) DataManager.WeaponDurabilityChance[0] && (double) this.rand < (double) DataManager.WeaponDurabilityChance[1])
        this.DurabilityLevel = 1;
      else if ((double) this.rand > (double) DataManager.WeaponDurabilityChance[1] && (double) this.rand < (double) DataManager.WeaponDurabilityChance[2])
        this.DurabilityLevel = 2;
      else if ((double) this.rand > (double) DataManager.WeaponDurabilityChance[2])
        this.DurabilityLevel = 3;
    }
    else
      this.TypeOfCurse = this.StartingCurse;
    this.itemSprite.sprite = EquipmentManager.GetEquipmentData(this.TypeOfCurse).WorldSprite;
    this.itemSprite.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
  }

  private string NumbersToRoman(int number)
  {
    switch (number)
    {
      case 1:
        return "I";
      case 2:
        return "II";
      case 3:
        return "III";
      case 4:
        return "IV";
      case 5:
        return "V";
      case 6:
        return "VI";
      default:
        return "";
    }
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    FoundItemPickUp.FoundItemPickUps.Add(this);
    if (this.typeOfCard != UINewItemOverlayController.TypeOfCard.Curse)
      return;
    this.GetCurse();
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    FoundItemPickUp.FoundItemPickUps.Remove(this);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.SpawnMenu();
  }

  private new void OnDestroy()
  {
    if (!((UnityEngine.Object) this.weaponPickupUI != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.weaponPickupUI.gameObject);
  }

  public void GetStartingWeapon()
  {
    switch (this.typeOfCard)
    {
      case UINewItemOverlayController.TypeOfCard.Weapon:
        this.StartingWeapon = DataManager.Instance.CurrentWeapon;
        break;
      case UINewItemOverlayController.TypeOfCard.Curse:
        this.StartingCurse = DataManager.Instance.CurrentCurse;
        break;
    }
  }

  public void SetStartingWeapon(
    EquipmentType _StartingWeapon,
    EquipmentType _StartingCurse,
    int _WeaponLevel,
    int _DurabilityLevel)
  {
    switch (this.typeOfCard)
    {
      case UINewItemOverlayController.TypeOfCard.Weapon:
        this.StartingWeapon = _StartingWeapon;
        this.WeaponLevel = _WeaponLevel;
        this.DurabilityLevel = _DurabilityLevel;
        this.GetWeapon();
        break;
      case UINewItemOverlayController.TypeOfCard.Curse:
        this.StartingCurse = _StartingCurse;
        this.WeaponLevel = _WeaponLevel;
        this.DurabilityLevel = _DurabilityLevel;
        break;
    }
  }

  private IEnumerator PickUpRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    FoundItemPickUp foundItemPickUp = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      foundItemPickUp.state.CURRENT_STATE = StateMachine.State.Idle;
      GameManager.GetInstance().OnConversationEnd();
      UnityEngine.Object.Destroy((UnityEngine.Object) foundItemPickUp.gameObject);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    foundItemPickUp.Timer = 0.0f;
    GameManager.GetInstance().OnConversationNew(SnapLetterBox: true);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 5f);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    PlayerSimpleInventory component = foundItemPickUp.state.gameObject.GetComponent<PlayerSimpleInventory>();
    foundItemPickUp.BookTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f);
    foundItemPickUp.BookTargetPosition = foundItemPickUp.state.transform.position + new Vector3(0.0f, 0.2f, -1.2f);
    foundItemPickUp.state.CURRENT_STATE = StateMachine.State.FoundItem;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_Short", foundItemPickUp.gameObject);
    foundItemPickUp.transform.DOMove(foundItemPickUp.BookTargetPosition, 0.2f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void SpawnMenu()
  {
    this.spawningMenu = true;
    this.GetStartingWeapon();
    BiomeConstants.Instance.EmitPickUpVFX(this.transform.position);
    switch (this.typeOfCard)
    {
      case UINewItemOverlayController.TypeOfCard.Weapon:
        if ((bool) (UnityEngine.Object) this.weaponPickupUI)
          UnityEngine.Object.Destroy((UnityEngine.Object) this.weaponPickupUI.gameObject);
        Debug.Log((object) this.TypeOfWeapon);
        this.state.GetComponent<PlayerWeapon>().SetWeapon(this.TypeOfWeapon, this.WeaponLevel);
        this.StartCoroutine((IEnumerator) this.PlayerShowWeaponRoutine());
        break;
      case UINewItemOverlayController.TypeOfCard.Decoration:
        this.itemType = this.gameObject.GetComponent<PickUp>().type;
        if (this.DecorationType != StructureBrain.TYPES.NONE)
        {
          StructuresData.CompleteResearch(this.DecorationType);
          StructuresData.SetRevealed(this.DecorationType);
        }
        this.CreateMenuWeapon();
        break;
      case UINewItemOverlayController.TypeOfCard.Gift:
        this.itemType = this.gameObject.GetComponent<PickUp>().type;
        this.gameObject.GetComponent<PickUp>().enabled = false;
        if (!DataManager.Instance.FoundItems.Contains(this.itemType))
        {
          DataManager.Instance.FoundItems.Add(this.itemType);
          this.CreateMenuItem();
        }
        else
        {
          this.StartCoroutine((IEnumerator) this.PickUpRoutine());
          this.spawningMenu = false;
        }
        Inventory.AddItem((int) this.itemType, 1);
        break;
      case UINewItemOverlayController.TypeOfCard.Necklace:
        this.itemType = this.gameObject.GetComponent<PickUp>().type;
        this.gameObject.GetComponent<PickUp>().enabled = false;
        if (!DataManager.Instance.FoundItems.Contains(this.itemType))
        {
          DataManager.Instance.FoundItems.Add(this.itemType);
          this.CreateMenuItem();
        }
        else
          this.spawningMenu = false;
        Inventory.AddItem((int) this.itemType, 1);
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
        break;
      case UINewItemOverlayController.TypeOfCard.FollowerSkin:
        this.PickedSkin = this.FollowerSkinForceSelection ? this.SkinToForce : DataManager.GetRandomLockedSkin();
        DataManager.SetFollowerSkinUnlocked(this.PickedSkin);
        if (this.GiveRecruit)
          FollowerManager.CreateNewRecruit(FollowerLocation.Base, this.PickedSkin, NotificationCentre.NotificationType.NewRecruit);
        this.CreateMenuFollowerSkin();
        break;
      case UINewItemOverlayController.TypeOfCard.MapLocation:
        Debug.Log((object) "AA");
        Debug.Log((object) this.Location);
        MonoSingleton<UIManager>.Instance.ShowWorldMap().Show(this.Location);
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
        break;
      default:
        Debug.Log((object) "Uh oh something went wrong :O");
        break;
    }
    this.CallbackEnd?.Invoke();
    if (this.spawningMenu)
    {
      IEnumerator enumerator = (IEnumerator) this.transform.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          ((Component) enumerator.Current).gameObject.SetActive(false);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
    }
    this.Interactable = false;
    this.Label = " ";
  }

  private IEnumerator PlayerShowWeaponRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    FoundItemPickUp foundItemPickUp = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      GameManager.GetInstance().CameraResetTargetZoom();
      foundItemPickUp.state.CURRENT_STATE = StateMachine.State.Idle;
      foundItemPickUp.CloseMenuCallback();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    foundItemPickUp.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    GameManager.GetInstance().CameraSetTargetZoom(4f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, EquipmentManager.GetWeaponData(DataManager.Instance.CurrentWeapon).PickupAnimationKey, false).Animation.Duration);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator PlayerShowCurseRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    FoundItemPickUp foundItemPickUp = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      GameManager.GetInstance().CameraResetTargetZoom();
      foundItemPickUp.state.CURRENT_STATE = StateMachine.State.Idle;
      foundItemPickUp.CloseMenuCallback();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    foundItemPickUp.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    GameManager.GetInstance().CameraSetTargetZoom(4f);
    AudioManager.Instance.PlayOneShot("event:/player/absorb_curse", foundItemPickUp.gameObject);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).PickupAnimationKey, false).Animation.Duration);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void CreateMenuWeapon()
  {
    AudioManager.Instance.PlayOneShot("event:/player/new_item_pickup", this.gameObject);
    UINewItemOverlayController overlayController1 = MonoSingleton<UIManager>.Instance.ShowNewItemOverlay();
    if (this.typeOfCard == UINewItemOverlayController.TypeOfCard.Decoration && this.DecorationType != StructureBrain.TYPES.NONE)
    {
      overlayController1.pickedBuilding = this.DecorationType;
      overlayController1.Show(this.typeOfCard, this.transform.position, false);
    }
    else
      overlayController1.Show(this.typeOfCard, this.transform.position, true);
    UINewItemOverlayController overlayController2 = overlayController1;
    overlayController2.OnHidden = overlayController2.OnHidden + new System.Action(this.CloseMenuCallback);
    DOTween.To((DOGetter<float>) (() => Time.timeScale), (DOSetter<float>) (x => Time.timeScale = x), 0.0f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
  }

  private void CreateMenuItem()
  {
    PlayerFarming.Instance.playerController.speed = 0.0f;
    UINewItemOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowNewItemOverlay();
    overlayController.Show(this.typeOfCard, this.transform.position, this.itemType);
    overlayController.OnHidden = overlayController.OnHidden + new System.Action(this.GetComponent<PickUp>().PickedUp);
    overlayController.OnHidden = overlayController.OnHidden + new System.Action(this.CloseMenuCallback);
    DOTween.To((DOGetter<float>) (() => Time.timeScale), (DOSetter<float>) (x => Time.timeScale = x), 0.0f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  private void CreateMenuFollowerSkin()
  {
    UINewItemOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowNewItemOverlay();
    overlayController.Show(this.typeOfCard, this.transform.position, this.PickedSkin);
    overlayController.OnHidden = overlayController.OnHidden + new System.Action(this.GetComponent<PickUp>().PickedUp);
    overlayController.OnHidden = overlayController.OnHidden + new System.Action(this.CloseMenuCallback);
    DOTween.To((DOGetter<float>) (() => Time.timeScale), (DOSetter<float>) (x => Time.timeScale = x), 0.0f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  private void CloseMenuCallback()
  {
    if (!((UnityEngine.Object) this != (UnityEngine.Object) null))
      return;
    switch (this.typeOfCard)
    {
      default:
        PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
        PlayerFarming.Instance.GoToAndStopping = false;
        PickUp component = this.GetComponent<PickUp>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.PickedUp();
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
        break;
    }
  }

  protected override void Update()
  {
    if (!((UnityEngine.Object) Interactor.CurrentInteraction != (UnityEngine.Object) this) || !((UnityEngine.Object) this.weaponPickupUI != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.weaponPickupUI.gameObject);
  }
}
