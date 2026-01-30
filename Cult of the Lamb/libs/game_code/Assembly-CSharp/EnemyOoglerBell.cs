// Decompiled with JetBrains decompiler
// Type: EnemyOoglerBell
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

#nullable disable
public class EnemyOoglerBell : MonoBehaviour
{
  [SerializeField]
  public float speed;
  [SerializeField]
  public float dropTime;
  [SerializeField]
  public float dropAnticipationTime;
  [SerializeField]
  public float dropInBackTime;
  [SerializeField]
  public float dropInBackAmount;
  [SerializeField]
  public float recoverWaitTime;
  [SerializeField]
  public float recoverTime;
  [SerializeField]
  public float attackDistance;
  [SerializeField]
  public float attackYOffset;
  [SerializeField]
  public Health health;
  [SerializeField]
  public Rigidbody2D rb;
  [SerializeField]
  public ColliderEvents damageCollider;
  [SerializeField]
  public float damage = 1f;
  [SerializeField]
  public float timeBetweenBells = 2f;
  [SerializeField]
  public float zOffset = -2f;
  [SerializeField]
  public SpriteRenderer indicator;
  [SerializeField]
  public Transform origin;
  [SerializeField]
  public GameObject avalanche;
  [FormerlySerializedAs("dropInterval")]
  [SerializeField]
  public float dropSpawnInterval = 1f;
  [SerializeField]
  public float dropSpeed;
  [SerializeField]
  public float followTime;
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  public bool avalanchesEnabled;
  public GameObject currentTarget;
  public ParticleSystem appearVFX;
  public GameObject impactVFX;
  public float shakeIntensity = 2f;
  public float flashTickTimer;
  public Color indicatorColor = Color.white;
  public static float lastSlamTime;
  public Health casterHealth;
  public Health EnemyHealth;

  public Health Health => this.health;

  public void Initialize(UnitObject caster, GameObject target)
  {
    this.appearVFX.Play();
    this.Spine.AnimationState.SetAnimation(0, "appear", false);
    this.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    this.currentTarget = target;
    this.casterHealth = caster.GetComponent<Health>();
    if ((Object) this.casterHealth != (Object) null)
      this.casterHealth.OnDieCallback.AddListener(new UnityAction(this.OnCasterDied));
    if ((Object) this.damageCollider != (Object) null)
    {
      this.damageCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageCollider.SetActive(false);
    }
    this.transform.position = new Vector3(this.currentTarget.transform.position.x, this.currentTarget.transform.position.y, this.zOffset);
    this.StartCoroutine((IEnumerator) this.FollowTarget());
  }

  public void OnDisable()
  {
    this.indicator.gameObject.SetActive(false);
    this.indicator.transform.parent = this.transform;
  }

