// Decompiled with JetBrains decompiler
// Type: FollowerOutfitShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using MMTools;
using Spine;
using Spine.Unity;
using src.UINavigator;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using WebSocketSharp;

#nullable disable
public class FollowerOutfitShop : Interaction
{
  public bool Activated;
  public bool SoldItem;
  public FollowerOutfitShop.OutfitAquisitionMethod aquisitionMethod;
  public GameObject g;
  [SerializeField]
  public GameObject CoinTarget;
  [SerializeField]
  public SkeletonAnimation Spine;
  [FormerlySerializedAs("BarksContainer")]
  [SerializeField]
  public GameObject GoToPosition;
  [SerializeField]
  public shopKeeperManager shopKeeperManager;
  [SerializeField]
  public GameObject particles;
  [SerializeField]
  public GameObject comeBackLaterConvo;
  [SerializeField]
  public GameObject noItemsBark;
  [SerializeField]
  public GameObject noItemsConvo;
  [SerializeField]
  public GameObject noItemsConvoWithBop;
  [SerializeField]
  public GameObject recordPlayer;
  [SerializeField]
  public GameObject defaultBark;
  [SerializeField]
  public Interaction_SimpleConversation talkToBopConvo;
  [SerializeField]
  public Interaction_SimpleConversation broughtBopConvo;
  [SerializeField]
  public Interaction_SimpleConversation talkedWithBopConvo;
  public List<FollowerClothingType> broughtBopReward = new List<FollowerClothingType>()
  {
    FollowerClothingType.Normal_MajorDLC_1
  };
  public List<FollowerClothingType> talkedWithBopReward = new List<FollowerClothingType>()
  {
    FollowerClothingType.Normal_MajorDLC_3
  };
  public List<FollowerClothingType> multipleOutfitsToGive;
  [SerializeField]
  public SkeletonAnimation Worm1;
  [SerializeField]
  public SkeletonAnimation Worm2;
  [SerializeField]
  public SkeletonAnimation Worm3;
  [SerializeField]
  public SkeletonAnimation Worm4;
  [SerializeField]
  public SkeletonAnimation Worm5;
  [SerializeField]
  public SkeletonAnimation Worm6;
  public EventInstance loop;
  public List<ClothingData> Outfits;
  public List<FollowerClothingType> OutfitsAvailable;
  public int clothesForSalePerTimeSeeing;
  public bool noSkins;
  public InventoryItem.ITEM_TYPE currencyType = InventoryItem.ITEM_TYPE.GOLD_REFINED;
  public int currencyCost = 5;

