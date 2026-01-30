// Decompiled with JetBrains decompiler
// Type: LightningRingExplosion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMODUnity;
using MMBiomeGeneration;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class LightningRingExplosion : BaseMonoBehaviour, ISpellOwning
{
  public ColliderEvents DamageCollider;
  public CircleCollider2D circleCollider;
  public MeshRenderer ringParticleInner;
  public MeshRenderer ringParticleOutter;
  public Transform[] FXChildren;
  public static GameObject explosionPrefab;
  public bool DoKnockback;
  public bool includeOwner;
  public float ringThickness = 2f;
  [EventRef]
  public string LightningStrikeSFX = "event:/dlc/dungeon05/enemy/attacks_shared/lightning_ring_explosion";
  public Dictionary<Collider2D, float> collisionDictionary;
  public Health.Team _team = Health.Team.PlayerTeam;
  public Health Origin;
  public float Damage = -1f;
  public float Team2Damage = -1f;
  [HideInInspector]
  public float progress;
  [HideInInspector]
  public bool forceEnd;
  public bool Init;
  public float shakeMultiplier;
  public UnitObject[] unitsToIgnore;
  public Health EnemyHealth;
  public Health.AttackFlags attackFlags;
  public static List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public const string ASSET_KEY = "Assets/Prefabs/Enemies/Weapons/Explosion_RingLightning.prefab";
  public float startRadius = 1f;
  [HideInInspector]
  public float maxRadius = 6.5f;
  public float expansionSpeed = 3f;
  public float damageBufferDuration = 0.5f;
  public float fadeTime = 0.25f;

  public Health.Team team
  {
    get => this._team;
    set => this._team = value;
  }

  public void OnEnable()
  {
    CameraManager.instance.ShakeCameraForDuration(0.5f * this.shakeMultiplier, 0.6f * this.shakeMultiplier, 0.3f * this.shakeMultiplier, false);
    this.circleCollider.radius = this.startRadius - 0.3f;
    this.ringParticleInner.material.SetFloat("_RingRadius", this.startRadius - 0.2f);
    this.ringParticleInner.material.SetFloat("_TotalOpacity", 1f);
    this.ringParticleOutter.material.SetFloat("_RingRadius", this.startRadius);
    this.ringParticleOutter.material.SetFloat("_TotalOpacity", 1f);
    this.fadeTime = 0.25f;
    this.forceEnd = false;
    this.UpdateParticleSize(0.0f);
  }

  public void FixedUpdate()
  {
    if (!this.Init || PlayerRelic.TimeFrozen)
      return;
    if (!this.forceEnd)
      this.progress += this.expansionSpeed * Time.deltaTime;
    this.circleCollider.radius = (float) ((double) this.startRadius + (double) this.progress - 0.30000001192092896);
    this.UpdateParticleSize(this.progress);
    if ((double) this.progress + (double) this.startRadius < (double) this.maxRadius && !this.forceEnd)
      return;
    this.fadeTime -= Time.deltaTime * (6.5f / this.maxRadius);
    if (this.forceEnd)
      this.fadeTime /= 1.5f;
    if ((double) this.fadeTime <= 0.0)
    {
      this.Init = false;
      ObjectPool.Recycle(this.gameObject);
    }
    else
    {
      this.ringParticleInner.material.SetFloat("_TotalOpacity", this.fadeTime);
      this.ringParticleOutter.material.SetFloat("_TotalOpacity", this.fadeTime);
    }
  }

  public void UpdateParticleSize(float progress)
  {
    this.ringParticleInner.material.SetFloat("_RingRadius", (float) ((double) progress + (double) this.startRadius - 0.20000000298023224));
    this.ringParticleOutter.material.SetFloat("_RingRadius", progress + this.startRadius);
  }

  public static void CreateExplosionPool(int count)
  {
    if (!((UnityEngine.Object) LightningRingExplosion.explosionPrefab == (UnityEngine.Object) null))
      return;
    Addressables.LoadAssetAsync<GameObject>((object) "Assets/Prefabs/Enemies/Weapons/Explosion_RingLightning.prefab").Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      LightningRingExplosion.loadedAddressableAssets.Add(obj);
      LightningRingExplosion.explosionPrefab = obj.Result;
      LightningRingExplosion.explosionPrefab.CreatePool(count);
    });
  }

  public static GameObject CreateExplosion(
    Vector3 position,
    Health.Team team,
    Health Origin,
    float expansionSpeed = 1f,
    float Damage = -1f,
    float Team2Damage = -1f,
    bool useKnockback = false,
    float shakeMultiplier = 1f,
    Health.AttackFlags attackFlags = (Health.AttackFlags) 0,
    bool includeOwner = false,
    float maxRadiusTarget = 6.5f,
    params UnitObject[] unitsToIgnore)
  {
    return LightningRingExplosion.SpawnExplosion(LightningRingExplosion.explosionPrefab, position, team, Origin, expansionSpeed, Damage, Team2Damage, useKnockback, shakeMultiplier, attackFlags, includeOwner, maxRadiusTarget, unitsToIgnore);
  }

  public static GameObject SpawnExplosion(
    GameObject explosionPrefab,
    Vector3 position,
    Health.Team team,
    Health Origin,
    float expansionSpeed = 1f,
    float Damage = -1f,
    float Team2Damage = -1f,
    bool useKnockback = false,
    float shakeMultiplier = 1f,
    Health.AttackFlags attackFlags = (Health.AttackFlags) 0,
    bool includeOwner = false,
    float maxRadiusTarget = 6.5f,
    params UnitObject[] unitsToIgnore)
  {
    GameObject gameObject = ObjectPool.Spawn(explosionPrefab, (UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null ? BiomeGenerator.Instance.CurrentRoom.generateRoom.transform : (Transform) null, position, Quaternion.identity);
    if (team == Health.Team.PlayerTeam)
      MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact, Origin.GetComponent<PlayerFarming>());
    if (team == Health.Team.PlayerTeam)
      MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact, Origin.GetComponent<PlayerFarming>());
    gameObject.transform.position = position;
    LightningRingExplosion component = gameObject.GetComponent<LightningRingExplosion>();
    AudioManager.Instance.PlayOneShot(component.LightningStrikeSFX, gameObject.transform.position);
    component.team = team;
    foreach (Transform fxChild in component.FXChildren)
      fxChild.localScale = Vector3.one * 0.25f;
    component.Origin = Origin;
    component.Damage = Damage;
    component.Team2Damage = Team2Damage;
    component.DoKnockback = useKnockback;
    component.DamageCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(component.DamageCollider_OnTriggerEnterEvent);
    component.DamageCollider.OnTriggerStayEvent += new ColliderEvents.TriggerEvent(component.DamageCollider_OnTriggerStayEvent);
    component.DamageCollider.SetActive(true);
    component.shakeMultiplier = shakeMultiplier;
    component.Init = true;
    component.attackFlags = attackFlags;
    component.includeOwner = includeOwner;
    component.unitsToIgnore = unitsToIgnore;
    component.expansionSpeed = expansionSpeed;
    component.maxRadius = maxRadiusTarget;
    component.collisionDictionary = new Dictionary<Collider2D, float>();
    return gameObject;
  }

  public static void Clear()
  {
    if (LightningRingExplosion.loadedAddressableAssets == null)
      return;
    LightningRingExplosion.explosionPrefab = (GameObject) null;
    foreach (AsyncOperationHandle<GameObject> addressableAsset in LightningRingExplosion.loadedAddressableAssets)
      Addressables.Release((AsyncOperationHandle) addressableAsset);
    LightningRingExplosion.loadedAddressableAssets.Clear();
  }

  public GameObject GetOwner()
  {
    return !((UnityEngine.Object) this.Origin != (UnityEngine.Object) null) ? (GameObject) null : this.Origin.gameObject;
  }

  public void SetOwner(GameObject owner) => this.Origin = owner.GetComponent<Health>();

  public void OnDisable()
  {
    this.Init = false;
    this.progress = 0.0f;
    this.gameObject.Recycle();
  }

  public void DamageCollider_OnTriggerEnterEvent(Collider2D collider)
  {
    if (!this.IsValidCollision(collider))
      return;
    this.DoDamageLogic(collider);
  }

  public void DamageCollider_OnTriggerStayEvent(Collider2D collider)
  {
    if (!this.IsValidCollision(collider))
      return;
    this.DoDamageLogic(collider);
  }

  public bool IsValidCollision(Collider2D collider)
  {
    Bounds bounds = this.circleCollider.bounds;
    Vector2 center1 = (Vector2) bounds.center;
    bounds = collider.bounds;
    Vector2 center2 = (Vector2) bounds.center;
    float num = Vector2.Distance(center1, center2) - this.circleCollider.radius;
    bool flag = this.IsCollisionBufferFinished(collider);
    return (((double) num < 0.0 ? 0 : ((double) num < (double) this.ringThickness ? 1 : 0)) & (flag ? 1 : 0)) != 0;
  }

  public void DoDamageLogic(Collider2D collider)
  {
    if (this.forceEnd)
      return;
    if ((double) this.Damage == -1.0)
      this.Damage = 1f;
    if ((double) this.Team2Damage == -1.0)
      this.Team2Damage = (float) (3 + DataManager.Instance.GetDungeonNumber(PlayerFarming.Location));
    this.EnemyHealth = collider.GetComponentInParent<Health>();
    UnitObject componentInParent = collider.GetComponentInParent<UnitObject>();
    if ((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null && this.EnemyHealth.team != this.team && ((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) this.Origin || (UnityEngine.Object) this.EnemyHealth == (UnityEngine.Object) this.Origin && this.includeOwner) && this.unitsToIgnore != null && !this.unitsToIgnore.Contains<UnitObject>(componentInParent))
    {
      this.EnemyHealth.DealDamage(this.EnemyHealth.team == Health.Team.Team2 ? this.Team2Damage : this.Damage, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f), AttackType: Health.AttackTypes.NoHitStop, AttackFlags: this.attackFlags);
      this.collisionDictionary[collider] = GameManager.GetInstance().CurrentTime;
    }
    if (!this.DoKnockback || this.unitsToIgnore == null || this.unitsToIgnore.Contains<UnitObject>(componentInParent))
      return;
    this.EnemyHealth.gameObject.GetComponent<UnitObject>().DoKnockBack(collider.gameObject, 1f, 2f);
  }

  public bool IsCollisionBufferFinished(Collider2D collider)
  {
    if (!this.collisionDictionary.ContainsKey(collider))
      return true;
    float collision = this.collisionDictionary[collider];
    return (double) GameManager.GetInstance().TimeSince(collision) > (double) this.damageBufferDuration;
  }
}
