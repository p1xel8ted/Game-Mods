// Decompiled with JetBrains decompiler
// Type: EnemyJellyGrower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyJellyGrower : EnemyExploder
{
  [SerializeField]
  public float increaseAmount;
  [SerializeField]
  public Vector2 growInterval;
  [SerializeField]
  public float decreasePerHit;
  [SerializeField]
  public float maxGrowSize;
  [SerializeField]
  public float anticipationDuration = 0.5f;
  public GameObject Prefab;
  public Vector2 DelayBetweenShots = new Vector2(0.1f, 0.3f);
  public float NumberOfShotsToFire = 5f;
  public float GravSpeed = -15f;
  public Vector2 RandomArcOffset = new Vector2(0.0f, 0.0f);
  public Vector2 ShootDistanceRange = new Vector2(2f, 3f);
  public Vector3 ShootOffset;
  public GameObject g;
  public GrenadeBullet GrenadeBullet;
  public bool killed;
  public bool anticipating;
  public float anticipateTimer;
  public float growTimestamp = -1f;

  public override void FixedUpdate()
  {
    base.FixedUpdate();
    if (!this.inRange)
      return;
    if ((double) this.growTimestamp == -1.0)
      this.growTimestamp = this.gm.CurrentTime + Random.Range(this.growInterval.x, this.growInterval.y);
    if (!this.anticipating && (double) this.gm.CurrentTime > (double) this.growTimestamp)
      this.Grow();
    if (!this.anticipating)
      return;
    this.anticipateTimer += Time.deltaTime;
    float amt = this.anticipateTimer / this.anticipationDuration;
    this.simpleSpineFlash.FlashWhite(amt);
    if ((double) amt < 1.0)
      return;
    this.Pop();
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.Spine.transform.localScale -= Vector3.one * this.decreasePerHit;
    this.Spine.transform.localPosition = new Vector3(this.Spine.transform.localPosition.x, this.Spine.transform.localPosition.y, this.Spine.transform.localScale.x / this.maxGrowSize);
    this.growTimestamp = this.gm.CurrentTime + Random.Range(this.growInterval.x, this.growInterval.y);
    if ((double) this.Spine.transform.localScale.x >= 1.0 || this.killed)
      return;
    this.killed = true;
    this.health.DealDamage(this.health.totalHP, Attacker, AttackLocation, AttackType: Health.AttackTypes.Heavy);
  }

  public void Grow()
  {
    if ((double) this.Spine.transform.localScale.x >= (double) this.maxGrowSize)
    {
      this.anticipating = true;
      this.anticipateTimer = 0.0f;
    }
    else
    {
      this.Spine.transform.localScale += Vector3.one * this.increaseAmount;
      this.Spine.transform.DOPunchScale(Vector3.one * 0.3f, 0.5f).SetEase<Tweener>(Ease.OutBounce);
      this.Spine.transform.DOLocalMove(new Vector3(this.Spine.transform.localPosition.x, this.Spine.transform.localPosition.y, this.Spine.transform.localScale.x / this.maxGrowSize), 0.25f);
      this.anticipating = false;
      this.simpleSpineFlash.FlashWhite(false);
      this.growTimestamp = this.gm.CurrentTime + Random.Range(this.growInterval.x, this.growInterval.y);
    }
  }

  public void Pop()
  {
    this.anticipating = false;
    this.StartCoroutine((IEnumerator) this.ShootRoutine());
    this.health.DealDamage(this.health.totalHP, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Heavy);
  }

  public IEnumerator ShootRoutine()
  {
    EnemyJellyGrower enemyJellyGrower = this;
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.2f);
    float randomStartAngle = (float) Random.Range(0, 360);
    int i = -1;
    while ((double) ++i < (double) enemyJellyGrower.NumberOfShotsToFire)
    {
      AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile", enemyJellyGrower.transform.position);
      float Angle = randomStartAngle + Random.Range(enemyJellyGrower.RandomArcOffset.x, enemyJellyGrower.RandomArcOffset.y);
      enemyJellyGrower.GrenadeBullet = ObjectPool.Spawn(enemyJellyGrower.Prefab, enemyJellyGrower.transform.parent, enemyJellyGrower.transform.position + enemyJellyGrower.ShootOffset, Quaternion.identity).GetComponent<GrenadeBullet>();
      enemyJellyGrower.GrenadeBullet.SetOwner(enemyJellyGrower.gameObject);
      enemyJellyGrower.GrenadeBullet.Play(-1f, Angle, Random.Range(enemyJellyGrower.ShootDistanceRange.x, enemyJellyGrower.ShootDistanceRange.y), Random.Range(enemyJellyGrower.GravSpeed - 0.5f, enemyJellyGrower.GravSpeed + 0.5f), enemyJellyGrower.health.team);
      randomStartAngle = Utils.Repeat(randomStartAngle + 360f / enemyJellyGrower.NumberOfShotsToFire, 360f);
      if (enemyJellyGrower.DelayBetweenShots != Vector2.zero)
        yield return (object) new WaitForSeconds(Random.Range(enemyJellyGrower.DelayBetweenShots.x, enemyJellyGrower.DelayBetweenShots.y));
    }
  }
}
