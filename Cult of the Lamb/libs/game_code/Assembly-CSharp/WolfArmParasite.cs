// Decompiled with JetBrains decompiler
// Type: WolfArmParasite
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class WolfArmParasite : MonoBehaviour
{
  [SerializeField]
  public float anticipation;
  [SerializeField]
  public float timeBetweenProjectileLaunch = 10f;
  [SerializeField]
  public LightningRingMortar mortar;
  [SerializeField]
  public float bombMoveDuration;
  [SerializeField]
  public WolfArmPiece piece;
  [SerializeField]
  public GrenadeBullet grenadeBullet;
  [SerializeField]
  public Vector2 amountToShoot;
  [SerializeField]
  public Vector2 angle;
  [SerializeField]
  public float timeBetweenShooting;
  [SerializeField]
  public float gravity;
  [SerializeField]
  public float speed;
  [SerializeField]
  public float startingHeight;
  [SerializeField]
  public SimpleSpineFlash simpleSpineFlash;
  [SerializeField]
  public SkeletonAnimation subSpine;
  [SerializeField]
  public ProjectilePatternBeam projectileBeam;
  public WolfBossArm arm;
  public bool active;
  public bool attacking;
  public float projectileLaunchTimer;
  public Vector3 fromPosition;
  public string attackDetachedArmBombLaunchSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_detached_arm_bomb_launch";
  public string attackDetachedArmRainStartSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_detached_arm_rain_start";
  public string attackDetachedArmRainImpactSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_detached_arm_rain_projectile_land";

  public SkeletonAnimation SubSpine => this.subSpine;

  public void SetParasite(Vector3 fromPosition)
  {
    this.fromPosition = fromPosition;
    this.arm = this.GetComponentInParent<WolfBossArm>();
    this.GetComponent<BoneFollower>().enabled = false;
    this.transform.localScale = Vector3.one;
    this.piece.Spine.gameObject.SetActive(false);
    this.subSpine.gameObject.SetActive(true);
    this.active = true;
    this.attacking = true;
    GameManager.GetInstance().WaitForSeconds(3f, (System.Action) (() => this.attacking = false));
    GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() =>
    {
      this.subSpine.AnimationState.SetAnimation(0, "mouth_close", false);
      this.subSpine.AnimationState.AddAnimation(0, "mouth_closed_loop", true, 0.0f);
    }));
  }

  public void Update()
  {
    if (!this.active || this.attacking || (double) Time.time <= (double) this.projectileLaunchTimer)
      return;
    this.StartCoroutine(this.LaunchProjectileIE());
  }

  public IEnumerator LaunchProjectileIE()
  {
    WolfArmParasite wolfArmParasite = this;
    wolfArmParasite.projectileLaunchTimer = Time.time + wolfArmParasite.timeBetweenProjectileLaunch;
    wolfArmParasite.attacking = true;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * wolfArmParasite.subSpine.timeScale) < (double) wolfArmParasite.anticipation)
    {
      float num = time / wolfArmParasite.anticipation;
      foreach (WolfArmPiece piece in wolfArmParasite.arm.Pieces)
        piece.FlashWhite(num);
      wolfArmParasite.simpleSpineFlash.FlashWhite(num);
      yield return (object) null;
    }
    AudioManager.Instance.PlayOneShot(wolfArmParasite.attackDetachedArmBombLaunchSFX, wolfArmParasite.gameObject);
    wolfArmParasite.subSpine.AnimationState.SetAnimation(0, "mouth_open", false);
    wolfArmParasite.subSpine.AnimationState.AddAnimation(0, "mouth_open_loop", true, 0.0f);
    foreach (WolfArmPiece piece in wolfArmParasite.arm.Pieces)
      piece.FlashWhite(0.0f);
    wolfArmParasite.simpleSpineFlash.FlashWhite(false);
    Vector3 position = new Vector3(0.0f, -5f, 0.0f) + (Vector3) UnityEngine.Random.insideUnitCircle * 7.5f;
    UnityEngine.Object.Instantiate<LightningRingMortar>(wolfArmParasite.mortar, position, Quaternion.identity).Play(wolfArmParasite.transform.position, wolfArmParasite.bombMoveDuration, Health.Team.Team2, PlayDefaultSFX: false);
    yield return (object) new WaitForSeconds(1f);
    wolfArmParasite.subSpine.AnimationState.SetAnimation(0, "mouth_close", false);
    wolfArmParasite.subSpine.AnimationState.AddAnimation(0, "mouth_closed_loop", true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    wolfArmParasite.attacking = false;
  }

  public void Shoot()
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.ShootIE());
  }

  public IEnumerator ShootIE()
  {
    WolfArmParasite wolfArmParasite = this;
    if (wolfArmParasite.attacking)
    {
      wolfArmParasite.transform.DOKill();
      wolfArmParasite.transform.DOMove(wolfArmParasite.fromPosition, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
      wolfArmParasite.subSpine.AnimationState.SetAnimation(0, "mouth_open", false);
      wolfArmParasite.subSpine.AnimationState.AddAnimation(0, "mouth_open_loop", true, 0.0f);
      yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, wolfArmParasite.subSpine);
    }
    wolfArmParasite.subSpine.AnimationState.SetAnimation(0, "mouth_close", false);
    wolfArmParasite.subSpine.AnimationState.AddAnimation(0, "mouth_closed_loop", true, 0.0f);
    wolfArmParasite.transform.DOMove(new Vector3(wolfArmParasite.fromPosition.x * 0.8f, wolfArmParasite.fromPosition.y + 5f, wolfArmParasite.fromPosition.z), wolfArmParasite.anticipation).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    wolfArmParasite.attacking = true;
    AudioManager.Instance.PlayOneShot(wolfArmParasite.attackDetachedArmRainStartSFX);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * wolfArmParasite.subSpine.timeScale) < (double) wolfArmParasite.anticipation)
    {
      float num = time / wolfArmParasite.anticipation;
      foreach (WolfArmPiece piece in wolfArmParasite.arm.Pieces)
        piece.FlashWhite(num);
      wolfArmParasite.simpleSpineFlash.FlashWhite(num);
      yield return (object) null;
    }
    foreach (WolfArmPiece piece in wolfArmParasite.arm.Pieces)
      piece.FlashWhite(0.0f);
    wolfArmParasite.simpleSpineFlash.FlashWhite(false);
    wolfArmParasite.subSpine.AnimationState.SetAnimation(0, "mouth_open", false);
    wolfArmParasite.subSpine.AnimationState.AddAnimation(0, "mouth_open_loop", true, 0.0f);
    int amount = (int) UnityEngine.Random.Range(wolfArmParasite.amountToShoot.x, wolfArmParasite.amountToShoot.y);
    int i = -1;
    while (++i < amount)
    {
      GrenadeBullet component = ObjectPool.Spawn<GrenadeBullet>(wolfArmParasite.grenadeBullet, wolfArmParasite.transform.position, Quaternion.identity).GetComponent<GrenadeBullet>();
      component.SetOwner(wolfArmParasite.gameObject);
      component.Play(wolfArmParasite.startingHeight, UnityEngine.Random.Range(wolfArmParasite.angle.x, wolfArmParasite.angle.y), UnityEngine.Random.Range(wolfArmParasite.speed - 2f, wolfArmParasite.speed + 2f), wolfArmParasite.gravity, customImpactSFX: wolfArmParasite.attackDetachedArmRainImpactSFX);
      yield return (object) new WaitForSeconds(wolfArmParasite.timeBetweenShooting);
    }
    wolfArmParasite.transform.DOMove(wolfArmParasite.fromPosition, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    yield return (object) new WaitForSeconds(2f);
    wolfArmParasite.attacking = false;
  }

  [CompilerGenerated]
  public void \u003CSetParasite\u003Eb__25_0() => this.attacking = false;

  [CompilerGenerated]
  public void \u003CSetParasite\u003Eb__25_1()
  {
    this.subSpine.AnimationState.SetAnimation(0, "mouth_close", false);
    this.subSpine.AnimationState.AddAnimation(0, "mouth_closed_loop", true, 0.0f);
  }
}
