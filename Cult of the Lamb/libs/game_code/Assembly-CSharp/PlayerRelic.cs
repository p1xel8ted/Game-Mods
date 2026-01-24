// Decompiled with JetBrains decompiler
// Type: PlayerRelic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using Lamb.UI;
using MMBiomeGeneration;
using MMRoomGeneration;
using MMTools;
using src.UI.Overlays.TutorialOverlay;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class PlayerRelic : MonoBehaviour
{
  [SerializeField]
  public SpriteRenderer relicIcon;
  [SerializeField]
  public ParticleSystem relicPuff;
  [SerializeField]
  public GameObject plane;
  [SerializeField]
  public AssetReferenceGameObject friendlyEnemy;
  [SerializeField]
  public string tarotProjectilePath;
  [SerializeField]
  public GameObject blackGoop;
  [SerializeField]
  public GameObject poison;
  [SerializeField]
  public VFXAbilitySequenceData coopRune;
  [SerializeField]
  public AssetReferenceGameObject[] spawnableEnemies;
  [SerializeField]
  public BiomeLightingSettings RedLightingSettings;
  [SerializeField]
  public OverrideLightingProperties overrideLightingProperties;
  [SerializeField]
  public IceyCoat_VFX_Manager iceCoatManager;
  [CompilerGenerated]
  public RelicData \u003CCurrentRelic\u003Ek__BackingField;
  public static bool TimeFrozen;
  public bool InvincibleFromRelic;
  public EventInstance loopingSound;
  public float previousChargeAmount;
  public static PlayerRelic.RelicEvent OnRelicChanged;
  public Vector3 relicIconOriginalPosition = new Vector3(0.0f, 0.806f, -1.468f);
  public DG.Tweening.Sequence animationSequence;
  public Coroutine animationCoroutine;
  public System.Action onDestroyWithSpawningAlly;
  public List<Health> enemies = new List<Health>();
  public bool UnlimitedFervour;
  public bool DoubleDamage;
  public float DamageMultiplier;
  public string relicFieryBloodTriggerSFX = "event:/dlc/relic/fireblood_trigger";
  public string relicFieryBloodTransformSFX = "event:/dlc/relic/fireblood_hearts_transform";
  public string relicIceBloodTriggerSFX = "event:/dlc/relic/iceblood_trigger";
  public string relicIceBloodTransformSFX = "event:/dlc/relic/iceblood_hearts_transform";
  public string relicFreezeAllSFX = "event:/relics/freeze_relic";
  public string relicPoisonAllSFX = "event:/relics/relic_poison";
  public string relicIgniteAllSFX = "event:/dlc/relic/igniteall_trigger";
  public string curseGoodChargeSFX = "event:/player/Curses/goop_charge";
  public string relicFireballRainImpactSFX = "event:/dlc/relic/fireballrain_impact";
  public string relicFireballRainTriggerSFX = "event:/dlc/relic/fireballrain_trigger";
  public string relicFireBurrowTriggerSFX = "event:/dlc/relic/fireburrow_trigger";
  public string relicFireBurrowLoopSFX = "event:/dlc/relic/fireburrow_loop";
  public string relicFireBurrowStopSFX = "event:/dlc/relic/fireburrow_stop";
  public string relicIceBurrowTriggerSFX = "event:/dlc/relic/iceburrow_trigger";
  public string relicIceBurrowLoopSFX = "event:/dlc/relic/iceburrow_loop";
  public string relicIceBurrowStopSFX = "event:/dlc/relic/iceburrow_stop";
  public string relicFlameFamiliarTriggerSFX = "event:/dlc/relic/flamefamiliar_trigger";
  public string relicFrozenGhostsTriggerSFX = "event:/dlc/relic/frozenghosts_trigger";
  public string relicIceyCoatLoopSFX = "event:/dlc/relic/icecoat_active_loop";
  public string relicIceSpikesTriggerSFX = "event:/dlc/relic/icespike_trigger";
  public string relicIceSpikesSingleBurstSFX = "event:/dlc/relic/icespike_burst_single";
  public string relicIceSpikesLoopSFX = "event:/dlc/relic/icespike_active_loop";
  public EventInstance burrowLoopInstance;
  public EventInstance iceyCoatLoopInstance;
  public EventInstance iceSpikesLoopInstance;
  [CompilerGenerated]
  public float \u003CPlayerScaleModifier\u003Ek__BackingField = 1f;
  public PlayerFarming playerFarming;
  public GameObject FieryTrailPrefab;
  public GameObject IceyTrailPrefab;
  public List<GameObject> FieryTrails = new List<GameObject>();
  public List<GameObject> IceyTrails = new List<GameObject>();
  public float DelayBetweenTrails = 0.2f;
  public float TrailsTimer;
  public GameObject t;
  public Vector3 previousSpawnPosition;
  public System.Action onEndBurrow;

  public RelicData CurrentRelic
  {
    get => this.\u003CCurrentRelic\u003Ek__BackingField;
    set => this.\u003CCurrentRelic\u003Ek__BackingField = value;
  }

  public float ChargedAmount
  {
    get => this.playerFarming.RelicChargeAmount;
    set
    {
      this.playerFarming.RelicChargeAmount = value;
      PlayerRelic.RelicEvent relicChargeModified = this.OnRelicChargeModified;
      if (relicChargeModified != null)
        relicChargeModified(this.CurrentRelic, this.playerFarming);
      if (!this.IsFullyCharged)
        return;
      this.playerFarming.RelicChargeAmount = float.MaxValue;
    }
  }

  public float RequiredChargeAmount
  {
    get
    {
      return !((UnityEngine.Object) this.CurrentRelic != (UnityEngine.Object) null) ? 0.0f : PlayerWeapon.GetDamage(this.CurrentRelic.DamageRequiredToCharge, this.playerFarming.currentWeaponLevel, this.playerFarming);
    }
  }

  public bool IsFullyCharged => (double) this.ChargedAmount >= (double) this.RequiredChargeAmount;

  public event PlayerRelic.RelicEvent OnRelicEquipped;

  public event PlayerRelic.RelicEvent OnRelicConsumed;

  public event PlayerRelic.RelicEvent OnRelicChargeModified;

  public event PlayerRelic.RelicEvent OnRelicUsed;

  public event PlayerRelic.RelicEvent OnRelicCantUse;

  public event PlayerRelic.RelicEvent OnSubRelicChanged;

  public float PlayerScaleModifier
  {
    get => this.\u003CPlayerScaleModifier\u003Ek__BackingField;
    set => this.\u003CPlayerScaleModifier\u003Ek__BackingField = value;
  }

  public void OnDisable()
  {
    AudioManager.Instance.ResumeForFreezeTime();
    System.Action onEndBurrow = this.onEndBurrow;
    if (onEndBurrow != null)
      onEndBurrow();
    AudioManager.Instance.StopLoop(this.iceSpikesLoopInstance);
  }

  public void Start()
  {
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.IncreaseChargedAmount);
    this.playerFarming = this.GetComponent<PlayerFarming>();
    if (this.playerFarming.currentRelicType != RelicType.None)
      this.EquipRelic(EquipmentManager.GetRelicData(this.playerFarming.currentRelicType), false);
    this.relicIcon.gameObject.SetActive(false);
  }

  public void OnDestroy()
  {
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.IncreaseChargedAmount);
    PlayerRelic.TimeFrozen = false;
    this.InvincibleFromRelic = false;
    this.UnlimitedFervour = false;
    System.Action withSpawningAlly = this.onDestroyWithSpawningAlly;
    if (withSpawningAlly != null)
      withSpawningAlly();
    AudioManager.Instance.StopLoop(this.loopingSound);
    if (!this.DoubleDamage)
      return;
    if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null && !TrinketManager.HasTrinket(TarotCards.Card.NoCorruption, this.playerFarming))
      this.playerFarming.health.DamageModifier /= 2f;
    this.DoubleDamage = false;
  }

  public void FadeRedIn()
  {
    LightingManager.Instance.inOverride = true;
    this.RedLightingSettings.overrideLightingProperties = this.overrideLightingProperties;
    LightingManager.Instance.overrideSettings = this.RedLightingSettings;
    LightingManager.Instance.transitionDurationMultiplier = 0.0f;
    LightingManager.Instance.UpdateLighting(true);
  }

  public void FadeRedAway()
  {
    LightingManager.Instance.inOverride = false;
    LightingManager.Instance.overrideSettings = (BiomeLightingSettings) null;
    LightingManager.Instance.transitionDurationMultiplier = 0.25f;
    LightingManager.Instance.lerpActive = false;
    LightingManager.Instance.UpdateLighting(true);
  }

  public void UseRelic(RelicType relicType, bool forceConsumableAnimation = false)
  {
    bool flag = true;
    Transform playerTransform = this.playerFarming.transform;
    if (TrinketManager.HasTrinket(TarotCards.Card.CorruptedHealForRelic, this.playerFarming) && !TrinketManager.IsCorruptedNegativeEffectNegated(TarotCards.Card.CorruptedHealForRelic, this.playerFarming) && (double) this.playerFarming.health.CurrentHP <= 1.0 && Health.team2.Count <= 0)
    {
      PlayerRelic.RelicEvent onRelicCantUse = this.OnRelicCantUse;
      if (onRelicCantUse == null)
        return;
      onRelicCantUse(EquipmentManager.GetRelicData(relicType), this.playerFarming);
    }
    else
    {
      switch (this.CurrentRelic.RelicSubType)
      {
        case RelicSubType.Any:
          AudioManager.Instance.PlayOneShot("event:/relics/relic_standard");
          break;
        case RelicSubType.Dammed:
          AudioManager.Instance.PlayOneShot("event:/relics/relic_damned");
          break;
        case RelicSubType.Blessed:
          AudioManager.Instance.PlayOneShot("event:/relics/relic_blessed");
          break;
      }
      if (this.CurrentRelic.RelicType != RelicType.UseRandomRelic && this.CurrentRelic.RelicType != RelicType.UseRandomRelic_Blessed && this.CurrentRelic.RelicType != RelicType.UseRandomRelic_Dammed)
        EquipmentManager.NextRandomRelic = RelicType.None;
      AudioManager.Instance.ToggleFilter("blessed", false);
      AudioManager.Instance.ToggleFilter("dammed", false);
      VFXSequence sequence;
      float duration;
      switch (relicType)
      {
        case RelicType.LightningStrike:
        case RelicType.LightningStrike_Dammed:
        case RelicType.LightningStrike_Blessed:
          this.LightningStrike();
          break;
        case RelicType.DestroyTarotDealDamge:
        case RelicType.DestroyTarotDealDamge_Dammed:
        case RelicType.DestroyTarotDealDamge_Blessed:
          if (this.playerFarming.RunTrinkets.Count <= 0)
          {
            PlayerRelic.RelicEvent onRelicCantUse = this.OnRelicCantUse;
            if (onRelicCantUse == null)
              return;
            onRelicCantUse(EquipmentManager.GetRelicData(relicType), this.playerFarming);
            return;
          }
          this.DestroyTarotsDealDamage(TrinketManager.GetRelicDamageMultiplier(this.playerFarming), int.MaxValue, relicType == RelicType.DestroyTarotDealDamge_Blessed ? 0.1f : 0.0f, relicType == RelicType.DestroyTarotDealDamge_Dammed ? 0.1f : 0.0f);
          break;
        case RelicType.IncreaseDamageForDuration:
          VFXSequence damageSequence = this.CurrentRelic.VFXData.PlayNewSequence(playerTransform, new Transform[1]
          {
            playerTransform
          });
          AudioManager.Instance.StopLoop(this.loopingSound);
          this.loopingSound = AudioManager.Instance.CreateLoop("event:/relics/increase_damage_for_duration", this.gameObject, true);
          DataManager.Instance.PLAYER_RUN_DAMAGE_LEVEL += 0.75f;
          GameManager.GetInstance().WaitForSeconds(10f, (System.Action) (() =>
          {
            foreach (VFXObject impactVfxObject in damageSequence.ImpactVFXObjects)
              impactVfxObject.StopVFX();
            AudioManager.Instance.StopLoop(this.loopingSound);
            DataManager.Instance.PLAYER_RUN_DAMAGE_LEVEL -= 0.75f;
          }));
          break;
        case RelicType.SpawnDemon:
        case RelicType.SpawnDemon_Dammed:
        case RelicType.SpawnDemon_Blessed:
          this.StartCoroutine((IEnumerator) this.Delay(1f, true, (System.Action) (() => AudioManager.Instance.PlayOneShot("event:/relics/demon_bubble", this.gameObject))));
          int level = 1;
          if (relicType == RelicType.SpawnDemon_Dammed)
            level = UnityEngine.Random.Range(2, 4);
          else if (relicType == RelicType.SpawnDemon_Blessed)
            this.SpawnDemon(1);
          this.SpawnDemon(level);
          break;
        case RelicType.DestroyTarotGainBuff:
        case RelicType.DestroyTarotGainBuff_Dammed:
        case RelicType.DestroyTarotGainBuff_Blessed:
          PlayerRelic.BonusType bonusType = relicType == RelicType.DestroyTarotGainBuff ? PlayerRelic.BonusType.GainStrength : PlayerRelic.BonusType.GainBlueHearts;
          if (relicType == RelicType.DestroyTarotGainBuff_Dammed)
            bonusType = PlayerRelic.BonusType.GainBlackHearts;
          if (this.playerFarming.RunTrinkets.Count <= 0)
          {
            PlayerRelic.RelicEvent onRelicCantUse = this.OnRelicCantUse;
            if (onRelicCantUse == null)
              return;
            onRelicCantUse(EquipmentManager.GetRelicData(relicType), this.playerFarming);
            return;
          }
          this.StartCoroutine((IEnumerator) this.DestroyTarotsGainStrength(bonusType, this.playerFarming));
          break;
        case RelicType.FiftyFiftyGamble:
        case RelicType.FiftyFiftyGamble_Dammed:
        case RelicType.FiftyFiftyGamble_Blessed:
        case RelicType.EnemyFiftyFifty_Corrupted:
          this.playerFarming.health.untouchable = true;
          this.FiftyFiftyGamble(relicType);
          break;
        case RelicType.SpawnBombs:
          sequence = EquipmentManager.GetRelicData(relicType).VFXData.PlayNewSequence(playerTransform, new Transform[1]
          {
            this.transform
          });
          GameManager.GetInstance().WaitForSeconds(0.5f, (System.Action) (() => BiomeGenerator.SpawnBombsInRoom(UnityEngine.Random.Range(15, 25), false, this.playerFarming, TrinketManager.GetRelicDamageMultiplier(this.playerFarming))));
          break;
        case RelicType.GungeonBlank:
        case RelicType.GungeonBlank_Dammed:
        case RelicType.GungeonBlank_Blessed:
          duration = 8f;
          if (relicType == RelicType.GungeonBlank_Blessed)
            duration = 12f;
          else if (relicType == RelicType.GungeonBlank_Dammed)
          {
            duration = 6f;
            this.playerFarming.EnableDamageOnTouchCollider(duration * 1.25f);
          }
          else if (DataManager.Instance.ShamuraHealed)
            duration = 10f;
          this.playerFarming.playerController.MakeUntouchable(duration * 1.25f);
          this.InvincibleFromRelic = true;
          VFXSequence sequence1 = EquipmentManager.GetRelicData(relicType).VFXData.PlayNewSequence(playerTransform, new Transform[1]
          {
            playerTransform
          });
          GameManager.GetInstance().StartCoroutine((IEnumerator) this.VFXTimer(duration * 1.25f, sequence1));
          break;
        case RelicType.FreezeAll:
        case RelicType.PoisonAll:
        case RelicType.IgniteAll:
          VFXSequence freezePoisonAllSequence = this.CurrentRelic.VFXData.PlayNewSequence(playerTransform, new Transform[1]
          {
            playerTransform
          });
          freezePoisonAllSequence.OnImpact += (Action<VFXObject, int>) ((vfxObject1, impactVFXIndex1) =>
          {
            freezePoisonAllSequence.OnImpact -= (Action<VFXObject, int>) ((vfxObject2, impactVFXIndex2) =>
            {
              // ISSUE: unable to decompile the method.
            });
            string soundPath = "";
            switch (relicType)
            {
              case RelicType.FreezeAll:
                soundPath = this.relicFreezeAllSFX;
                AudioManager.Instance.ToggleFilter("freeze", true);
                AudioManager.Instance.PlayOneShot(this.curseGoodChargeSFX, this.gameObject);
                break;
              case RelicType.PoisonAll:
                soundPath = this.relicPoisonAllSFX;
                AudioManager.Instance.PlayOneShot(this.curseGoodChargeSFX, this.gameObject);
                break;
            }
            if (relicType != RelicType.IgniteAll)
              AudioManager.Instance.PlayOneShot(soundPath, this.gameObject);
            GameManager.GetInstance().WaitForSeconds(0.2f, (System.Action) (() =>
            {
              for (int index = Health.team2.Count - 1; index >= 0; --index)
              {
                if (!((UnityEngine.Object) Health.team2[index] == (UnityEngine.Object) null) && Health.team2[index].gameObject.activeInHierarchy)
                {
                  Health health = Health.team2[index];
                  switch (relicType)
                  {
                    case RelicType.FreezeAll:
                      if (!((UnityEngine.Object) health == (UnityEngine.Object) null))
                      {
                        float duration2;
                        switch (DifficultyManager.PrimaryDifficulty)
                        {
                          case DifficultyManager.Difficulty.Hard:
                            duration2 = 15f;
                            break;
                          case DifficultyManager.Difficulty.ExtraHard:
                            duration2 = 10f;
                            break;
                          default:
                            duration2 = 20f;
                            break;
                        }
                        health.AddIce(duration2);
                        AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile", this.gameObject);
                        BiomeConstants.Instance.EmitSmokeExplosionVFX(health.transform.position, Color.cyan);
                        continue;
                      }
                      continue;
                    case RelicType.PoisonAll:
                      if (!((UnityEngine.Object) health == (UnityEngine.Object) null))
                      {
                        health.AddPoison(this.gameObject, 10f);
                        AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile", this.gameObject);
                        BiomeConstants.Instance.EmitSmokeExplosionVFX(health.transform.position, Color.green);
                        continue;
                      }
                      continue;
                    default:
                      if (relicType == RelicType.IgniteAll && !((UnityEngine.Object) health == (UnityEngine.Object) null))
                      {
                        health.AddBurn(this.gameObject, UnityEngine.Random.Range(8f, 12f));
                        BiomeConstants.Instance.EmitSmokeExplosionVFX(health.transform.position, Color.red);
                        continue;
                      }
                      continue;
                  }
                }
              }
              AudioManager.Instance.ToggleFilter("freeze", false);
            }));
          });
          if (relicType == RelicType.IgniteAll)
          {
            AudioManager.Instance.PlayOneShot(this.relicIgniteAllSFX, this.gameObject);
            break;
          }
          break;
        case RelicType.Shrink:
        case RelicType.Enlarge:
          BiomeConstants.Instance.PsychedelicFadeIn(0.25f, onComplete: (System.Action) (() => BiomeConstants.Instance.PsychedelicFadeOut(0.5f, 1f, onComplete: (System.Action) (() =>
          {
            AudioManager.Instance.SetMusicPsychedelic(0.0f);
            this.playerFarming.playerController.ToggleGhost(false);
          }))));
          AudioManager.Instance.SetMusicPsychedelic(1f);
          BiomeConstants.Instance.EmitSmokeExplosionVFX(playerTransform.position, Color.cyan);
          CameraManager.instance.ShakeCameraForDuration(1f, 1.25f, 0.5f);
          sequence = EquipmentManager.GetRelicData(relicType).VFXData.PlayNewSequence(playerTransform, new Transform[1]
          {
            this.transform
          });
          this.PlayerScaleModifier = 1f;
          this.playerFarming.playerController.ToggleGhost(true);
          Vector3 localScale = playerTransform.localScale;
          if (relicType == RelicType.Shrink)
          {
            AudioManager.Instance.PlayOneShot("event:/relics/shrink", this.playerFarming.gameObject);
            --this.PlayerScaleModifier;
          }
          else if (relicType == RelicType.Enlarge)
          {
            AudioManager.Instance.PlayOneShot("event:/relics/enlarge", this.playerFarming.gameObject);
            ++this.PlayerScaleModifier;
          }
          Vector3 endValue = Vector3.one * Mathf.Lerp(0.66f, 1.5f, this.PlayerScaleModifier / 2f);
          this.transform.DOKill();
          this.transform.DOScale(endValue, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce);
          break;
        case RelicType.HeartConversion_Dammed:
        case RelicType.HeartConversion_Blessed:
          if ((double) this.playerFarming.health.BlueHearts == 0.0)
          {
            PlayerRelic.RelicEvent onRelicCantUse = this.OnRelicCantUse;
            if (onRelicCantUse == null)
              return;
            onRelicCantUse(EquipmentManager.GetRelicData(relicType), this.playerFarming);
            return;
          }
          if (relicType == RelicType.HeartConversion_Blessed)
            AudioManager.Instance.PlayOneShot("event:/relics/heart_convert_blessed", this.gameObject);
          else
            AudioManager.Instance.PlayOneShot("event:/relics/heart_convert_dammed", this.gameObject);
          List<HUD_Heart> blueHeartsList = new List<HUD_Heart>();
          foreach (HUD_Heart heartIcon in this.playerFarming.hudHearts.HeartIcons)
          {
            if (heartIcon.MyHeartType == HUD_Heart.HeartType.Blue)
              blueHeartsList.Add(heartIcon);
          }
          List<GameObject> tempObjs = new List<GameObject>();
          for (int i = 0; (double) i < (double) this.playerFarming.health.BlueHearts; i += 2)
          {
            Camera main = Camera.main;
            Vector3 worldPoint1 = main.ScreenToWorldPoint(new Vector3(playerTransform.position.x, playerTransform.position.y, main.nearClipPlane));
            GameObject gameObject = new GameObject();
            tempObjs.Add(gameObject);
            gameObject.transform.position = worldPoint1;
            sequence = EquipmentManager.GetRelicData(relicType).VFXData.PlayNewSequence(gameObject.transform, new Transform[1]
            {
              this.transform
            });
            Vector3 position = blueHeartsList[i / 2].rectTransform.position;
            Vector3 worldPoint2 = main.ScreenToWorldPoint(new Vector3(position.x, position.y, 10f));
            Transform transform = sequence.ImpactVFXObjects[0].gameObject.transform;
            transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
            float delay = (float) ((double) i / 2.0 * 0.5);
            transform.DOMove(worldPoint2, 1.5f - delay).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(delay).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
            {
              blueHeartsList[i / 2].transform.DOKill();
              blueHeartsList[i / 2].transform.DOPunchScale(Vector3.one, 0.5f);
            }));
          }
          this.StartCoroutine((IEnumerator) this.Delay(1.5f, true, (System.Action) (() =>
          {
            for (int index = tempObjs.Count - 1; index >= 0; --index)
              UnityEngine.Object.Destroy((UnityEngine.Object) tempObjs[index]);
            if (relicType == RelicType.HeartConversion_Blessed && (double) this.playerFarming.health.BlueHearts > 0.0)
            {
              float blueHearts = this.playerFarming.health.BlueHearts;
              this.playerFarming.health.BlueHearts = 0.0f;
              this.playerFarming.health.TotalSpiritHearts += blueHearts;
              BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "red", "burst_big");
              AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", playerTransform.position);
            }
            else
            {
              if (relicType != RelicType.HeartConversion_Dammed || (double) this.playerFarming.health.BlueHearts <= 0.0)
                return;
              float blueHearts = this.playerFarming.health.BlueHearts;
              this.playerFarming.health.BlueHearts = 0.0f;
              this.playerFarming.health.BlackHearts += blueHearts * 2f;
              BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "black", "burst_big");
              AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", playerTransform.position);
            }
          })));
          break;
        case RelicType.RerollWeapon:
        case RelicType.RerollCurse:
          sequence = EquipmentManager.GetRelicData(relicType).VFXData.PlayNewSequence(playerTransform, new Transform[1]
          {
            this.transform
          });
          if (relicType == RelicType.RerollWeapon)
            sequence.OnComplete += (System.Action) (() =>
            {
              ++this.playerFarming.currentWeaponLevel;
              DataManager.Instance.CurrentRunWeaponLevel = this.playerFarming.currentWeaponLevel;
              sequence.OnComplete -= (System.Action) (() =>
              {
                // ISSUE: unable to decompile the method.
              });
              this.playerFarming.playerWeapon.SetWeapon(DataManager.Instance.GetRandomWeaponInPool(), this.playerFarming.currentWeaponLevel);
              AudioManager.Instance.PlayOneShot("event:/player/weapon_equip", this.transform.position);
              AudioManager.Instance.PlayOneShot("event:/player/weapon_unlocked", this.transform.position);
              this.playerFarming.TimedAction(1.6f, (System.Action) null, this.playerFarming.CurrentWeaponInfo.WeaponData.PickupAnimationKey);
            });
          if (relicType == RelicType.RerollCurse)
          {
            sequence.OnComplete += (System.Action) (() =>
            {
              ++this.playerFarming.currentCurseLevel;
              DataManager.Instance.CurrentRunCurseLevel = this.playerFarming.currentCurseLevel;
              sequence.OnComplete -= (System.Action) (() =>
              {
                // ISSUE: unable to decompile the method.
              });
              this.playerFarming.playerSpells.SetSpell(DataManager.Instance.GetRandomCurseInPool(), this.playerFarming.currentCurseLevel);
              this.playerFarming.TimedAction(1.3f, (System.Action) null, "Curses/curse-get");
              this.playerFarming.playerSpells.faithAmmo.Reload();
              AudioManager.Instance.PlayOneShot("event:/player/absorb_curse", this.gameObject);
              this.relicIcon.DOKill();
              if (this.animationSequence != null)
                this.animationSequence.Complete();
              this.relicIcon.gameObject.SetActive(true);
              this.relicIcon.transform.localPosition = new Vector3(-1.25f * (float) this.playerFarming.simpleSpineAnimator.Dir, 0.0f, -0.2f);
              this.relicIcon.transform.localScale = Vector3.one * 0.0f;
              this.relicIcon.transform.DOShakePosition(2f, 0.25f);
              this.relicIcon.transform.DOShakeRotation(2f, new Vector3(0.0f, 0.0f, 15f));
              this.animationSequence = DOTween.Sequence();
              this.animationSequence.Append((Tween) this.relicIcon.transform.DOScale(Vector3.one * 1.2f, 0.3f));
              this.animationSequence.Append((Tween) this.relicIcon.transform.DOScale(Vector3.one * 0.8f, 0.3f));
              this.animationSequence.Append((Tween) this.relicIcon.transform.DOScale(Vector3.one * 1.2f, 0.3f));
              this.animationSequence.Append((Tween) this.relicIcon.transform.DOScale(Vector3.one * 0.0f, 0.3f));
              this.animationSequence.AppendCallback((TweenCallback) (() => this.relicIcon.gameObject.SetActive(false)));
              this.relicIcon.sprite = EquipmentManager.GetCurseData(this.playerFarming.currentCurse).WorldSprite;
            });
            break;
          }
          break;
        case RelicType.InstantlyKillModifiedEnemies:
          for (int index = Health.team2.Count - 1; index >= 0; --index)
          {
            if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null && Health.team2[index].gameObject.activeSelf && (UnityEngine.Object) Health.team2[index].GetComponent<UnitObject>() != (UnityEngine.Object) null && Health.team2[index].GetComponent<UnitObject>().HasModifier)
              Health.team2[index].DealDamage(Health.team2[index].HP, this.gameObject, Health.team2[index].transform.position, AttackFlags: Health.AttackFlags.DoesntChargeRelics);
          }
          break;
        case RelicType.DealDamagePerFollower:
        case RelicType.DealDamagePerFollower_Blessed:
        case RelicType.DealDamagePerFollower_Dammed:
        case RelicType.FrozenGhosts:
          float damage1 = 0.0f;
          if (relicType == RelicType.FrozenGhosts)
          {
            for (int index = 0; index < DataManager.Instance.Followers_Dead.Count; ++index)
              damage1 += DataManager.Instance.Followers_Dead[index].FrozeToDeath ? 4f : 0.0f;
            if ((double) damage1 > 0.0)
              AudioManager.Instance.PlayOneShot(this.relicFrozenGhostsTriggerSFX, this.gameObject);
          }
          else
          {
            for (int index = 0; index < DataManager.Instance.Followers.Count; ++index)
            {
              if (relicType == RelicType.DealDamagePerFollower_Blessed)
                damage1 += DataManager.Instance.Followers[index].CursedState == Thought.OldAge ? 2f : 0.0f;
              else if (relicType == RelicType.DealDamagePerFollower_Dammed)
                damage1 += DataManager.Instance.Followers[index].CursedState == Thought.Dissenter ? 4f : 0.0f;
              else
                damage1 += 0.2f;
            }
          }
          if ((double) damage1 <= 0.0)
          {
            AudioManager.Instance.PlayOneShot("event:/dlc/relic/shared_cantuse", this.transform.position);
            PlayerRelic.RelicEvent onRelicCantUse = this.OnRelicCantUse;
            if (onRelicCantUse == null)
              return;
            onRelicCantUse(EquipmentManager.GetRelicData(relicType), this.playerFarming);
            return;
          }
          this.DealDamagePerFollower(damage1);
          break;
        case RelicType.HealPerFollower:
        case RelicType.HealPerFollower_Blessed:
        case RelicType.HealPerFollower_Dammed:
          int followersTotal = 0;
          foreach (FollowerInfo follower in DataManager.Instance.Followers)
          {
            if (relicType == RelicType.HealPerFollower_Blessed && follower.CursedState == Thought.OldAge || relicType == RelicType.HealPerFollower_Dammed && follower.CursedState == Thought.Dissenter || relicType == RelicType.HealPerFollower)
              ++followersTotal;
          }
          if (relicType == RelicType.HealPerFollower_Blessed && followersTotal <= 0 || relicType == RelicType.HealPerFollower_Dammed && followersTotal <= 0 || relicType == RelicType.HealPerFollower && followersTotal < 5)
          {
            PlayerRelic.RelicEvent onRelicCantUse = this.OnRelicCantUse;
            if (onRelicCantUse == null)
              return;
            onRelicCantUse(EquipmentManager.GetRelicData(relicType), this.playerFarming);
            return;
          }
          this.HealPerFollower(relicType, followersTotal);
          break;
        case RelicType.SpawnCombatFollower:
          AudioManager.Instance.PlayOneShot("event:/relics/undead_impact", this.gameObject);
          this.FadeRedIn();
          sequence = EquipmentManager.GetRelicData(relicType).VFXData.PlayNewSequence(playerTransform, new Transform[1]
          {
            playerTransform
          });
          sequence.OnImpact += (Action<VFXObject, int>) ((obj, slot) =>
          {
            int num = 1;
            if (DataManager.Instance.KallamarHealed)
              num = 2;
            for (int index = 0; index < num; ++index)
              this.SpawnFriendlyEnemy(obj, slot);
          });
          sequence.OnImpact += (Action<VFXObject, int>) ((result, slot) =>
          {
            AudioManager.Instance.PlayOneShot("event:/relics/follower_impact", result.gameObject);
            this.StartCoroutine((IEnumerator) this.Delay(0.25f, true, (System.Action) (() => this.FadeRedAway())));
          });
          break;
        case RelicType.UseRandomRelic:
        case RelicType.UseRandomRelic_Blessed:
        case RelicType.UseRandomRelic_Dammed:
          RelicData currentRelic = this.CurrentRelic;
          this.CurrentRelic = EquipmentManager.GetRelicData(EquipmentManager.NextRandomRelic);
          this.UseRelic(this.CurrentRelic.RelicType);
          switch (relicType)
          {
            case RelicType.UseRandomRelic:
              EquipmentManager.PickRandomRelicData(true);
              break;
            case RelicType.UseRandomRelic_Blessed:
              EquipmentManager.PickRandomRelicData(true, RelicSubType.Blessed);
              break;
            case RelicType.UseRandomRelic_Dammed:
              EquipmentManager.PickRandomRelicData(true, RelicSubType.Dammed);
              break;
          }
          this.CurrentRelic = currentRelic;
          if (this.playerFarming.currentRelicType == RelicType.None)
            this.playerFarming.currentRelicType = currentRelic.RelicType;
          PlayerRelic.RelicEvent onSubRelicChanged1 = this.OnSubRelicChanged;
          if (onSubRelicChanged1 != null)
          {
            onSubRelicChanged1(this.CurrentRelic, this.playerFarming);
            break;
          }
          break;
        case RelicType.SpawnCombatFollowerFromBodies:
          int num1 = 5;
          List<Transform> transformList = new List<Transform>();
          List<DeadBodySliding> targetBodies = new List<DeadBodySliding>();
          List<DeadBodySliding> otherBodies = new List<DeadBodySliding>();
          foreach (DeadBodySliding deadBody in DeadBodySliding.DeadBodies)
          {
            if ((transformList.Count == 0 || (double) UnityEngine.Random.value < 1.0 / (double) DeadBodySliding.DeadBodies.Count) && transformList.Count < num1)
            {
              transformList.Add(deadBody.transform);
              targetBodies.Add(deadBody);
            }
            else
              otherBodies.Add(deadBody);
          }
          if (transformList.Count == 0)
          {
            PlayerRelic.RelicEvent onRelicCantUse = this.OnRelicCantUse;
            if (onRelicCantUse == null)
              return;
            onRelicCantUse(EquipmentManager.GetRelicData(relicType), this.playerFarming);
            return;
          }
          AudioManager.Instance.PlayOneShot("event:/relics/undead_impact", this.gameObject);
          this.FadeRedIn();
          int count = transformList.Count;
          sequence = EquipmentManager.GetRelicData(relicType).VFXData.PlayNewSequence(playerTransform, transformList.ToArray());
          sequence.OnImpact += new Action<VFXObject, int>(this.SpawnFriendlyEnemy);
          sequence.OnImpact += (Action<VFXObject, int>) ((result, slot) =>
          {
            AudioManager.Instance.PlayOneShot("event:/relics/follower_impact", result.gameObject);
            if (slot == count - 1)
              this.StartCoroutine((IEnumerator) this.Delay(0.5f, true, (System.Action) (() =>
              {
                this.StartCoroutine((IEnumerator) this.Delay(0.5f, true, (System.Action) (() =>
                {
                  foreach (DeadBodySliding deadBodySliding in otherBodies)
                  {
                    if ((UnityEngine.Object) deadBodySliding != (UnityEngine.Object) null)
                    {
                      deadBodySliding.OnDie();
                      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BONE, UnityEngine.Random.Range(2, 5), deadBodySliding.transform.position);
                    }
                  }
                })));
                this.FadeRedAway();
              })));
            this.StartCoroutine((IEnumerator) this.Delay(1f, true, (System.Action) (() =>
            {
              if (!((UnityEngine.Object) targetBodies[slot] != (UnityEngine.Object) null))
                return;
              targetBodies[slot].OnDie();
              InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BONE, UnityEngine.Random.Range(2, 5), targetBodies[slot].transform.position);
            })));
          });
          break;
        case RelicType.FillUpFervour:
          Debug.Log((object) "===========================  RelicType.FillUpFervour".Colour(Color.green));
          Vector3 vector3 = Vector3.back;
          UI_Transitions component = this.playerFarming.hudHearts.faithAmmo.GetComponent<UI_Transitions>();
          if (vector3 == Vector3.back)
            vector3 = component.transform.localPosition;
          if ((double) this.playerFarming.playerSpells.faithAmmo.Ammo >= (double) this.playerFarming.playerSpells.faithAmmo.Total)
          {
            component.transform.DOKill();
            component.transform.localPosition = vector3;
            component.transform.DOPunchPosition(new Vector3(1f, 0.0f, 0.0f), 0.5f);
            PlayerRelic.RelicEvent onRelicCantUse = this.OnRelicCantUse;
            if (onRelicCantUse == null)
              return;
            onRelicCantUse(EquipmentManager.GetRelicData(relicType), this.playerFarming);
            return;
          }
          sequence = EquipmentManager.GetRelicData(relicType).VFXData.PlayNewSequence(this.transform, new Transform[1]
          {
            this.transform
          });
          int num2 = 30;
          int num3 = -1;
          float num4 = 0.5f;
          while (++num3 < num2)
          {
            float Angle = 360f / (float) num2 * (float) num3;
            BlackSoul blackSoul = InventoryItem.SpawnBlackSoul(1, this.transform.position + new Vector3(num4 * Mathf.Cos(Angle * ((float) Math.PI / 180f)), num4 * Mathf.Sin(Angle * ((float) Math.PI / 180f))), simulated: true);
            if ((UnityEngine.Object) blackSoul != (UnityEngine.Object) null)
            {
              blackSoul.Delta = 0;
              blackSoul.Delay = 0.2f;
              blackSoul.SetAngle(Angle, new Vector2(10f, 10f));
              blackSoul.magnetiseDistance = 100f;
            }
          }
          GameManager.GetInstance().WaitForSeconds(0.75f, (System.Action) (() => this.playerFarming.playerSpells.faithAmmo.Reload()));
          break;
        case RelicType.DamageBoss:
          for (int index = Health.team2.Count - 1; index >= 0; --index)
          {
            if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null && (UnityEngine.Object) Health.team2[index].GetComponent<UnitObject>() != (UnityEngine.Object) null && Health.team2[index].GetComponent<UnitObject>().IsBoss)
              Health.team2[index].DealDamage(PlayerWeapon.GetDamage(20f, this.playerFarming.currentWeaponLevel, this.playerFarming), this.gameObject, this.transform.position, dealDamageImmediately: true, AttackFlags: Health.AttackFlags.DoesntChargeRelics);
          }
          break;
        case RelicType.SpawnTentacle:
          sequence = EquipmentManager.GetRelicData(relicType).VFXData.PlayNewSequence(this.transform, new Transform[1]
          {
            this.transform
          });
          duration = 60f;
          Tentacle t = UnityEngine.Object.Instantiate<GameObject>(EquipmentManager.GetCurseData(EquipmentType.TENTACLE_TAROT_REF).Prefab, (UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null ? GenerateRoom.Instance.transform : this.transform.parent, true).GetComponent<Tentacle>();
          t.ShootsProjectiles = true;
          t.TimeBetweenProjectiles = 10f;
          t.transform.position = this.transform.position;
          t.AttackFlags = Health.AttackFlags.DoesntChargeRelics;
          t.GetComponent<Health>().enabled = false;
          GameObject gameObject1 = UnityEngine.Object.Instantiate<GameObject>(EquipmentManager.GetCurseData(EquipmentType.TENTACLE_TAROT_REF).SecondaryPrefab, (UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null ? GenerateRoom.Instance.transform : this.transform.parent);
          gameObject1.transform.position = this.transform.position - Vector3.right;
          gameObject1.GetComponent<FX_CrackController>().duration = duration + 0.5f;
          float damage2 = EquipmentManager.GetCurseData(EquipmentType.TENTACLE_TAROT_REF).Damage * PlayerSpells.GetCurseDamageMultiplier(this.playerFarming) * TrinketManager.GetRelicDamageMultiplier(this.playerFarming);
          t.Play(0.0f, duration, damage2, Health.Team.PlayerTeam, false, 0, true, true);
          CameraManager.instance.ShakeCameraForDuration(0.6f, 0.8f, 0.25f);
          AudioManager.Instance.PlayOneShot("event:/player/Curses/tentacles", t.gameObject);
          AudioManager.Instance.PlayOneShot("event:/material/stone_break", this.gameObject);
          AudioManager.Instance.PlayOneShot("event:/followers/break_free", this.gameObject);
          BiomeConstants.Instance.EmitParticleChunk(BiomeConstants.TypeOfParticle.stone, t.transform.position, Vector3.one, 5);
          GameManager.GetInstance().WaitForSeconds(duration + 0.5f, (System.Action) (() =>
          {
            AudioManager.Instance.PlayOneShot("event:/material/stone_break", this.gameObject);
            AudioManager.Instance.PlayOneShot("event:/followers/break_free", this.gameObject);
            CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.2f);
            BiomeConstants.Instance.EmitSmokeExplosionVFX(t.transform.position);
            BiomeConstants.Instance.EmitParticleChunk(BiomeConstants.TypeOfParticle.stone, t.transform.position, Vector3.one, 10);
          }));
          break;
        case RelicType.ProjectileRing:
          sequence = EquipmentManager.GetRelicData(relicType).VFXData.PlayNewSequence(this.transform, new Transform[1]
          {
            this.transform
          });
          int amount = this.playerFarming.RunTrinkets.Count * 3;
          if (amount == 0)
            amount = 2;
          Projectile.CreatePlayerProjectiles(amount, (Health) this.playerFarming.health, this.transform.position, this.tarotProjectilePath, 16f, PlayerWeapon.GetDamage(4f, this.playerFarming.currentWeaponLevel, this.playerFarming) * TrinketManager.GetRelicDamageMultiplier(this.playerFarming), explosive: true, attackFlags: Health.AttackFlags.DoesntChargeRelics);
          CameraManager.instance.ShakeCameraForDuration(0.6f, 0.8f, 0.25f);
          AudioManager.Instance.PlayOneShot("event:/player/Curses/goop_shot", this.gameObject);
          break;
        case RelicType.RandomEnemyIntoCritter:
          List<Health> source = new List<Health>();
          for (int index = Health.team2.Count - 1; index >= 0; --index)
          {
            if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null && (double) Health.team2[index].DamageModifier >= 1.0 && (UnityEngine.Object) Health.team2[index].protector == (UnityEngine.Object) null && !Health.team2[index].HasShield && Health.team2[index].CanBeTurnedIntoCritter && Health.team2[index].gameObject.activeSelf && (UnityEngine.Object) Health.team2[index].GetComponent<UnitObject>() != (UnityEngine.Object) null && !Health.team2[index].GetComponent<UnitObject>().IsBoss)
              source.Add(Health.team2[index]);
          }
          if (source.Count == 0)
          {
            PlayerRelic.RelicEvent onRelicCantUse = this.OnRelicCantUse;
            if (onRelicCantUse == null)
              return;
            onRelicCantUse(EquipmentManager.GetRelicData(relicType), this.playerFarming);
            return;
          }
          UnitObject enemy = source.OrderByDescending<Health, float>((Func<Health, float>) (x => x.HP)).ToList<Health>()[0].GetComponent<UnitObject>();
          sequence = EquipmentManager.GetRelicData(relicType).VFXData.PlayNewSequence(this.transform, new Transform[1]
          {
            enemy.transform
          });
          AudioManager.Instance.PlayOneShot("event:/relics/rainbow_bubble", enemy.gameObject);
          GameManager.GetInstance().WaitForSeconds(0.1f, (System.Action) (() =>
          {
            BiomeGenerator.Instance.CurrentRoom.generateRoom.TurnEnemyIntoCritter(enemy);
            AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/staff_magic", this.gameObject);
          }));
          break;
        case RelicType.TeleportToBoss:
          this.StartCoroutine((IEnumerator) this.TeleportToBossRoom());
          break;
        case RelicType.RandomTeleport:
          if (!BiomeGenerator.Instance.CurrentRoom.IsBoss && BiomeGenerator.Instance.CurrentRoom.y != 999 && (UnityEngine.Object) DungeonLeaderMechanics.Instance == (UnityEngine.Object) null)
          {
            this.StartCoroutine((IEnumerator) this.TeleportToRandomRoom());
            break;
          }
          break;
        case RelicType.DamageOnTouch_Familiar:
        case RelicType.FreezeOnTouch_Familiar:
        case RelicType.PoisonOnTouch_Familiar:
        case RelicType.DamageEye_Coop:
        case RelicType.IgniteOnTouch_Familiar:
          if (relicType == RelicType.IgniteOnTouch_Familiar)
            AudioManager.Instance.PlayOneShot(this.relicFlameFamiliarTriggerSFX, this.gameObject);
          else
            AudioManager.Instance.PlayOneShot("event:/relics/relic_runedraw_long", this.gameObject);
          int num5 = 2;
          string key = "Assets/Prefabs/Familiars/Damage Familiar.prefab";
          if (relicType == RelicType.FreezeOnTouch_Familiar)
            key = "Assets/Prefabs/Familiars/Freeze Familiar.prefab";
          else if (relicType == RelicType.IgniteOnTouch_Familiar)
            key = "Assets/Prefabs/Familiars/Flame Familiar.prefab";
          else if (relicType == RelicType.PoisonOnTouch_Familiar)
            key = "Assets/Prefabs/Familiars/Poison Familiar.prefab";
          else if (relicType == RelicType.DamageEye_Coop)
          {
            key = "Assets/Prefabs/Familiars/Coop Damage Familiar.prefab";
            num5 = 1;
            this.PlayCOOPRune();
          }
          List<Transform> targets = new List<Transform>();
          for (int index = 0; index < num5; ++index)
            Addressables_wrapper.InstantiateAsync((object) key, playerTransform.position, Quaternion.identity, this.transform.parent, (Action<AsyncOperationHandle<GameObject>>) (o =>
            {
              Familiar familiar = o.Result.GetComponent<Familiar>();
              familiar.SetMaster(this.playerFarming);
              familiar.gameObject.SetActive(false);
              familiar.enabled = false;
              familiar.Container.transform.localScale = Vector3.zero;
              familiar.SetDirection(targets.Count == 0 ? 1 : -1);
              if (relicType == RelicType.DamageOnTouch_Familiar && DataManager.Instance.LeshyHealed)
                familiar.DamageMultiplier = 2f;
              Vector3 position = o.Result.transform.position;
              GameManager.GetInstance().WaitForSeconds(0.6f, (System.Action) (() =>
              {
                familiar.gameObject.SetActive(true);
                familiar.Container.transform.DOScale(0.25f, 0.75f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => familiar.GetComponentInChildren<DOTweenAnimation>().DOPlay()));
                GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() => familiar.enabled = true));
              }));
              targets.Add(familiar.transform);
              if (targets.Count < 2 && relicType != RelicType.DamageEye_Coop)
                return;
              sequence = EquipmentManager.GetRelicData(relicType).VFXData.PlayNewSequence(playerTransform, targets.ToArray());
            }));
          break;
        case RelicType.ShootCurses_Familiar:
          GameObject tempObj = new GameObject();
          tempObj.transform.position = this.transform.position + ((double) UnityEngine.Random.value > 0.5 ? Vector3.right : Vector3.left);
          Addressables_wrapper.InstantiateAsync((object) "Assets/Prefabs/Familiars/Curse Familiar.prefab", tempObj.transform.position - Vector3.forward, Quaternion.identity, this.transform.parent, (Action<AsyncOperationHandle<GameObject>>) (o =>
          {
            Familiar familiar = o.Result.GetComponent<Familiar>();
            familiar.SetMaster(this.playerFarming);
            familiar.gameObject.SetActive(false);
            familiar.GetComponentInChildren<DOTweenAnimation>().DOPlay();
            sequence = EquipmentManager.GetRelicData(relicType).VFXData.PlayNewSequence(playerTransform, new Transform[1]
            {
              tempObj.transform
            });
            GameManager.GetInstance().WaitForSeconds(0.8f, (System.Action) (() => familiar.gameObject.SetActive(true)));
            sequence.OnComplete += (System.Action) (() => UnityEngine.Object.Destroy((UnityEngine.Object) tempObj));
          }));
          break;
        case RelicType.FreezeTime:
          this.FreezeTime(this.CurrentRelic.VFXData.PlayNewSequence(this.playerFarming.transform, new Transform[1]
          {
            this.playerFarming.transform
          }));
          break;
        case RelicType.SpawnBlackGoop:
          this.CurrentRelic.VFXData.PlayNewSequence(playerTransform, new Transform[1]
          {
            playerTransform
          });
          this.StartCoroutine((IEnumerator) this.Delay(0.25f, true, (System.Action) (() =>
          {
            for (int index = 0; index < 20; ++index)
            {
              Vector2 vector2 = UnityEngine.Random.insideUnitCircle * 2f;
              float num6 = UnityEngine.Random.Range(0.2f, 2f);
              if (index == 0)
              {
                vector2 *= 0.0f;
                num6 = 1f;
              }
              GameObject gameObject2 = ObjectPool.Spawn(this.blackGoop, BiomeGenerator.Instance.CurrentRoom.generateRoom.transform, this.transform.position + (Vector3) vector2, Quaternion.identity);
              gameObject2.transform.localScale = new Vector3(num6, num6, 1f);
              if ((bool) (UnityEngine.Object) gameObject2.GetComponent<TrapGoop>())
              {
                gameObject2.GetComponent<TrapGoop>().DamageMultiplier = PlayerWeapon.GetDamage(5f, this.playerFarming.currentWeaponLevel, this.playerFarming) * TrinketManager.GetRelicDamageMultiplier(this.playerFarming);
                gameObject2.GetComponent<TrapGoop>().TickDurationMultiplier = 0.5f;
              }
              RaycastHit hitInfo;
              if (UnityEngine.Physics.Raycast(gameObject2.transform.position - Vector3.forward * 2f, Vector3.forward, out hitInfo, float.PositiveInfinity) && (UnityEngine.Object) hitInfo.collider.gameObject.GetComponent<MeshCollider>() != (UnityEngine.Object) null)
                gameObject2.transform.position = new Vector3(gameObject2.transform.position.x, gameObject2.transform.position.y, hitInfo.point.z);
            }
          })));
          break;
        case RelicType.KillNonBossEnemies:
          this.KillNonBossEnemies();
          break;
        case RelicType.UnlimitedFervour:
          this.playerFarming.playerSpells.faithAmmo.SetUnlimited(5f);
          this.CurrentRelic.VFXData.PlayNewSequence(playerTransform, new Transform[1]
          {
            playerTransform
          });
          this.UnlimitedFervour = true;
          GameManager.GetInstance().WaitForSeconds(5f, (System.Action) (() => this.UnlimitedFervour = false));
          break;
        case RelicType.DiseasedPoison_Corrupted:
          this.CurrentRelic.VFXData.PlayNewSequence(playerTransform, new Transform[1]
          {
            playerTransform
          });
          ++this.playerFarming.health.BlackHearts;
          if (!TrinketManager.HasTrinket(TarotCards.Card.NoCorruption, this.playerFarming))
          {
            for (int index = 0; index < 20; ++index)
            {
              Vector2 vector2 = UnityEngine.Random.insideUnitCircle * 2f;
              float num7 = UnityEngine.Random.Range(0.2f, 2f);
              if (index == 0)
              {
                vector2 *= 0.0f;
                num7 = 1f;
              }
              GameObject gameObject3 = ObjectPool.Spawn(this.poison, BiomeGenerator.Instance.CurrentRoom.generateRoom.transform, this.transform.position + (Vector3) vector2, Quaternion.identity);
              gameObject3.transform.localScale = new Vector3(num7, num7, 1f);
              RaycastHit hitInfo;
              if (UnityEngine.Physics.Raycast(gameObject3.transform.position - Vector3.forward * 2f, Vector3.forward, out hitInfo, float.PositiveInfinity) && (UnityEngine.Object) hitInfo.collider.gameObject.GetComponent<MeshCollider>() != (UnityEngine.Object) null)
                gameObject3.transform.position = new Vector3(gameObject3.transform.position.x, gameObject3.transform.position.y, hitInfo.point.z);
            }
            break;
          }
          break;
        case RelicType.Frozen_Corrupted:
          this.CurrentRelic.VFXData.PlayNewSequence(playerTransform, new Transform[1]
          {
            playerTransform
          });
          if (!TrinketManager.HasTrinket(TarotCards.Card.NoCorruption, this.playerFarming))
          {
            this.playerFarming.playerController.RunSpeed /= 4f;
            GameManager.GetInstance().StartCoroutine((IEnumerator) this.Delay(6f, true, (System.Action) (() => this.playerFarming.playerController.RunSpeed *= 4f)));
          }
          this.DamageMultiplier += 5f;
          GameManager.GetInstance().WaitForSeconds(6f, (System.Action) (() =>
          {
            this.DamageMultiplier -= 5f;
            AudioManager.Instance.PlayOneShot("event:/relics/chemach_leaves_whoosh");
          }));
          break;
        case RelicType.DeathForLife_Corrupted:
          List<FollowerBrain> possibleFollowers = new List<FollowerBrain>();
          foreach (FollowerInfo follower in DataManager.Instance.Followers)
          {
            if (!FollowerManager.FollowerLocked(in follower.ID))
              possibleFollowers.Add(FollowerBrain.GetOrCreateBrain(follower));
          }
          if (possibleFollowers.Count > 0)
          {
            this.CurrentRelic.VFXData.PlayNewSequence(playerTransform, new Transform[1]
            {
              playerTransform
            });
            this.StartCoroutine((IEnumerator) this.Delay(UnityEngine.Random.Range(0.5f, 1f), true, (System.Action) (() =>
            {
              if (!TrinketManager.HasTrinket(TarotCards.Card.NoCorruption, this.playerFarming))
                possibleFollowers[UnityEngine.Random.Range(0, possibleFollowers.Count)].Die(NotificationCentre.NotificationType.Died);
              this.playerFarming.health.FullHeal();
              BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "red", "burst_big");
              AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", this.playerFarming.transform.position);
            })));
            break;
          }
          PlayerRelic.RelicEvent onRelicCantUse1 = this.OnRelicCantUse;
          if (onRelicCantUse1 == null)
            return;
          onRelicCantUse1(EquipmentManager.GetRelicData(relicType), this.playerFarming);
          return;
        case RelicType.DiseasedHeart_Corrupted:
          this.DiseasedHeartDamageEnemies();
          break;
        case RelicType.CashForHearts_Corrupted:
          if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD) < 50)
          {
            PlayerRelic.RelicEvent onRelicCantUse2 = this.OnRelicCantUse;
            if (onRelicCantUse2 == null)
              return;
            onRelicCantUse2(EquipmentManager.GetRelicData(relicType), this.playerFarming);
            return;
          }
          this.CurrentRelic.VFXData.PlayNewSequence(playerTransform, new Transform[1]
          {
            playerTransform
          });
          this.StartCoroutine((IEnumerator) this.Delay(0.5f, true, (System.Action) (() =>
          {
            ++this.playerFarming.health.BlueHearts;
            BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "blue", "burst_big");
            AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", this.playerFarming.transform.position);
            if (!TrinketManager.HasTrinket(TarotCards.Card.NoCorruption, this.playerFarming))
              Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD, -50);
            for (int index = 0; index < 10; ++index)
            {
              PickUp pickup = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, this.playerFarming.transform.position, 0.0f);
              pickup.SetInitialSpeedAndDiraction(5f, (float) UnityEngine.Random.Range(0, 360));
              pickup.MagnetToPlayer = false;
              pickup.AddToInventory = false;
              pickup.CanBePickedUp = false;
              this.StartCoroutine((IEnumerator) this.Delay(UnityEngine.Random.Range(0.5f, 1f), true, (System.Action) (() => pickup.transform.DOScale(0.0f, 0.25f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) pickup.gameObject))))));
            }
          })));
          break;
        case RelicType.LoadoutSwap_Coop:
          this.PlayCOOPRune();
          this.CurrentRelic.VFXData.PlayNewSequence(playerTransform, new Transform[1]
          {
            playerTransform
          });
          this.StartCoroutine((IEnumerator) this.Delay(0.5f, true, (System.Action) (() =>
          {
            AudioManager.Instance.PlayOneShot("event:/player/weapon_equip", this.transform.position);
            AudioManager.Instance.PlayOneShot("event:/player/weapon_unlocked", this.transform.position);
            PlayerFarming playerFarming = (PlayerFarming) null;
            foreach (PlayerFarming player in PlayerFarming.players)
            {
              if ((UnityEngine.Object) player != (UnityEngine.Object) this.playerFarming)
                playerFarming = player;
            }
            EquipmentType weaponType = this.playerFarming.CurrentWeaponInfo.WeaponType;
            EquipmentType currentCurse = this.playerFarming.currentCurse;
            ++this.playerFarming.currentWeaponLevel;
            ++this.playerFarming.currentCurseLevel;
            this.playerFarming.currentWeaponLevel = playerFarming.currentWeaponLevel;
            this.playerFarming.currentCurseLevel = playerFarming.currentCurseLevel;
            if (DataManager.Instance.PlayerFleece != 6)
            {
              this.playerFarming.playerWeapon.SetWeapon(playerFarming.CurrentWeaponInfo.WeaponType, this.playerFarming.playerWeapon.CurrentWeaponLevel);
              this.playerFarming.TimedAction(1.6f, (System.Action) null, this.playerFarming.CurrentWeaponInfo.WeaponData.PickupAnimationKey);
            }
            this.playerFarming.playerSpells.SetSpell(playerFarming.currentCurse, this.playerFarming.currentCurseLevel);
            playerFarming.currentWeaponLevel = this.playerFarming.currentWeaponLevel;
            playerFarming.currentCurseLevel = this.playerFarming.currentCurseLevel;
            if (DataManager.Instance.PlayerFleece != 6)
            {
              playerFarming.playerWeapon.SetWeapon(weaponType, playerFarming.playerWeapon.CurrentWeaponLevel);
              playerFarming.TimedAction(1.6f, (System.Action) null, playerFarming.CurrentWeaponInfo.WeaponData.PickupAnimationKey);
            }
            playerFarming.playerSpells.SetSpell(currentCurse, playerFarming.currentCurseLevel);
          })));
          break;
        case RelicType.Sacrifice_Coop:
          if ((double) this.playerFarming.health.CurrentHP <= 1.0)
          {
            PlayerRelic.RelicEvent onRelicCantUse3 = this.OnRelicCantUse;
            if (onRelicCantUse3 == null)
              return;
            onRelicCantUse3(EquipmentManager.GetRelicData(relicType), this.playerFarming);
            return;
          }
          this.PlayCOOPRune();
          this.CurrentRelic.VFXData.PlayNewSequence(playerTransform, new Transform[1]
          {
            playerTransform
          });
          this.StartCoroutine((IEnumerator) this.Delay(0.5f, true, (System.Action) (() =>
          {
            this.playerFarming.health.DealDamage(1f, this.playerFarming.gameObject, this.transform.position, false, Health.AttackTypes.Melee, false, (Health.AttackFlags) 0);
            foreach (PlayerFarming player in PlayerFarming.players)
            {
              if ((UnityEngine.Object) player != (UnityEngine.Object) this.playerFarming)
              {
                player.playerController.MakeUntouchable(12.5f);
                player.playerRelic.InvincibleFromRelic = true;
                VFXSequence sequence2 = EquipmentManager.GetRelicData(RelicType.GungeonBlank).VFXData.PlayNewSequence(player.transform, new Transform[1]
                {
                  player.transform
                });
                GameManager.GetInstance().StartCoroutine((IEnumerator) this.VFXTimer(12.5f, sequence2));
              }
            }
          })));
          break;
        case RelicType.FriendlyHealing_Coop:
          this.PlayCOOPRune();
          this.CurrentRelic.VFXData.PlayNewSequence(playerTransform, new Transform[1]
          {
            playerTransform
          });
          this.StartCoroutine((IEnumerator) this.Delay(0.5f, true, (System.Action) (() =>
          {
            Vector3 position = (PlayerFarming.players[0].transform.position + PlayerFarming.players[1].transform.position) / 2f;
            SoulCustomTarget.Create(PlayerFarming.players[0].gameObject, position, Color.red, (System.Action) (() =>
            {
              PlayerFarming.players[0].health.Heal(1f);
              AudioManager.Instance.PlayOneShot("event:/player/collect_heart", PlayerFarming.players[0].transform.position);
              BiomeConstants.Instance.EmitHeartPickUpVFX(PlayerFarming.players[0].transform.position, 0.0f, "red", "burst_big");
            }));
            SoulCustomTarget.Create(PlayerFarming.players[1].gameObject, position, Color.red, (System.Action) (() =>
            {
              PlayerFarming.players[1].health.Heal(1f);
              AudioManager.Instance.PlayOneShot("event:/player/collect_heart", PlayerFarming.players[1].transform.position);
              BiomeConstants.Instance.EmitHeartPickUpVFX(PlayerFarming.players[1].transform.position, 0.0f, "red", "burst_big");
            }));
          })));
          break;
        case RelicType.Explosive_Coop:
          this.PlayCOOPRune();
          this.CurrentRelic.VFXData.PlayNewSequence(playerTransform, new Transform[1]
          {
            playerTransform
          });
          int explosions = 10;
          float r = 2f;
          duration = 1f;
          this.StartCoroutine((IEnumerator) this.Delay(0.5f, true, (System.Action) (() =>
          {
            PlayerFarming.players[0].StartCoroutine((IEnumerator) PlayerFarming.players[0].playerRelic.ExplosiveRingIE(explosions, r, duration));
            PlayerFarming.players[1].StartCoroutine((IEnumerator) PlayerFarming.players[1].playerRelic.ExplosiveRingIE(explosions, r, duration));
          })));
          break;
        case RelicType.FervourForResources_Corrupted:
          this.CurrentRelic.VFXData.PlayNewSequence(playerTransform, new Transform[1]
          {
            playerTransform
          });
          float num8 = Mathf.Lerp(1f, 3f, (float) (DataManager.Instance.BossesCompleted.Count + 1) / 5f) * (this.playerFarming.playerSpells.faithAmmo.Ammo / this.playerFarming.playerSpells.faithAmmo.Total);
          if (!TrinketManager.HasTrinket(TarotCards.Card.NoCorruption, this.playerFarming))
            DOTween.To((DOGetter<float>) (() => this.playerFarming.playerSpells.faithAmmo.Ammo), (DOSetter<float>) (x => this.playerFarming.playerSpells.faithAmmo.Ammo = x), 0.0f, 1f);
          int resourcesToGive = Mathf.RoundToInt(10f * num8);
          if (!TrinketManager.HasTrinket(TarotCards.Card.CorruptedFullCorruption, this.playerFarming))
          {
            this.StartCoroutine((IEnumerator) this.Delay(1f, true, (System.Action) (() =>
            {
              List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>()
              {
                InventoryItem.ITEM_TYPE.BLACK_GOLD,
                InventoryItem.ITEM_TYPE.LOG,
                InventoryItem.ITEM_TYPE.STONE
              };
              for (int index = 0; index < resourcesToGive; ++index)
                InventoryItem.Spawn(itemTypeList[UnityEngine.Random.Range(0, itemTypeList.Count)], 1, this.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle);
            })));
            break;
          }
          break;
        case RelicType.Realm_Corrupted:
          this.CurrentRelic.VFXData.PlayNewSequence(this.playerFarming.transform, new Transform[1]
          {
            this.playerFarming.transform
          });
          GameManager.GetInstance().StartCoroutine((IEnumerator) this.Delay(0.5f, true, (System.Action) (() =>
          {
            bool doubledDamage = false;
            bool increasedRecivedDamage = false;
            if (!TrinketManager.HasTrinket(TarotCards.Card.CorruptedFullCorruption, this.playerFarming))
            {
              foreach (PlayerFarming player in PlayerFarming.players)
                player.playerRelic.DoubleDamage = true;
              doubledDamage = true;
            }
            if (!TrinketManager.HasTrinket(TarotCards.Card.NoCorruption, this.playerFarming))
            {
              foreach (PlayerFarming player in PlayerFarming.players)
                this.playerFarming.health.DamageModifier *= 2f;
              increasedRecivedDamage = true;
            }
            GameManager.GetInstance().StartCoroutine((IEnumerator) this.Delay(5f, true, (System.Action) (() =>
            {
              if (doubledDamage)
              {
                foreach (PlayerFarming player in PlayerFarming.players)
                  player.playerRelic.DoubleDamage = false;
                doubledDamage = false;
              }
              if (!increasedRecivedDamage)
                return;
              foreach (PlayerFarming player in PlayerFarming.players)
                this.playerFarming.health.DamageModifier /= 2f;
              increasedRecivedDamage = false;
            })));
          })));
          break;
        case RelicType.FireballsRain:
          this.FireballRainStrikes();
          break;
        case RelicType.FieryBlood:
        case RelicType.IceyBlood:
          AudioManager.Instance.PlayOneShot("event:/relics/heart_convert_dammed", this.gameObject);
          AudioManager.Instance.PlayOneShot(relicType == RelicType.FieryBlood ? this.relicFieryBloodTriggerSFX : this.relicIceBloodTriggerSFX);
          List<GameObject> tmpObjs = new List<GameObject>();
          List<HUD_Heart> heartIcons = this.playerFarming.hudHearts.HeartIcons;
          if (heartIcons.Count <= 0 || !heartIcons[0].gameObject.activeInHierarchy)
            return;
          Camera main1 = Camera.main;
          Vector3 worldPoint3 = main1.ScreenToWorldPoint(new Vector3(playerTransform.position.x, playerTransform.position.y, main1.nearClipPlane));
          GameObject gameObject4 = new GameObject();
          tmpObjs.Add(gameObject4);
          gameObject4.transform.position = worldPoint3;
          sequence = EquipmentManager.GetRelicData(relicType).VFXData.PlayNewSequence(gameObject4.transform, new Transform[1]
          {
            this.transform
          });
          Vector3 position1 = heartIcons[0].rectTransform.position;
          Vector3 worldPoint4 = main1.ScreenToWorldPoint(new Vector3(position1.x, position1.y, 10f));
          Transform transform1 = sequence.ImpactVFXObjects[0].gameObject.transform;
          transform1.position = new Vector3(transform1.position.x, transform1.position.y, -1f);
          transform1.DOMove(worldPoint4, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => { }));
          this.StartCoroutine((IEnumerator) this.Delay(1.5f, true, (System.Action) (() =>
          {
            for (int index = tmpObjs.Count - 1; index >= 0; --index)
              UnityEngine.Object.Destroy((UnityEngine.Object) tmpObjs[index]);
            switch (relicType)
            {
              case RelicType.FieryBlood:
                this.playerFarming.health.FireHearts += this.playerFarming.health.CurrentHP;
                BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "red", "burst_big");
                AudioManager.Instance.PlayOneShot(this.relicFieryBloodTransformSFX, playerTransform.position);
                this.playerFarming.health.IceHearts = 0.0f;
                break;
              case RelicType.IceyBlood:
                this.playerFarming.health.IceHearts += this.playerFarming.health.CurrentHP;
                BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "red", "burst_big");
                AudioManager.Instance.PlayOneShot(this.relicIceBloodTransformSFX, playerTransform.position);
                this.playerFarming.health.FireHearts = 0.0f;
                break;
            }
            this.playerFarming.health.totalHP = 0.0f;
            this.playerFarming.health.BlueHearts = 0.0f;
            this.playerFarming.health.BlackHearts = 0.0f;
            this.playerFarming.health.TotalSpiritHearts = 0.0f;
          })));
          break;
        case RelicType.FieryBurrow:
        case RelicType.IceyBurrow:
          this.StartCoroutine((IEnumerator) this.BurrowRoutine(relicType));
          break;
        case RelicType.IceyCoat:
          EquipmentManager.GetRelicData(relicType).VFXData.PlayNewSequence(this.playerFarming.transform, new Transform[0], GenerateRoom.Instance.transform);
          this.iceyCoatLoopInstance = AudioManager.Instance.CreateLoop(this.relicIceyCoatLoopSFX, this.gameObject, true);
          this.StartCoroutine((IEnumerator) this.Delay(0.5f, true, (System.Action) (() =>
          {
            this.iceCoatManager.SetActive(true);
            this.playerFarming.playerController.MakeImmuneToProjectiles(10f);
            this.StartCoroutine((IEnumerator) this.Delay(10f, true, (System.Action) (() => this.iceCoatManager.SetActive(false))));
            this.StartCoroutine((IEnumerator) this.Delay(9f, true, (System.Action) (() => AudioManager.Instance.StopLoop(this.iceyCoatLoopInstance))));
          })));
          break;
        case RelicType.IceSpikes:
          this.StartCoroutine((IEnumerator) this.SpawnIceSpikes());
          break;
      }
      if ((this.CurrentRelic.InteractionType == RelicInteractionType.Fragile || this.CurrentRelic.InteractionType == RelicInteractionType.Instant || TrinketManager.AreRelicsFragile(this.playerFarming)) && EquipmentManager.NextRandomRelic != this.CurrentRelic.RelicType)
      {
        this.CurrentRelic = (RelicData) null;
        this.playerFarming.currentRelicType = RelicType.None;
        PlayerRelic.RelicEvent onRelicEquipped = this.OnRelicEquipped;
        if (onRelicEquipped != null)
          onRelicEquipped((RelicData) null, this.playerFarming);
        PlayerRelic.RelicEvent onRelicChanged = PlayerRelic.OnRelicChanged;
        if (onRelicChanged != null)
          onRelicChanged((RelicData) null, this.playerFarming);
        PlayerRelic.RelicEvent onSubRelicChanged2 = this.OnSubRelicChanged;
        if (onSubRelicChanged2 != null)
          onSubRelicChanged2((RelicData) null, this.playerFarming);
      }
      if (TrinketManager.HasTrinket(TarotCards.Card.CorruptedHealForRelic, this.playerFarming) && !TrinketManager.IsCorruptedNegativeEffectNegated(TarotCards.Card.CorruptedHealForRelic, this.playerFarming))
        this.TakeDamageOnRelicUse(playerTransform);
      this.ResetChargedAmount();
      PlayerRelic.RelicEvent relicChargeModified = this.OnRelicChargeModified;
      if (relicChargeModified != null)
        relicChargeModified(this.CurrentRelic, this.playerFarming);
      if (!flag)
        return;
      this.ConsumeRelic(relicType, forceConsumableAnimation);
    }
  }

  public IEnumerator VFXTimer(float duration, VFXSequence sequence)
  {
    yield return (object) new WaitForSeconds(duration);
    if (sequence != null && sequence.ImpactVFXObjects != null && (UnityEngine.Object) sequence.ImpactVFXObjects[0] != (UnityEngine.Object) null)
      sequence.ImpactVFXObjects[0].StopVFX();
  }

  public void RemoveRelic(bool forceConsumableAnimation = true)
  {
    if (!((UnityEngine.Object) this.CurrentRelic != (UnityEngine.Object) null))
      return;
    this.ConsumeRelic(this.CurrentRelic.RelicType, forceConsumableAnimation);
    this.CurrentRelic = (RelicData) null;
    this.playerFarming.currentRelicType = RelicType.None;
    PlayerRelic.RelicEvent onRelicChanged = PlayerRelic.OnRelicChanged;
    if (onRelicChanged != null)
      onRelicChanged((RelicData) null, this.playerFarming);
    PlayerRelic.RelicEvent onRelicEquipped = this.OnRelicEquipped;
    if (onRelicEquipped == null)
      return;
    onRelicEquipped((RelicData) null, this.playerFarming);
  }

  public void ConsumeRelic(RelicType relicType, bool forceConsumableAnimation)
  {
    PlayerRelic.RelicEvent onRelicConsumed = this.OnRelicConsumed;
    if (onRelicConsumed != null)
      onRelicConsumed((RelicData) null, this.playerFarming);
    if (this.animationSequence != null)
      this.animationSequence.Complete();
    if (this.animationCoroutine != null)
      this.StopCoroutine(this.animationCoroutine);
    this.relicIcon.DOKill();
    this.relicIcon.gameObject.SetActive(false);
    RelicData data = EquipmentManager.GetRelicData(relicType);
    if (!data.ShowAnimationAbovePlayer)
      return;
    GameManager.GetInstance().WaitForSeconds(0.25f, (System.Action) (() =>
    {
      this.relicIcon.gameObject.SetActive(true);
      this.relicIcon.transform.localPosition = this.relicIconOriginalPosition;
      this.relicIcon.transform.localScale = Vector3.one * 0.25f;
      this.relicIcon.transform.DOPunchScale(Vector3.one * 0.125f, 0.15f);
      this.relicIcon.sprite = data.Sprite;
      if (data.InteractionType == RelicInteractionType.Fragile | forceConsumableAnimation || TrinketManager.AreRelicsFragile(this.playerFarming))
      {
        AudioManager.Instance.PlayOneShot("event:/relics/relic_break", this.gameObject);
        this.animationCoroutine = this.StartCoroutine((IEnumerator) this.Delay(1.5f, true, (System.Action) (() =>
        {
          this.relicPuff.Play();
          AudioManager.Instance.PlayOneShot("event:/relics/puff_of_smoke", this.gameObject);
          this.relicIcon.gameObject.SetActive(false);
        })));
      }
      else
      {
        if (data.InteractionType != RelicInteractionType.Charging)
          return;
        this.animationSequence = DOTween.Sequence();
        this.animationSequence.AppendInterval(1.2f);
        this.animationSequence.Append((Tween) this.relicIcon.transform.DOLocalMove(Vector3.zero, 0.35f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack));
        this.animationSequence.Join((Tween) this.relicIcon.transform.DOScale(Vector3.zero, 0.35f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack));
      }
    }));
  }

  public void EquipRelic(RelicData relicData, bool fullyCharge = true, bool initialEquip = false)
  {
    System.Action action = (System.Action) (() =>
    {
      AudioManager.Instance.PlayOneShot("event:/relics/relic_get");
      this.CurrentRelic = relicData;
      DataManager.Instance.PreviousRelic = this.CurrentRelic.RelicType;
      if ((UnityEngine.Object) this.CurrentRelic != (UnityEngine.Object) null)
        this.playerFarming.currentRelicType = this.CurrentRelic.RelicType;
      if (TrinketManager.HasTrinket(TarotCards.Card.CorruptedRelicCharge, this.playerFarming) && !TrinketManager.IsCorruptedNegativeEffectNegated(TarotCards.Card.CorruptedRelicCharge, this.playerFarming))
        this.ChargedAmount = 0.0f;
      else if (fullyCharge)
        this.ChargedAmount = Mathf.Clamp(float.MaxValue, 0.0f, this.RequiredChargeAmount);
      if (initialEquip)
      {
        switch (relicData.RelicType)
        {
          case RelicType.UseRandomRelic:
            EquipmentManager.PickRandomRelicData(true);
            break;
          case RelicType.UseRandomRelic_Blessed:
            EquipmentManager.PickRandomRelicData(true, RelicSubType.Blessed);
            break;
          case RelicType.UseRandomRelic_Dammed:
            EquipmentManager.PickRandomRelicData(true, RelicSubType.Dammed);
            break;
        }
        if (TrinketManager.HasTrinket(TarotCards.Card.CorruptedHealForRelic, this.playerFarming) && !TrinketManager.HasTrinket(TarotCards.Card.CorruptedFullCorruption, this.playerFarming))
        {
          this.playerFarming.health.Heal(1f);
          AudioManager.Instance.PlayOneShot("event:/player/collect_heart", this.transform.position);
          BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "red", "burst_big");
        }
      }
      PlayerRelic.RelicEvent onSubRelicChanged = this.OnSubRelicChanged;
      if (onSubRelicChanged != null)
        onSubRelicChanged(this.CurrentRelic, this.playerFarming);
      PlayerRelic.RelicEvent onRelicEquipped = this.OnRelicEquipped;
      if (onRelicEquipped != null)
        onRelicEquipped(relicData, this.playerFarming);
      PlayerRelic.RelicEvent onRelicChanged = PlayerRelic.OnRelicChanged;
      if (onRelicChanged != null)
        onRelicChanged(relicData, this.playerFarming);
      if (this.animationSequence != null)
        this.animationSequence.Complete();
      this.relicIcon.DOKill();
      this.relicIcon.gameObject.SetActive(true);
      this.relicIcon.transform.localScale = Vector3.one * 0.25f;
      this.relicIcon.transform.DOPunchScale(Vector3.one * 0.2f, 0.25f);
      this.relicIcon.sprite = this.CurrentRelic.Sprite;
      this.animationSequence = DOTween.Sequence();
      this.animationSequence.AppendInterval(1f);
      this.animationSequence.Join((Tween) this.relicIcon.transform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack));
      if (this.CurrentRelic.InteractionType != RelicInteractionType.Instant)
        return;
      this.UseRelic(this.CurrentRelic.RelicType, true);
    });
    if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Relics))
    {
      UITutorialOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Relics);
      overlayController.OnHidden = overlayController.OnHidden + action;
    }
    else
      action();
  }

  public void IncreaseChargedAmount()
  {
    if (!((UnityEngine.Object) this.CurrentRelic != (UnityEngine.Object) null))
      return;
    this.ChargedAmount = Mathf.Clamp(this.ChargedAmount + 1f * PlayerFleeceManager.GetRelicChargeMultiplier() * TrinketManager.GetRelicChargeMultiplier(this.playerFarming), 0.0f, this.RequiredChargeAmount);
    PlayerRelic.RelicEvent relicChargeModified = this.OnRelicChargeModified;
    if (relicChargeModified == null)
      return;
    relicChargeModified(this.CurrentRelic, this.playerFarming);
  }

  public void IncreaseChargedAmount(float increase = 1f)
  {
    if (!((UnityEngine.Object) this.CurrentRelic != (UnityEngine.Object) null))
      return;
    increase *= 0.85f;
    increase *= PlayerFleeceManager.GetRelicChargeMultiplier();
    increase *= TrinketManager.GetRelicChargeMultiplier(this.playerFarming);
    this.ChargedAmount = Mathf.Clamp(this.ChargedAmount + increase, 0.0f, this.RequiredChargeAmount);
    PlayerRelic.RelicEvent relicChargeModified = this.OnRelicChargeModified;
    if (relicChargeModified == null)
      return;
    relicChargeModified(this.CurrentRelic, this.playerFarming);
  }

  public void ResetChargedAmount()
  {
    if (!((UnityEngine.Object) this.CurrentRelic != (UnityEngine.Object) null))
      return;
    this.ChargedAmount = 0.0f;
    PlayerRelic.RelicEvent relicChargeModified = this.OnRelicChargeModified;
    if (relicChargeModified == null)
      return;
    relicChargeModified(this.CurrentRelic, this.playerFarming);
  }

  public static void Reload()
  {
    List<PlayerFarming> players = PlayerFarming.players;
    for (int index = 0; index < players.Count; ++index)
      players[index].playerRelic.FullyCharge();
  }

  public void FullyCharge()
  {
    if (!((UnityEngine.Object) this.CurrentRelic != (UnityEngine.Object) null))
      return;
    this.ChargedAmount = this.RequiredChargeAmount;
    PlayerRelic.RelicEvent relicChargeModified = this.OnRelicChargeModified;
    if (relicChargeModified == null)
      return;
    relicChargeModified(this.CurrentRelic, this.playerFarming);
  }

  public void Update()
  {
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null || this.playerFarming.GoToAndStopping || this.playerFarming.IsKnockedOut)
      return;
    switch (this.playerFarming.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
      case StateMachine.State.Moving:
      case StateMachine.State.Attacking:
      case StateMachine.State.Aiming:
        if (!((UnityEngine.Object) this.CurrentRelic != (UnityEngine.Object) null) || !InputManager.Gameplay.GetRelicButtonDown(this.playerFarming) || (double) this.ChargedAmount < (double) this.RequiredChargeAmount)
          break;
        PlayerRelic.RelicEvent onRelicUsed = this.OnRelicUsed;
        if (onRelicUsed != null)
          onRelicUsed(this.CurrentRelic, this.playerFarming);
        this.UseRelic(this.CurrentRelic.RelicType);
        break;
    }
  }

  public IEnumerator Delay(float delay, bool useTimeScale, System.Action callback)
  {
    if (useTimeScale)
      yield return (object) new WaitForSeconds(delay);
    else
      yield return (object) new WaitForSecondsRealtime(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void LightningStrike()
  {
    float damage = (float) (12.0 + (double) PlayerWeapon.GetDamage(1f, this.playerFarming.currentWeaponLevel, this.playerFarming) * 5.0) * TrinketManager.GetRelicDamageMultiplier(this.playerFarming);
    float dropChance = this.CurrentRelic.RelicType != RelicType.LightningStrike ? 2.5f : 0.0f;
    new LightningStrikeAbility(this.CurrentRelic.VFXData, 5, this.CurrentRelic.RelicType == RelicType.LightningStrike_Blessed ? InventoryItem.ITEM_TYPE.BLUE_HEART : InventoryItem.ITEM_TYPE.BLACK_HEART, dropChance).Play(this.gameObject, Health.Team.Team2, damage, this.playerFarming, true, (List<Health>) null, "");
  }

  public void FireballRainStrikes()
  {
    float damage = (4f + PlayerWeapon.GetDamage(1f, this.playerFarming.currentWeaponLevel, this.playerFarming)) * TrinketManager.GetRelicDamageMultiplier(this.playerFarming);
    AudioManager.Instance.PlayOneShot(this.relicFireballRainTriggerSFX, this.gameObject);
    AOEStrikeAbility aoeStrikeAbility = new AOEStrikeAbility(this.CurrentRelic.VFXData, 5, InventoryItem.ITEM_TYPE.NONE, 0.0f);
    aoeStrikeAbility.AddAttackFlags(Health.AttackFlags.Burn);
    aoeStrikeAbility.Play(this.gameObject, Health.Team.Team2, damage, this.playerFarming, false, (List<Health>) null, this.relicFireballRainImpactSFX);
  }

  public void SpawnDemon(int level)
  {
    int type = UnityEngine.Random.Range(0, 5);
    int num1 = 0;
    while (++num1 < 30)
    {
      int num2 = UnityEngine.Random.Range(0, 5);
      if (!DataManager.Instance.Followers_Demons_Types.Contains(num2))
      {
        type = num2;
        break;
      }
    }
    RelicType relicType = this.CurrentRelic.RelicType;
    BiomeGenerator.Instance.SpawnDemon(type, -1, level, true, this.gameObject, (Action<Demon>) (demon => this.StartCoroutine((IEnumerator) this.DemonSpawned(demon, relicType))));
  }

  public void SpawnRotDemon()
  {
    RelicType relicType = RelicType.SpawnDemon_Dammed;
    BiomeGenerator.Instance.SpawnDemon(13, -1, 1, true, this.gameObject, (Action<Demon>) (demon =>
    {
      AudioManager.Instance.PlayOneShot("event:/dlc/tarot/rotdemon_summon_trigger", demon.transform.position);
      this.StartCoroutine((IEnumerator) this.DemonSpawned(demon, relicType));
    }));
  }

  public IEnumerator DemonSpawned(Demon demon, RelicType relicType)
  {
    demon.gameObject.SetActive(false);
    EquipmentManager.GetRelicData(relicType).VFXData.PlayNewSequence(this.playerFarming.transform, new Transform[1]
    {
      demon.transform
    }, GenerateRoom.Instance.transform);
    System.Action setupDemonAction = (System.Action) (() => this.SetupSpawnedDemon(demon));
    setupDemonAction += (System.Action) (() => this.onDestroyWithSpawningAlly -= setupDemonAction);
    this.onDestroyWithSpawningAlly += setupDemonAction;
    yield return (object) new WaitForSeconds(2f);
    AudioManager.Instance.PlayOneShot("event:/relics/demon_spawn", demon.gameObject);
    setupDemonAction();
  }

  public void SetupSpawnedDemon(Demon demon)
  {
    demon.SetMaster(this.playerFarming.gameObject);
    demon.gameObject.SetActive(true);
    demon.transform.localScale = Vector3.zero;
    demon.transform.DOScale(1f, 0.25f);
  }

  public IEnumerator DestroyTarotsGainStrength(
    PlayerRelic.BonusType bonusType,
    PlayerFarming playerFarming)
  {
    PlayerRelic playerRelic = this;
    playerRelic.CurrentRelic.VFXData.PlayNewSequence(playerFarming.transform, new Transform[1]
    {
      playerFarming.transform
    });
    float increment = 1f / (float) playerFarming.RunTrinkets.Count;
    if (bonusType == PlayerRelic.BonusType.GainStrength)
    {
      AudioManager.Instance.PlayOneShot("event:/player/weapon_equip", playerRelic.transform.position);
      AudioManager.Instance.PlayOneShot("event:/player/weapon_unlocked", playerRelic.transform.position);
      playerFarming.TimedAction(1.6f, (System.Action) null, playerFarming.CurrentWeaponInfo.WeaponData.PickupAnimationKey);
    }
    for (int i = playerFarming.RunTrinkets.Count - 1; i >= 0; --i)
    {
      TrinketManager.RemoveTrinket(playerFarming.RunTrinkets[i].CardType, playerFarming);
      switch (bonusType)
      {
        case PlayerRelic.BonusType.GainStrength:
          DataManager.Instance.PLAYER_RUN_DAMAGE_LEVEL += 0.25f;
          BiomeConstants.Instance.EmitHeartPickUpVFX(playerFarming.CameraBone.transform.position, 0.0f, "strength", "strength");
          AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", playerFarming.transform.position);
          ++playerFarming.currentWeaponLevel;
          ++playerFarming.currentRunWeaponLevel;
          ++playerFarming.currentCurseLevel;
          ++playerFarming.currentRunCurseLevel;
          BiomeConstants.Instance.EmitHeartPickUpVFX(playerFarming.CameraBone.transform.position, 0.0f, "strength", "strength");
          AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", playerFarming.transform.position);
          break;
        case PlayerRelic.BonusType.GainBlueHearts:
          ++playerFarming.health.BlueHearts;
          BiomeConstants.Instance.EmitHeartPickUpVFX(playerRelic.transform.position, 0.0f, "blue", "burst_big");
          AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", playerFarming.transform.position);
          break;
        case PlayerRelic.BonusType.GainBlackHearts:
          ++playerFarming.health.BlackHearts;
          BiomeConstants.Instance.EmitHeartPickUpVFX(playerRelic.transform.position, 0.0f, "black", "burst_big");
          AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", playerFarming.transform.position);
          break;
      }
      yield return (object) new WaitForSeconds(increment);
    }
    if (bonusType == PlayerRelic.BonusType.GainStrength)
    {
      playerFarming.playerWeapon.SetWeapon(playerFarming.currentWeapon, playerFarming.currentWeaponLevel);
      playerFarming.playerSpells.SetSpell(playerFarming.currentCurse, playerFarming.currentCurseLevel);
    }
  }

  public void DestroyTarotsDealDamage(
    float damageMultiplier,
    int maxEnemies,
    float chanceForBlueHearts,
    float chanceForBlackHearts)
  {
    float damage = Mathf.Clamp(PlayerWeapon.GetDamage(2f, this.playerFarming.currentWeaponLevel, this.playerFarming) * (float) this.playerFarming.RunTrinkets.Count, 3f, 20f);
    damage *= damageMultiplier;
    List<Transform> transformList = new List<Transform>();
    List<Health> targetEnemies = new List<Health>();
    for (int index = Health.team2.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null && targetEnemies.Count < maxEnemies)
      {
        targetEnemies.Add(Health.team2[index]);
        transformList.Add(Health.team2[index].transform);
      }
    }
    VFXSequence sequence = this.CurrentRelic.VFXData.PlayNewSequence(this.playerFarming.transform, transformList.ToArray());
    sequence.OnImpact += (Action<VFXObject, int>) ((vfxObject, targetIndex) =>
    {
      if (targetEnemies.Count <= targetIndex || !((UnityEngine.Object) targetEnemies[targetIndex] != (UnityEngine.Object) null))
        return;
      Vector3 position = targetEnemies[targetIndex].transform.position;
      vfxObject.transform.SetPositionAndRotation(position, Quaternion.identity);
      targetEnemies[targetIndex].DealDamage(damage, this.gameObject, position, AttackType: Health.AttackTypes.Projectile, AttackFlags: Health.AttackFlags.DoesntChargeRelics);
      if ((double) chanceForBlueHearts != 0.0 && (double) UnityEngine.Random.value < (double) chanceForBlueHearts)
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLUE_HEART, 1, position);
      if ((double) chanceForBlackHearts == 0.0 || (double) UnityEngine.Random.value >= (double) chanceForBlackHearts)
        return;
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_HEART, 1, position).MagnetToPlayer = false;
    });
    sequence.OnComplete += (System.Action) (() =>
    {
      sequence.OnImpact -= (Action<VFXObject, int>) ((vfxObject, targetIndex) =>
      {
        if (targetEnemies.Count <= targetIndex || !((UnityEngine.Object) targetEnemies[targetIndex] != (UnityEngine.Object) null))
          return;
        Vector3 position = targetEnemies[targetIndex].transform.position;
        vfxObject.transform.SetPositionAndRotation(position, Quaternion.identity);
        targetEnemies[targetIndex].DealDamage(damage, this.gameObject, position, AttackType: Health.AttackTypes.Projectile, AttackFlags: Health.AttackFlags.DoesntChargeRelics);
        if ((double) chanceForBlueHearts != 0.0 && (double) UnityEngine.Random.value < (double) chanceForBlueHearts)
          InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLUE_HEART, 1, position);
        if ((double) chanceForBlackHearts == 0.0 || (double) UnityEngine.Random.value >= (double) chanceForBlackHearts)
          return;
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_HEART, 1, position).MagnetToPlayer = false;
      });
      sequence.OnComplete -= (System.Action) (() =>
      {
        // ISSUE: unable to decompile the method.
      });
    });
  }

  public void FiftyFiftyGamble(RelicType relicType)
  {
    this.playerFarming.interactor.CurrentInteraction?.EndIndicateHighlighted(this.playerFarming);
    SimpleSetCamera.DisableAll(2f);
    MMConversation.isPlaying = false;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.gameObject, 5f);
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.5f, -1f));
    VFXSequence sequence = EquipmentManager.GetRelicData(relicType).VFXData.PlayNewSequence(this.playerFarming.transform, new Transform[1]
    {
      this.transform
    });
    AudioManager.Instance.PlayOneShot("event:/relics/fifty_fifty_dice", this.gameObject);
    this.playerFarming.SpineUseDeltaTime(false);
    this.playerFarming.Spine.AnimationState.AddAnimation(0, "cast-spell2-loop", true, 0.0f);
    VFX_Dice component = sequence.ImpactVFXObjects[0].GetComponent<VFX_Dice>();
    if (component.OnDiceRolled != null)
    {
      foreach (Delegate invocation in component.OnDiceRolled.GetInvocationList())
      {
        VFX_Dice vfxDice = component;
        vfxDice.OnDiceRolled = (Action<bool>) Delegate.Remove((Delegate) vfxDice.OnDiceRolled, invocation);
      }
    }
    component.OnDiceRolled += (Action<bool>) (won =>
    {
      if ((double) this.playerFarming.playerController.UntouchableTimer <= 0.0)
        this.playerFarming.health.untouchable = false;
      GameManager.GetInstance().WaitForSeconds(0.25f, (System.Action) (() =>
      {
        switch (relicType)
        {
          case RelicType.FiftyFiftyGamble:
            if (!won)
            {
              this.playerFarming.health.TotalSpiritHearts += 2f;
              AudioManager.Instance.PlayOneShot("event:/player/collect_heart", this.transform.position);
              BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "red", "burst_big");
              break;
            }
            this.playerFarming.health.Heal((float) TrinketManager.GetHealthAmountMultiplier(this.playerFarming));
            AudioManager.Instance.PlayOneShot("event:/player/collect_heart", this.transform.position);
            BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "red", "burst_big");
            break;
          case RelicType.FiftyFiftyGamble_Dammed:
            if (!won)
            {
              Health.DamageAllEnemies(PlayerWeapon.GetDamage(5f, this.playerFarming.currentWeaponLevel, this.playerFarming) * TrinketManager.GetRelicDamageMultiplier(this.playerFarming), Health.DamageAllEnemiesType.Manipulation);
              break;
            }
            this.playerFarming.health.BlackHearts += 2f;
            AudioManager.Instance.PlayOneShot("event:/player/collect_heart", this.transform.position);
            BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "black", "burst_big");
            break;
          case RelicType.FiftyFiftyGamble_Blessed:
            if (won)
            {
              this.playerFarming.playerController.MakeUntouchable(12.5f);
              this.InvincibleFromRelic = true;
              VFXSequence sequence2 = EquipmentManager.GetRelicData(RelicType.GungeonBlank).VFXData.PlayNewSequence(this.playerFarming.transform, new Transform[1]
              {
                this.playerFarming.transform
              });
              GameManager.GetInstance().StartCoroutine((IEnumerator) this.VFXTimer(12.5f, sequence2));
              break;
            }
            this.playerFarming.health.BlueHearts += 2f;
            AudioManager.Instance.PlayOneShot("event:/player/collect_heart", this.transform.position);
            BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "blue", "burst_big");
            break;
          case RelicType.EnemyFiftyFifty_Corrupted:
            sequence = EquipmentManager.GetRelicData(RelicType.SpawnCombatFollower).VFXData.PlayNewSequence(this.transform, new Transform[1]
            {
              this.transform
            }, onlyImpact: true);
            sequence.OnImpact += (Action<VFXObject, int>) ((result, slot) => AudioManager.Instance.PlayOneShot("event:/relics/follower_impact", result.gameObject));
            if (!won && !TrinketManager.HasTrinket(TarotCards.Card.NoCorruption, this.playerFarming) || TrinketManager.HasTrinket(TarotCards.Card.CorruptedFullCorruption, this.playerFarming))
            {
              sequence.OnImpact += new Action<VFXObject, int>(this.SpawnEnemy);
              break;
            }
            sequence.OnImpact += new Action<VFXObject, int>(this.SpawnFriendlyEnemy);
            break;
        }
      }));
      this.playerFarming.SpineUseDeltaTime(true);
      GameManager.GetInstance().OnConversationEnd();
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      this.playerFarming.interactor.CurrentInteraction?.IndicateHighlighted(this.playerFarming);
      SimpleSetCamera.EnableAll();
      this.playerFarming.indicator.Activate();
    });
  }

  public IEnumerator TeleportToBossRoom()
  {
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.playerFarming.gameObject);
    this.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    this.playerFarming.Spine.AnimationState.SetAnimation(0, "warp-out-down", false);
    yield return (object) new WaitForSeconds(2.5f);
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.5f, "", (System.Action) (() =>
    {
      GameManager.GetInstance().OnConversationEnd();
      Vector2 vector2 = new Vector2((float) BiomeGenerator.Instance.LastRoom.x, (float) BiomeGenerator.Instance.LastRoom.y);
      Vector2 bossRoom = (Vector2) BiomeGenerator.Instance.GetBossRoom();
      if (bossRoom != (Vector2) new Vector2Int(0, 0) && bossRoom != new Vector2(0.0f, 999f))
        vector2 = (Vector2) BiomeGenerator.Instance.GetBossRoom();
      BiomeGenerator.ChangeRoom((int) vector2.x, (int) vector2.y);
    }));
  }

  public IEnumerator TeleportToRandomRoom()
  {
    BiomeRoom room = (BiomeRoom) null;
    int num = 0;
    while (num++ < 100)
    {
      room = BiomeGenerator.Instance.Rooms[UnityEngine.Random.Range(0, BiomeGenerator.Instance.Rooms.Count)];
      if (room != BiomeGenerator.Instance.CurrentRoom && room != BiomeGenerator.Instance.RespawnRoom && room != BiomeGenerator.Instance.DeathCatRoom)
        break;
    }
    if (room != null)
    {
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(this.playerFarming.gameObject);
      this.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      this.playerFarming.Spine.AnimationState.SetAnimation(0, "warp-out-down", false);
      yield return (object) new WaitForSeconds(2.5f);
      MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.5f, "", (System.Action) (() =>
      {
        GameManager.GetInstance().OnConversationEnd();
        BiomeGenerator.ChangeRoom(room.x, room.y);
      }));
    }
  }

  public void SpawnFriendlyEnemy(VFXObject result, int slot)
  {
    Addressables_wrapper.InstantiateAsync((object) this.friendlyEnemy, this.transform.parent, false, (Action<AsyncOperationHandle<GameObject>>) (friendlyEnemy =>
    {
      Health friendlyEnemyHealth = friendlyEnemy.Result.GetComponent<Health>();
      friendlyEnemyHealth.enabled = false;
      friendlyEnemyHealth.team = Health.Team.PlayerTeam;
      friendlyEnemyHealth.HP = friendlyEnemyHealth.totalHP = 15f;
      friendlyEnemyHealth.ImmuneToPlayer = true;
      friendlyEnemyHealth.ImmuneToTraps = false;
      friendlyEnemyHealth.transform.position = result.transform.position;
      friendlyEnemyHealth.gameObject.SetActive(false);
      System.Action setupFriendlyEnemyAction = (System.Action) (() => this.SetupFriendlyEnemy(friendlyEnemyHealth));
      setupFriendlyEnemyAction += (System.Action) (() => this.onDestroyWithSpawningAlly -= setupFriendlyEnemyAction);
      this.onDestroyWithSpawningAlly += setupFriendlyEnemyAction;
      this.StartCoroutine((IEnumerator) this.Delay(1f, true, (System.Action) (() =>
      {
        setupFriendlyEnemyAction();
        CameraManager.instance.ShakeCameraForDuration(0.5f, 0.7f, 0.2f);
      })));
    }));
  }

  public void SpawnEnemy(VFXObject result, int slot)
  {
    Addressables_wrapper.InstantiateAsync((object) this.spawnableEnemies[UnityEngine.Random.Range(0, this.spawnableEnemies.Length)], this.transform.parent, false, (Action<AsyncOperationHandle<GameObject>>) (enemy =>
    {
      Health enemyHealth = enemy.Result.GetComponent<Health>();
      enemyHealth.enabled = false;
      enemyHealth.team = Health.Team.Team2;
      enemyHealth.gameObject.SetActive(false);
      enemyHealth.transform.position = result.transform.position;
      this.StartCoroutine((IEnumerator) this.Delay(1f, true, (System.Action) (() =>
      {
        enemyHealth.enabled = true;
        enemyHealth.gameObject.SetActive(true);
        Interaction_Chest.Instance?.AddEnemy(enemyHealth);
        CameraManager.instance.ShakeCameraForDuration(0.5f, 0.7f, 0.2f);
      })));
    }));
  }

  public void SetupFriendlyEnemy(Health friendlyEnemy)
  {
    friendlyEnemy.enabled = true;
    EnemySwordsman component = friendlyEnemy.GetComponent<EnemySwordsman>();
    component.SeperateObject = true;
    component.gameObject.SetActive(true);
    component.Damage = PlayerWeapon.GetDamage(2f, this.playerFarming.currentWeaponLevel, this.playerFarming) * TrinketManager.GetRelicDamageMultiplier(this.playerFarming);
    component.health.team = Health.Team.PlayerTeam;
    component.FollowPlayer = true;
    component.enabled = true;
    component.VisionRange = int.MaxValue;
    this.SetFriendlyEnemySpawnPosition(component);
  }

  public void SetFriendlyEnemySpawnPosition(EnemySwordsman enemy)
  {
    CompositeCollider2D roomTransform = GenerateRoom.Instance.RoomTransform;
    float magnitude1 = (roomTransform.ClosestPoint((Vector2) enemy.transform.position) - (Vector2) roomTransform.transform.position).magnitude;
    if ((double) (enemy.transform.position - roomTransform.transform.position).magnitude <= (double) magnitude1 || !RoomLockController.DoorsOpen)
      return;
    List<RoomLockController> roomLockControllers = RoomLockController.RoomLockControllers;
    if (roomLockControllers.Count == 0)
    {
      enemy.transform.position = this.playerFarming.transform.position;
    }
    else
    {
      if (RoomLockController.DoorsOpen && BiomeGenerator.Instance.CurrentRoom.Completed)
        return;
      Transform transform1 = (Transform) null;
      float num = 0.0f;
      for (int index = 0; index < roomLockControllers.Count; ++index)
      {
        if ((UnityEngine.Object) transform1 == (UnityEngine.Object) null)
        {
          transform1 = roomLockControllers[index].transform;
          num = (transform1.position - this.playerFarming.transform.position).magnitude;
        }
        else
        {
          Transform transform2 = roomLockControllers[index].transform;
          float magnitude2 = (transform2.position - this.playerFarming.transform.position).magnitude;
          if ((double) magnitude2 < (double) num)
          {
            transform1 = transform2;
            num = magnitude2;
          }
        }
      }
      Vector3 vector3 = (transform1.position - this.playerFarming.transform.position).normalized * 1.5f;
      enemy.transform.position = transform1.position + vector3;
    }
  }

  public void AddFrozenEnemy(Health enemy)
  {
    if (!((UnityEngine.Object) enemy != (UnityEngine.Object) null))
      return;
    enemy.AddFreezeTime();
    this.enemies.Add(enemy);
  }

  public void DealDamagePerFollower(float damage)
  {
    List<Transform> transformList = new List<Transform>();
    List<Health> targetEnemies = new List<Health>();
    for (int index = Health.team2.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null)
      {
        targetEnemies.Add(Health.team2[index]);
        transformList.Add(Health.team2[index].transform);
      }
    }
    VFXSequence sequence = this.CurrentRelic.VFXData.PlayNewSequence(this.playerFarming.transform, transformList.ToArray());
    sequence.OnImpact += (Action<VFXObject, int>) ((vfxObject, targetIndex) =>
    {
      if (targetEnemies.Count <= targetIndex || !((UnityEngine.Object) targetEnemies[targetIndex] != (UnityEngine.Object) null))
        return;
      Vector3 position = targetEnemies[targetIndex].transform.position;
      vfxObject.transform.SetPositionAndRotation(position, Quaternion.identity);
      targetEnemies[targetIndex].DealDamage(PlayerWeapon.GetDamage(damage, this.playerFarming.currentWeaponLevel, this.playerFarming) * TrinketManager.GetRelicDamageMultiplier(this.playerFarming), this.gameObject, position, AttackType: Health.AttackTypes.Projectile, AttackFlags: Health.AttackFlags.DoesntChargeRelics);
    });
    sequence.OnComplete += (System.Action) (() =>
    {
      sequence.OnImpact -= (Action<VFXObject, int>) ((vfxObject, targetIndex) =>
      {
        if (targetEnemies.Count <= targetIndex || !((UnityEngine.Object) targetEnemies[targetIndex] != (UnityEngine.Object) null))
          return;
        Vector3 position = targetEnemies[targetIndex].transform.position;
        vfxObject.transform.SetPositionAndRotation(position, Quaternion.identity);
        targetEnemies[targetIndex].DealDamage(PlayerWeapon.GetDamage(damage, this.playerFarming.currentWeaponLevel, this.playerFarming) * TrinketManager.GetRelicDamageMultiplier(this.playerFarming), this.gameObject, position, AttackType: Health.AttackTypes.Projectile, AttackFlags: Health.AttackFlags.DoesntChargeRelics);
      });
      sequence.OnComplete -= (System.Action) (() =>
      {
        // ISSUE: unable to decompile the method.
      });
    });
  }

  public void KillNonBossEnemies()
  {
    List<Transform> transformList = new List<Transform>();
    List<Health> targetEnemies = new List<Health>();
    for (int index = Health.team2.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null && (UnityEngine.Object) Health.team2[index].GetComponentInParent<BossIntro>() == (UnityEngine.Object) null && (UnityEngine.Object) Health.team2[index].GetComponentInParent<DeathCatController>() == (UnityEngine.Object) null && (UnityEngine.Object) Health.team2[index].GetComponentInParent<WolfArmPiece>() == (UnityEngine.Object) null)
      {
        UnitObject component = Health.team2[index].GetComponent<UnitObject>();
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.IsBoss)
        {
          targetEnemies.Add(Health.team2[index]);
          transformList.Add(Health.team2[index].transform);
        }
      }
    }
    VFXSequence sequence = this.CurrentRelic.VFXData.PlayNewSequence(this.playerFarming.transform, transformList.ToArray());
    sequence.OnImpact += (Action<VFXObject, int>) ((vfxObject, targetIndex) =>
    {
      if (targetEnemies.Count <= targetIndex || !((UnityEngine.Object) targetEnemies[targetIndex] != (UnityEngine.Object) null))
        return;
      targetEnemies[targetIndex].GetComponent<IAttackResilient>()?.StopResilience();
      Vector3 position = targetEnemies[targetIndex].transform.position;
      vfxObject.transform.SetPositionAndRotation(position, Quaternion.identity);
      targetEnemies[targetIndex].invincible = false;
      targetEnemies[targetIndex].enabled = true;
      targetEnemies[targetIndex].HasShield = false;
      targetEnemies[targetIndex].DealDamage(targetEnemies[targetIndex].HP, this.gameObject, position, AttackType: Health.AttackTypes.NoHitStop, AttackFlags: Health.AttackFlags.DoesntChargeRelics | Health.AttackFlags.ForceKill);
    });
    sequence.OnComplete += (System.Action) (() =>
    {
      sequence.OnImpact -= (Action<VFXObject, int>) ((vfxObject, targetIndex) =>
      {
        if (targetEnemies.Count <= targetIndex || !((UnityEngine.Object) targetEnemies[targetIndex] != (UnityEngine.Object) null))
          return;
        targetEnemies[targetIndex].GetComponent<IAttackResilient>()?.StopResilience();
        Vector3 position = targetEnemies[targetIndex].transform.position;
        vfxObject.transform.SetPositionAndRotation(position, Quaternion.identity);
        targetEnemies[targetIndex].invincible = false;
        targetEnemies[targetIndex].enabled = true;
        targetEnemies[targetIndex].HasShield = false;
        targetEnemies[targetIndex].DealDamage(targetEnemies[targetIndex].HP, this.gameObject, position, AttackType: Health.AttackTypes.NoHitStop, AttackFlags: Health.AttackFlags.DoesntChargeRelics | Health.AttackFlags.ForceKill);
      });
      sequence.OnComplete -= (System.Action) (() =>
      {
        // ISSUE: unable to decompile the method.
      });
    });
  }

  public void DiseasedHeartDamageEnemies()
  {
    List<Transform> transformList = new List<Transform>();
    List<Health> targetEnemies = new List<Health>();
    for (int index = Health.team2.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null)
      {
        targetEnemies.Add(Health.team2[index]);
        transformList.Add(Health.team2[index].transform);
      }
    }
    if (!TrinketManager.HasTrinket(TarotCards.Card.NoCorruption, this.playerFarming))
      this.playerFarming.health.DealDamage(1f, this.playerFarming.gameObject, this.transform.position, false, Health.AttackTypes.Melee, false, (Health.AttackFlags) 0);
    VFXSequence sequence = this.CurrentRelic.VFXData.PlayNewSequence(this.playerFarming.transform, transformList.ToArray());
    sequence.OnImpact += (Action<VFXObject, int>) ((vfxObject, targetIndex) =>
    {
      if (targetEnemies.Count <= targetIndex || !((UnityEngine.Object) targetEnemies[targetIndex] != (UnityEngine.Object) null) || TrinketManager.HasTrinket(TarotCards.Card.CorruptedFullCorruption, this.playerFarming))
        return;
      Vector3 position = targetEnemies[targetIndex].transform.position;
      vfxObject.transform.SetPositionAndRotation(position, Quaternion.identity);
      float Damage = PlayerWeapon.GetDamage(20f, this.playerFarming.currentWeaponLevel, this.playerFarming) * TrinketManager.GetRelicDamageMultiplier(this.playerFarming);
      targetEnemies[targetIndex].DealDamage(Damage, this.gameObject, position, AttackType: Health.AttackTypes.Projectile, AttackFlags: Health.AttackFlags.DoesntChargeRelics);
    });
    sequence.OnComplete += (System.Action) (() =>
    {
      sequence.OnImpact -= (Action<VFXObject, int>) ((vfxObject, targetIndex) =>
      {
        if (targetEnemies.Count <= targetIndex || !((UnityEngine.Object) targetEnemies[targetIndex] != (UnityEngine.Object) null) || TrinketManager.HasTrinket(TarotCards.Card.CorruptedFullCorruption, this.playerFarming))
          return;
        Vector3 position = targetEnemies[targetIndex].transform.position;
        vfxObject.transform.SetPositionAndRotation(position, Quaternion.identity);
        float Damage = PlayerWeapon.GetDamage(20f, this.playerFarming.currentWeaponLevel, this.playerFarming) * TrinketManager.GetRelicDamageMultiplier(this.playerFarming);
        targetEnemies[targetIndex].DealDamage(Damage, this.gameObject, position, AttackType: Health.AttackTypes.Projectile, AttackFlags: Health.AttackFlags.DoesntChargeRelics);
      });
      sequence.OnComplete -= (System.Action) (() =>
      {
        // ISSUE: unable to decompile the method.
      });
    });
  }

  public void HealPerFollower(RelicType relicType, int followersTotal)
  {
    this.CurrentRelic.VFXData.PlayNewSequence(this.playerFarming.transform, new Transform[1]
    {
      this.playerFarming.transform
    });
    this.StartCoroutine((IEnumerator) this.Delay(0.5f, true, (System.Action) (() =>
    {
      switch (relicType)
      {
        case RelicType.HealPerFollower:
          this.playerFarming.health.TotalSpiritHearts += (float) Mathf.FloorToInt((float) followersTotal / 5f);
          BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "red", "burst_big");
          AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", this.playerFarming.transform.position);
          break;
        case RelicType.HealPerFollower_Blessed:
          this.playerFarming.health.TotalSpiritHearts += (float) followersTotal;
          BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "red", "burst_big");
          AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", this.playerFarming.transform.position);
          break;
        case RelicType.HealPerFollower_Dammed:
          this.playerFarming.health.TotalSpiritHearts += (float) followersTotal;
          BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "red", "burst_big");
          AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", this.playerFarming.transform.position);
          break;
      }
    })));
  }

  public IEnumerator ExplosiveRingIE(int explosions, float radius, float duration)
  {
    PlayerRelic playerRelic = this;
    float increment = duration / (float) explosions;
    float angle = 360f / (float) explosions;
    float startingAngle = UnityEngine.Random.value * 360f;
    for (int i = 0; i < explosions; ++i)
    {
      Vector3 normalized = (Vector3) new Vector2(Mathf.Cos(startingAngle * ((float) Math.PI / 180f)), Mathf.Sin(startingAngle * ((float) Math.PI / 180f))).normalized;
      Vector3 position = playerRelic.transform.position + normalized * radius;
      float num = 1f * TrinketManager.GetRelicDamageMultiplier(playerRelic.playerFarming);
      HealthPlayer health = playerRelic.playerFarming.health;
      double Damage = (double) num;
      UnitObject[] unitObjectArray = Array.Empty<UnitObject>();
      Explosion.CreateExplosion(position, Health.Team.PlayerTeam, (Health) health, 2f, (float) Damage, -1f, false, 1f, (Health.AttackFlags) 0, true, false, true, unitObjectArray);
      startingAngle = Mathf.Repeat(startingAngle + angle, 360f);
      yield return (object) new WaitForSeconds(increment);
    }
  }

  public void TakeDamageOnRelicUse(Transform playerTransform)
  {
    Health component = playerTransform.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.StartCoroutine((IEnumerator) this.TakeDamageOnRelicUse(component, playerTransform));
  }

  public IEnumerator TakeDamageOnRelicUse(Health playerHealth, Transform playerTransform)
  {
    PlayerRelic playerRelic = this;
    yield return (object) new WaitForSeconds(1.5f);
    BiomeConstants.Instance.ShowTarotCardDamage(playerTransform.transform, Vector3.up);
    yield return (object) new WaitForSeconds(0.5f);
    playerHealth.DealDamage(1f, playerTransform.gameObject, playerTransform.position);
    BiomeConstants.Instance.PlayerEmitHitImpactEffect(playerRelic.transform.position + Vector3.back * 0.5f, Quaternion.identity.z, false);
    if (CoopManager.CoopActive && Health.team2.Count <= 0)
    {
      bool flag = true;
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if (!player.IsKnockedOut)
        {
          flag = false;
          break;
        }
      }
      Interaction_CoopRevive reviveItneraction = playerRelic.playerFarming.GetComponent<Interaction_CoopRevive>();
      bool wasReviveActive = reviveItneraction.enabled;
      if (!flag)
      {
        while (!wasReviveActive && !reviveItneraction.enabled)
          yield return (object) null;
        CoopManager.Instance.WakeAllKnockedOutPlayers();
      }
      reviveItneraction = (Interaction_CoopRevive) null;
    }
  }

  public void FreezeTime(VFXSequence freezeSequence)
  {
    // ISSUE: variable of a compiler-generated type
    PlayerRelic.\u003C\u003Ec__DisplayClass126_0 displayClass1260;
    freezeSequence.OnImpact += (Action<VFXObject, int>) ((vfxObject1, i1) =>
    {
      AudioManager.Instance.PauseForFreezeTime();
      freezeSequence.OnImpact -= (Action<VFXObject, int>) ((vfxObject2, i2) =>
      {
        // ISSUE: unable to decompile the method.
      });
      vfxObject1.OnStopped += (Action<VFXObject>) (o =>
      {
        vfxObject1.OnStopped -= (Action<VFXObject>) (o2 =>
        {
          // ISSUE: unable to decompile the method.
        });
        vfxObject1.OnDisabled -= (Action<VFXObject>) (o3 =>
        {
          // ISSUE: unable to decompile the method.
        });
        AudioManager.Instance.ToggleFilter("freeze", false);
        AudioManager.Instance.ResumeForFreezeTime();
        // ISSUE: reference to a compiler-generated method
        displayClass1260.\u003CFreezeTime\u003Eg__UnFreezeTimeEnemies\u007C2();
      });
      vfxObject1.OnDisabled += (Action<VFXObject>) (o =>
      {
        vfxObject1.OnStopped -= (Action<VFXObject>) (o5 =>
        {
          // ISSUE: unable to decompile the method.
        });
        vfxObject1.OnDisabled -= (Action<VFXObject>) (o6 =>
        {
          // ISSUE: unable to decompile the method.
        });
        AudioManager.Instance.ToggleFilter("freeze", false);
        AudioManager.Instance.ResumeForFreezeTime();
        // ISSUE: reference to a compiler-generated method
        displayClass1260.\u003CFreezeTime\u003Eg__UnFreezeTimeEnemies\u007C2();
      });
      AudioManager.Instance.PlayOneShot("event:/relics/freeze_time", this.gameObject);
      AudioManager.Instance.ToggleFilter("freeze", true);
      // ISSUE: reference to a compiler-generated method
      this.\u003CFreezeTime\u003Eg__FreezeTimeEnemies\u007C1();
    });
  }

  public void PlayCOOPRune()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((UnityEngine.Object) player != (UnityEngine.Object) this.playerFarming)
        this.coopRune.PlayNewSequence(player.transform, new Transform[1]
        {
          player.transform
        });
    }
  }

  public void GiveSkin(PlayerFarming playerFarming)
  {
    if (DataManager.GetFollowerSkinUnlocked("Wombat"))
      return;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.gameObject, 6f);
    FollowerSkinCustomTarget.Create(playerFarming.Spine.transform.position, playerFarming.Spine.transform.position, 2f, "Wombat", (System.Action) (() =>
    {
      GameManager.GetInstance().OnConversationEnd();
      GameManager.GetInstance().AddPlayerToCamera();
    }));
    DataManager.SetFollowerSkinUnlocked("Wombat");
  }

  public IEnumerator BurrowRoutine(RelicType relicType)
  {
    PlayerRelic playerRelic = this;
    string soundPath1 = relicType == RelicType.FieryBurrow ? playerRelic.relicFireBurrowTriggerSFX : playerRelic.relicIceBurrowTriggerSFX;
    string soundPath2 = relicType == RelicType.FieryBurrow ? playerRelic.relicFireBurrowLoopSFX : playerRelic.relicIceBurrowLoopSFX;
    string burrowStopSFX = relicType == RelicType.FieryBurrow ? playerRelic.relicFireBurrowStopSFX : playerRelic.relicIceBurrowStopSFX;
    AudioManager.Instance.PlayOneShot(soundPath1, playerRelic.gameObject);
    playerRelic.burrowLoopInstance = AudioManager.Instance.CreateLoop(soundPath2, playerRelic.gameObject, true);
    playerRelic.playerFarming.playerWeapon.enabled = false;
    playerRelic.playerFarming.playerSpells.enabled = false;
    playerRelic.playerFarming.AllowDodging = false;
    playerRelic.onEndBurrow = (System.Action) (() =>
    {
      this.playerFarming.playerWeapon.enabled = true;
      this.playerFarming.playerSpells.enabled = true;
      this.playerFarming.AllowDodging = true;
      this.onEndBurrow = (System.Action) null;
      this.playerFarming.playerController.ResetSpecificMovementAnimations();
      this.playerFarming.IsBurrowing = false;
      this.GiveSkin(this.playerFarming);
      AudioManager.Instance.StopLoop(this.burrowLoopInstance);
    });
    playerRelic.playerFarming.CustomAnimationWithCallback("burrow-in", false, (System.Action) (() =>
    {
      this.SpawnTrails(relicType, this.DelayBetweenTrails);
      this.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
      this.playerFarming.IsBurrowing = true;
      string str = "burrowed";
      this.playerFarming.playerController.SetSpecificMovementAnimations(str, str, str, str, str, str);
      BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
    }));
    while (playerRelic.playerFarming.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    float Progress = 0.0f;
    float Duration = 4f;
    playerRelic.playerFarming.playerController.MakeInvincible(Duration);
    while ((double) (Progress += Time.deltaTime * playerRelic.playerFarming.Spine.timeScale) < (double) Duration)
    {
      if (playerRelic.playerFarming.GoToAndStopping || playerRelic.playerFarming.state.CURRENT_STATE != StateMachine.State.Idle && playerRelic.playerFarming.state.CURRENT_STATE != StateMachine.State.Moving || MMConversation.isPlaying)
      {
        System.Action onEndBurrow = playerRelic.onEndBurrow;
        if (onEndBurrow == null)
          yield break;
        onEndBurrow();
        yield break;
      }
      playerRelic.SpawnTrails(relicType, playerRelic.DelayBetweenTrails);
      yield return (object) null;
    }
    AudioManager.Instance.PlayOneShot(burrowStopSFX, playerRelic.gameObject);
    AudioManager.Instance.StopLoop(playerRelic.burrowLoopInstance);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(playerRelic.transform.position);
    playerRelic.playerFarming.CustomAnimationWithCallback("burrow-out", false, (System.Action) (() =>
    {
      this.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
      System.Action onEndBurrow = this.onEndBurrow;
      if (onEndBurrow == null)
        return;
      onEndBurrow();
    }));
  }

  public void SpawnTrails(RelicType relicType, float delayBetweenTrails)
  {
    if ((double) (this.TrailsTimer += Time.deltaTime) <= (double) delayBetweenTrails || (double) Vector3.Distance(this.transform.position, this.previousSpawnPosition) <= 0.10000000149011612)
      return;
    this.TrailsTimer = 0.0f;
    this.t = (GameObject) null;
    if (relicType == RelicType.FieryBurrow)
    {
      if (this.FieryTrails.Count > 0)
      {
        foreach (GameObject fieryTrail in this.FieryTrails)
        {
          if (!fieryTrail.activeSelf)
          {
            this.t = fieryTrail;
            this.t.transform.position = this.transform.position;
            this.t.SetActive(true);
            this.t.GetComponentInChildren<SimpleSpineSkineRandomiser>().Start();
            if (relicType == RelicType.FieryBurrow)
            {
              AudioManager.Instance.PlayOneShot("event:/dlc/tarot/verglasshard_icespike_spawn", this.t.transform.position);
              GameManager.GetInstance().WaitForSeconds(0.5f, (System.Action) (() => AudioManager.Instance.PlayOneShot("event:/dlc/tarot/verglasshard_icespike_despawn", this.t.transform.position)));
              break;
            }
            break;
          }
        }
      }
    }
    else if (this.IceyTrails.Count > 0)
    {
      foreach (GameObject iceyTrail in this.IceyTrails)
      {
        if (!iceyTrail.activeSelf)
        {
          this.t = iceyTrail;
          this.t.transform.position = this.transform.position;
          this.t.SetActive(true);
          this.t.GetComponentInChildren<SimpleSpineSkineRandomiser>().Start();
          if (relicType != RelicType.IceyBurrow)
          {
            if (relicType != RelicType.IceSpikes)
              break;
          }
          AudioManager.Instance.PlayOneShot("event:/dlc/tarot/verglasshard_icespike_spawn", this.t.transform.position);
          GameManager.GetInstance().WaitForSeconds(0.5f, (System.Action) (() => AudioManager.Instance.PlayOneShot("event:/dlc/tarot/verglasshard_icespike_despawn", this.t.transform.position)));
          break;
        }
      }
    }
    if ((UnityEngine.Object) this.t == (UnityEngine.Object) null)
    {
      Transform parent = this.transform.parent;
      if (BiomeGenerator.Instance.CurrentRoom != null && (UnityEngine.Object) BiomeGenerator.Instance.CurrentRoom.generateRoom != (UnityEngine.Object) null)
        parent = BiomeGenerator.Instance.CurrentRoom.generateRoom.transform;
      if (relicType == RelicType.FieryBurrow)
      {
        this.t = UnityEngine.Object.Instantiate<GameObject>(this.FieryTrailPrefab, this.transform.position, Quaternion.identity, parent);
        this.FieryTrails.Add(this.t);
      }
      else
      {
        this.t = UnityEngine.Object.Instantiate<GameObject>(this.IceyTrailPrefab, this.transform.position, Quaternion.identity, parent);
        this.IceyTrails.Add(this.t);
        AudioManager.Instance.PlayOneShot("event:/dlc/tarot/verglasshard_icespike_spawn", this.t.transform.position);
        GameManager.GetInstance().WaitForSeconds(0.5f, (System.Action) (() => AudioManager.Instance.PlayOneShot("event:/dlc/tarot/verglasshard_icespike_despawn", this.t.transform.position)));
      }
      ColliderEvents componentInChildren = this.t.GetComponentInChildren<ColliderEvents>();
      if ((bool) (UnityEngine.Object) componentInChildren)
        componentInChildren.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnBurrowDamageTriggerEnter);
    }
    this.previousSpawnPosition = this.t.transform.position;
  }

  public void OnBurrowDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.playerFarming.health.team && !this.playerFarming.health.IsCharmedEnemy)
      return;
    Health.AttackFlags AttackFlags = Health.AttackFlags.DoesntChargeRelics;
    if ((UnityEngine.Object) this.CurrentRelic != (UnityEngine.Object) null && this.CurrentRelic.RelicType == RelicType.FieryBurrow)
      AttackFlags |= Health.AttackFlags.Burn;
    else if ((UnityEngine.Object) this.CurrentRelic == (UnityEngine.Object) null || this.CurrentRelic.RelicType == RelicType.IceyBurrow)
      AttackFlags |= Health.AttackFlags.Ice;
    component.DealDamage(PlayerWeapon.GetDamage(0.1f, this.playerFarming.currentWeaponLevel, this.playerFarming), this.gameObject, component.transform.position, AttackFlags: AttackFlags);
  }

  public IEnumerator SpawnIceSpikes()
  {
    PlayerRelic playerRelic = this;
    LayerMask layerMask = (LayerMask) ((int) (LayerMask) ((int) new LayerMask() | 1 << LayerMask.NameToLayer("Island")) | 1 << LayerMask.NameToLayer("Obstacles Player Ignore"));
    float x = Physics2D.Raycast((Vector2) Vector3.zero, (Vector2) Vector3.right, 100f, (int) layerMask).point.x;
    float y = Physics2D.Raycast((Vector2) Vector3.zero, (Vector2) Vector3.up, 100f, (int) layerMask).point.y;
    AudioManager.Instance.PlayOneShot(playerRelic.relicIceSpikesTriggerSFX, playerRelic.gameObject);
    playerRelic.iceSpikesLoopInstance = AudioManager.Instance.CreateLoop(playerRelic.relicIceSpikesLoopSFX, true);
    int amountOfSpikes = UnityEngine.Random.Range(25, 40);
    for (int i = 0; i < amountOfSpikes; ++i)
    {
      Vector3 position = new Vector3(UnityEngine.Random.Range(-x, x), UnityEngine.Random.Range(-y, y));
      playerRelic.SpawnIceSpike(position);
      yield return (object) CoroutineStatics.WaitForScaledSeconds(0.25f, playerRelic.playerFarming.Spine);
    }
    AudioManager.Instance.StopLoop(playerRelic.iceSpikesLoopInstance);
  }

  public void SpawnIceSpike(Vector3 position)
  {
    Tentacle component = UnityEngine.Object.Instantiate<GameObject>(EquipmentManager.GetCurseData(EquipmentType.Tentacles_Ice).Prefab, (UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null ? GenerateRoom.Instance.transform : this.transform.parent, true).GetComponent<Tentacle>();
    component.transform.position = position;
    component.SetOwner(this.gameObject);
    component.AttackFlags = Health.AttackFlags.Ice | Health.AttackFlags.DoesntChargeRelics;
    float damage = EquipmentManager.GetCurseData(EquipmentType.Tentacles_Ice).Damage * PlayerSpells.GetCurseDamageMultiplier(this.playerFarming) * TrinketManager.GetRelicDamageMultiplier(this.playerFarming);
    component.Play(0.0f, 0.5f, damage, this.playerFarming.health.team, false, 0, true, playExitSFX: false, playLoop: false);
    AudioManager.Instance.PlayOneShot(this.relicIceSpikesSingleBurstSFX, component.gameObject);
  }

  [CompilerGenerated]
  public void \u003CSpawnTrails\u003Eb__139_0()
  {
    AudioManager.Instance.PlayOneShot("event:/dlc/tarot/verglasshard_icespike_despawn", this.t.transform.position);
  }

  [CompilerGenerated]
  public void \u003CSpawnTrails\u003Eb__139_1()
  {
    AudioManager.Instance.PlayOneShot("event:/dlc/tarot/verglasshard_icespike_despawn", this.t.transform.position);
  }

  [CompilerGenerated]
  public void \u003CSpawnTrails\u003Eb__139_2()
  {
    AudioManager.Instance.PlayOneShot("event:/dlc/tarot/verglasshard_icespike_despawn", this.t.transform.position);
  }

  public delegate void RelicEvent(RelicData relic, PlayerFarming target = null);

  public enum BonusType
  {
    GainStrength,
    GainBlueHearts,
    GainBlackHearts,
    TakeDamage,
    GainSpiritHearts,
    GainInvincibility,
    LoseHeart,
  }
}
