// Decompiled with JetBrains decompiler
// Type: Interaction_WeaponSelectionPodium
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMBiomeGeneration;
using MMTools;
using src.Extensions;
using src.UI.Overlays.TutorialOverlay;
using src.UI.Prompts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_WeaponSelectionPodium : Interaction
{
  public static List<Interaction_WeaponSelectionPodium> Podiums = new List<Interaction_WeaponSelectionPodium>();
  [CompilerGenerated]
  public EquipmentType \u003CTypeOfWeapon\u003Ek__BackingField = EquipmentType.None;
  [CompilerGenerated]
  public RelicType \u003CTypeOfRelic\u003Ek__BackingField;
  public int WeaponLevel;
  [SerializeField]
  public SpriteRenderer IconSpriteRenderer;
  public Vector3 spritesInitialScale;
  [SerializeField]
  public SpriteRenderer LockedIcon;
  [SerializeField]
  public Interaction_WeaponSelectionPodium[] otherWeaponOptions;
  [SerializeField]
  public PlayerFleeceManager.FleeceType[] onlyTurnOffOtherPodiumsWhileWearingFleeceType;
  [SerializeField]
  public PlayerFleeceManager.FleeceType[] coopKeepBothPodiumsActiveWhileWearingFleeceType;
  public ParticleSystem particleEffect;
  public GameObject podiumOn;
  public GameObject podiumOff;
  public Canvas canvas;
  [SerializeField]
  public Animator AvailableGoop;
  [SerializeField]
  public NewWeaponEffect NewWeaponEffect;
  public static System.Action<bool, PlayerFarming> OnHighlightWeapon;
  public static System.Action<bool, PlayerFarming> OnHighlightCurse;
  public GameObject Lighting;
  public bool DestroyOtherWeaponInCoop = true;
  public int LevelIncreaseAmount = 1;
  public Material WeaponMaterial;
  public Material CurseMaterial;
  public LayerMask collisionMask;
  public SpriteRenderer weaponBetterIcon;
  public Sprite weaponUp;
  public Sprite weaponDown;
  [SerializeField]
  public GameObject ps_relicsBlessed;
  [SerializeField]
  public GameObject ps_relicsDammed;
  [SerializeField]
  public GameObject Container;
  [SerializeField]
  public Interaction_WeaponSelectionPodium CoopPodium;
  [SerializeField]
  public Interaction_WeaponSelectionPodium TwinPodium;
  public Interaction_WeaponSelectionPodium.Types Type;
  public EquipmentType ForceEquipmentType = EquipmentType.None;
  public RelicType ForceRelicType;
  public bool allowSplitInPurgatory;
  public bool RemoveIfNotFirstLayer;
  [CompilerGenerated]
  public bool \u003CWeaponTaken\u003Ek__BackingField;
  public const int rerollLevelDecrease = 1;
  public bool initialDungeonEnter;
  public bool coopInitialDungeonEnterSet;
  public bool activated;
  public GameObject VFXRerollWeapon;
  public GameObject VFXRerollCurse;
  public bool increaseWeaponLevel = true;
  public bool CanOpenDoors = true;
  public UIWeaponPickupPromptController _weaponPickupUI;
  [CompilerGenerated]
  public bool \u003CRelicStartCharged\u003Ek__BackingField = true;
  public List<FollowerLocation> bossLocations = new List<FollowerLocation>()
  {
    FollowerLocation.Boss_5,
    FollowerLocation.Boss_Wolf,
    FollowerLocation.Boss_Yngya
  };
  public bool isCoopPodium;
  public EquipmentType PrevEquipment = EquipmentType.None;
  public static EquipmentType LastCurseSelected;
  public float WobbleTimer;
  public string sLabel = "";
  public bool AllowResummonWeapon;
  public int ResummonCost = 50;
  public static System.Action OnTutorialShown;

  public override bool InactiveAfterStopMoving => false;

  public EquipmentType TypeOfWeapon
  {
    get => this.\u003CTypeOfWeapon\u003Ek__BackingField;
    set => this.\u003CTypeOfWeapon\u003Ek__BackingField = value;
  }

  public RelicType TypeOfRelic
  {
    get => this.\u003CTypeOfRelic\u003Ek__BackingField;
    set => this.\u003CTypeOfRelic\u003Ek__BackingField = value;
  }

  public bool WeaponTaken
  {
    get => this.\u003CWeaponTaken\u003Ek__BackingField;
    set => this.\u003CWeaponTaken\u003Ek__BackingField = value;
  }

  public bool RelicStartCharged
  {
    get => this.\u003CRelicStartCharged\u003Ek__BackingField;
    set => this.\u003CRelicStartCharged\u003Ek__BackingField = value;
  }

  public void Awake()
  {
    if (!this.coopInitialDungeonEnterSet)
      this.initialDungeonEnter = GameManager.InitialDungeonEnter;
    if ((UnityEngine.Object) this.CoopPodium != (UnityEngine.Object) null)
    {
      this.CoopPodium.coopInitialDungeonEnterSet = true;
      this.CoopPodium.initialDungeonEnter = this.initialDungeonEnter;
      this.CoopPodium.ForceEquipmentType = this.ForceEquipmentType;
    }
    if (this.initialDungeonEnter)
      CoopManager.PreventWeaponSpawn = true;
    this.collisionMask = (LayerMask) ((int) this.collisionMask | 1 << LayerMask.NameToLayer("Island"));
    this.collisionMask = (LayerMask) ((int) this.collisionMask | 1 << LayerMask.NameToLayer("Obstacles"));
    this.spritesInitialScale = this.IconSpriteRenderer.transform.localScale;
  }

  public override void OnDisable()
  {
    base.OnDisable();
    PlayerWeapon.OnWeaponChanged -= new PlayerWeapon.WeaponEvent(this.OnWeaponChanged);
    PlayerSpells.OnCurseChanged -= new PlayerSpells.CurseEvent(this.OnCurseChanged);
    PlayerRelic.OnRelicChanged -= new PlayerRelic.RelicEvent(this.OnRelicChanged);
  }

  public void OnPlayerJoined()
  {
    bool flag = this.allowSplitInPurgatory && (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && (PlayerFarming.Instance.currentWeapon == EquipmentType.None || PlayerFarming.Instance.currentCurse == EquipmentType.None);
    if (PlayerFleeceManager.FleeceSwapsWeaponForCurse() || !this.initialDungeonEnter || !(!DungeonSandboxManager.Active | flag) || !((UnityEngine.Object) this.CoopPodium != (UnityEngine.Object) null) || this.CoopPodium.Type == Interaction_WeaponSelectionPodium.Types.Relic || this.CoopPodium.WeaponTaken)
      return;
    this.CoopPodium.LockedIcon.gameObject.SetActive(false);
    this.CoopPodium.IconSpriteRenderer.enabled = true;
    this.CoopPodium.podiumOn.SetActive(true);
    this.CoopPodium.podiumOff.SetActive(false);
    this.CoopPodium.AvailableGoop.Play("Show");
    this.CoopPodium.activated = false;
    this.CoopPodium.particleEffect.Stop();
    this.CoopPodium.Lighting.SetActive(true);
    this.CoopPodium.CheckWeaponLevel();
    this.CoopPodium.increaseWeaponLevel = false;
    this.CoopPodium.gameObject.SetActive(true);
    this.CoopPodium.transform.DOLocalMove(new Vector3(1.5f, 0.0f), 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.Container.transform.DOLocalMove(new Vector3(-1.5f, 0.0f), 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    if (this.bossLocations.Contains(LocationManager._Instance.Location))
      return;
    GameManager.GetInstance().WaitForSeconds(0.0f, (System.Action) (() => this.CoopPodium.RemoveIfNotFirstLayer = true));
  }

  public new void OnPlayerLeft() => this.TryToOpenDoors();

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    Lamb.UI.SettingsMenu.AccessibilitySettings.OnWeaponForceChange += new System.Action(this.OnWeaponForceChange);
    if ((UnityEngine.Object) this.ps_relicsBlessed != (UnityEngine.Object) null)
      this.ps_relicsBlessed.SetActive(false);
    if ((UnityEngine.Object) this.ps_relicsDammed != (UnityEngine.Object) null)
      this.ps_relicsDammed.SetActive(false);
    this.HasSecondaryInteraction = true;
    if ((UnityEngine.Object) this.CoopPodium != (UnityEngine.Object) null)
    {
      this.CoopPodium.Type = this.Type;
      if (CoopManager.CoopActive)
        this.OnPlayerJoined();
      else
        this.CoopPodium.gameObject.SetActive(false);
    }
    if ((UnityEngine.Object) CoopManager.Instance != (UnityEngine.Object) null)
    {
      CoopManager.Instance.OnPlayerJoined += new System.Action(this.OnPlayerJoined);
      CoopManager.Instance.OnPlayerLeft += new System.Action(this.OnPlayerLeft);
    }
    Interaction_WeaponSelectionPodium.Podiums.Add(this);
    this.WobbleTimer = (float) UnityEngine.Random.Range(0, 360);
    if (this.Type == Interaction_WeaponSelectionPodium.Types.Random)
      this.Type = (double) UnityEngine.Random.value >= 0.5 ? Interaction_WeaponSelectionPodium.Types.Curse : Interaction_WeaponSelectionPodium.Types.Weapon;
    if (PlayerFarming.Location == FollowerLocation.IntroDungeon)
      this.Type = Interaction_WeaponSelectionPodium.Types.Weapon;
    bool flag = this.RemoveIfNotFirstLayer && (GameManager.CurrentDungeonFloor > 1 || !this.initialDungeonEnter) && !this.IsPlayerInBishopRoom();
    if (DataManager.Instance.ForcingPlayerWeaponDLC != EquipmentType.None)
      flag = true;
    if (flag)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    else if (this.Type == Interaction_WeaponSelectionPodium.Types.Curse && !DataManager.Instance.EnabledSpells)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
    {
      if (!this.WeaponTaken && (UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && !BiomeGenerator.Instance.CurrentRoom.Completed && this.Type != Interaction_WeaponSelectionPodium.Types.Relic)
      {
        if (this.RemoveIfNotFirstLayer && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_ResummonWeapon))
        {
          this.AllowResummonWeapon = true;
          if ((UnityEngine.Object) this.CoopPodium != (UnityEngine.Object) null)
            this.CoopPodium.SetAllowResummonWeapon(this.AllowResummonWeapon);
        }
        BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.LockDoors);
      }
      Interaction_Chest.OnChestRevealed += new Interaction_Chest.ChestEvent(this.Reveal);
      if (!this.WeaponTaken)
      {
        this.AvailableGoop.Play("Show");
      }
      else
      {
        this.AvailableGoop.Play("Hidden");
        this.particleEffect.Stop();
      }
      this.weaponBetterIcon.enabled = false;
      if (this.WeaponTaken)
        return;
      this.CheckWeaponLevel();
    }
  }

  public bool IsPlayerInBishopRoom()
  {
    return PlayerFarming.Location == FollowerLocation.Boss_1 || PlayerFarming.Location == FollowerLocation.Boss_2 || PlayerFarming.Location == FollowerLocation.Boss_3 || PlayerFarming.Location == FollowerLocation.Boss_4;
  }

  public void OnWeaponForceChange()
  {
    this.StartCoroutine((IEnumerator) this.SetItem(ForceLevel: DataManager.Instance.CurrentRunWeaponLevel));
  }

  public void CheckWeaponLevel()
  {
    if (this.Type == Interaction_WeaponSelectionPodium.Types.Curse)
    {
      if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null && this.playerFarming.currentCurseLevel > this.WeaponLevel)
      {
        this.weaponBetterIcon.sprite = this.weaponDown;
        this.weaponBetterIcon.enabled = true;
      }
      else if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null && this.playerFarming.currentCurseLevel < this.WeaponLevel)
      {
        this.weaponBetterIcon.sprite = this.weaponUp;
        this.weaponBetterIcon.enabled = true;
      }
      if (!((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null) && this.playerFarming.currentCurse != EquipmentType.None)
        return;
      this.weaponBetterIcon.enabled = false;
    }
    else
    {
      if (this.Type != Interaction_WeaponSelectionPodium.Types.Weapon)
        return;
      if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null || this.playerFarming.currentWeaponLevel > this.WeaponLevel)
      {
        this.weaponBetterIcon.sprite = this.weaponDown;
        this.weaponBetterIcon.enabled = true;
      }
      else if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null || this.playerFarming.currentWeaponLevel < this.WeaponLevel)
      {
        this.weaponBetterIcon.sprite = this.weaponUp;
        this.weaponBetterIcon.enabled = true;
      }
      if (!((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null) && this.playerFarming.currentWeapon != EquipmentType.None)
        return;
      this.weaponBetterIcon.enabled = false;
    }
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    Lamb.UI.SettingsMenu.AccessibilitySettings.OnWeaponForceChange -= new System.Action(this.OnWeaponForceChange);
    if ((UnityEngine.Object) CoopManager.Instance != (UnityEngine.Object) null)
    {
      CoopManager.Instance.OnPlayerJoined -= new System.Action(this.OnPlayerJoined);
      CoopManager.Instance.OnPlayerLeft -= new System.Action(this.OnPlayerLeft);
    }
    Interaction_WeaponSelectionPodium.Podiums.Remove(this);
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.LockDoors);
    Interaction_Chest.OnChestRevealed -= new Interaction_Chest.ChestEvent(this.Reveal);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.LockDoors);
    Interaction_Chest.OnChestRevealed -= new Interaction_Chest.ChestEvent(this.Reveal);
  }

  public void Reveal()
  {
    Debug.Log((object) "Reveal()");
    if (PlayerFarming.Location == FollowerLocation.IntroDungeon)
      return;
    this.gameObject.SetActive(true);
    this.NewWeaponEffect.gameObject.SetActive(true);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    Debug.Log((object) $"OnEnable()  {this.TypeOfWeapon.ToString()} {this.TypeOfRelic.ToString()}");
    if (this.TypeOfWeapon == EquipmentType.None && this.TypeOfRelic == RelicType.None)
    {
      this.ActivateDistance = 2f;
      this.UpdateLocalisation();
      this.StartCoroutine((IEnumerator) this.SetItem());
      this.podiumOff.SetActive(false);
    }
    if (GameManager.CurrentDungeonFloor > 1)
      return;
    PlayerWeapon.OnWeaponChanged -= new PlayerWeapon.WeaponEvent(this.OnWeaponChanged);
    PlayerSpells.OnCurseChanged -= new PlayerSpells.CurseEvent(this.OnCurseChanged);
    PlayerRelic.OnRelicChanged -= new PlayerRelic.RelicEvent(this.OnRelicChanged);
    PlayerWeapon.OnWeaponChanged += new PlayerWeapon.WeaponEvent(this.OnWeaponChanged);
    PlayerSpells.OnCurseChanged += new PlayerSpells.CurseEvent(this.OnCurseChanged);
    PlayerRelic.OnRelicChanged += new PlayerRelic.RelicEvent(this.OnRelicChanged);
  }

  public void ResetRandom(bool ForceShowGoop = false, int ForceLevel = -1)
  {
    this.IconSpriteRenderer.enabled = true;
    this.WeaponTaken = false;
    this.StartCoroutine((IEnumerator) this.SetItem(ForceShowGoop, ForceLevel));
  }

  public IEnumerator SetItem(bool ForceShowGoop = false, int ForceLevel = -1)
  {
    yield return (object) new WaitForEndOfFrame();
    switch (this.Type)
    {
      case Interaction_WeaponSelectionPodium.Types.Weapon:
        this.SetWeapon(ForceLevel);
        break;
      case Interaction_WeaponSelectionPodium.Types.Curse:
        if (DataManager.Instance.EnabledSpells)
        {
          this.SetCurse(ForceLevel);
          break;
        }
        this.Type = Interaction_WeaponSelectionPodium.Types.Weapon;
        this.SetWeapon(ForceLevel);
        break;
      case Interaction_WeaponSelectionPodium.Types.Relic:
        this.SetRelic();
        break;
    }
    this.IconSpriteRenderer.sprite = this.GetIcon();
    this.LockedIcon.gameObject.SetActive(false);
    this.canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
    if (!ForceShowGoop)
    {
      if (!this.WeaponTaken)
        this.AvailableGoop.Play("Show");
      else
        this.AvailableGoop.Play("Hidden");
    }
    this.CheckWeaponLevel();
  }

  public void LockDoors()
  {
    if (!this.RemoveIfNotFirstLayer || GameManager.CurrentDungeonFloor > 1)
      return;
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.LockDoors);
    RoomLockController.CloseAll(false);
  }

  public virtual void SetWeapon(int ForceLevel = -1)
  {
    Debug.Log((object) "Set weapon here");
    if (PlayerFleeceManager.FleeceSwapsWeaponForCurse() && this.ForceEquipmentType == EquipmentType.None)
    {
      this.SetCurse((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null || this.playerFarming.currentCurse == EquipmentType.None ? DataManager.StartingEquipmentLevel + this.LevelIncreaseAmount : DataManager.Instance.CurrentRunCurseLevel + this.LevelIncreaseAmount - 1);
    }
    else
    {
      this.IconSpriteRenderer.material = this.WeaponMaterial;
      this.TypeOfWeapon = this.ForceEquipmentType != EquipmentType.None ? this.ForceEquipmentType : DataManager.Instance.GetRandomWeaponInPool(coopPodium: this.isCoopPodium);
      int num = 100;
      if (this.PrevEquipment != EquipmentType.None)
      {
        while ((this.TypeOfWeapon == this.PrevEquipment || TrinketManager.HasTrinket(TarotCards.Card.Spider, this.playerFarming) && EquipmentManager.IsPoisonWeapon(this.TypeOfWeapon)) && --num > 0)
          this.TypeOfWeapon = DataManager.Instance.GetRandomWeaponInPool(coopPodium: this.isCoopPodium);
      }
      this.PrevEquipment = this.TypeOfWeapon;
      this.WeaponLevel = !this.bossLocations.Contains(PlayerFarming.Location) ? DataManager.Instance.CurrentRunWeaponLevel + 1 : 1;
      if (!this.increaseWeaponLevel && !this.bossLocations.Contains(PlayerFarming.Location))
      {
        --this.WeaponLevel;
      }
      else
      {
        if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null || this.playerFarming.currentWeapon == EquipmentType.None)
          this.WeaponLevel += DataManager.StartingEquipmentLevel;
        if (ForceLevel == -1)
        {
          DataManager.Instance.CurrentRunWeaponLevel = this.WeaponLevel;
          this.WeaponLevel += Mathf.Clamp(this.LevelIncreaseAmount - 1, 0, this.LevelIncreaseAmount);
        }
        else
          this.WeaponLevel = DataManager.Instance.CurrentRunWeaponLevel = ForceLevel;
      }
    }
  }

  public virtual void SetRelic()
  {
    this.SetRelic((this.ForceRelicType == RelicType.None ? EquipmentManager.GetRandomRelicData(false, this.playerFarming) : EquipmentManager.GetRelicData(this.ForceRelicType)).RelicType);
  }

  public void SetRelic(RelicType relicType)
  {
    if (DataManager.Instance.FirstRelic)
    {
      DataManager.Instance.FirstRelic = false;
      relicType = RelicType.LightningStrike;
    }
    this.IconSpriteRenderer.material = this.WeaponMaterial;
    this.IconSpriteRenderer.transform.localScale = Vector3.one * 0.41f;
    this.spritesInitialScale = this.IconSpriteRenderer.transform.localScale;
    this.IconSpriteRenderer.transform.parent.localPosition = new Vector3(this.IconSpriteRenderer.transform.parent.localPosition.x, this.IconSpriteRenderer.transform.parent.localPosition.y, -1.5f);
    this.TypeOfRelic = relicType;
    if (this.TypeOfRelic.ToString().Contains("Blessed"))
    {
      this.ps_relicsBlessed.SetActive(true);
      if (DataManager.Instance.ForceBlessedRelic)
        DataManager.Instance.ForceBlessedRelic = false;
    }
    else if (this.TypeOfRelic.ToString().Contains("Dammed"))
    {
      this.ps_relicsDammed.SetActive(true);
      if (DataManager.Instance.ForceDammedRelic)
        DataManager.Instance.ForceDammedRelic = false;
    }
    if (!DataManager.Instance.SpawnedRelicsThisRun.Contains(this.TypeOfRelic))
      DataManager.Instance.SpawnedRelicsThisRun.Add(this.TypeOfRelic);
    this.Type = Interaction_WeaponSelectionPodium.Types.Relic;
  }

  public void ForceSetRelic(RelicType relicType)
  {
    this.IconSpriteRenderer.material = this.WeaponMaterial;
    this.IconSpriteRenderer.transform.localScale = Vector3.one * 0.41f;
    this.IconSpriteRenderer.transform.parent.localPosition = new Vector3(this.IconSpriteRenderer.transform.parent.localPosition.x, this.IconSpriteRenderer.transform.parent.localPosition.y, -1.5f);
    this.ForceRelicType = relicType;
    this.Type = Interaction_WeaponSelectionPodium.Types.Relic;
  }

  public virtual void SetCurse(int ForceLevel = -1)
  {
    if (PlayerFleeceManager.FleeceSwapsCurseForRelic())
    {
      this.SetRelic();
    }
    else
    {
      this.Type = Interaction_WeaponSelectionPodium.Types.Curse;
      this.IconSpriteRenderer.material = this.CurseMaterial;
      for (int index = 0; index < 100; ++index)
      {
        this.TypeOfWeapon = DataManager.Instance.GetRandomCurseInPool(this.isCoopPodium);
        if (this.TypeOfWeapon != Interaction_WeaponSelectionPodium.LastCurseSelected)
          break;
      }
      Interaction_WeaponSelectionPodium.LastCurseSelected = this.TypeOfWeapon;
      if (this.ForceEquipmentType != EquipmentType.None)
        this.TypeOfWeapon = this.ForceEquipmentType;
      if (PlayerFleeceManager.FleeceSwapsWeaponForCurse())
        this.WeaponLevel += DataManager.Instance.CurrentRunCurseLevel;
      else
        this.WeaponLevel = !this.bossLocations.Contains(PlayerFarming.Location) ? DataManager.Instance.CurrentRunCurseLevel : 0;
      if (DataManager.Instance.ForcedStartingCurse != EquipmentType.None && !DungeonSandboxManager.Active)
      {
        this.TypeOfWeapon = DataManager.Instance.ForcedStartingCurse;
        DataManager.Instance.ForcedStartingCurse = EquipmentType.None;
      }
      if (!this.increaseWeaponLevel && !this.bossLocations.Contains(PlayerFarming.Location))
        return;
      ++this.WeaponLevel;
      if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null || this.playerFarming.currentCurse == EquipmentType.None)
        this.WeaponLevel += DataManager.StartingEquipmentLevel;
      if (ForceLevel == -1)
      {
        DataManager.Instance.CurrentRunCurseLevel = this.WeaponLevel;
        this.WeaponLevel += Mathf.Clamp(this.LevelIncreaseAmount - 1, 0, this.LevelIncreaseAmount);
      }
      else
        this.WeaponLevel = DataManager.Instance.CurrentRunWeaponLevel = ForceLevel;
    }
  }

  public Sprite GetIcon()
  {
    return this.TypeOfRelic != RelicType.None ? EquipmentManager.GetRelicData(this.TypeOfRelic).Sprite : EquipmentManager.GetEquipmentData(this.TypeOfWeapon).WorldSprite;
  }

  public override void Update()
  {
    base.Update();
    if (this.WeaponTaken || !(bool) (UnityEngine.Object) this.IconSpriteRenderer)
      return;
    this.IconSpriteRenderer.transform.localPosition = new Vector3(0.0f, 0.0f, Mathf.Sin(this.WobbleTimer += Time.deltaTime * 2f) * 0.1f);
  }

  public override void IndicateHighlighted(PlayerFarming playerFarming)
  {
    base.IndicateHighlighted(playerFarming);
    float damage = 0.0f;
    float speed = 0.0f;
    switch (this.Type)
    {
      case Interaction_WeaponSelectionPodium.Types.Weapon:
        System.Action<bool, PlayerFarming> onHighlightWeapon = Interaction_WeaponSelectionPodium.OnHighlightWeapon;
        if (onHighlightWeapon != null)
        {
          onHighlightWeapon(true, playerFarming);
          break;
        }
        break;
      case Interaction_WeaponSelectionPodium.Types.Curse:
        System.Action<bool, PlayerFarming> onHighlightCurse = Interaction_WeaponSelectionPodium.OnHighlightCurse;
        if (onHighlightCurse != null)
        {
          onHighlightCurse(true, playerFarming);
          break;
        }
        break;
    }
    if (this.Type == Interaction_WeaponSelectionPodium.Types.Weapon)
    {
      damage = playerFarming.playerWeapon.GetAverageWeaponDamage(this.TypeOfWeapon, this.WeaponLevel);
      speed = playerFarming.playerWeapon.GetWeaponSpeed(this.TypeOfWeapon);
    }
    AudioManager.Instance.PlayOneShot("event:/ui/open_menu", playerFarming.transform.position);
    if ((UnityEngine.Object) this._weaponPickupUI == (UnityEngine.Object) null)
    {
      this._weaponPickupUI = MonoSingleton<UIManager>.Instance.WeaponPickPromptControllerTemplate.Instantiate<UIWeaponPickupPromptController>();
      this._weaponPickupUI.Init(playerFarming);
      UIWeaponPickupPromptController weaponPickupUi = this._weaponPickupUI;
      weaponPickupUi.OnHidden = weaponPickupUi.OnHidden + (System.Action) (() => this._weaponPickupUI = (UIWeaponPickupPromptController) null);
    }
    if (this.Type == Interaction_WeaponSelectionPodium.Types.Relic)
      this._weaponPickupUI.Show(playerFarming, this.TypeOfRelic);
    else
      this._weaponPickupUI.Show(playerFarming, this.TypeOfWeapon, damage, speed, this.WeaponLevel);
  }

  public override void EndIndicateHighlighted(PlayerFarming playerFarming)
  {
    base.EndIndicateHighlighted(playerFarming);
    switch (this.Type)
    {
      case Interaction_WeaponSelectionPodium.Types.Weapon:
        System.Action<bool, PlayerFarming> onHighlightWeapon = Interaction_WeaponSelectionPodium.OnHighlightWeapon;
        if (onHighlightWeapon != null)
        {
          onHighlightWeapon(false, playerFarming);
          break;
        }
        break;
      case Interaction_WeaponSelectionPodium.Types.Curse:
        System.Action<bool, PlayerFarming> onHighlightCurse = Interaction_WeaponSelectionPodium.OnHighlightCurse;
        if (onHighlightCurse != null)
        {
          onHighlightCurse(false, playerFarming);
          break;
        }
        break;
    }
    if (!((UnityEngine.Object) this._weaponPickupUI != (UnityEngine.Object) null))
      return;
    this._weaponPickupUI.Hide();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = ScriptLocalization.Interactions.Equip;
  }

  public override void GetLabel()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (!((UnityEngine.Object) player.interactor.TempInteraction != (UnityEngine.Object) this))
      {
        if (this.WeaponTaken)
        {
          this.Label = "";
          this.Interactable = false;
        }
        else
        {
          this.Label = this.sLabel;
          this.Interactable = true;
        }
      }
    }
  }

  public override void GetSecondaryLabel()
  {
    this.SecondaryLabel = !this.Interactable || !this.AllowResummonWeapon || this.WeaponTaken ? "" : $"{ScriptLocalization.Interactions.Resummon} {CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, this.ResummonCost)}";
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    if (!this.AllowResummonWeapon)
      return;
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD) < this.ResummonCost)
    {
      this.playerFarming.indicator.PlayShake();
      UIManager.PlayAudio("event:/ui/negative_feedback");
    }
    else
    {
      Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD, -this.ResummonCost);
      switch (this.Type)
      {
        case Interaction_WeaponSelectionPodium.Types.Weapon:
          GameObject gameObject1 = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/WeaponPickUp"), this.transform.position, Quaternion.identity, this.GetPickupTransformParent()) as GameObject;
          PickUp component1 = gameObject1.GetComponent<PickUp>();
          component1.SetIgnoreBoundsCheck();
          component1.SetInitialSpeedAndDiraction(12f, 270f);
          gameObject1.GetComponent<Interaction_WeaponPickUp>().SetWeapon(this.TypeOfWeapon, this.WeaponLevel, Interaction_WeaponPickUp.Types.Weapon);
          this.VFXRerollWeapon.SetActive(true);
          --DataManager.Instance.CurrentRunWeaponLevel;
          AudioManager.Instance.PlayOneShot("event:/temple_key/become_whole", this.transform.position);
          break;
        case Interaction_WeaponSelectionPodium.Types.Curse:
          GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/WeaponPickUp"), this.transform.position, Quaternion.identity, this.GetPickupTransformParent()) as GameObject;
          PickUp component2 = gameObject2.GetComponent<PickUp>();
          component2.SetIgnoreBoundsCheck();
          component2.SetInitialSpeedAndDiraction(12f, 270f);
          gameObject2.GetComponent<Interaction_WeaponPickUp>().SetWeapon(this.TypeOfWeapon, this.WeaponLevel, Interaction_WeaponPickUp.Types.Curse);
          this.VFXRerollCurse.SetActive(true);
          --DataManager.Instance.CurrentRunCurseLevel;
          AudioManager.Instance.PlayOneShot("event:/temple_key/fragment_into_place", this.transform.position);
          break;
      }
      this.HasSecondaryInteraction = false;
      this.AllowResummonWeapon = false;
      Vector3 localScale = this.IconSpriteRenderer.transform.localScale;
      this.activated = true;
      this.WeaponTaken = true;
      this.HasChanged = true;
      this.EndIndicateHighlighted(this.playerFarming);
      DG.Tweening.Sequence sequence = DOTween.Sequence();
      sequence.Append((Tween) this.IconSpriteRenderer.transform.DOScale(Vector3.zero, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack));
      sequence.AppendInterval(0.5f);
      sequence.AppendCallback((TweenCallback) (() =>
      {
        this.ResetRandom(true, this.WeaponLevel - 1);
        this.WeaponTaken = true;
      }));
      sequence.Append((Tween) this.IconSpriteRenderer.transform.DOScale(localScale, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack));
      sequence.AppendCallback((TweenCallback) (() =>
      {
        this.WeaponTaken = false;
        this.HasChanged = true;
        this.activated = false;
      }));
      sequence.Play<DG.Tweening.Sequence>();
    }
  }

  public override void OnInteract(StateMachine state)
  {
    if (!this.activated)
    {
      PlayerFarming component = state.GetComponent<PlayerFarming>();
      if (component.isLamb && this.GoatOnly || !component.isLamb && this.LambOnly || (this.initialDungeonEnter || (UnityEngine.Object) this.CoopPodium != (UnityEngine.Object) null && !DungeonSandboxManager.Active) && !this.GetAllPlayersWearingWeapons() && PlayerFarming.players.Count > 1 && (this.Type == Interaction_WeaponSelectionPodium.Types.Weapon && component.currentWeapon != EquipmentType.None || this.Type == Interaction_WeaponSelectionPodium.Types.Curse && component.currentCurse != EquipmentType.None))
        return;
      if (component.isLamb && (bool) (UnityEngine.Object) this.TwinPodium)
      {
        this.TwinPodium.GoatOnly = true;
        this.TwinPodium.LambOnly = false;
      }
      else if (!component.isLamb && (bool) (UnityEngine.Object) this.TwinPodium)
      {
        this.TwinPodium.LambOnly = true;
        this.TwinPodium.GoatOnly = false;
      }
      base.OnInteract(state);
      this.weaponBetterIcon.enabled = false;
      this.activated = true;
      switch (this.Type)
      {
        case Interaction_WeaponSelectionPodium.Types.Weapon:
          this.StartCoroutine((IEnumerator) this.PlayerShowWeaponRoutine(component));
          break;
        case Interaction_WeaponSelectionPodium.Types.Curse:
          this.StartCoroutine((IEnumerator) this.PlayerShowCurseRoutine(component));
          break;
        case Interaction_WeaponSelectionPodium.Types.Relic:
          this.ps_relicsBlessed.GetComponent<ParticleSystem>().Stop();
          this.ps_relicsDammed.GetComponent<ParticleSystem>().Stop();
          component.playerRelic.EquipRelic(EquipmentManager.GetRelicData(this.TypeOfRelic), this.RelicStartCharged, true);
          this.Interactable = false;
          this.Lighting.SetActive(false);
          this.IconSpriteRenderer.enabled = false;
          this.weaponBetterIcon.enabled = false;
          this.podiumOn.SetActive(false);
          this.podiumOff.SetActive(true);
          this.particleEffect.Stop();
          this.AvailableGoop.Play("Hide");
          this.enabled = false;
          state.CURRENT_STATE = StateMachine.State.Idle;
          break;
      }
    }
    PlayerFleeceManager.FleeceType playerFleece = (PlayerFleeceManager.FleeceType) DataManager.Instance.PlayerFleece;
    if (CoopManager.CoopActive && !this.DestroyOtherWeaponInCoop && this.coopKeepBothPodiumsActiveWhileWearingFleeceType.Contains<PlayerFleeceManager.FleeceType>(playerFleece) || CoopManager.CoopActive && !this.DestroyOtherWeaponInCoop || !this.onlyTurnOffOtherPodiumsWhileWearingFleeceType.Contains<PlayerFleeceManager.FleeceType>(playerFleece) && this.onlyTurnOffOtherPodiumsWhileWearingFleeceType.Length != 0)
      return;
    for (int index = this.otherWeaponOptions.Length - 1; index >= 0; --index)
    {
      if (this.IsPodiumInSameRoom(this.otherWeaponOptions[index]))
      {
        this.otherWeaponOptions[index].Interactable = false;
        this.otherWeaponOptions[index].Lighting.SetActive(false);
        this.otherWeaponOptions[index].IconSpriteRenderer.enabled = false;
        this.otherWeaponOptions[index].weaponBetterIcon.enabled = false;
        this.otherWeaponOptions[index].podiumOn.SetActive(false);
        this.otherWeaponOptions[index].podiumOff.SetActive(true);
        this.otherWeaponOptions[index].particleEffect.Stop();
        this.otherWeaponOptions[index].AvailableGoop.Play("Hide");
        this.otherWeaponOptions[index].enabled = false;
      }
    }
  }

  public bool IsPodiumInSameRoom(Interaction_WeaponSelectionPodium otherPodium)
  {
    return BiomeGenerator.Instance.CurrentRoom.GameObject.GetComponentsInChildren<Interaction_WeaponSelectionPodium>().Contains<Interaction_WeaponSelectionPodium>(otherPodium);
  }

  public IEnumerator PlayerShowWeaponRoutine(PlayerFarming playerFarming)
  {
    Interaction_WeaponSelectionPodium weaponSelectionPodium = this;
    weaponSelectionPodium.AvailableGoop.Play("Hide");
    playerFarming.GoToAndStop(weaponSelectionPodium.transform.position + Vector3.down * 0.5f, weaponSelectionPodium.gameObject);
    while (playerFarming.GoToAndStopping)
      yield return (object) null;
    DataManager.Instance.CurrentRunWeaponLevel += Mathf.Clamp(weaponSelectionPodium.LevelIncreaseAmount - 1, 0, weaponSelectionPodium.LevelIncreaseAmount);
    if (playerFarming.currentWeapon != EquipmentType.None)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/WeaponPickUp"), weaponSelectionPodium.state.transform.position, Quaternion.identity, weaponSelectionPodium.GetPickupTransformParent()) as GameObject;
      PickUp component = gameObject.GetComponent<PickUp>();
      component.SetIgnoreBoundsCheck();
      component.SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-2f, 2.5f), 270f);
      gameObject.GetComponent<Interaction_WeaponPickUp>().SetWeapon(playerFarming.currentWeapon, playerFarming.playerWeapon.CurrentWeaponLevel, Interaction_WeaponPickUp.Types.Weapon);
    }
    if (weaponSelectionPodium.TypeOfWeapon == EquipmentType.None)
      weaponSelectionPodium.TypeOfWeapon = EquipmentType.Sword;
    playerFarming.playerWeapon.SetWeapon(weaponSelectionPodium.TypeOfWeapon, weaponSelectionPodium.WeaponLevel);
    weaponSelectionPodium.SetWeaponInactive();
    if (!DataManager.Instance.WeaponPool.Contains(weaponSelectionPodium.TypeOfWeapon))
      DataManager.Instance.WeaponPool.Add(weaponSelectionPodium.TypeOfWeapon);
    weaponSelectionPodium.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    GameManager.GetInstance().CameraSetTargetZoom(8f);
    AudioManager.Instance.PlayOneShot("event:/player/weapon_equip", weaponSelectionPodium.transform.position);
    AudioManager.Instance.PlayOneShot("event:/player/weapon_unlocked", weaponSelectionPodium.transform.position);
    yield return (object) new WaitForSeconds(playerFarming.Spine.AnimationState.SetAnimation(0, playerFarming.CurrentWeaponInfo.WeaponData.PickupAnimationKey, false).Animation.Duration);
    GameManager.GetInstance().CameraResetTargetZoom();
    weaponSelectionPodium.state.CURRENT_STATE = StateMachine.State.Idle;
    if (CoopManager.CoopActive && MMConversation.CURRENT_CONVERSATION != null)
    {
      for (int index = 0; index < PlayerFarming.players.Count; ++index)
      {
        if ((UnityEngine.Object) PlayerFarming.players[index] != (UnityEngine.Object) playerFarming && PlayerFarming.players[index].state.CURRENT_STATE == StateMachine.State.InActive)
          weaponSelectionPodium.state.CURRENT_STATE = StateMachine.State.InActive;
      }
    }
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.PUpgrade_HeavyAttacks) && DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.HeavyAttacks))
    {
      UITutorialOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.HeavyAttacks);
      overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
      {
        System.Action onTutorialShown = Interaction_WeaponSelectionPodium.OnTutorialShown;
        if (onTutorialShown == null)
          return;
        onTutorialShown();
      });
    }
  }

  public IEnumerator PlayerShowCurseRoutine(PlayerFarming playerFarming)
  {
    Interaction_WeaponSelectionPodium weaponSelectionPodium = this;
    weaponSelectionPodium.AvailableGoop.Play("Hide");
    LayerMask layerMask = (LayerMask) ((int) (LayerMask) ((int) new LayerMask() | 1 << LayerMask.NameToLayer("Obstacles")) | 1 << LayerMask.NameToLayer("Island"));
    bool flag1 = (bool) Physics2D.Raycast((Vector2) weaponSelectionPodium.transform.position, (Vector2) Vector3.right, 1.5f, (int) layerMask);
    bool flag2 = (bool) Physics2D.Raycast((Vector2) weaponSelectionPodium.transform.position, (Vector2) Vector3.left, 1.5f, (int) layerMask);
    if ((((double) weaponSelectionPodium.transform.position.x >= (double) weaponSelectionPodium.state.transform.position.x ? 0 : (!flag1 ? 1 : 0)) | (flag2 ? 1 : 0)) != 0)
      playerFarming.GoToAndStop(weaponSelectionPodium.transform.position + Vector3.right * 1.25f, weaponSelectionPodium.gameObject);
    else if ((((double) weaponSelectionPodium.transform.position.x < (double) weaponSelectionPodium.state.transform.position.x ? 0 : (!flag2 ? 1 : 0)) | (flag1 ? 1 : 0)) != 0)
      playerFarming.GoToAndStop(weaponSelectionPodium.transform.position + Vector3.left * 1.25f, weaponSelectionPodium.gameObject);
    while (playerFarming.GoToAndStopping)
      yield return (object) null;
    playerFarming.playerSpells.faithAmmo.Reload();
    weaponSelectionPodium.WeaponTaken = true;
    weaponSelectionPodium.IconSpriteRenderer.transform.DOShakePosition(2f, 0.25f);
    weaponSelectionPodium.IconSpriteRenderer.transform.DOShakeRotation(2f, new Vector3(0.0f, 0.0f, 15f));
    DG.Tweening.Sequence Sequence = DOTween.Sequence();
    Sequence.Append((Tween) weaponSelectionPodium.IconSpriteRenderer.transform.DOScale(Vector3.one * 1.2f, 0.3f));
    Sequence.Append((Tween) weaponSelectionPodium.IconSpriteRenderer.transform.DOScale(Vector3.one * 0.8f, 0.3f));
    Sequence.Play<DG.Tweening.Sequence>().SetLoops<DG.Tweening.Sequence>(-1);
    if (!DataManager.Instance.CursePool.Contains(weaponSelectionPodium.TypeOfWeapon))
      DataManager.Instance.CursePool.Add(weaponSelectionPodium.TypeOfWeapon);
    weaponSelectionPodium.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    GameManager.GetInstance().CameraSetTargetZoom(8f);
    AudioManager.Instance.PlayOneShot("event:/player/absorb_curse", weaponSelectionPodium.gameObject);
    playerFarming.Spine.AnimationState.SetAnimation(0, "Curses/curse-get", false);
    yield return (object) new WaitForSeconds(1.0333333f);
    weaponSelectionPodium.IconSpriteRenderer.enabled = false;
    Sequence.Kill();
    weaponSelectionPodium.Lighting.SetActive(false);
    weaponSelectionPodium.podiumOn.SetActive(false);
    weaponSelectionPodium.podiumOff.SetActive(true);
    weaponSelectionPodium.particleEffect.Stop();
    yield return (object) new WaitForSeconds(0.3f);
    GameManager.GetInstance().CameraResetTargetZoom();
    weaponSelectionPodium.state.CURRENT_STATE = StateMachine.State.Idle;
    DataManager.Instance.CurrentRunCurseLevel += Mathf.Clamp(weaponSelectionPodium.LevelIncreaseAmount - 1, 0, weaponSelectionPodium.LevelIncreaseAmount);
    if (playerFarming.currentCurse != EquipmentType.None)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Resources/WeaponPickUp"), weaponSelectionPodium.state.transform.position, Quaternion.identity, weaponSelectionPodium.GetPickupTransformParent()) as GameObject;
      PickUp component = gameObject.GetComponent<PickUp>();
      component.SetIgnoreBoundsCheck();
      component.SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-2f, 2.5f), 270f);
      gameObject.GetComponent<Interaction_WeaponPickUp>().SetWeapon(playerFarming.currentCurse, playerFarming.currentCurseLevel, Interaction_WeaponPickUp.Types.Curse);
    }
    playerFarming.playerSpells.SetSpell(weaponSelectionPodium.TypeOfWeapon, weaponSelectionPodium.WeaponLevel);
    if (weaponSelectionPodium.RemoveIfNotFirstLayer)
      weaponSelectionPodium.TryToOpenDoors();
  }

  public void OnWeaponChanged(EquipmentType Weapon, int Level, PlayerFarming playerFarming)
  {
    this.TryToOpenDoors();
  }

  public void OnCurseChanged(EquipmentType curse, int Level, PlayerFarming playerfarming)
  {
    this.TryToOpenDoors();
  }

  public void OnRelicChanged(RelicData relic, PlayerFarming playerfarming) => this.TryToOpenDoors();

  public void TryToOpenDoors()
  {
    if (GameManager.CurrentDungeonFloor > 1)
      return;
    bool playersWearingWeapons = this.GetAllPlayersWearingWeapons();
    foreach (Interaction_WeaponSelectionPodium podium in Interaction_WeaponSelectionPodium.Podiums)
    {
      if (podium.Type == Interaction_WeaponSelectionPodium.Types.Relic && !podium.activated)
        return;
    }
    if (playersWearingWeapons)
    {
      if (this.CanOpenDoors)
        RoomLockController.RoomCompleted();
      CoopManager.PreventWeaponSpawn = false;
      PlayerWeapon.OnWeaponChanged -= new PlayerWeapon.WeaponEvent(this.OnWeaponChanged);
      PlayerSpells.OnCurseChanged -= new PlayerSpells.CurseEvent(this.OnCurseChanged);
      PlayerRelic.OnRelicChanged -= new PlayerRelic.RelicEvent(this.OnRelicChanged);
    }
    else
      CoopManager.PreventWeaponSpawn = true;
  }

  public bool GetAllPlayersWearingWeapons()
  {
    if (PlayerFarming.players.Count <= 0)
      return false;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (player.currentWeapon == EquipmentType.None && !PlayerFleeceManager.FleeceSwapsWeaponForCurse() || player.currentCurse == EquipmentType.None && DataManager.Instance.EnabledSpells)
        return false;
    }
    return true;
  }

  public void SetAllowResummonWeapon(bool allow) => this.AllowResummonWeapon = allow;

  public Transform GetPickupTransformParent()
  {
    return !((UnityEngine.Object) BiomeGenerator.Instance.CurrentRoom.generateRoom.transform != (UnityEngine.Object) null) ? this.transform.parent : BiomeGenerator.Instance.CurrentRoom.generateRoom.transform;
  }

  public Transform GetContainerTransform() => this.Container.transform;

  public void SetWeaponInactive()
  {
    this.WeaponTaken = true;
    this.Lighting.SetActive(false);
    this.IconSpriteRenderer.enabled = false;
    this.podiumOn.SetActive(false);
    this.podiumOff.SetActive(true);
    this.particleEffect.Stop();
    this.Interactable = false;
  }

  [CompilerGenerated]
  public void \u003COnPlayerJoined\u003Eb__66_0() => this.CoopPodium.RemoveIfNotFirstLayer = true;

  [CompilerGenerated]
  public void \u003CIndicateHighlighted\u003Eb__91_0()
  {
    this._weaponPickupUI = (UIWeaponPickupPromptController) null;
  }

  [CompilerGenerated]
  public void \u003COnSecondaryInteract\u003Eb__99_0()
  {
    this.ResetRandom(true, this.WeaponLevel - 1);
    this.WeaponTaken = true;
  }

  [CompilerGenerated]
  public void \u003COnSecondaryInteract\u003Eb__99_1()
  {
    this.WeaponTaken = false;
    this.HasChanged = true;
    this.activated = false;
  }

  public enum Types
  {
    Random,
    Weapon,
    Curse,
    Relic,
  }

  [Serializable]
  public class WeaponIcons
  {
    public TarotCards.Card Weapon;
    public Sprite Sprite;
  }
}
