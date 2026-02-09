// Decompiled with JetBrains decompiler
// Type: Familiar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMBiomeGeneration;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Familiar : MonoBehaviour
{
  public static List<Familiar> Familiars = new List<Familiar>();
  [SerializeField]
  public Familiar.MovementType familiarType;
  [SerializeField]
  public GameObject container;
  [SerializeField]
  public GameObject shadow;
  [SerializeField]
  public float minDistance;
  [SerializeField]
  public float hardMaxSpeed = -1f;
  [SerializeField]
  public Familiar.CombatType combatType;
  [SerializeField]
  public float baseVariable;
  [SerializeField]
  public ColliderEvents colliderEvents;
  [Space]
  [SerializeField]
  public float lifetime;
  [SerializeField]
  public float timeBetween;
  [SerializeField]
  public SpriteRenderer sprite;
  [SerializeField]
  public SkeletonAnimation spine;
  public Vector3 wanderPos;
  public GameObject testPos;
  public float spineVZ;
  public float spineVY;
  public float vx;
  public float vy;
  public float bobbing;
  public float targetAngle;
  public float speed;
  public float timestamp;
  public float whiteFlashDuration = 0.2f;
  public float squashDuration = 0.2f;
  public float stretchDuration = 0.2f;
  public float squashScale = 0.7f;
  public float stretchScale = 1.7f;
  public Vector3 originalScale;
  public int direction = 1;
  public bool destroyingSelf;
  public PlayerFarming master;
  public Health health;
  public StateMachine state;
  public Renderer[] renderers;
  public float seperatorVX;
  public float seperatorVY;
  public float wanderTimestamp;
  public float wanderDirection;
  [CompilerGenerated]
  public float \u003CDamageMultiplier\u003Ek__BackingField = 1f;
  public int wanderCount;
  public GameObject whiteFlash;
  public Coroutine hitEffectsRoutine;

  public GameObject Container => this.container;

  public Health.Team team
  {
    get
    {
      if ((UnityEngine.Object) this.health == (UnityEngine.Object) null && (UnityEngine.Object) this.master != (UnityEngine.Object) null)
        this.health = this.master.GetComponent<Health>();
      return (UnityEngine.Object) this.health == (UnityEngine.Object) null ? Health.Team.PlayerTeam : this.health.team;
    }
  }

  public float maxSpeed
  {
    get
    {
      if ((UnityEngine.Object) this.master == (UnityEngine.Object) null)
        this.master = PlayerFarming.Instance;
      if ((double) this.hardMaxSpeed != -1.0)
        return this.hardMaxSpeed;
      if (!((UnityEngine.Object) this.master != (UnityEngine.Object) null))
        return 1f;
      return this.master.state.CURRENT_STATE == StateMachine.State.Dodging ? this.master.playerController.DodgeSpeed : this.master.playerController.GetPlayerMaxSpeed();
    }
  }

  public float DamageMultiplier
  {
    get => this.\u003CDamageMultiplier\u003Ek__BackingField;
    set => this.\u003CDamageMultiplier\u003Ek__BackingField = value;
  }

  public void Awake()
  {
    this.state = this.GetComponent<StateMachine>();
    this.colliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnTriggerEnterEvent);
    this.originalScale = this.transform.localScale;
    this.renderers = this.GetComponentsInChildren<Renderer>();
    this.direction = (double) UnityEngine.Random.value > 0.5 ? 1 : -1;
    this.state.facingAngle += this.direction == -1 ? 180f : 0.0f;
    this.timestamp = Time.time + 2.5f;
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
  }

  public void OnDestroy()
  {
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
  }

  public void SetDirection(int dir)
  {
    this.direction = dir;
    this.state.facingAngle += this.direction == -1 ? 180f : 0.0f;
  }

  public void ShootCurse(EquipmentType type)
  {
    if (this.combatType != Familiar.CombatType.ShootCurrentCurse)
      return;
    this.timestamp = Time.time + this.timeBetween;
    this.sprite.material.DOFloat(1f, "_FillAlpha", 0.5f);
    this.StartCoroutine((IEnumerator) this.Delay(0.5f, (System.Action) (() =>
    {
      float damageMultiplier = TrinketManager.GetRelicDamageMultiplier(PlayerFarming.GetPlayerFarmingComponent(this.master.gameObject));
      this.sprite.material.DOFloat(0.0f, "_FillAlpha", 0.0f);
      PlayerFarming.Instance.playerSpells.CastSpell(type, wasSpell: false, smallScale: true, shooter: this.gameObject, damageMultiplier: damageMultiplier, isFromFamiliar: true);
    })));
  }

  public void OnEnable() => Familiar.Familiars.Add(this);

  public void OnDisable() => Familiar.Familiars.Remove(this);

  public void SetMaster(PlayerFarming master, float startingHeight = -1.25f)
  {
    this.master = master;
    this.spineVZ = startingHeight;
    if ((UnityEngine.Object) this.state == (UnityEngine.Object) null)
      this.state = this.GetComponent<StateMachine>();
    if (this.familiarType == Familiar.MovementType.Follow)
      this.UpdateFollowState();
    else if (this.familiarType == Familiar.MovementType.Spin)
    {
      this.UpdateSpinState();
    }
    else
    {
      if (this.familiarType != Familiar.MovementType.BetweenPlayers)
        return;
      this.UpdateBetweenState();
    }
  }

  public void FreezeForCutscene()
  {
    this.state.CURRENT_STATE = StateMachine.State.InActive;
    this.colliderEvents.enabled = false;
  }

  public void UnFreezeForCutscene()
  {
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.colliderEvents.enabled = true;
  }

  public virtual void Update()
  {
    if (this.state.CURRENT_STATE == StateMachine.State.InActive)
      return;
    if ((UnityEngine.Object) this.master == (UnityEngine.Object) null && (bool) (UnityEngine.Object) PlayerFarming.Instance)
      this.master = PlayerFarming.Instance;
    if ((UnityEngine.Object) this.master == (UnityEngine.Object) null || (UnityEngine.Object) RespawnRoomManager.Instance != (UnityEngine.Object) null && RespawnRoomManager.Instance.gameObject.activeSelf || (UnityEngine.Object) DeathCatRoomManager.Instance != (UnityEngine.Object) null && DeathCatRoomManager.Instance.gameObject.activeSelf || (UnityEngine.Object) MysticShopKeeperManager.Instance != (UnityEngine.Object) null && MysticShopKeeperManager.Instance.gameObject.activeSelf)
    {
      this.container.gameObject.SetActive(false);
      this.shadow.gameObject.SetActive(false);
    }
    else
    {
      this.container.gameObject.SetActive(true);
      this.shadow.gameObject.SetActive(true);
    }
    if ((UnityEngine.Object) this.master == (UnityEngine.Object) null || Health.isGlobalTimeFreeze)
      return;
    if (this.familiarType == Familiar.MovementType.Follow || this.familiarType == Familiar.MovementType.Wander)
      this.UpdateFollowState();
    else if (this.familiarType == Familiar.MovementType.Spin)
      this.UpdateSpinState();
    else if (this.familiarType == Familiar.MovementType.BetweenPlayers)
      this.UpdateBetweenState();
    if ((double) this.lifetime != -1.0)
    {
      this.lifetime -= Time.deltaTime;
      if ((double) this.lifetime <= 0.0 && !this.destroyingSelf)
      {
        this.destroyingSelf = true;
        foreach (Renderer renderer in this.renderers)
          renderer.material.DOFade(0.0f, 0.5f);
      }
      else if ((double) this.lifetime < -0.5 && this.destroyingSelf)
      {
        BiomeConstants.Instance.EmitSmokeInteractionVFX(this.container.transform.position, Vector3.one * 0.5f);
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      }
    }
    if ((double) Time.time <= (double) this.timestamp || (this.team != Health.Team.PlayerTeam || Health.team2.Count <= 0) && this.team == Health.Team.PlayerTeam)
      return;
    this.ShootCurse(PlayerFarming.Instance.currentCurse);
  }

  public virtual void UpdateFollowState()
  {
    float num1 = Vector3.Distance(this.transform.position, this.master.transform.position);
    if (this.familiarType == Familiar.MovementType.Wander)
    {
      if ((double) Time.time > (double) this.wanderTimestamp)
      {
        ++this.wanderCount;
        int num2 = 0;
        this.wanderPos = this.combatType != Familiar.CombatType.Decoy || this.wanderCount % 3 != 0 ? BiomeGenerator.GetRandomPositionInIsland() : new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f));
        if (this.combatType == Familiar.CombatType.Decoy)
        {
          while (num2 < 10)
          {
            ++num2;
            this.wanderPos = BiomeGenerator.GetRandomPositionInIsland();
            if (this.lineOfSightPlayers((Vector2) this.wanderPos, true))
              break;
          }
        }
        if ((bool) (UnityEngine.Object) this.testPos)
        {
          this.testPos.transform.SetParent(this.transform.parent);
          this.testPos.transform.position = this.wanderPos;
        }
        this.wanderDirection = Utils.GetAngle(this.transform.position, this.wanderPos);
        this.wanderTimestamp = Time.time + UnityEngine.Random.Range(2f, 4f);
      }
      this.targetAngle = this.wanderDirection;
      this.state.facingAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.targetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.targetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / (15.0 * (double) GameManager.DeltaTime));
    }
    else
    {
      this.targetAngle = Utils.GetAngle(this.transform.position, this.master.transform.position);
      this.state.facingAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.targetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.targetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / (15.0 * (double) GameManager.DeltaTime));
    }
    if ((double) num1 < (double) this.minDistance)
      this.speed = Mathf.Clamp(this.speed - 0.35f * GameManager.DeltaTime, 0.0f, 100f);
    else
      this.speed += (float) (((double) this.maxSpeed - (double) this.speed) / (15.0 * (double) GameManager.DeltaTime));
    if (float.IsNaN(this.speed))
      this.speed = this.maxSpeed;
    if (float.IsNaN(this.state.facingAngle))
      this.state.facingAngle = 0.0f;
    this.vx = this.speed * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)) * Time.deltaTime;
    this.vy = this.speed * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)) * Time.deltaTime;
    if (float.IsNaN(this.vx))
      this.vx = 0.0f;
    if (float.IsNaN(this.vy))
      this.vy = 0.0f;
    if ((UnityEngine.Object) this.spine != (UnityEngine.Object) null)
      this.spine.transform.localScale = new Vector3((double) this.vx > 0.0 ? -1f : 1f, this.spine.transform.localScale.y, this.spine.transform.localScale.z);
    this.transform.position = this.transform.position + new Vector3(this.vx, this.vy);
    this.container.transform.eulerAngles = (double) Time.deltaTime != 0.0 ? new Vector3(-60f, 0.0f, this.vx * -5f / Time.deltaTime) : new Vector3(-60f, 0.0f, this.vx * -5f);
    this.spineVZ = Mathf.Lerp(this.spineVZ, -1f, 5f * Time.deltaTime);
    this.spineVY = Mathf.Lerp(this.spineVY, 0.5f, 5f * Time.deltaTime);
    this.container.transform.localPosition = new Vector3(0.0f, 0.0f, this.spineVZ + 0.1f * Mathf.Cos(this.bobbing += 5f * Time.deltaTime));
  }

  public void FixedUpdate()
  {
    if (this.combatType != Familiar.CombatType.Decoy)
      return;
    this.Seperate(1f);
    this.transform.position = this.transform.position + new Vector3(this.seperatorVX, this.seperatorVY);
    if ((double) Vector3.Distance(this.wanderPos, this.transform.position) < 1.0)
      this.wanderTimestamp = 0.0f;
    this.lineOfSightPlayers((Vector2) this.transform.position);
  }

  public bool lineOfSightPlayers(Vector2 pos, bool justCheck = false)
  {
    if (PlayerFarming.players == null || PlayerFarming.players.Count == 0)
      return true;
    PlayerFarming playerFarming = (PlayerFarming) null;
    float num1 = float.MaxValue;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (!((UnityEngine.Object) player == (UnityEngine.Object) null))
      {
        float num2 = Vector2.Distance((Vector2) player.transform.position, pos);
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          playerFarming = player;
        }
      }
    }
    if ((UnityEngine.Object) playerFarming == (UnityEngine.Object) null || this.combatType == Familiar.CombatType.Decoy)
      return true;
    LayerMask mask = (LayerMask) LayerMask.GetMask("Island", "Obstacles");
    Vector2 vector2_1 = (Vector2) playerFarming.transform.position - (Vector2) this.transform.position;
    float magnitude = vector2_1.magnitude;
    if ((double) magnitude <= 0.10000000149011612)
      return true;
    Vector2 vector2_2 = vector2_1 / magnitude;
    float num3 = Mathf.Min(0.1f * magnitude, 1f);
    Vector2 origin = pos + vector2_2 * num3;
    float num4 = magnitude - num3;
    Vector2 direction = vector2_2;
    double distance = (double) num4;
    int layerMask = (int) mask;
    RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, direction, (float) distance, layerMask);
    if (!((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null))
      return true;
    if (!justCheck)
    {
      this.transform.position = (Vector3) raycastHit2D.point;
      this.wanderTimestamp = 0.0f;
    }
    return false;
  }

  public virtual void UpdateSpinState()
  {
    this.vx = Mathf.Cos(this.state.facingAngle) * this.minDistance;
    this.vy = Mathf.Sin(this.state.facingAngle) * this.minDistance;
    this.transform.position = Vector3.Lerp(this.transform.position, this.master.transform.position + new Vector3(this.vx, this.vy), 9f * Time.deltaTime);
    this.state.facingAngle += Time.deltaTime * (float) (2 * this.direction);
    this.spineVZ = Mathf.Lerp(this.spineVZ, -1f, 5f * Time.deltaTime);
    this.spineVY = Mathf.Lerp(this.spineVY, 0.5f, 5f * Time.deltaTime);
    this.container.transform.localPosition = new Vector3(0.0f, 0.0f, this.spineVZ + 0.1f * Mathf.Cos(this.bobbing += 5f * Time.deltaTime));
  }

  public virtual void UpdateBetweenState()
  {
    double num = (double) Vector3.Distance(this.transform.position, this.master.transform.position);
    this.targetAngle = Utils.GetAngle(this.transform.position, this.master.transform.position);
    this.state.facingAngle += (float) ((double) Mathf.Atan2(Mathf.Sin((float) (((double) this.targetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0))), Mathf.Cos((float) (((double) this.targetAngle - (double) this.state.facingAngle) * (Math.PI / 180.0)))) * 57.295780181884766 / (15.0 * (double) GameManager.DeltaTime));
    double minDistance = (double) this.minDistance;
    if (num < minDistance)
    {
      this.speed = Mathf.Clamp(this.speed - 0.35f * GameManager.DeltaTime, 0.0f, 100f);
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if ((UnityEngine.Object) player != (UnityEngine.Object) this.master)
        {
          this.master = player;
          break;
        }
      }
    }
    else
      this.speed += (float) (((double) this.maxSpeed - (double) this.speed) / (15.0 * (double) GameManager.DeltaTime));
    if (float.IsNaN(this.speed))
      this.speed = this.maxSpeed;
    if (float.IsNaN(this.state.facingAngle))
      this.state.facingAngle = 0.0f;
    this.vx = this.speed * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)) * Time.deltaTime;
    this.vy = this.speed * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)) * Time.deltaTime;
    if (float.IsNaN(this.vx))
      this.vx = 0.0f;
    if (float.IsNaN(this.vy))
      this.vy = 0.0f;
    this.transform.position = this.transform.position + new Vector3(this.vx, this.vy);
    this.container.transform.eulerAngles = (double) Time.deltaTime != 0.0 ? new Vector3(-60f, 0.0f, this.vx * -5f / Time.deltaTime) : new Vector3(-60f, 0.0f, this.vx * -5f);
    this.spineVZ = Mathf.Lerp(this.spineVZ, -1f, 5f * Time.deltaTime);
    this.spineVY = Mathf.Lerp(this.spineVY, 0.5f, 5f * Time.deltaTime);
    this.container.transform.localPosition = new Vector3(0.0f, 0.0f, this.spineVZ + 0.1f * Mathf.Cos(this.bobbing += 5f * Time.deltaTime));
  }

  public void OnTriggerEnterEvent(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.team || component.team == Health.Team.Neutral)
      return;
    PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(this.master.gameObject);
    bool flag = false;
    if (!component.IsHidden && this.combatType == Familiar.CombatType.DamageOnTouch)
    {
      float Damage = (this.team == Health.Team.PlayerTeam ? PlayerWeapon.GetDamage(this.baseVariable, farmingComponent.currentWeaponLevel, farmingComponent) : 1f) * TrinketManager.GetRelicDamageMultiplier(farmingComponent) * this.DamageMultiplier;
      flag = component.DealDamage(Damage, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.NoHitStop);
    }
    else if (component.CanBePoisoned && !component.IsHidden && this.combatType == Familiar.CombatType.PoisonOnTouch)
    {
      float Damage = (this.team == Health.Team.PlayerTeam ? PlayerWeapon.GetDamage(0.1f, farmingComponent.currentWeaponLevel, farmingComponent) : 1f) * TrinketManager.GetRelicDamageMultiplier(farmingComponent);
      flag = component.DealDamage(Damage, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.NoHitStop);
      component.AddPoison(this.gameObject, this.baseVariable);
    }
    else if (component.CanBeIced && !component.IsHidden && this.combatType == Familiar.CombatType.FreezeOnTouch)
    {
      float Damage = (this.team == Health.Team.PlayerTeam ? PlayerWeapon.GetDamage(0.1f, farmingComponent.currentWeaponLevel, farmingComponent) : 1f) * TrinketManager.GetRelicDamageMultiplier(farmingComponent);
      flag = component.DealDamage(Damage, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.NoHitStop);
      component.AddIce(this.baseVariable);
    }
    else if (this.combatType == Familiar.CombatType.IgniteOnTouch)
    {
      float Damage = (this.team == Health.Team.PlayerTeam ? PlayerWeapon.GetDamage(0.1f, farmingComponent.currentWeaponLevel, farmingComponent) : 1f) * TrinketManager.GetRelicDamageMultiplier(farmingComponent);
      flag = component.DealDamage(Damage, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.NoHitStop);
      component.AddBurn(farmingComponent.gameObject, this.baseVariable);
    }
    if (!flag)
      return;
    if (this.hitEffectsRoutine != null)
      this.StopCoroutine(this.hitEffectsRoutine);
    this.hitEffectsRoutine = this.StartCoroutine((IEnumerator) this.DoHitVisualEffects());
  }

  public IEnumerator DoHitVisualEffects()
  {
    Familiar familiar = this;
    DG.Tweening.Sequence sequence = DOTween.Sequence();
    sequence.Append((Tween) familiar.transform.DOScale(new Vector3(familiar.squashScale, familiar.stretchScale, familiar.originalScale.z), familiar.squashDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad));
    sequence.Append((Tween) familiar.transform.DOScale(new Vector3(familiar.stretchScale, familiar.squashScale, familiar.originalScale.z), familiar.stretchDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad));
    sequence.Append((Tween) familiar.transform.DOScale(familiar.originalScale, familiar.squashDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad));
    sequence.Play<DG.Tweening.Sequence>();
    if ((bool) (UnityEngine.Object) familiar.whiteFlash && BiomeConstants.Instance.IsFlashLightsActive)
    {
      familiar.whiteFlash.SetActive(true);
      yield return (object) new WaitForSeconds(familiar.whiteFlashDuration);
      familiar.whiteFlash.SetActive(false);
    }
  }

  public void BiomeGenerator_OnBiomeChangeRoom()
  {
    this.transform.position = (!((UnityEngine.Object) this.master == (UnityEngine.Object) null) ? this.master.transform : PlayerFarming.Instance.transform).position + Vector3.right;
    if (this.combatType != Familiar.CombatType.Decoy)
      return;
    this.wanderPos = new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f));
    this.wanderTimestamp = Time.time + 2f;
    this.wanderCount = 0;
  }

  public IEnumerator Delay(float seconds, System.Action callback)
  {
    yield return (object) new WaitForSeconds(seconds);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public bool TryGetRelicType(out RelicType relicType)
  {
    switch (this.combatType)
    {
      case Familiar.CombatType.DamageOnTouch:
        relicType = this.familiarType != Familiar.MovementType.BetweenPlayers ? RelicType.DamageOnTouch_Familiar : RelicType.DamageEye_Coop;
        return true;
      case Familiar.CombatType.FreezeOnTouch:
        relicType = RelicType.FreezeOnTouch_Familiar;
        return true;
      case Familiar.CombatType.PoisonOnTouch:
        relicType = RelicType.PoisonOnTouch_Familiar;
        return true;
      case Familiar.CombatType.ShootCurrentCurse:
        relicType = RelicType.ShootCurses_Familiar;
        return true;
      case Familiar.CombatType.Decoy:
        relicType = RelicType.None;
        return false;
      default:
        relicType = RelicType.None;
        Debug.LogError((object) $"Unhandled CombatType = {this.combatType}");
        return false;
    }
  }

  public void Seperate(float SeperationRadius)
  {
    this.seperatorVX = 0.0f;
    this.seperatorVY = 0.0f;
    foreach (Familiar familiar in Familiar.Familiars)
    {
      if ((UnityEngine.Object) familiar != (UnityEngine.Object) null && (UnityEngine.Object) familiar != (UnityEngine.Object) this && familiar.combatType == Familiar.CombatType.Decoy)
      {
        float num = Vector2.Distance((Vector2) familiar.gameObject.transform.position, (Vector2) this.transform.position);
        float angle = Utils.GetAngle(familiar.gameObject.transform.position, this.transform.position);
        if ((double) num < (double) SeperationRadius)
        {
          this.seperatorVX += (float) (((double) SeperationRadius - (double) num) / 2.0) * Mathf.Cos(angle * ((float) Math.PI / 180f)) * GameManager.FixedDeltaTime;
          this.seperatorVY += (float) (((double) SeperationRadius - (double) num) / 2.0) * Mathf.Sin(angle * ((float) Math.PI / 180f)) * GameManager.FixedDeltaTime;
        }
      }
    }
  }

  public enum MovementType
  {
    Follow,
    Spin,
    BetweenPlayers,
    Wander,
  }

  public enum CombatType
  {
    DamageOnTouch,
    FreezeOnTouch,
    PoisonOnTouch,
    ShootCurrentCurse,
    IgniteOnTouch,
    Decoy,
  }
}
