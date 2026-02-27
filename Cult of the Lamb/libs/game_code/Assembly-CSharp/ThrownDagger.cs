// Decompiled with JetBrains decompiler
// Type: ThrownDagger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMBiomeGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class ThrownDagger : MonoBehaviour, IHeavyAttackWeapon, ISpellOwning
{
  public GameObject AxeContainer;
  public SpriteRenderer SpriteRenderer;
  public SpriteRenderer SpriteRendererHeavy;
  public SpriteRenderer ShadowSpriteRenderer;
  public Vector3 StartingPosition;
  public float ScaleTime = 0.5f;
  public float WaitScaleTime = 0.5f;
  public float MoveTime = 0.5f;
  public float LifeTimeDuration = 0.5f;
  public float ShrinkTime = 0.5f;
  public Vector2 ShakeIntensity = new Vector2(1f, 1.2f);
  public Ease ThrowEase = Ease.InBack;
  public GameObject weaponOwner;
  public float Damage = 1f;
  public Collider2D DamageCollider;
  public List<Collider2D> collider2DList;
  public Health CollisionHealth;

  public static void SpawnThrownDagger(
    Vector3 position,
    float Damage,
    float Delay,
    Sprite DaggerImage,
    GameObject owner)
  {
    GameManager.GetInstance().StartCoroutine(ThrownDagger.DelayCallback(Delay, (System.Action) (() => ThrownDagger.SpawnThrownDagger(position, Damage, DaggerImage, owner))));
  }

  public static IEnumerator DelayCallback(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public static void SpawnThrownDagger(
    Vector3 position,
    float Damage,
    Sprite DaggerImage,
    GameObject owner)
  {
    Addressables_wrapper.InstantiateAsync((object) "Assets/Prefabs/Thrown Dagger.prefab", position, Quaternion.identity, BiomeGenerator.Instance.CurrentRoom.generateRoom.transform, (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      ThrownDagger component = obj.Result.GetComponent<ThrownDagger>();
      component.Damage = Damage;
      component.SetOwner(owner);
      component.Throw(DaggerImage);
    }));
  }

  public void OnEnable()
  {
    this.SpriteRenderer.enabled = false;
    this.SpriteRendererHeavy.enabled = false;
    this.ShadowSpriteRenderer.enabled = false;
  }

  public void Throw(Sprite DaggerImage) => this.StartCoroutine(this.ThrowRoutine(DaggerImage));

  public IEnumerator ThrowRoutine(Sprite DaggerImage)
  {
    ThrownDagger thrownDagger = this;
    thrownDagger.AxeContainer.transform.localPosition = thrownDagger.StartingPosition;
    thrownDagger.AxeContainer.transform.localScale = Vector3.zero;
    thrownDagger.AxeContainer.transform.DOScale(Vector3.one, thrownDagger.ScaleTime).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    AudioManager.Instance.PlayOneShot("event:/enemy/chaser_boss/chaser_boss_egg_spawn", thrownDagger.gameObject);
    if ((UnityEngine.Object) DaggerImage != (UnityEngine.Object) null)
    {
      thrownDagger.SpriteRenderer.enabled = true;
      thrownDagger.SpriteRendererHeavy.enabled = false;
      thrownDagger.SpriteRenderer.sprite = DaggerImage;
    }
    else
    {
      thrownDagger.SpriteRenderer.enabled = false;
      thrownDagger.SpriteRendererHeavy.enabled = true;
    }
    float x = thrownDagger.ShadowSpriteRenderer.transform.localScale.x;
    thrownDagger.ShadowSpriteRenderer.transform.localScale = Vector3.zero;
    thrownDagger.ShadowSpriteRenderer.enabled = true;
    thrownDagger.ShadowSpriteRenderer.transform.DOScale(x, thrownDagger.ScaleTime).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    yield return (object) new WaitForSeconds(thrownDagger.WaitScaleTime);
    AudioManager.Instance.PlayOneShot("event:/weapon/melee_swing_fast", thrownDagger.gameObject);
    thrownDagger.AxeContainer.transform.DOLocalMove(Vector3.zero, thrownDagger.MoveTime).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(thrownDagger.ThrowEase);
    yield return (object) new WaitForSeconds(thrownDagger.MoveTime);
    CameraManager.instance.ShakeCameraForDuration(thrownDagger.ShakeIntensity.x, thrownDagger.ShakeIntensity.y, 0.3f);
    thrownDagger.SpriteRenderer.transform.localScale = new Vector3(1.5f, 0.5f);
    thrownDagger.SpriteRenderer.transform.DOScale(Vector3.one, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    AudioManager.Instance.PlayOneShot("event:/enemy/impact_normal", thrownDagger.gameObject);
    thrownDagger.collider2DList = new List<Collider2D>();
    thrownDagger.DamageCollider.GetContacts(thrownDagger.collider2DList);
    foreach (Collider2D collider2D in thrownDagger.collider2DList)
    {
      thrownDagger.CollisionHealth = collider2D.gameObject.GetComponent<Health>();
      if ((UnityEngine.Object) thrownDagger.CollisionHealth != (UnityEngine.Object) null && !thrownDagger.CollisionHealth.invincible && !thrownDagger.CollisionHealth.untouchable && (thrownDagger.CollisionHealth.team != Health.Team.PlayerTeam || thrownDagger.CollisionHealth.IsCharmedEnemy))
      {
        Health.AttackTypes AttackType = Health.AttackTypes.Projectile;
        if (thrownDagger.CollisionHealth.HasShield)
          AttackType = Health.AttackTypes.Heavy;
        thrownDagger.CollisionHealth.DealDamage(thrownDagger.Damage, thrownDagger.gameObject, thrownDagger.transform.position, AttackType: AttackType, dealDamageImmediately: true, AttackFlags: Health.AttackFlags.Penetration);
      }
    }
    yield return (object) new WaitForSeconds(thrownDagger.LifeTimeDuration);
    thrownDagger.AxeContainer.transform.DOScale(Vector3.zero, thrownDagger.ShrinkTime).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    thrownDagger.ShadowSpriteRenderer.transform.DOScale(Vector3.zero, thrownDagger.ShrinkTime).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    yield return (object) new WaitForSeconds(thrownDagger.ShrinkTime);
    UnityEngine.Object.Destroy((UnityEngine.Object) thrownDagger.gameObject);
  }

  public GameObject GetOwner() => this.weaponOwner;

  public void SetOwner(GameObject owner) => this.weaponOwner = owner;
}
