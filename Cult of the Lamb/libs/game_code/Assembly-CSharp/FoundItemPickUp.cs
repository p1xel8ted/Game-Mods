// Decompiled with JetBrains decompiler
// Type: FoundItemPickUp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Spine.Unity;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class FoundItemPickUp : Interaction
{
  public UnityEvent CallbackEnd;
  public static List<FoundItemPickUp> FoundItemPickUps = new List<FoundItemPickUp>();
  public GameObject uINewCard;
  public UINewItemOverlayController.TypeOfCard typeOfCard;
  public FollowerClothingType clothingType;
  [SpineSkin("", "", true, false, false)]
  public string SkinToForce;
  public bool GiveRecruit;
  public SkeletonAnimation Spine;
  public bool FollowerSkinForceSelection;
  public int WeaponLevel = -1;
  public int DurabilityLevel = -1;
  public TarotCards.Card WeaponModifier = TarotCards.Card.Count;
  [CompilerGenerated]
  public float \u003CCurrentWeaponDurability\u003Ek__BackingField;
  public SpriteRenderer itemSprite;
  public StructureBrain.TYPES DecorationType;
  public FollowerLocation Location;
  public int LocationLayer = 1;
  public EquipmentType TypeOfWeapon;
  public RelicType TypeOfRelic;
  public EquipmentType StartingWeapon;
  public EquipmentType TypeOfCurse;
  public EquipmentType StartingCurse;
  public bool SpawnedOldCard;
  public List<WeaponIcons> WeaponSprites = new List<WeaponIcons>();
  public float WeaponOnGroundDamage;
  public float CurrentWeaponDamage;
  public float DamageDifference;
  public string DamageDifferenceString;
  public List<WeaponAttachmentData> weaponAttachments = new List<WeaponAttachmentData>();
  public bool weaponAssigned;
  public float rand;
  public PickUp newCard;
  public GameObject g;
  public InventoryItem.ITEM_TYPE itemType;
  public List<string> FollowerSkinsAvailable = new List<string>();
  public string PickedSkin;
  public Vector3 BookTargetPosition;
  public float Timer;
  public bool spawningMenu = true;
  public bool isLoadingAssets;

  public float CurrentWeaponDurability
  {
    get => this.\u003CCurrentWeaponDurability\u003Ek__BackingField;
    set => this.\u003CCurrentWeaponDurability\u003Ek__BackingField = value;
  }

  public void Start()
  {
    if (this.typeOfCard != UINewItemOverlayController.TypeOfCard.Weapon)
      return;
    this.GetWeapon();
  }

  public override void GetLabel()
  {
    this.Label = ScriptLocalization.Interactions.PickUp;
    if (!this.Interactable)
    {
      this.Label = "";
    }
    else
    {
      switch (this.typeOfCard)
      {
        case UINewItemOverlayController.TypeOfCard.Weapon:
          break;
        case UINewItemOverlayController.TypeOfCard.Curse:
          this.DamageDifference = this.CurrentWeaponDamage - this.WeaponOnGroundDamage;
          string str1 = LocalizeIntegration.ReverseText(this.WeaponOnGroundDamage.ToString());
          string str2 = LocalizeIntegration.ReverseText(this.DamageDifference.ToString());
          if ((double) this.DamageDifference > 0.0)
          {
            this.DamageDifferenceString = $" | DMG: {str1}<color=red> -{str2}</color>";
          }
          else
          {
            string str3 = LocalizeIntegration.ReverseText((this.DamageDifference * -1f).ToString());
            this.DamageDifferenceString = $" | DMG: {str1}<color=green>+{str3}</color>";
          }
          this.Label = $"{ScriptLocalization.Interactions.PickUp} {PlayerDetails_Player.GetWeaponCondition(this.DurabilityLevel)}{PlayerDetails_Player.GetWeaponMod(this.WeaponModifier)}{EquipmentManager.GetEquipmentData(this.TypeOfCurse).GetLocalisedTitle()} {PlayerDetails_Player.GetWeaponLevel(this.WeaponLevel)}{this.DamageDifferenceString}";
          break;
        case UINewItemOverlayController.TypeOfCard.Relic:
          this.Label = $"{ScriptLocalization.Interactions.PickUp} <color=#FFD201>{RelicData.GetTitleLocalisation(this.TypeOfRelic)}";
          break;
        default:
          this.Label = ScriptLocalization.Interactions.PickUp;
          break;
      }
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

  public void GetWeapon()
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

  public void GetCurse()
  {
    if (!this.SpawnedOldCard)
    {
      this.TypeOfCurse = DataManager.Instance.CursePool[UnityEngine.Random.Range(0, DataManager.Instance.CursePool.Count)];
      while (this.TypeOfCurse == this.playerFarming.currentCurse)
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

  public string NumbersToRoman(int number)
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

  public void GetStartingWeapon(PlayerFarming playerFarming)
  {
    switch (this.typeOfCard)
    {
      case UINewItemOverlayController.TypeOfCard.Weapon:
        this.StartingWeapon = playerFarming.currentWeapon;
        break;
      case UINewItemOverlayController.TypeOfCard.Curse:
        this.StartingCurse = playerFarming.currentCurse;
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

  public static bool IsOutfitPickUpActive(FollowerClothingType clothing)
  {
    foreach (FoundItemPickUp foundItemPickUp in FoundItemPickUp.FoundItemPickUps)
    {
      if (foundItemPickUp.clothingType == clothing)
        return true;
    }
    return false;
  }

  public IEnumerator PickUpRoutine()
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
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(foundItemPickUp.playerFarming.gameObject, 5f);
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
    if (this.isLoadingAssets)
      return;
    this.spawningMenu = true;
    PlayerFarming component = this.state.GetComponent<PlayerFarming>();
    this.GetStartingWeapon(component);
    BiomeConstants.Instance.EmitPickUpVFX(this.transform.position);
    switch (this.typeOfCard)
    {
      case UINewItemOverlayController.TypeOfCard.Weapon:
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
        this.isLoadingAssets = true;
        this.StartCoroutine((IEnumerator) UIManager.LoadAssets(MonoSingleton<UIManager>.Instance.LoadWorldMapAssets(), (System.Action) (() =>
        {
          this.isLoadingAssets = false;
          MonoSingleton<UIManager>.Instance.ShowWorldMap().Show(this.Location);
          UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
        })));
        break;
      case UINewItemOverlayController.TypeOfCard.Relic:
        GameManager.GetInstance().OnConversationNew();
        AudioManager.Instance.PlayOneShot("event:/player/new_item_pickup", this.gameObject);
        BiomeConstants.Instance.EmitPickUpVFX(this.transform.position);
        CameraManager.instance.ShakeCameraForDuration(0.7f, 0.9f, 0.3f);
        MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
        if (this.TypeOfRelic != RelicType.None)
          DataManager.UnlockRelic(this.TypeOfRelic);
        foreach (Objective_FindRelic objectiveFindRelic in new List<Objective_FindRelic>((IEnumerable<Objective_FindRelic>) ObjectiveManager.GetObjectivesOfType<Objective_FindRelic>()))
        {
          if (objectiveFindRelic.RelicType == this.TypeOfRelic)
            DataManager.Instance.CanFindLeaderRelic = false;
        }
        UIRelicMenuController relicMenuController = MonoSingleton<UIManager>.Instance.RelicMenuTemplate.Instantiate<UIRelicMenuController>();
        relicMenuController.Show(this.TypeOfRelic, component);
        relicMenuController.OnHidden = relicMenuController.OnHidden + (System.Action) (() =>
        {
          this.CloseMenuCallback();
          DOTween.To((DOGetter<float>) (() => Time.timeScale), (DOSetter<float>) (x => Time.timeScale = x), 1f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
          GameManager.GetInstance().OnConversationEnd();
        });
        if (this.TypeOfRelic == RelicType.ProjectileRing)
          DataManager.Instance.FoundRelicAtHubShore = true;
        if (ObjectiveManager.HasCustomObjective(Objectives.TYPES.FIND_RELIC) && ObjectiveManager.GetObjectivesOfType<Objective_FindRelic>()[0].RelicType == this.TypeOfRelic)
        {
          Objective_FindRelic objective = ObjectiveManager.GetObjectivesOfType<Objective_FindRelic>()[0];
          objective.Complete();
          ObjectiveManager.UpdateObjective((ObjectivesData) objective);
        }
        if (GameManager.IsDungeon(PlayerFarming.Location))
          component.playerRelic.EquipRelic(EquipmentManager.GetRelicData(this.TypeOfRelic), initialEquip: true);
        component.indicator.text.text = "";
        this.Interactable = false;
        this.HasChanged = true;
        break;
      case UINewItemOverlayController.TypeOfCard.Outfit:
        this.itemType = this.gameObject.GetComponent<PickUp>().type;
        if (this.clothingType != FollowerClothingType.None)
        {
          NotificationCentre.Instance.PlayGenericNotification("Notifications/OutfitUnlocked/Notification/On", NotificationBase.Flair.Positive);
          DataManager.Instance.Alerts.ClothingAlerts.AddOnce(this.clothingType);
          DataManager.Instance.AddNewClothes(this.clothingType);
        }
        this.CreateMenuWeapon();
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

  public IEnumerator PlayerShowWeaponRoutine()
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
    PlayerFarming component = foundItemPickUp.state.GetComponent<PlayerFarming>();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(component.Spine.AnimationState.SetAnimation(0, EquipmentManager.GetWeaponData(component.currentWeapon).PickupAnimationKey, false).Animation.Duration);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator PlayerShowCurseRoutine()
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
    PlayerFarming component = foundItemPickUp.state.GetComponent<PlayerFarming>();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(component.Spine.AnimationState.SetAnimation(0, EquipmentManager.GetCurseData(component.currentCurse).PickupAnimationKey, false).Animation.Duration);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void CreateMenuWeapon()
  {
    AudioManager.Instance.PlayOneShot("event:/player/new_item_pickup", this.gameObject);
    this.state.CURRENT_STATE = StateMachine.State.InActive;
    UINewItemOverlayController overlayController1 = MonoSingleton<UIManager>.Instance.ShowNewItemOverlay();
    if (this.typeOfCard == UINewItemOverlayController.TypeOfCard.Decoration && this.DecorationType != StructureBrain.TYPES.NONE)
    {
      overlayController1.pickedBuilding = this.DecorationType;
      overlayController1.Show(this.typeOfCard, this.transform.position, false);
    }
    else if (this.typeOfCard == UINewItemOverlayController.TypeOfCard.Outfit)
      overlayController1.Show(this.typeOfCard, this.transform.position, this.clothingType);
    else
      overlayController1.Show(this.typeOfCard, this.transform.position, true);
    UINewItemOverlayController overlayController2 = overlayController1;
    overlayController2.OnHidden = overlayController2.OnHidden + new System.Action(this.CloseMenuCallback);
    DOTween.To((DOGetter<float>) (() => Time.timeScale), (DOSetter<float>) (x => Time.timeScale = x), 0.0f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
  }

  public void CreateMenuItem()
  {
    this.playerFarming.playerController.speed = 0.0f;
    UINewItemOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowNewItemOverlay();
    overlayController.Show(this.typeOfCard, this.transform.position, this.itemType);
    overlayController.OnHidden = overlayController.OnHidden + new System.Action(this.GetComponent<PickUp>().PickedUp);
    overlayController.OnHidden = overlayController.OnHidden + new System.Action(this.CloseMenuCallback);
    DOTween.To((DOGetter<float>) (() => Time.timeScale), (DOSetter<float>) (x => Time.timeScale = x), 0.0f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void CreateMenuFollowerSkin()
  {
    UINewItemOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowNewItemOverlay();
    overlayController.Show(this.typeOfCard, this.transform.position, this.PickedSkin);
    overlayController.OnHidden = overlayController.OnHidden + new System.Action(this.GetComponent<PickUp>().PickedUp);
    overlayController.OnHidden = overlayController.OnHidden + new System.Action(this.CloseMenuCallback);
    DOTween.To((DOGetter<float>) (() => Time.timeScale), (DOSetter<float>) (x => Time.timeScale = x), 0.0f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void CloseMenuCallback()
  {
    if (!((UnityEngine.Object) this != (UnityEngine.Object) null))
      return;
    switch (this.typeOfCard)
    {
      default:
        PlayerFarming.SetStateForAllPlayers(abortGoto: true);
        this.playerFarming.GoToAndStopping = false;
        PickUp component = this.GetComponent<PickUp>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.PickedUp();
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
        break;
    }
  }

  [CompilerGenerated]
  public void \u003CMagnetToPlayer\u003Eb__35_0()
  {
    CameraManager.instance.ShakeCameraForDuration(0.7f, 0.9f, 0.3f);
    this.SpawnMenu();
  }

  [CompilerGenerated]
  public void \u003CSpawnMenu\u003Eb__56_1()
  {
    this.CloseMenuCallback();
    DOTween.To((DOGetter<float>) (() => Time.timeScale), (DOSetter<float>) (x => Time.timeScale = x), 1f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    GameManager.GetInstance().OnConversationEnd();
  }

  [CompilerGenerated]
  public void \u003CSpawnMenu\u003Eb__56_0()
  {
    this.isLoadingAssets = false;
    MonoSingleton<UIManager>.Instance.ShowWorldMap().Show(this.Location);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
