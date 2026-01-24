// Decompiled with JetBrains decompiler
// Type: EnemyDogPack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using FMODUnity;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemyDogPack : UnitObject
{
  public static List<EnemyDogPack> Dogs = new List<EnemyDogPack>();
  public bool isAsleep;
  public bool isCircling;
  public bool isAttacking;
  [SerializeField]
  public GameObject SleepUI;
  [SerializeField]
  public GameObject WakeUI;
  [SerializeField]
  public GameObject AwarenessUI;
  [SerializeField]
  public float DamageMultiplierWhileSleeping = 2f;
  [SerializeField]
  public LayerMask borderMask;
  public ColliderEvents damageColliderEvents;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SleepAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string WakeUpAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string MovingAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SignPostAttackAnimation;
  public bool LoopSignPostAttackAnimation = true;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string LandAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DiveIntoGroundAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string HowlAnimation;
  [SerializeField]
  public SimpleSpineFlash[] SimpleSpineFlashes;
  public SkeletonAnimation warningIcon;
  public bool HowlToAlertDogsOnWake = true;
  [HideInInspector]
  public bool WasAlerted;
  public float WakeRange = 1.5f;
  public float SignPostAttackDuration = 0.2f;
  public Vector3 TargetPosition;
  public ShowHPBar ShowHPBar;
  public bool IsAttacking;
  public float IdleWait;
  public bool ShownWarning;
  public float originalDamageModifier = -1f;
  [HideInInspector]
  public float currentPlayerVolumeMultiplier = 0.5f;
  public float lastSilentTime;
  public float lineOfSightTime;
  public float circleLerpAcceleration = 0.01f;
  public Health circlingPlayer;
  public Health circlingTarget;
  public float lastDogAttack;
  [EventRef]
  public string AttackVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_basic/attack";
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_basic/death";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_basic/gethit";
  [EventRef]
  public string WarningVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_basic/warning";
  [EventRef]
  public string HowlVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_basic_small/howl";
  [EventRef]
  public string AttackBasicWindupSFX = "event:/dlc/dungeon05/enemy/dog_pack/attack_basic_windup";
  [EventRef]
  public string AttackBasicBiteSFX = "event:/dlc/dungeon05/enemy/dog_pack/attack_basic_bite";
  [EventRef]
  public string SnoreLoopSFX = "event:/dlc/dungeon05/enemy/dog_pack/mv_snore_loop";
  [EventRef]
  public string WakeUpSFX = "event:/dlc/dungeon05/enemy/dog_pack/mv_wakeup";
  public EventInstance snoreLoopInstanceSFX;
  public float PackAngle;
  public float PackCircleSpeed = 1f;
  public float CircleRadius = 3f;
  public Coroutine currentStateRoutine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string BackStunnedAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string BackStunnedResetAnimation;
  public float BackStunnedDuration = 2.25f;
  public bool EnableBackStun = true;
  public float BackStunHealthThreshold = 0.5f;
  public float lastPackUpdate;
  public float packUpdateInterval = 0.1f;
  public Vector3 oldPosition;
  public bool hasBegunCircle;

  public void StartState(IEnumerator newState)
  {
    this.isAsleep = false;
    this.isAttacking = false;
    this.isCircling = false;
    this.WakeUI.gameObject.SetActive(false);
    this.SleepUI.gameObject.SetActive(false);
    this.AwarenessUI.gameObject.SetActive(false);
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
    {
      DOTween.Kill((object) simpleSpineFlash.transform);
      simpleSpineFlash.FlashWhite(0.0f);
    }
    if (this.currentStateRoutine != null)
      this.StopCoroutine(this.currentStateRoutine);
    this.currentStateRoutine = this.StartCoroutine((IEnumerator) newState);
  }

  public override void Update()
  {
    if (EnemyDogPack.Dogs.Count > 0 && (UnityEngine.Object) EnemyDogPack.Dogs[0] == (UnityEngine.Object) this)
    {
      this.UpdatePackTargetPosition();
      this.PickNewAttackingDog();
    }
    base.Update();
  }

  public void UpdatePackTargetPosition()
  {
    if ((double) this.lastPackUpdate >= (double) Time.time + (double) this.packUpdateInterval)
      return;
    this.lastPackUpdate = Time.time;
    GameObject gameObject = PlayerFarming.Instance?.gameObject;
    Vector3 position;
    if ((UnityEngine.Object) this.circlingTarget != (UnityEngine.Object) null)
    {
      position = this.circlingTarget.transform.position;
    }
    else
    {
      if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
        return;
      position = gameObject.transform.position;
    }
    this.PackAngle += this.PackCircleSpeed * Time.deltaTime;
    if ((double) this.PackAngle > 6.2831854820251465)
      this.PackAngle -= 6.28318548f;
    for (int index = 0; index < EnemyDogPack.Dogs.Count; ++index)
    {
      EnemyDogPack dog = EnemyDogPack.Dogs[index];
      float f = this.PackAngle + (float) index * (6.28318548f / (float) EnemyDogPack.Dogs.Count);
      Vector3 vector3 = position + new Vector3(Mathf.Cos(f) * dog.CircleRadius, Mathf.Sin(f) * dog.CircleRadius, 0.0f);
      dog.TargetPosition = vector3;
    }
  }

  public override void Awake()
  {
    base.Awake();
    this.SimpleSpineFlashes = this.GetComponentsInChildren<SimpleSpineFlash>();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (this.health.Dormant && (double) this.originalDamageModifier == -1.0)
    {
      this.originalDamageModifier = this.health.DamageModifier;
      this.health.DamageModifier = this.DamageMultiplierWhileSleeping;
    }
    this.ShowHPBar = this.GetComponent<ShowHPBar>();
    EnemyDogPack.Dogs.Add(this);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    if (this.health.Dormant)
      this.StartState(this.SleepRoutineState());
    else
      this.StartState(this.CirclingState());
    this.health.OnDamaged += new Health.HealthEvent(this.OnDamaged);
    this.health.OnCharmed += new Health.StasisEvent(this.OnCharmed);
    this.health.OnPoisoned += new Health.StasisEvent(this.OnStatusWake);
    this.health.OnFreezeTimeCleared += new Health.StasisEvent(this.OnStatusWake);
    this.health.OnFreezeTime += new Health.StasisEvent(this.OnStatusWake);
    this.health.OnCharmed += new Health.StasisEvent(this.OnCharmed);
    this.health.OnStasisCleared += new Health.StasisEvent(this.OnStatusWake);
  }

  public void OnStatusWake() => this.WasAlerted = true;

  public void OnCharmed()
  {
    this.WasAlerted = true;
    foreach (UnitObject dog in EnemyDogPack.Dogs)
      dog.health.AddCharm();
  }

  public void OnDamaged(
    GameObject attacker,
    Vector3 attackLocation,
    float damage,
    Health.AttackTypes attackType,
    Health.AttackFlags attackFlag)
  {
    this.WasAlerted = true;
  }

  public IEnumerator SleepRoutineState()
  {
    EnemyDogPack enemyDogPack = this;
    enemyDogPack.isAsleep = true;
    enemyDogPack.IdleWait = 0.0f;
    enemyDogPack.IsAttacking = false;
    enemyDogPack.health.invincible = false;
    enemyDogPack.UsePathing = false;
    enemyDogPack.SleepUI.gameObject.SetActive(true);
    enemyDogPack.AwarenessUI.gameObject.SetActive(true);
    if (!string.IsNullOrEmpty(enemyDogPack.SleepAnimation))
      enemyDogPack.Spine.AnimationState.SetAnimation(0, enemyDogPack.SleepAnimation, true);
    if (!string.IsNullOrEmpty(enemyDogPack.SnoreLoopSFX))
      enemyDogPack.snoreLoopInstanceSFX = AudioManager.Instance.CreateLoop(enemyDogPack.SnoreLoopSFX, enemyDogPack.gameObject, true);
    enemyDogPack.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) new WaitForSeconds(0.25f);
    int playerLineOfSightCount = 0;
    bool closedByWakingDogs = false;
    bool soundUpdated = false;
    enemyDogPack.oldPosition = (UnityEngine.Object) enemyDogPack.circlingPlayer != (UnityEngine.Object) null ? enemyDogPack.circlingPlayer.transform.position : enemyDogPack.transform.position;
    while (enemyDogPack.health.Dormant)
    {
      if (enemyDogPack.health.IsIced)
        enemyDogPack.currentPlayerVolumeMultiplier = 0.0f;
      enemyDogPack.currentPlayerVolumeMultiplier -= Time.deltaTime * 0.5f;
      enemyDogPack.currentPlayerVolumeMultiplier = Mathf.Clamp(enemyDogPack.currentPlayerVolumeMultiplier, 0.5f, 2.5f);
      float num1 = enemyDogPack.WakeRange * 0.9f * enemyDogPack.currentPlayerVolumeMultiplier;
      enemyDogPack.AwarenessUI.transform.localScale = Vector3.one * num1;
      enemyDogPack.AwarenessUI.transform.Rotate(0.0f, 0.0f, 0.5f);
      enemyDogPack.circlingPlayer = enemyDogPack.GetClosestPlayerTeamUnit();
      if ((UnityEngine.Object) enemyDogPack.circlingPlayer != (UnityEngine.Object) null && !soundUpdated)
      {
        float num2 = 0.0f;
        float num3;
        if ((UnityEngine.Object) enemyDogPack.circlingPlayer.playerFarming != (UnityEngine.Object) null && !enemyDogPack.circlingPlayer.playerFarming.GoToAndStopping)
        {
          if (enemyDogPack.circlingPlayer.playerFarming.playerWeapon.CurrentAttackState == PlayerWeapon.AttackState.Begin)
            num2 += 0.02f;
          if (enemyDogPack.circlingPlayer.state.CURRENT_STATE == StateMachine.State.Dodging)
            num2 += 0.02f;
          float num4 = Mathf.Clamp(Vector3.Distance(enemyDogPack.oldPosition, enemyDogPack.circlingPlayer.transform.position) / Time.deltaTime, 0.0f, 2.5f);
          num3 = num2 + num4 * 0.01f;
          enemyDogPack.oldPosition = enemyDogPack.circlingPlayer.transform.position;
        }
        else
        {
          float num5 = Mathf.Clamp(Vector3.Distance(enemyDogPack.oldPosition, enemyDogPack.circlingPlayer.transform.position) / Time.deltaTime, 0.0f, 1.5f);
          num3 = num2 + num5 * 0.01f;
          enemyDogPack.oldPosition = enemyDogPack.circlingPlayer.transform.position;
        }
        if ((double) num3 > 0.0)
        {
          soundUpdated = true;
          if ((double) Time.fixedTime > (double) enemyDogPack.lastSilentTime + 0.20000000298023224)
            enemyDogPack.currentPlayerVolumeMultiplier += num3;
        }
        else
          enemyDogPack.lastSilentTime = Time.fixedTime;
        if (!enemyDogPack.health.IsIced && (enemyDogPack.WasAlerted || (double) Vector3.Distance(enemyDogPack.transform.position, enemyDogPack.circlingPlayer.transform.position) < (double) enemyDogPack.WakeRange * (double) enemyDogPack.currentPlayerVolumeMultiplier) && GameManager.RoomActive)
        {
          if (RoomLockController.DoorsOpen)
            RoomLockController.CloseAll();
          if ((bool) (UnityEngine.Object) DormantEnemyChecker.Instance)
            DormantEnemyChecker.Instance.nextCheck = Time.time + 0.1f;
          closedByWakingDogs = true;
          bool flag = true;
          foreach (PlayerFarming player in PlayerFarming.players)
          {
            Vector2 position1 = (Vector2) enemyDogPack.transform.position;
            Vector2 position2 = (Vector2) player.transform.position;
            Vector2 normalized = (position2 - position1).normalized;
            float distance = Vector2.Distance(position1, position2);
            if ((UnityEngine.Object) Physics2D.Raycast(position1, normalized, distance, LayerMask.GetMask("Island", "Obstacles")).collider != (UnityEngine.Object) null)
            {
              flag = false;
              break;
            }
          }
          if (flag)
          {
            ++playerLineOfSightCount;
            if (playerLineOfSightCount > 0)
            {
              enemyDogPack.SleepUI.gameObject.SetActive(false);
              enemyDogPack.AwarenessUI.gameObject.SetActive(false);
              enemyDogPack.WakeUI.gameObject.SetActive(true);
              AudioManager.Instance.StopLoop(enemyDogPack.snoreLoopInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
              Vector3 localScale = enemyDogPack.WakeUI.transform.localScale;
              enemyDogPack.WakeUI.transform.DOPunchScale(localScale * 0.2f, 1.25f, elasticity: 0.5f).OnComplete<Tweener>(new TweenCallback(enemyDogPack.\u003CSleepRoutineState\u003Eb__70_0));
              enemyDogPack.health.Dormant = false;
              if (!string.IsNullOrEmpty(enemyDogPack.WakeUpAnimation))
              {
                Spine.Animation animation = enemyDogPack.Spine.AnimationState.Data.SkeletonData.FindAnimation(enemyDogPack.WakeUpAnimation);
                if (!string.IsNullOrEmpty(enemyDogPack.WakeUpSFX))
                  AudioManager.Instance.PlayOneShot(enemyDogPack.WakeUpSFX, enemyDogPack.transform.position);
                if (!string.IsNullOrEmpty(enemyDogPack.AttackVO))
                  AudioManager.Instance.PlayOneShot(enemyDogPack.AttackVO, enemyDogPack.transform.position);
                if (animation != null)
                {
                  enemyDogPack.Spine.AnimationState.SetAnimation(0, animation, false);
                  yield return (object) new WaitForSeconds(animation.Duration);
                }
                else
                  yield return (object) new WaitForSeconds(1.5f);
              }
              else
                yield return (object) new WaitForSeconds(1.5f);
              enemyDogPack.health.DamageModifier = enemyDogPack.originalDamageModifier;
              if (enemyDogPack.HowlToAlertDogsOnWake && EnemyDogPack.Dogs.Count > 1)
              {
                if (!enemyDogPack.WasAlerted)
                {
                  yield return (object) enemyDogPack.StartCoroutine((IEnumerator) enemyDogPack.DoHowlRoutine());
                }
                else
                {
                  for (int index = 0; index < EnemyDogPack.Dogs.Count; ++index)
                  {
                    if ((UnityEngine.Object) EnemyDogPack.Dogs[index] != (UnityEngine.Object) enemyDogPack)
                      EnemyDogPack.Dogs[index].WasAlerted = true;
                  }
                }
              }
            }
          }
          else
          {
            foreach (EnemyDogPack dog in EnemyDogPack.Dogs)
              dog.currentPlayerVolumeMultiplier = 0.0f;
          }
        }
        else
        {
          playerLineOfSightCount = 0;
          if (closedByWakingDogs)
          {
            closedByWakingDogs = false;
            if (!RoomLockController.DoorsOpen)
              RoomLockController.OpenAll();
          }
        }
      }
      soundUpdated = false;
      yield return (object) new WaitForFixedUpdate();
    }
    enemyDogPack.WakeUI.gameObject.SetActive(false);
    enemyDogPack.SleepUI.gameObject.SetActive(false);
    enemyDogPack.AwarenessUI.gameObject.SetActive(false);
    AudioManager.Instance.StopLoop(enemyDogPack.snoreLoopInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    enemyDogPack.StartState(enemyDogPack.CirclingState());
  }

  public override void OnDisable()
  {
    base.OnDisable();
    EnemyDogPack.Dogs.Remove(this);
    this.PackCircleSpeed *= -1f;
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.SetActive(false);
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
    this.ClearPaths();
    this.StopAllCoroutines();
    this.DisableForces = false;
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashWhite(false);
    AudioManager.Instance.StopLoop(this.snoreLoopInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    this.health.OnDamaged -= new Health.HealthEvent(this.OnDamaged);
    this.health.OnCharmed -= new Health.StasisEvent(this.OnCharmed);
    this.health.OnPoisoned -= new Health.StasisEvent(this.OnStatusWake);
    this.health.OnIced -= new Health.StasisEvent(this.OnStatusWake);
    this.health.OnFreezeTime -= new Health.StasisEvent(this.OnStatusWake);
    this.health.OnCharmed -= new Health.StasisEvent(this.OnCharmed);
    this.health.OnStasisCleared -= new Health.StasisEvent(this.OnStatusWake);
  }

  public void ShowWarningIcon(float duration = 2f)
  {
    if ((UnityEngine.Object) this.warningIcon == (UnityEngine.Object) null || this.ShownWarning)
      return;
    this.warningIcon.AnimationState.SetAnimation(0, "warn-start", false);
    this.warningIcon.AnimationState.AddAnimation(0, "warn-stop", false, duration);
    this.ShownWarning = true;
    if (string.IsNullOrEmpty(this.WarningVO))
      return;
    AudioManager.Instance.PlayOneShot(this.WarningVO, this.transform.position);
  }

  public void InitializePackAngle()
  {
    if (EnemyDogPack.Dogs.Count <= 0)
      return;
    this.PackAngle = 0.0f;
  }

  public IEnumerator CirclingState()
  {
    EnemyDogPack enemyDogPack = this;
    enemyDogPack.hasBegunCircle = true;
    enemyDogPack.state.CURRENT_STATE = StateMachine.State.Moving;
    enemyDogPack.Spine.AnimationState.SetAnimation(0, enemyDogPack.MovingAnimation, true);
    enemyDogPack.UsePathing = false;
    enemyDogPack.speed = 0.0f;
    enemyDogPack.maxSpeed = 0.0f;
    enemyDogPack.health.invincible = false;
    enemyDogPack.isCircling = true;
    yield return (object) new WaitForEndOfFrame();
    while (true)
    {
      while ((double) enemyDogPack.Spine.timeScale != 9.9999997473787516E-05)
      {
        if (enemyDogPack.health.IsCharmed && (UnityEngine.Object) enemyDogPack.circlingTarget != (UnityEngine.Object) null && enemyDogPack.circlingTarget.isPlayer && !enemyDogPack.IsTargetAlive(enemyDogPack.circlingTarget))
          enemyDogPack.circlingTarget = (Health) null;
        else if (!enemyDogPack.health.IsCharmed && (UnityEngine.Object) enemyDogPack.circlingTarget != (UnityEngine.Object) null && !enemyDogPack.circlingTarget.isPlayer || !enemyDogPack.IsTargetAlive(enemyDogPack.circlingTarget))
          enemyDogPack.circlingTarget = (Health) null;
        if ((UnityEngine.Object) enemyDogPack.circlingTarget == (UnityEngine.Object) null)
        {
          if ((UnityEngine.Object) enemyDogPack.circlingPlayer != (UnityEngine.Object) null)
            enemyDogPack.circlingTarget = enemyDogPack.IsTargetAlive(enemyDogPack.circlingPlayer) ? enemyDogPack.circlingPlayer : enemyDogPack.GetClosestPlayerTeamUnit();
          if (enemyDogPack.health.IsCharmed)
          {
            float num1 = float.MaxValue;
            Health health = (Health) null;
            foreach (Health allUnit in Health.allUnits)
            {
              if (!((UnityEngine.Object) allUnit == (UnityEngine.Object) null) && !((UnityEngine.Object) allUnit.transform == (UnityEngine.Object) null) && allUnit.team == Health.Team.Team2 && !allUnit.IsCharmed && enemyDogPack.IsTargetAlive(allUnit))
              {
                float num2 = Vector3.Distance(allUnit.transform.position, enemyDogPack.transform.position);
                if ((double) num2 < (double) num1)
                {
                  num1 = num2;
                  health = allUnit;
                }
              }
            }
            if ((UnityEngine.Object) health != (UnityEngine.Object) null)
              enemyDogPack.circlingTarget = health;
          }
        }
        enemyDogPack.state.LookAngle = Utils.GetAngle(enemyDogPack.transform.position, enemyDogPack.circlingTarget.transform.position);
        enemyDogPack.state.facingAngle = enemyDogPack.state.LookAngle;
        Vector3 b = enemyDogPack.TargetPosition;
        RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) enemyDogPack.circlingTarget.transform.position, (Vector2) (b - enemyDogPack.circlingTarget.transform.position), enemyDogPack.CircleRadius, (int) enemyDogPack.borderMask);
        if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
          b = (Vector3) raycastHit2D.point;
        if ((double) enemyDogPack.circleLerpAcceleration < 0.5)
          enemyDogPack.circleLerpAcceleration += Time.deltaTime * 0.25f;
        float num = 5f * enemyDogPack.circleLerpAcceleration;
        enemyDogPack.transform.position = Vector3.Lerp(enemyDogPack.transform.position, b, num * Time.deltaTime);
        yield return (object) null;
      }
      yield return (object) null;
    }
  }

  public IEnumerator DoHowlRoutine()
  {
    EnemyDogPack enemyDogPack = this;
    enemyDogPack.Spine.AnimationState.SetAnimation(0, enemyDogPack.HowlAnimation, false);
    enemyDogPack.Spine.AnimationState.AddAnimation(0, enemyDogPack.IdleAnimation, true, 0.0f);
    if (!string.IsNullOrEmpty(enemyDogPack.HowlVO))
      AudioManager.Instance.PlayOneShot(enemyDogPack.HowlVO, enemyDogPack.transform.position);
    for (int index = 0; index < EnemyDogPack.Dogs.Count; ++index)
    {
      if (EnemyDogPack.Dogs[index].health.Dormant)
        EnemyDogPack.Dogs[index].WasAlerted = true;
    }
    yield return (object) new WaitForSeconds(1.5f);
  }

  public virtual IEnumerator MeleeAttackState()
  {
    EnemyDogPack enemyDogPack = this;
    enemyDogPack.circleLerpAcceleration = 0.1f;
    enemyDogPack.circlingPlayer = enemyDogPack.GetClosestPlayerTeamUnit();
    if ((UnityEngine.Object) enemyDogPack.circlingTarget == (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) enemyDogPack.circlingPlayer != (UnityEngine.Object) null)
        enemyDogPack.circlingTarget = enemyDogPack.circlingPlayer;
      if (enemyDogPack.health.IsCharmed)
      {
        Health closestTarget = UnitObject.GetClosestTarget(enemyDogPack.transform, Health.Team.Team2);
        if ((UnityEngine.Object) closestTarget != (UnityEngine.Object) null && (UnityEngine.Object) closestTarget.transform != (UnityEngine.Object) null && !closestTarget.IsCharmed)
          enemyDogPack.circlingTarget = closestTarget;
      }
    }
    if ((UnityEngine.Object) enemyDogPack.circlingTarget == (UnityEngine.Object) null)
    {
      enemyDogPack.StartState(enemyDogPack.CirclingState());
    }
    else
    {
      enemyDogPack.UsePathing = false;
      enemyDogPack.isAttacking = true;
      enemyDogPack.Spine.AnimationState.SetAnimation(0, enemyDogPack.SignPostAttackAnimation, enemyDogPack.LoopSignPostAttackAnimation);
      enemyDogPack.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
      if (!string.IsNullOrEmpty(enemyDogPack.WarningVO))
        AudioManager.Instance.PlayOneShot(enemyDogPack.WarningVO, enemyDogPack.transform.position);
      if (!string.IsNullOrEmpty(enemyDogPack.AttackBasicWindupSFX))
        AudioManager.Instance.PlayOneShot(enemyDogPack.AttackBasicWindupSFX, enemyDogPack.transform.position);
      enemyDogPack.state.LookAngle = Utils.GetAngle(enemyDogPack.transform.position, enemyDogPack.circlingTarget.transform.position);
      enemyDogPack.state.facingAngle = enemyDogPack.state.LookAngle;
      yield return (object) DOTween.To((DOGetter<float>) (() => 0.0f), new DOSetter<float>(enemyDogPack.\u003CMeleeAttackState\u003Eb__76_1), 1f, enemyDogPack.SignPostAttackDuration / enemyDogPack.Spine.timeScale).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear).WaitForCompletion();
      yield return (object) new WaitForSeconds(0.05f);
      foreach (SimpleSpineFlash simpleSpineFlash in enemyDogPack.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(0.0f);
      yield return (object) enemyDogPack.StartCoroutine((IEnumerator) enemyDogPack.PounceAttack(enemyDogPack.circlingTarget));
      yield return (object) new WaitForSeconds(0.5f);
      enemyDogPack.StartState(enemyDogPack.CirclingState());
    }
  }

  public IEnumerator PounceAttack(Health closestHealth)
  {
    EnemyDogPack enemyDogPack = this;
    if (!((UnityEngine.Object) closestHealth == (UnityEngine.Object) null) && enemyDogPack.IsTargetAlive(closestHealth))
    {
      Vector3 startPosition = enemyDogPack.transform.position;
      Vector3 targetPosition = closestHealth.transform.position;
      yield return (object) new WaitForSeconds(0.5f);
      Vector3 normalized = (targetPosition - startPosition).normalized;
      Vector3 vector3 = targetPosition + normalized * 2.5f;
      float duration = 0.6f;
      RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) startPosition, (Vector2) normalized, Vector3.Distance(startPosition, vector3), (int) enemyDogPack.borderMask);
      if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
        vector3 = (Vector3) raycastHit2D.point - normalized * 0.2f;
      if (!string.IsNullOrEmpty(enemyDogPack.AttackVO))
        AudioManager.Instance.PlayOneShot(enemyDogPack.AttackVO, enemyDogPack.transform.position);
      if (!string.IsNullOrEmpty(enemyDogPack.AttackBasicBiteSFX))
        AudioManager.Instance.PlayOneShot(enemyDogPack.AttackBasicBiteSFX, enemyDogPack.transform.position);
      enemyDogPack.Spine.AnimationState.SetAnimation(0, enemyDogPack.AttackAnimation, false);
      enemyDogPack.damageColliderEvents.SetActive(true);
      enemyDogPack.state.LookAngle = Utils.GetAngle(enemyDogPack.transform.position, vector3);
      enemyDogPack.state.facingAngle = enemyDogPack.state.LookAngle;
      yield return (object) enemyDogPack.transform.DOMove(vector3, duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad).WaitForCompletion();
      enemyDogPack.damageColliderEvents.SetActive(false);
    }
  }

  public void PickNewAttackingDog()
  {
    if ((double) this.lastDogAttack >= (double) Time.time - 5.0 || EnemyDogPack.Dogs.Count == 0)
      return;
    int index = UnityEngine.Random.Range(0, EnemyDogPack.Dogs.Count);
    EnemyDogPack dog = EnemyDogPack.Dogs[index];
    if (!((UnityEngine.Object) dog != (UnityEngine.Object) null) || !dog.isCircling || dog.health.IsCharmed && (!((UnityEngine.Object) this.circlingTarget != (UnityEngine.Object) null) || this.circlingTarget.isPlayer))
      return;
    dog.StartState(dog.MeleeAttackState());
    this.lastDogAttack = Time.time;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
    AudioManager.Instance.StopLoop(this.snoreLoopInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
    {
      DOTween.Kill((object) simpleSpineFlash.transform);
      simpleSpineFlash.FlashWhite(0.0f);
    }
    for (int index = 0; index < EnemyDogPack.Dogs.Count; ++index)
    {
      if (EnemyDogPack.Dogs[index].health.Dormant)
        EnemyDogPack.Dogs[index].WasAlerted = true;
    }
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
    this.damageColliderEvents.SetActive(false);
    if (AttackType == Health.AttackTypes.NoReaction)
      return;
    this.DisableForces = false;
    float num = (double) this.health.totalHP > 0.0 ? this.health.HP / this.health.totalHP : 0.0f;
    if ((!this.EnableBackStun || string.IsNullOrEmpty(this.BackStunnedAnimation) || string.IsNullOrEmpty(this.BackStunnedResetAnimation) || (double) num >= (double) this.BackStunHealthThreshold ? 0 : (this.hasBegunCircle ? 1 : 0)) != 0)
    {
      this.BackStunHealthThreshold = 0.0f;
      this.StartCoroutine((IEnumerator) this.ApplyBackstunForceRoutine(Utils.GetAngle(Attacker.transform.position, this.transform.position) * ((float) Math.PI / 180f), 25f));
      this.StartState(this.BackStunnedRoutine());
      foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      {
        simpleSpineFlash.FlashWhite(0.0f);
        simpleSpineFlash.FlashRed(0.0f);
      }
    }
    else
    {
      this.StartState(this.HurtRoutine());
      foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
        simpleSpineFlash.FlashFillRed();
    }
  }

  public IEnumerator ApplyBackstunForceRoutine(float angle, float localMultiplier = 1f)
  {
    EnemyDogPack enemyDogPack = this;
    enemyDogPack.DisableForces = true;
    float timer = 0.0f;
    Vector2 Force = new Vector2(1500f * Mathf.Cos(angle), 1500f * Mathf.Sin(angle)) * localMultiplier;
    while ((double) timer < 0.25)
    {
      enemyDogPack.rb.AddForce(Force * Time.deltaTime);
      timer += Time.deltaTime;
      yield return (object) null;
    }
    enemyDogPack.DisableForces = false;
  }

  public IEnumerator HurtRoutine()
  {
    EnemyDogPack enemyDogPack = this;
    enemyDogPack.health.invincible = true;
    enemyDogPack.damageColliderEvents.SetActive(false);
    enemyDogPack.IsAttacking = false;
    enemyDogPack.ClearPaths();
    enemyDogPack.state.CURRENT_STATE = StateMachine.State.KnockBack;
    enemyDogPack.Spine.AnimationState.SetAnimation(0, enemyDogPack.IdleAnimation, true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyDogPack.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyDogPack.DisableForces = false;
    enemyDogPack.IdleWait = 0.0f;
    enemyDogPack.health.invincible = false;
    enemyDogPack.StartState(enemyDogPack.CirclingState());
  }

  public IEnumerator BackStunnedRoutine()
  {
    EnemyDogPack enemyDogPack = this;
    enemyDogPack.isAttacking = false;
    enemyDogPack.ClearPaths();
    enemyDogPack.DisableForces = false;
    foreach (SimpleSpineFlash simpleSpineFlash in enemyDogPack.SimpleSpineFlashes)
      simpleSpineFlash.FlashWhite(0.0f);
    enemyDogPack.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    if (!string.IsNullOrEmpty(enemyDogPack.BackStunnedAnimation) && !string.IsNullOrEmpty(enemyDogPack.BackStunnedResetAnimation))
    {
      enemyDogPack.Spine.AnimationState.SetAnimation(0, enemyDogPack.BackStunnedAnimation, false);
      AudioManager.Instance.PlayOneShot("event:/dlc/dungeon05/enemy/dog_pack/mv_knockdown_start", enemyDogPack.gameObject);
      float t = 0.0f;
      float stunTime = enemyDogPack.BackStunnedDuration - UnityEngine.Random.value;
      while ((double) (t += Time.deltaTime * enemyDogPack.Spine.timeScale) < (double) stunTime)
        yield return (object) null;
      enemyDogPack.Spine.AnimationState.SetAnimation(0, enemyDogPack.BackStunnedResetAnimation, false);
      enemyDogPack.Spine.AnimationState.AddAnimation(0, enemyDogPack.IdleAnimation, true, 0.0f);
      AudioManager.Instance.PlayOneShot("event:/dlc/dungeon05/enemy/dog_pack/mv_knockdown_getup", enemyDogPack.gameObject);
      Spine.Animation animation = enemyDogPack.Spine.Skeleton.Data.FindAnimation(enemyDogPack.BackStunnedResetAnimation);
      if (animation != null)
        yield return (object) new WaitForSeconds(animation.Duration * enemyDogPack.Spine.timeScale);
    }
    enemyDogPack.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyDogPack.StartState(enemyDogPack.CirclingState());
  }

  public bool IsTargetAlive(Health target)
  {
    if ((UnityEngine.Object) target == (UnityEngine.Object) null)
      return false;
    bool flag = true;
    if ((UnityEngine.Object) target.playerFarming != (UnityEngine.Object) null)
      flag = !target.playerFarming.IsKnockedOut;
    if ((double) target.HP <= 0.0)
      flag = false;
    return flag;
  }

  public virtual void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public Health GetClosestPlayerTeamUnit()
  {
    Health closestPlayerTeamUnit = (Health) null;
    float num1 = float.MaxValue;
    foreach (Health allUnit in Health.allUnits)
    {
      if (!((UnityEngine.Object) allUnit == (UnityEngine.Object) null) && !((UnityEngine.Object) allUnit.transform == (UnityEngine.Object) null) && allUnit.team == Health.Team.PlayerTeam && this.IsTargetAlive(allUnit))
      {
        float num2 = Vector3.Distance(this.transform.position, allUnit.transform.position);
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          closestPlayerTeamUnit = allUnit;
        }
      }
    }
    return closestPlayerTeamUnit;
  }

  [CompilerGenerated]
  public void \u003CSleepRoutineState\u003Eb__70_0()
  {
    this.WakeUI.transform.DOScale(Vector3.zero, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.WakeUI.gameObject.SetActive(false)));
  }

  [CompilerGenerated]
  public void \u003CSleepRoutineState\u003Eb__70_1() => this.WakeUI.gameObject.SetActive(false);

  [CompilerGenerated]
  public void \u003CMeleeAttackState\u003Eb__76_1(float progress)
  {
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashWhite(progress / this.SignPostAttackDuration);
  }
}
