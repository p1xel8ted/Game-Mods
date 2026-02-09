// Decompiled with JetBrains decompiler
// Type: YngyaChunk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class YngyaChunk : MonoBehaviour
{
  [SerializeField]
  public Health health;
  [SerializeField]
  public YngyaChunk.ChunkType chunkType;
  [SerializeField]
  public GameObject child;
  [SerializeField]
  public GameObject prefab;
  [SerializeField]
  public int amount;
  [SerializeField]
  public float speed;
  [SerializeField]
  public Vector2 fervourToDrop;
  [SerializeField]
  public SimpleSpineFlash simpleSpineFlash;
  [SerializeField]
  public ParticleSystem deathParticles;
  public float timer;
  public float childZ;
  public float vz;
  public Vector2 squishScale;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string ExplodeAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string ExplodeStartAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SplatterAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string BounceAnimation;
  public ParticleSystem explodeParticles;
  public bool attacking;
  public List<Projectile> spawnedProjectiles = new List<Projectile>();
  public string landOnGroundSFX = "event:/dlc/dungeon06/enemy/yngya_minion/mv_land";
  public string attackCircleStartSFX = "event:/dlc/dungeon06/enemy/yngya_minion/attack_circle_start";
  public string attackCircleWindupSFX = "event:/dlc/dungeon06/enemy/yngya_minion/attack_circle_windup";
  public string attackCircleWindupVO = "event:/dlc/dungeon06/enemy/yngya_minion/attack_circle_windup_vo";
  public string attackCircleExplodeSFX = "event:/dlc/dungeon06/enemy/yngya_minion/attack_circle_explode";
  public string attackSpitStartSFX = "event:/dlc/dungeon06/enemy/yngya_minion/attack_spit_start";
  public string attackSpitStartVO = "event:/dlc/dungeon06/enemy/yngya_minion/attack_spit_start_vo";
  public string attackSpitProjectileLaunchSFX = "event:/dlc/dungeon06/enemy/yngya_minion/attack_spit_projectile_launch";
  public string attackSpitExplodeSFX = "event:/dlc/dungeon06/enemy/yngya_minion/attack_spit_explode";
  public string deathVO = "event:/dlc/dungeon06/enemy/yngya_minion/death";

  public void Configure(Vector3 targetPos)
  {
    this.transform.DOMove(targetPos, 1f);
    this.childZ = -2f;
    this.timer = 0.0f;
    this.health.OnDie += new Health.DieAction(this.Health_OnDie);
    this.health.OnHit += new Health.HitAction(this.Health_OnHit);
  }

  public void OnDestroy()
  {
    this.health.OnDie -= new Health.DieAction(this.Health_OnDie);
    this.health.OnHit -= new Health.HitAction(this.Health_OnHit);
  }

  public void Health_OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    this.simpleSpineFlash.FlashFillRed();
  }

  public void Health_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    AudioManager.Instance.PlayOneShot(this.deathVO, this.gameObject);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    InventoryItem.SpawnBlackSoul(Mathf.RoundToInt(Random.Range(this.fervourToDrop.x * 2f, this.fervourToDrop.y * 2f) * TrinketManager.GetBlackSoulsMultiplier(PlayerFarming.Instance)), this.transform.position);
    for (int index = 0; index < this.spawnedProjectiles.Count; ++index)
    {
      if ((Object) this.spawnedProjectiles[index] != (Object) null)
        Object.Destroy((Object) this.spawnedProjectiles[index].gameObject);
    }
    this.spawnedProjectiles.Clear();
    if (!((Object) this.deathParticles != (Object) null))
      return;
    this.deathParticles.transform.parent = this.transform.parent;
    this.deathParticles.Play();
  }

  public void Update()
  {
    this.timer += Time.deltaTime;
    if ((double) this.timer < 1.0)
    {
      this.BounceChild();
    }
    else
    {
      if (this.attacking)
        return;
      this.StartCoroutine((IEnumerator) this.AttackIE());
    }
  }

  public void BounceChild()
  {
    if ((double) this.childZ >= 0.0)
    {
      if ((double) this.vz > 0.079999998211860657)
      {
        this.vz *= -0.4f;
        this.squishScale = new Vector2(0.8f, 1.2f);
        this.Spine.AnimationState.SetAnimation(0, this.BounceAnimation, false);
        this.Spine.AnimationState.AddAnimation(0, this.IdleAnimation, true, 0.0f);
        AudioManager.Instance.PlayOneShot(this.landOnGroundSFX, this.gameObject);
      }
      else
        this.vz = 0.0f;
      this.childZ = 0.0f;
    }
    else
      this.vz += (float) (0.019999999552965164 * (double) Time.deltaTime * 60.0);
    this.childZ += (float) ((double) this.vz * (double) Time.deltaTime * 60.0);
    this.child.transform.localPosition = new Vector3(0.0f, 0.0f, this.childZ);
  }

  public IEnumerator AttackIE()
  {
    YngyaChunk yngyaChunk = this;
    yngyaChunk.attacking = true;
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.5f, yngyaChunk.Spine);
    if (yngyaChunk.chunkType == YngyaChunk.ChunkType.ProjectileRing)
      yield return (object) yngyaChunk.StartCoroutine((IEnumerator) yngyaChunk.ProjectileRingAttack());
    else if (yngyaChunk.chunkType == YngyaChunk.ChunkType.ProjectileSplatter)
      yield return (object) yngyaChunk.StartCoroutine((IEnumerator) yngyaChunk.ProjectileSplatAttack());
    else if (yngyaChunk.chunkType == YngyaChunk.ChunkType.Flames)
      yield return (object) yngyaChunk.StartCoroutine((IEnumerator) yngyaChunk.FlameAttack());
    string soundPath = "";
    switch (yngyaChunk.chunkType)
    {
      case YngyaChunk.ChunkType.ProjectileRing:
        soundPath = yngyaChunk.attackCircleExplodeSFX;
        break;
      case YngyaChunk.ChunkType.ProjectileSplatter:
        soundPath = yngyaChunk.attackSpitExplodeSFX;
        break;
    }
    if (!string.IsNullOrEmpty(soundPath))
      AudioManager.Instance.PlayOneShot(soundPath, yngyaChunk.gameObject);
    if (!string.IsNullOrEmpty(yngyaChunk.deathVO))
      AudioManager.Instance.PlayOneShot(yngyaChunk.deathVO, yngyaChunk.gameObject);
    if ((Object) yngyaChunk.deathParticles != (Object) null)
    {
      yngyaChunk.deathParticles.transform.parent = yngyaChunk.transform.parent;
      yngyaChunk.deathParticles.Play();
    }
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    InventoryItem.SpawnBlackSoul(Mathf.RoundToInt(Random.Range(yngyaChunk.fervourToDrop.x, yngyaChunk.fervourToDrop.y) * TrinketManager.GetBlackSoulsMultiplier(PlayerFarming.Instance)), yngyaChunk.transform.position);
    Object.Destroy((Object) yngyaChunk.gameObject);
  }

  public IEnumerator ProjectileRingAttack()
  {
    YngyaChunk yngyaChunk = this;
    yngyaChunk.spawnedProjectiles.Clear();
    yngyaChunk.Spine.AnimationState.SetAnimation(0, yngyaChunk.ExplodeStartAnimation, true);
    yngyaChunk.StartCoroutine((IEnumerator) yngyaChunk.FlashFillWhite((float) ((double) yngyaChunk.amount * 0.05000000074505806 + 2.5)));
    AudioManager.Instance.PlayOneShot(yngyaChunk.attackCircleStartSFX, yngyaChunk.gameObject);
    float aimingAngle = Random.Range(0.0f, 360f);
    for (int i = 0; i < yngyaChunk.amount; ++i)
    {
      Vector3 position = yngyaChunk.transform.position + (Vector3) Utils.DegreeToVector2(aimingAngle) * 1f;
      aimingAngle += (float) (360 / yngyaChunk.amount);
      Projectile component = ObjectPool.Spawn(yngyaChunk.prefab, position, Quaternion.identity).GetComponent<Projectile>();
      component.Angle = aimingAngle;
      component.Speed = 0.0f;
      yngyaChunk.spawnedProjectiles.Add(component);
      yield return (object) CoroutineStatics.WaitForScaledSeconds(0.05f, yngyaChunk.Spine);
    }
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.5f, yngyaChunk.Spine);
    yngyaChunk.Spine.AnimationState.SetAnimation(0, yngyaChunk.ExplodeAnimation, true);
    AudioManager.Instance.PlayOneShot(yngyaChunk.attackCircleWindupSFX, yngyaChunk.gameObject);
    AudioManager.Instance.PlayOneShot(yngyaChunk.attackCircleWindupVO, yngyaChunk.gameObject);
    float time = 0.0f;
    while ((double) time < 2.0)
    {
      time += Time.deltaTime;
      float num = time / 2f;
      foreach (Projectile spawnedProjectile in yngyaChunk.spawnedProjectiles)
      {
        Vector3 vector3 = (Vector3) (Random.insideUnitCircle * num * 0.25f) with
        {
          z = spawnedProjectile.ArrowImage.transform.localPosition.z
        };
        spawnedProjectile.ArrowImage.transform.localPosition = vector3;
      }
      yield return (object) null;
    }
    for (int index = 0; index < yngyaChunk.spawnedProjectiles.Count; ++index)
    {
      yngyaChunk.spawnedProjectiles[index].team = Health.Team.Team2;
      yngyaChunk.spawnedProjectiles[index].ArrowImage.localPosition = new Vector3(0.0f, 0.0f, yngyaChunk.spawnedProjectiles[index].ArrowImage.localPosition.z);
      yngyaChunk.spawnedProjectiles[index].Speed = yngyaChunk.speed;
    }
  }

  public IEnumerator ProjectileSplatAttack()
  {
    YngyaChunk yngyaChunk = this;
    yngyaChunk.Spine.AnimationState.SetAnimation(0, yngyaChunk.SplatterAnimation, true);
    yngyaChunk.StartCoroutine((IEnumerator) yngyaChunk.FlashFillWhite((float) ((double) yngyaChunk.amount * 0.05000000074505806 + 2.0)));
    AudioManager.Instance.PlayOneShot(yngyaChunk.attackSpitStartSFX, yngyaChunk.gameObject);
    AudioManager.Instance.PlayOneShot(yngyaChunk.attackSpitStartVO, yngyaChunk.gameObject);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(2f, yngyaChunk.Spine);
    int i = -1;
    while (++i < yngyaChunk.amount)
    {
      GrenadeBullet component = ObjectPool.Spawn(yngyaChunk.prefab, yngyaChunk.transform.position, Quaternion.identity).GetComponent<GrenadeBullet>();
      component.SetOwner(yngyaChunk.gameObject);
      component.Play(-1f, (float) Random.Range(0, 360), Random.Range(2f, 4f), Random.Range(yngyaChunk.speed - 2f, yngyaChunk.speed + 2f));
      AudioManager.Instance.PlayOneShot(yngyaChunk.attackSpitProjectileLaunchSFX, yngyaChunk.gameObject);
      yield return (object) CoroutineStatics.WaitForScaledSeconds(0.05f, yngyaChunk.Spine);
    }
  }

  public IEnumerator FlameAttack()
  {
    YngyaChunk yngyaChunk = this;
    yngyaChunk.StartCoroutine((IEnumerator) yngyaChunk.FlashFillWhite(1.5f));
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1.5f, yngyaChunk.Spine);
    yngyaChunk.child.gameObject.SetActive(false);
    TrapLavaTimed component = yngyaChunk.prefab.GetComponent<TrapLavaTimed>();
    component.gameObject.SetActive(true);
    component.ActivateLava();
    yield return (object) CoroutineStatics.WaitForScaledSeconds(5f, yngyaChunk.Spine);
  }

  public IEnumerator FlashFillWhite(float dur)
  {
    float time = 0.0f;
    while ((double) (time += Time.deltaTime) < (double) dur)
    {
      this.simpleSpineFlash.FlashWhite(time / dur);
      yield return (object) null;
    }
    this.simpleSpineFlash.FlashWhite(false);
  }

  public enum ChunkType
  {
    ProjectileRing,
    ProjectileSplatter,
    Flames,
    Enemies,
  }
}