  public void Start()
  {
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.TryDispose);
  }

  public void OnDestroy()
  {
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.TryDispose);
  }

  public IEnumerator StartDroppingAvalanches()
  {
    float intervalTime = this.dropSpawnInterval;
    while (this.avalanchesEnabled)
    {
      while (PlayerRelic.TimeFrozen)
        yield return (object) null;
      intervalTime -= Time.deltaTime * this.Spine.timeScale;
      if ((double) intervalTime <= 0.0)
      {
        this.DropAvalanche();
        intervalTime = this.dropSpawnInterval;
      }
      yield return (object) null;
    }
  }

  public void Update()
  {
    if ((double) Time.timeScale == 0.0)
      return;
    if (this.indicator.gameObject.activeSelf && (double) this.flashTickTimer >= 0.11999999731779099 && BiomeConstants.Instance.IsFlashLightsActive)
    {
      this.indicatorColor = this.indicatorColor == Color.white ? Color.red : Color.white;
      this.indicator.material.SetColor("_Color", this.indicatorColor);
      this.flashTickTimer = 0.0f;
    }
    this.flashTickTimer += Time.deltaTime;
  }

  public IEnumerator FollowTarget()
  {
    EnemyOoglerBell enemyOoglerBell = this;
    yield return (object) new WaitForSeconds(0.5f);
    while (!((Object) enemyOoglerBell.currentTarget == (Object) null))
    {
      Vector3 target = new Vector3(enemyOoglerBell.currentTarget.transform.position.x, enemyOoglerBell.currentTarget.transform.position.y, enemyOoglerBell.zOffset);
      enemyOoglerBell.transform.position = Vector3.MoveTowards(enemyOoglerBell.transform.position, target, Time.deltaTime * enemyOoglerBell.speed);
      if ((double) Vector2.Distance((Vector2) (enemyOoglerBell.transform.position + Vector3.up * enemyOoglerBell.attackYOffset), (Vector2) enemyOoglerBell.currentTarget.transform.position) <= (double) enemyOoglerBell.attackDistance && (double) Time.time > (double) EnemyOoglerBell.lastSlamTime)
      {
        enemyOoglerBell.StartCoroutine((IEnumerator) enemyOoglerBell.Drop());
        break;
      }
      yield return (object) null;
    }
  }

  public IEnumerator Drop()
  {
    EnemyOoglerBell enemyOoglerBell = this;
    EnemyOoglerBell.lastSlamTime = Time.time + 2f;
    DG.Tweening.Sequence s = DOTween.Sequence();
    enemyOoglerBell.StartCoroutine((IEnumerator) enemyOoglerBell.FlashWhite(enemyOoglerBell.dropAnticipationTime));
    enemyOoglerBell.Spine.AnimationState.SetAnimation(0, "attack-charge", false);
    s.Append((Tween) enemyOoglerBell.transform.DOMoveZ(enemyOoglerBell.dropInBackAmount, enemyOoglerBell.dropInBackTime).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine));
    s.AppendInterval(enemyOoglerBell.dropAnticipationTime);
    s.AppendCallback(new TweenCallback(enemyOoglerBell.\u003CDrop\u003Eb__41_0));
    s.Append((Tween) enemyOoglerBell.transform.DOMoveZ(0.0f, enemyOoglerBell.dropTime).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine));
    enemyOoglerBell.indicator.transform.parent = (Transform) null;
    enemyOoglerBell.indicator.transform.position = new Vector3(enemyOoglerBell.transform.position.x, enemyOoglerBell.transform.position.y, 0.0f);
    enemyOoglerBell.indicator.gameObject.SetActive(true);
    yield return (object) s.WaitForPosition((float) (((double) enemyOoglerBell.dropTime + (double) enemyOoglerBell.dropInBackTime + (double) enemyOoglerBell.dropAnticipationTime) * 0.89999997615814209));
    enemyOoglerBell.damageCollider.SetActive(true);
    enemyOoglerBell.indicator.gameObject.SetActive(false);
    yield return (object) s.WaitForCompletion();
    Object.Instantiate<GameObject>(enemyOoglerBell.impactVFX, enemyOoglerBell.transform.position, Quaternion.identity);
    CameraManager.shakeCamera(enemyOoglerBell.shakeIntensity, 270f);
    enemyOoglerBell.Spine.AnimationState.SetAnimation(0, "attack-impact", false);
    enemyOoglerBell.Spine.AnimationState.SetAnimation(0, "down-idle", true);
    yield return (object) new WaitForSeconds(0.25f);
    enemyOoglerBell.damageCollider.SetActive(false);
    Object.Destroy((Object) enemyOoglerBell.gameObject);
  }

  public IEnumerator FlashWhite(float time)
  {
    float elapsed = 0.0f;
    while ((double) elapsed < (double) time)
    {
      this.SimpleSpineFlash.FlashWhite(elapsed / time);
      elapsed += Time.deltaTime;
      yield return (object) null;
    }
    this.SimpleSpineFlash.FlashWhite(false);
  }

  public IEnumerator Recover()
  {
    EnemyOoglerBell enemyOoglerBell = this;
    enemyOoglerBell.Spine.AnimationState.SetAnimation(0, "attack-charge", false);
    enemyOoglerBell.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    yield return (object) enemyOoglerBell.transform.DOMoveZ(enemyOoglerBell.zOffset, enemyOoglerBell.recoverTime).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).WaitForCompletion();
    yield return (object) new WaitForSeconds(0.1f);
    enemyOoglerBell.StartCoroutine((IEnumerator) enemyOoglerBell.FollowTarget());
  }

  public void DropAvalanche()
  {
    if ((Object) this.currentTarget == (Object) null)
      return;
    TrapAvalanche trap = ObjectPool.Spawn(this.avalanche, this.origin.position, Quaternion.identity).GetComponent<TrapAvalanche>();
    trap.DropSpeed = this.dropSpeed;
    this.BindTrapToTarget(trap);
    trap.onLand.AddListener((UnityAction) (() => this.UnBindTrapListeners(trap)));
    trap.Drop();
    trap.chaseTarget = this.currentTarget.transform;
    this.Spine.AnimationState.SetAnimation(0, "attack-charge", false);
    this.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/building/building_bell_ring", this.gameObject);
  }

  public void BindTrapToTarget(TrapAvalanche trap)
  {
    trap.chaseTarget = this.currentTarget.transform;
    Debug.Log((object) this.followTime);
    DOVirtual.DelayedCall(this.followTime * this.Spine.timeScale, (TweenCallback) (() => trap.chaseTarget = (Transform) null));
  }

  public void UnBindTrapListeners(TrapAvalanche trap)
  {
    trap.onDrop.RemoveListener((UnityAction) (() => this.BindTrapToTarget(trap)));
  }

  public void SetTarget(Transform target) => this.transform.SetParent(target);

  public void OnCasterDied()
  {
    this.StopAllCoroutines();
    this.avalanchesEnabled = false;
    this.TryDispose();
  }

  public void TryDispose()
  {
    if ((Object) this.casterHealth != (Object) null)
      this.casterHealth.OnDieCallback.RemoveListener(new UnityAction(this.OnCasterDied));
    BellMechanics.RemoveOoglerBellFromTarget(this.currentTarget);
    Object.Destroy((Object) this.gameObject);
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((Object) this.EnemyHealth != (Object) null))
      return;
    if (this.EnemyHealth.team != this.health.team)
    {
      this.EnemyHealth.DealDamage(this.damage, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
    }
    else
    {
      if (this.health.team != Health.Team.PlayerTeam || this.health.isPlayerAlly || this.EnemyHealth.isPlayer)
        return;
      this.EnemyHealth.DealDamage(this.damage, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
    }
  }

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    DOVirtual.DelayedCall(this.timeBetweenBells, (TweenCallback) (() => BellMechanics.RemoveOoglerBellFromTarget(this.currentTarget)));
  }

  public void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (this.health.HasShield || this.health.WasJustParried)
      return;
    if ((Object) this.damageCollider != (Object) null)
      this.damageCollider.SetActive(false);
    this.SimpleSpineFlash.FlashFillRed();
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(new Vector3(this.transform.position.x, this.transform.position.y, 0.0f) + Vector3.up * this.attackYOffset, this.attackDistance, Color.yellow);
  }

  [CompilerGenerated]
  public void \u003CDrop\u003Eb__41_0()
  {
    this.Spine.AnimationState.SetAnimation(0, "attack-fall", true);
  }

  [CompilerGenerated]
  public void \u003COnDie\u003Eb__52_0()
  {
    BellMechanics.RemoveOoglerBellFromTarget(this.currentTarget);
  }
}
