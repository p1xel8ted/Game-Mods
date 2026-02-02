// Decompiled with JetBrains decompiler
// Type: Explosion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class Explosion : BaseMonoBehaviour, ISpellOwning
{
  public ColliderEvents DamageCollider;
  public Health.Team _team = Health.Team.PlayerTeam;
  public Health Origin;
  public float Damage = -1f;
  public float Team2Damage = -1f;
  public Transform[] FXChildren;
  public bool Init;
  public bool DoKnockback;
  public static GameObject explosionPrefab;
  public static GameObject noDistortExplosionPrefab;
  public bool includeOwner;
  public bool playSFX = true;
  public float shakeMultiplier;
  public UnitObject[] unitsToIgnore;
  public Health.AttackFlags attackFlags;
  public static List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public Health EnemyHealth;

  public Health.Team team
  {
    get => this._team;
    set => this._team = value;
  }

  public static void CreateExplosionPool(int count)
  {
    if ((UnityEngine.Object) Explosion.explosionPrefab == (UnityEngine.Object) null)
      Addressables.LoadAssetAsync<GameObject>((object) "Assets/Prefabs/Enemies/Weapons/Explosion.prefab").Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        Explosion.loadedAddressableAssets.Add(obj);
        Explosion.explosionPrefab = obj.Result;
        Explosion.explosionPrefab.CreatePool(count);
      });
    if (!((UnityEngine.Object) Explosion.noDistortExplosionPrefab == (UnityEngine.Object) null))
      return;
    Addressables.LoadAssetAsync<GameObject>((object) "Assets/Prefabs/Enemies/Weapons/Explosion_noDistort.prefab").Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      Explosion.loadedAddressableAssets.Add(obj);
      Explosion.noDistortExplosionPrefab = obj.Result;
      Explosion.noDistortExplosionPrefab.CreatePool(count);
    });
  }

  public static GameObject CreateExplosion(
    Vector3 position,
    Health.Team team,
    Health Origin,
    float Size,
    float Damage = -1f,
    float Team2Damage = -1f,
    bool useKnockback = false,
    float shakeMultiplier = 1f,
    Health.AttackFlags attackFlags = (Health.AttackFlags) 0,
    bool noDistort = false,
    bool includeOwner = false,
    bool playSFX = true,
    params UnitObject[] unitsToIgnore)
  {
    return noDistort ? Explosion.SpawnExplosion(Explosion.noDistortExplosionPrefab, position, team, Origin, Size, Damage, Team2Damage, useKnockback, shakeMultiplier, attackFlags, includeOwner, playSFX, unitsToIgnore) : Explosion.SpawnExplosion(Explosion.explosionPrefab, position, team, Origin, Size, Damage, Team2Damage, useKnockback, shakeMultiplier, attackFlags, includeOwner, playSFX, unitsToIgnore);
  }

  public static GameObject SpawnExplosion(
    GameObject explosionPrefab,
    Vector3 position,
    Health.Team team,
    Health Origin,
    float Size,
    float Damage = -1f,
    float Team2Damage = -1f,
    bool useKnockback = false,
    float shakeMultiplier = 1f,
    Health.AttackFlags attackFlags = (Health.AttackFlags) 0,
    bool includeOwner = false,
    bool playSFX = true,
    params UnitObject[] unitsToIgnore)
  {
    GameObject gameObject = ObjectPool.Spawn(explosionPrefab, (Transform) null, position, Quaternion.identity);
    if (playSFX)
      AudioManager.Instance.PlayOneShot("event:/explosion/explosion", gameObject.transform.position);
    if ((UnityEngine.Object) Origin != (UnityEngine.Object) null && team == Health.Team.PlayerTeam)
    {
      PlayerFarming component = Origin.GetComponent<PlayerFarming>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact, component);
        MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact, component);
      }
    }
    gameObject.transform.position = position;
    Explosion component1 = gameObject.GetComponent<Explosion>();
    component1.team = team;
    foreach (Transform fxChild in component1.FXChildren)
      fxChild.localScale = Vector3.one * (Size / 2f);
    component1.Origin = Origin;
    component1.Damage = Damage;
    component1.Team2Damage = Team2Damage;
    component1.DamageCollider.GetComponent<CircleCollider2D>().radius = Size * 0.5f;
    component1.DoKnockback = useKnockback;
    component1.DamageCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(component1.DamageCollider_OnTriggerEnterEvent);
    component1.DamageCollider.SetActive(false);
    component1.shakeMultiplier = shakeMultiplier;
    component1.Init = true;
    component1.attackFlags = attackFlags;
    component1.includeOwner = includeOwner;
    component1.playSFX = playSFX;
    component1.unitsToIgnore = unitsToIgnore;
    if ((UnityEngine.Object) Origin != (UnityEngine.Object) null)
      component1.SetOwner(Origin.gameObject);
    return gameObject;
  }

  public static void CreateExplosionCustomFX(
    Vector3 position,
    Health.Team team,
    Health Origin,
    float Size,
    GameObject ExplosionFXPrefab,
    float Damage = -1f,
    float Team2Damage = -1f,
    bool playSFX = true)
  {
    GameObject gameObject = ObjectPool.Spawn(ExplosionFXPrefab);
    if (playSFX)
      AudioManager.Instance.PlayOneShot("event:/explosion/explosion", gameObject.transform.position);
    if (team == Health.Team.PlayerTeam)
      MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact, Origin.GetComponent<PlayerFarming>());
    gameObject.transform.position = position;
    Explosion component = gameObject.GetComponent<Explosion>();
    component.team = team;
    foreach (Transform fxChild in component.FXChildren)
      fxChild.localScale = Vector3.one * (Size / 4f);
    component.Origin = Origin;
    component.Damage = Damage;
    component.Team2Damage = Team2Damage;
    component.playSFX = playSFX;
    component.DamageCollider.GetComponent<CircleCollider2D>().radius = Size * 0.5f;
    component.DamageCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(component.DamageCollider_OnTriggerEnterEvent);
    component.DamageCollider.SetActive(false);
    component.Init = true;
  }

  public void OnEnable()
  {
    CameraManager.instance.ShakeCameraForDuration(0.5f * this.shakeMultiplier, 0.6f * this.shakeMultiplier, 0.3f * this.shakeMultiplier, false);
    this.StartCoroutine((IEnumerator) this.DelayDestroy());
  }

  public IEnumerator DelayDestroy()
  {
    Explosion explosion = this;
    while (!explosion.Init)
      yield return (object) null;
    if ((UnityEngine.Object) explosion.DamageCollider != (UnityEngine.Object) null)
    {
      explosion.DamageCollider.SetActive(true);
      yield return (object) new WaitForSeconds(0.2f);
      explosion.DamageCollider.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(explosion.DamageCollider_OnTriggerEnterEvent);
      explosion.DamageCollider.SetActive(false);
      yield return (object) new WaitForSeconds(2f);
    }
    ObjectPool.Recycle(explosion.gameObject);
    explosion.Init = false;
  }

  public void DamageCollider_OnTriggerEnterEvent(Collider2D collider)
  {
    if ((double) this.Damage == -1.0)
      this.Damage = 1f;
    if ((double) this.Team2Damage == -1.0)
      this.Team2Damage = (float) (3 + DataManager.Instance.GetDungeonNumber(PlayerFarming.Location));
    this.EnemyHealth = collider.GetComponentInParent<Health>();
    UnitObject componentInParent1 = collider.GetComponentInParent<UnitObject>();
    float num = 1f;
    VulnerableToExplosions componentInParent2 = collider.GetComponentInParent<VulnerableToExplosions>();
    if ((UnityEngine.Object) componentInParent2 != (UnityEngine.Object) null)
      num = componentInParent2.Multiplier;
    PlayerFarming component = collider.GetComponent<PlayerFarming>();
    if (this.attackFlags.HasFlag((Enum) Health.AttackFlags.Trap) && (bool) (UnityEngine.Object) component && TrinketManager.HasTrinket(TarotCards.Card.ImmuneToTraps, component))
      return;
    if ((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null && this.EnemyHealth.team != this.team && ((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) this.Origin || (UnityEngine.Object) this.EnemyHealth == (UnityEngine.Object) this.Origin && this.includeOwner) && this.unitsToIgnore != null && !this.unitsToIgnore.Contains<UnitObject>(componentInParent1))
      this.EnemyHealth.DealDamage(this.EnemyHealth.team == Health.Team.Team2 ? this.Team2Damage * num : this.Damage * num, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f), AttackType: Health.AttackTypes.Heavy, AttackFlags: this.attackFlags);
    if (!this.DoKnockback || this.unitsToIgnore == null || this.unitsToIgnore.Contains<UnitObject>(componentInParent1))
      return;
    this.EnemyHealth.gameObject.GetComponent<UnitObject>().DoKnockBack(collider.gameObject, 1f, 2f);
  }

  public static void Clear()
  {
    if (Explosion.loadedAddressableAssets == null)
      return;
    Explosion.explosionPrefab = (GameObject) null;
    Explosion.noDistortExplosionPrefab = (GameObject) null;
    foreach (AsyncOperationHandle<GameObject> addressableAsset in Explosion.loadedAddressableAssets)
      Addressables.Release((AsyncOperationHandle) addressableAsset);
    Explosion.loadedAddressableAssets.Clear();
  }

  public GameObject GetOwner()
  {
    return !((UnityEngine.Object) this.Origin != (UnityEngine.Object) null) ? (GameObject) null : this.Origin.gameObject;
  }

  public Health.Team GetTeam() => this.team;

  public Health GetOrigin() => this.Origin;

  public void SetOwner(GameObject owner) => this.Origin = owner?.GetComponent<Health>();
}
