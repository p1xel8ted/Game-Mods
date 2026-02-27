// Decompiled with JetBrains decompiler
// Type: TrapCharger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class TrapCharger : BaseMonoBehaviour
{
  public StateMachine state;
  public ColliderEvents damageColliderEventsUp;
  public ColliderEvents damageColliderEventsDown;
  public ColliderEvents damageColliderEventsLeft;
  public ColliderEvents damageColliderEventsRight;
  public Collider2D BlockingCollider;
  public SkeletonAnimation Spine;
  public SimpleSpineFlash[] SimpleSpineFlashes;
  public SimpleSpriteFlash[] SimpleSpriteFlashes;
  public AnimationCurve shakeCurve;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DirectionAnimationOff;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DirectionAnimationUp;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DirectionAnimationDown;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DirectionAnimationLeft;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DirectionAnimationRight;
  private TrapCharger.AttackDirection currentDirection;
  [SerializeField]
  private GameObject TargetObject;
  [SerializeField]
  private GameObject ActiveToggle;
  public LayerMask layerToCheck;
  public float AttackDelayTime;
  [SerializeField]
  private int enemyDamage = 3;
  [HideInInspector]
  public float AttackDelay;
  public float MovementSpeed = 1f;
  public float SignPostAttackDuration = 0.5f;
  public Vector2 scanDimensions = new Vector2(1f, 1f);
  [Space]
  [SerializeField]
  private Vector3[] path = new Vector3[0];
  [SerializeField]
  private bool loop;
  private float shakeDuration = 0.25f;
  private float shakeDistance = 0.25f;
  public ParticleSystem movementParticles;
  public ParticleSystem impactParticles;
  private LineRenderer lineRenderer;
  private int pathIndex;
  private bool deactivated;
  private LayerMask playerMask;
  [HideInInspector]
  public Rigidbody2D rb;
  public bool DisableForces;

  private void Awake()
  {
    this.state = this.GetComponent<StateMachine>();
    this.rb = this.gameObject.GetComponent<Rigidbody2D>();
    Vector3[] vector3Array = new Vector3[this.loop ? this.path.Length + 1 : this.path.Length];
    for (int index = 0; index < this.path.Length; ++index)
      vector3Array[index] = this.transform.TransformPoint(this.path[index]);
    if (this.loop)
      vector3Array[vector3Array.Length - 1] = this.transform.TransformPoint(this.path[0]);
    this.path = vector3Array;
    this.lineRenderer = this.GetComponent<LineRenderer>();
    this.lineRenderer.positionCount = this.path.Length;
    this.lineRenderer.SetPositions(this.path);
    this.playerMask = (LayerMask) ((int) this.playerMask | 1 << LayerMask.NameToLayer("Player"));
  }

  private void Start()
  {
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  private void OnDestroy()
  {
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  private void RoomLockController_OnRoomCleared()
  {
    if (this.gameObject.activeInHierarchy)
      this.deactivated = true;
    this.ActiveToggle.transform.DOScale(Vector3.zero, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.ActiveToggle.SetActive(false)));
  }

  private void OnEnable()
  {
    if ((Object) this.damageColliderEventsUp != (Object) null)
    {
      this.damageColliderEventsUp.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEventsUp.SetActive(false);
    }
    if ((Object) this.damageColliderEventsDown != (Object) null)
    {
      this.damageColliderEventsDown.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEventsDown.SetActive(false);
    }
    if ((Object) this.damageColliderEventsLeft != (Object) null)
    {
      this.damageColliderEventsLeft.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEventsLeft.SetActive(false);
    }
    if ((Object) this.damageColliderEventsRight != (Object) null)
    {
      this.damageColliderEventsRight.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEventsRight.SetActive(false);
    }
    this.SimpleSpineFlashes = this.GetComponentsInChildren<SimpleSpineFlash>();
    this.SimpleSpriteFlashes = this.GetComponentsInChildren<SimpleSpriteFlash>();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.AttackDelay = 0.0f;
    this.StartCoroutine((IEnumerator) this.WaitForTargetRoutine());
  }

  private void OnDisable()
  {
    if ((Object) this.damageColliderEventsUp != (Object) null)
    {
      this.damageColliderEventsUp.SetActive(false);
      this.damageColliderEventsUp.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
    if ((Object) this.damageColliderEventsDown != (Object) null)
    {
      this.damageColliderEventsDown.SetActive(false);
      this.damageColliderEventsDown.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
    if ((Object) this.damageColliderEventsLeft != (Object) null)
    {
      this.damageColliderEventsLeft.SetActive(false);
      this.damageColliderEventsLeft.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
    if ((Object) this.damageColliderEventsRight != (Object) null)
    {
      this.damageColliderEventsRight.SetActive(false);
      this.damageColliderEventsRight.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
    this.StopAllCoroutines();
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashWhite(false);
    foreach (SimpleSpriteFlash simpleSpriteFlash in this.SimpleSpriteFlashes)
      simpleSpriteFlash.FlashWhite(false);
  }

  protected virtual void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((Object) component != (Object) null))
      return;
    component.DealDamage(component.team == Health.Team.Team2 ? (float) this.enemyDamage : 1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  protected IEnumerator WaitForTargetRoutine()
  {
    TrapCharger trapCharger = this;
    yield return (object) new WaitForEndOfFrame();
    if ((Object) trapCharger.Spine != (Object) null)
      yield return (object) new WaitForSeconds(trapCharger.AttackDelay);
    while ((Object) PlayerFarming.Instance == (Object) null)
      yield return (object) null;
    trapCharger.TargetObject = PlayerFarming.Instance.gameObject;
    while (true)
    {
      while (!trapCharger.deactivated)
      {
        if (trapCharger.state.CURRENT_STATE == StateMachine.State.Idle)
        {
          Vector3 vector3 = trapCharger.TargetObject.transform.position - trapCharger.transform.position;
          Vector3 normalized1 = vector3.normalized;
          Vector3 b = trapCharger.path[trapCharger.pathIndex];
          Vector3 targetPosition = Vector3.zero;
          int t1 = (int) Mathf.Repeat((float) (trapCharger.pathIndex + 1), (float) trapCharger.path.Length);
          int t2 = (int) Mathf.Repeat((float) (trapCharger.pathIndex - 1), (float) trapCharger.path.Length);
          if (trapCharger.pathIndex + 1 > trapCharger.path.Length - 1 && trapCharger.loop)
            ++t1;
          if (trapCharger.pathIndex - 1 < 0 && trapCharger.loop)
            --t2;
          if (!trapCharger.loop && trapCharger.pathIndex - 1 < 0)
            t2 = 0;
          else if (!trapCharger.loop && trapCharger.pathIndex + 1 > trapCharger.path.Length - 1)
            t1 = trapCharger.pathIndex;
          vector3 = trapCharger.path[t1] - b;
          Vector3 normalized2 = vector3.normalized;
          vector3 = trapCharger.path[t2] - b;
          Vector3 normalized3 = vector3.normalized;
          if ((bool) Physics2D.BoxCast((Vector2) trapCharger.transform.position, (Vector2) (Vector3.one * 2f), 0.0f, (Vector2) normalized2, Vector3.Distance(trapCharger.path[t1], b), (int) trapCharger.playerMask))
          {
            targetPosition = trapCharger.path[t1];
            trapCharger.pathIndex = (int) Mathf.Repeat((float) t1, (float) trapCharger.path.Length);
          }
          else if ((bool) Physics2D.BoxCast((Vector2) trapCharger.transform.position, (Vector2) (Vector3.one * 2f), 0.0f, (Vector2) normalized3, Vector3.Distance(trapCharger.path[t2], b), (int) trapCharger.playerMask))
          {
            targetPosition = trapCharger.path[t2];
            trapCharger.pathIndex = (int) Mathf.Repeat((float) t2, (float) trapCharger.path.Length);
          }
          else
          {
            yield return (object) null;
            continue;
          }
          trapCharger.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
          AudioManager.Instance.PlayOneShot("event:/enemy/moving_spike_trap/moving_spike_trap_start", trapCharger.gameObject);
          foreach (SimpleSpineFlash simpleSpineFlash in trapCharger.SimpleSpineFlashes)
            simpleSpineFlash.FlashWhite(0.5f);
          foreach (SimpleSpriteFlash simpleSpriteFlash in trapCharger.SimpleSpriteFlashes)
            simpleSpriteFlash.FlashWhite(0.5f);
          yield return (object) new WaitForSeconds(trapCharger.SignPostAttackDuration);
          foreach (SimpleSpineFlash simpleSpineFlash in trapCharger.SimpleSpineFlashes)
            simpleSpineFlash.FlashWhite(false);
          foreach (SimpleSpriteFlash simpleSpriteFlash in trapCharger.SimpleSpriteFlashes)
            simpleSpriteFlash.FlashWhite(false);
          trapCharger.state.CURRENT_STATE = StateMachine.State.Moving;
          trapCharger.movementParticles.Play();
          trapCharger.damageColliderEventsUp.SetActive(true);
          trapCharger.damageColliderEventsDown.SetActive(true);
          trapCharger.damageColliderEventsLeft.SetActive(true);
          trapCharger.damageColliderEventsRight.SetActive(true);
          trapCharger.state.LookAngle = trapCharger.state.facingAngle;
          trapCharger.BlockingCollider.enabled = false;
          bool moving = true;
          trapCharger.transform.DOMove(targetPosition, trapCharger.MovementSpeed).SetSpeedBased<TweenerCore<Vector3, Vector3, VectorOptions>>().SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => moving = false));
          while (moving)
            yield return (object) null;
          trapCharger.EndCharge();
          trapCharger.BlockingCollider.enabled = true;
          yield return (object) new WaitForSeconds(1f);
          trapCharger.state.CURRENT_STATE = StateMachine.State.Idle;
          targetPosition = new Vector3();
          break;
        }
        yield return (object) null;
        break;
      }
      yield return (object) null;
    }
  }

  private void OnDrawGizmos()
  {
    if (Application.isPlaying)
      return;
    Vector3[] positions = new Vector3[this.loop ? this.path.Length + 1 : this.path.Length];
    for (int index = 0; index < this.path.Length; ++index)
      positions[index] = this.transform.TransformPoint(this.path[index]);
    if (this.loop)
      positions[positions.Length - 1] = this.transform.TransformPoint(this.path[0]);
    this.GetComponent<LineRenderer>().positionCount = positions.Length;
    this.GetComponent<LineRenderer>().SetPositions(positions);
    foreach (Vector3 center in positions)
      Utils.DrawCircleXY(center, 0.5f, Color.blue);
    if ((Object) this.TargetObject == (Object) null)
      return;
    Utils.DrawLine(this.transform.position, this.transform.position + (this.TargetObject.transform.position - this.transform.position).normalized * (this.TargetObject.transform.position - this.transform.position).magnitude, Color.magenta);
  }

  private void EndCharge()
  {
    AudioManager.Instance.PlayOneShot("event:/enemy/moving_spike_trap/moving_spike_trap_stop", this.gameObject);
    this.damageColliderEventsUp.SetActive(false);
    this.damageColliderEventsDown.SetActive(false);
    this.damageColliderEventsLeft.SetActive(false);
    this.damageColliderEventsRight.SetActive(false);
    CameraManager.shakeCamera(2f);
    this.StartCoroutine((IEnumerator) this.ShakeRoutine(this.currentDirection));
    this.impactParticles.Play();
    this.AttackDelay = this.AttackDelayTime;
    this.movementParticles.Stop();
    this.StartCoroutine((IEnumerator) this.WaitForTargetRoutine());
  }

  private IEnumerator ShakeRoutine(TrapCharger.AttackDirection attackDirection)
  {
    TrapCharger trapCharger = this;
    float t = 0.0f;
    trapCharger.DisableForces = true;
    Vector3 targetPos = trapCharger.transform.position;
    Vector2 targetDir = Vector2.down;
    switch (attackDirection)
    {
      case TrapCharger.AttackDirection.Down:
        targetDir = Vector2.up;
        break;
      case TrapCharger.AttackDirection.Left:
        targetDir = Vector2.right;
        break;
      case TrapCharger.AttackDirection.Right:
        targetDir = Vector2.left;
        break;
    }
    while ((double) t < (double) trapCharger.shakeDuration)
    {
      trapCharger.transform.position = targetPos + (Vector3) (targetDir * trapCharger.shakeCurve.Evaluate(t / trapCharger.shakeDuration) * trapCharger.shakeDistance);
      t += Time.deltaTime;
      yield return (object) null;
    }
    trapCharger.transform.position = targetPos;
    trapCharger.DisableForces = false;
  }

  public enum AttackDirection
  {
    Up,
    Down,
    Left,
    Right,
  }
}
