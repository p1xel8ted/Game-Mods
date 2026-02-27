// Decompiled with JetBrains decompiler
// Type: Interaction_Chest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMBiomeGeneration;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
  private string sLabel;
  private float Timer;
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
  private float delayTimestamp = -1f;
  private bool active;
  public static Interaction_Chest.ChestEvent OnChestRevealed;
  [Space]
  [SerializeField]
  private string spiderCritter;
  private int SilverChestMaxRandom = 12;
  private int GoldChestMaxRandom = 10;
  private Health EnemyHealth;
  private bool givenReward;
  private float CacheCameraMaxZoom;
  private float CacheCameraMinZoom;
  private float CacheCameraZoomLimiter;
  public bool RevealedGiveReward;
  private FoundItemPickUp p;
  private float coinMultiplier = 1f;
  private InventoryItem.ITEM_TYPE previousPick;
  private InventoryItem.ITEM_TYPE FirstGoldReward;
  public GameObject WeaponPodiumPrefab;
  public int DeathCount;
  private bool InCombat;
  private InventoryItem.ITEM_TYPE Reward;
  private HealthPlayer healthPlayer;
  public static int RevealCount = -1;
  private float randomReward;
  private RewardsItem.ChestRewards pickedReward;
  private float previousChance;
  private bool Loop;
  private TarotCards.TarotCard DrawnCard;
  private float totalProbability;
  private float previousTotal;
  private float multiplyer;
  private float previousRewardChance;

  public Interaction_Chest.State MyState { get; private set; }

  public float Delay { get; set; }

  private void SetTypeOfChest()
  {
    if (this.TypeOfChest == Interaction_Chest.ChestType.None)
    {
      if (!this.BossChest)
      {
        if (TrinketManager.HasTrinket(TarotCards.Card.RabbitFoot))
        {
          this.SilverChestMaxRandom = 7;
          this.GoldChestMaxRandom = 10;
        }
        else
        {
          this.SilverChestMaxRandom = 15;
          this.GoldChestMaxRandom = 30;
        }
        this.TypeOfChest = UnityEngine.Random.Range(0, this.SilverChestMaxRandom) != 5 || DataManager.Instance.dungeonRun <= 2 ? (UnityEngine.Random.Range(0, this.GoldChestMaxRandom) != 5 || DataManager.Instance.dungeonRun <= 2 ? Interaction_Chest.ChestType.Wooden : Interaction_Chest.ChestType.Gold) : Interaction_Chest.ChestType.Silver;
      }
      else
        this.TypeOfChest = Interaction_Chest.ChestType.Gold;
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

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    if (!this.active)
    {
      if (!this.BossChest)
        this.StartCoroutine((IEnumerator) this.DelayGetEnemies());
      this.DamageCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.DamageCollider.SetActive(false);
    }
    Interaction_Chest.Instance = this;
    this.StartCoroutine((IEnumerator) this.EnableInteractionDelay());
    this.active = true;
  }

  private IEnumerator EnableInteractionDelay()
  {
    Interaction_Chest interactionChest = this;
    yield return (object) new WaitForEndOfFrame();
    if ((UnityEngine.Object) interactionChest.Spine != (UnityEngine.Object) null)
      interactionChest.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(interactionChest.HandleEvent);
  }

  private void OnDamageTriggerEnter(Collider2D collider)
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

  private void Start()
  {
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

  protected override void OnEnable()
  {
    base.OnEnable();
    if (!this.StartRevealed || this.givenReward)
      return;
    this.StartCoroutine((IEnumerator) this.DelayReveal());
  }

  private IEnumerator DelayReveal()
  {
    Interaction_Chest interactionChest = this;
    yield return (object) new WaitForEndOfFrame();
    if (interactionChest.gameObject.activeInHierarchy)
    {
      interactionChest.givenReward = true;
      interactionChest.Reveal();
    }
  }

  private IEnumerator DelayGetEnemies()
  {
    yield return (object) new WaitForEndOfFrame();
    this.GetEnemies();
  }

  public void GetEnemies()
  {
    foreach (Health health in new List<Health>((IEnumerable<Health>) this.transform.parent.GetComponentsInChildren<Health>()))
    {
      if (health.team == Health.Team.Team2 && !health.InanimateObject)
      {
        this.Enemies.Add(health);
        health.OnDie += new Health.DieAction(this.OnSpawnedDie);
      }
    }
    BiomeGenerator.OnRoomActive += new BiomeGenerator.BiomeAction(this.OnRoomActivate);
  }

  private void OnRoomActivate()
  {
    if (this.Enemies.Count <= 0)
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
    this.Enemies.Add(h);
    if (!(bool) (UnityEngine.Object) h)
      return;
    h.OnDie += new Health.DieAction(this.OnSpawnedDie);
  }

  private void QueryRoomWeapon()
  {
    Debug.Log((object) ("BiomeGenerator.Instance.CurrentRoom.HasWeapon: " + BiomeGenerator.Instance.CurrentRoom.HasWeapon.ToString()));
  }

  public void Reveal()
  {
    if (!DataManager.Instance.ShownInventoryTutorial)
    {
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject);
      PlayerFarming.Instance._state.facingAngle = Utils.GetAngle(PlayerFarming.Instance.transform.position, this.gameObject.transform.position);
      CameraFollowTarget cameraFollowTarget = CameraFollowTarget.Instance;
      cameraFollowTarget.SetOffset((this.gameObject.transform.position - PlayerFarming.Instance.transform.position) * 0.85f);
      HUD_Manager.Instance.Hide(false, 0);
      this.StartCoroutine((IEnumerator) this.DelayCallback(2f, (System.Action) (() => cameraFollowTarget.SetOffset(Vector3.zero))));
    }
    Debug.Log((object) "Reveal()");
    this.RevealedGiveReward = true;
    this.StartCoroutine((IEnumerator) this.DamageColliderRoutine());
    if (!this.BlockWeapons && (UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && BiomeGenerator.Instance.CurrentRoom.HasWeapon)
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

  private IEnumerator DelayCallback(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  private IEnumerator ShowShadow()
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

  public void RevealBossReward(InventoryItem.ITEM_TYPE ForceItem)
  {
    this.StartCoroutine((IEnumerator) this.DamageColliderRoutine());
    this.StartCoroutine((IEnumerator) this.GiveBossReward(ForceItem));
  }

  private void RevealSfx()
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

  private void ChestLandSfx()
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

  private void ChestOpenSfx()
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

  private IEnumerator GiveBossReward(InventoryItem.ITEM_TYPE ForceIncludeItem)
  {
    Interaction_Chest interactionChest = this;
    interactionChest.RevealedGiveReward = false;
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
      }
      else
      {
        List<RewardsItem.ChestRewards> chestRewardsList = new List<RewardsItem.ChestRewards>();
        if (DataManager.CheckIfThereAreSkinsAvailable())
          chestRewardsList.Add(RewardsItem.ChestRewards.FOLLOWER_SKIN);
        if (DataManager.GetDecorationsAvailableCategory(PlayerFarming.Location))
          chestRewardsList.Add(RewardsItem.ChestRewards.BASE_DECORATION);
        PickUp pickUp1 = (PickUp) null;
        if (chestRewardsList.Count > 0)
        {
          interactionChest.Reward = RewardsItem.Instance.ReturnItemType(chestRewardsList[UnityEngine.Random.Range(0, chestRewardsList.Count)]);
          interactionChest.p = InventoryItem.Spawn(interactionChest.Reward, 1, interactionChest.transform.position + Vector3.back, 0.0f).GetComponent<FoundItemPickUp>();
          if ((UnityEngine.Object) interactionChest.p != (UnityEngine.Object) null)
            pickUp1 = interactionChest.p.GetComponent<PickUp>();
          if ((UnityEngine.Object) interactionChest.p != (UnityEngine.Object) null && (UnityEngine.Object) pickUp1 != (UnityEngine.Object) null)
            interactionChest.p.GetComponent<PickUp>().SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
          FoundItemPickUp component = pickUp1.GetComponent<FoundItemPickUp>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            component.AutomaticallyInteract = true;
        }
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
            break;
          case FollowerLocation.Dungeon1_4:
            if (!StructuresData.GetUnlocked(StructureBrain.TYPES.DECORATION_BOSS_TROPHY_4))
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
            break;
        }
        if ((UnityEngine.Object) interactionChest.p != (UnityEngine.Object) null)
          interactionChest.p.GetComponent<PickUp>().SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
        Rewards = UnityEngine.Random.Range(25, 50) * (int) DungeonModifier.HasPositiveModifier(DungeonPositiveModifier.DoubleGold, 2f, 1f);
        if (DoctrineUpgradeSystem.TrySermonsStillAvailable() && DoctrineUpgradeSystem.TryGetStillDoctrineStone() && DataManager.Instance.GetVariable(DataManager.Variables.FirstDoctrineStone))
        {
          ForceIncludeItem = InventoryItem.ITEM_TYPE.DOCTRINE_STONE;
          PickUp pickUp2 = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.DOCTRINE_STONE, 1, interactionChest.transform.position);
          if ((UnityEngine.Object) pickUp2 != (UnityEngine.Object) null)
          {
            pickUp2.SetInitialSpeedAndDiraction(5f, 250f);
            AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionChest.gameObject);
            Interaction_DoctrineStone component = pickUp2.GetComponent<Interaction_DoctrineStone>();
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
      if (DataManager.Instance.BonesEnabled)
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
      if (ForceIncludeItem == InventoryItem.ITEM_TYPE.BEHOLDER_EYE)
      {
        while ((UnityEngine.Object) BeholderEye.Instance != (UnityEngine.Object) null)
          yield return (object) null;
      }
      if (ForceIncludeItem == InventoryItem.ITEM_TYPE.DOCTRINE_STONE)
      {
        while (Interaction_DoctrineStone.DoctrineStones.Count > 0)
          yield return (object) null;
      }
      while ((UnityEngine.Object) interactionChest.p != (UnityEngine.Object) null)
        yield return (object) null;
      RoomLockController.RoomCompleted(true);
      interactionChest.RevealedGiveReward = true;
    }
  }

  private InventoryItem.ITEM_TYPE GetRandomFoodItem()
  {
    return PlayerFarming.Location == FollowerLocation.Dungeon1_1 ? CookingData.GetLowQualityFoods()[UnityEngine.Random.Range(0, CookingData.GetLowQualityFoods().Length)] : CookingData.GetMediumQualityFoods()[UnityEngine.Random.Range(0, CookingData.GetMediumQualityFoods().Length)];
  }

  private IEnumerator GiveFullHeal()
  {
    Interaction_Chest interactionChest = this;
    int i = -1;
    while ((double) ++i < (double) PlayerFarming.Instance.health.totalHP / 2.0)
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

  private IEnumerator GiveBlackSouls()
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

  private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == this.ChestLand))
      return;
    this.ChestLandSfx();
    CameraManager.shakeCamera(0.5f);
    if (!((UnityEngine.Object) this.Spine != (UnityEngine.Object) null))
      return;
    this.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  private IEnumerator DamageColliderRoutine()
  {
    if ((UnityEngine.Object) this.DamageCollider != (UnityEngine.Object) null)
      this.DamageCollider.SetActive(true);
    yield return (object) new WaitForSeconds(0.1f);
    if ((UnityEngine.Object) this.DamageCollider != (UnityEngine.Object) null)
      this.DamageCollider.SetActive(false);
  }

  private int ChestCoinMultiplier()
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

  private void SpawnGoodReward()
  {
    AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", this.gameObject);
    this.Reward = RewardsItem.Instance.ReturnItemType(RewardsItem.Instance.GetGoodReward());
    int num1 = 100;
    while (this.Reward == this.previousPick && --num1 > 0)
      this.Reward = RewardsItem.Instance.ReturnItemType(RewardsItem.Instance.GetGoodReward());
    this.previousPick = this.Reward;
    if (this.FirstGoldReward != InventoryItem.ITEM_TYPE.NONE)
      this.FirstGoldReward = this.Reward;
    else if (InventoryItem.IsGiftOrNecklace(this.FirstGoldReward) && InventoryItem.IsGiftOrNecklace(this.Reward))
      this.Reward = InventoryItem.ITEM_TYPE.BLACK_GOLD;
    if (this.Reward == InventoryItem.ITEM_TYPE.GOLD_NUGGET)
    {
      int num2 = UnityEngine.Random.Range(20, 30);
      for (int index = 0; index < num2; ++index)
        InventoryItem.Spawn(this.Reward, 1, this.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(5f, (float) UnityEngine.Random.Range(150, 350));
    }
    else if (this.Reward == InventoryItem.ITEM_TYPE.BLACK_GOLD)
    {
      for (int index = 0; index < 10; ++index)
        InventoryItem.Spawn(this.Reward, 1, this.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
    }
    else if (InventoryItem.IsFood(this.Reward))
    {
      int num3 = UnityEngine.Random.Range(3, 6);
      for (int index = 0; index < num3; ++index)
        InventoryItem.Spawn(this.Reward, 1, this.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(5f, (float) UnityEngine.Random.Range(150, 350));
    }
    else
    {
      PickUp pickUp = InventoryItem.Spawn(this.Reward, 1, this.transform.position + Vector3.back, 0.0f);
      if ((bool) (UnityEngine.Object) pickUp)
        pickUp.SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      FoundItemPickUp component1 = pickUp.GetComponent<FoundItemPickUp>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.AutomaticallyInteract = true;
      Interaction_DoctrineStone component2 = pickUp.GetComponent<Interaction_DoctrineStone>();
      if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
        return;
      component2.MagnetToPlayer();
    }
  }

  private void SpawnBlackSouls()
  {
    BlackSoul blackSoul = InventoryItem.SpawnBlackSoul(Mathf.RoundToInt((float) UnityEngine.Random.Range(8, 12) * TrinketManager.GetBlackSoulsMultiplier()), this.transform.position, simulated: true);
    if (!(bool) (UnityEngine.Object) blackSoul)
      return;
    blackSoul.SetAngle((float) UnityEngine.Random.Range(0, 360), new Vector2(2f, 4f));
  }

  private IEnumerator GiveRewardDelay(float Delay = 1.06666672f)
  {
    Interaction_Chest interactionChest = this;
    yield return (object) new WaitForSeconds(Delay);
    interactionChest.Spine.AnimationState.SetAnimation(0, "open", false);
    interactionChest.ChestOpenSfx();
    interactionChest.Spine.AnimationState.AddAnimation(0, "opened", true, 0.0f);
    yield return (object) new WaitForSeconds(0.25f);
    interactionChest.RevealedGiveReward = false;
    int Quantity = 1;
    if (interactionChest.Reward == InventoryItem.ITEM_TYPE.GOLD_NUGGET)
      Quantity = UnityEngine.Random.Range(20, 30);
    if (interactionChest.Reward == InventoryItem.ITEM_TYPE.SEED || interactionChest.Reward == InventoryItem.ITEM_TYPE.SEED_PUMPKIN || interactionChest.Reward == InventoryItem.ITEM_TYPE.SEED_BEETROOT || interactionChest.Reward == InventoryItem.ITEM_TYPE.SEED_CAULIFLOWER)
      Quantity = UnityEngine.Random.Range(3, 6);
    if (InventoryItem.IsFood(interactionChest.Reward))
      Quantity = UnityEngine.Random.Range(3, 6);
    if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && interactionChest.Reward == InventoryItem.ITEM_TYPE.BLACK_GOLD)
    {
      switch (PlayerFarming.Location)
      {
        case FollowerLocation.Dungeon1_1:
          Quantity = 3;
          break;
        case FollowerLocation.Dungeon1_2:
          Quantity = 3;
          break;
        case FollowerLocation.Dungeon1_3:
          Quantity = 3;
          break;
        case FollowerLocation.Dungeon1_4:
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
    while (!interactionChest.gameObject.activeInHierarchy)
      yield return (object) null;
    if (interactionChest.Reward != InventoryItem.ITEM_TYPE.NONE)
    {
      for (int index = 0; index < Quantity; ++index)
      {
        PickUp pickUp = InventoryItem.Spawn(interactionChest.Reward, 1, interactionChest.transform.position + Vector3.back, 0.0f);
        pickUp.SetInitialSpeedAndDiraction(3f, (float) (270 + UnityEngine.Random.Range(-35, 35)));
        pickUp.MagnetDistance = 100f;
        pickUp.CanStopFollowingPlayer = false;
      }
    }
    if (UnityEngine.Random.Range(1, 15) == 3)
      interactionChest.SpawnBlackSouls();
    if (interactionChest.pickedReward == RewardsItem.ChestRewards.SPIDERS && interactionChest.TypeOfChest == Interaction_Chest.ChestType.Wooden)
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
      if (interactionChest.TypeOfChest == Interaction_Chest.ChestType.Silver)
      {
        interactionChest.SpawnGoodReward();
        interactionChest.SpawnBlackSouls();
      }
      if (interactionChest.TypeOfChest == Interaction_Chest.ChestType.Gold)
      {
        interactionChest.SpawnGoodReward();
        interactionChest.SpawnGoodReward();
      }
      if ((UnityEngine.Object) interactionChest.healthPlayer != (UnityEngine.Object) null)
      {
        double num1 = ((double) interactionChest.healthPlayer.totalHP - (double) interactionChest.healthPlayer.HP) / 10.0 * (double) DifficultyManager.GetHealthDropsMultiplier() * (PlayerWeapon.FirstTimeUsingWeapon ? 1.2000000476837158 : 1.0);
        float num2 = UnityEngine.Random.Range(0.0f, 1f);
        float num3 = UnityEngine.Random.Range(0.0f, 1f);
        double num4 = (double) num2;
        if (num1 >= num4)
        {
          interactionChest.Reward = (double) num3 >= 0.75 ? ((double) UnityEngine.Random.value < 0.5 ? InventoryItem.ITEM_TYPE.BLUE_HEART : InventoryItem.ITEM_TYPE.HALF_BLUE_HEART) : ((double) UnityEngine.Random.value < 0.5 ? InventoryItem.ITEM_TYPE.RED_HEART : InventoryItem.ITEM_TYPE.HALF_HEART);
          PickUp pickUp = InventoryItem.Spawn(interactionChest.Reward, 1, interactionChest.transform.position + Vector3.back, 0.0f);
          pickUp.SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
          pickUp.MagnetDistance = 2f;
          pickUp.CanStopFollowingPlayer = false;
        }
      }
    }
    AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionChest.gameObject);
    Interaction_Chest.ChestEvent onChestRevealed = Interaction_Chest.OnChestRevealed;
    if (onChestRevealed != null)
      onChestRevealed();
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

  private new void Update()
  {
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
          AudioManager.Instance.SetMusicCombatState(false);
          this.Reveal();
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

  private void SelectReward()
  {
    this.healthPlayer = (HealthPlayer) null;
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
      this.healthPlayer = PlayerFarming.Instance.GetComponent<HealthPlayer>();
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

  private void BackToIdle()
  {
    Debug.Log((object) "BACKL TO IDLE!");
    this.Loop = false;
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = false;
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().CameraResetTargetZoom();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.StartCoroutine((IEnumerator) this.DelayEffectsRoutine());
  }

  private IEnumerator DelayEffectsRoutine()
  {
    yield return (object) new WaitForSeconds(0.2f);
    TrinketManager.AddTrinket(this.DrawnCard);
  }

  private void CloseMenuCallback() => this.Loop = false;

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
