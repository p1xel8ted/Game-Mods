// Decompiled with JetBrains decompiler
// Type: Interaction_Chest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using MMBiomeGeneration;
using MMTools;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class Interaction_Chest : Interaction
{
  public Interaction_Chest.ChestType TypeOfChest;
  public GameObject CameraBone;
  public ColliderEvents DamageCollider;
  public static Interaction_Chest Instance;
  public bool DropFullHeal;
  public GameObject Shadow;
  public GameObject Lighting;
  public bool RevealOnDistance;
  public bool StartRevealed;
  public List<Health> Enemies = new List<Health>();
  public InventoryItemDisplay Item;
  public GameObject PlayerPosition;
  public SkeletonAnimation Spine;
  [SpineEvent("", "", true, false, false)]
  public string ChestLand = "shake";
  public string sLabel;
  public float Timer;
  [CompilerGenerated]
  public Interaction_Chest.State \u003CMyState\u003Ek__BackingField;
  public StealthCover[] StealthCovers;
  public GameObject uIBlueprint;
  public GameObject uINewCard;
  public List<RewardsItem> ChestRewards = new List<RewardsItem>();
  public RewardsItem.ChestRewards OverrideChestReward;
  public int OverrideChestRewardQuantity = 1;
  public bool ForceGoodReward;
  public bool BlockWeapons;
  public bool BossChest;
  public bool BigBossChest;
  [CompilerGenerated]
  public float \u003CDelay\u003Ek__BackingField;
  public float delayTimestamp = -1f;
  public bool active;
  public float CoopChanceForHeartMultiplier = 0.3f;
  public static Interaction_Chest.ChestEvent OnChestRevealed;
  [Space]
  [SerializeField]
  public string spiderCritter;
  public int SilverChestMaxRandom = 12;
  public int GoldChestMaxRandom = 10;
  public AddressableLoader loader;
  public Health EnemyHealth;
  public bool givenReward;
  public float CacheCameraMaxZoom;
  public float CacheCameraMinZoom;
  public float CacheCameraZoomLimiter;
  public bool RevealedGiveReward;
  public FoundItemPickUp p;
  public float coinMultiplier = 1f;
  public InventoryItem.ITEM_TYPE previousPick;
  public InventoryItem.ITEM_TYPE FirstGoldReward;
  public bool skinOrDecSpawned;
  public AsyncOperationHandle<GameObject> weaponPodium_addressableHandle;
  public GameObject _weaponPodiumPrefab;
  public AssetReferenceGameObject Addr_WeaponPodiumPrefab;
  public int DeathCount;
  public bool InCombat;
  public InventoryItem.ITEM_TYPE Reward;
  public HealthPlayer healthPlayer;
  public static int RevealCount = -1;
  public float randomReward;
  public RewardsItem.ChestRewards pickedReward;
  public float previousChance;
  public bool Loop;
  public TarotCards.TarotCard DrawnCard;
  public float totalProbability;
  public float previousTotal;
  public float multiplyer;
  public float previousRewardChance;

  public Interaction_Chest.State MyState
  {
    get => this.\u003CMyState\u003Ek__BackingField;
    set => this.\u003CMyState\u003Ek__BackingField = value;
  }

  public float Delay
  {
    get => this.\u003CDelay\u003Ek__BackingField;
    set => this.\u003CDelay\u003Ek__BackingField = value;
  }

  public void SetTypeOfChest()
  {
    if (this.TypeOfChest == Interaction_Chest.ChestType.None)
    {
      if (!this.BossChest && !DataManager.Instance.NextChestGold)
      {
        if (TrinketManager.HasTrinket(TarotCards.Card.RabbitFoot, this.playerFarming))
        {
          this.SilverChestMaxRandom = 7;
          this.GoldChestMaxRandom = 10;
        }
        else
        {
          this.SilverChestMaxRandom = 15;
          this.GoldChestMaxRandom = 30;
        }
        if (DungeonSandboxManager.Active)
        {
          this.SilverChestMaxRandom = Mathf.RoundToInt((float) (this.SilverChestMaxRandom * 3));
          this.GoldChestMaxRandom = Mathf.RoundToInt((float) (this.GoldChestMaxRandom * 3));
        }
        this.TypeOfChest = UnityEngine.Random.Range(0, this.SilverChestMaxRandom) != 5 || DataManager.Instance.dungeonRun <= 2 ? (UnityEngine.Random.Range(0, this.GoldChestMaxRandom) != 5 || DataManager.Instance.dungeonRun <= 2 ? Interaction_Chest.ChestType.Wooden : Interaction_Chest.ChestType.Gold) : Interaction_Chest.ChestType.Silver;
      }
      else
      {
        this.TypeOfChest = Interaction_Chest.ChestType.Gold;
        DataManager.Instance.NextChestGold = false;
      }
    }
    switch (this.TypeOfChest)
    {
      case Interaction_Chest.ChestType.Wooden:
        this.Spine.skeleton.SetSkin("Wooden");
        break;
      case Interaction_Chest.ChestType.Silver:
        this.Spine.skeleton.SetSkin("Silver");
        break;
      case Interaction_Chest.ChestType.Gold:
        this.Spine.skeleton.SetSkin("Gold");
        break;
    }
  }

  public void Awake()
  {
    this.loader = this.GetComponent<AddressableLoader>();
    this.loader.Initialize();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.InitializeAsync((System.Action) (() =>
    {
      ChestLoader componentInChildren = this.GetComponentInChildren<ChestLoader>(true);
      this.CameraBone = componentInChildren.CameraBone;
      this.DamageCollider = componentInChildren.DamageCollider;
      this.Shadow = componentInChildren.Shadow;
      this.Lighting = componentInChildren.Lighting;
      this.Item = componentInChildren.Item;
      this.PlayerPosition = componentInChildren.PlayerPosition;
      this.Spine = componentInChildren.Spine;
      this.RevealedGiveReward = false;
      this.Enemies.Clear();
      this.Item.gameObject.SetActive(false);
      this.UpdateLocalisation();
      if (this.MyState == Interaction_Chest.State.Hidden)
        this.Spine.AnimationState.SetAnimation(0, "hidden", true);
      else
        this.Spine.AnimationState.SetAnimation(0, "closed", true);
      this.SetTypeOfChest();
      this.Shadow.SetActive(false);
      this.Lighting.SetActive(false);
      if ((UnityEngine.Object) this.transform.parent != (UnityEngine.Object) null)
        this.StealthCovers = this.transform.parent.GetComponentsInChildren<StealthCover>();
      if (!this.RevealOnDistance)
        return;
      this.AutomaticallyInteract = true;
      this.ActivateDistance = 3f;
      this.Spine.AnimationState.SetAnimation(0, "reveal", true);
      this.RevealSfx();
      this.Spine.AnimationState.AddAnimation(0, "closed", true, 0.0f);
      this.MyState = Interaction_Chest.State.Closed;
      this.StartCoroutine((IEnumerator) this.ShowShadow());
    })));
  }

  public IEnumerator InitializeAsync(System.Action onInitialized = null)
  {
    yield return (object) new WaitUntil((Func<bool>) (() => this.loader.isInitialized));
    System.Action action = onInitialized;
    if (action != null)
      action();
  }

  public override void OnEnableInteraction()
  {
    this.StartCoroutine((IEnumerator) this.InitializeAsync((System.Action) (() =>
    {
      base.OnEnableInteraction();
      if (!this.active)
      {
        if (!this.BossChest)
          this.StartCoroutine((IEnumerator) this.DelayGetEnemies());
        if ((UnityEngine.Object) this.DamageCollider != (UnityEngine.Object) null)
        {
          this.DamageCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
          this.DamageCollider.SetActive(false);
        }
      }
      Interaction_Chest.Instance = this;
      this.StartCoroutine((IEnumerator) this.EnableInteractionDelay());
      this.active = true;
      if (this.Enemies.Count > 0)
        return;
      this.StartCoroutine((IEnumerator) this.DelayGetEnemies());
    })));
  }

  public IEnumerator EnableInteractionDelay()
  {
    Interaction_Chest interactionChest = this;
    yield return (object) new WaitForEndOfFrame();
    if ((UnityEngine.Object) interactionChest.Spine != (UnityEngine.Object) null)
      interactionChest.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(interactionChest.HandleEvent);
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if ((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null && this.EnemyHealth.team != Health.Team.PlayerTeam)
      this.EnemyHealth.DealDamage((float) int.MaxValue, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
    TrapSpikes component = this.GetComponent<TrapSpikes>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.DestroySpikes();
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
      this.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    BiomeGenerator.OnRoomActive -= new BiomeGenerator.BiomeAction(this.OnRoomActivate);
    if (!((UnityEngine.Object) Interaction_Chest.Instance == (UnityEngine.Object) this))
      return;
    Interaction_Chest.Instance = (Interaction_Chest) null;
  }

  public override void OnEnable()
  {
    this.StartCoroutine((IEnumerator) this.InitializeAsync((System.Action) (() =>
    {
      base.OnEnable();
      if (!this.StartRevealed || this.givenReward)
        return;
      this.StartCoroutine((IEnumerator) this.DelayReveal());
    })));
  }

  public IEnumerator DelayReveal()
  {
    Interaction_Chest interactionChest = this;
    yield return (object) new WaitForEndOfFrame();
    if (interactionChest.gameObject.activeInHierarchy)
      interactionChest.Reveal();
  }

  public IEnumerator DelayGetEnemies()
  {
    yield return (object) new WaitForEndOfFrame();
    this.GetEnemies();
  }

  public void GetEnemies()
  {
    foreach (Health health in new List<Health>((IEnumerable<Health>) this.transform.parent.GetComponentsInChildren<Health>()))
    {
      if (health.team == Health.Team.Team2 && !health.InanimateObject && !this.Enemies.Contains(health))
      {
        this.Enemies.Add(health);
        health.OnDie += new Health.DieAction(this.OnSpawnedDie);
      }
    }
    BiomeGenerator.OnRoomActive -= new BiomeGenerator.BiomeAction(this.OnRoomActivate);
    BiomeGenerator.OnRoomActive += new BiomeGenerator.BiomeAction(this.OnRoomActivate);
  }

  public void OnRoomActivate()
  {
    if (this.Enemies.Count <= 0 || !((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null) || !((UnityEngine.Object) GameManager.GetInstance().CamFollowTarget != (UnityEngine.Object) null))
      return;
    this.CacheCameraMaxZoom = GameManager.GetInstance().CamFollowTarget.MaxZoom;
    this.CacheCameraMinZoom = GameManager.GetInstance().CamFollowTarget.MinZoom;
    this.CacheCameraZoomLimiter = GameManager.GetInstance().CamFollowTarget.ZoomLimiter;
    GameManager.GetInstance().CamFollowTarget.MaxZoom = 13f;
    GameManager.GetInstance().CamFollowTarget.MinZoom = 11f;
    GameManager.GetInstance().CamFollowTarget.ZoomLimiter = 5f;
    GameManager.GetInstance().AddToCamera(this.gameObject);
  }

  public void AddEnemy(Health h)
  {
    if (!this.Enemies.Contains(h))
    {
      this.Enemies.Add(h);
      if (!(bool) (UnityEngine.Object) h)
        return;
      h.OnDie += new Health.DieAction(this.OnSpawnedDie);
    }
    else
      Debug.Log((object) "Attempt to add enemy more than once to chest");
  }

  public void QueryRoomWeapon()
  {
    Debug.Log((object) ("BiomeGenerator.Instance.CurrentRoom.HasWeapon: " + BiomeGenerator.Instance.CurrentRoom.HasWeapon.ToString()));
  }

  public void Reveal()
  {
    if (this.givenReward)
      return;
    this.givenReward = true;
    this.StartCoroutine((IEnumerator) this.InitializeAsync((System.Action) (() =>
    {
      if (!DataManager.Instance.ShownInventoryTutorial)
      {
        GameManager.GetInstance().OnConversationNew();
        GameManager.GetInstance().OnConversationNext(this.playerFarming.gameObject);
        this.playerFarming._state.facingAngle = Utils.GetAngle(this.playerFarming.transform.position, this.gameObject.transform.position);
        CameraFollowTarget cameraFollowTarget = CameraFollowTarget.Instance;
        cameraFollowTarget.SetOffset((this.gameObject.transform.position - this.playerFarming.transform.position) * 0.85f);
        HUD_Manager.Instance.Hide(false, 0);
        this.StartCoroutine((IEnumerator) this.DelayCallback(2f, (System.Action) (() => cameraFollowTarget.SetOffset(Vector3.zero))));
      }
      Debug.Log((object) "Reveal()");
      this.RevealedGiveReward = true;
      this.StartCoroutine((IEnumerator) this.DamageColliderRoutine());
      if (!this.BlockWeapons && (UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && BiomeGenerator.Instance.CurrentRoom.HasWeapon && !BiomeGenerator.Instance.OnboardingDungeon5)
      {
        Debug.Log((object) "CHEST: Weapon!".Colour(Color.red));
        if (DataManager.Instance.WeaponPool.Count > 2 && DataManager.Instance.CursePool.Count > 2)
        {
          UnityEngine.Object.Instantiate<GameObject>(this.WeaponPodiumPrefab, this.transform.position - new Vector3(-1f, 0.0f), Quaternion.identity, this.transform.parent);
          UnityEngine.Object.Instantiate<GameObject>(this.WeaponPodiumPrefab, this.transform.position - new Vector3(1f, 0.0f), Quaternion.identity, this.transform.parent);
        }
        else
          UnityEngine.Object.Instantiate<GameObject>(this.WeaponPodiumPrefab, this.transform.position - new Vector3(0.0f, 0.0f), Quaternion.identity, this.transform.parent);
        Interaction_Chest.ChestEvent onChestRevealed = Interaction_Chest.OnChestRevealed;
        if (onChestRevealed != null)
          onChestRevealed();
        this.MyState = Interaction_Chest.State.Open;
      }
      else
      {
        Debug.Log((object) "CHEST: No weapon".Colour(Color.red));
        float num = 0.04f * TrinketManager.GetChanceForRelicsMultiplier(this.playerFarming);
        if (this.playerFarming.currentRelicType == RelicType.None)
          num += 0.04f;
        foreach (UnityEngine.Object player in PlayerFarming.players)
        {
          if (player != (UnityEngine.Object) this.playerFarming)
            num -= 0.06f;
        }
        if (!BiomeGenerator.Instance.HasSpawnedRelic && (double) UnityEngine.Random.value < (double) num && DataManager.Instance.OnboardedRelics && this.pickedReward != RewardsItem.ChestRewards.DISSENTER && this.pickedReward != RewardsItem.ChestRewards.MISSIONARY && !BiomeGenerator.Instance.OnboardingDungeon5)
        {
          Debug.Log((object) "CHEST: Relic!".Colour(Color.red));
          UnityEngine.Object.Instantiate<GameObject>(this.WeaponPodiumPrefab, this.transform.position - new Vector3(0.0f, 0.0f), Quaternion.identity, this.transform.parent).GetComponent<Interaction_WeaponChoiceChest>().Type = Interaction_WeaponSelectionPodium.Types.Relic;
          BiomeGenerator.Instance.HasSpawnedRelic = true;
          Interaction_Chest.ChestEvent onChestRevealed = Interaction_Chest.OnChestRevealed;
          if (onChestRevealed != null)
            onChestRevealed();
          this.MyState = Interaction_Chest.State.Open;
          this.Spine.gameObject.SetActive(false);
        }
        else
        {
          Debug.Log((object) "CHEST: No Relic".Colour(Color.red));
          this.Spine.AnimationState.SetAnimation(0, "reveal", true);
          this.RevealSfx();
          this.Spine.AnimationState.AddAnimation(0, "closed", true, 0.0f);
          this.MyState = Interaction_Chest.State.Closed;
          this.StartCoroutine((IEnumerator) this.ShowShadow());
          this.SelectReward();
          this.MyState = Interaction_Chest.State.Open;
          GameManager.GetInstance().StartCoroutine((IEnumerator) this.GiveRewardDelay());
          Projectile.ClearProjectiles();
        }
      }
    })));
  }

  public IEnumerator DelayCallback(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public IEnumerator ShowShadow()
  {
    this.Shadow.SetActive(true);
    this.Lighting?.SetActive(true);
    float Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      this.Shadow.transform.localScale = new Vector3(3f, 2f, 1f) * (Progress / Duration);
      yield return (object) null;
    }
    this.Shadow.transform.localScale = new Vector3(3f, 2f, 1f);
  }

  public void RevealBossReward(InventoryItem.ITEM_TYPE ForceItem, System.Action callback = null)
  {
    this.StartCoroutine((IEnumerator) this.DamageColliderRoutine());
    this.StartCoroutine((IEnumerator) this.GiveBossReward(ForceItem, callback));
  }

  public void RevealSfx()
  {
    switch (this.TypeOfChest)
    {
      case Interaction_Chest.ChestType.Wooden:
        AudioManager.Instance.PlayOneShot("event:/chests/chest_small_appear", this.gameObject);
        break;
      case Interaction_Chest.ChestType.Silver:
        AudioManager.Instance.PlayOneShot("event:/chests/chest_big_appear", this.gameObject);
        break;
      case Interaction_Chest.ChestType.Gold:
        AudioManager.Instance.PlayOneShot("event:/chests/chest_big_appear", this.gameObject);
        break;
      default:
        AudioManager.Instance.PlayOneShot("event:/chests/chest_small_appear", this.gameObject);
        break;
    }
  }

  public void ChestLandSfx()
  {
    switch (this.TypeOfChest)
    {
      case Interaction_Chest.ChestType.Wooden:
        AudioManager.Instance.PlayOneShot("event:/chests/chest_small_land", this.gameObject);
        break;
      case Interaction_Chest.ChestType.Silver:
        AudioManager.Instance.PlayOneShot("event:/chests/chest_big_land", this.gameObject);
        break;
      case Interaction_Chest.ChestType.Gold:
        AudioManager.Instance.PlayOneShot("event:/chests/chest_big_land", this.gameObject);
        break;
      default:
        AudioManager.Instance.PlayOneShot("event:/chests/chest_small_land", this.gameObject);
        break;
    }
  }

  public void ChestOpenSfx()
  {
    switch (this.TypeOfChest)
    {
      case Interaction_Chest.ChestType.Wooden:
        AudioManager.Instance.PlayOneShot("event:/chests/chest_small_open", this.gameObject);
        break;
      case Interaction_Chest.ChestType.Silver:
        AudioManager.Instance.PlayOneShot("event:/chests/chest_big_open", this.gameObject);
        break;
      case Interaction_Chest.ChestType.Gold:
        AudioManager.Instance.PlayOneShot("event:/chests/chest_big_open", this.gameObject);
        break;
      default:
        AudioManager.Instance.PlayOneShot("event:/chests/chest_small_open", this.gameObject);
        break;
    }
  }

  public IEnumerator GiveBossReward(InventoryItem.ITEM_TYPE ForceIncludeItem, System.Action callback = null)
  {
    Interaction_Chest interactionChest = this;
    interactionChest.RevealedGiveReward = false;
    bool roomCompleteOnOpen = true;
    if (PlayerFarming.Location != FollowerLocation.IntroDungeon)
    {
      interactionChest.StartCoroutine((IEnumerator) interactionChest.ShowShadow());
      interactionChest.Spine.AnimationState.SetAnimation(0, "reveal", true);
      interactionChest.RevealSfx();
      interactionChest.Spine.AnimationState.AddAnimation(0, "closed", true, 0.0f);
      interactionChest.MyState = Interaction_Chest.State.Closed;
      yield return (object) new WaitForSeconds(1.13333333f);
      interactionChest.Spine.AnimationState.SetAnimation(0, "open", false);
      interactionChest.ChestOpenSfx();
      interactionChest.Spine.AnimationState.AddAnimation(0, "opened", true, 0.0f);
      interactionChest.StartCoroutine((IEnumerator) interactionChest.GiveBlackSouls());
      if (interactionChest.DropFullHeal)
        yield return (object) interactionChest.StartCoroutine((IEnumerator) interactionChest.GiveFullHeal());
      Debug.Log((object) ("chest ForceIncludeItem " + ForceIncludeItem.ToString()));
      if (ForceIncludeItem != InventoryItem.ITEM_TYPE.NONE)
      {
        yield return (object) new WaitForSeconds(0.2f);
        InventoryItem.Spawn(ForceIncludeItem, 1, interactionChest.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(5f, 250f);
        AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionChest.gameObject);
        Debug.Log((object) "SPAWNED FORCED ITEM!");
      }
      yield return (object) new WaitForSeconds(0.2f);
      List<PickUp> itemsRequired = new List<PickUp>();
      int Rewards;
      if (!interactionChest.BigBossChest)
      {
        interactionChest.coinMultiplier = 1f;
        Rewards = UnityEngine.Random.Range(10, 15) * (int) DungeonModifier.HasPositiveModifier(DungeonPositiveModifier.DoubleGold, 2f, 1f);
        Rewards = (int) ((double) Rewards * (double) interactionChest.coinMultiplier);
        if (DoctrineUpgradeSystem.TrySermonsStillAvailable() && DoctrineUpgradeSystem.TryGetStillDoctrineStone() && DataManager.Instance.GetVariable(DataManager.Variables.FirstDoctrineStone))
        {
          PickUp pickUp = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.DOCTRINE_STONE, 1, interactionChest.transform.position);
          if ((UnityEngine.Object) pickUp != (UnityEngine.Object) null)
          {
            pickUp.SetInitialSpeedAndDiraction(5f, 250f);
            AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionChest.gameObject);
            Interaction_DoctrineStone component = pickUp.GetComponent<Interaction_DoctrineStone>();
            if ((UnityEngine.Object) component != (UnityEngine.Object) null)
              component.AutomaticallyInteract = true;
          }
        }
        switch (PlayerFarming.Location)
        {
          case FollowerLocation.Dungeon1_5:
          case FollowerLocation.Dungeon1_6:
            interactionChest.IncludeDecOrSkin();
            interactionChest.SpawnGoodReward(includeTarot: false);
            break;
          case FollowerLocation.Boss_Yngya:
            interactionChest.IncludeDecOrSkin();
            break;
          case FollowerLocation.Boss_Wolf:
            PickUp pickUp1 = interactionChest.IncludeDecOrSkin();
            if (!DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_5) && !DungeonSandboxManager.Active)
            {
              itemsRequired.Add(pickUp1);
              break;
            }
            break;
        }
      }
      else
      {
        switch (PlayerFarming.Location)
        {
          case FollowerLocation.Dungeon1_5:
          case FollowerLocation.Dungeon1_6:
          case FollowerLocation.Boss_Yngya:
            interactionChest.IncludeDecOrSkin();
            break;
          case FollowerLocation.Boss_Wolf:
            PickUp pickUp2 = interactionChest.IncludeDecOrSkin();
            if (!DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_5) && !DungeonSandboxManager.Active && (UnityEngine.Object) pickUp2 != (UnityEngine.Object) null)
            {
              itemsRequired.Add(pickUp2);
              break;
            }
            break;
        }
        if (!DungeonSandboxManager.Active)
        {
          switch (PlayerFarming.Location)
          {
            case FollowerLocation.Dungeon1_1:
              if (!StructuresData.GetUnlocked(StructureBrain.TYPES.DECORATION_BOSS_TROPHY_1))
              {
                interactionChest.p = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION, 1, interactionChest.transform.position).GetComponent<FoundItemPickUp>();
                AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionChest.gameObject);
                interactionChest.p.DecorationType = StructureBrain.TYPES.DECORATION_BOSS_TROPHY_1;
                interactionChest.p.GetComponent<PickUp>().DisableSeperation = true;
                FoundItemPickUp component = interactionChest.p.GetComponent<FoundItemPickUp>();
                if ((UnityEngine.Object) component != (UnityEngine.Object) null)
                {
                  component.AutomaticallyInteract = true;
                  break;
                }
                break;
              }
              if (ForceIncludeItem == InventoryItem.ITEM_TYPE.GOD_TEAR)
              {
                for (int index = 0; index < 2; ++index)
                  itemsRequired.Add(InventoryItem.Spawn(InventoryItem.ITEM_TYPE.GOD_TEAR, 1, interactionChest.transform.position));
                break;
              }
              break;
            case FollowerLocation.Dungeon1_2:
              if (!StructuresData.GetUnlocked(StructureBrain.TYPES.DECORATION_BOSS_TROPHY_2))
              {
                interactionChest.p = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION, 1, interactionChest.transform.position).GetComponent<FoundItemPickUp>();
                AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionChest.gameObject);
                interactionChest.p.DecorationType = StructureBrain.TYPES.DECORATION_BOSS_TROPHY_2;
                interactionChest.p.GetComponent<PickUp>().DisableSeperation = true;
                FoundItemPickUp component = interactionChest.p.GetComponent<FoundItemPickUp>();
                if ((UnityEngine.Object) component != (UnityEngine.Object) null)
                {
                  component.AutomaticallyInteract = true;
                  break;
                }
                break;
              }
              if (ForceIncludeItem == InventoryItem.ITEM_TYPE.GOD_TEAR)
              {
                for (int index = 0; index < 2; ++index)
                  itemsRequired.Add(InventoryItem.Spawn(InventoryItem.ITEM_TYPE.GOD_TEAR, 1, interactionChest.transform.position));
                break;
              }
              break;
            case FollowerLocation.Dungeon1_3:
              if (!StructuresData.GetUnlocked(StructureBrain.TYPES.DECORATION_BOSS_TROPHY_3))
              {
                interactionChest.p = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION, 1, interactionChest.transform.position).GetComponent<FoundItemPickUp>();
                AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionChest.gameObject);
                interactionChest.p.DecorationType = StructureBrain.TYPES.DECORATION_BOSS_TROPHY_3;
                interactionChest.p.GetComponent<PickUp>().DisableSeperation = true;
                FoundItemPickUp component = interactionChest.p.GetComponent<FoundItemPickUp>();
                if ((UnityEngine.Object) component != (UnityEngine.Object) null)
                {
                  component.AutomaticallyInteract = true;
                  break;
                }
                break;
              }
              if (ForceIncludeItem == InventoryItem.ITEM_TYPE.GOD_TEAR)
              {
                for (int index = 0; index < 2; ++index)
                  itemsRequired.Add(InventoryItem.Spawn(InventoryItem.ITEM_TYPE.GOD_TEAR, 1, interactionChest.transform.position));
                break;
              }
              break;
            case FollowerLocation.Dungeon1_4:
              if (!StructuresData.GetUnlocked(StructureBrain.TYPES.DECORATION_BOSS_TROPHY_4) && !DungeonSandboxManager.Active)
              {
                interactionChest.p = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION, 1, interactionChest.transform.position).GetComponent<FoundItemPickUp>();
                AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionChest.gameObject);
                interactionChest.p.DecorationType = StructureBrain.TYPES.DECORATION_BOSS_TROPHY_4;
                interactionChest.p.GetComponent<PickUp>().DisableSeperation = true;
                FoundItemPickUp component = interactionChest.p.GetComponent<FoundItemPickUp>();
                if ((UnityEngine.Object) component != (UnityEngine.Object) null)
                {
                  component.AutomaticallyInteract = true;
                  break;
                }
                break;
              }
              if (ForceIncludeItem == InventoryItem.ITEM_TYPE.GOD_TEAR)
              {
                for (int index = 0; index < 2; ++index)
                  itemsRequired.Add(InventoryItem.Spawn(InventoryItem.ITEM_TYPE.GOD_TEAR, 1, interactionChest.transform.position));
                break;
              }
              break;
            case FollowerLocation.Dungeon1_5:
            case FollowerLocation.Boss_Wolf:
              if (!DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_5))
              {
                if (DataManager.Instance.GivenUpHeartToWolf)
                {
                  GameManager.GetInstance().OnConversationNew();
                  GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject);
                  PermanentHeart_CustomTarget.Create(interactionChest.transform.position, PlayerFarming.Instance.gameObject.transform.position, 2f, (System.Action<Interaction_PermanentHeart>) null);
                }
                interactionChest.StartCoroutine((IEnumerator) interactionChest.SpawnGivenUpWolfFood());
              }
              if (!StructuresData.GetUnlocked(StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_WOLF) && !DungeonSandboxManager.Active)
              {
                interactionChest.p = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION, 1, interactionChest.transform.position).GetComponent<FoundItemPickUp>();
                AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionChest.gameObject);
                interactionChest.p.DecorationType = StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_WOLF;
                interactionChest.p.GetComponent<PickUp>().DisableSeperation = true;
                FoundItemPickUp component = interactionChest.p.GetComponent<FoundItemPickUp>();
                if ((UnityEngine.Object) component != (UnityEngine.Object) null)
                {
                  component.AutomaticallyInteract = true;
                  break;
                }
                break;
              }
              if (ForceIncludeItem == InventoryItem.ITEM_TYPE.GOD_TEAR)
              {
                for (int index = 0; index < 2; ++index)
                  itemsRequired.Add(InventoryItem.Spawn(InventoryItem.ITEM_TYPE.GOD_TEAR, 1, interactionChest.transform.position));
                break;
              }
              break;
            case FollowerLocation.Dungeon1_6:
            case FollowerLocation.Boss_Yngya:
              if (!StructuresData.GetUnlocked(StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_YNGYA) && !DungeonSandboxManager.Active)
              {
                interactionChest.p = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION, 1, interactionChest.transform.position).GetComponent<FoundItemPickUp>();
                AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionChest.gameObject);
                interactionChest.p.DecorationType = StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_YNGYA;
                interactionChest.p.GetComponent<PickUp>().DisableSeperation = true;
                FoundItemPickUp component = interactionChest.p.GetComponent<FoundItemPickUp>();
                if ((UnityEngine.Object) component != (UnityEngine.Object) null)
                {
                  component.AutomaticallyInteract = true;
                  break;
                }
                break;
              }
              if (ForceIncludeItem == InventoryItem.ITEM_TYPE.GOD_TEAR)
              {
                for (int index = 0; index < 2; ++index)
                  itemsRequired.Add(InventoryItem.Spawn(InventoryItem.ITEM_TYPE.GOD_TEAR, 1, interactionChest.transform.position));
                break;
              }
              break;
          }
        }
        if ((UnityEngine.Object) interactionChest.p != (UnityEngine.Object) null)
          interactionChest.p.GetComponent<PickUp>().SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
        Rewards = UnityEngine.Random.Range(25, 50) * (int) DungeonModifier.HasPositiveModifier(DungeonPositiveModifier.DoubleGold, 2f, 1f);
        if (DoctrineUpgradeSystem.TrySermonsStillAvailable() && DoctrineUpgradeSystem.TryGetStillDoctrineStone() && DataManager.Instance.GetVariable(DataManager.Variables.FirstDoctrineStone))
        {
          ForceIncludeItem = InventoryItem.ITEM_TYPE.DOCTRINE_STONE;
          PickUp pickUp3 = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.DOCTRINE_STONE, 1, interactionChest.transform.position);
          if ((UnityEngine.Object) pickUp3 != (UnityEngine.Object) null)
          {
            pickUp3.SetInitialSpeedAndDiraction(5f, 250f);
            AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionChest.gameObject);
            Interaction_DoctrineStone component = pickUp3.GetComponent<Interaction_DoctrineStone>();
            if ((UnityEngine.Object) component != (UnityEngine.Object) null)
              component.MagnetToPlayer();
          }
          yield return (object) new WaitForSeconds(0.1f);
        }
      }
      int i = -1;
      while (++i <= Rewards)
      {
        AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionChest.gameObject);
        CameraManager.shakeCamera(UnityEngine.Random.Range(0.4f, 0.6f));
        PickUp pickUp = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, interactionChest.transform.position + Vector3.back, 0.0f);
        pickUp.SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
        pickUp.MagnetDistance = 3f;
        pickUp.CanStopFollowingPlayer = false;
        yield return (object) new WaitForSeconds(0.01f);
      }
      if (DataManager.Instance.BonesEnabled && !DataManager.Instance.DeathCatBeaten)
      {
        i = -1;
        int Bones = UnityEngine.Random.Range(10, 15);
        while (++i <= Bones)
        {
          AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionChest.gameObject);
          CameraManager.shakeCamera(UnityEngine.Random.Range(0.4f, 0.6f));
          PickUp pickUp = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BONE, 1, interactionChest.transform.position + Vector3.back, 0.0f);
          if ((UnityEngine.Object) pickUp != (UnityEngine.Object) null)
          {
            pickUp.SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
            pickUp.MagnetDistance = 3f;
            pickUp.CanStopFollowingPlayer = false;
          }
          yield return (object) new WaitForSeconds(0.01f);
        }
      }
      yield return (object) new WaitForSeconds(0.5f);
      interactionChest.Reward = InventoryItem.ITEM_TYPE.BLACK_GOLD;
      if (interactionChest.Reward == InventoryItem.ITEM_TYPE.BLACK_GOLD)
      {
        for (int index = 0; index < 10; ++index)
        {
          PickUp pickUp = InventoryItem.Spawn(interactionChest.Reward, 1, interactionChest.transform.position + Vector3.back, 0.0f);
          pickUp.SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
          pickUp.MagnetDistance = 3f;
          pickUp.CanStopFollowingPlayer = false;
        }
      }
      else
      {
        PickUp pickUp = InventoryItem.Spawn(interactionChest.Reward, 1, interactionChest.transform.position + Vector3.back, 0.0f);
        pickUp.SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
        pickUp.MagnetDistance = 3f;
        pickUp.CanStopFollowingPlayer = false;
      }
      yield return (object) new WaitForSeconds(2f);
      if (ForceIncludeItem == InventoryItem.ITEM_TYPE.KEY_PIECE)
      {
        while ((UnityEngine.Object) Interaction_KeyPiece.Instance != (UnityEngine.Object) null)
          yield return (object) null;
      }
      if (ForceIncludeItem == InventoryItem.ITEM_TYPE.BEHOLDER_EYE || ForceIncludeItem == InventoryItem.ITEM_TYPE.BEHOLDER_EYE_ROT)
      {
        while ((UnityEngine.Object) BeholderEye.Instance != (UnityEngine.Object) null)
          yield return (object) null;
      }
      if (ForceIncludeItem == InventoryItem.ITEM_TYPE.DOCTRINE_STONE)
      {
        while (Interaction_DoctrineStone.DoctrineStones.Count > 0)
          yield return (object) null;
      }
      while (true)
      {
        int num = 0;
        foreach (UnityEngine.Object @object in itemsRequired)
        {
          if (@object == (UnityEngine.Object) null)
            ++num;
        }
        if (num < itemsRequired.Count)
          yield return (object) null;
        else
          break;
      }
      while ((UnityEngine.Object) interactionChest.p != (UnityEngine.Object) null)
        yield return (object) null;
      while ((UnityEngine.Object) Interaction_DivineCrystal.Instance != (UnityEngine.Object) null)
        yield return (object) null;
      if (roomCompleteOnOpen)
        RoomLockController.RoomCompleted(true);
      interactionChest.RevealedGiveReward = true;
      System.Action action = callback;
      if (action != null)
        action();
    }
  }

  public PickUp IncludeDecOrSkin(float chance = 1f)
  {
    if ((double) UnityEngine.Random.value > (double) chance)
      return (PickUp) null;
    List<RewardsItem.ChestRewards> chestRewardsList = new List<RewardsItem.ChestRewards>();
    if (DataManager.CheckIfThereAreSkinsAvailable())
      chestRewardsList.Add(RewardsItem.ChestRewards.FOLLOWER_SKIN);
    if (DataManager.Instance.GetDecorationListFromLocation(PlayerFarming.Location).Count > 0)
      chestRewardsList.Add(RewardsItem.ChestRewards.BASE_DECORATION);
    if (PlayerFarming.Location == FollowerLocation.Dungeon1_6 && !DataManager.Instance.ClothesUnlocked(FollowerClothingType.Winter_1))
    {
      chestRewardsList.Clear();
      chestRewardsList.Add(RewardsItem.ChestRewards.OUTFIT);
    }
    PickUp pickUp = (PickUp) null;
    if (chestRewardsList.Count > 0)
    {
      int index = UnityEngine.Random.Range(0, chestRewardsList.Count);
      this.Reward = RewardsItem.Instance.ReturnItemType(chestRewardsList[index]);
      pickUp = InventoryItem.Spawn(this.Reward, 1, this.transform.position + Vector3.back, 0.0f);
      if ((UnityEngine.Object) pickUp != (UnityEngine.Object) null)
        this.p = pickUp.GetComponent<FoundItemPickUp>();
      if ((UnityEngine.Object) this.p != (UnityEngine.Object) null && (UnityEngine.Object) pickUp != (UnityEngine.Object) null)
      {
        this.p.GetComponent<PickUp>().SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
        if (chestRewardsList[index] == RewardsItem.ChestRewards.OUTFIT && TailorManager.GetClothingFromChest(true).Count > 0)
          this.p.clothingType = TailorManager.GetClothingFromChest(true)[0];
      }
      if ((UnityEngine.Object) pickUp != (UnityEngine.Object) null)
      {
        FoundItemPickUp component = pickUp.GetComponent<FoundItemPickUp>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.AutomaticallyInteract = true;
      }
    }
    return pickUp;
  }

  public InventoryItem.ITEM_TYPE GetRandomFoodItem()
  {
    return PlayerFarming.Location == FollowerLocation.Dungeon1_1 ? CookingData.GetLowQualityFoods()[UnityEngine.Random.Range(0, CookingData.GetLowQualityFoods().Length)] : CookingData.GetMediumQualityFoods()[UnityEngine.Random.Range(0, CookingData.GetMediumQualityFoods().Length)];
  }

  public IEnumerator GiveFullHeal()
  {
    Interaction_Chest interactionChest = this;
    int i = -1;
    while ((double) ++i < (double) interactionChest.playerFarming.health.totalHP / 2.0)
    {
      AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionChest.gameObject);
      CameraManager.shakeCamera(UnityEngine.Random.Range(0.4f, 0.6f));
      PickUp pickUp = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.RED_HEART, 1, interactionChest.transform.position + Vector3.back, 0.0f);
      pickUp.SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      pickUp.MagnetDistance = 2f;
      pickUp.CanStopFollowingPlayer = false;
      yield return (object) new WaitForSeconds(0.1f);
    }
  }

  public IEnumerator GiveBlackSouls()
  {
    Interaction_Chest interactionChest = this;
    int Rewards = 25;
    int i = -1;
    while (++i < Rewards)
    {
      AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionChest.gameObject);
      CameraManager.shakeCamera(UnityEngine.Random.Range(0.4f, 0.6f));
      BlackSoul blackSoul = InventoryItem.SpawnBlackSoul(1, interactionChest.transform.position + Vector3.back);
      if ((UnityEngine.Object) blackSoul != (UnityEngine.Object) null)
        blackSoul.SetAngle((float) (270 + UnityEngine.Random.Range(-90, 90)), new Vector2(2f, 4f));
      yield return (object) new WaitForSeconds(0.05f);
    }
  }

  public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == this.ChestLand))
      return;
    this.ChestLandSfx();
    CameraManager.shakeCamera(0.5f);
    if (!((UnityEngine.Object) this.Spine != (UnityEngine.Object) null))
      return;
    this.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  public IEnumerator DamageColliderRoutine()
  {
    if ((UnityEngine.Object) this.DamageCollider != (UnityEngine.Object) null)
      this.DamageCollider.SetActive(true);
    yield return (object) new WaitForSeconds(0.1f);
    if ((UnityEngine.Object) this.DamageCollider != (UnityEngine.Object) null)
      this.DamageCollider.SetActive(false);
  }

  public int ChestCoinMultiplier()
  {
    switch (this.TypeOfChest)
    {
      case Interaction_Chest.ChestType.Wooden:
        return 0;
      case Interaction_Chest.ChestType.Silver:
        return 2;
      case Interaction_Chest.ChestType.Gold:
        return 5;
      default:
        return 0;
    }
  }

  public void SpawnGoodReward(
    float includeDLCSkinChance = 0.0f,
    float includeDLCDecoChance = 0.0f,
    float includeDLCNecklaceChance = 0.0f,
    bool includeTarot = true)
  {
    AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", this.gameObject);
    bool flag1 = false;
    if ((double) includeDLCSkinChance > 0.0 && (double) UnityEngine.Random.value < (double) includeDLCSkinChance && (PlayerFarming.Location == FollowerLocation.Dungeon1_5 || PlayerFarming.Location == FollowerLocation.Dungeon1_6 || PlayerFarming.Location == FollowerLocation.Boss_Wolf || PlayerFarming.Location == FollowerLocation.Boss_Yngya) && DataManager.CheckIfThereAreSkinsAvailable())
    {
      this.Reward = RewardsItem.Instance.ReturnItemType(RewardsItem.ChestRewards.FOLLOWER_SKIN);
      flag1 = true;
    }
    bool flag2 = false;
    if ((double) includeDLCDecoChance > 0.0 && (double) UnityEngine.Random.value < (double) includeDLCDecoChance && (PlayerFarming.Location == FollowerLocation.Dungeon1_5 || PlayerFarming.Location == FollowerLocation.Dungeon1_6 || PlayerFarming.Location == FollowerLocation.Boss_Wolf || PlayerFarming.Location == FollowerLocation.Boss_Yngya) && DataManager.Instance.GetDecorationListFromLocation(PlayerFarming.Location).Count > 0)
    {
      this.Reward = InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION;
      flag2 = true;
    }
    bool flag3 = PlayerFarming.Location == FollowerLocation.Dungeon1_5 || PlayerFarming.Location == FollowerLocation.Dungeon1_6 || PlayerFarming.Location == FollowerLocation.Boss_Wolf || PlayerFarming.Location == FollowerLocation.Boss_Yngya;
    bool flag4 = !flag2 && !flag3;
    bool flag5 = !flag1 && !flag3;
    if (!flag1 && !flag2)
    {
      RewardsItem instance1 = RewardsItem.Instance;
      RewardsItem instance2 = RewardsItem.Instance;
      int num1 = includeTarot ? 1 : 0;
      bool flag6 = flag4;
      int num2 = flag5 ? 1 : 0;
      int num3 = flag6 ? 1 : 0;
      int goodReward = (int) instance2.GetGoodReward(num1 != 0, includeSkin: num2 != 0, includeDeco: num3 != 0);
      this.Reward = instance1.ReturnItemType((RewardsItem.ChestRewards) goodReward);
    }
    RewardsItem instance3;
    int goodReward1;
    for (int index = 100; this.Reward == this.previousPick && --index > 0; this.Reward = instance3.ReturnItemType((RewardsItem.ChestRewards) goodReward1))
    {
      instance3 = RewardsItem.Instance;
      RewardsItem instance4 = RewardsItem.Instance;
      int num4 = includeTarot ? 1 : 0;
      bool flag7 = flag4;
      int num5 = flag5 ? 1 : 0;
      int num6 = flag7 ? 1 : 0;
      goodReward1 = (int) instance4.GetGoodReward(num4 != 0, includeSkin: num5 != 0, includeDeco: num6 != 0);
    }
    this.previousPick = this.Reward;
    if (this.FirstGoldReward != InventoryItem.ITEM_TYPE.NONE)
      this.FirstGoldReward = this.Reward;
    else if (InventoryItem.IsGiftOrNecklace(this.FirstGoldReward) && InventoryItem.IsGiftOrNecklace(this.Reward))
      this.Reward = InventoryItem.ITEM_TYPE.BLACK_GOLD;
    bool flag8 = false;
    if ((double) includeDLCNecklaceChance > 0.0 && (double) UnityEngine.Random.value < (double) includeDLCNecklaceChance && (PlayerFarming.Location == FollowerLocation.Dungeon1_5 || PlayerFarming.Location == FollowerLocation.Dungeon1_6 || PlayerFarming.Location == FollowerLocation.Boss_Wolf || PlayerFarming.Location == FollowerLocation.Boss_Yngya))
    {
      this.Reward = InventoryItem.Necklaces_DLC[UnityEngine.Random.Range(0, InventoryItem.Necklaces_DLC.Count)];
      flag8 = true;
    }
    if (this.Reward == InventoryItem.ITEM_TYPE.GOLD_NUGGET)
    {
      int num = UnityEngine.Random.Range(20, 30);
      for (int index = 0; index < num; ++index)
        InventoryItem.Spawn(this.Reward, 1, this.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(5f, (float) UnityEngine.Random.Range(150, 350));
    }
    else
    {
      if (this.Reward == InventoryItem.ITEM_TYPE.BLACK_GOLD)
        this.Reward = DungeonModifier.HasNeutralModifier(DungeonNeutralModifier.ChestsDropFoodNotGold) ? this.GetRandomFoodItem() : this.Reward;
      if (this.Reward == InventoryItem.ITEM_TYPE.BLACK_GOLD)
      {
        for (int index = 0; index < 10; ++index)
          InventoryItem.Spawn(this.Reward, 1, this.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      }
      else if (InventoryItem.IsFood(this.Reward))
      {
        int num = UnityEngine.Random.Range(3, 6);
        for (int index = 0; index < num; ++index)
          InventoryItem.Spawn(this.Reward, 1, this.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(5f, (float) UnityEngine.Random.Range(150, 350));
      }
      else if (this.Reward == InventoryItem.ITEM_TYPE.FOUND_ITEM_OUTFIT)
      {
        List<FollowerClothingType> clothingFromChest = TailorManager.GetClothingFromChest(true);
        PickUp pickUp = InventoryItem.Spawn(this.Reward, 1, this.transform.position + Vector3.back, 0.0f);
        pickUp.SetInitialSpeedAndDiraction(5f, (float) UnityEngine.Random.Range(150, 350));
        pickUp.GetComponent<FoundItemPickUp>().clothingType = clothingFromChest[0];
      }
      else if (this.Reward == InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION)
      {
        PickUp pickUp = InventoryItem.Spawn(this.Reward, 1, this.transform.position + Vector3.back, 0.0f);
        if ((bool) (UnityEngine.Object) pickUp)
          pickUp.SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
        FoundItemPickUp component = pickUp.GetComponent<FoundItemPickUp>();
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
          return;
        component.AutomaticallyInteract = true;
        List<StructureBrain.TYPES> listFromLocation = DataManager.Instance.GetDecorationListFromLocation(PlayerFarming.Location);
        if (listFromLocation != null && listFromLocation.Count > 0)
          component.DecorationType = listFromLocation[UnityEngine.Random.Range(0, listFromLocation.Count)];
        else
          UnityEngine.Object.Destroy((UnityEngine.Object) pickUp.gameObject);
      }
      else
      {
        PickUp pickUp = InventoryItem.Spawn(this.Reward, 1, this.transform.position + Vector3.back, 0.0f);
        if ((bool) (UnityEngine.Object) pickUp)
          pickUp.SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
        FoundItemPickUp component1 = pickUp.GetComponent<FoundItemPickUp>();
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        {
          component1.AutomaticallyInteract = true;
          if (flag8)
            component1.MagnetToPlayer();
        }
        Interaction_DoctrineStone component2 = pickUp.GetComponent<Interaction_DoctrineStone>();
        if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
          return;
        component2.MagnetToPlayer();
      }
    }
  }

  public void SpawnBlackSouls()
  {
    BlackSoul blackSoul = InventoryItem.SpawnBlackSoul(Mathf.RoundToInt((float) UnityEngine.Random.Range(8, 12) * TrinketManager.GetBlackSoulsMultiplier(this.playerFarming)), this.transform.position, simulated: true);
    if (!(bool) (UnityEngine.Object) blackSoul)
      return;
    blackSoul.SetAngle((float) UnityEngine.Random.Range(0, 360), new Vector2(2f, 4f));
  }

  public IEnumerator GiveRewardDelay(float Delay = 1.06666672f)
  {
    Interaction_Chest interactionChest = this;
    yield return (object) new WaitForSeconds(Delay);
    interactionChest.Spine.AnimationState.SetAnimation(0, "open", false);
    interactionChest.ChestOpenSfx();
    interactionChest.Spine.AnimationState.AddAnimation(0, "opened", true, 0.0f);
    if (interactionChest.pickedReward != RewardsItem.ChestRewards.DISSENTER && interactionChest.pickedReward != RewardsItem.ChestRewards.MISSIONARY)
      yield return (object) new WaitForSeconds(0.25f);
    interactionChest.RevealedGiveReward = false;
    int Quantity = 1;
    if (interactionChest.Reward == InventoryItem.ITEM_TYPE.GOLD_NUGGET)
      Quantity = UnityEngine.Random.Range(20, 30);
    if (InventoryItem.AllSeeds.Contains(interactionChest.Reward))
      Quantity = UnityEngine.Random.Range(3, 6);
    if (InventoryItem.IsFood(interactionChest.Reward))
      Quantity = UnityEngine.Random.Range(3, 6);
    if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && interactionChest.Reward == InventoryItem.ITEM_TYPE.BLACK_GOLD)
    {
      switch (PlayerFarming.Location)
      {
        case FollowerLocation.Dungeon1_5:
        case FollowerLocation.Dungeon1_6:
          Quantity = 4;
          break;
        default:
          Quantity = 3;
          break;
      }
      Quantity *= (int) DungeonModifier.HasPositiveModifier(DungeonPositiveModifier.DoubleGold, 2f, 1f);
      Quantity += interactionChest.ChestCoinMultiplier();
    }
    if (interactionChest.OverrideChestReward != RewardsItem.ChestRewards.NONE)
      Quantity = interactionChest.OverrideChestRewardQuantity;
    interactionChest.Reward = DungeonModifier.HasNeutralModifier(DungeonNeutralModifier.ChestsDropFoodNotGold) ? interactionChest.GetRandomFoodItem() : interactionChest.Reward;
    if (DungeonModifier.HasNeutralModifier(DungeonNeutralModifier.ChestsDropFoodNotGold))
      Quantity = UnityEngine.Random.Range(2, 4);
    if (RewardsItem.Instance.IsBiomeResource(interactionChest.Reward))
      Quantity = UnityEngine.Random.Range(1, 3) * BiomeGenerator.Instance.GoldToGive;
    if (!((UnityEngine.Object) interactionChest == (UnityEngine.Object) null) && !((UnityEngine.Object) interactionChest.gameObject == (UnityEngine.Object) null))
    {
      while (!interactionChest.gameObject.activeInHierarchy)
        yield return (object) null;
      if (DungeonSandboxManager.Active && interactionChest.Reward != InventoryItem.ITEM_TYPE.BLACK_GOLD && interactionChest.pickedReward != RewardsItem.ChestRewards.SPIDERS)
        interactionChest.Reward = InventoryItem.ITEM_TYPE.BLACK_GOLD;
      if (interactionChest.pickedReward != RewardsItem.ChestRewards.SPIDERS)
      {
        if (PlayerFarming.Location == FollowerLocation.Dungeon1_5 && DataManager.Instance.OnboardedLightningShardDungeon && (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD) < 3 || (double) UnityEngine.Random.value < 0.15000000596046448))
        {
          interactionChest.Reward = InventoryItem.ITEM_TYPE.LIGHTNING_SHARD;
          Quantity = UnityEngine.Random.Range(3, 7);
        }
        else if (PlayerFarming.Location == FollowerLocation.Dungeon1_6 && DataManager.Instance.CollectedYewMutated && (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.YEW_CURSED) < 3 || (double) UnityEngine.Random.value < 0.15000000596046448))
        {
          interactionChest.Reward = InventoryItem.ITEM_TYPE.YEW_CURSED;
          Quantity = UnityEngine.Random.Range(3, 7);
        }
        else if (DataManager.Instance.CollectedRotstone && (PlayerFarming.Location == FollowerLocation.Dungeon1_6 || UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Upgrade_Rotstone_Spread)) && (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.MAGMA_STONE) < 3 || (double) UnityEngine.Random.value < 0.15000000596046448))
        {
          interactionChest.Reward = InventoryItem.ITEM_TYPE.MAGMA_STONE;
          Quantity = UnityEngine.Random.Range(3, 7);
        }
      }
      if (interactionChest.Reward == InventoryItem.ITEM_TYPE.WOOL)
        Quantity = UnityEngine.Random.Range(4, 8);
      if (interactionChest.Reward != InventoryItem.ITEM_TYPE.NONE && interactionChest.pickedReward != RewardsItem.ChestRewards.DISSENTER && interactionChest.pickedReward != RewardsItem.ChestRewards.MISSIONARY)
      {
        for (int index = 0; index < Quantity; ++index)
        {
          PickUp pickUp = InventoryItem.Spawn(interactionChest.Reward, 1, interactionChest.transform.position + Vector3.back, 0.0f);
          if ((UnityEngine.Object) pickUp != (UnityEngine.Object) null)
          {
            pickUp.SetInitialSpeedAndDiraction(3f, (float) (270 + UnityEngine.Random.Range(-35, 35)));
            pickUp.MagnetDistance = 100f;
            pickUp.CanStopFollowingPlayer = false;
          }
        }
      }
      if (UnityEngine.Random.Range(1, 15) == 3 && interactionChest.pickedReward != RewardsItem.ChestRewards.DISSENTER && interactionChest.pickedReward != RewardsItem.ChestRewards.MISSIONARY)
        interactionChest.SpawnBlackSouls();
      if (interactionChest.pickedReward == RewardsItem.ChestRewards.DISSENTER)
        interactionChest.StartCoroutine((IEnumerator) interactionChest.SpawnDissentingFollowerIE());
      else if (interactionChest.pickedReward == RewardsItem.ChestRewards.MISSIONARY)
        interactionChest.StartCoroutine((IEnumerator) interactionChest.SpawnMissionaryFollowerIE());
      else if (interactionChest.pickedReward == RewardsItem.ChestRewards.SPIDERS && interactionChest.TypeOfChest == Interaction_Chest.ChestType.Wooden)
      {
        AudioManager.Instance.PlayOneShot("event:/Stings/generic_negative", interactionChest.transform.position);
        for (int index = 0; index < 10; ++index)
          ObjectPool.Spawn(interactionChest.spiderCritter, interactionChest.transform.position, Quaternion.identity, interactionChest.transform.parent, (System.Action<GameObject>) (obj =>
          {
            obj.GetComponent<UnitObject>().DoKnockBack((float) UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0.2f, 0.3f), 0.5f);
            obj.name = "Critter";
          }));
      }
      else
      {
        int typeOfChest = (int) interactionChest.TypeOfChest;
        if (interactionChest.TypeOfChest == Interaction_Chest.ChestType.Silver)
        {
          interactionChest.SpawnGoodReward(0.1f, 0.1f, 0.05f);
          interactionChest.SpawnBlackSouls();
        }
        if (interactionChest.TypeOfChest == Interaction_Chest.ChestType.Gold)
        {
          interactionChest.SpawnGoodReward();
          interactionChest.SpawnGoodReward(0.25f, 0.25f, 0.1f);
        }
        if ((UnityEngine.Object) interactionChest.healthPlayer != (UnityEngine.Object) null)
        {
          float num1 = (float) (((double) interactionChest.healthPlayer.totalHP - (double) interactionChest.healthPlayer.HP) / 10.0);
          if (PlayerFarming.playersCount > 1)
          {
            float num2 = 0.0f;
            foreach (PlayerFarming player in PlayerFarming.players)
              num2 += (float) (((double) interactionChest.healthPlayer.totalHP - (double) interactionChest.healthPlayer.HP) / 10.0);
            num1 = num2 / (float) PlayerFarming.playersCount * interactionChest.CoopChanceForHeartMultiplier;
          }
          float num3 = num1 * DifficultyManager.GetHealthDropsMultiplier() * (PlayerWeapon.FirstTimeUsingWeapon ? 1.2f : 1f);
          float num4 = UnityEngine.Random.Range(0.0f, 1f);
          float num5 = UnityEngine.Random.Range(0.0f, 1f);
          if ((double) num3 >= (double) num4)
          {
            interactionChest.Reward = (double) num5 >= 0.75 ? ((double) UnityEngine.Random.value < 0.5 ? InventoryItem.ITEM_TYPE.BLUE_HEART : InventoryItem.ITEM_TYPE.HALF_BLUE_HEART) : ((double) UnityEngine.Random.value < 0.5 ? InventoryItem.ITEM_TYPE.RED_HEART : InventoryItem.ITEM_TYPE.HALF_HEART);
            PickUp pickUp = InventoryItem.Spawn(interactionChest.Reward, 1, interactionChest.transform.position + Vector3.back, 0.0f);
            if ((UnityEngine.Object) pickUp != (UnityEngine.Object) null)
            {
              pickUp.SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
              pickUp.MagnetDistance = 2f;
              pickUp.CanStopFollowingPlayer = false;
            }
          }
        }
      }
      AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionChest.gameObject);
      Interaction_Chest.ChestEvent onChestRevealed = Interaction_Chest.OnChestRevealed;
      if (onChestRevealed != null)
        onChestRevealed();
    }
  }

  public IEnumerator SpawnDissentingFollowerIE()
  {
    Interaction_Chest interactionChest = this;
    FollowerInfo followerInfo = DataManager.Instance.Followers_Dissented[0];
    FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(FollowerManager.CombatFollowerPrefab, followerInfo, interactionChest.transform.position, interactionChest.transform.parent, BiomeGenerator.Instance.DungeonLocation);
    spawnedFollower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num1 = (double) spawnedFollower.Follower.SetBodyAnimation("spawn-in-base", false);
    spawnedFollower.Follower.Spine.transform.localPosition = new Vector3(0.2f, -0.2f, -1f);
    spawnedFollower.Follower.Spine.transform.DOLocalMove(Vector3.zero, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    spawnedFollower.Follower.Health.invincible = true;
    spawnedFollower.Follower.GetComponent<UnitObject>().CanHaveModifier = false;
    spawnedFollower.Follower.GetComponent<UnitObject>().RemoveModifier();
    spawnedFollower.Follower.OverridingEmotions = true;
    interactionChest.SetFollowerOutfit(spawnedFollower, Thought.Dissenter);
    FollowerBrain.RemoveBrain(spawnedFollower.FollowerBrain.Info.ID);
    FollowerBrain.RemoveBrain(spawnedFollower.FollowerFakeBrain.Info.ID);
    yield return (object) new WaitForSeconds(3.83333325f);
    double num2 = (double) spawnedFollower.Follower.SetBodyAnimation("Combat/idle", true);
    interactionChest.playerFarming.state.facingAngle = interactionChest.playerFarming.state.LookAngle = Utils.GetAngle(interactionChest.playerFarming.transform.position, spawnedFollower.Follower.transform.position);
    spawnedFollower.Follower.FacePosition(interactionChest.playerFarming.transform.position);
    int num3 = UnityEngine.Random.Range(0, 3);
    List<ConversationEntry> Entries = new List<ConversationEntry>()
    {
      new ConversationEntry(spawnedFollower.Follower.gameObject, $"Conversation_NPC/DissentingFollower/Dungeon_{num3}/0"),
      new ConversationEntry(spawnedFollower.Follower.gameObject, $"Conversation_NPC/DissentingFollower/Dungeon_{num3}/1")
    };
    foreach (ConversationEntry conversationEntry in Entries)
      conversationEntry.CharacterName = followerInfo.Name;
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false);
    yield return (object) null;
    while (MMConversation.isPlaying)
      yield return (object) null;
    spawnedFollower.Follower.transform.DOScale(1.5f, 1.46f);
    spawnedFollower.Follower.Spine.AnimationState.SetAnimation(1, "tantrum-big", false).TrackTime = 4.5f;
    CameraManager.instance.ShakeCameraForDuration(0.5f, 0.7f, 1.46f);
    yield return (object) new WaitForSeconds(1.56f);
    spawnedFollower.Follower.transform.localScale = Vector3.one * 1.5f;
    GameManager.GetInstance().OnConversationEnd();
    EnemyFollower component = spawnedFollower.Follower.GetComponent<EnemyFollower>();
    component.health.enabled = true;
    component.enabled = true;
    component.CanShoot = true;
    component.CanSpawnEnemies = true;
    component.maxSpeed *= 2f;
    EnemyModifier.ForceModifiers = true;
    component.ForceSetModifier(EnemyModifier.GetModifier(1f), false);
    component.health.HP = component.health.totalHP;
    component.health.invincible = false;
    EnemyModifier.ForceModifiers = false;
    spawnedFollower.Follower.Health.OnDie += (Health.DieAction) ((Attacker, AttackLocation, Victim, AttackType, AttackFlags) =>
    {
      DataManager.Instance.Followers_Dissented.Remove(followerInfo);
      DataManager.Instance.Followers_Dead.Insert(0, followerInfo);
      DataManager.Instance.Followers_Dead_IDs.Insert(0, followerInfo.ID);
      RoomLockController.RoomCompleted(true);
    });
  }

  public IEnumerator SpawnMissionaryFollowerIE()
  {
    Interaction_Chest interactionChest = this;
    DataManager.Instance.TimeSinceLastMissionaryFollowerEncounter = TimeManager.TotalElapsedGameTime + (float) UnityEngine.Random.Range(3600, 7200);
    int index = UnityEngine.Random.Range(0, DataManager.Instance.Followers_OnMissionary_IDs.Count);
    if (DataManager.Instance.Followers_OnMissionary_IDs.Count == 0)
    {
      GameManager.GetInstance().OnConversationEnd();
    }
    else
    {
      FollowerInfo followerInfo = FollowerInfo.GetInfoByID(DataManager.Instance.Followers_OnMissionary_IDs[index]);
      FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(FollowerManager.CombatFollowerPrefab, followerInfo, interactionChest.transform.position, interactionChest.transform.parent, BiomeGenerator.Instance.DungeonLocation);
      if (spawnedFollower != null)
      {
        spawnedFollower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
        double num1 = (double) spawnedFollower.Follower.SetBodyAnimation("spawn-in-base", false);
        spawnedFollower.Follower.Spine.transform.localPosition = new Vector3(0.2f, -0.2f, -1f);
        spawnedFollower.Follower.Spine.transform.DOLocalMove(Vector3.zero, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
        spawnedFollower.Follower.Health.enabled = false;
        spawnedFollower.Follower.Health.team = Health.Team.PlayerTeam;
        Health.team2.Remove(spawnedFollower.Follower.Health);
        spawnedFollower.Follower.GetComponent<UnitObject>().CanHaveModifier = false;
        spawnedFollower.Follower.GetComponent<UnitObject>().RemoveModifier();
        List<Health> healthList = new List<Health>((IEnumerable<Health>) Health.team2);
        healthList.AddRange((IEnumerable<Health>) Health.killAll);
        foreach (Health health in healthList)
        {
          if ((UnityEngine.Object) health != (UnityEngine.Object) null && (UnityEngine.Object) health != (UnityEngine.Object) spawnedFollower.Follower.Health)
          {
            health.invincible = false;
            health.enabled = true;
            if ((double) health.HP > 0.0)
              health.DealDamage(float.PositiveInfinity, health.gameObject, Vector3.zero, AttackType: Health.AttackTypes.Projectile);
          }
        }
        DungeonMissionaryFollower dungeonMissionary = spawnedFollower.Follower.gameObject.AddComponent<DungeonMissionaryFollower>();
        if ((UnityEngine.Object) dungeonMissionary != (UnityEngine.Object) null)
        {
          dungeonMissionary.Type = DungeonMissionaryFollower.HiddenType.HiddenInChest;
          dungeonMissionary.IsActivated = true;
          spawnedFollower.Follower.OverridingEmotions = true;
          spawnedFollower.Follower.Brain.Info.Outfit = FollowerOutfitType.Sherpa;
          spawnedFollower.Follower.SetOutfit(FollowerOutfitType.Sherpa, false);
          FollowerBrain.RemoveBrain(spawnedFollower.FollowerFakeBrain.Info.ID);
          GameManager.GetInstance().OnConversationNew();
          GameManager.GetInstance().OnConversationNext(spawnedFollower.Follower.gameObject);
          yield return (object) new WaitForSeconds(3.83333325f);
          interactionChest.playerFarming.state.facingAngle = interactionChest.playerFarming.state.LookAngle = Utils.GetAngle(interactionChest.playerFarming.transform.position, spawnedFollower.Follower.transform.position);
          spawnedFollower.Follower.FacePosition(interactionChest.playerFarming.transform.position);
          string str;
          if ((double) UnityEngine.Random.value < 0.5)
          {
            double num2 = (double) spawnedFollower.Follower.SetBodyAnimation("idle", true);
            spawnedFollower.Follower.Brain.CurrentState = (FollowerState) new FollowerState_Default();
            str = "Normal";
          }
          else
          {
            double num3 = (double) spawnedFollower.Follower.SetBodyAnimation("Injured/idle", true);
            spawnedFollower.Follower.Brain.CurrentState = (FollowerState) new FollowerState_Injured();
            str = "Injured";
          }
          spawnedFollower.Follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, spawnedFollower.Follower.Brain.CurrentState.OverrideIdleAnim);
          spawnedFollower.Follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, spawnedFollower.Follower.Brain.CurrentState.OverrideWalkAnim == null ? "run" : spawnedFollower.Follower.Brain.CurrentState.OverrideWalkAnim);
          List<ConversationEntry> Entries = new List<ConversationEntry>()
          {
            new ConversationEntry(spawnedFollower.Follower.gameObject, $"Conversation_NPC/MissionaryTrapped/{str}/0"),
            new ConversationEntry(spawnedFollower.Follower.gameObject, $"Conversation_NPC/MissionaryTrapped/{str}/1")
          };
          foreach (ConversationEntry conversationEntry in Entries)
          {
            conversationEntry.CharacterName = followerInfo.Name;
            conversationEntry.Speaker = spawnedFollower.Follower.gameObject;
          }
          MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false);
          yield return (object) null;
          while (MMConversation.isPlaying)
            yield return (object) null;
          RoomLockController.RoomCompleted(true);
          GameManager.GetInstance().OnConversationEnd();
          yield return (object) new WaitForSeconds(1f);
          dungeonMissionary.GoToRandomEntrance();
        }
        else
          GameManager.GetInstance().OnConversationEnd();
      }
      else
        GameManager.GetInstance().OnConversationEnd();
    }
  }

  public IEnumerator SpawnGivenUpWolfFood()
  {
    Interaction_Chest interactionChest = this;
    List<InventoryItem> unqiueFoodItems = new List<InventoryItem>();
    for (int index1 = 0; index1 < DataManager.Instance.GivenUpWolfFood.Count; ++index1)
    {
      bool flag = true;
      for (int index2 = 0; index2 < unqiueFoodItems.Count; ++index2)
      {
        if (unqiueFoodItems[index2].type == DataManager.Instance.GivenUpWolfFood[index1].type)
        {
          flag = false;
          unqiueFoodItems[index2].quantity += DataManager.Instance.GivenUpWolfFood[index1].quantity;
          break;
        }
      }
      if (flag)
        unqiueFoodItems.Add(new InventoryItem(DataManager.Instance.GivenUpWolfFood[index1]));
    }
    for (int x = 0; x < unqiueFoodItems.Count; ++x)
    {
      int foodToSpawn = Mathf.Min(5, unqiueFoodItems[x].quantity);
      if (foodToSpawn != 0)
      {
        int baseAmount = unqiueFoodItems[x].quantity / foodToSpawn;
        int remainder = unqiueFoodItems[x].quantity % foodToSpawn;
        for (int y = 0; y < foodToSpawn; ++y)
        {
          PickUp pickUp = InventoryItem.Spawn((InventoryItem.ITEM_TYPE) unqiueFoodItems[x].type, 1, interactionChest.transform.position);
          pickUp.Quantity = baseAmount;
          if (y == 0)
            pickUp.Quantity += remainder;
          yield return (object) new WaitForSeconds(0.25f / (float) foodToSpawn);
        }
      }
    }
    DataManager.Instance.GivenUpWolfFood.Clear();
  }

  public void SetFollowerOutfit(
    FollowerManager.SpawnedFollower spawnedFollower,
    Thought cursedState)
  {
    FollowerBrain.SetFollowerCostume(spawnedFollower.Follower.Spine.Skeleton, 0, spawnedFollower.FollowerBrain._directInfoAccess.SkinName, spawnedFollower.FollowerBrain._directInfoAccess.SkinColour, FollowerOutfitType.Dissenter, FollowerHatType.None, spawnedFollower.FollowerBrain.Info.Clothing, spawnedFollower.FollowerBrain.Info.Customisation, spawnedFollower.FollowerBrain.Info.Special, spawnedFollower.FollowerBrain.Info.Necklace, spawnedFollower.FollowerBrain.Info.ClothingVariant, spawnedFollower.FollowerBrain._directInfoAccess);
    spawnedFollower.Follower.Spine.AnimationState.SetAnimation(0, "Emotions/emotion-dissenter", true);
  }

  public GameObject WeaponPodiumPrefab
  {
    get
    {
      if ((UnityEngine.Object) this._weaponPodiumPrefab == (UnityEngine.Object) null)
      {
        this.weaponPodium_addressableHandle = Addressables.LoadAssetAsync<GameObject>((object) this.Addr_WeaponPodiumPrefab);
        this.weaponPodium_addressableHandle.WaitForCompletion();
        this._weaponPodiumPrefab = this.weaponPodium_addressableHandle.Result;
      }
      return this._weaponPodiumPrefab;
    }
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.CleanUpAddressables();
  }

  public void CleanUpAddressables()
  {
    this._weaponPodiumPrefab = (GameObject) null;
    if (!this.weaponPodium_addressableHandle.IsValid())
      return;
    Addressables.Release<GameObject>(this.weaponPodium_addressableHandle);
  }

  public void OnSpawnedDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    Victim.OnDie -= new Health.DieAction(this.OnSpawnedDie);
    ++this.DeathCount;
  }

  public void ReviveSpawned(Health Victim)
  {
    --this.DeathCount;
    this.DeathCount = Math.Max(0, this.DeathCount);
    Victim.OnDie += new Health.DieAction(this.OnSpawnedDie);
  }

  public override void Update()
  {
    base.Update();
    if (this.BossChest)
      return;
    if (this.MyState == Interaction_Chest.State.Hidden && this.Enemies.Count > 0)
    {
      if (this.DeathCount >= this.Enemies.Count)
      {
        if ((double) this.Delay != 0.0 && (double) this.delayTimestamp == -1.0)
          this.delayTimestamp = GameManager.GetInstance().CurrentTime + this.Delay;
        if ((double) this.Delay == 0.0 || (double) GameManager.GetInstance().CurrentTime > (double) this.delayTimestamp)
        {
          foreach (UnityEngine.Object enemy in this.Enemies)
          {
            if (enemy != (UnityEngine.Object) null)
              return;
          }
          bool flag = DataManager.Instance.BossesCompleted.Count >= 4 || DataManager.Instance.QuickStartActive;
          if (((!(bool) (UnityEngine.Object) this.GetComponentInParent<EnemyOnboarding>() || DungeonSandboxManager.Active || DataManager.Instance.Followers_Dissented.Count <= 0 ? 0 : ((double) UnityEngine.Random.value < 0.029999999329447746 ? 1 : 0)) & (flag ? 1 : 0)) != 0 && DataManager.Instance.Followers_Possessed.Count <= 0 && !BiomeGenerator.Instance.CurrentRoom.HasWeapon && !BiomeGenerator.Instance.OverrideRandomWalk)
          {
            this.pickedReward = RewardsItem.ChestRewards.DISSENTER;
            GameManager.GetInstance().WaitForSeconds(0.5f, (System.Action) (() =>
            {
              GameManager.GetInstance().OnConversationNew();
              GameManager.GetInstance().OnConversationNext(this.gameObject, 6f);
            }));
          }
          else if (!DungeonSandboxManager.Active && DataManager.Instance.Followers_OnMissionary_IDs.Count > 0 && (double) UnityEngine.Random.value < 0.039999999105930328 && DataManager.Instance.Followers_Possessed.Count <= 0 && !BiomeGenerator.Instance.CurrentRoom.HasWeapon && (double) TimeManager.TotalElapsedGameTime > (double) DataManager.Instance.TimeSinceLastMissionaryFollowerEncounter)
          {
            this.pickedReward = RewardsItem.ChestRewards.MISSIONARY;
            GameManager.GetInstance().WaitForSeconds(0.5f, (System.Action) (() =>
            {
              GameManager.GetInstance().OnConversationNew();
              GameManager.GetInstance().OnConversationNext(this.gameObject, 6f);
            }));
          }
          AudioManager.Instance.SetMusicCombatState(false);
          this.Reveal();
          if (this.pickedReward != RewardsItem.ChestRewards.DISSENTER && this.pickedReward != RewardsItem.ChestRewards.MISSIONARY)
            RoomLockController.RoomCompleted(true);
          GameManager.GetInstance().CamFollowTarget.MaxZoom = this.CacheCameraMaxZoom;
          GameManager.GetInstance().CamFollowTarget.MinZoom = this.CacheCameraMinZoom;
          GameManager.GetInstance().CamFollowTarget.ZoomLimiter = this.CacheCameraZoomLimiter;
          GameManager.GetInstance().RemoveFromCamera(this.gameObject);
        }
      }
      else
        this.delayTimestamp = -1f;
      if (this.DeathCount < this.Enemies.Count)
      {
        foreach (Health enemy in this.Enemies)
        {
          if ((UnityEngine.Object) enemy != (UnityEngine.Object) null && enemy.state.CURRENT_STATE != StateMachine.State.Idle)
          {
            AudioManager.Instance.SetMusicCombatState();
            break;
          }
        }
      }
    }
    if (this.MyState != Interaction_Chest.State.Closed)
      return;
    this.Timer += Time.deltaTime;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = ScriptLocalization.Interactions.OpenChest;
  }

  public override void GetLabel()
  {
    this.Label = this.MyState != Interaction_Chest.State.Closed || (double) this.Timer <= 0.5 ? "" : this.sLabel;
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.MyState != Interaction_Chest.State.Closed)
      return;
    base.OnInteract(state);
    this.SelectReward();
    this.MyState = Interaction_Chest.State.Open;
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.GiveRewardDelay(UnityEngine.Random.Range(0.0f, 0.3f)));
  }

  public void SelectReward()
  {
    if (this.pickedReward == RewardsItem.ChestRewards.DISSENTER || this.pickedReward == RewardsItem.ChestRewards.MISSIONARY)
      return;
    this.healthPlayer = (HealthPlayer) null;
    if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null)
      this.healthPlayer = this.playerFarming.GetComponent<HealthPlayer>();
    if (++DataManager.Instance.ChestRewardCount >= 3)
      DataManager.Instance.ChestRewardCount = 0;
    List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>();
    DataManager.Instance.ChestRewardCount = 0;
    if (this.ChestRewards.Count <= 0)
      return;
    this.randomReward = (float) UnityEngine.Random.Range(0, 100);
    bool flag = false;
    foreach (RewardsItem chestReward in this.ChestRewards)
    {
      if (!flag)
      {
        if ((double) this.randomReward >= (double) this.previousChance && (double) this.randomReward <= (double) chestReward.SpawnNumber)
        {
          this.previousChance = chestReward.SpawnNumber;
          this.pickedReward = chestReward.chestReward;
          break;
        }
      }
      else
        break;
    }
    this.UpdateChestRewards();
    if (this.OverrideChestReward != RewardsItem.ChestRewards.NONE)
      this.pickedReward = this.OverrideChestReward;
    this.Reward = !this.ForceGoodReward ? RewardsItem.Instance.ReturnItemType(this.pickedReward) : RewardsItem.Instance.ReturnItemType(RewardsItem.Instance.GetGoodReward());
    if (PlayerFarming.Location != FollowerLocation.IntroDungeon)
      return;
    Debug.Log((object) "Chest: Just spawn gold in intro");
    this.Reward = InventoryItem.ITEM_TYPE.BLACK_GOLD;
  }

  public void BackToIdle()
  {
    Debug.Log((object) "BACKL TO IDLE!");
    this.Loop = false;
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = false;
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().CameraResetTargetZoom();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.StartCoroutine((IEnumerator) this.DelayEffectsRoutine(this.playerFarming));
  }

  public IEnumerator DelayEffectsRoutine(PlayerFarming playerFarming)
  {
    yield return (object) new WaitForSeconds(0.2f);
    TrinketManager.AddTrinket(this.DrawnCard, playerFarming);
  }

  public void CloseMenuCallback() => this.Loop = false;

  public override void OnDrawGizmos()
  {
    base.OnDrawGizmos();
    foreach (Health enemy in this.Enemies)
    {
      if ((UnityEngine.Object) enemy != (UnityEngine.Object) null)
        Utils.DrawLine(this.transform.position, enemy.transform.position, Color.yellow);
    }
  }

  public void UpdateChestRewards()
  {
    this.totalProbability = 0.0f;
    this.previousRewardChance = 0.0f;
    foreach (RewardsItem chestReward in this.ChestRewards)
      this.totalProbability += chestReward.ChanceToSpawn;
    foreach (RewardsItem chestReward in this.ChestRewards)
    {
      this.multiplyer = 100f / this.totalProbability;
      chestReward.probabilityChance = chestReward.ChanceToSpawn * this.multiplyer;
      chestReward.SpawnNumber = chestReward.probabilityChance + this.previousRewardChance;
      this.previousRewardChance = chestReward.SpawnNumber;
    }
  }

  [CompilerGenerated]
  public void \u003CAwake\u003Eb__46_0()
  {
    ChestLoader componentInChildren = this.GetComponentInChildren<ChestLoader>(true);
    this.CameraBone = componentInChildren.CameraBone;
    this.DamageCollider = componentInChildren.DamageCollider;
    this.Shadow = componentInChildren.Shadow;
    this.Lighting = componentInChildren.Lighting;
    this.Item = componentInChildren.Item;
    this.PlayerPosition = componentInChildren.PlayerPosition;
    this.Spine = componentInChildren.Spine;
    this.RevealedGiveReward = false;
    this.Enemies.Clear();
    this.Item.gameObject.SetActive(false);
    this.UpdateLocalisation();
    if (this.MyState == Interaction_Chest.State.Hidden)
      this.Spine.AnimationState.SetAnimation(0, "hidden", true);
    else
      this.Spine.AnimationState.SetAnimation(0, "closed", true);
    this.SetTypeOfChest();
    this.Shadow.SetActive(false);
    this.Lighting.SetActive(false);
    if ((UnityEngine.Object) this.transform.parent != (UnityEngine.Object) null)
      this.StealthCovers = this.transform.parent.GetComponentsInChildren<StealthCover>();
    if (!this.RevealOnDistance)
      return;
    this.AutomaticallyInteract = true;
    this.ActivateDistance = 3f;
    this.Spine.AnimationState.SetAnimation(0, "reveal", true);
    this.RevealSfx();
    this.Spine.AnimationState.AddAnimation(0, "closed", true, 0.0f);
    this.MyState = Interaction_Chest.State.Closed;
    this.StartCoroutine((IEnumerator) this.ShowShadow());
  }

  [CompilerGenerated]
  public bool \u003CInitializeAsync\u003Eb__47_0() => this.loader.isInitialized;

  [CompilerGenerated]
  public void \u003COnEnableInteraction\u003Eb__48_0()
  {
    base.OnEnableInteraction();
    if (!this.active)
    {
      if (!this.BossChest)
        this.StartCoroutine((IEnumerator) this.DelayGetEnemies());
      if ((UnityEngine.Object) this.DamageCollider != (UnityEngine.Object) null)
      {
        this.DamageCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
        this.DamageCollider.SetActive(false);
      }
    }
    Interaction_Chest.Instance = this;
    this.StartCoroutine((IEnumerator) this.EnableInteractionDelay());
    this.active = true;
    if (this.Enemies.Count > 0)
      return;
    this.StartCoroutine((IEnumerator) this.DelayGetEnemies());
  }

  [CompilerGenerated]
  public void \u003COnEnable\u003Eb__54_0()
  {
    base.OnEnable();
    if (!this.StartRevealed || this.givenReward)
      return;
    this.StartCoroutine((IEnumerator) this.DelayReveal());
  }

  [CompilerGenerated]
  public void \u003CReveal\u003Eb__65_0()
  {
    if (!DataManager.Instance.ShownInventoryTutorial)
    {
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(this.playerFarming.gameObject);
      this.playerFarming._state.facingAngle = Utils.GetAngle(this.playerFarming.transform.position, this.gameObject.transform.position);
      CameraFollowTarget cameraFollowTarget = CameraFollowTarget.Instance;
      cameraFollowTarget.SetOffset((this.gameObject.transform.position - this.playerFarming.transform.position) * 0.85f);
      HUD_Manager.Instance.Hide(false, 0);
      this.StartCoroutine((IEnumerator) this.DelayCallback(2f, (System.Action) (() => cameraFollowTarget.SetOffset(Vector3.zero))));
    }
    Debug.Log((object) "Reveal()");
    this.RevealedGiveReward = true;
    this.StartCoroutine((IEnumerator) this.DamageColliderRoutine());
    if (!this.BlockWeapons && (UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && BiomeGenerator.Instance.CurrentRoom.HasWeapon && !BiomeGenerator.Instance.OnboardingDungeon5)
    {
      Debug.Log((object) "CHEST: Weapon!".Colour(Color.red));
      if (DataManager.Instance.WeaponPool.Count > 2 && DataManager.Instance.CursePool.Count > 2)
      {
        UnityEngine.Object.Instantiate<GameObject>(this.WeaponPodiumPrefab, this.transform.position - new Vector3(-1f, 0.0f), Quaternion.identity, this.transform.parent);
        UnityEngine.Object.Instantiate<GameObject>(this.WeaponPodiumPrefab, this.transform.position - new Vector3(1f, 0.0f), Quaternion.identity, this.transform.parent);
      }
      else
        UnityEngine.Object.Instantiate<GameObject>(this.WeaponPodiumPrefab, this.transform.position - new Vector3(0.0f, 0.0f), Quaternion.identity, this.transform.parent);
      Interaction_Chest.ChestEvent onChestRevealed = Interaction_Chest.OnChestRevealed;
      if (onChestRevealed != null)
        onChestRevealed();
      this.MyState = Interaction_Chest.State.Open;
    }
    else
    {
      Debug.Log((object) "CHEST: No weapon".Colour(Color.red));
      float num = 0.04f * TrinketManager.GetChanceForRelicsMultiplier(this.playerFarming);
      if (this.playerFarming.currentRelicType == RelicType.None)
        num += 0.04f;
      foreach (UnityEngine.Object player in PlayerFarming.players)
      {
        if (player != (UnityEngine.Object) this.playerFarming)
          num -= 0.06f;
      }
      if (!BiomeGenerator.Instance.HasSpawnedRelic && (double) UnityEngine.Random.value < (double) num && DataManager.Instance.OnboardedRelics && this.pickedReward != RewardsItem.ChestRewards.DISSENTER && this.pickedReward != RewardsItem.ChestRewards.MISSIONARY && !BiomeGenerator.Instance.OnboardingDungeon5)
      {
        Debug.Log((object) "CHEST: Relic!".Colour(Color.red));
        UnityEngine.Object.Instantiate<GameObject>(this.WeaponPodiumPrefab, this.transform.position - new Vector3(0.0f, 0.0f), Quaternion.identity, this.transform.parent).GetComponent<Interaction_WeaponChoiceChest>().Type = Interaction_WeaponSelectionPodium.Types.Relic;
        BiomeGenerator.Instance.HasSpawnedRelic = true;
        Interaction_Chest.ChestEvent onChestRevealed = Interaction_Chest.OnChestRevealed;
        if (onChestRevealed != null)
          onChestRevealed();
        this.MyState = Interaction_Chest.State.Open;
        this.Spine.gameObject.SetActive(false);
      }
      else
      {
        Debug.Log((object) "CHEST: No Relic".Colour(Color.red));
        this.Spine.AnimationState.SetAnimation(0, "reveal", true);
        this.RevealSfx();
        this.Spine.AnimationState.AddAnimation(0, "closed", true, 0.0f);
        this.MyState = Interaction_Chest.State.Closed;
        this.StartCoroutine((IEnumerator) this.ShowShadow());
        this.SelectReward();
        this.MyState = Interaction_Chest.State.Open;
        GameManager.GetInstance().StartCoroutine((IEnumerator) this.GiveRewardDelay());
        Projectile.ClearProjectiles();
      }
    }
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__103_0()
  {
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.gameObject, 6f);
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__103_1()
  {
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.gameObject, 6f);
  }

  public enum ChestType
  {
    None,
    Wooden,
    Silver,
    Gold,
  }

  public enum State
  {
    Hidden,
    Closed,
    Open,
  }

  public delegate void ChestEvent();
}
