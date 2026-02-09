// Decompiled with JetBrains decompiler
// Type: EnemyShootGrenadeBullet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyShootGrenadeBullet : BaseMonoBehaviour
{
  public GameObject Prefab;
  public SimpleSpineFlash SimpleSpineFlash;
  public bool SpineAnimations;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string ShootAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AfterShootAnimation;
  public bool ShootFromGameObject;
  public Transform ShootFromGameObjectTransform;
  public float WaitBetweenShooting = 5f;
  public Vector2 DelayBetweenShots = new Vector2(0.1f, 0.3f);
  public float NumberOfShotsToFire = 5f;
  public Vector2 DistanceRange = new Vector2(2f, 3f);
  public float DistanceFromPlayerToFire = 5f;
  public float GravSpeed = -15f;
  public float AnticipationTime;
  public EnemyShootGrenadeBullet.ShootDirectionMode ShootDirection;
  public float Arc = 360f;
  public Vector2 RandomArcOffset = new Vector2(0.0f, 0.0f);
  public StateMachine state;
  public GameObject g;
  public GrenadeBullet GrenadeBullet;
  public bool Shooting;
  public float CacheShootDirectionCache;

  public float Direction
  {
    get
    {
      switch (this.ShootDirection)
      {
        case EnemyShootGrenadeBullet.ShootDirectionMode.Player:
          return !((Object) PlayerFarming.Instance == (Object) null) ? Utils.GetAngle(this.transform.position, PlayerFarming.Instance.transform.position) : 0.0f;
        case EnemyShootGrenadeBullet.ShootDirectionMode.Looking:
          return this.state.LookAngle;
        case EnemyShootGrenadeBullet.ShootDirectionMode.Facing:
          return this.state.facingAngle;
        default:
          return 0.0f;
      }
    }
  }

  public void OnEnable()
  {
    this.state = this.GetComponent<StateMachine>();
    this.Shooting = false;
  }

  public void Update()
  {
    if (Time.frameCount % 10 != 0 || this.Shooting)
      return;
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
      case StateMachine.State.Moving:
        if (!((Object) PlayerFarming.Instance != (Object) null) || (double) Vector3.Distance(this.transform.position, PlayerFarming.Instance.transform.position) >= (double) this.DistanceFromPlayerToFire)
          break;
        this.StartCoroutine((IEnumerator) this.Shoot());
        break;
    }
  }

  public IEnumerator Shoot()
  {
    EnemyShootGrenadeBullet shootGrenadeBullet = this;
    shootGrenadeBullet.Shooting = true;
    if ((double) shootGrenadeBullet.AnticipationTime > 0.0)
    {
      float Progress = 0.0f;
      while ((double) (Progress += Time.deltaTime) < (double) shootGrenadeBullet.AnticipationTime)
      {
        shootGrenadeBullet.SimpleSpineFlash.FlashWhite(Progress / shootGrenadeBullet.AnticipationTime);
        yield return (object) null;
      }
      shootGrenadeBullet.SimpleSpineFlash.FlashWhite(false);
    }
    if (shootGrenadeBullet.SpineAnimations)
    {
      shootGrenadeBullet.Spine.AnimationState.SetAnimation(0, shootGrenadeBullet.ShootAnimation, false);
      shootGrenadeBullet.Spine.AnimationState.AddAnimation(0, shootGrenadeBullet.AfterShootAnimation, true, 0.0f);
    }
    shootGrenadeBullet.CacheShootDirectionCache = shootGrenadeBullet.Direction;
    int i = -1;
    while ((double) ++i < (double) shootGrenadeBullet.NumberOfShotsToFire)
    {
      AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile", shootGrenadeBullet.transform.position);
      Vector3 position = (shootGrenadeBullet.ShootFromGameObject ? shootGrenadeBullet.ShootFromGameObjectTransform.position : shootGrenadeBullet.transform.position) with
      {
        z = 0.0f
      };
      shootGrenadeBullet.GrenadeBullet = ObjectPool.Spawn(shootGrenadeBullet.Prefab, position, Quaternion.identity).GetComponent<GrenadeBullet>();
      shootGrenadeBullet.GrenadeBullet.SetOwner(shootGrenadeBullet.gameObject);
      shootGrenadeBullet.GrenadeBullet.Play(-1f, (float) ((double) shootGrenadeBullet.CacheShootDirectionCache - (double) shootGrenadeBullet.Arc / 2.0 + (double) shootGrenadeBullet.Arc / (double) shootGrenadeBullet.NumberOfShotsToFire * (double) i) + Random.Range(shootGrenadeBullet.RandomArcOffset.x, shootGrenadeBullet.RandomArcOffset.y), Random.Range(shootGrenadeBullet.DistanceRange.x, shootGrenadeBullet.DistanceRange.y), shootGrenadeBullet.GravSpeed);
      yield return (object) new WaitForSeconds(Random.Range(shootGrenadeBullet.DelayBetweenShots.x, shootGrenadeBullet.DelayBetweenShots.y));
    }
    yield return (object) new WaitForSeconds(shootGrenadeBullet.WaitBetweenShooting);
    shootGrenadeBullet.Shooting = false;
  }

  public enum ShootDirectionMode
  {
    Player,
    Looking,
    Facing,
  }
}
