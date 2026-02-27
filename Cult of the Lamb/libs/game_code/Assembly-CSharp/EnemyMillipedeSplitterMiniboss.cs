// Decompiled with JetBrains decompiler
// Type: EnemyMillipedeSplitterMiniboss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyMillipedeSplitterMiniboss : EnemyMillipedeSpiker
{
  [SerializeField]
  public float speedIncrementPerHit;
  [SerializeField]
  public GameObject projectile;
  [SerializeField]
  public float shootAnticipation;
  [SerializeField]
  public Vector2 amountToShoot;
  [SerializeField]
  public Vector2 delayBetweenShots;
  [SerializeField]
  public float shootingCooldown;
  [SerializeField]
  public float gravSpeed;
  [SerializeField]
  public float shootOffset;
  [SerializeField]
  public GameObject shootBone;
  [SerializeField]
  public float height;
  [SerializeField]
  public float duration;
  [SerializeField]
  public float partRadius;
  [SerializeField]
  public AnimationCurve heightCurve;
  [SerializeField]
  public LayerMask avoidMask;
  [SerializeField]
  public AssetReferenceGameObject[] enemies;
  [Space]
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootAnimation;
  public List<MillipedeBodyPart> parts;
  public float startingHP;
  public int totalBodyParts;
  public float shootTimestamp;
  public List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();

  public override void Awake()
  {
    base.Awake();
    this.parts = ((IEnumerable<MillipedeBodyPart>) this.GetComponentsInChildren<MillipedeBodyPart>()).ToList<MillipedeBodyPart>();
    this.totalBodyParts = this.parts.Count - 1;
    this.startingHP = this.health.totalHP;
  }

  public IEnumerator Start()
  {
    this.\u003C\u003En__0();
    yield return (object) null;
    this.Preload();
  }

  public void Preload()
  {
    for (int index = 0; index < this.enemies.Length; ++index)
    {
      AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) this.enemies[index]);
      asyncOperationHandle.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        this.loadedAddressableAssets.Add(obj);
        obj.Result.CreatePool(this.totalBodyParts * 3, true);
      });
      asyncOperationHandle.WaitForCompletion();
    }
  }

  public override void Update()
  {
    base.Update();
    float? currentTime = GameManager.GetInstance()?.CurrentTime;
    float num = this.shootTimestamp / this.Spine.timeScale;
    if (!((double) currentTime.GetValueOrDefault() > (double) num & currentTime.HasValue) || this.attacking)
      return;
    this.StartCoroutine(this.ShootProjectiles());
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    int index1 = this.totalBodyParts - Mathf.RoundToInt((this.startingHP - this.health.HP) / (this.startingHP / (float) this.totalBodyParts));
    this.maxSpeed += (float) (this.parts.Count - index1) * this.speedIncrementPerHit;
    this.turnDamper -= (float) (this.parts.Count - index1) * 0.05f;
    if (this.parts.Count <= index1 || this.parts.Count <= 1)
      return;
    for (int index2 = index1; index2 < this.parts.Count; ++index2)
      this.DropBodyPart(this.parts[index2]);
    this.parts.RemoveRange(index1, this.parts.Count - index1);
  }

  public void DropBodyPart(MillipedeBodyPart bodyPart)
  {
    bodyPart.GetComponent<FollowAsTail>().enabled = false;
    bodyPart.GetComponent<Health>().enabled = false;
    this.flashes.RemoveAt(this.flashes.Count - 1);
    bodyPart.DroppedPart();
    this.spines.Remove(bodyPart.GetComponent<SkeletonAnimation>());
    this.StartCoroutine(this.ThrowBodyPart(bodyPart));
  }

  public IEnumerator ThrowBodyPart(MillipedeBodyPart bodyPart)
  {
    EnemyMillipedeSplitterMiniboss splitterMiniboss = this;
    Vector3 fromPosition = bodyPart.transform.position;
    Vector3 targetPosition = splitterMiniboss.GetRandomPosition() with
    {
      z = 0.0f
    };
    float t = 0.0f;
    while ((double) t < (double) splitterMiniboss.duration)
    {
      float num = t / splitterMiniboss.duration;
      Vector3 vector3 = Vector3.Lerp(fromPosition, targetPosition, num) with
      {
        z = (float) ((double) splitterMiniboss.heightCurve.Evaluate(num) * (double) splitterMiniboss.height * -1.0)
      };
      bodyPart.transform.position = vector3;
      t += Time.deltaTime * splitterMiniboss.Spine.timeScale;
      yield return (object) null;
    }
    bodyPart.SpawnEnemy(splitterMiniboss.loadedAddressableAssets, splitterMiniboss.transform.parent);
  }

  public Vector3 GetRandomPosition() => (Vector3) UnityEngine.Random.insideUnitCircle * 4f;

  public IEnumerator ShootProjectiles()
  {
    EnemyMillipedeSplitterMiniboss splitterMiniboss = this;
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/spider/warning", splitterMiniboss.gameObject);
    splitterMiniboss.DisableForces = true;
    splitterMiniboss.attacking = true;
    splitterMiniboss.SetAnimation(splitterMiniboss.shootAnticipationAnimation, true);
    yield return (object) new WaitForEndOfFrame();
    splitterMiniboss.moveVX = 0.0f;
    splitterMiniboss.moveVY = 0.0f;
    float t = 0.0f;
    while ((double) t < (double) splitterMiniboss.shootAnticipation)
    {
      float amt = t / splitterMiniboss.shootAnticipation;
      foreach (SimpleSpineFlash flash in splitterMiniboss.flashes)
        flash.FlashWhite(amt);
      t += Time.deltaTime * splitterMiniboss.Spine.timeScale;
      yield return (object) null;
    }
    foreach (SimpleSpineFlash flash in splitterMiniboss.flashes)
      flash.FlashWhite(false);
    splitterMiniboss.SetAnimation(splitterMiniboss.shootAnimation);
    splitterMiniboss.AddAnimation(splitterMiniboss.idleAnimation, true);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.2f, 0.4f);
    int i = -1;
    int total = (int) UnityEngine.Random.Range(splitterMiniboss.amountToShoot.x, splitterMiniboss.amountToShoot.y + 1f);
    while (++i < total)
    {
      float Angle = UnityEngine.Random.Range(0.0f, 360f);
      float Speed = UnityEngine.Random.Range(2f, 3f);
      if ((UnityEngine.Object) splitterMiniboss.GetClosestTarget() != (UnityEngine.Object) null)
      {
        Speed = Vector3.Distance(splitterMiniboss.transform.position, splitterMiniboss.GetClosestTarget().transform.position) / 1.5f + UnityEngine.Random.Range(-2f, 2f);
        Angle = Utils.GetAngle(splitterMiniboss.transform.position, splitterMiniboss.GetClosestTarget().transform.position) + UnityEngine.Random.Range(-20f, 20f);
      }
      AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile", AudioManager.Instance.Listener);
      Vector3 position = splitterMiniboss.shootBone.transform.position;
      GrenadeBullet component = ObjectPool.Spawn(splitterMiniboss.projectile, position, Quaternion.identity).GetComponent<GrenadeBullet>();
      component.SetOwner(splitterMiniboss.gameObject);
      component.Play(splitterMiniboss.shootOffset, Angle, Speed, splitterMiniboss.gravSpeed);
      float dur = UnityEngine.Random.Range(splitterMiniboss.delayBetweenShots.x, splitterMiniboss.delayBetweenShots.y);
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * splitterMiniboss.Spine.timeScale) < (double) dur)
        yield return (object) null;
    }
    splitterMiniboss.shootTimestamp = GameManager.GetInstance().CurrentTime + splitterMiniboss.shootingCooldown;
    splitterMiniboss.attacking = false;
    splitterMiniboss.DisableForces = false;
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (this.loadedAddressableAssets == null)
      return;
    foreach (AsyncOperationHandle<GameObject> addressableAsset in this.loadedAddressableAssets)
      Addressables.Release((AsyncOperationHandle) addressableAsset);
    this.loadedAddressableAssets.Clear();
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public void \u003C\u003En__0() => base.Start();

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__24_0(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedAddressableAssets.Add(obj);
    obj.Result.CreatePool(this.totalBodyParts * 3, true);
  }
}
