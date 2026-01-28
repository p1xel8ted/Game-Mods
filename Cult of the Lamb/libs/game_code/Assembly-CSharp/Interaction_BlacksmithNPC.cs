// Decompiled with JetBrains decompiler
// Type: Interaction_BlacksmithNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMTools;
using Spine.Unity;
using src.Extensions;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_BlacksmithNPC : Interaction
{
  public const string NAME_WEAPON_SOUND_PATH = "event:/dlc/dialogue/ramiel/shy";
  [SerializeField]
  [TermsPopup("")]
  public string characterName;
  [SerializeField]
  [TermsPopup("")]
  public string weaponToBringQuestCharacterName;
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "spine")]
  public string namedWeaponAnimation;
  [Header("Legendary Weapons")]
  [SerializeField]
  public Interaction_SimpleConversation brokenHammerQuestConversation;
  [SerializeField]
  public Interaction_SimpleConversation giveBrokenHammerConversation;
  [SerializeField]
  public Interaction_SimpleConversation repairHammerConversation;
  [SerializeField]
  public Interaction_SimpleConversation giveBrokenSwordConversation;
  [SerializeField]
  public Interaction_SimpleConversation repairSwordConversation;
  [SerializeField]
  public Interaction_SimpleConversation giveBrokenDaggerConversation;
  [SerializeField]
  public Interaction_SimpleConversation repairDaggerConversation;
  [SerializeField]
  public Interaction_SimpleConversation giveBrokenAxeConversation;
  [SerializeField]
  public Interaction_SimpleConversation repairAxeConversation;
  [SerializeField]
  public Interaction_SimpleConversation giveBrokenGauntletsConversation;
  [SerializeField]
  public Interaction_SimpleConversation repairGauntletsConversation;
  [SerializeField]
  public Interaction_SimpleConversation giveBrokenBlundebussConversation;
  [SerializeField]
  public Interaction_SimpleConversation repairBlunderbussConversation;
  [SerializeField]
  public Interaction_SimpleConversation giveBrokenChainConversation;
  [SerializeField]
  public Interaction_SimpleConversation repairChainConversation;
  [Space]
  [SerializeField]
  public Transform forgePosition;
  [SerializeField]
  public ParticleSystem hitImpactVFX;
  [SerializeField]
  public SimpleBark _cantAffordBark;
  public EquipmentType weaponToRepair = EquipmentType.None;

  public bool canGiveBrokenWeapon => this.GetNextBrokenWeapon() != EquipmentType.None;

  public override void OnEnable()
  {
    base.OnEnable();
    if (!ObjectiveManager.GroupExists("Objectives/GroupTitles/LegendaryWeapons", true) && DataManager.Instance.BlacksmithShopFixed && !DataManager.Instance.GivenBrokenHammerWeaponQuest)
      this.brokenHammerQuestConversation.gameObject.SetActive(true);
    if (this.weaponToRepair == EquipmentType.None)
    {
      switch (this.GetNextBrokenWeapon())
      {
        case EquipmentType.Sword_Legendary:
          this.giveBrokenSwordConversation.gameObject.SetActive(true);
          break;
        case EquipmentType.Axe_Legendary:
          this.giveBrokenAxeConversation.gameObject.SetActive(true);
          break;
        case EquipmentType.Hammer_Legendary:
          this.giveBrokenHammerConversation.gameObject.SetActive(true);
          break;
        case EquipmentType.Dagger_Legendary:
          this.giveBrokenDaggerConversation.gameObject.SetActive(true);
          break;
        case EquipmentType.Gauntlet_Legendary:
          this.giveBrokenGauntletsConversation.gameObject.SetActive(true);
          break;
        case EquipmentType.Blunderbuss_Legendary:
          this.giveBrokenBlundebussConversation.gameObject.SetActive(true);
          break;
        case EquipmentType.Chain_Legendary:
          this.giveBrokenChainConversation.gameObject.SetActive(true);
          break;
      }
    }
    this.brokenHammerQuestConversation.Callback.AddListener(new UnityAction(this.StartLegendaryWeaponQuest));
    this.repairSwordConversation.Callback.AddListener(new UnityAction(this.NameWeapon));
    this.repairDaggerConversation.Callback.AddListener(new UnityAction(this.NameWeapon));
    this.repairBlunderbussConversation.Callback.AddListener(new UnityAction(this.NameWeapon));
    this.repairGauntletsConversation.Callback.AddListener(new UnityAction(this.NameWeapon));
    this.repairHammerConversation.Callback.AddListener(new UnityAction(this.NameWeapon));
    this.repairAxeConversation.Callback.AddListener(new UnityAction(this.NameWeapon));
    this.repairChainConversation.Callback.AddListener(new UnityAction(this.NameWeapon));
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.brokenHammerQuestConversation.Callback.RemoveListener(new UnityAction(this.StartLegendaryWeaponQuest));
    this.repairSwordConversation.Callback.RemoveListener(new UnityAction(this.NameWeapon));
    this.repairAxeConversation.Callback.RemoveListener(new UnityAction(this.NameWeapon));
    this.repairDaggerConversation.Callback.RemoveListener(new UnityAction(this.NameWeapon));
    this.repairBlunderbussConversation.Callback.RemoveListener(new UnityAction(this.NameWeapon));
    this.repairGauntletsConversation.Callback.RemoveListener(new UnityAction(this.NameWeapon));
    this.repairHammerConversation.Callback.RemoveListener(new UnityAction(this.NameWeapon));
    this.repairChainConversation.Callback.RemoveListener(new UnityAction(this.NameWeapon));
  }

  public override void GetLabel()
  {
    if (this.canGiveBrokenWeapon)
    {
      int weaponCost = this.GetWeaponCost(this.GetNextBrokenWeapon());
      this.Label = string.Format(LocalizationManager.GetTranslation("UI/ItemSelector/Context/Give"), (object) $"{CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.WOOL, weaponCost)} | {CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT, 1)}");
    }
    else
      this.Label = "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (!this.canGiveBrokenWeapon)
      return;
    EquipmentType nextBrokenWeapon = this.GetNextBrokenWeapon();
    if (!this.CanAfford(nextBrokenWeapon))
    {
      this.OnCantAfford();
    }
    else
    {
      SimulationManager.Pause();
      GameManager.GetInstance().OnConversationNew(true, true, this.playerFarming);
      this.StartCoroutine((IEnumerator) this.GiveWeaponFragment(nextBrokenWeapon));
    }
  }

  public void OnCantAfford()
  {
    AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.gameObject);
    this.playerFarming.indicator.PlayShake();
    PlayerFarming.SetStateForAllPlayers();
    if (!((UnityEngine.Object) this._cantAffordBark != (UnityEngine.Object) null))
      return;
    this._cantAffordBark.gameObject.SetActive(true);
  }

  public EquipmentType GetNextBrokenWeapon()
  {
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT) > 0)
    {
      foreach (EquipmentType nextBrokenWeapon in DataManager.Instance.LegendaryWeaponsUnlockOrder)
      {
        if (!DataManager.Instance.WeaponPool.Contains(nextBrokenWeapon))
          return nextBrokenWeapon;
      }
    }
    return EquipmentType.None;
  }

  public IEnumerator GiveWeaponFragment(EquipmentType weapon)
  {
    Interaction_BlacksmithNPC interactionBlacksmithNpc = this;
    AudioManager.Instance.PlayOneShot("event:/dlc/env/legendary_weapon_repair/sequence_start");
    for (int x = 0; x < 5; ++x)
    {
      ResourceCustomTarget.Create(interactionBlacksmithNpc.gameObject, interactionBlacksmithNpc.playerFarming.transform.position, InventoryItem.ITEM_TYPE.WOOL, (System.Action) null);
      yield return (object) new WaitForSeconds(0.1f);
    }
    interactionBlacksmithNpc.weaponToRepair = weapon;
    interactionBlacksmithNpc.PlayRepairSequence();
  }

  public void PlayRepairSequence() => this.StartCoroutine((IEnumerator) this.RepairSequence());

  public IEnumerator RepairSequence()
  {
    Interaction_BlacksmithNPC interactionBlacksmithNpc = this;
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionBlacksmithNpc.gameObject, 5f);
    Interaction_BrokenWeapon brokenWeapon = (Interaction_BrokenWeapon) null;
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT, 1, interactionBlacksmithNpc.transform.position, result: (System.Action<PickUp>) (p =>
    {
      p.enabled = false;
      brokenWeapon = p.GetComponent<Interaction_BrokenWeapon>();
      brokenWeapon.SetWeapon(this.weaponToRepair);
      brokenWeapon.enabled = false;
      p.transform.parent = this.transform.parent;
    }));
    while ((UnityEngine.Object) brokenWeapon == (UnityEngine.Object) null)
      yield return (object) null;
    brokenWeapon.GetComponent<PickUp>().child.transform.localScale = Vector3.one;
    GameManager.GetInstance().OnConversationNext(brokenWeapon.gameObject, 7f);
    bool isMoving = true;
    TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore1 = brokenWeapon.transform.DOMove(interactionBlacksmithNpc.forgePosition.position, 1f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    tweenerCore1.onComplete = tweenerCore1.onComplete + (TweenCallback) (() => isMoving = false);
    while (isMoving)
      yield return (object) null;
    GameManager.GetInstance().CameraSetZoom(5f);
    yield return (object) new WaitForSeconds(0.3f);
    int hitCount = 3;
    for (int x = 0; x < hitCount; ++x)
    {
      yield return (object) new WaitForSeconds(1f);
      interactionBlacksmithNpc.hitImpactVFX.Play();
      BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionBlacksmithNpc.forgePosition.position);
      BiomeConstants.Instance.EmitHitImpactEffect(interactionBlacksmithNpc.forgePosition.position, UnityEngine.Random.value * 360f);
      CameraManager.shakeCamera(4f);
      if (x == hitCount - 1)
      {
        brokenWeapon.SetWeaponVisual();
        BiomeConstants.Instance.ImpactFrameForDuration();
      }
    }
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().CameraSetZoom(7f);
    PlayerSimpleInventory component = PlayerFarming.Instance.GetComponent<PlayerSimpleInventory>();
    Vector3 BookTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f);
    isMoving = true;
    TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore2 = brokenWeapon.transform.DOMove(BookTargetPosition, 1f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    tweenerCore2.onComplete = tweenerCore2.onComplete + (TweenCallback) (() => isMoving = false);
    while (isMoving)
      yield return (object) null;
    brokenWeapon.transform.position = BookTargetPosition;
    PlayerFarming.Instance.CustomAnimation("found-item", true);
    yield return (object) new WaitForSeconds(1f);
    interactionBlacksmithNpc.playerFarming.state.CURRENT_STATE = StateMachine.State.InActive;
    UnityEngine.Object.Destroy((UnityEngine.Object) brokenWeapon.gameObject);
    interactionBlacksmithNpc.RepairWeaponCallback();
  }

  public void NameWeapon() => this.StartCoroutine((IEnumerator) this.NameWeaponSequence());

  public IEnumerator NameWeaponSequence()
  {
    Interaction_BlacksmithNPC interactionBlacksmithNpc = this;
    while (interactionBlacksmithNpc.playerFarming.state.CURRENT_STATE == StateMachine.State.InActive)
      yield return (object) null;
    SimulationManager.Pause();
    GameManager.GetInstance().OnConversationNew();
    if ((UnityEngine.Object) interactionBlacksmithNpc.state == (UnityEngine.Object) null)
      interactionBlacksmithNpc.state = MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.state;
    GameManager.GetInstance().OnConversationNext(interactionBlacksmithNpc.state.gameObject);
    yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/dlc/env/legendary_weapon_repair/naming_start");
    UIWeaponsNameChangeMenu weaponsNameChangeMenu = MonoSingleton<UIManager>.Instance.WeaponsNameChangeMenuTemplate.Instantiate<UIWeaponsNameChangeMenu>();
    weaponsNameChangeMenu.OnNameConfirmed += new System.Action<string>(interactionBlacksmithNpc.SetLegendaryWeaponName);
    weaponsNameChangeMenu.Show(interactionBlacksmithNpc.weaponToRepair, false);
  }

  public void SetLegendaryWeaponName(string newWeaponName)
  {
    string str = "";
    ObjectiveManager.GiveItem(InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT);
    Inventory.ChangeItemQuantity(165, -this.GetWeaponCost(this.weaponToRepair));
    Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT, -1);
    switch (this.weaponToRepair)
    {
      case EquipmentType.Sword_Legendary:
        DataManager.Instance.ForcedStartingWeapon = EquipmentType.Sword_Legendary;
        DataManager.Instance.AddWeapon(EquipmentType.Sword_Legendary);
        this.PlayNamedWeaponConvo(newWeaponName, EquipmentManager.GetWeaponData(EquipmentType.Sword_Legendary).GetLocalisedTitle(), new System.Action(this.PlayUnlockPlinth));
        DataManager.Instance.LegendarySwordCustomName = newWeaponName;
        str = DataManager.Instance.LegendarySwordCustomName;
        break;
      case EquipmentType.Axe_Legendary:
        DataManager.Instance.ForcedStartingWeapon = EquipmentType.Axe_Legendary;
        DataManager.Instance.AddWeapon(EquipmentType.Axe_Legendary);
        this.PlayNamedWeaponConvo(newWeaponName, EquipmentManager.GetWeaponData(EquipmentType.Axe_Legendary).GetLocalisedTitle(), new System.Action(this.PlayUnlockPlinth));
        DataManager.Instance.LegendaryAxeCustomName = newWeaponName;
        break;
      case EquipmentType.Hammer_Legendary:
        DataManager.Instance.ForcedStartingWeapon = EquipmentType.Hammer_Legendary;
        DataManager.Instance.AddWeapon(EquipmentType.Hammer_Legendary);
        DataManager.Instance.OnboardedLegendaryWeapons = true;
        this.PlayNamedWeaponConvo(newWeaponName, EquipmentManager.GetWeaponData(EquipmentType.Hammer_Legendary).GetLocalisedTitle(), new System.Action(this.PlayUnlockPlinth));
        DataManager.Instance.LegendaryHammerCustomName = newWeaponName;
        str = DataManager.Instance.LegendaryHammerCustomName;
        break;
      case EquipmentType.Dagger_Legendary:
        DataManager.Instance.ForcedStartingWeapon = EquipmentType.Dagger_Legendary;
        DataManager.Instance.AddWeapon(EquipmentType.Dagger_Legendary);
        this.PlayNamedWeaponConvo(newWeaponName, EquipmentManager.GetWeaponData(EquipmentType.Dagger_Legendary).GetLocalisedTitle(), new System.Action(this.PlayUnlockPlinth));
        DataManager.Instance.LegendaryDaggerCustomName = newWeaponName;
        str = DataManager.Instance.LegendaryDaggerCustomName;
        break;
      case EquipmentType.Gauntlet_Legendary:
        DataManager.Instance.ForcedStartingWeapon = EquipmentType.Gauntlet_Legendary;
        DataManager.Instance.AddWeapon(EquipmentType.Gauntlet_Legendary);
        this.PlayNamedWeaponConvo(newWeaponName, EquipmentManager.GetWeaponData(EquipmentType.Gauntlet_Legendary).GetLocalisedTitle(), new System.Action(this.PlayUnlockPlinth));
        DataManager.Instance.LegendaryGauntletCustomName = newWeaponName;
        str = DataManager.Instance.LegendaryGauntletCustomName;
        break;
      case EquipmentType.Blunderbuss_Legendary:
        DataManager.Instance.ForcedStartingWeapon = EquipmentType.Blunderbuss_Legendary;
        DataManager.Instance.AddWeapon(EquipmentType.Blunderbuss_Legendary);
        this.PlayNamedWeaponConvo(newWeaponName, EquipmentManager.GetWeaponData(EquipmentType.Blunderbuss_Legendary).GetLocalisedTitle(), new System.Action(this.PlayUnlockPlinth));
        DataManager.Instance.LegendaryBlunderbussCustomName = newWeaponName;
        str = DataManager.Instance.LegendaryBlunderbussCustomName;
        break;
      case EquipmentType.Chain_Legendary:
        DataManager.Instance.ForcedStartingWeapon = EquipmentType.Chain_Legendary;
        DataManager.Instance.AddWeapon(EquipmentType.Chain_Legendary);
        this.PlayNamedWeaponConvo(newWeaponName, EquipmentManager.GetWeaponData(EquipmentType.Chain_Legendary).GetLocalisedTitle(), new System.Action(this.PlayUnlockPlinth));
        DataManager.Instance.LegendaryChainCustomName = newWeaponName;
        str = DataManager.Instance.LegendaryChainCustomName;
        break;
    }
    NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/LegendaryWeaponAddedToPool", str);
    DataManager.CheckAllLegendaryWeaponsUnlocked();
  }

  public void PlayNamedWeaponConvo(
    string newWeaponName,
    string originalWeaponName,
    System.Action callback)
  {
    string namedWeaponTerm = this.GetNamedWeaponTerm(newWeaponName.Equals(originalWeaponName));
    string termVo = MMConversation.GetTermVO(namedWeaponTerm);
    List<ConversationEntry> Entries = new List<ConversationEntry>()
    {
      new ConversationEntry(this.spine.gameObject, string.Format(LocalizationManager.GetTranslation(namedWeaponTerm), (object) newWeaponName))
    };
    foreach (ConversationEntry conversationEntry in Entries)
    {
      conversationEntry.CharacterName = this.characterName;
      conversationEntry.SkeletonData = this.spine;
      conversationEntry.Animation = this.namedWeaponAnimation;
      conversationEntry.soundPath = termVo;
    }
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, callback));
  }

  public string GetNamedWeaponTerm(bool isOriginalWeaponName)
  {
    string str = "Conversation_NPC/BlacksmithNPC/Name/";
    return isOriginalWeaponName ? str + "DefaultName/0" : str + "Renamed/0";
  }

  public void StartLegendaryWeaponQuest()
  {
    DataManager.Instance.FindBrokenHammerWeapon = true;
    DataManager.Instance.GivenBrokenHammerWeaponQuest = true;
    ObjectiveManager.Add((ObjectivesData) new Objectives_CollectItem("Objectives/GroupTitles/LegendaryWeapons", InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT, 1, false, FollowerLocation.Dungeon1_6), true, true);
  }

  public void RepairWeaponCallback()
  {
    switch (this.weaponToRepair)
    {
      case EquipmentType.Sword_Legendary:
        this.repairSwordConversation.gameObject.SetActive(true);
        this.repairSwordConversation.Play();
        break;
      case EquipmentType.Axe_Legendary:
        this.repairAxeConversation.gameObject.SetActive(true);
        this.repairAxeConversation.Play();
        break;
      case EquipmentType.Hammer_Legendary:
        this.repairHammerConversation.gameObject.SetActive(true);
        this.repairHammerConversation.Play();
        break;
      case EquipmentType.Dagger_Legendary:
        this.repairDaggerConversation.gameObject.SetActive(true);
        this.repairDaggerConversation.Play();
        break;
      case EquipmentType.Gauntlet_Legendary:
        this.repairGauntletsConversation.gameObject.SetActive(true);
        this.repairGauntletsConversation.Play();
        break;
      case EquipmentType.Blunderbuss_Legendary:
        this.repairBlunderbussConversation.gameObject.SetActive(true);
        this.repairBlunderbussConversation.Play();
        break;
      case EquipmentType.Chain_Legendary:
        this.repairChainConversation.gameObject.SetActive(true);
        this.repairChainConversation.Play();
        break;
    }
  }

  public void PlayUnlockPlinth() => this.StartCoroutine((IEnumerator) this.UnlockPlinth());

  public IEnumerator UnlockPlinth()
  {
    Interaction_BlacksmithNPC interactionBlacksmithNpc = this;
    while (interactionBlacksmithNpc.playerFarming.state.CURRENT_STATE == StateMachine.State.InActive)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    yield return (object) new WaitForSeconds(0.5f);
    Interaction_LegendaryWeaponPlinth plinth = Interaction_LegendaryWeaponPlinth.GetWeaponPlinth(interactionBlacksmithNpc.weaponToRepair);
    GameManager.GetInstance().OnConversationNext(plinth.gameObject, 7f);
    yield return (object) new WaitForSeconds(1f);
    Interaction_LegendaryWeaponPlinth.DeactivateAllPlinthEffects();
    plinth.SetVisual();
    plinth.Activate();
    interactionBlacksmithNpc.weaponToRepair = EquipmentType.None;
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    SimulationManager.UnPause();
  }

  public bool CanAfford(EquipmentType weaponType)
  {
    return Inventory.GetItemQuantity(165) >= this.GetWeaponCost(weaponType);
  }

  public int GetWeaponCost(EquipmentType weapon)
  {
    int weaponCost = 5;
    if (DataManager.Instance.RepairedLegendaryHammer)
      weaponCost += 5;
    if (DataManager.Instance.RepairedLegendarySword)
      weaponCost += 5;
    if (DataManager.Instance.RepairedLegendaryGauntlet)
      weaponCost += 5;
    if (DataManager.Instance.RepairedLegendaryDagger)
      weaponCost += 5;
    if (DataManager.Instance.RepairedLegendaryChains)
      weaponCost += 5;
    if (DataManager.Instance.RepairedLegendaryBlunderbuss)
      weaponCost += 5;
    if (DataManager.Instance.RepairedLegendaryAxe)
      weaponCost += 5;
    return weaponCost;
  }

  [Serializable]
  public class WeaponEntry
  {
    public EquipmentType Weapon;
    public InventoryItem.ITEM_TYPE CostType;
    public int Cost;
    public EquipmentType[] _weaponItems = new EquipmentType[7]
    {
      EquipmentType.Hammer_Legendary,
      EquipmentType.Sword_Legendary,
      EquipmentType.Dagger_Legendary,
      EquipmentType.Gauntlet_Legendary,
      EquipmentType.Blunderbuss_Legendary,
      EquipmentType.Axe_Legendary,
      EquipmentType.Chain_Legendary
    };
  }
}