  public void Start()
  {
    this.Setup();
    this.loop = AudioManager.Instance.CreateLoop("event:/atmos/misc/vinyl_crackle", this.recordPlayer, true);
    string skinName;
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_1:
        skinName = "4";
        this.currencyType = InventoryItem.ITEM_TYPE.GOLD_REFINED;
        this.currencyCost = 5;
        break;
      case FollowerLocation.Dungeon1_2:
        skinName = "2";
        this.currencyType = InventoryItem.ITEM_TYPE.GOLD_REFINED;
        this.currencyCost = 5;
        break;
      case FollowerLocation.Dungeon1_3:
        skinName = "3";
        this.currencyType = InventoryItem.ITEM_TYPE.GOLD_REFINED;
        this.currencyCost = 5;
        break;
      case FollowerLocation.Dungeon1_4:
        skinName = "1";
        this.currencyType = InventoryItem.ITEM_TYPE.GOLD_REFINED;
        this.currencyCost = 5;
        break;
      case FollowerLocation.Dungeon1_5:
        skinName = "5";
        this.currencyType = InventoryItem.ITEM_TYPE.NONE;
        this.currencyCost = 0;
        break;
      case FollowerLocation.Dungeon1_6:
        skinName = "6";
        this.currencyType = InventoryItem.ITEM_TYPE.NONE;
        this.currencyCost = 0;
        break;
      default:
        skinName = "6";
        this.currencyType = InventoryItem.ITEM_TYPE.NONE;
        this.currencyCost = 0;
        break;
    }
    if (!skinName.IsNullOrEmpty())
      this.Spine.skeleton.SetSkin(skinName);
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.TailorSystem) && DataManager.Instance.RevealedTailor && (PlayerFarming.Location == FollowerLocation.Dungeon1_5 || PlayerFarming.Location == FollowerLocation.Dungeon1_6) && DataManager.Instance.MAJOR_DLC && !DataManager.Instance.BerithTalkedWithBop)
    {
      bool flag = Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BOP) > 0;
      if (!ObjectiveManager.GroupExists("Objectives/GroupTitles/BerithAndBop", true) && !flag && !DataManager.Instance.LeftBopAtTailor)
      {
        this.talkToBopConvo.Callback.AddListener((UnityAction) (() =>
        {
          ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/BerithAndBop", Objectives.CustomQuestTypes.VisitKlunkoAndBop), isDLCObjective: true);
          DataManager.Instance.Alerts.Locations.Add(FollowerLocation.Hub1_RatauOutside);
        }));
        this.noItemsBark.SetActive(false);
        this.defaultBark.SetActive(false);
        this.talkToBopConvo.gameObject.SetActive(true);
      }
      else if (flag && !DataManager.Instance.LeftBopAtTailor)
      {
        this.aquisitionMethod = FollowerOutfitShop.OutfitAquisitionMethod.Reward;
        this.multipleOutfitsToGive = this.broughtBopReward;
        this.noItemsBark.SetActive(false);
        this.defaultBark.SetActive(false);
        this.broughtBopConvo.Callback.AddListener(new UnityAction(this.GiveBop));
        this.broughtBopConvo.gameObject.SetActive(true);
      }
      else if (DataManager.Instance.LeftBopAtTailor)
      {
        DataManager.Instance.BerithTalkedWithBop = true;
        this.aquisitionMethod = FollowerOutfitShop.OutfitAquisitionMethod.Reward;
        this.multipleOutfitsToGive = this.talkedWithBopReward;
        this.noItemsBark.SetActive(false);
        this.defaultBark.SetActive(false);
        this.talkedWithBopConvo.Callback.AddListener(new UnityAction(this.PlayGiveSkinSquence));
        this.talkedWithBopConvo.gameObject.SetActive(true);
        this.Spine.Skeleton.Skin.AddSkin(this.Spine.Skeleton.Data.FindSkin("Bop"));
        this.Spine.Skeleton.SetSlotsToSetupPose();
      }
    }
    if (DataManager.Instance.BerithTalkedWithBop)
    {
      this.Spine.Skeleton.Skin.AddSkin(this.Spine.Skeleton.Data.FindSkin("BopCool"));
      this.Spine.Skeleton.SetSlotsToSetupPose();
    }
    else
    {
      if (!DataManager.Instance.LeftBopAtTailor)
        return;
      this.Spine.Skeleton.Skin.AddSkin(this.Spine.Skeleton.Data.FindSkin("Bop"));
      this.Spine.Skeleton.SetSlotsToSetupPose();
    }
  }

  public void DestroyRecordPlayer()
  {
    AudioManager.Instance.StopLoop(this.loop);
    AudioManager.Instance.StopCurrentMusic();
  }

  public override void OnDestroy() => AudioManager.Instance.StopLoop(this.loop);

  public void Setup() => this.CheckOutfitAvailability();

  public override void OnDisable()
  {
    base.OnDisable();
    this.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.particles.gameObject.SetActive(false);
    this.ActivateDistance = 3f;
    this.CheckOutfitAvailability();
    if (this.OutfitsAvailable.Count == 0 && !this.SoldItem && this.aquisitionMethod != FollowerOutfitShop.OutfitAquisitionMethod.Reward)
      this.noItemsBark.gameObject.SetActive(true);
    this.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  public void CheckOutfitAvailability()
  {
    this.OutfitsAvailable = TailorManager.GetClothingToSell();
  }

  public int GetCost() => this.currencyCost;

  public override void GetLabel()
  {
    if (this.SoldItem)
      this.Label = "";
    else if (this.OutfitsAvailable.Count <= 0)
    {
      this.Interactable = false;
      this.noSkins = true;
      this.Label = ScriptLocalization.Interactions.SoldOut;
    }
    else
    {
      int cost = this.GetCost();
      if (cost > 0)
        this.Label = string.Join(" ", ScriptLocalization.Interactions.BuyRobeDesign, CostFormatter.FormatCost(this.currencyType, cost));
      else
        this.Label = string.Join(" ", LocalizationManager.GetTranslation("Interactions/FreeRobeDesign"));
    }
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.SoldItem)
      return;
    base.OnInteract(state);
    if (this.noSkins)
      this.NoSkins();
    else if (this.GetCost() <= Inventory.GetItemQuantity((int) this.currencyType) && !this.Activated)
    {
      AudioManager.Instance.PlayOneShot("event:/shop/buy", this.transform.position);
      this.Activated = true;
      this.Interactable = false;
      this.aquisitionMethod = FollowerOutfitShop.OutfitAquisitionMethod.Purchased;
      ++this.clothesForSalePerTimeSeeing;
      this.StartCoroutine(this.GiveSkin());
    }
    else
      this.CantAfford();
  }

  public void NoSkins()
  {
    AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.gameObject);
    MMVibrate.Haptic(MMVibrate.HapticTypes.Failure);
    this.playerFarming.indicator.PlayShake();
    this.noItemsBark.gameObject.SetActive(true);
    if ((UnityEngine.Object) this.shopKeeperManager.CantAffordBark != (UnityEngine.Object) null)
      this.shopKeeperManager.CantAffordBark.SetActive(false);
    if (!((UnityEngine.Object) this.shopKeeperManager.NormalBark != (UnityEngine.Object) null))
      return;
    this.shopKeeperManager.NormalBark.SetActive(false);
  }

  public void CantAfford()
  {
    AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.gameObject);
    MMVibrate.Haptic(MMVibrate.HapticTypes.Failure);
    this.playerFarming.indicator.PlayShake();
    if ((UnityEngine.Object) this.shopKeeperManager.CantAffordBark != (UnityEngine.Object) null)
    {
      if (MMConversation.isBark && (UnityEngine.Object) MMConversation.mmConversation != (UnityEngine.Object) null)
        MMConversation.mmConversation.Close();
      this.shopKeeperManager.CantAffordBark.SetActive(true);
      SimpleBark component = this.shopKeeperManager.CantAffordBark.gameObject.GetComponent<SimpleBark>();
      GameManager.GetInstance().WaitForSeconds(0.5f, new System.Action(component.Show));
    }
    if (!((UnityEngine.Object) this.shopKeeperManager.NormalBark != (UnityEngine.Object) null))
      return;
    this.shopKeeperManager.NormalBark.SetActive(false);
  }

  public void BoughtItem()
  {
    if ((UnityEngine.Object) this.shopKeeperManager.BoughtItemBark != (UnityEngine.Object) null)
      this.shopKeeperManager.BoughtItemBark.SetActive(true);
    if ((UnityEngine.Object) this.shopKeeperManager.NormalBark != (UnityEngine.Object) null)
      this.shopKeeperManager.NormalBark.SetActive(false);
    this.Activated = false;
    this.Interactable = true;
  }

  public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == "clothesMade")
    {
      GameManager.GetInstance().CameraSetTargetZoom(8f);
      this.particles.gameObject.SetActive(false);
      if (this.multipleOutfitsToGive != null && this.multipleOutfitsToGive.Count > 0)
        this.GiveMultipleOutfits();
      else
        this.GiveSingleOutfit();
      this.HasChanged = true;
    }
    else if (e.Data.Name == "audio/knit1")
      AudioManager.Instance.PlayOneShot("event:/material/knitting_needle", this.transform.position);
    else if (e.Data.Name == "audio/hatLand")
      AudioManager.Instance.PlayOneShot("event:/player/layer_clothes", this.transform.position);
    else if (e.Data.Name == "audio/hat bounce1")
    {
      AudioManager.Instance.PlayOneShot("event:/material/hat_bounce", this.transform.position);
    }
    else
    {
      if (!(e.Data.Name == "startMaking"))
        return;
      GameManager.GetInstance().CameraSetTargetZoom(6f);
      AudioManager.Instance.PlayOneShot("event:/material/knitting_needle", this.transform.position);
      this.particles.gameObject.SetActive(true);
      CameraManager.instance.ShakeCameraForDuration(0.1f, 1f, 1f);
      MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
      AudioManager.Instance.PlayOneShot("event:/player/layer_clothes", this.transform.position);
    }
  }

  public void PlayGiveSkinSquence()
  {
    this.talkedWithBopConvo.Callback.RemoveListener(new UnityAction(this.PlayGiveSkinSquence));
    this.StartCoroutine(this.GiveSkin());
  }

  public IEnumerator GiveSkin()
  {
    FollowerOutfitShop followerOutfitShop = this;
    MMConversation.mmConversation?.Close();
    while (MMConversation.isPlaying)
      yield return (object) null;
    followerOutfitShop.defaultBark.SetActive(false);
    MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew(followerOutfitShop.playerFarming);
    followerOutfitShop.playerFarming.GoToAndStop(followerOutfitShop.GoToPosition.transform.position, followerOutfitShop.gameObject, true, true, new System.Action(followerOutfitShop.\u003CGiveSkin\u003Eb__50_0));
    GameManager.GetInstance().CameraSetTargetZoom(8f);
    if (followerOutfitShop.aquisitionMethod == FollowerOutfitShop.OutfitAquisitionMethod.Purchased)
    {
      for (int i = 0; i < followerOutfitShop.GetCost(); ++i)
      {
        AudioManager.Instance.PlayOneShot("event:/followers/pop_in", followerOutfitShop.gameObject);
        ResourceCustomTarget.Create(followerOutfitShop.CoinTarget, followerOutfitShop.playerFarming.transform.position, followerOutfitShop.currencyType, (System.Action) null);
        Inventory.ChangeItemQuantity((int) followerOutfitShop.currencyType, -1);
        yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.05f, 0.1f));
      }
    }
    AudioManager.Instance.PlayOneShot("event:/shop/bebo_make_item", followerOutfitShop.gameObject);
    AudioManager.Instance.PlayOneShot("event:/dialogue/shop_tailor_bebo/short_yes_bebo", followerOutfitShop.gameObject);
    followerOutfitShop.Spine.AnimationState.ClearTracks();
    followerOutfitShop.Spine.AnimationState.SetAnimation(0, "idle_Jan", true);
    yield return (object) new WaitForSeconds(0.5f);
    followerOutfitShop.Spine.AnimationState.SetAnimation(0, "make-clothes", false);
    followerOutfitShop.Spine.AnimationState.AddAnimation(0, "idle_Jan", true, 0.0f);
  }

  public void PickedUp()
  {
    this.SecondaryInteractable = true;
    this.Activated = false;
    this.Interactable = true;
    this.CheckOutfitAvailability();
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().CameraResetTargetZoom();
    PlayerFarming.SetStateForAllPlayers();
    this.HasChanged = true;
    Interaction_SimpleConversation simpleConversation = (Interaction_SimpleConversation) null;
    if (!this.SoldItem)
    {
      if (this.OutfitsAvailable.Count == 0)
      {
        if (DataManager.Instance.LeftBopAtTailor && DataManager.Instance.MAJOR_DLC)
        {
          this.noItemsConvoWithBop.SetActive(true);
          simpleConversation = this.noItemsConvoWithBop.GetComponent<Interaction_SimpleConversation>();
        }
        else
        {
          this.noItemsConvo.SetActive(true);
          simpleConversation = this.noItemsConvo.GetComponent<Interaction_SimpleConversation>();
        }
      }
      else if (this.clothesForSalePerTimeSeeing >= 1)
      {
        this.comeBackLaterConvo.SetActive(true);
        this.SoldItem = true;
        simpleConversation = this.comeBackLaterConvo.GetComponent<Interaction_SimpleConversation>();
      }
    }
    if (!((UnityEngine.Object) simpleConversation != (UnityEngine.Object) null))
      return;
    this.Interactable = false;
    this.SecondaryInteractable = false;
    simpleConversation.Callback.AddListener((UnityAction) (() =>
    {
      this.Interactable = true;
      this.SecondaryInteractable = true;
    }));
  }

  public void StopVinylLoop()
  {
    this.Worm1.AnimationState.SetAnimation(0, "get-sad", false);
    this.Worm1.AnimationState.AddAnimation(0, "sad", true, 0.0f);
    this.Worm2.AnimationState.SetAnimation(0, "get-sad", false);
    this.Worm2.AnimationState.AddAnimation(0, "sad", true, 0.0f);
    this.Worm3.AnimationState.SetAnimation(0, "get-sad", false);
    this.Worm3.AnimationState.AddAnimation(0, "sad", true, 0.0f);
    this.Worm4.AnimationState.SetAnimation(0, "get-sad", false);
    this.Worm4.AnimationState.AddAnimation(0, "sad", true, 0.0f);
    this.Worm5.AnimationState.SetAnimation(0, "get-sad", false);
    this.Worm5.AnimationState.AddAnimation(0, "sad", true, 0.0f);
    this.Worm6.AnimationState.SetAnimation(0, "get-sad", false);
    this.Worm6.AnimationState.AddAnimation(0, "sad", true, 0.0f);
    this.Worm1.AnimationState.SetAnimation(0, "get-sad", false);
    this.Worm1.AnimationState.AddAnimation(0, "sad", true, 0.0f);
    AudioManager.Instance.StopLoop(this.loop);
  }

  public void GiveSingleOutfit()
  {
    if (this.aquisitionMethod == FollowerOutfitShop.OutfitAquisitionMethod.Reward)
      this.SoldItem = true;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", this.gameObject);
    FollowerOutfitCustomTarget.Create(this.gameObject.transform.position + new Vector3(0.0f, 0.0f, -1f), this.playerFarming.gameObject.transform.position, 2f, this.OutfitsAvailable[UnityEngine.Random.Range(0, this.OutfitsAvailable.Count)], new System.Action(this.PickedUp));
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
    RumbleManager.Instance.Rumble();
    if (this.aquisitionMethod == FollowerOutfitShop.OutfitAquisitionMethod.Purchased)
    {
      this.BoughtItem();
    }
    else
    {
      this.Activated = false;
      this.Interactable = true;
    }
    this.aquisitionMethod = FollowerOutfitShop.OutfitAquisitionMethod.None;
  }

  public void GiveMultipleOutfits() => this.StartCoroutine(this.GiveMultipleOutfitsSequence());

  public IEnumerator GiveMultipleOutfitsSequence()
  {
    FollowerOutfitShop followerOutfitShop = this;
    int outfitsGiven = 0;
    for (int x = 0; x < followerOutfitShop.multipleOutfitsToGive.Count; ++x)
    {
      AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", followerOutfitShop.gameObject);
      FollowerOutfitCustomTarget.Create(followerOutfitShop.gameObject.transform.position + new Vector3(0.0f, 0.0f, -1f), followerOutfitShop.playerFarming.gameObject.transform.position, 2f, followerOutfitShop.multipleOutfitsToGive[x], (System.Action) (() => ++outfitsGiven));
      BiomeConstants.Instance.EmitSmokeExplosionVFX(followerOutfitShop.transform.position);
      RumbleManager.Instance.Rumble();
      while (outfitsGiven == x)
        yield return (object) null;
      HUD_Manager.Instance.Hide(true, 0);
      PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
      followerOutfitShop.playerFarming.Spine.AnimationState.SetAnimation(0, "idle", true);
      followerOutfitShop.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    }
    if (followerOutfitShop.aquisitionMethod == FollowerOutfitShop.OutfitAquisitionMethod.Reward)
      followerOutfitShop.SoldItem = true;
    followerOutfitShop.PickedUp();
    followerOutfitShop.multipleOutfitsToGive = (List<FollowerClothingType>) null;
    if (followerOutfitShop.aquisitionMethod == FollowerOutfitShop.OutfitAquisitionMethod.Purchased)
    {
      followerOutfitShop.BoughtItem();
    }
    else
    {
      followerOutfitShop.Activated = false;
      followerOutfitShop.Interactable = true;
    }
    followerOutfitShop.aquisitionMethod = FollowerOutfitShop.OutfitAquisitionMethod.None;
  }

  public void GiveBop()
  {
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BringBopToBerith);
    PickUp bopItem = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BOP, 1, MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.transform.position);
    bopItem.enabled = false;
    TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = bopItem.transform.DOMove(this.transform.position + Vector3.right + Vector3.back, 0.8f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    tweenerCore.onComplete = tweenerCore.onComplete + (TweenCallback) (() =>
    {
      BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position + Vector3.right);
      this.Spine.Skeleton.Skin.AddSkin(this.Spine.Skeleton.Data.FindSkin("Bop"));
      this.Spine.Skeleton.SetSlotsToSetupPose();
      UnityEngine.Object.Destroy((UnityEngine.Object) bopItem.gameObject);
      Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.BOP, -1);
      DataManager.Instance.LeftBopAtTailor = true;
      this.PlayGiveSkinSquence();
      this.CheckOutfitAvailability();
    });
  }

  [CompilerGenerated]
  public void \u003CGiveSkin\u003Eb__50_0()
  {
    this.playerFarming.Spine.AnimationState.SetAnimation(0, "idle", true);
    this.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
  }

  [CompilerGenerated]
  public void \u003CPickedUp\u003Eb__51_0()
  {
    this.Interactable = true;
    this.SecondaryInteractable = true;
  }

  public enum OutfitAquisitionMethod
  {
    None,
    Purchased,
    Reward,
  }
}
