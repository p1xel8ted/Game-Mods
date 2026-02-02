// Decompiled with JetBrains decompiler
// Type: Interaction_MysticShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using MMTools;
using Spine.Unity;
using src.Extensions;
using src.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_MysticShop : Interaction
{
  public const int maxAmountOfTalismanPieces = 12;
  public const int maxAmountOfCrystalDoctrines = 24;
  [SerializeField]
  public Interaction_SimpleConversation onboardGodTears;
  [SerializeField]
  public Interaction_SimpleConversation onboardEndlessModeA;
  [SerializeField]
  public Interaction_SimpleConversation onboardEndlessModeB;
  [SerializeField]
  public Interaction_SimpleConversation[] enterMysticDimension;
  [SerializeField]
  public Interaction_SimpleConversation beatenLeshy;
  [SerializeField]
  public Interaction_SimpleConversation beatenHeket;
  [SerializeField]
  public Interaction_SimpleConversation beatenKallamar;
  [SerializeField]
  public Interaction_SimpleConversation beatenShamura;
  [SerializeField]
  public Interaction_SimpleConversation beatenAllA;
  [SerializeField]
  public Interaction_SimpleConversation beatenAllB;
  [SerializeField]
  public Interaction_SimpleConversation beatenAllC;
  [SerializeField]
  public Interaction_SimpleConversation deathCatConvo;
  [SerializeField]
  public Interaction_SimpleConversation firstPurchaseConvo;
  [SerializeField]
  public Interaction_SimpleConversation sinOnboardedConvo;
  [SerializeField]
  public Interaction_SimpleConversation winterAwakenedConvo;
  [SerializeField]
  public Interaction_SimpleConversation beatenYngyaConvo;
  [SerializeField]
  public SimpleBark boughtBark;
  [SerializeField]
  public SimpleBark noGodTearsBark;
  [SerializeField]
  public SimpleSetCamera simpleCamera;
  [SerializeField]
  public SimpleSetCamera simpleCameraTwo;
  [SerializeField]
  public Reveal_MysticShop mysticShopReveal;
  [SerializeField]
  public SkeletonAnimation mysticKeeperSpine;
  [SerializeField]
  public GameObject cameraPos;
  [SerializeField]
  public GameObject followerToSpawn;
  [SerializeField]
  public SkeletonAnimation portalSpine;
  [SerializeField]
  public ParticleSystem recruitParticles;
  [SerializeField]
  public GameObject deathCatTargetPosition;
  [Header("God Tear Sequencing")]
  [SerializeField]
  public GameObject _godTearPrefab;
  [SerializeField]
  public Transform _godTearTarget;
  [SerializeField]
  public Interaction_KeyPiece _keyPiecePrefab;
  [SerializeField]
  public GameObject LUTOverlay;
  public bool waitingForContinueSequence = true;
  public int GodTearCost = 1;
  public List<string> possibleSkins;
  public List<StructureBrain.TYPES> possibleDecos;
  public List<InventoryItem.ITEM_TYPE> necklaces;
  public List<InventoryItem.ITEM_TYPE> allNecklaces = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.Necklace_Dark,
    InventoryItem.ITEM_TYPE.Necklace_Demonic,
    InventoryItem.ITEM_TYPE.Necklace_Gold_Skull,
    InventoryItem.ITEM_TYPE.Necklace_Light,
    InventoryItem.ITEM_TYPE.Necklace_Loyalty,
    InventoryItem.ITEM_TYPE.Necklace_Missionary
  };
  public static RelicType RelicToUnlock;
  [SerializeField]
  public Transform playerPosition;
  public EventInstance loopedSound;
  public GameObject LightingOverrideAngry;
  public EventInstance loopedSoundDeathCat;

  public override bool InactiveAfterStopMoving => false;

  public string mysticKeeperName
  {
    get
    {
      return string.IsNullOrEmpty(DataManager.Instance.MysticKeeperName) ? "???" : DataManager.Instance.MysticKeeperName;
    }
    set
    {
      DataManager.Instance.MysticKeeperName = value;
      this.SetConvosName();
    }
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.SetCost();
  }

  public void TestGodTear()
  {
    this.onboardGodTears.enabled = true;
    this.onboardGodTears.AutomaticallyInteract = true;
    this.simpleCamera.enabled = false;
  }

  public void TestOnboardEndless()
  {
    this.onboardEndlessModeA.enabled = true;
    foreach (ConversationEntry entry in this.onboardEndlessModeA.Entries)
      entry.CharacterName = this.mysticKeeperName;
  }

  public void Start()
  {
    if (DataManager.Instance.OnboardedMysticShop)
      this.ActivateDistance = 6f;
    this.SetConvosName();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.onboardGodTears.enabled = false;
    this.onboardEndlessModeA.enabled = false;
    this.onboardEndlessModeB.enabled = false;
    if (DataManager.Instance.OnboardedMysticShop && !DataManager.Instance.OnboardedGodTear && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.GOD_TEAR) >= 1)
    {
      this.onboardGodTears.enabled = true;
      this.onboardGodTears.AutomaticallyInteract = true;
      this.simpleCamera.enabled = false;
    }
    else if (!DataManager.Instance.OnboardedEndlessMode && DataManager.GetGodTearNotchesTotal() >= 4)
    {
      this.onboardEndlessModeA.enabled = true;
      foreach (ConversationEntry entry in this.onboardEndlessModeA.Entries)
        entry.CharacterName = this.mysticKeeperName;
    }
    else if (!DataManager.Instance.MysticKeeperBeatenLeshy && DataManager.Instance.BeatenLeshyLayer2)
      this.beatenLeshy.enabled = true;
    else if (!DataManager.Instance.MysticKeeperBeatenHeket && DataManager.Instance.BeatenHeketLayer2)
      this.beatenHeket.enabled = true;
    else if (!DataManager.Instance.MysticKeeperBeatenKallamar && DataManager.Instance.BeatenKallamarLayer2)
      this.beatenKallamar.enabled = true;
    else if (!DataManager.Instance.MysticKeeperBeatenShamura && DataManager.Instance.BeatenShamuraLayer2)
      this.beatenShamura.enabled = true;
    else if (!DataManager.Instance.MysticKeeperBeatenYngya && DataManager.Instance.BeatenYngya)
      this.beatenYngyaConvo.enabled = true;
    if (!DataManager.Instance.MysticKeeperBeatenAll && DataManager.Instance.BeatenLeshyLayer2 && DataManager.Instance.BeatenHeketLayer2 && DataManager.Instance.BeatenKallamarLayer2 && DataManager.Instance.BeatenShamuraLayer2)
    {
      this.beatenAllA.enabled = true;
      this.beatenAllA.Entries[3].TermToSpeak = !StructuresData.GetUnlocked(StructureBrain.TYPES.DECORATION_BOSS_TROPHY_5) ? "Conversation_NPC/MysticShopSeller/BeatenAll/3_Shrine" : "Conversation_NPC/MysticShopSeller/BeatenAll/3_Follower";
    }
    if (DataManager.Instance.PleasureEnabled && !DataManager.Instance.MysticKeeperOnboardedSin)
      this.sinOnboardedConvo.enabled = true;
    if (!DataManager.Instance.OnboardedMysticShop || !SeasonsManager.Active || DataManager.Instance.SpokenToMysticKeeperWinter)
      return;
    this.winterAwakenedConvo.enabled = true;
  }

  public void SetConvosName()
  {
    foreach (Interaction_SimpleConversation simpleConversation in this.enterMysticDimension)
    {
      foreach (ConversationEntry entry in simpleConversation.Entries)
        entry.CharacterName = this.mysticKeeperName;
    }
    foreach (ConversationEntry entry in this.boughtBark.Entries)
      entry.CharacterName = this.mysticKeeperName;
    foreach (ConversationEntry entry in this.noGodTearsBark.Entries)
      entry.CharacterName = this.mysticKeeperName;
    foreach (ConversationEntry entry in this.beatenLeshy.Entries)
      entry.CharacterName = this.mysticKeeperName;
    foreach (ConversationEntry entry in this.beatenHeket.Entries)
      entry.CharacterName = this.mysticKeeperName;
    foreach (ConversationEntry entry in this.beatenKallamar.Entries)
      entry.CharacterName = this.mysticKeeperName;
    foreach (ConversationEntry entry in this.beatenShamura.Entries)
      entry.CharacterName = this.mysticKeeperName;
    foreach (ConversationEntry entry in this.beatenAllA.Entries)
      entry.CharacterName = this.mysticKeeperName;
    foreach (ConversationEntry entry in this.beatenAllB.Entries)
      entry.CharacterName = this.mysticKeeperName;
    foreach (ConversationEntry entry in this.beatenAllC.Entries)
      entry.CharacterName = this.mysticKeeperName;
    foreach (ConversationEntry entry in this.firstPurchaseConvo.Entries)
      entry.CharacterName = this.mysticKeeperName;
    foreach (ConversationEntry entry in this.sinOnboardedConvo.Entries)
      entry.CharacterName = this.mysticKeeperName;
    foreach (ConversationEntry entry in this.winterAwakenedConvo.Entries)
      entry.CharacterName = this.mysticKeeperName;
    foreach (ConversationEntry entry in this.beatenYngyaConvo.Entries)
      entry.CharacterName = this.mysticKeeperName;
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (DataManager.Instance.OnboardedMysticShop)
    {
      if (DataManager.Instance.OnboardedGodTear)
      {
        this.AutomaticallyInteract = false;
        this.Label = $"{LocalizationManager.GetTranslation("Interactions/GiveGodTear")}: {InventoryItem.CapacityString(InventoryItem.ITEM_TYPE.GOD_TEAR, this.GodTearCost)}";
      }
      else
        this.Label = "";
    }
    else
      this.Label = ".";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Interactable = false;
    if (DataManager.Instance.OnboardedMysticShop && DataManager.Instance.OnboardedGodTear)
    {
      if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.GOD_TEAR) >= this.GodTearCost)
      {
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.MysticShopReturn);
        this.StartCoroutine((IEnumerator) this.DoRewardSequence());
      }
      else
      {
        this.playerFarming.indicator.PlayShake();
        this.noGodTearsBark.gameObject.SetActive(true);
        this.noGodTearsBark.Show();
        this.Interactable = true;
      }
    }
    else
    {
      if (DataManager.Instance.OnboardedMysticShop)
        return;
      this.mysticShopReveal.Reveal();
      this.enabled = false;
      this.ActivateDistance = 6f;
    }
  }

  public void NameMysticKeeper()
  {
    this.StartCoroutine((IEnumerator) UIManager.LoadAssets(MonoSingleton<UIManager>.Instance.LoadMysticSellerNameMenuAssets(), (System.Action) (() =>
    {
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.MysticShopReturn);
      UIMysticSellerNameMenuController mysticSellerNameMenu = MonoSingleton<UIManager>.Instance.MysticSellerNameMenuTemplate.Instantiate<UIMysticSellerNameMenuController>();
      mysticSellerNameMenu.Show(false, false);
      UIMysticSellerNameMenuController nameMenuController1 = mysticSellerNameMenu;
      nameMenuController1.OnNameConfirmed = nameMenuController1.OnNameConfirmed + (System.Action<string>) (name =>
      {
        this.mysticKeeperName = name;
        MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
        {
          new ConversationEntry(this.cameraPos, LocalizationManager.GetTranslation("Conversation_NPC/MysticShopSeller/GodTearIntro/4"))
        }, (List<MMTools.Response>) null, (System.Action) (() =>
        {
          this.simpleCamera.enabled = true;
          this.StopMusic();
        })));
      });
      UIMysticSellerNameMenuController nameMenuController2 = mysticSellerNameMenu;
      nameMenuController2.OnHide = nameMenuController2.OnHide + (System.Action) (() => { });
      UIMysticSellerNameMenuController nameMenuController3 = mysticSellerNameMenu;
      nameMenuController3.OnHidden = nameMenuController3.OnHidden + (System.Action) (() => mysticSellerNameMenu = (UIMysticSellerNameMenuController) null);
      DataManager.Instance.OnboardedGodTear = true;
    })));
  }

  public void GiveDefeatBishopsQuest()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/NewGamePlus", Objectives.CustomQuestTypes.NewGamePlus1), true);
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/NewGamePlus", Objectives.CustomQuestTypes.NewGamePlus2), true);
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/NewGamePlus", Objectives.CustomQuestTypes.NewGamePlus3), true);
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/NewGamePlus", Objectives.CustomQuestTypes.NewGamePlus4), true);
    this.enabled = true;
    DataManager.Instance.UnlockedBossTempleDoor.Clear();
    DataManager.Instance.OnboardedLayer2 = true;
  }

  public void CheckWinterAwaken()
  {
    if (!SeasonsManager.Active || DataManager.Instance.SpokenToMysticKeeperWinter)
      return;
    this.winterAwakenedConvo.enabled = true;
    this.winterAwakenedConvo.Play();
  }

  public void UnlockEndlessMode()
  {
    this.simpleCamera.Reset();
    this.simpleCamera.AutomaticallyActivate = false;
    this.onboardGodTears.enabled = false;
    CrownStatueController.Instance.EndlessModeOnboarded((System.Action) (() =>
    {
      foreach (ConversationEntry entry in this.onboardEndlessModeB.Entries)
        entry.CharacterName = this.mysticKeeperName;
      this.onboardEndlessModeB.Entries[this.onboardEndlessModeB.Entries.Count - 1].TermToSpeak = string.Format(LocalizationManager.GetTranslation("Conversation_NPC/MysticShopSeller/EndlessModeIntro/4"), (object) this.mysticKeeperName);
      this.onboardEndlessModeB.Play();
      this.onboardEndlessModeB.Callback.AddListener((UnityAction) (() =>
      {
        this.StopMusic();
        this.simpleCamera.AutomaticallyActivate = false;
      }));
    }));
  }

  public IEnumerator DoRewardSequence()
  {
    Interaction_MysticShop interactionMysticShop = this;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
    interactionMysticShop.PlayMusic();
    interactionMysticShop.simpleCameraTwo.enabled = false;
    GameManager.GetInstance().OnConversationNew();
    Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.GOD_TEAR, -interactionMysticShop.GodTearCost);
    GameObject godTear = UnityEngine.Object.Instantiate<GameObject>(interactionMysticShop._godTearPrefab, interactionMysticShop.playerFarming.transform.position + new Vector3(0.0f, 1f, -1f), Quaternion.identity, interactionMysticShop.transform.parent);
    godTear.transform.localScale = Vector3.zero;
    godTear.transform.DOScale(Vector3.one, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    GameManager.GetInstance().OnConversationNext(godTear, 5.5f);
    yield return (object) new WaitForSeconds(1.25f);
    interactionMysticShop.LUTOverlay.gameObject.SetActive(true);
    AudioManager.Instance.PlayOneShot("event:/player/float_follower", godTear);
    yield return (object) new WaitForSeconds(0.25f);
    godTear.transform.DOMove(interactionMysticShop._godTearTarget.position, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.PlayOneShot("event:/Stings/global_faith_up", godTear);
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", godTear);
    SkeletonAnimation componentInChildren = godTear.GetComponentInChildren<SkeletonAnimation>();
    componentInChildren.timeScale = 0.75f;
    yield return (object) componentInChildren.YieldForAnimation("godTearScale");
    GameManager.GetInstance().OnConversationNext(interactionMysticShop._godTearTarget.gameObject, 3f);
    UnityEngine.Object.Destroy((UnityEngine.Object) godTear);
    interactionMysticShop.DoWheelReward();
  }

  public void CheckAvailableRewards()
  {
    this.possibleSkins = new List<string>((IEnumerable<string>) DataManager.MysticShopKeeperSkins);
    this.possibleDecos = new List<StructureBrain.TYPES>((IEnumerable<StructureBrain.TYPES>) DataManager.MysticShopKeeperDecorations);
    this.necklaces = new List<InventoryItem.ITEM_TYPE>((IEnumerable<InventoryItem.ITEM_TYPE>) this.allNecklaces);
    for (int index = this.necklaces.Count - 1; index >= 0; --index)
    {
      if (Inventory.GetItemQuantity(this.necklaces[index]) > 0)
        this.necklaces.Remove(this.necklaces[index]);
    }
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if (this.necklaces.Contains(follower.Necklace))
        this.necklaces.Remove(follower.Necklace);
    }
    if (DataManager.Instance.HasBaalSkin && this.necklaces.Contains(InventoryItem.ITEM_TYPE.Necklace_Light))
      this.necklaces.Remove(InventoryItem.ITEM_TYPE.Necklace_Light);
    if (DataManager.Instance.HasAymSkin && this.necklaces.Contains(InventoryItem.ITEM_TYPE.Necklace_Dark))
      this.necklaces.Remove(InventoryItem.ITEM_TYPE.Necklace_Dark);
    if (this.necklaces.Count <= 0)
    {
      this.necklaces = new List<InventoryItem.ITEM_TYPE>((IEnumerable<InventoryItem.ITEM_TYPE>) this.allNecklaces);
      this.necklaces.Remove(InventoryItem.ITEM_TYPE.Necklace_Light);
      this.necklaces.Remove(InventoryItem.ITEM_TYPE.Necklace_Dark);
    }
    for (int index = this.possibleSkins.Count - 1; index >= 0; --index)
    {
      if (DataManager.GetFollowerSkinUnlocked(this.possibleSkins[index]))
        this.possibleSkins.RemoveAt(index);
    }
    for (int index = this.possibleDecos.Count - 1; index >= 0; --index)
    {
      if (DataManager.Instance.UnlockedStructures.Contains(this.possibleDecos[index]))
        this.possibleDecos.RemoveAt(index);
    }
  }

  public void SetCost() => this.GodTearCost = 1;

  public void DoWheelReward()
  {
    this.CheckAvailableRewards();
    WeightedCollection<InventoryItem.ITEM_TYPE> rewards = new WeightedCollection<InventoryItem.ITEM_TYPE>();
    bool flag1 = !UpgradeSystem.IsUpgradeMaxed(UpgradeSystem.Type.Ritual_CrystalDoctrine) && 24 + (DataManager.Instance.MAJOR_DLC ? 4 : 0) - DataManager.Instance.CrystalDoctrinesReceivedFromMysticShop > 0;
    int num1 = DataManager.Instance.TalismanPiecesReceivedFromMysticShop < 12 ? 1 : 0;
    bool flag2 = this.possibleSkins.Count > 0 || this.possibleDecos.Count > 0;
    int num2 = 1;
    if (num1 != 0)
      ++num2;
    if (flag2)
      ++num2;
    float weight = 0.6f / (float) num2;
    if (flag1)
      rewards.Add(InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE, 0.4f);
    else
      weight += 0.4f / (float) num2;
    if (num1 != 0)
      rewards.Add(InventoryItem.ITEM_TYPE.TALISMAN, weight);
    if (flag2)
    {
      if (this.possibleSkins.Count > 0 && this.possibleDecos.Count > 0)
      {
        if ((double) UnityEngine.Random.value < 0.5)
          rewards.Add(InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN, weight);
        else
          rewards.Add(InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION_ALT, weight);
      }
      else if (this.possibleDecos.Count > 0)
        rewards.Add(InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION_ALT, weight);
      else
        rewards.Add(InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN, weight);
    }
    rewards.Add(this.necklaces[UnityEngine.Random.Range(0, this.necklaces.Count)], weight);
    bool flag3 = false;
    foreach (TarotCards.Card mysticCard in TarotCards.MysticCards)
    {
      if (!DataManager.Instance.PlayerFoundTrinkets.Contains(mysticCard))
      {
        flag3 = true;
        break;
      }
    }
    List<RelicType> relicTypeList = new List<RelicType>();
    relicTypeList.Add(RelicType.SpawnBlackGoop);
    relicTypeList.Add(RelicType.UnlimitedFervour);
    bool flag4 = false;
    foreach (RelicType relicType in relicTypeList)
    {
      if (!DataManager.Instance.PlayerFoundRelics.Contains(relicType))
      {
        Interaction_MysticShop.RelicToUnlock = relicType;
        flag4 = true;
        break;
      }
    }
    if (flag3 & flag4)
      rewards.Add((double) UnityEngine.Random.value < 0.5 ? InventoryItem.ITEM_TYPE.TRINKET_CARD : InventoryItem.ITEM_TYPE.RELIC, weight);
    else if (flag3)
      rewards.Add(InventoryItem.ITEM_TYPE.TRINKET_CARD, weight);
    else if (flag4)
      rewards.Add(InventoryItem.ITEM_TYPE.RELIC, weight);
    if (rewards.Count <= 1)
    {
      rewards.Clear();
      List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>((IEnumerable<InventoryItem.ITEM_TYPE>) this.necklaces);
      int num3 = Mathf.Min(itemTypeList.Count, 4);
      for (int index = 0; index < num3; ++index)
      {
        InventoryItem.ITEM_TYPE element = itemTypeList[UnityEngine.Random.Range(0, itemTypeList.Count)];
        itemTypeList.Remove(element);
        rewards.Add(element, 1f / (float) num3);
      }
    }
    InventoryItem.ITEM_TYPE itemType1 = DataManager.Instance.PreviousMysticShopItem;
    if (rewards.Count <= 2)
      itemType1 = InventoryItem.ITEM_TYPE.NONE;
    List<InventoryItem.ITEM_TYPE> itemTypeList1 = new List<InventoryItem.ITEM_TYPE>();
    if (itemType1 != InventoryItem.ITEM_TYPE.NONE)
      itemTypeList1.Add(itemType1);
    if (!DataManager.Instance.OnboardedCrystalDoctrine)
    {
      DataManager.Instance.OnboardedCrystalDoctrine = true;
      foreach (InventoryItem.ITEM_TYPE itemType2 in rewards)
        itemTypeList1.Add(itemType2);
      itemTypeList1.Remove(InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE);
    }
    InventoryItem.ITEM_TYPE chosenReward = InventoryItem.ITEM_TYPE.NONE;
    this.StartCoroutine((IEnumerator) UIManager.LoadAssets(MonoSingleton<UIManager>.Instance.LoadMysticShopAssets(), (System.Action) (() =>
    {
      UIMysticShopOverlayController overlayController = MonoSingleton<UIManager>.Instance.MysticShopOverlayTemplate.Instantiate<UIMysticShopOverlayController>();
      overlayController.Show(rewards);
      overlayController.OnRewardChosen += (System.Action<InventoryItem.ITEM_TYPE>) (reward => chosenReward = reward);
      overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
      {
        DataManager.Instance.PreviousMysticShopItem = chosenReward;
        this.StartCoroutine((IEnumerator) this.GiveChosenReward(chosenReward));
        this.LUTOverlay.gameObject.SetActive(false);
      });
    })));
  }

  public IEnumerator GiveChosenReward(InventoryItem.ITEM_TYPE chosenReward)
  {
    Interaction_MysticShop interactionMysticShop = this;
    GameObject necklace;
    ResourceCustomTarget resourceCustomTarget;
    if (chosenReward == InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE)
    {
      MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
      necklace = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Resources/ResourceCustomTarget"), (Transform) null, true);
      necklace.transform.position = interactionMysticShop._godTearTarget.position;
      necklace.transform.rotation = Quaternion.identity;
      resourceCustomTarget = necklace.GetComponent<ResourceCustomTarget>();
      resourceCustomTarget.createdObject = necklace;
      resourceCustomTarget.inventoryItemDisplay.SetImage(chosenReward);
      resourceCustomTarget.inventoryItemDisplay.transform.localPosition = (Vector3) Vector2.zero;
      resourceCustomTarget.enabled = false;
      GameManager.GetInstance().OnConversationNext(necklace, 6f);
      AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", interactionMysticShop.gameObject);
      necklace.transform.localScale = Vector3.zero;
      necklace.transform.DOScale(Vector3.one, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      yield return (object) new WaitForSecondsRealtime(1.25f);
      AudioManager.Instance.PlayOneShot("event:/player/float_follower", necklace.gameObject);
      yield return (object) new WaitForSecondsRealtime(0.25f);
      necklace.transform.DOMove(interactionMysticShop.playerFarming.transform.position + new Vector3(0.0f, 1f, -1f), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
      yield return (object) new WaitForSecondsRealtime(1f);
      resourceCustomTarget.CollectMe();
      Inventory.AddItem(InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE, 1);
      ++DataManager.Instance.CrystalDoctrinesReceivedFromMysticShop;
      UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_CrystalDoctrine);
      yield return (object) new WaitForSecondsRealtime(0.25f);
      if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.CrystalDoctrine))
      {
        while (MonoSingleton<UIManager>.Instance.MenusBlocked)
          yield return (object) null;
        yield return (object) MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.CrystalDoctrine).YieldUntilHidden();
        UIPlayerUpgradesMenuController menu = MonoSingleton<UIManager>.Instance.PlayerUpgradesMenuTemplate.Instantiate<UIPlayerUpgradesMenuController>();
        menu.ShowCrystalUnlock();
        yield return (object) menu.YieldUntilHidden();
      }
      necklace = (GameObject) null;
      resourceCustomTarget = (ResourceCustomTarget) null;
    }
    else if (interactionMysticShop.necklaces.Contains(chosenReward))
    {
      necklace = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Resources/ResourceCustomTarget"), (Transform) null, true);
      necklace.transform.position = interactionMysticShop._godTearTarget.position;
      necklace.transform.rotation = Quaternion.identity;
      resourceCustomTarget = necklace.GetComponent<ResourceCustomTarget>();
      resourceCustomTarget.createdObject = necklace;
      resourceCustomTarget.inventoryItemDisplay.SetImage(chosenReward);
      resourceCustomTarget.inventoryItemDisplay.transform.localPosition = (Vector3) Vector2.zero;
      resourceCustomTarget.enabled = false;
      GameManager.GetInstance().OnConversationNext(necklace, 6f);
      AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", interactionMysticShop.gameObject);
      necklace.transform.localScale = Vector3.zero;
      necklace.transform.DOScale(Vector3.one * 1.5f, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      yield return (object) new WaitForSecondsRealtime(1.25f);
      AudioManager.Instance.PlayOneShot("event:/player/float_follower", necklace.gameObject);
      yield return (object) new WaitForSecondsRealtime(0.25f);
      necklace.transform.DOMove(interactionMysticShop.playerFarming.transform.position + new Vector3(0.0f, 1f, -1f), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
      yield return (object) new WaitForSecondsRealtime(1f);
      resourceCustomTarget.CollectMe();
      Inventory.AddItem(chosenReward, 1);
      if (!DataManager.Instance.FoundItems.Contains(chosenReward))
      {
        DataManager.Instance.FoundItems.Add(chosenReward);
        UINewItemOverlayController menu = MonoSingleton<UIManager>.Instance.ShowNewItemOverlay();
        menu.Show(UINewItemOverlayController.TypeOfCard.Necklace, interactionMysticShop.transform.position, chosenReward);
        yield return (object) menu.YieldUntilHidden();
      }
      yield return (object) new WaitForSecondsRealtime(0.25f);
      necklace = (GameObject) null;
      resourceCustomTarget = (ResourceCustomTarget) null;
    }
    else
    {
      switch (chosenReward)
      {
        case InventoryItem.ITEM_TYPE.TRINKET_CARD:
          bool WaitToGetTarot1 = true;
          TarotCards.Card CardType = TarotCards.Card.Arrows;
          foreach (TarotCards.Card mysticCard in TarotCards.MysticCards)
          {
            if (!DataManager.Instance.PlayerFoundTrinkets.Contains(mysticCard))
            {
              CardType = mysticCard;
              break;
            }
          }
          TarotCustomTarget tarotCustomTarget = TarotCustomTarget.Create(interactionMysticShop._godTearTarget.position, interactionMysticShop.playerFarming.transform.position + new Vector3(0.0f, 1f, -1f), 1f, CardType, (System.Action) (() => WaitToGetTarot1 = false));
          GameManager.GetInstance().OnConversationNext(tarotCustomTarget.gameObject, 6f);
          AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", interactionMysticShop.gameObject);
          AudioManager.Instance.PlayOneShot("event:/player/float_follower", tarotCustomTarget.gameObject);
          while (WaitToGetTarot1)
            yield return (object) null;
          break;
        case InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN:
          string possibleSkin = interactionMysticShop.possibleSkins[UnityEngine.Random.Range(0, interactionMysticShop.possibleSkins.Count)];
          FollowerSkinCustomTarget skinCustomTarget = FollowerSkinCustomTarget.Create(interactionMysticShop._godTearTarget.position, interactionMysticShop.playerFarming.transform.position + new Vector3(0.0f, 1f, -1f), (Transform) null, 1.25f, possibleSkin, (System.Action) null);
          GameManager.GetInstance().OnConversationNext(skinCustomTarget.gameObject, 6f);
          AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", interactionMysticShop.gameObject);
          AudioManager.Instance.PlayOneShot("event:/player/float_follower", skinCustomTarget.gameObject);
          yield return (object) new WaitForSecondsRealtime(1.25f);
          yield return (object) new WaitForSecondsRealtime(1.5f);
          while (UIMenuBase.ActiveMenus.Count > 0)
            yield return (object) null;
          break;
        case InventoryItem.ITEM_TYPE.TALISMAN:
          Interaction_KeyPiece keyPiece = UnityEngine.Object.Instantiate<Interaction_KeyPiece>(interactionMysticShop._keyPiecePrefab, interactionMysticShop._godTearTarget.position, Quaternion.identity, interactionMysticShop.transform.parent);
          keyPiece.transform.localScale = Vector3.zero;
          keyPiece.transform.DOScale(Vector3.one, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
          AudioManager.Instance.PlayOneShot("event:/Stings/Choir_Short", interactionMysticShop._godTearTarget.position);
          GameManager.GetInstance().OnConversationNext(keyPiece.gameObject, 6f);
          yield return (object) new WaitForSeconds(1.5f);
          keyPiece.transform.DOMove(interactionMysticShop.playerFarming.transform.position + new Vector3(0.0f, 1f, -1f), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
          AudioManager.Instance.PlayOneShot("event:/player/float_follower", keyPiece.gameObject);
          yield return (object) new WaitForSeconds(1f);
          keyPiece.OnInteract(interactionMysticShop.playerFarming.state);
          ++DataManager.Instance.TalismanPiecesReceivedFromMysticShop;
          yield return (object) new WaitForSeconds(2.5f);
          UnityEngine.Object.Destroy((UnityEngine.Object) keyPiece.gameObject);
          keyPiece = (Interaction_KeyPiece) null;
          break;
        case InventoryItem.ITEM_TYPE.RELIC:
          bool WaitToGetTarot2 = true;
          GameObject gameObject1 = RelicCustomTarget.Create(interactionMysticShop._godTearTarget.position, interactionMysticShop.playerFarming.transform.position + new Vector3(0.0f, 1f, -1f), 1f, Interaction_MysticShop.RelicToUnlock, (System.Action) (() => WaitToGetTarot2 = false));
          GameManager.GetInstance().OnConversationNext(gameObject1.gameObject, 6f);
          AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", interactionMysticShop.gameObject);
          AudioManager.Instance.PlayOneShot("event:/player/float_follower", gameObject1.gameObject);
          while (WaitToGetTarot2)
            yield return (object) null;
          break;
        case InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION_ALT:
          StructureBrain.TYPES possibleDeco = interactionMysticShop.possibleDecos[UnityEngine.Random.Range(0, interactionMysticShop.possibleDecos.Count)];
          GameObject gameObject2 = DecorationCustomTarget.Create(interactionMysticShop._godTearTarget.position, interactionMysticShop.playerFarming.transform.position + new Vector3(0.0f, 1f, -1f), 1.25f, possibleDeco, (Transform) null, (System.Action) null);
          GameManager.GetInstance().OnConversationNext(gameObject2.gameObject, 6f);
          AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", interactionMysticShop.gameObject);
          AudioManager.Instance.PlayOneShot("event:/player/float_follower", gameObject2.gameObject);
          yield return (object) new WaitForSecondsRealtime(1.25f);
          yield return (object) new WaitForSecondsRealtime(1.5f);
          while (UIMenuBase.ActiveMenus.Count > 0)
            yield return (object) null;
          break;
        default:
          List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>();
          int num = UnityEngine.Random.Range(10, 20);
          for (int index = 0; index < num; ++index)
            InventoryItem.Spawn(chosenReward, 1, interactionMysticShop.transform.position + Vector3.down).SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
          break;
      }
    }
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    interactionMysticShop.Interactable = true;
    GameManager.GetInstance().OnConversationEnd();
    interactionMysticShop.HasChanged = true;
    ++DataManager.Instance.MysticRewardCount;
    interactionMysticShop.SetCost();
    interactionMysticShop.boughtBark.gameObject.SetActive(true);
    interactionMysticShop.boughtBark.Show();
    if (!DataManager.Instance.MysticKeeperFirstPurchase)
      interactionMysticShop.firstPurchaseConvo.Play();
    GameManager.GetInstance().OnConversationEnd(false);
    interactionMysticShop.StopMusic();
    yield return (object) null;
  }

  public override void OnDisable()
  {
    base.OnDisable();
    AudioManager.Instance.StopLoop(this.loopedSound);
  }

  public IEnumerator AngrySequenceIE(PlayerFarming playerFarming)
  {
    Interaction_MysticShop interactionMysticShop = this;
    if (playerFarming.state.CURRENT_STATE != StateMachine.State.InActive)
    {
      GameManager.GetInstance().OnConversationNew(false);
      GameManager.GetInstance().OnConversationNext(interactionMysticShop.gameObject);
      CameraManager.instance.ShakeCameraForDuration(1f, 1.2f, 0.25f);
      playerFarming.unitObject.LockToGround = false;
      interactionMysticShop.LightingOverrideAngry.SetActive(true);
      AudioManager.Instance.StopLoop(interactionMysticShop.loopedSound);
      interactionMysticShop.loopedSound = AudioManager.Instance.CreateLoop("event:/atmos/adventure_map/hum_adventure_map", true);
      playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      playerFarming.Spine.AnimationState.SetAnimation(0, "floating-boss-loop", true);
      AudioManager.Instance.SetMusicPsychedelic(1f);
      playerFarming._state.facingAngle = 90f;
      GameManager.GetInstance().OnConversationNext(playerFarming.gameObject);
      float x = (UnityEngine.Object) playerFarming == (UnityEngine.Object) PlayerFarming.players[0] ? 0.0f : 1f;
      playerFarming.transform.DOMove(new Vector3(x, 42f, -5f), 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCirc);
      yield return (object) new WaitForSeconds(0.5f);
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if ((UnityEngine.Object) playerFarming != (UnityEngine.Object) player && player.state.CURRENT_STATE != StateMachine.State.CustomAnimation)
          player.state.CURRENT_STATE = StateMachine.State.InActive;
      }
      yield return (object) new WaitForSeconds(1f);
      playerFarming.transform.DOMove(new Vector3(x, 42f, -1.8f), 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
      yield return (object) new WaitForSeconds(1.5f);
      AudioManager.Instance.StopLoop(interactionMysticShop.loopedSound);
      AudioManager.Instance.SetMusicPsychedelic(0.0f);
      playerFarming.Spine.AnimationState.SetAnimation(0, "floating-boss-land", false);
      yield return (object) new WaitForSeconds(1.5f);
      playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      playerFarming.unitObject.LockToGround = true;
      interactionMysticShop.enterMysticDimension[DataManager.Instance.PlayerTriedToEnterMysticDimensionCount].Spoken = false;
      interactionMysticShop.enterMysticDimension[DataManager.Instance.PlayerTriedToEnterMysticDimensionCount].Finished = false;
      interactionMysticShop.enterMysticDimension[DataManager.Instance.PlayerTriedToEnterMysticDimensionCount].Play();
      ++DataManager.Instance.PlayerTriedToEnterMysticDimensionCount;
      DataManager.Instance.PlayerTriedToEnterMysticDimensionCount = Mathf.Clamp(DataManager.Instance.PlayerTriedToEnterMysticDimensionCount, 0, interactionMysticShop.enterMysticDimension.Length - 1);
      yield return (object) null;
      while (MMConversation.CURRENT_CONVERSATION != null)
        yield return (object) null;
      interactionMysticShop.LightingOverrideAngry.SetActive(false);
    }
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (!((UnityEngine.Object) collision.GetComponent<PlayerFarming>() != (UnityEngine.Object) null))
      return;
    PlayerFarming.SetMainPlayer(collision);
    this.StartCoroutine((IEnumerator) this.AngrySequenceIE(collision.GetComponent<PlayerFarming>()));
  }

  public void TestFinalReward()
  {
    DataManager.Instance.BeatenLeshyLayer2 = true;
    DataManager.Instance.BeatenHeketLayer2 = true;
    DataManager.Instance.BeatenKallamarLayer2 = true;
    DataManager.Instance.BeatenShamuraLayer2 = true;
    this.beatenAllA.Entries[3].TermToSpeak = !StructuresData.GetUnlocked(StructureBrain.TYPES.DECORATION_BOSS_TROPHY_5) ? "Conversation_NPC/MysticShopSeller/BeatenAll/3_Shrine" : "Conversation_NPC/MysticShopSeller/BeatenAll/3_Follower";
    this.PlayMusic();
    this.beatenAllA.Play();
  }

  public void GiveDeathCatReward()
  {
    this.StartCoroutine((IEnumerator) this.GiveDeathCatRewardIE());
  }

  public IEnumerator GiveDeathCatRewardIE()
  {
    Interaction_MysticShop interactionMysticShop = this;
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    bool waiting = true;
    if (!StructuresData.GetUnlocked(StructureBrain.TYPES.DECORATION_BOSS_TROPHY_5))
    {
      GameManager.GetInstance().OnConversationNew();
      waiting = true;
      DecorationCustomTarget.Create(interactionMysticShop.mysticKeeperSpine.transform.position - Vector3.forward + Vector3.down * 1f, interactionMysticShop.playerFarming.transform.position, 1.5f, StructureBrain.TYPES.DECORATION_BOSS_TROPHY_5, interactionMysticShop.transform.parent, (System.Action) (() => waiting = false));
      while (waiting)
        yield return (object) null;
    }
    else
    {
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(interactionMysticShop.deathCatTargetPosition);
      interactionMysticShop.simpleCameraTwo.enabled = false;
      FollowerInfo deathcat = FollowerInfo.NewCharacter(FollowerLocation.Base, "Boss Death Cat");
      deathcat.Name = ScriptLocalization.NAMES.DeathNPC;
      deathcat.ID = 666;
      deathcat.Traits.Add(FollowerTrait.TraitType.Immortal);
      if (BiomeBaseManager.Instance.SpawnExistingRecruits && DataManager.Instance.Followers_Recruit.Count == 0)
        BiomeBaseManager.Instance.SpawnExistingRecruits = false;
      DataManager.Instance.Followers_Recruit.Add(deathcat);
      DataManager.SetFollowerSkinUnlocked("Boss Death Cat");
      FollowerManager.SpawnedFollower follower = FollowerManager.SpawnCopyFollower(deathcat, interactionMysticShop.deathCatConvo.transform.position, interactionMysticShop.transform.parent, FollowerLocation.Base);
      double num1 = (double) follower.Follower.SetBodyAnimation("picked-up-hate", true);
      follower.Follower.gameObject.SetActive(false);
      follower.Follower.transform.position = interactionMysticShop.deathCatConvo.transform.position + Vector3.back * 2f + Vector3.up * 5f;
      follower.Follower.transform.localScale = Vector3.zero;
      MeshRenderer renderer = follower.Follower.Spine.GetComponent<MeshRenderer>();
      renderer.material.SetColor("_FillColor", Color.white);
      renderer.material.SetFloat("_FillAlpha", 1f);
      yield return (object) new WaitForSeconds(2f);
      CameraManager.instance.ShakeCameraForDuration(1.2f, 1.3f, 0.25f);
      interactionMysticShop.LightingOverrideAngry.SetActive(true);
      interactionMysticShop.loopedSoundDeathCat = AudioManager.Instance.CreateLoop("event:/door/eye_beam_door_open", interactionMysticShop.playerFarming.gameObject, true);
      MMVibrate.RumbleContinuous(0.1f, 0.2f, interactionMysticShop.playerFarming);
      DeviceLightingManager.FlashColor(Color.red);
      yield return (object) new WaitForSeconds(0.25f);
      CameraManager.instance.ShakeCameraForDuration(0.2f, 0.3f, 3f);
      yield return (object) new WaitForSeconds(2f);
      follower.Follower.gameObject.SetActive(true);
      float t = 0.0f;
      DOTween.To((DOGetter<float>) (() => t), (DOSetter<float>) (x => t = x), 1f, 2f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => renderer.material.SetFloat("_FillAlpha", 1f - t)));
      follower.Follower.transform.DOScale(1f, 1.25f);
      follower.Follower.transform.DOMove(interactionMysticShop.deathCatTargetPosition.transform.position, 2f);
      yield return (object) new WaitForSeconds(1f);
      yield return (object) new WaitForSeconds(2f);
      foreach (ConversationEntry entry in interactionMysticShop.deathCatConvo.Entries)
      {
        entry.Speaker = follower.Follower.Spine.gameObject;
        entry.SkeletonData = follower.Follower.Spine;
        entry.soundPath = "event:/dialogue/followers/boss/fol_deathcat";
        entry.pitchValue = deathcat.follower_pitch;
        entry.vibratoValue = deathcat.follower_vibrato;
      }
      yield return (object) new WaitForEndOfFrame();
      interactionMysticShop.LightingOverrideAngry.SetActive(false);
      AudioManager.Instance.StopLoop(interactionMysticShop.loopedSoundDeathCat);
      MMVibrate.StopRumble(interactionMysticShop.playerFarming);
      interactionMysticShop.deathCatConvo.Play();
      yield return (object) new WaitForEndOfFrame();
      GameManager.GetInstance().CamFollowTarget.TargetOffset = new Vector3(0.0f, 0.0f, 0.3f);
      while (LetterBox.IsPlaying)
        yield return (object) null;
      yield return (object) new WaitForEndOfFrame();
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(follower.Follower.gameObject, 6f);
      Vector3 TargetPosition = follower.Follower.transform.position + Vector3.left * 1.5f;
      interactionMysticShop.playerFarming.GoToAndStop(TargetPosition, follower.Follower.gameObject);
      while (interactionMysticShop.playerFarming.GoToAndStopping)
        yield return (object) null;
      follower.Follower.Spine.GetComponent<SimpleSpineAnimator>().enabled = false;
      if ((UnityEngine.Object) interactionMysticShop.state == (UnityEngine.Object) null)
        interactionMysticShop.state = interactionMysticShop.GetComponent<StateMachine>();
      GameManager.GetInstance().OnConversationNext(follower.Follower.gameObject);
      AudioManager.Instance.PlayOneShot("event:/followers/ascend", interactionMysticShop.gameObject);
      yield return (object) new WaitForEndOfFrame();
      interactionMysticShop.LightingOverrideAngry.SetActive(false);
      GameManager.GetInstance().OnConversationNext(follower.Follower.gameObject);
      follower.Follower.transform.DOMove(follower.Follower.transform.position + Vector3.forward, 2f);
      follower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      double num2 = (double) follower.Follower.SetBodyAnimation("convert-short", false);
      if ((bool) (UnityEngine.Object) interactionMysticShop.recruitParticles)
        interactionMysticShop.recruitParticles.Play();
      interactionMysticShop.portalSpine.gameObject.SetActive(true);
      interactionMysticShop.portalSpine.AnimationState.SetAnimation(0, "convert-short", false);
      float duration = interactionMysticShop.playerFarming.Spine.AnimationState.SetAnimation(0, "specials/special-activate-long", false).Animation.Duration;
      interactionMysticShop.playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
      yield return (object) new WaitForSeconds(duration - 1f);
      float num3 = UnityEngine.Random.value;
      Thought thought = Thought.None;
      if ((double) num3 < 0.699999988079071)
      {
        float num4 = UnityEngine.Random.value;
        if ((double) num4 <= 0.30000001192092896)
          thought = Thought.HappyConvert;
        else if ((double) num4 > 0.30000001192092896 && (double) num4 < 0.60000002384185791)
          thought = Thought.GratefulConvert;
        else if ((double) num4 >= 0.60000002384185791)
          thought = Thought.SkepticalConvert;
      }
      else
        thought = (double) UnityEngine.Random.value > 0.30000001192092896 || DataManager.Instance.Followers.Count <= 0 ? Thought.InstantBelieverConvert : Thought.ResentfulConvert;
      ThoughtData data = FollowerThoughts.GetData(thought);
      data.Init();
      deathcat.Thoughts.Add(data);
      FollowerManager.CleanUpCopyFollower(follower);
      deathcat = (FollowerInfo) null;
      follower = (FollowerManager.SpawnedFollower) null;
    }
    interactionMysticShop.beatenAllC.Play();
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.MysticShopReturn);
    yield return (object) null;
    while (interactionMysticShop.waitingForContinueSequence)
      yield return (object) null;
    DataManager.Instance.UnlockedFleeces.Add(1000);
    UIPlayerUpgradesMenuController menu = MonoSingleton<UIManager>.Instance.PlayerUpgradesMenuTemplate.Instantiate<UIPlayerUpgradesMenuController>();
    menu.ShowNewFleecesUnlocked(new PlayerFleeceManager.FleeceType[1]
    {
      PlayerFleeceManager.FleeceType.GodOfDeath
    }, true);
    yield return (object) menu.YieldUntilHidden();
    interactionMysticShop.simpleCameraTwo.enabled = false;
    AudioManager.Instance.PlayOneShot("event:/Stings/finish_game_second_time", interactionMysticShop.playerFarming.gameObject);
    AudioManager.Instance.PlayOneShot("event:/ui/heretics_defeated", interactionMysticShop.playerFarming.gameObject);
    float tt = 0.0f;
    DOTween.To((DOGetter<float>) (() => tt), (DOSetter<float>) (x => tt = x), 1f, 10f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => GameManager.GetInstance().CameraSetZoom(Mathf.Lerp(14f, 20f, tt))));
    yield return (object) new WaitForSeconds(7f);
    DataManager.Instance.BeatenPostGame = true;
    SaveAndLoad.Save();
    interactionMysticShop.StopMusic();
    MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Credits", 2f, "", (System.Action) null);
    Credits.GoToMainMenu = true;
  }

  public void ContinueSequence() => this.waitingForContinueSequence = false;

  public void CheckConversation()
  {
    Debug.Log((object) "CheckConversation()".Colour(Color.yellow));
    if (!DataManager.Instance.MysticKeeperBeatenAll && DataManager.Instance.BeatenLeshyLayer2 && DataManager.Instance.BeatenHeketLayer2 && DataManager.Instance.BeatenKallamarLayer2 && DataManager.Instance.BeatenShamuraLayer2)
      this.StartCoroutine((IEnumerator) this.WaitForConvoToFinish());
    else
      this.StopMusic();
  }

  public IEnumerator WaitForConvoToFinish()
  {
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    this.PlayMusic();
    this.beatenAllA.Play();
  }

  public void PlayMusic()
  {
    Debug.Log((object) "Play music!".Colour(Color.green));
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.Temple);
  }

  public void StopMusic()
  {
    Debug.Log((object) "Stop music!".Colour(Color.yellow));
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.DungeonDoor);
  }

  [CompilerGenerated]
  public void \u003CNameMysticKeeper\u003Eb__48_0()
  {
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.MysticShopReturn);
    UIMysticSellerNameMenuController mysticSellerNameMenu = MonoSingleton<UIManager>.Instance.MysticSellerNameMenuTemplate.Instantiate<UIMysticSellerNameMenuController>();
    mysticSellerNameMenu.Show(false, false);
    UIMysticSellerNameMenuController nameMenuController1 = mysticSellerNameMenu;
    nameMenuController1.OnNameConfirmed = nameMenuController1.OnNameConfirmed + (System.Action<string>) (name =>
    {
      this.mysticKeeperName = name;
      MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
      {
        new ConversationEntry(this.cameraPos, LocalizationManager.GetTranslation("Conversation_NPC/MysticShopSeller/GodTearIntro/4"))
      }, (List<MMTools.Response>) null, (System.Action) (() =>
      {
        this.simpleCamera.enabled = true;
        this.StopMusic();
      })));
    });
    UIMysticSellerNameMenuController nameMenuController2 = mysticSellerNameMenu;
    nameMenuController2.OnHide = nameMenuController2.OnHide + (System.Action) (() => { });
    UIMysticSellerNameMenuController nameMenuController3 = mysticSellerNameMenu;
    nameMenuController3.OnHidden = nameMenuController3.OnHidden + (System.Action) (() => mysticSellerNameMenu = (UIMysticSellerNameMenuController) null);
    DataManager.Instance.OnboardedGodTear = true;
  }

  [CompilerGenerated]
  public void \u003CNameMysticKeeper\u003Eb__48_1(string name)
  {
    this.mysticKeeperName = name;
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(this.cameraPos, LocalizationManager.GetTranslation("Conversation_NPC/MysticShopSeller/GodTearIntro/4"))
    }, (List<MMTools.Response>) null, (System.Action) (() =>
    {
      this.simpleCamera.enabled = true;
      this.StopMusic();
    })));
  }

  [CompilerGenerated]
  public void \u003CNameMysticKeeper\u003Eb__48_2()
  {
    this.simpleCamera.enabled = true;
    this.StopMusic();
  }

  [CompilerGenerated]
  public void \u003CUnlockEndlessMode\u003Eb__51_0()
  {
    foreach (ConversationEntry entry in this.onboardEndlessModeB.Entries)
      entry.CharacterName = this.mysticKeeperName;
    this.onboardEndlessModeB.Entries[this.onboardEndlessModeB.Entries.Count - 1].TermToSpeak = string.Format(LocalizationManager.GetTranslation("Conversation_NPC/MysticShopSeller/EndlessModeIntro/4"), (object) this.mysticKeeperName);
    this.onboardEndlessModeB.Play();
    this.onboardEndlessModeB.Callback.AddListener((UnityAction) (() =>
    {
      this.StopMusic();
      this.simpleCamera.AutomaticallyActivate = false;
    }));
  }

  [CompilerGenerated]
  public void \u003CUnlockEndlessMode\u003Eb__51_1()
  {
    this.StopMusic();
    this.simpleCamera.AutomaticallyActivate = false;
  }
}
