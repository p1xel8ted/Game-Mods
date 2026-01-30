// Decompiled with JetBrains decompiler
// Type: TrapCharger
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
  [SerializeField]
  public bool animate;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DirectionAnimationStop;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DirectionAnimationIdle;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DirectionAnimationUp;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DirectionAnimationDown;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DirectionAnimationLeft;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DirectionAnimationRight;
  public TrapCharger.AttackDirection currentDirection;
  [SerializeField]
  public GameObject TargetObject;
  [SerializeField]
  public GameObject ActiveToggle;
  public LayerMask layerToCheck;
  public float AttackDelayTime;
  [SerializeField]
  public int enemyDamage = 3;
  [HideInInspector]
  public float AttackDelay;
  public float MovementSpeed = 1f;
  public float SignPostAttackDuration = 0.5f;
  public Vector2 scanDimensions = new Vector2(1f, 1f);
  [Space]
  [SerializeField]
  public Vector3[] path = new Vector3[0];
  [SerializeField]
  public bool loop;
  [SerializeField]
  public LineRenderer additionalLine;
  [SerializeField]
  public bool autoMove;
  [SerializeField]
  public float delay = 2f;
  public float shakeDuration = 0.25f;
  public float shakeDistance = 0.25f;
  public ParticleSystem movementParticles;
  public ParticleSystem impactParticles;
  public LineRenderer lineRenderer;
  public int pathIndex;
  public bool deactivated;
  public LayerMask playerMask;
  public int interruptedIndex;
  public bool interrupted;
  public float autoMoveTimer;
  public int autoMoveDirection = 1;
  [HideInInspector]
  public Rigidbody2D rb;
  public bool DisableForces;

  public LineRenderer AdditionalLine => this.additionalLine;

  public void Awake()
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
    if (!((Object) this.additionalLine != (Object) null))
      return;
    this.additionalLine.positionCount = this.lineRenderer.positionCount;
    for (int index = this.lineRenderer.positionCount - 1; index >= 0; --index)
      this.additionalLine.SetPosition(this.lineRenderer.positionCount - 1 - index, this.lineRenderer.GetPosition(index));
  }

  public void Start()
  {
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  public void OnDestroy()
  {
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  public void RoomLockController_OnRoomCleared()
  {
    if (this.gameObject.activeInHierarchy)
      this.deactivated = true;
    this.ActiveToggle.transform.DOScale(Vector3.zero, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.ActiveToggle.SetActive(false)));
  }

  public void OnEnable()
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

  public void OnDisable()
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

  public virtual void OnDamageTriggerEnter(Collider2D collider)
  {
    if (this.enemyDamage <= 0)
      return;
    Health component = collider.GetComponent<Health>();
    PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(collider.gameObject);
    if (!((Object) component != (Object) null) || component.team == Health.Team.PlayerTeam && TrinketManager.HasTrinket(TarotCards.Card.ImmuneToTraps, farmingComponent) || component.ImmuneToTraps)
      return;
    component.DealDamage(component.team == Health.Team.Team2 ? (float) this.enemyDamage : 1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public IEnumerator WaitForTargetRoutine()
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
          Vector3 position = trapCharger.path[trapCharger.pathIndex];
          Vector3 targetPosition = Vector3.zero;
          if (trapCharger.transform.position == position && trapCharger.interrupted)
            trapCharger.interrupted = false;
          int v1_1 = (int) Utils.Repeat((float) (trapCharger.pathIndex + 1), (float) trapCharger.path.Length);
          int v1_2 = (int) Utils.Repeat((float) (trapCharger.pathIndex - 1), (float) trapCharger.path.Length);
          if (trapCharger.pathIndex + 1 > trapCharger.path.Length - 1 && trapCharger.loop)
            ++v1_1;
          if (trapCharger.pathIndex - 1 < 0 && trapCharger.loop)
            --v1_2;
          if (!trapCharger.loop && trapCharger.pathIndex - 1 < 0)
            v1_2 = 0;
          else if (!trapCharger.loop && trapCharger.pathIndex + 1 > trapCharger.path.Length - 1)
            v1_1 = trapCharger.pathIndex;
          if (trapCharger.interrupted)
          {
            v1_1 = trapCharger.pathIndex;
            v1_2 = trapCharger.interruptedIndex;
            position = trapCharger.transform.position;
          }
          vector3 = trapCharger.path[v1_1] - position;
          Vector3 normalized2 = vector3.normalized;
          vector3 = trapCharger.path[v1_2] - position;
          Vector3 normalized3 = vector3.normalized;
          int pathIndex = trapCharger.pathIndex;
          if (trapCharger.autoMove)
          {
            if ((double) Time.time < (double) trapCharger.autoMoveTimer)
            {
              yield return (object) null;
              continue;
            }
            if (trapCharger.pathIndex + trapCharger.autoMoveDirection >= trapCharger.path.Length || trapCharger.pathIndex + trapCharger.autoMoveDirection < 0)
            {
              trapCharger.autoMoveDirection *= -1;
              trapCharger.pathIndex += trapCharger.autoMoveDirection;
            }
            else
              trapCharger.pathIndex += trapCharger.autoMoveDirection;
            targetPosition = trapCharger.path[trapCharger.pathIndex];
          }
          else if ((bool) Physics2D.BoxCast((Vector2) (trapCharger.transform.position + normalized2), (Vector2) (Vector3.one * 2f), 0.0f, (Vector2) normalized2, Vector3.Distance(trapCharger.path[v1_1], position), (int) trapCharger.playerMask))
          {
            targetPosition = trapCharger.path[v1_1];
            trapCharger.pathIndex = (int) Utils.Repeat((float) v1_1, (float) trapCharger.path.Length);
          }
          else if ((bool) Physics2D.BoxCast((Vector2) (trapCharger.transform.position + normalized3), (Vector2) (Vector3.one * 2f), 0.0f, (Vector2) normalized3, Vector3.Distance(trapCharger.path[v1_2], position), (int) trapCharger.playerMask))
          {
            targetPosition = trapCharger.path[v1_2];
            trapCharger.pathIndex = (int) Utils.Repeat((float) v1_2, (float) trapCharger.path.Length);
          }
          else
          {
            yield return (object) null;
            continue;
          }
          trapCharger.interrupted = false;
          vector3 = targetPosition - trapCharger.transform.position;
          Vector3 normalized4 = vector3.normalized;
          RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) (trapCharger.transform.position + normalized4 * 1.5f), (Vector2) normalized4, Vector3.Distance(trapCharger.transform.position, targetPosition), (int) trapCharger.layerToCheck);
          if ((Object) raycastHit2D.collider != (Object) null)
          {
            targetPosition = trapCharger.transform.position + normalized4 * (Vector3.Distance(trapCharger.transform.position, raycastHit2D.collider.transform.position) - 1.5f);
            if (pathIndex != trapCharger.pathIndex)
              trapCharger.interruptedIndex = pathIndex;
            trapCharger.interrupted = true;
          }
          string animation = "";
          if (Mathf.RoundToInt(normalized4.x) < 0 && Mathf.RoundToInt(normalized4.y) == 0)
          {
            trapCharger.currentDirection = TrapCharger.AttackDirection.Left;
            animation = trapCharger.DirectionAnimationLeft;
          }
          else if (Mathf.RoundToInt(normalized4.x) > 0 && Mathf.RoundToInt(normalized4.y) == 0)
          {
            trapCharger.currentDirection = TrapCharger.AttackDirection.Right;
            animation = trapCharger.DirectionAnimationRight;
          }
          else if (Mathf.RoundToInt(normalized4.x) == 0 && Mathf.RoundToInt(normalized4.y) > 0)
          {
            trapCharger.currentDirection = TrapCharger.AttackDirection.Up;
            animation = trapCharger.DirectionAnimationUp;
          }
          else if (Mathf.RoundToInt(normalized4.x) == 0 && Mathf.RoundToInt(normalized4.y) < 0)
          {
            trapCharger.currentDirection = TrapCharger.AttackDirection.Down;
            animation = trapCharger.DirectionAnimationDown;
          }
          trapCharger.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
          if (trapCharger.autoMove)
            AudioManager.Instance.PlayOneShot("event:/dlc/env/puzzle_room/effigy_move_start", trapCharger.gameObject);
          else
            AudioManager.Instance.PlayOneShot("event:/enemy/moving_spike_trap/moving_spike_trap_start", trapCharger.gameObject);
          foreach (SimpleSpineFlash simpleSpineFlash in trapCharger.SimpleSpineFlashes)
            simpleSpineFlash.FlashWhite(0.5f);
          foreach (SimpleSpriteFlash simpleSpriteFlash in trapCharger.SimpleSpriteFlashes)
            simpleSpriteFlash.FlashWhite(0.5f);
          yield return (object) new WaitForSeconds(trapCharger.SignPostAttackDuration);
          if (trapCharger.animate)
            trapCharger.Spine.AnimationState.SetAnimation(0, animation, true);
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
          TweenerCore<Vector3, Vector3, VectorOptions> tween = trapCharger.transform.DOMove(targetPosition, trapCharger.MovementSpeed).SetSpeedBased<TweenerCore<Vector3, Vector3, VectorOptions>>().SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => moving = false));
          while (moving)
          {
            if (PlayerRelic.TimeFrozen && tween.IsPlaying())
              tween.Pause<TweenerCore<Vector3, Vector3, VectorOptions>>();
            else if (!PlayerRelic.TimeFrozen && !tween.IsPlaying())
              tween.Play<TweenerCore<Vector3, Vector3, VectorOptions>>();
            yield return (object) null;
          }
          trapCharger.EndCharge();
          trapCharger.BlockingCollider.enabled = true;
          if (trapCharger.interrupted)
            trapCharger.autoMoveDirection *= -1;
          if (trapCharger.animate)
          {
            trapCharger.Spine.AnimationState.SetAnimation(0, trapCharger.DirectionAnimationStop, false);
            trapCharger.Spine.AnimationState.AddAnimation(0, trapCharger.DirectionAnimationIdle, true, 0.0f);
          }
          yield return (object) new WaitForSeconds(1f);
          trapCharger.state.CURRENT_STATE = StateMachine.State.Idle;
          trapCharger.autoMoveTimer = Time.time + trapCharger.delay;
          targetPosition = new Vector3();
          animation = (string) null;
          tween = (TweenerCore<Vector3, Vector3, VectorOptions>) null;
          break;
        }
        yield return (object) null;
        break;
      }
      yield return (object) null;
    }
  }

  public void OnDrawGizmos()
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

  public void EndCharge()
  {
    if (this.autoMove)
      AudioManager.Instance.PlayOneShot("event:/dlc/env/puzzle_room/effigy_move_stop", this.gameObject);
    else
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

  public IEnumerator ShakeRoutine(TrapCharger.AttackDirection attackDirection)
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

  [CompilerGenerated]
  public void \u003CRoomLockController_OnRoomCleared\u003Eb__50_0()
  {
    this.ActiveToggle.SetActive(false);
  }

  public enum AttackDirection
  {
    Up,
    Down,
    Left,
    Right,
  }
}
