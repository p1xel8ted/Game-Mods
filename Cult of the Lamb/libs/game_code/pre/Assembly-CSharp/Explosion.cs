// Decompiled with JetBrains decompiler
// Type: Explosion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class Explosion : BaseMonoBehaviour
{
  public ColliderEvents DamageCollider;
  private Health.Team _team = Health.Team.PlayerTeam;
  private Health Origin;
  private float Damage = -1f;
  private float Team2Damage = -1f;
  public Transform[] FXChildren;
  private bool Init;
  public bool DoKnockback;
  public static GameObject explosionPrefab;
  private Health EnemyHealth;

  private Health.Team team
  {
    get => this._team;
    set => this._team = value;
  }

  public static void CreateExplosion(
    Vector3 position,
    Health.Team team,
    Health Origin,
    float Size,
    float Damage = -1f,
    float Team2Damage = -1f,
    bool useKnockback = false)
  {
    if ((UnityEngine.Object) Explosion.explosionPrefab == (UnityEngine.Object) null)
      Addressables.LoadAssetAsync<GameObject>((object) "Assets/Prefabs/Enemies/Weapons/Explosion.prefab").Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        Explosion.explosionPrefab = obj.Result;
        Explosion.SpawnExplosion(Explosion.explosionPrefab, position, team, Origin, Size, Damage, Team2Damage, useKnockback);
      });
    else
      Explosion.SpawnExplosion(Explosion.explosionPrefab, position, team, Origin, Size, Damage, Team2Damage, useKnockback);
  }

  private static void SpawnExplosion(
    GameObject explosionPrefab,
    Vector3 position,
    Health.Team team,
    Health Origin,
    float Size,
    float Damage = -1f,
    float Team2Damage = -1f,
    bool useKnockback = false)
  {
    GameObject gameObject = ObjectPool.Spawn(explosionPrefab, (Transform) null, position, Quaternion.identity);
    AudioManager.Instance.PlayOneShot("event:/explosion/explosion", gameObject.transform.position);
    if (team == Health.Team.PlayerTeam)
      MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    if (team == Health.Team.PlayerTeam)
      MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    gameObject.transform.position = position;
    Explosion component = gameObject.GetComponent<Explosion>();
    component.team = team;
    foreach (Transform fxChild in component.FXChildren)
      fxChild.localScale = Vector3.one * (Size / 4f);
    component.Origin = Origin;
    component.Damage = Damage;
    component.Team2Damage = Team2Damage;
    component.DamageCollider.GetComponent<CircleCollider2D>().radius = Size * 0.5f;
    component.DoKnockback = useKnockback;
    component.DamageCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(component.DamageCollider_OnTriggerEnterEvent);
    component.DamageCollider.SetActive(false);
    component.Init = true;
  }

  public static void CreateExplosionCustomFX(
    Vector3 position,
    Health.Team team,
    Health Origin,
    float Size,
    GameObject ExplosionFXPrefab,
    float Damage = -1f,
    float Team2Damage = -1f)
  {
    GameObject gameObject = ObjectPool.Spawn(ExplosionFXPrefab);
    AudioManager.Instance.PlayOneShot("event:/explosion/explosion", gameObject.transform.position);
    if (team == Health.Team.PlayerTeam)
      MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact);
    gameObject.transform.position = position;
    Explosion component = gameObject.GetComponent<Explosion>();
    component.team = team;
    foreach (Transform fxChild in component.FXChildren)
      fxChild.localScale = Vector3.one * (Size / 4f);
    component.Origin = Origin;
    component.Damage = Damage;
    component.Team2Damage = Team2Damage;
    component.DamageCollider.GetComponent<CircleCollider2D>().radius = Size * 0.5f;
    component.DamageCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(component.DamageCollider_OnTriggerEnterEvent);
    component.DamageCollider.SetActive(false);
    component.Init = true;
  }

  private void OnEnable()
  {
    CameraManager.instance.ShakeCameraForDuration(0.5f, 0.6f, 0.3f, false);
    this.StartCoroutine((IEnumerator) this.DelayDestroy());
  }

  private IEnumerator DelayDestroy()
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
    explosion.Init = false;
    ObjectPool.Recycle(explosion.gameObject);
  }

  private void DamageCollider_OnTriggerEnterEvent(Collider2D collider)
  {
    if ((double) this.Damage == -1.0)
      this.Damage = 1f;
    if ((double) this.Team2Damage == -1.0)
      this.Team2Damage = (float) (3 + DataManager.Instance.GetDungeonNumber(PlayerFarming.Location));
    this.EnemyHealth = collider.GetComponentInParent<Health>();
    if ((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null && this.EnemyHealth.team != this.team && (UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) this.Origin)
      this.EnemyHealth.DealDamage(this.EnemyHealth.team == Health.Team.Team2 ? this.Team2Damage : this.Damage, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f), AttackType: Health.AttackTypes.Heavy);
    if (!this.DoKnockback)
      return;
    this.EnemyHealth.gameObject.GetComponent<UnitObject>().DoKnockBack(collider.gameObject, 1f, 2f);
  }
}
