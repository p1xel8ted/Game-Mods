// Decompiled with JetBrains decompiler
// Type: GuardianPet_BeamAttack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using MMBiomeGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class GuardianPet_BeamAttack : GuardianPet
{
  public GameObject PetGameObject;
  public AssetReferenceGameObject indicatorPrefab;
  [SerializeField]
  public Vector2 attacks;
  [SerializeField]
  public float timeBetween;
  [SerializeField]
  public GameObject lava;
  public float acceleration = 2f;
  public float turningSpeed = 1f;
  public float angleNoiseAmplitude;
  public float angleNoiseFrequency;
  public float timestamp;
  public float IdleSpeed = 0.03f;
  public float ChaseSpeed = 0.1f;
  public float MaximumRange = 10f;
  public float BeamAttackDuration = 5f;
  public float BeamTrackingSpeed = 5f;
  [SerializeField]
  public bool useAcceleration;
  public float Angle;
  public bool ChasingPlayer;
  public bool avoidTarget;
  public float noticePlayerDistance = 5f;
  public bool NoticedPlayer;
  public Vector3? StartingPosition;
  public Vector3 TargetPosition;
  public float AttackCoolDown;
  public Vector2 AttackCoolDownDuration = new Vector2(1f, 2f);
  public int RanDirection = 1;
  public int NumberOfAttacks = 3;
  [SerializeField]
  public DamageCollider collider;
  public string DetachSFX = "event:/dlc/dungeon06/enemy/miniboss_petguardian/attack_eye_detach";
  public string AttackWindupSFX = "event:/dlc/dungeon06/enemy/miniboss_petguardian/attack_eye_windup";
  public string AttackBeamShootSFX = "event:/dlc/dungeon06/enemy/miniboss_petguardian/attack_eye_beam_shoot";
  public string AttackBeamImpactSFX = "event:/dlc/dungeon06/enemy/miniboss_petguardian/attack_eye_beam_impact";
  public string DescendSFX = "event:/dlc/dungeon06/enemy/miniboss_petguardian/attack_eye_descend";
  public EventInstance attackWindupInstanceSFX;
  public GameObject loadedIndicator;
  public static List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public int KnockBackForce = 750;
  public int AttackCycle;
  [SerializeField]
  public GameObject ChildContainer;
  public GameObject currentIndicator;

  public override void Awake()
  {
    base.Awake();
    this.health = this.GetComponent<Health>();
    this.collider.gameObject.SetActive(false);
    this.LoadAssets();
  }

  public void LoadAssets()
  {
    Addressables.LoadAssetAsync<GameObject>((object) this.indicatorPrefab).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      GuardianPet_BeamAttack.loadedAddressableAssets.Add(obj);
      this.loadedIndicator = obj.Result;
      this.loadedIndicator.CreatePool(5, true);
    });
  }

  public override void LaunchPet(GuardianPetController TargetObject)
  {
    if (!string.IsNullOrEmpty(this.DetachSFX))
      AudioManager.Instance.PlayOneShot(this.DetachSFX, this.transform.position);
    this.ParentPetController = TargetObject;
    this.transform.eulerAngles = Vector3.zero;
    this.transform.localScale = Vector3.one * 1.5f;
    this.transform.parent = BiomeGenerator.Instance.CurrentRoom.generateRoom.transform;
    this.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.transform.DOMoveZ(-3f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    foreach (Behaviour behaviour in this.BehavioursToActivate)
      behaviour.enabled = true;
    this.Play();
    this.spawnVfx.gameObject.SetActive(true);
  }

  public override void Play()
  {
    base.Play();
    Debug.Log((object) "PLAY!".Colour(Color.yellow));
    this.timestamp = !((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null) ? Time.time : GameManager.GetInstance().CurrentTime;
    this.PetGameObject = this.gameObject;
    this.turningSpeed += UnityEngine.Random.Range(-0.1f, 0.1f);
    this.angleNoiseFrequency += UnityEngine.Random.Range(-0.1f, 0.1f);
    this.angleNoiseAmplitude += UnityEngine.Random.Range(-0.1f, 0.1f);
    this.RanDirection = (double) UnityEngine.Random.value < 0.5 ? -1 : 1;
    if ((UnityEngine.Object) this.health == (UnityEngine.Object) null)
      this.health = this.GetComponent<Health>();
    this.health.HP = this.health.totalHP;
    this.health.enabled = true;
    this.AttackCoolDown = UnityEngine.Random.Range(this.AttackCoolDownDuration.x, this.AttackCoolDownDuration.y);
    this.StartCoroutine((IEnumerator) this.ActiveRoutine());
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.StartCoroutine((IEnumerator) this.KnockBackRoutine(Utils.GetAngle(Attacker.transform.position, this.transform.position) * ((float) Math.PI / 180f), (System.Action) (() =>
    {
      this.AttackCoolDown = UnityEngine.Random.Range(this.AttackCoolDownDuration.x, this.AttackCoolDownDuration.y);
      this.StartCoroutine((IEnumerator) this.ActiveRoutine());
    })));
  }

  public IEnumerator KnockBackRoutine(float angle, System.Action Callback)
  {
    GuardianPet_BeamAttack guardianPetBeamAttack = this;
    guardianPetBeamAttack.DisableForces = true;
    Vector3 force = (Vector3) new Vector2((float) guardianPetBeamAttack.KnockBackForce * Mathf.Cos(angle), (float) guardianPetBeamAttack.KnockBackForce * Mathf.Sin(angle));
    guardianPetBeamAttack.rb.AddForce((Vector2) force);
    yield return (object) new WaitForSeconds(0.5f);
    guardianPetBeamAttack.DisableForces = false;
    guardianPetBeamAttack.rb.velocity = Vector2.zero;
    guardianPetBeamAttack.AttackCoolDown = 0.0f;
    System.Action action = Callback;
    if (action != null)
      action();
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    Debug.Log((object) "DIE!".Colour(Color.yellow));
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
  }

  public override void OnDestroy()
  {
    AudioManager.Instance.StopOneShotInstanceEarly(this.attackWindupInstanceSFX, STOP_MODE.ALLOWFADEOUT);
    base.OnDestroy();
    if (GuardianPet_BeamAttack.loadedAddressableAssets == null)
      return;
    foreach (AsyncOperationHandle<GameObject> addressableAsset in GuardianPet_BeamAttack.loadedAddressableAssets)
      Addressables.Release((AsyncOperationHandle) addressableAsset);
    GuardianPet_BeamAttack.loadedAddressableAssets.Clear();
  }

  public override void OnDisable()
  {
    if (!((UnityEngine.Object) this.currentIndicator != (UnityEngine.Object) null))
      return;
    this.currentIndicator.Recycle();
  }

  public Health GetClosestTarget() => this.GetClosestTarget(true);

  public virtual IEnumerator ActiveRoutine()
  {
    GuardianPet_BeamAttack guardianPetBeamAttack1 = this;
    guardianPetBeamAttack1.maxSpeed = guardianPetBeamAttack1.ChaseSpeed;
    yield return (object) new WaitForEndOfFrame();
    while (true)
    {
      float turningSpeed = guardianPetBeamAttack1.turningSpeed;
      float num1;
      if ((UnityEngine.Object) guardianPetBeamAttack1.GetClosestTarget() == (UnityEngine.Object) null || (double) Vector3.Distance(guardianPetBeamAttack1.transform.position, guardianPetBeamAttack1.GetClosestTarget().transform.position) > 12.0)
      {
        if (guardianPetBeamAttack1.StartingPosition.HasValue)
          guardianPetBeamAttack1.TargetPosition = guardianPetBeamAttack1.StartingPosition.Value;
        guardianPetBeamAttack1.maxSpeed = guardianPetBeamAttack1.IdleSpeed;
        guardianPetBeamAttack1.ChasingPlayer = false;
      }
      else
      {
        if (guardianPetBeamAttack1.avoidTarget)
        {
          guardianPetBeamAttack1.TargetPosition = -guardianPetBeamAttack1.GetClosestTarget().transform.position;
          int num2 = 0;
          while (num2 < 10 && (double) Vector3.Magnitude(guardianPetBeamAttack1.TargetPosition - guardianPetBeamAttack1.transform.position) < 3.0)
          {
            num1 = Vector3.Magnitude(guardianPetBeamAttack1.TargetPosition - guardianPetBeamAttack1.transform.position);
            Debug.Log((object) $"Dist {num1.ToString()} {num2.ToString()}");
            ++num2;
            guardianPetBeamAttack1.TargetPosition *= 3f;
          }
        }
        else
          guardianPetBeamAttack1.TargetPosition = guardianPetBeamAttack1.GetClosestTarget().transform.position;
        if ((UnityEngine.Object) guardianPetBeamAttack1.state != (UnityEngine.Object) null && (UnityEngine.Object) guardianPetBeamAttack1.GetClosestTarget() != (UnityEngine.Object) null)
          guardianPetBeamAttack1.state.LookAngle = Utils.GetAngle(guardianPetBeamAttack1.transform.position, guardianPetBeamAttack1.GetClosestTarget().transform.position);
      }
      guardianPetBeamAttack1.AttackCoolDown = num1 = guardianPetBeamAttack1.AttackCoolDown - Time.deltaTime;
      if ((double) num1 >= 0.0)
      {
        guardianPetBeamAttack1.Angle = Mathf.LerpAngle(guardianPetBeamAttack1.Angle, Utils.GetAngle(guardianPetBeamAttack1.transform.position, guardianPetBeamAttack1.TargetPosition), Time.deltaTime * turningSpeed);
        if ((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null && (double) guardianPetBeamAttack1.angleNoiseAmplitude > 0.0 && (double) guardianPetBeamAttack1.angleNoiseFrequency > 0.0 && (double) Vector3.Distance(guardianPetBeamAttack1.TargetPosition, guardianPetBeamAttack1.transform.position) < (double) guardianPetBeamAttack1.MaximumRange)
          guardianPetBeamAttack1.Angle += (Mathf.PerlinNoise(GameManager.GetInstance().TimeSince(guardianPetBeamAttack1.timestamp) * guardianPetBeamAttack1.angleNoiseFrequency, 0.0f) - 0.5f) * guardianPetBeamAttack1.angleNoiseAmplitude * (float) guardianPetBeamAttack1.RanDirection;
        if (!guardianPetBeamAttack1.useAcceleration)
          guardianPetBeamAttack1.speed = guardianPetBeamAttack1.maxSpeed * guardianPetBeamAttack1.SpeedMultiplier;
        guardianPetBeamAttack1.state.facingAngle = guardianPetBeamAttack1.Angle;
        yield return (object) null;
      }
      else
        break;
    }
    guardianPetBeamAttack1.StopAllCoroutines();
    if ((UnityEngine.Object) guardianPetBeamAttack1.currentIndicator != (UnityEngine.Object) null)
    {
      guardianPetBeamAttack1.currentIndicator.Recycle();
      guardianPetBeamAttack1.currentIndicator = (GameObject) null;
    }
    yield return (object) guardianPetBeamAttack1.StartCoroutine((IEnumerator) guardianPetBeamAttack1.BeamAttackRoutine());
    GuardianPet_BeamAttack guardianPetBeamAttack2 = guardianPetBeamAttack1;
    int num3 = guardianPetBeamAttack1.AttackCycle + 1;
    int num4 = num3;
    guardianPetBeamAttack2.AttackCycle = num4;
    if (num3 > 1)
      guardianPetBeamAttack1.AttackCycle = 0;
  }

  public virtual IEnumerator FleeRoutine()
  {
    yield return (object) null;
  }

  public virtual bool ShouldStartCharging() => false;

  public virtual IEnumerator ChargingRoutine()
  {
    yield return (object) null;
  }

  public virtual bool ShouldAttack() => false;

  public void BeamAttack()
  {
  }

  public IEnumerator BeamAttackRoutine()
  {
    GuardianPet_BeamAttack guardianPetBeamAttack = this;
    guardianPetBeamAttack.DisableForces = false;
    guardianPetBeamAttack.rb.velocity = Vector2.zero;
    double beamAttackDuration = (double) guardianPetBeamAttack.BeamAttackDuration;
    guardianPetBeamAttack.state.CURRENT_STATE = StateMachine.State.Attacking;
    Health currentTarget = guardianPetBeamAttack.GetClosestTarget();
    guardianPetBeamAttack.spine.AnimationState.SetAnimation(0, "attack-beam-charge", false);
    guardianPetBeamAttack.spine.AnimationState.AddAnimation(0, "attack-beam-loop", true, 0.0f);
    if (!string.IsNullOrEmpty(guardianPetBeamAttack.AttackWindupSFX))
      guardianPetBeamAttack.attackWindupInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(guardianPetBeamAttack.AttackWindupSFX, guardianPetBeamAttack.transform);
    float time = 0.0f;
    while ((double) time < 0.5)
    {
      time += Time.deltaTime * guardianPetBeamAttack.ParentPetController.HostSpine.timeScale;
      yield return (object) null;
    }
    guardianPetBeamAttack.health.invincible = true;
    guardianPetBeamAttack.transform.DOKill();
    guardianPetBeamAttack.transform.DOMoveZ(-3f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    guardianPetBeamAttack.currentIndicator = ObjectPool.Spawn(guardianPetBeamAttack.loadedIndicator);
    guardianPetBeamAttack.currentIndicator.transform.position = currentTarget.transform.position;
    float aimSpeed = 4f;
    time = 0.0f;
    while ((double) time < 1.5)
    {
      float timeScale = guardianPetBeamAttack.ParentPetController.HostSpine.timeScale;
      time += Time.deltaTime * timeScale;
      if ((UnityEngine.Object) guardianPetBeamAttack.currentIndicator != (UnityEngine.Object) null)
        guardianPetBeamAttack.currentIndicator.transform.position = Vector3.MoveTowards(guardianPetBeamAttack.currentIndicator.transform.position, currentTarget.transform.position, aimSpeed * Time.deltaTime * timeScale);
      yield return (object) null;
    }
    int a = (int) UnityEngine.Random.Range(guardianPetBeamAttack.attacks.x, guardianPetBeamAttack.attacks.y + 1f);
    for (int i = 0; i < a; ++i)
    {
      if (i != 0 && (UnityEngine.Object) guardianPetBeamAttack.currentIndicator != (UnityEngine.Object) null)
        guardianPetBeamAttack.currentIndicator.transform.position = currentTarget.transform.position;
      for (int index = 0; index < 5; ++index)
      {
        Vector3 target = (UnityEngine.Object) guardianPetBeamAttack.currentIndicator != (UnityEngine.Object) null ? guardianPetBeamAttack.currentIndicator.transform.position : currentTarget.transform.position;
        guardianPetBeamAttack.StartCoroutine((IEnumerator) guardianPetBeamAttack.CreateBeam(guardianPetBeamAttack.transform.position + Vector3.down * 0.15f, target, index == 0));
      }
      if (!string.IsNullOrEmpty(guardianPetBeamAttack.AttackBeamShootSFX))
        AudioManager.Instance.PlayOneShot(guardianPetBeamAttack.AttackBeamShootSFX, guardianPetBeamAttack.gameObject);
      time = 0.0f;
      while ((double) time < (double) guardianPetBeamAttack.timeBetween)
      {
        time += Time.deltaTime * guardianPetBeamAttack.ParentPetController.HostSpine.timeScale;
        yield return (object) null;
      }
      if (i == a - 1)
      {
        GameObject currentIndicator = guardianPetBeamAttack.currentIndicator;
        if (currentIndicator != null)
          currentIndicator.Recycle();
        guardianPetBeamAttack.currentIndicator = (GameObject) null;
      }
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * (PlayerRelic.TimeFrozen ? 0.0f : 1f)) < 1.0)
      yield return (object) null;
    guardianPetBeamAttack.spine.AnimationState.SetAnimation(0, "pop", false);
    guardianPetBeamAttack.spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    guardianPetBeamAttack.health.invincible = false;
    guardianPetBeamAttack.state.CURRENT_STATE = StateMachine.State.Idle;
    guardianPetBeamAttack.AttackCoolDown = UnityEngine.Random.Range(guardianPetBeamAttack.AttackCoolDownDuration.x, guardianPetBeamAttack.AttackCoolDownDuration.y);
    guardianPetBeamAttack.EndBeamAttack();
    yield return (object) new WaitForSeconds(1f);
    guardianPetBeamAttack.StartCoroutine((IEnumerator) guardianPetBeamAttack.ActiveRoutine());
  }

  public IEnumerator CreateBeam(Vector3 from, Vector3 target, bool triggerSFX)
  {
    GuardianPet_BeamAttack guardianPetBeamAttack = this;
    float num1 = 0.5f;
    int num2 = (int) Vector3.Distance(from, target) + 2;
    List<Vector3> vector3List = new List<Vector3>();
    vector3List.Add(from);
    for (int index = 1; index < num2 + 1; ++index)
    {
      Vector3 vector3 = Vector3.Lerp(from, target, (float) index / (float) num2) + (Vector3) UnityEngine.Random.insideUnitCircle * 0.2f;
      vector3List.Add(vector3);
    }
    vector3List.Add(target);
    ArrowLightningBeam.CreateBeamFire(vector3List.ToArray(), true, 0.5f, num1, Health.Team.Team2, (Transform) null, true, true);
    yield return (object) new WaitForSeconds(num1);
    BiomeConstants.Instance.EmitHammerEffects(target, 0.3f, 0.5f, 0.2f, playSFX: false);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    guardianPetBeamAttack.collider.gameObject.SetActive(true);
    guardianPetBeamAttack.collider.transform.position = target;
    if (triggerSFX && !string.IsNullOrEmpty(guardianPetBeamAttack.AttackBeamImpactSFX))
      AudioManager.Instance.PlayOneShot(guardianPetBeamAttack.AttackBeamImpactSFX, target);
    for (int index = 0; index < 3; ++index)
      TrapLava.CreateLava(guardianPetBeamAttack.lava, new Vector3(target.x, target.y, 0.0f) + (Vector3) UnityEngine.Random.insideUnitCircle * 0.25f, (UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null ? BiomeGenerator.Instance.CurrentRoom.generateRoom.transform : (Transform) null, guardianPetBeamAttack.health);
    yield return (object) new WaitForSeconds(0.1f);
    guardianPetBeamAttack.collider.gameObject.SetActive(false);
  }

  public void EndBeamAttack()
  {
    this.health.invincible = false;
    this.transform.DOMoveZ(-1f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    if (string.IsNullOrEmpty(this.DescendSFX))
      return;
    AudioManager.Instance.PlayOneShot(this.DescendSFX, this.gameObject);
  }

  [CompilerGenerated]
  public void \u003CLoadAssets\u003Eb__37_0(AsyncOperationHandle<GameObject> obj)
  {
    GuardianPet_BeamAttack.loadedAddressableAssets.Add(obj);
    this.loadedIndicator = obj.Result;
    this.loadedIndicator.CreatePool(5, true);
  }

  [CompilerGenerated]
  public void \u003COnHit\u003Eb__40_0()
  {
    this.AttackCoolDown = UnityEngine.Random.Range(this.AttackCoolDownDuration.x, this.AttackCoolDownDuration.y);
    this.StartCoroutine((IEnumerator) this.ActiveRoutine());
  }
}
