// Decompiled with JetBrains decompiler
// Type: Interaction_SacrificeTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.RanchSelect;
using MMTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_SacrificeTable : Interaction
{
  public static Interaction_SacrificeTable Instance;
  [SerializeField]
  public Interaction_SimpleConversation waitForBlizzardConversation;
  [SerializeField]
  public List<Interaction_SacrificeTable.Item> items;
  [SerializeField]
  public SpriteRenderer rightSprite;
  [SerializeField]
  public SpriteRenderer leftSprite;
  [SerializeField]
  public Animator offeringReady;
  [SerializeField]
  public InventoryItemDisplay reward;
  [SerializeField]
  public GameObject animalPos;
  [SerializeField]
  public GameObject blood;
  [SerializeField]
  public GameObject sacrificeRanchAnimal;
  [SerializeField]
  public GameObject completedRitual;
  [SerializeField]
  public GameObject icegoreCameraTarget;
  [SerializeField]
  public IcegoreController icegoreController;
  [SerializeField]
  public Transform icegoreStartWaypoint;
  [SerializeField]
  public Transform icegorePickupWaypoint;
  [SerializeField]
  public Transform icegoreEndWaypoint;
  public ProgressBarWidget progressBarWidget;

  public bool IsBlizzard
  {
    get => DataManager.Instance.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard;
  }

  public bool IsComplete => DataManager.Instance.BlizzardOfferingsCompleted >= 5;

  public void Awake()
  {
    this.progressBarWidget = this.GetComponentInChildren<ProgressBarWidget>();
    this.icegoreController.Hide();
    SeasonsManager.OnWeatherBegan += new SeasonsManager.WeatherTypeEvent(this.SeasonsManager_OnWeatherBegan);
    SeasonsManager.OnWeatherEnded += new SeasonsManager.WeatherTypeEvent(this.SeasonsManager_OnWeatherBegan);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    SeasonsManager.OnWeatherBegan -= new SeasonsManager.WeatherTypeEvent(this.SeasonsManager_OnWeatherBegan);
    SeasonsManager.OnWeatherEnded -= new SeasonsManager.WeatherTypeEvent(this.SeasonsManager_OnWeatherBegan);
  }

  public void SeasonsManager_OnWeatherBegan(SeasonsManager.WeatherEvent weatherEvent)
  {
    this.offeringReady.gameObject.SetActive(weatherEvent == SeasonsManager.WeatherEvent.Blizzard && !DataManager.Instance.CompletedOfferingThisBlizzard);
  }

  public void Start()
  {
    Interaction_SacrificeTable.Instance = this;
    this.SetSprites();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.SetSprites();
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Interactable = true;
    if (DataManager.Instance.SacrificeTableInventory.Count > 0)
      this.Label = $"{ScriptLocalization.Interactions.Collect} {FontImageNames.GetIconByType((InventoryItem.ITEM_TYPE) DataManager.Instance.SacrificeTableInventory[0].type)}";
    else if (this.IsBlizzard)
    {
      if (DataManager.Instance.CompletedOfferingThisBlizzard)
      {
        this.Interactable = false;
        this.Label = ScriptLocalization.Interactions.Recharging;
      }
      else
        this.Label = this.Interactable ? LocalizationManager.GetTranslation("Interactions/PlaceOffering") : ScriptLocalization.Interactions.Recharging;
    }
    else
      this.Label = ScriptLocalization.Interactions.Read;
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    string str = ScriptLocalization.Interactions.Requires + ": ";
    for (int index = 0; index < DataManager.Instance.BlizzardOfferingRequirements.Count; ++index)
    {
      bool flag = DataManager.Instance.BlizzardOfferingsGiven[index].Quantity >= DataManager.Instance.BlizzardOfferingRequirements[index].Quantity;
      str = $"{str}{$"{DataManager.Instance.BlizzardOfferingsGiven[index].Quantity.ToString().Colour(flag ? StaticColors.OffWhiteColor : StaticColors.RedColor)}/{DataManager.Instance.BlizzardOfferingRequirements[index].Quantity} "}{FontImageNames.GetIconByType(DataManager.Instance.BlizzardOfferingRequirements[index].Type)} ";
    }
  }

  public bool RanchAnimalsAvailable()
  {
    foreach (Interaction_Ranch ranch in Interaction_Ranch.Ranches)
    {
      if (ranch.Brain.Data.Animals.Count > 0)
        return true;
    }
    return false;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.playerFarming.IsLeashingAnimal() || this.playerFarming.IsRidingAnimal())
    {
      Interaction_Ranchable animal = (Interaction_Ranchable) null;
      if (this.playerFarming.IsLeashingAnimal())
      {
        animal = this.playerFarming.GetLeashingAnimal();
        this.playerFarming.StopLeashingAnimal();
      }
      else if (this.playerFarming.IsRidingAnimal())
      {
        animal = this.playerFarming.GetRidingAnimal();
        this.playerFarming.StopRidingOnAnimal();
      }
      this.SacrificeAnimal(animal);
    }
    else if (DataManager.Instance.SacrificeTableInventory.Count > 0)
      this.OnCollectNecklace();
    else if (!this.IsBlizzard)
    {
      if (this.waitForBlizzardConversation.Spoken)
        this.waitForBlizzardConversation.ResetConvo();
      this.waitForBlizzardConversation.Play();
    }
    else
      this.OnInteractSacrifice();
  }

  public void OnCollectNecklace()
  {
    InventoryItem.ITEM_TYPE type = (InventoryItem.ITEM_TYPE) DataManager.Instance.SacrificeTableInventory[0].type;
    Inventory.AddItem(type, 1);
    MonoSingleton<UIManager>.Instance.ShowNewItemOverlay().Show(UINewItemOverlayController.TypeOfCard.Necklace, this.transform.position, type);
    DataManager.Instance.SacrificeTableInventory.Clear();
    this.SetSprites(false);
    this.HasChanged = true;
  }

  public void OnInteractSacrifice()
  {
    List<Structures_Ranch> ranches = StructureManager.GetAllStructuresOfType<Structures_Ranch>();
    List<RanchSelectEntry> animal1 = new List<RanchSelectEntry>();
    foreach (StructureBrain structureBrain in ranches)
    {
      foreach (StructuresData.Ranchable_Animal animal2 in structureBrain.Data.Animals)
      {
        if (animal2.Age >= 2)
        {
          RanchSelectEntry ranchSelectEntry = new RanchSelectEntry(animal2, showNecklaceReward: this.IsComplete);
          if (animal2.State != Interaction_Ranchable.State.Dead)
            animal1.Add(ranchSelectEntry);
        }
      }
    }
    UIRanchSelectMenuController itemSelector = MonoSingleton<UIManager>.Instance.ShowRanchSacrificeMenu(animal1);
    UIRanchSelectMenuController selectMenuController1 = itemSelector;
    selectMenuController1.OnAnimalSelected = selectMenuController1.OnAnimalSelected + (System.Action<RanchSelectEntry>) (sacrificedRanchable =>
    {
      int id = sacrificedRanchable.AnimalInfo.ID;
      int type = (int) sacrificedRanchable.AnimalInfo.Type;
      int age = sacrificedRanchable.AnimalInfo.Age;
      foreach (Structures_Ranch structuresRanch in ranches)
        structuresRanch.RemoveAnimal(sacrificedRanchable.AnimalInfo);
      this.SacrificeAnimal(Interaction_Ranchable.Ranchables.Find((Predicate<Interaction_Ranchable>) (x => x.Animal.ID == id)));
      itemSelector.Hide();
    });
    UIRanchSelectMenuController selectMenuController2 = itemSelector;
    selectMenuController2.OnShow = selectMenuController2.OnShow + (System.Action) (() =>
    {
      if (!this.IsComplete)
        return;
      DOVirtual.DelayedCall(0.1f, (TweenCallback) (() =>
      {
        foreach (RanchMenuItem followerInfoBox in itemSelector.FollowerInfoBoxes)
          followerInfoBox.ShowDropItem();
      }));
    });
    UIRanchSelectMenuController selectMenuController3 = itemSelector;
    selectMenuController3.OnShown = selectMenuController3.OnShown + (System.Action) (() =>
    {
      if (!this.IsComplete)
        return;
      DOVirtual.DelayedCall(0.1f, (TweenCallback) (() =>
      {
        foreach (RanchMenuItem followerInfoBox in itemSelector.FollowerInfoBoxes)
          followerInfoBox.ShowDropItem();
      }));
    });
  }

  public void SetSprites(bool updateProgress = true)
  {
    if ((UnityEngine.Object) this.progressBarWidget.SkeletonAnimation == (UnityEngine.Object) null)
      return;
    this.blood.gameObject.SetActive(DataManager.Instance.CompletedOfferingThisBlizzard);
    List<InventoryItem> sacrificeTableInventory = DataManager.Instance.SacrificeTableInventory;
    this.reward.gameObject.SetActive(false);
    if (sacrificeTableInventory.Count > 0)
    {
      this.reward.SetImage((InventoryItem.ITEM_TYPE) sacrificeTableInventory[0].type);
      this.reward.gameObject.SetActive(true);
    }
    if (updateProgress)
    {
      int offeringsCompleted = DataManager.Instance.BlizzardOfferingsCompleted;
      if (offeringsCompleted > 0)
        this.progressBarWidget.SetProgress(offeringsCompleted);
      else
        this.progressBarWidget.SetInactivate();
    }
    this.offeringReady.gameObject.SetActive(SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard && !DataManager.Instance.CompletedOfferingThisBlizzard);
  }

  public void SacrificeAnimal(Interaction_Ranchable animal)
  {
    this.StartCoroutine((IEnumerator) this.SacrificeRanchableRoutine(animal, this.IsComplete));
  }

  public static IEnumerator FadeIn()
  {
    bool waitingForFade = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 1f, "", (System.Action) (() => waitingForFade = false));
    while (waitingForFade)
      yield return (object) null;
  }

  public static IEnumerator FadeOut()
  {
    bool waitingForFade = true;
    MMTransition.ResumePlay((System.Action) (() => waitingForFade = false));
    while (waitingForFade)
      yield return (object) null;
  }

  public void PlaceItemInShrine(InventoryItem.ITEM_TYPE itemType)
  {
    DataManager.Instance.SacrificeTableInventory.Add(new InventoryItem(itemType, 1));
    this.SetSprites(false);
  }

  public void PlaySacrificeRanchableSequence(bool complete)
  {
    this.StartCoroutine((IEnumerator) this.SacrificeRanchableRoutine((Interaction_Ranchable) null, complete));
  }

  public IEnumerator SacrificeRanchableRoutine(Interaction_Ranchable animal, bool isComplete)
  {
    Interaction_SacrificeTable interactionSacrificeTable = this;
    MonoSingleton<UIManager>.Instance.ForceBlockPause = true;
    AudioManager.Instance.PlayOneShot("event:/dlc/env/icegore/sacrifice_confirm", interactionSacrificeTable.transform.position);
    interactionSacrificeTable.icegoreController.transform.position = interactionSacrificeTable.icegoreStartWaypoint.position;
    interactionSacrificeTable.Outliner.OutlineLayers[0].Clear();
    if ((UnityEngine.Object) animal != (UnityEngine.Object) null)
    {
      animal.CurrentState = Interaction_Ranchable.State.Default;
      animal.UnitObject.enabled = false;
      animal.gameObject.SetActive(true);
      animal.transform.SetParent(interactionSacrificeTable.transform);
    }
    AudioManager.Instance.StopCurrentMusic();
    EventInstance introInstance = AudioManager.Instance.PlayOneShotWithInstance("event:/music/intro/intro_bass");
    Vector3 position1 = interactionSacrificeTable.transform.position;
    --position1.y;
    position1.x -= 2f;
    interactionSacrificeTable.playerFarming.GoToAndStop(position1, interactionSacrificeTable.gameObject);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionSacrificeTable.animalPos.gameObject, 6f);
    if ((UnityEngine.Object) animal != (UnityEngine.Object) null)
    {
      animal.CurrentState = Interaction_Ranchable.State.Overcrowded;
      animal.transform.position = interactionSacrificeTable.animalPos.transform.position;
      animal.UnitObject.LockToGround = false;
    }
    interactionSacrificeTable.offeringReady.SetTrigger("Hide");
    yield return (object) new WaitForSeconds(1f);
    int offeringsCompleted = DataManager.Instance.BlizzardOfferingsCompleted;
    InventoryItem.ITEM_TYPE necklaceToDrop = isComplete ? InventoryItem.ITEM_TYPE.NONE : InventoryItem.Necklaces_DLC[offeringsCompleted];
    if (isComplete && (UnityEngine.Object) animal != (UnityEngine.Object) null)
      necklaceToDrop = (InventoryItem.ITEM_TYPE) Interaction_Ranchable.GetNecklaceLoot(animal.Animal).type;
    AudioManager.Instance.PlayOneShot("event:/dlc/env/icegore/sacrifice_start", interactionSacrificeTable.transform.position);
    interactionSacrificeTable.icegoreController.Show();
    yield return (object) interactionSacrificeTable.icegoreController.WalkTo(interactionSacrificeTable.icegorePickupWaypoint.position, 1.5f);
    GameManager.GetInstance().WaitForSeconds(0.5f, new System.Action(interactionSacrificeTable.\u003CSacrificeRanchableRoutine\u003Eb__39_0));
    yield return (object) interactionSacrificeTable.icegoreController.Roar();
    yield return (object) interactionSacrificeTable.icegoreController.Pickup(animal?.gameObject);
    yield return (object) new WaitForSeconds(0.5f);
    interactionSacrificeTable.StartCoroutine((IEnumerator) interactionSacrificeTable.icegoreController.WalkTo(interactionSacrificeTable.icegoreEndWaypoint.position, 1.5f));
    yield return (object) new WaitForSeconds(3f);
    animal.PlayDieIcegoreVO();
    interactionSacrificeTable.offeringReady.gameObject.SetActive(false);
    GameManager.GetInstance().OnConversationNext(interactionSacrificeTable.animalPos.gameObject, 6f);
    interactionSacrificeTable.icegoreController.Hide();
    if ((UnityEngine.Object) animal != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) animal.gameObject);
    yield return (object) new WaitForSecondsRealtime(1f);
    interactionSacrificeTable.PlaceItemInShrine(necklaceToDrop);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionSacrificeTable.reward.transform.position);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/icegore/item_poof", interactionSacrificeTable.reward.transform.position);
    yield return (object) new WaitForSeconds(2f);
    DataManager.Instance.CompletedOfferingThisBlizzard = true;
    if (!interactionSacrificeTable.IsComplete)
    {
      GameManager.GetInstance().OnConversationNext(interactionSacrificeTable.animalPos.gameObject, 3f);
      yield return (object) new WaitForSeconds(1.5f);
      AudioManager.Instance.PlayOneShot("event:/dlc/env/icegore/bar_fillup");
      yield return (object) interactionSacrificeTable.progressBarWidget.ProgressNext();
      yield return (object) new WaitForSeconds(0.5f);
    }
    if (DataManager.Instance.BlizzardOfferingsCompleted < 5 && DataManager.Instance.BlizzardOfferingsCompleted + 1 >= 5)
    {
      GameManager.GetInstance().OnConversationNext(interactionSacrificeTable.animalPos.gameObject, 6f);
      Vector3 position2 = interactionSacrificeTable.transform.position;
      --position2.y;
      interactionSacrificeTable.playerFarming.GoToAndStop(position2, interactionSacrificeTable.gameObject);
      yield return (object) new WaitForSeconds(1f);
      CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 1f);
      AudioManager.Instance.PlayOneShot("event:/dlc/env/icegore/powerup_full");
      yield return (object) new WaitForSeconds(1f);
      interactionSacrificeTable.completedRitual.gameObject.SetActive(true);
      CameraManager.shakeCamera(3f, 0.0f);
      BiomeConstants.Instance.ImpactFrameForDuration();
      BiomeConstants.Instance.EmitDisplacementEffect(interactionSacrificeTable.transform.position);
      AudioManager.Instance.PlayOneShot("event:/rituals/blood_sacrifice", interactionSacrificeTable.gameObject);
      AudioManager.Instance.PlayOneShot("event:/Stings/DLC_IntroBell", interactionSacrificeTable.gameObject);
      AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/trap/flesh_rock/block_petrified_destroy", interactionSacrificeTable.gameObject);
      AudioManager.Instance.PlayOneShot("event:/Stings/bishop_healed", interactionSacrificeTable.gameObject);
      GameManager.GetInstance().OnConversationNext(interactionSacrificeTable.animalPos.gameObject, 12f);
      yield return (object) new WaitForSeconds(2f);
      UINewItemOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowNewItemOverlay();
      overlayController.Show(UINewItemOverlayController.TypeOfCard.FollowerSkin, PlayerFarming.Instance.gameObject.transform.position, "IceGore");
      overlayController.OnHidden = overlayController.OnHidden + new System.Action(interactionSacrificeTable.\u003CSacrificeRanchableRoutine\u003Eb__39_2);
    }
    else
      GameManager.GetInstance().OnConversationEnd();
    ++DataManager.Instance.BlizzardOfferingsCompleted;
    AudioManager.Instance.StopOneShotInstanceEarly(introInstance, STOP_MODE.ALLOWFADEOUT);
    AudioManager.Instance.PlayMusic("event:/music/base/base_main", false);
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.woolhaven);
    GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() => AudioManager.Instance.StartMusic()));
    MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
  }

  [CompilerGenerated]
  public void \u003CSacrificeRanchableRoutine\u003Eb__39_0()
  {
    GameManager.GetInstance().OnConversationNext(this.animalPos.gameObject, 12f);
  }

  [CompilerGenerated]
  public void \u003CSacrificeRanchableRoutine\u003Eb__39_2()
  {
    this.completedRitual.gameObject.SetActive(false);
    GameManager.GetInstance().OnConversationEnd();
  }

  [Serializable]
  public struct Item
  {
    public InventoryItem.ITEM_TYPE type;
    public Sprite rightSprite;
    public Sprite leftSprite;
  }
}
